using System.ComponentModel.DataAnnotations;

namespace FreeWheel.Logic.Models
{
    public class MovieQuery
    {
        public MovieQuery() {

            this.Genre = new string[] { };
        }

        public string Title { get; set; }
        public int? Year { get; set; }
        public string[] Genre { get; set; }

        public bool AssertIsValid() {
            return !(
                string.IsNullOrEmpty(Title) &&
                Year == null &&
                Genre.Length == 0);
        }
    }
}
