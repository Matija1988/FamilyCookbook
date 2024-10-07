﻿namespace FamilyCookbook.Mapping.MapperWrappers
{
    public interface IMapperExtended<T, TDR, TDI, TDI2>
    {
        T MapToEntity(TDI dto);
        TDR MapReadToDto(T entity);

        TDI2 MapReadToCreateDTO(TDI dto);

        List<TDR> MapToReadList(List<T> entities);
    }
}