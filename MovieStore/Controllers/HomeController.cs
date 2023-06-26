using Microsoft.AspNetCore.Mvc;
using MovieStore.Repositories.Abstract;

namespace MovieStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMovieService _movieService;

        public HomeController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        public IActionResult Index(string term = "", string orderBy = "", int currentPage = 1)
        {
            var movies = _movieService.GetAll(term, orderBy, true, currentPage);
            return View(movies);
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult MovieDetail(int id)
        {
            var movie = _movieService.GetMovieDetail(id);
            return View(movie);
        }
    }
}
