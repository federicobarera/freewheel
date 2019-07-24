using FreeWheel.Logic.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FreeWheel.Logic.DataAccess
{
    public interface IMoviesRepository
    {
        Task<Movie> GetMovieById(int iId);
        Task<IEnumerable<MovieProjection>> GetMoviesByQuery(MovieQuery query);
        Task<IEnumerable<MovieProjection>> GetTopMoviesByRating(int iMax, int? iUserId);
        Task UpsertRating(RatingUpdateCommand rUpdate);
    }
}
