using FreeWheel.Logic.DataAccess;
using FreeWheel.Logic.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace FreeWheel.DataAccess
{
    public class EFMovieRepository : IMoviesRepository
    {
        private MovieContext _db;

        public EFMovieRepository(MovieContext db) {
            _db = db;
        }

        public async Task<IEnumerable<MovieProjection>> GetMoviesByQuery(MovieQuery query)
        {
            var ratings = GetMovieRatingsQuerable(null);

            return await (from movie in _db.Movies
                    where
                        (string.IsNullOrEmpty(query.Title) || movie.Title.ToLower().Contains(query.Title.ToLower().Trim())) &&
                        (query.Year == null || movie.YearOfRelease == query.Year) &&
                        (query.Genre.Length == 0 || query.Genre.Contains(movie.Genre))
                    join rating in ratings
                    on movie.Id equals rating.Id into pRatings
                    from rating in pRatings.DefaultIfEmpty()
                    select new MovieProjection
                    {
                        Title = movie.Title,
                        AverageRating = RoundToClosest(rating != null ? rating.Avg : 0, .5),
                        Genre = movie.Genre,
                        YearOfRelease = movie.YearOfRelease
                    }
                    ).ToListAsync();
        }

        public async Task<IEnumerable<MovieProjection>> GetTopMoviesByRating(int iMax, int? iUserId)
        {
            var ratings = GetMovieRatingsQuerable(iUserId);

            var movies = await (from rating in ratings
                                join movie in _db.Movies
                                on rating.Id equals movie.Id
                                select new MovieProjection
                                {
                                    Title = movie.Title,
                                    AverageRating = RoundToClosest(rating.Avg, .5),
                                    Genre = movie.Genre,
                                    YearOfRelease = movie.YearOfRelease
                                }
                          )
                          .OrderByDescending(x => x.AverageRating)
                          .ThenBy(x => x.Title)
                          .Take(iMax)
                          .ToListAsync();

            return movies;
        }

        private IQueryable<RatingAverageProjection> GetMovieRatingsQuerable(int? iUserId) {
            var ratings = (from rating in _db.Ratings
                           where iUserId == null || rating.UserId == iUserId
                           group rating by rating.MovieId into gRating

                           select new RatingAverageProjection { Id = gRating.First().MovieId, Avg = gRating.Average(x => x.Rate) }
                         );
            return ratings;
        }

        public Task<Movie> GetMovieById(int iId)
        {
            return _db.Movies.FirstOrDefaultAsync(x => x.Id == iId);
        }

        public async Task UpsertRating(RatingUpdateCommand rUpdate)
        {
            var rating =
                _db
                .Ratings
                .SingleOrDefault(x => x.MovieId == rUpdate.MovieId && x.UserId == rUpdate.UserId);

            if (rating == null) {
                rating = new Rating();
                _db.Ratings.Add(rating);
            }

            rating.MovieId = rUpdate.MovieId;
            rating.UserId = rUpdate.UserId;
            rating.Rate = rUpdate.Rating;

            
            await _db.SaveChangesAsync();
        }

        private double RoundToClosest(double value, double closest) {
            return Math.Round(value / closest) * closest;
        }

        private class RatingAverageProjection {
            public int Id { get; set; }
            public double Avg { get; set; }
        }
    }
}
