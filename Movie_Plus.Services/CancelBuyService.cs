using Movie_Plus.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Movie_Plus.Services
{
    public class CancelBuyService : ICancelBuyService
    {
        public IEnumerable<Buy_Ticket> Filters(string _code, string _title, string _localMovie, 
                                              DateTime _minDate, DateTime _maxDate, IEnumerable<Buy_Ticket> userBuys)
        {
            if (!string.IsNullOrEmpty(_code))
            {
                foreach (var item in _code.Split(new char[] { ' ' },
                         StringSplitOptions.RemoveEmptyEntries))
                {
                    userBuys = userBuys.Where(x => x.Voucher.ToLower().Contains(item.ToLower())).ToList();
                }
            }

            if (!string.IsNullOrEmpty(_title))
            {
                foreach (var item in _title.Split(new char[] { ' ' },
                         StringSplitOptions.RemoveEmptyEntries))
                {
                    userBuys = userBuys.Where(x => x.Horary.Movie.Title.ToLower().Contains(item.ToLower())).ToList();
                }
            }

            if (!string.IsNullOrEmpty(_localMovie))
            {
                foreach (var item in _localMovie.Split(new char[] { ' ' },
                         StringSplitOptions.RemoveEmptyEntries))
                {
                    userBuys = userBuys.Where(x => x.Horary.Movie_Local.Local_Name.ToLower().Contains(item.ToLower())).ToList();
                }
            }

            if (_minDate != default(DateTime))
            {
                userBuys = userBuys.Where(x => x.Date >= _minDate).ToList();
            }

            if (_maxDate != default(DateTime))
            {
                userBuys = userBuys.Where(x => x.Date <= _maxDate).ToList();
            }

            return userBuys;
        }
    }
}
