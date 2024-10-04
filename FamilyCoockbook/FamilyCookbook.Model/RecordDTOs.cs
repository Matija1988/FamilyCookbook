using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyCookbook.Model
{
    public record RecipeTagArray(
        [Required]
        int recipeId,
        [Required]
        int[] tagId);

    public record RecipeTag(int recipeId, int tagId);

}
