using ForumNG.Domain.DTOs;
using ForumNG.Domain.Entities;
using Mapster;

namespace ForumNG.Application.Mappings;

public static class MapsterRegister
{
    public static void RegisterMappings()
    {
        TypeAdapterConfig<Post, PostDto>.NewConfig();
    }
}
