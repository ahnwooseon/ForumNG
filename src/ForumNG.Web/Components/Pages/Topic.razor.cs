using System.ComponentModel.DataAnnotations;
using ForumNG.Domain.DTOs;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;

namespace ForumNG.Web.Components.Pages;

public partial class Topic : ComponentBase, IAsyncDisposable
{
    [Parameter] public required string CategoryName { get; set; }
    [Parameter] public Guid TopicId { get; set; }

    [Inject] public IHttpClientFactory HttpClientFactory { get; set; } = default!;

    protected TopicDto? _topic;
    protected List<PostDto>? _posts;
    protected FormModel _form = new();
    protected string _formMessage = string.Empty;
    protected bool _processing;
    protected bool _loading = true;
    private Timer? _refreshTimer;
    private HubConnection? hub;
    protected int ConnectedCount;

    private bool _autoRefreshBacking;
    protected bool _autoRefresh
    {
        get => _autoRefreshBacking;
        set
        {
            if (_autoRefreshBacking != value)
            {
                _autoRefreshBacking = value;
                StartOrStopTimer();
            }
        }
    }

    protected override async Task OnInitializedAsync()
    {
        StartOrStopTimer();
        await InitializeSignalRAsync();
        await base.OnInitializedAsync();
    }

    protected override async Task OnParametersSetAsync()
    {
        _loading = true;
        HttpClient client = HttpClientFactory.CreateClient("MyApiClient");
        var getTopicTask = client.GetFromJsonAsync<TopicDto>($"api/topics/{TopicId}");
        var getPostsTask = client.GetFromJsonAsync<List<PostDto>>($"api/topics/{TopicId}/posts");
        await Task.WhenAll(getTopicTask, getPostsTask);
        _topic = getTopicTask.Result;
        _posts = getPostsTask.Result ?? [];
        _loading = false;
    }

    protected async Task CreatePostAsync()
    {
        _processing = true;
        _formMessage = string.Empty;
        try
        {
            HttpClient client = HttpClientFactory.CreateClient("MyApiClient");
            var payload = new { TopicId, _form.Content };
            HttpResponseMessage response = await client.PostAsJsonAsync("api/posts", payload);
            if (response.IsSuccessStatusCode)
            {
                PostDto? created = await response.Content.ReadFromJsonAsync<PostDto>();
                if (created is not null)
                {
                    _posts ??= [];
                    _posts.Add(created);
                }
                else
                {
                    _posts = await client.GetFromJsonAsync<List<PostDto>>($"api/topics/{TopicId}/posts") ?? [];
                }
                _form = new();
                _formMessage = "Message added!";
            }
            else
            {
                string err = await response.Content.ReadAsStringAsync();
                _formMessage = !string.IsNullOrWhiteSpace(err)
                    ? $"Error when adding message: {err}"
                    : "Error when adding message.";
            }
        }
        catch (Exception ex)
        {
            _formMessage = "Network or unexpected error: " + ex.Message;
        }
        _processing = false;
    }

    protected static string DisplayAuthor(Guid authorId)
        => authorId == Guid.Empty ? "Anonymous" : authorId.ToString()[..8] + "…";

    private void StartOrStopTimer()
    {
        _refreshTimer?.Dispose();
        if (_autoRefresh)
        {
            _refreshTimer = new Timer(
                async _ => await AutoRefreshPosts(),
                null,
                TimeSpan.FromSeconds(5),
                TimeSpan.FromSeconds(10));
        }
    }

    private async Task AutoRefreshPosts()
    {
        await InvokeAsync(async () =>
        {
            await LoadPostsAsync();
            StateHasChanged();
        });
    }

    private async Task LoadPostsAsync()
    {
        HttpClient client = HttpClientFactory.CreateClient("MyApiClient");
        _posts = await client.GetFromJsonAsync<List<PostDto>>($"api/topics/{TopicId}/posts") ?? [];
    }

    private async Task InitializeSignalRAsync()
    {
        hub = new HubConnectionBuilder()
            .WithUrl(NavigationManager.ToAbsoluteUri("/topichub"))
            .WithAutomaticReconnect()
            .Build();

        hub.On<int>("UserCountChanged", (count) =>
        {
            ConnectedCount = count;
            InvokeAsync(StateHasChanged);
        });

        await hub.StartAsync();
        await hub.SendAsync("JoinTopic", TopicId.ToString());
    }

    public async ValueTask DisposeAsync()
    {
        _refreshTimer?.Dispose();
        if (hub is not null)
        {
            await hub.SendAsync("LeaveTopic", TopicId.ToString());
            await hub.DisposeAsync();
        }
    }
    
    [Inject] public NavigationManager NavigationManager { get; set; } = default!;

    public class FormModel
    {
        [Required(ErrorMessage = "Message is required")]
        [MinLength(10, ErrorMessage = "A post must contain at least 10 characters")]
        [MaxLength(1000, ErrorMessage = "Maximum 1000 characters")]
        public string Content { get; set; } = string.Empty;
    }
}
