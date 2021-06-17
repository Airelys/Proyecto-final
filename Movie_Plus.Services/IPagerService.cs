using Movie_Plus.Data;
using Movie_Plus.Services.Pager;
using System;
using System.Collections.Generic;
using System.Text;

namespace Movie_Plus.Services
{
    public interface IPagerService<T> where T : class
    {
        GenericPager<T> GetPager(List<T> entities, int page);
    }
}
