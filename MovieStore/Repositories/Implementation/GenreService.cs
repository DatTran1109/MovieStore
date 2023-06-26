using MovieStore.Models.Database;
using MovieStore.Repositories.Abstract;

namespace MovieStore.Repositories.Implementation
{
    public class GenreService : IGenreService
    {
        private readonly DatabaseContext databaseContext;

        public GenreService(DatabaseContext databaseContext)
        {
            this.databaseContext = databaseContext;
        }

        public bool Add(Genre model)
        {
            try
            {
                databaseContext.Genre.Add(model);
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
                var data = databaseContext.Genre.Find(id);
                if (data == null)
                {
                    return false;
                }
                databaseContext.Genre.Remove(data);
                databaseContext.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public IQueryable<Genre> GetAll()
        {
            var data = databaseContext.Genre.AsQueryable();
            return data;
        }

        public Genre? GetById(int id)
        {
            return databaseContext.Genre.Find(id);
        }

        public bool Update(Genre model)
        {
            try
            {
                databaseContext.Genre.Update(model);
                databaseContext.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
