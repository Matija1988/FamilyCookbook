using FamilyCookbook.Model;
using FamilyCookbook.REST_Models.Recipe;

namespace FamilyCookbook.Mapping.MapperWrappers
{
    public class RecipeMapperWrapper : IMapperExtended<Recipe, RecipeRead, RecipeCreate, RecipeCreateDTO>
    {
        private readonly RecipeMapper _mapper = new();

        public RecipeCreateDTO MapReadToCreateDTO(RecipeCreate dto)
        {
            return _mapper.RecipeCreateToRecipeCreateDTO(dto);
        }

        public RecipeRead MapReadToDto(Recipe entity)
        {
            return _mapper.RecipeToRecipeRead(entity);
        }

        public Recipe MapToEntity(RecipeCreate dto)
        {
            return _mapper.RecipeCreateToRecipe(dto);
        }

        public List<RecipeRead> MapToReadList(List<Recipe> entities)
        {
            return _mapper.RecipeToRecipeReadList(entities);
        }
    }
}
