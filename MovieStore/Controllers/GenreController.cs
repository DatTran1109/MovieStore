using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieStore.Models.Database;
using MovieStore.Repositories.Abstract;

namespace MovieStore.Controllers
{
    [Authorize(Roles = "Admin")]
    public class GenreController : Controller
    {
        private readonly IGenreService _genreService;

        public GenreController(IGenreService genreService)
        {
            _genreService = genreService;
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(Genre model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = _genreService.Add(model);

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
            var data = _genreService.GetById(id);
            return View(data);
        }

        [HttpPost]
        public IActionResult Edit(Genre model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var result = _genreService.Update(model);

            if (!result)
            {
                TempData["msg"] = "Could not update";
                return View();
            }

            return RedirectToAction("GenreToList");
        }

        public IActionResult GenreToList()
        {
            var data = _genreService.GetAll();
            return View(data);
        }

        public IActionResult Delete(int id)
        {
            var result = _genreService.Delete(id);

            if (!result)
            {
                TempData["msg"] = "Could not delete";
                return View();
            }

            return RedirectToAction("GenreToList");
        }
    }
}
