using FamilyCookbook.Model;
using FamilyCookbook.REST_Models.Category;

namespace FamilyCookbook.Mapping
{
    public class CategoryMapperWrapper : IMapper<Category, CategoryRead, CategoryCreate>
    {
        private readonly CategoryMapper _mapper = new();

        public CategoryRead MapReadToDto(Category entity)
        {
            return _mapper.CategoryToCategoryRead(entity);
        }

        public Category MapToEntity(CategoryCreate dto)
        {
            return _mapper.CategoryCreateToCategory(dto);
        }

        public List<CategoryRead> MapToReadList(List<Category> entities)
        {
            return _mapper.CategoryToCategoryReadList(entities);
        }
    }
}
