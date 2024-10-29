namespace FamilyCookbook.Repository.Common
{
    public interface IPictureRespository : IRepository<Picture, PictureFilter>
    {
        Task<MessageResponse> CreateAsync(Picture entity);

    }
}
