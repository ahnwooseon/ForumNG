using ForumNG.Domain.DTOs;
using ForumNG.Domain.Entities;
using Mapster;

namespace ForumNG.Application.Mappings;

public static class MapsterRegister
{
    public static void RegisterMappings()
    {
        TypeAdapterConfig<Post, PostDto>.NewConfig();

        TypeAdapterConfig<Topic, TopicDto>.NewConfig()
            .Map(dest => dest.Count, src => src.Posts.Count)
            .Map(dest => dest.LastUpdated, src => src.Posts.Any()
                ? src.Posts.Max(p => p.CreatedAt)
                : DateTime.MinValue
            );
    }
}
