using System;
using System.Collections.Generic;
using System.Text;

namespace FreeWheel.Logic.Models
{
    public class MovieProjection
    {
        public string Title { get; set; }
        public int YearOfRelease { get; set; }
        public string Genre { get; set; }

        public double AverageRating { get; set; }
    }
}
