using Movie_Plus.Data;
using Movie_Plus.Services.Pager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Movie_Plus.Services
{
    public class PagerService<T> : IPagerService<T> where T: class
    {
        private static int _RegistersByPage = 10;
        private static GenericPager<T> _pager;
        public GenericPager<T> GetPager(List<T> entities, int page)
        {
            int _TotalRegisters = entities.Count;

            entities = entities.Skip((page - 1) * _RegistersByPage)
                                             .Take(_RegistersByPage)
                                             .ToList();

            var _TotalPages = (int)Math.Ceiling((double)_TotalRegisters / _RegistersByPage);

            _pager = new GenericPager<T>()
            {
                RegistersByPage = _RegistersByPage,
                TotalRegisters = _TotalRegisters,
                TotalPages = _TotalPages,
                ActualPage = page,
                Result = entities
            };

            return _pager;
        }
    }
}
