using Microsoft.EntityFrameworkCore;
using ProductManagement.Model;

namespace ProductManagement.Data
{
    public partial interface IRepository<T> where T : BaseEntity
    {

        T GetById(object id);

        void Insert(T entity);

        void Insert(IEnumerable<T> entities);

        
        void Update(T entity);

        void Update(IEnumerable<T> entities);
        void Delete(T entity);

        void Delete(IEnumerable<T> entities);


        IRepository<T> Init(DbContext dbContext);
        IQueryable<T> Table { get; }
        IQueryable<T> TableNoTracking { get; }

        DbContext GetDbContext();

    }

}
