using Core.Domain.Mapper.Interfaces;
using Mapster;

namespace Core.Mapper;

public class MapperService : IMapperService
{
    public TDestination Map<TSource, TDestination>(TSource source)
    {
        return source.Adapt<TDestination>();
    }
    
    public TDestination Map<TSource, TDestination>(TSource source, TypeAdapterConfig config)
    {
        return source.Adapt<TDestination>(config);
    }
}