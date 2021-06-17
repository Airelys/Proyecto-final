using Microsoft.EntityFrameworkCore;
using Movie_Plus.Data;
using Movie_Plus.Repository;
using Movie_Plus.Services.Estadistic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Movie_Plus.Services
{
    public class EstadisticService: IEstadisticService
    {
        public IEnumerable<Tuple<int, int>> MovieIds(DateTime _minDate, DateTime _maxDate, DbSet<Buy_Ticket> AllBuyTickets)
        {
            var _movieIds = (from buy in AllBuyTickets
                             where buy.PayCompleted == true
                             group buy by buy.Horary.MovieId into g
                             orderby g.Sum(x => x.NumberOfEntrance) descending
                             select new Tuple<int,int>(g.Key, g.Sum(x => x.NumberOfEntrance).Value )).ToList();

            if (_minDate != default(DateTime) && _maxDate == default(DateTime))
            {
                _movieIds = (from buy in AllBuyTickets
                             where buy.Date.Date >= _minDate.Date && buy.PayCompleted == true
                             group buy by buy.Horary.MovieId into g
                             orderby g.Sum(x => x.NumberOfEntrance) descending
                             select new Tuple<int, int>(g.Key, g.Sum(x => x.NumberOfEntrance).Value)).ToList();

            }

            if (_maxDate != default(DateTime) && _minDate == default(DateTime))
            {
                _movieIds = (from buy in AllBuyTickets
                             where buy.Date.Date <= _maxDate.Date && buy.PayCompleted == true
                             group buy by buy.Horary.MovieId into g
                             orderby g.Sum(x => x.NumberOfEntrance) descending
                             select new Tuple<int, int>(g.Key, g.Sum(x => x.NumberOfEntrance).Value)).ToList();

            }

            if (_maxDate != default(DateTime) && _minDate != default(DateTime))
            {
                _movieIds = (from buy in AllBuyTickets
                             where buy.Date.Date <= _maxDate.Date
                                    && buy.Date.Date >= _minDate.Date
                                    && buy.PayCompleted == true
                             group buy by buy.Horary.MovieId into g
                             orderby g.Sum(x => x.NumberOfEntrance) descending
                             select new Tuple<int, int>(g.Key, g.Sum(x => x.NumberOfEntrance).Value)).ToList();

            }

            return _movieIds;
        }

        public IEnumerable<EstadisticViewModel> Filters(string _title, string _country, string _kindOfMovie,int _duration, 
                                                        string _actor, int _ranking, List<EstadisticViewModel> _estadistics)
        {
            if (!string.IsNullOrEmpty(_title))
            {
                foreach (var item in _title.Split(new char[] { ' ' },
                         StringSplitOptions.RemoveEmptyEntries))
                {
                    _estadistics = _estadistics.Where(x => x.Title.ToLower().Contains(item.ToLower())).ToList();
                }
            }

            if (!string.IsNullOrEmpty(_country))
            {
                foreach (var item in _country.Split(new char[] { ' ' },
                         StringSplitOptions.RemoveEmptyEntries))
                {
                    _estadistics = _estadistics.Where(x => x.Country.ToLower().Contains(item.ToLower())).ToList();
                }
            }

            if (!string.IsNullOrEmpty(_kindOfMovie))
            {
                foreach (var item in _kindOfMovie.Split(new char[] { ' ' },
                         StringSplitOptions.RemoveEmptyEntries))
                {
                    _estadistics = _estadistics.Where(x => x.KindOfMovie.ToLower().Contains(item.ToLower())).ToList();
                }
            }

            if (!string.IsNullOrEmpty(_actor))
            {
                foreach (var item in _actor.Split(new char[] { ' ' },
                         StringSplitOptions.RemoveEmptyEntries))
                {
                    _estadistics = _estadistics.Where(x => x.Actors.ToLower().Contains(item.ToLower())).ToList();
                }
            }

            if (_duration != 0)
            {
                _estadistics = _estadistics.Where(x => x.Duration <= _duration).ToList();
            }

            if (_ranking != 0)
            {
                _estadistics = _estadistics.Where(x => x.Ranking >= _ranking).ToList();
            }

            return _estadistics;
        }
    }
}
