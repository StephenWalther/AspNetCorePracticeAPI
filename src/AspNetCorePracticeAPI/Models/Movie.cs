using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCorePracticeAPI.Models
{
    public class Movie
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "You must include a movie title.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "You must include a link to IMDB.")]
        public string IMDBLink { get; set; }

        public int? GenreId { get; set; }
    }
}
