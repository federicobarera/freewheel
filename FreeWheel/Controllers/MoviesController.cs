using FreeWheel.Extensions;
using FreeWheel.Logic.DataAccess;
using FreeWheel.Logic.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace FreeWheel.Controllers
{
    [ApiController]
    [Route("api/movies")]
    public class MoviesController : ControllerBase
    {
        private IMoviesRepository _movieRepository;

        public MoviesController(IMoviesRepository movieRepo) {
            _movieRepository = movieRepo;
        }

        public async Task<ActionResult> Get([FromQuery]MovieQuery query) {

            if (!query.AssertIsValid())
                return BadRequest();

            var results = await _movieRepository.GetMoviesByQuery(query);

            if (results.Count() == 0)
                return NotFound();

            return Ok(results);
        }

        [Route("byRating")]
        [HttpGet]
        public async Task<ActionResult> GetByRating()
        {
            int? uId = null;
            if (User.Identity.IsAuthenticated)
                uId = User.GetUserId();

            var results = await _movieRepository.GetTopMoviesByRating(5, uId);
            return Ok(results);
        }

        [Route("ratings")]
        [HttpPost]
        [Authorize]
        public async Task<ActionResult> GetByRating([FromBody]RatingCommand update)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var movie = await _movieRepository.GetMovieById(update.MovieId);
            if (movie == null)
                return NotFound();

            await _movieRepository.UpsertRating(new RatingUpdateCommand() {
                MovieId = update.MovieId,
                Rating = update.Rating,
                UserId = User.GetUserId()
            });

            return Ok();
        }
    }
}
