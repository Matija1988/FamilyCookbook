using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyCookbook.Model
{
    public class Recipe
    {
        [Key]
        public int Id { get; set; }

        public string Title { get; set; }

        public string Subtitle { get; set; }

        public string Text { get; set; }

        public bool IsActive { get; set; }

        public DateOnly DateCreated { get; set; }

        public DateOnly DateUpdated { get; set; }

        [ForeignKey("CategoryId")]
        public Category Category { get; set; }

        [ForeignKey("PictureId")]
        public Picture PictureId { get; set; }

        public IList<Member> Members { get; set; }
    }
}
