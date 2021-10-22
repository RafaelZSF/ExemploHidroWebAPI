using HidroWebAPI.Aplicacao.Repositorios;
using HidroWebAPI.Infraestrutura.Contexto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace HidroWebAPI.Infraestrutura.BancoDados.Repositorios
{
    public class EntityRepositorioBase<TEntity> : IRepositorioBase<TEntity> where TEntity : class
    {
        internal HidroContexto context;
        internal DbSet<TEntity> dbSet;

        public EntityRepositorioBase(HidroContexto context)
        {
            this.context = context;
            this.dbSet = context.Set<TEntity>();
        }

        public virtual IEnumerable<TEntity> GetWithRawSql(string query,
            params object[] parameters)
        {
            return this.dbSet.FromSqlRaw(query, parameters);
        }


        public virtual IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<TEntity> query = this.dbSet.AsNoTracking();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }


            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }
        }

        public virtual TEntity GetFirst(
            Expression<Func<TEntity, bool>> filter = null,
            string includeProperties = "")
        {
            IQueryable<TEntity> query = this.dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }

            return query.FirstOrDefault();
        }

        public virtual TEntity GetByID(object id)
        {
            return this.dbSet.Find(id);
        }

        public virtual TEntity Insert(TEntity entity)
        {
            this.dbSet.Add(entity);
            this.context.Entry(entity).Reload();
            return entity;
        }

        public virtual void Delete(object id)
        {
            TEntity entityToDelete = this.dbSet.Find(id);
            Delete(entityToDelete);
        }

        public virtual void Delete(TEntity entityToDelete)
        {
            if (context.Entry(entityToDelete).State == EntityState.Detached)
            {
                this.dbSet.Attach(entityToDelete);
            }
            this.dbSet.Remove(entityToDelete);
        }

        public virtual void Update(TEntity entityToUpdate)
        {
            this.dbSet.Attach(entityToUpdate);
            this.context.Entry(entityToUpdate).State = EntityState.Modified;
        }

        public void Commit()
        {
            this.context.SaveChanges();
        }

        public DbDataReader executeQuery(string sql, params object[] parameters)
        {
            var command = context.Database.GetDbConnection().CreateCommand();
            command.CommandText = sql;
            if (parameters != null)
            {
                foreach (var param in parameters)
                {
                    command.Parameters.Add(param);
                }
            }
            context.Database.OpenConnection();
            return command.ExecuteReader();
        }

    }
}
