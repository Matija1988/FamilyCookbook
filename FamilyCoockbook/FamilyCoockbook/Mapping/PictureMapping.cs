namespace FamilyCookbook.Mapping
{
    [Mapper]
    public partial class PictureMapping
    {
        public partial Picture PictureCreateToPicture(PictureCreate picture);

        public partial PictureRead PictureToPictureRead(Picture picture);

        public partial List<PictureRead> PistureToPictureReadAll(List<Picture> pictures);
    }
}
