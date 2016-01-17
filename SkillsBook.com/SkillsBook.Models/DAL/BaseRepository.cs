using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkillsBook.Models.Models;
using System.Linq.Expressions;

namespace SkillsBook.Models.DAL
{
    /*
     * This class is the base repsitory which other repositories can own it by passing its name.
     * 
     */
    public class BaseRepository<TEntity> where TEntity : class
    {
        internal SiteContext Context;
        internal DbSet<TEntity> DbSet;

        public BaseRepository(SiteContext context)
        {
            this.Context = context;
            this.DbSet = Context.Set<TEntity>();
        }
        /**
         * if the repository is instantiated for the Student entity type,
         * the code in the calling method might specify student => student.LastName == "Smith" for the filter parameter.
         * if the repository is instantiated for the Student entity type, the code in 
         * the calling method might specify q => q.OrderBy(s => s.LastName) for the orderBy parameter.
         */
        public virtual IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = ""){

            IQueryable<TEntity> query = DbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            query = includeProperties.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries)
                    .Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

            return orderBy != null ? orderBy(query).ToList() : query.ToList();
            }
        
        
        public virtual TEntity GetById(object id)
        {
            return this.DbSet.Find(id);
        }
        
        public virtual void Insert(TEntity entity)
        {
            DbSet.Add(entity);
        }

        public virtual void Delete(object id)
        {
            TEntity entityToDelete = DbSet.Find(id);
            Delete(entityToDelete);
        }

        public virtual void Delete(TEntity entityToDelete)
        {
            if (Context.Entry(entityToDelete).State == EntityState.Detached)
            {
                DbSet.Attach(entityToDelete);
            }
            DbSet.Remove(entityToDelete);
        }

        public virtual void DeleteRows(TEntity entityToDelete)
        {
            if (Context.Entry(entityToDelete).State == EntityState.Detached)
            {
                DbSet.Attach(entityToDelete);
            }
            DbSet.Remove(entityToDelete);
        }

        public virtual void Update(TEntity entityToUpdate)
        {
            DbSet.Attach(entityToUpdate);
            Context.Entry(entityToUpdate).State = EntityState.Modified;
        }
       
        public virtual IEnumerable<TEntity> GetWithRawSql(string query, params object[] parameters)
        {
            return DbSet.SqlQuery(query, parameters).ToList();
        }
        
    }
}
