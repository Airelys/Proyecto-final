using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Movie_PlusPlus.Paginado
{
    public class GenericPager<T> where T: class
    {
        public int ActualPage { get; set; }
        public int RegistersByPage { get; set; }
        public int TotalRegisters { get; set; }
        public int TotalPages { get; set; }
        public string ActualSearch { get; set; }
        public IEnumerable<T> Result { get; set; }
    }
}
