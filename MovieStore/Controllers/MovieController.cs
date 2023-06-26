using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MovieStore.Models.Database;
using MovieStore.Repositories.Abstract;

namespace MovieStore.Controllers
{
    [Authorize(Roles = "Admin")]
    public class MovieController : Controller
    {
        private readonly IMovieService _movieService;
        private readonly IFileService _fileService;
        private readonly IGenreService _genreService;

        public MovieController(IMovieService movieService, IFileService fileService, IGenreService genreService)
        {
            _movieService = movieService;
            _fileService = fileService;
            _genreService = genreService;
        }

        public IActionResult Add()
        {
            var model = new Movie();
            model.GenreList = _genreService.GetAll().Select(a => new SelectListItem
            {
                Text = a.GenreName,
                Value = a.Id.ToString(),
            });
            return View(model);
        }

        [HttpPost]
        public IActionResult Add(Movie model)
        {
            model.GenreList = _genreService.GetAll().Select(a => new SelectListItem
            {
                Text = a.GenreName,
                Value = a.Id.ToString(),
            });

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (model.ImageFile != null)
            {
                var fileResult = _fileService.SaveImage(model.ImageFile);

                if (fileResult.Item1 == 0)
                {
                    TempData["msg"] = "File could not save";
                    return View(model);
                }

                model.ImageName = fileResult.Item2;
            }
            
            var result = _movieService.Add(model);

            if (!result)
            {
                TempData["msg"] = "Could not add";
                return View();
            }

            TempData["msg"] = "Added successfully";
            return RedirectToAction("Add");
        }

        public IActionResult Edit(int id)
        {
            var model = _movieService.GetById(id);
            var selectedGenres = _movieService.GetGenreById(model.Id);
            MultiSelectList multiGenreList = new MultiSelectList(_genreService.GetAll(), "Id", "GenreName", selectedGenres);
            model.MultiGenreList = multiGenreList;
            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(Movie model)
        {
            var selectedGenres = _movieService.GetGenreById(model.Id);
            MultiSelectList multiGenreList = new MultiSelectList(_genreService.GetAll(), "Id", "GenreName", selectedGenres);
            model.MultiGenreList = multiGenreList;

            if (!ModelState.IsValid)
            {
                return View();
            }

            if (model.ImageFile != null)
            {
                var fileResult = _fileService.SaveImage(model.ImageFile);

                if (fileResult.Item1 == 0)
                {
                    TempData["msg"] = "File could not save";
                    return View(model);
                }

                model.ImageName = fileResult.Item2;
            }

            var result = _movieService.Update(model);

            if (!result)
            {
                TempData["msg"] = "Could not update";
                return View();
            }

            return RedirectToAction("MovieToList");
        }

        public IActionResult MovieToList()
        {
            var data = _movieService.GetAll();
            return View(data);
        }

        public IActionResult Delete(int id)
        {
            var result = _movieService.Delete(id);

            if (!result)
            {
                TempData["msg"] = "Could not delete";
                return View();
            }

            return RedirectToAction("MovieToList");
        }
    }
}
