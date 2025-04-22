using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

public class TopicHub : Hub
{
    // nb utilisateurs connectés par topic (group)
    private static readonly ConcurrentDictionary<string, HashSet<string>> GroupUsers = new();

    public async Task JoinTopic(string topicId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, topicId);

        HashSet<string> users = GroupUsers.GetOrAdd(topicId, _ => []);
        lock (users) { users.Add(Context.ConnectionId); }
        await Clients.Group(topicId).SendAsync("UserCountChanged", users.Count);
    }

    public async Task LeaveTopic(string topicId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, topicId);
        if (GroupUsers.TryGetValue(topicId, out var users))
        {
            lock (users) { users.Remove(Context.ConnectionId); }
            await Clients.Group(topicId).SendAsync("UserCountChanged", users.Count);
        }
    }

    public override async Task OnDisconnectedAsync(Exception? ex)
    {
        foreach (var kvp in GroupUsers)
        {
            lock (kvp.Value)
            {
                if (kvp.Value.Remove(Context.ConnectionId))
                {
                    Clients.Group(kvp.Key).SendAsync("UserCountChanged", kvp.Value.Count);
                }
            }
        }
        await base.OnDisconnectedAsync(ex);
    }
}
