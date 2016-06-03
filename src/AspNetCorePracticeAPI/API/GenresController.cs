


using AspNetCorePracticeAPI.Data;
using AspNetCorePracticeAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace AspNetCorePracticeAPI.API
{
    [Route("api/[controller]")]
    public class GenresController : Controller
    {
        private ApplicationDbContext _db;

        public GenresController(ApplicationDbContext db)
        {
            this._db = db;
        }

        // GET: api/values
        [HttpGet]
        public IEnumerable<Genre> Get()
        {
            return _db.Genres.OrderBy(g => g.Name).ToList();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var genre = _db.Genres.Where(g => g.Id == id).Include(g => g.Movies).FirstOrDefault();
            if (genre == null)
            {
                return NotFound();
            }
            return Ok(genre);
        }

    
    }
}
