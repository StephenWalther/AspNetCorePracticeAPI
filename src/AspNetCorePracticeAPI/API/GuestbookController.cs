using AspNetCorePracticeAPI.Data;
using AspNetCorePracticeAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace AspNetCorePracticeAPI.API
{
    [Route("api/Guestbook")]
    public class GuestbookController : Controller
    {
        private ApplicationDbContext _context;

        public GuestbookController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/guestbook
        [HttpGet]
        public IEnumerable<Entry> GetEntries()
        {
            return _context.Entries.OrderByDescending(m => m.Id).Take(25).ToList();
        }

        // GET: api/guestbook/5
        [HttpGet("{id}", Name = "GetEntry")]
        public IActionResult Get([FromRoute] int id)
        {
          

            var entry = _context.Entries.Single(m => m.Id == id);

            if (entry == null)
            {
                return NotFound();
            }

            return Ok(entry);
        }



        // POST: api/guestbook
        [HttpPost]
        public IActionResult PostEntry([FromBody] Entry entry)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // add or update depending on id
            if (entry.Id == 0)
            {
                _context.Entries.Add(entry);
                _context.SaveChanges();
                return CreatedAtRoute("GetEntry", new { id = entry.Id }, entry);

            }
            else {
                var original = _context.Entries.Single(m => m.Id == entry.Id);
                original.Author = entry.Author;
                original.Message = entry.Message;
                _context.SaveChanges();
                return Ok(entry);
            }

        }

     

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            base.Dispose(disposing);
        }


     

    }
}