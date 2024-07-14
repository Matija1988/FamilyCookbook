using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyCookbook.Model
{
    public class MemberRecipe
    {
        [ForeignKey("MemberId")]
        public int MemberId { get; set; }

        [ForeignKey("RecipeId")]
        public int RecipeId { get; set; }
    }
}
