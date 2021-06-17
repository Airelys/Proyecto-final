using System;
using System.Collections.Generic;
using System.Text;

namespace Movie_Plus.Services.Pager
{
    public class GenericPager<T> where T : class
    {
        public int ActualPage { get; set; }
        public int RegistersByPage { get; set; }
        public int TotalRegisters { get; set; }
        public int TotalPages { get; set; }
        public IEnumerable<T> Result { get; set; }
    }
}
