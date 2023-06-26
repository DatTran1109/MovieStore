using Humanizer.Localisation;
using Microsoft.AspNetCore.Mvc.Rendering;
using MovieStore.Models.Database;
using MovieStore.Models.Identity;
using MovieStore.Repositories.Abstract;

namespace MovieStore.Repositories.Implementation
{
    public class MovieService : IMovieService
    {
        private readonly DatabaseContext databaseContext;
        private readonly IGenreService _genreService;

        public MovieService(DatabaseContext databaseContext, IGenreService genreService)
        {
            this.databaseContext = databaseContext;
            _genreService = genreService;
        }

        public bool Add(Movie model)
        {
            try
            {
                databaseContext.Movie.Add(model);
                databaseContext.SaveChanges();

                foreach (int genreId in model.GenreIdList)
                {
                    var movieGenre = new MovieGenre
                    {
                        MovieId = model.Id,
                        GenreId = genreId
                    };
                    databaseContext.MovieGenre.Add(movieGenre);
                }

                databaseContext.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Delete(int id)
        {
            try
            {
                var data = databaseContext.Movie.Find(id);

                if (data == null)
                {
                    return false;
                }

                var movieGenres = databaseContext.MovieGenre.Where(a => a.MovieId == data.Id);

                foreach (var movieGenre in movieGenres)
                {
                    databaseContext.MovieGenre.Remove(movieGenre);
                }

                databaseContext.Movie.Remove(data);
                databaseContext.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public MovieListViewModel GetAll(string term = "", string orderBy = "", bool paging = false, int currentPage = 0)
        {
            try
            {
                var list = databaseContext.Movie.ToList();
                var data = new MovieListViewModel();

                if (!string.IsNullOrEmpty(term) )
                {
                    term = term.ToLower();
                    list = list.Where(a => a.Title.ToLower().Contains(term)).ToList();
                    data.Term = term;
                }

                switch (orderBy)
                {
                    case "title_desc":
                        data.OrderBy = orderBy;
                        list = list.OrderByDescending(a => a.Title).ToList();
                        break;
                    case "title":
                        data.OrderBy = orderBy;
                        list = list.OrderBy(a => a.Title).ToList();
                        break;
                    case "release_desc":
                        data.OrderBy = orderBy;
                        list = list.OrderByDescending(a => a.ReleaseYear).ToList();
                        break;
                    case "release":
                        data.OrderBy = orderBy;
                        list = list.OrderBy(a => a.ReleaseYear).ToList();
                        break;
                    default:
                        data.OrderBy = "none";
                        break;
                }

                if (paging)
                {
                    int limit = 5;
                    int count = list.Count;
                    int totalPages = (int)Math.Ceiling(count / (double)limit);
                    list = list.Skip((currentPage - 1) * limit).Take(limit).ToList();
                    data.TotalPages = totalPages;
                    data.CurrentPage = currentPage;
                }

                foreach (var movie in list)
                {
                    var genres = (from genre in databaseContext.Genre
                                  join mg in databaseContext.MovieGenre
                                  on genre.Id equals mg.GenreId
                                  where mg.MovieId == movie.Id
                                  select genre.GenreName
                                  ).ToList();
                    var genreNames = string.Join(", ", genres);
                    movie.GenreNames = genreNames;
                }

                data.MovieList = list.AsQueryable();
                return data;            
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Movie? GetById(int id)
        {
            return databaseContext.Movie.Find(id);
        }

        public MovieListViewModel GetMovieDetail(int id)
        {
            var data = new MovieListViewModel();
            var list = databaseContext.Movie.Where(a => a.Id == id).ToList();

            foreach (var movie in list)
            {
                var genres = (from genre in databaseContext.Genre
                              join mg in databaseContext.MovieGenre
                              on genre.Id equals mg.GenreId
                              where mg.MovieId == movie.Id
                              select genre.GenreName
                              ).ToList();
                var genreNames = string.Join(", ", genres);
                movie.GenreNames = genreNames;
            }

            data.MovieList = list.AsQueryable();
            return data;
        }

        public bool Update(Movie model)
        {
            try
            {
                var genresToRemove = databaseContext.MovieGenre.Where(a => a.MovieId == model.Id
                && !model.GenreIdList.Contains(a.GenreId));

                foreach (var genre in genresToRemove)
                {
                    databaseContext.MovieGenre.Remove(genre);
                }
                databaseContext.SaveChanges();

                foreach (int genreId in model.GenreIdList)
                {
                    var movieGenre = databaseContext.MovieGenre.FirstOrDefault(a => a.MovieId == model.Id
                    && a.GenreId == genreId);

                    if (movieGenre == null)
                    {
                        movieGenre = new MovieGenre
                        {
                            MovieId = model.Id,
                            GenreId = genreId,
                        };
                        databaseContext.MovieGenre.Add(movieGenre);
                    }
                }
                databaseContext.SaveChanges();

                databaseContext.Movie.Update(model);
                databaseContext.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<int> GetGenreById(int id)
        {
            var genreIds = databaseContext.MovieGenre.Where(a => a.MovieId == id).Select(a => a.GenreId).ToList();
            return genreIds;
        }
    }
}
