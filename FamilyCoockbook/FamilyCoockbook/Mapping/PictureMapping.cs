using FamilyCookbook.Model;
using FamilyCookbook.REST_Models.Picture;
using Riok.Mapperly.Abstractions;

namespace FamilyCookbook.Mapping
{
    [Mapper]
    public partial class PictureMapping
    {
        public partial Picture PictureCreateToPicture(PictureCreate picture);
    }
}
