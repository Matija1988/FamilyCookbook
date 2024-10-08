using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyCookbook.Model
{
    public class RecipeCreateDTO
    {
        [Key]
        public int Id { get; set; }

        public string Title { get; set; }

        public string Subtitle { get; set; }

        public string Text { get; set; }

        public bool IsActive { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateUpdated { get; set; }

        [ForeignKey("CategoryId")]
        public int CategoryId { get; set; }

        [ForeignKey("PictureId")]
        public Picture? Picture { get; set; }
        public List<int> MemberIds { get; set; }

        public int[]? TagIds { get; set; }

    }
}
