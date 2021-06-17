using Microsoft.EntityFrameworkCore;
using Movie_Plus.Data;
using Movie_Plus.Services.Estadistic;
using System;
using System.Collections.Generic;
using System.Text;

namespace Movie_Plus.Services
{
    public interface IEstadisticService
    {
        IEnumerable<Tuple<int, int>> MovieIds(DateTime _minDate, DateTime _maxDate, DbSet<Buy_Ticket> AllBuyTickets);
        IEnumerable<EstadisticViewModel> Filters(string _title, string _country, string _kindOfMovie, int _duration,
                                                 string _actor, int _ranking, List<EstadisticViewModel> _estadistics);
    }
}
