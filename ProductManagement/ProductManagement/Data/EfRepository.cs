using Microsoft.EntityFrameworkCore;
using ProductManagement.Model;
using System.ComponentModel.DataAnnotations;

namespace ProductManagement.Data
{
    public partial class EfRepository<T> : IRepository<T> where T : BaseEntity
    {
        #region Fields

        private DbContext _context;
        private DbSet<T> _entities;

        #endregion

        public EfRepository(DbContext context)
        {
            this._context = context;
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
            return this.Entities.Find(id);
        }

       
        public virtual void Insert(T entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException("entity");

                this.Entities.Add(entity);

                this._context.SaveChanges();
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
                    this.Entities.Add(entity);

                this._context.SaveChanges();
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

                this._context.SaveChanges();
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

                this._context.SaveChanges();
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
                    this._context.SaveChanges();
                }
            }
        }

      
        public virtual void Delete(T entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException("entity");

                this.Entities.Remove(entity);

                this._context.SaveChanges();
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
                    this.Entities.Remove(entity);

                this._context.SaveChanges();
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

                this._context.ChangeTracker.AutoDetectChangesEnabled = false;

                foreach (var entity in entities)
                    this.Entities.Remove(entity);

                this._context.SaveChanges();

                this._context.ChangeTracker.AutoDetectChangesEnabled = true;
            }
            catch (ValidationException dbEx)
            {
                throw new Exception(GetFullErrorText(dbEx), dbEx);
            }
        }

        public DbContext GetDbContext()
        {
            return this._context;
        }

      

        public IRepository<T> Init(DbContext dbContext)
        {
            this._context = dbContext;
            return this;
        }

        #endregion

        #region Properties
        public virtual IQueryable<T> Table
        {
            get
            {
                return this.Entities;
            }
        }

        
        public virtual IQueryable<T> TableNoTracking
        {
            get
            {
                return this.Entities.AsNoTracking();
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
