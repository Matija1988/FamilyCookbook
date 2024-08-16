using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FamilyCookbook.Common
{
    public class RecipeFilter
    {
        public string? SearchByTitle { get; set; }

        public string? SearchBySubtitle { get; set; }

        public int? SearchByActivityStatus {  get; set; }

        public DateTime? SearchByDateCreated { get; set; }

        public int? SearchByCategory { get; set; }

        public string? SearchByAuthorName {  get; set; }

        public string? SearchByAuthorSurname { get; set; }
    }
}
