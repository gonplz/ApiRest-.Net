using Microsoft.EntityFrameworkCore;
using Proyecto_Api.Crud;
using Proyecto_Api.Repository.IRepository;
using System.Linq.Expressions;

namespace Proyecto_Api.Repository
{
    public class Repositorie<T> : IRepositorie<T> where T : class
    {
        private readonly DataBaseContext _context;
        internal DbSet<T> dbSet;

        public Repositorie(DataBaseContext dataBase)
        {
            _context = dataBase;
            this.dbSet = _context.Set<T>();
        }

        public async Task create(T entity)
        {
          await _context.AddAsync(entity);
            await Save();
        }

        public async Task delete(T entity)
        {
            _context.Remove(entity);
            await Save();
        }

        public async Task<T> find(Expression<Func<T, bool>> filtro = null, bool Tracked = true)
        {
            IQueryable<T> query = dbSet;
            if (!Tracked)
            {
                query = query.AsNoTracking();
            }
            if (filtro != null)
            {
              query = query.Where(filtro);
            }

            return await query.FirstOrDefaultAsync();

        }

        public async Task<List<T>> findAll(Expression<Func<T, bool>>? filtro = null)
        {
            IQueryable<T> query = dbSet;
            if (filtro != null)
            {
                query = query.Where(filtro);
            }

            return await query.ToListAsync();
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync(); 
        }
    }
}
