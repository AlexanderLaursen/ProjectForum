using Mapster;
using WebApi.Dto;
using WebApi.Models;

public static class MapsterConfig
{
    public static void RegisterMappings()
    {
        TypeAdapterConfig<Post, PostHistory>.NewConfig()
            .Ignore(target => target.Id);
    }
}
