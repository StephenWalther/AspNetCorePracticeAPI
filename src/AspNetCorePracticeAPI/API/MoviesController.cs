using AspNetCorePracticeAPI.Data;
using AspNetCorePracticeAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace AspNetCorePracticeAPI.API
{
    [Route("api/Movies")]
    public class MoviesController : Controller
    {
        private ApplicationDbContext _context;

        public MoviesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Movies
        [HttpGet]
        public IEnumerable<Movie> GetMovies()
        {
            return _context.Movies.OrderByDescending(m => m.Id).Take(25).ToList();
        }

        // GET: api/Movies/5
        [HttpGet("{id}", Name = "GetMovie")]
        public IActionResult GetMovie([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Movie movie = _context.Movies.Single(m => m.Id == id);

            if (movie == null)
            {
                return NotFound();
            }

            return Ok(movie);
        }

     

        // POST: api/Movies
        [HttpPost]
        public IActionResult PostMovie([FromBody] Movie movie)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // add or update depending on id
            if (movie.Id == 0)
            {
                _context.Movies.Add(movie);
                _context.SaveChanges();
                return CreatedAtRoute("GetMovie", new { id = movie.Id }, movie);

            }
            else {
                var original = _context.Movies.Single(m => m.Id == movie.Id);
                original.Title = movie.Title;
                original.IMDBLink = movie.IMDBLink;
                _context.SaveChanges();
                return Ok(movie);
            }
 
        }

        // DELETE: api/Movies/5
        [HttpDelete("{id}")]
        public IActionResult DeleteMovie(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Movie movie = _context.Movies.Single(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            _context.Movies.Remove(movie);
            _context.SaveChanges();

            return Ok(movie);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            base.Dispose(disposing);
        }


        [HttpGet("search/{search}")]
        public IEnumerable<Movie> Search(string search)
        {
            return _context.Movies.Where(m => m.Title.StartsWith(search)).ToList();
        }

    }
}