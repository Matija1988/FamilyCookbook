using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FamilyCookbook.Common.Filters
{
    public class RecipeFilter
    {

        [StringLength(30, ErrorMessage = "Maximum allowed number of characters: 30")]
        public string? SearchByTitle { get; set; }

        [StringLength(30, ErrorMessage = "Maximum allowed number of characters: 30")]
        public string? SearchBySubtitle { get; set; }


        [Range(0, 1, ErrorMessage = "Allowed inputs 0/false, 1/true")]
        public int? SearchByActivityStatus { get; set; }

        public DateTime? SearchByDateCreated { get; set; }

        public int? SearchByCategory { get; set; }


        [StringLength(30, ErrorMessage = "Maximum allowed number of characters: 30")]
        public string? SearchByAuthorName { get; set; }

        [StringLength(30, ErrorMessage = "Maximum allowed number of characters: 30")]
        public string? SearchByAuthorSurname { get; set; }
    }
}
