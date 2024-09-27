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
        public int MemberId { get; set; }
        public Member Member { get; set; }

        [ForeignKey("RecipeId")]
        public int RecipeId { get; set; }

        public Recipe Recipe { get; set; }

        public string Text { get; set; }

        public int? Rating { get; set; }

        public bool IsActive { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateUpdated { get; set; }
    }
}
