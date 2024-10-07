using FamilyCookbook.Model;
using FamilyCookbook.REST_Models.Tags;
using Riok.Mapperly.Abstractions;

namespace FamilyCookbook.Mapping
{
    [Mapper]
    public partial class TagMapper
    {
        public partial List<Tag> ListTagToListTagCreate(List<TagCreate> entity);

        public partial Tag TagCreateToTag(TagCreate tagCreate);
    }
}
