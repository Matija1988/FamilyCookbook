using FamilyCookbook.Model;
using FamilyCookbook.REST_Models.Category;
using Riok.Mapperly.Abstractions;

namespace FamilyCookbook.Mapping
{
    [Mapper]
    public partial class CategoryMapper
    {
        public partial Category CategoryCreateToCategory(CategoryCreate category);
    }
}
