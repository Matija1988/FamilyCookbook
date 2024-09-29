using FamilyCookbook.Model;
using FamilyCookbook.REST_Models.Picture;

namespace FamilyCookbook.Mapping
{
    public class PictureMapperWrapper : IMapper<Picture, PictureRead, PictureCreate>
    {
        private readonly PictureMapping _mapper = new();
        public PictureRead MapReadToDto(Picture entity)
        {
            return _mapper.PictureToPictureRead(entity);
        }

        public Picture MapToEntity(PictureCreate dto)
        {
            return _mapper.PictureCreateToPicture(dto);
        }

        public List<PictureRead> MapToReadList(List<Picture> entities)
        {
            return _mapper.PistureToPictureReadAll(entities);
        }
    }
}
