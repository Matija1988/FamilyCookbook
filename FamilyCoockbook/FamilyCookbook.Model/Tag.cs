using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyCookbook.Model
{
    public class Tag
    {
        [Key]
        public int Id { get; set; }

        [StringLength(20, ErrorMessage ="Max tag lenght is 20 characters!")]
        public string Text { get; set; }
    }
}
