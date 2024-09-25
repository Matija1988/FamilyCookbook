using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyCookbook.Model
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("MemberId")]
        public int Member { get; set; }

        [ForeignKey("RecipeId")]
        public int Recipe { get; set; }

        public string Text { get; set; }

        public int Rating { get; set; }
    }
}
