namespace FamilyCookbook.Mapping
{
    [Mapper]
    public partial class RecipeMapper
    {
        public partial List<RecipeRead> RecipeToRecipeReadList(List<Recipe> recipes);

        public partial RecipeRead RecipeToRecipeRead(Recipe recipe);

         public partial Recipe RecipeCreateToRecipe(RecipeCreate newRecipe);

         public partial RecipeCreateDTO RecipeCreateToRecipeCreateDTO(RecipeCreate newRecipeCreate);

        public partial List<RecipeRead> MapListToListRead(List<Recipe> recipes);

    }

   
}


