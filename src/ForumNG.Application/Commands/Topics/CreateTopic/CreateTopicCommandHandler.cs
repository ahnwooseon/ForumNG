using FluentResults;
using ForumNG.Domain.DTOs;
using ForumNG.Domain.Entities;
using ForumNG.Domain.Interfaces;
using ForumNG.Infrastructure.Data;
using Mapster;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace ForumNG.Application.Commands.Topics.CreateTopic;

public class CreateTopicCommandHandler(
    ApplicationDbContext context,
    ITopicRepository topicRepository,
    IPostRepository postRepository
) : IRequestHandler<CreateTopicCommand, Result<TopicDto>>
{
    public async ValueTask<Result<TopicDto>> Handle(
        CreateTopicCommand command,
        CancellationToken ct
    )
    {
        if (string.IsNullOrWhiteSpace(command.Content) || command.Content.Length < 10)
            return Result.Fail<TopicDto>("Message must be at least 10 characters long.");

        await using var transaction = await context.Database.BeginTransactionAsync(ct);
        try
        {
            Topic topic = await CreateTopicAsync(command, ct);
            await CreateFirstPostAsync(command, topic.Id, ct);

            await transaction.CommitAsync(ct);

            TopicDto dto = topic.Adapt<TopicDto>();
            return Result.Ok(dto);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(ct);
            return Result.Fail<TopicDto>("Topic was not created: " + ex.Message);
        }
    }

    private async Task<Topic> CreateTopicAsync(CreateTopicCommand command, CancellationToken ct)
    {
        Category? category = await context.Categories
            .Where(c => c.Name == command.CategoryName)
            .FirstOrDefaultAsync(ct);

        if (category == null)
            throw new InvalidOperationException("Category not found");

        Topic topic = new()
        {
            Id = Guid.NewGuid(),
            CategoryId = category.Id,
            AuthorId = command.AuthorId,
            Title = command.Title,
            CreatedAt = DateTime.UtcNow,
        };
        await topicRepository.AddAsync(topic, ct);
        return topic;
    }

    private async Task CreateFirstPostAsync(
        CreateTopicCommand command,
        Guid topicId,
        CancellationToken ct
    )
    {
        Post post = new()
        {
            Id = Guid.NewGuid(),
            TopicId = topicId,
            AuthorId = command.AuthorId,
            Content = command.Content,
            CreatedAt = DateTime.UtcNow,
        };
        await postRepository.AddAsync(post, ct);
    }
}
