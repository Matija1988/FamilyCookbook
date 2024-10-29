namespace FamilyCookbook.Service.Common
{
    public interface IPictureService : IService<Picture, PictureFilter>
    {
        Task<MessageResponse> CreateAsync(Picture entity);

        Task<MessageResponse> UpdateAsync(int id, Picture entity);
    }
}
