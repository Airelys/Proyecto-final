using Microsoft.EntityFrameworkCore;
using Movie_Plus.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Movie_Plus.Services
{
    public interface IHoraryService
    {
        DbSet<Horary> GetAllHoraries();
        Horary GetHorary(int id);
        void InsertHorary(Horary horary);
        void UpdateHorary(Horary horary);
        bool ExistsHorary(int id);
        void DeleteHorary(Horary horary);
        IEnumerable<Horary> Filters(string _title, string _localName, DateTime _minDate, DateTime _maxDate,
                                   int _price, int _priceInPoints, List<Horary> horaries);
        bool DuplicateHorary(Horary horary);
    }
}
