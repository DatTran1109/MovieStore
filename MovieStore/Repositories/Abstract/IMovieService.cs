using MovieStore.Models.Database;
using MovieStore.Models.Identity;

namespace MovieStore.Repositories.Abstract
{
    public interface IMovieService
    {
        bool Add(Movie model);
        bool Update(Movie model);
        bool Delete(int id);
        Movie? GetById(int id);
        MovieListViewModel GetMovieDetail(int id);
        MovieListViewModel GetAll(string term = "", string orderBy = "", bool paging = false, int currentPage = 0);
        public List<int> GetGenreById(int id);
    }
}
