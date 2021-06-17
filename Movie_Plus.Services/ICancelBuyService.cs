using Movie_Plus.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Movie_Plus.Services
{
    public interface ICancelBuyService
    {
        IEnumerable<Buy_Ticket> Filters(string _code, string _title, string _localMovie, 
                                       DateTime _minDate, DateTime _maxDate, IEnumerable<Buy_Ticket> userBuys);
    }
}
