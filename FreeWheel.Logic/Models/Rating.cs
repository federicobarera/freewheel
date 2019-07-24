using System.ComponentModel.DataAnnotations;

namespace FreeWheel.Logic.Models
{
    public class Rating
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }

        public int MovieId { get; set; }

        public int Rate { get; set; }
    }
}
