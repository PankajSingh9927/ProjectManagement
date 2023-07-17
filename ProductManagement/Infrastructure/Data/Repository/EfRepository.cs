using Domain;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Repository
{
    public partial class EfRepository<T> : IRepository<T> where T : BaseEntity
    {
        #region Fields

        private DbContext _context;
        private DbSet<T> _entities;

        #endregion

        public EfRepository(MyDbContext context)
        {
            _context = context;
        }


        protected string GetFullErrorText(ValidationException exc)
        {
            var msg = string.Empty;
            msg += string.Format("Property: {0} Error: {1}", string.Join(", ", exc.ValidationResult.MemberNames), exc.ValidationResult.ErrorMessage) + Environment.NewLine;
            return msg;
        }



        #region Methods
        public virtual T GetById(object id)
        {
            return Entities.Find(id);
        }


        public virtual void Insert(T entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException("entity");

                Entities.Add(entity);

                _context.SaveChanges();
            }
            catch (ValidationException dbEx)
            {
                throw new Exception(GetFullErrorText(dbEx), dbEx);
            }
        }


        public virtual void Insert(IEnumerable<T> entities)
        {
            try
            {
                if (entities == null)
                    throw new ArgumentNullException("entities");

                foreach (var entity in entities)
                    Entities.Add(entity);

                _context.SaveChanges();
            }
            catch (ValidationException dbEx)
            {
                throw new Exception(GetFullErrorText(dbEx), dbEx);
            }
        }



        public virtual void Update(T entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException("entity");

                _context.SaveChanges();
            }
            catch (ValidationException dbEx)
            {
                throw new Exception(GetFullErrorText(dbEx), dbEx);
            }
        }


        public virtual void Update(IEnumerable<T> entities)
        {
            try
            {
                if (entities == null)
                    throw new ArgumentNullException("entities");

                _context.SaveChanges();
            }
            catch (ValidationException dbEx)
            {
                throw new Exception(GetFullErrorText(dbEx), dbEx);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                foreach (var entry in ex.Entries)
                {
                    var databaseValues = entry.GetDatabaseValues();
                    entry.OriginalValues.SetValues(databaseValues);
                    _context.SaveChanges();
                }
            }
        }


        public virtual void Delete(T entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException("entity");

                Entities.Remove(entity);

                _context.SaveChanges();
            }
            catch (ValidationException dbEx)
            {
                throw new Exception(GetFullErrorText(dbEx), dbEx);
            }
        }


        public virtual void Delete(IEnumerable<T> entities)
        {
            try
            {
                if (entities == null)
                    throw new ArgumentNullException("entities");

                foreach (var entity in entities)
                    Entities.Remove(entity);

                _context.SaveChanges();
            }
            catch (ValidationException dbEx)
            {
                throw new Exception(GetFullErrorText(dbEx), dbEx);
            }
        }

        public virtual void BulkDelete(IEnumerable<T> entities)
        {
            try
            {
                if (entities == null)
                    throw new ArgumentNullException("entities");

                _context.ChangeTracker.AutoDetectChangesEnabled = false;

                foreach (var entity in entities)
                    Entities.Remove(entity);

                _context.SaveChanges();

                _context.ChangeTracker.AutoDetectChangesEnabled = true;
            }
            catch (ValidationException dbEx)
            {
                throw new Exception(GetFullErrorText(dbEx), dbEx);
            }
        }

        public DbContext GetDbContext()
        {
            return _context;
        }



        public IRepository<T> Init(DbContext dbContext)
        {
            _context = dbContext;
            return this;
        }

        #endregion

        #region Properties
        public virtual IQueryable<T> Table
        {
            get
            {
                return Entities;
            }
        }


        public virtual IQueryable<T> TableNoTracking
        {
            get
            {
                return Entities.AsNoTracking();
            }
        }

        protected virtual DbSet<T> Entities
        {
            get
            {
                if (_entities == null)
                    _entities = _context.Set<T>();
                return _entities;
            }
        }

        #endregion
    }
}
