using System;
using System.Collections.Generic;
using System.Text;

namespace FreeWheel.Logic.Models
{
    public class RatingUpdateCommand
    {
        public int MovieId { get; set; }
        public int Rating { get; set; }
        public int UserId { get; set; }
    }
}
