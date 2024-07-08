using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyCookbook.Model
{
    public class RepositoryResponse<T> where T : class
    {
        public T Items { get; set; }

        public bool Success { get; set; }

        public string Message { get; set; }

        public int TotalCount { get; set; }
    }
}
