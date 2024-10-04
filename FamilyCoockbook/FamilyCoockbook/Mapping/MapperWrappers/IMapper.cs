namespace FamilyCookbook.Mapping.MapperWrappers
{
    public interface IMapper<T, TDR, TDI>
    {
        T MapToEntity(TDI dto);
        TDR MapReadToDto(T entity);

        List<TDR> MapToReadList(List<T> entities);
    }
}
