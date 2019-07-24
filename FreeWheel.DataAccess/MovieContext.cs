using Microsoft.EntityFrameworkCore;
using FreeWheel.Logic.Models;

namespace FreeWheel.DataAccess
{
    public class MovieContext : DbContext
    {
        public MovieContext(DbContextOptions<MovieContext> options)
            : base(options)
        {

        }

        public DbSet<Movie> Movies { get; set; }

        public DbSet<Rating> Ratings { get; set; }
    }
}
