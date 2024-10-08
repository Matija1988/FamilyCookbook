using FamilyCookbook.Model;
using FamilyCookbook.REST_Models.Member;
using FamilyCookbook.REST_Models.Recipe;
using Riok.Mapperly.Abstractions;
using System.Collections.Immutable;

namespace FamilyCookbook.Mapping
{
    [Mapper]
    public partial class RecipeMapper
    {
        public partial List<RecipeRead> RecipeToRecipeReadList(List<Recipe> recipes);

        public partial RecipeRead RecipeToRecipeRead(Recipe recipe);

         public partial Recipe RecipeCreateToRecipe(RecipeCreate newRecipe);

         public partial RecipeCreateDTO RecipeCreateToRecipeCreateDTO(RecipeCreate newRecipeCreate);

        public partial ImmutableList<RecipeRead> MapListToListRead(ImmutableList<Recipe> recipes);

    }

   
}


