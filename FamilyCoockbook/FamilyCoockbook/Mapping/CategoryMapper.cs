using FamilyCookbook.Model;
using FamilyCookbook.REST_Models.Category;
using Riok.Mapperly.Abstractions;

namespace FamilyCookbook.Mapping
{
    [Mapper]
    public partial class CategoryMapper
    {
        public partial Category CategoryCreateToCategory(CategoryCreate category);

        public partial CategoryRead CategoryToCategoryRead(Category category);

        public partial List<CategoryRead> CategoryToCategoryReadList(List<Category> categories);
    }
}
