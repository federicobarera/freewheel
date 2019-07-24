using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FreeWheel.Logic.Models
{
    public class RatingCommand
    {
        [Required]
        public int MovieId { get; set; }

        [Required]
        public int Rating { get; set; }
    }
}
