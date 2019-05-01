using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using CourseworkBD.DAL.DbContext;
using CourseworkBD.DAL.Models;
//using MVC_Practice.Models.DbModels;
using System.Data.Entity;
using System.Threading.Tasks;

namespace MVC_Practice.Repository
{
    public class Repository<Model>:IDisposable where Model : class
    {
        public CourseworkDBContext context;
        private DbSet<Model> set;

        public Repository()
        {
            context = new CourseworkDBContext();
            set = context.Set<Model>();
        }

        public async Task<IList<Model>> GetAllAsync()
        {
            return await set.ToListAsync();
        }

        public async Task<Model> GetAsync(int id)
        {
            return await set.FindAsync(id);
        }

        public bool Add(Model model)
        {
            try
            {
                set.Add(model);
                return true;
            }
            catch(Exception e)
            {
                return false;
            }
        }

        public bool Update(Model model)
        {
            try
            {
                context.Entry<Model>(model).State = EntityState.Modified;
                set = context.Set<Model>();
                return true;
            }
            catch(Exception e)
            {
                return false;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var instance = await set.FindAsync(id);
                set.Remove(instance);
                return true;
            }
            catch(Exception e)
            {
                return false;
            }
        }

        public async Task SaveAsync()
        {
            await context.SaveChangesAsync();
            set = context.Set<Model>();
        }

        public void Save()
        {
            context.SaveChanges();
            set = context.Set<Model>();
        }

        #region Disposing
        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}