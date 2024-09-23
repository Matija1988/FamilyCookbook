using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyCookbook.Common.Validations
{
    public static class Nullchks
    {
        public static bool CheckDictionary<T>(Dictionary<int, T> dictionary)
        {
            if (dictionary == null) 
            { 
                return true; 
            }

            return false;
        }
    }
}
