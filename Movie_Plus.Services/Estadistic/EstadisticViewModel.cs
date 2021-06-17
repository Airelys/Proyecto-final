using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Movie_Plus.Services.Estadistic
{
    public class EstadisticViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Duration { get; set; }
        public string KindOfMovie { get; set; }
        public string Country { get; set; }
        public string Actors { get; set; }
        public double Ranking { get; set; }
        public int TotalOfEntrance { get; set; }
    }
}
