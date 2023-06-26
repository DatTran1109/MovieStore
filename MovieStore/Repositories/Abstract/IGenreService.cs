using MovieStore.Models.Database;

namespace MovieStore.Repositories.Abstract
{
    public interface IGenreService
    {
        bool Add(Genre model);
        bool Update(Genre model);
        bool Delete(int id);
        Genre? GetById(int id);
        IQueryable<Genre> GetAll();
    }
}
