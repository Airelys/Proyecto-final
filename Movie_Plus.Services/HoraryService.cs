using Microsoft.EntityFrameworkCore;
using Movie_Plus.Data;
using Movie_Plus.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Movie_Plus.Services
{
    public class HoraryService : IHoraryService
    {
        private IRepository<Horary> _HoraryRepository;

        public HoraryService(IRepository<Horary> HoraryRepository)
        {
            _HoraryRepository = HoraryRepository;
        }

        public void DeleteHorary(Horary horary)
        {
            _HoraryRepository.Remove(horary);
        }

        public bool DuplicateHorary(Horary horary)
        {
            return _HoraryRepository.GetAll().AsNoTracking().ToList()
                                    .Any(x => x.MovieId == horary.MovieId &&
                                              x.Movie_LocalId == horary.Movie_LocalId &&
                                              x.Date.Date == horary.Date.Date &&
                                              x.Time == x.Time && 
                                              x.Id != horary.Id);
        }

        public bool ExistsHorary(int id)
        {
            return _HoraryRepository.Exists(id);
        }

        public IEnumerable<Horary> Filters(string _title, string _localName, DateTime _minDate, DateTime _maxDate, 
                                            int _price, int _priceInPoints, List<Horary> horaries)
        {
            if (!string.IsNullOrEmpty(_title))
            {
                foreach (var item in _title.Split(new char[] { ' ' },
                         StringSplitOptions.RemoveEmptyEntries))
                {
                    horaries = horaries.Where(x => x.Movie.Title.ToLower().Contains(item.ToLower())).ToList();
                }
            }

            if (!string.IsNullOrEmpty(_localName))
            {
                foreach (var item in _localName.Split(new char[] { ' ' },
                         StringSplitOptions.RemoveEmptyEntries))
                {
                    horaries = horaries.Where(x => x.Movie_Local.Local_Name.ToLower().Contains(item.ToLower())).ToList();
                }
            }

            if (_price != 0)
            {
                horaries = horaries.Where(x => x.Price <= _price).ToList();
            }

            if (_priceInPoints != 0)
            {
                horaries = horaries.Where(x => x.PriceInPoints <= _priceInPoints).ToList();
            }

            if(_minDate != default(DateTime))
            {
                horaries = horaries.Where(x => x.Date >= _minDate).ToList();
            }

            if(_maxDate != default(DateTime))
            {
                horaries = horaries.Where(x => x.Date <= _maxDate).ToList();
            }

            return horaries;
        }

        public DbSet<Horary> GetAllHoraries()
        {
            return _HoraryRepository.GetAll();
        }

        public Horary GetHorary(int id)
        {
            return _HoraryRepository.Get(id);
        }

        public void InsertHorary(Horary horary)
        {
            _HoraryRepository.Insert(horary);
        }

        public void UpdateHorary(Horary horary)
        {
            _HoraryRepository.Update(horary);
        }
    }
}
