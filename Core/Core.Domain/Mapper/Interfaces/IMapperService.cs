using Mapster;

namespace Core.Domain.Mapper.Interfaces;

public interface IMapperService
{
    public TDestination Map<TSource, TDestination>(TSource source);
    public TDestination Map<TSource, TDestination>(TSource source, TypeAdapterConfig config);
}