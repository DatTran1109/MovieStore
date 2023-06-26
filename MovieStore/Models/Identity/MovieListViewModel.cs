using Microsoft.AspNetCore.Mvc.Rendering;
using MovieStore.Models.Database;

namespace MovieStore.Models.Identity
{
    public class MovieListViewModel
    {
        public IQueryable<Movie>? MovieList { get; set; }

        public int CurrentPage { get; set; }

        public int TotalPages { get; set; }

        public string? Term { get; set; }

        public string? OrderBy { get; set; }
    }
}
