using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using BarTicaret.Application.Abstractions;
using BarTicaret.Domain.Entities;
using BarTicaret.Infrastructure.Data;

namespace BarTicaret.Infrastructure.Repositories
{
    public class EfRepository<T> : IRepository<T> where T : class
    {
        private readonly AppDbContext _ctx;

        public EfRepository(AppDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            if (typeof(T) == typeof(Urun))
            {
                var q = _ctx.Set<Urun>()
                    .Include(u => u.Kategori)
                    .Include(u => u.UrunMateryaller)
                        .ThenInclude(um => um.Materyal)
                    .AsNoTracking();

                var found = await q.FirstOrDefaultAsync(u => u.Id == id);
                return found as T;
            }

            return await _ctx.Set<T>().FindAsync(id);
        }

        public async Task<IReadOnlyList<T>> ListAsync(Expression<Func<T, bool>>? predicate = null)
        {
            if (typeof(T) == typeof(Urun))
            {
                IQueryable<Urun> q = _ctx.Set<Urun>()
                    .Include(u => u.Kategori)
                    .Include(u => u.UrunMateryaller)
                        .ThenInclude(um => um.Materyal)
                    .AsNoTracking();

                if (predicate != null)
                    q = q.Where((Expression<Func<Urun, bool>>)(object)predicate);

                var list = await q.ToListAsync();
                return list.Cast<T>().ToList();
            }

            IQueryable<T> query = _ctx.Set<T>().AsNoTracking();
            if (predicate != null) query = query.Where(predicate);
            return await query.ToListAsync();
        }

        public async Task<T> AddAsync(T entity)
        {
            _ctx.Set<T>().Add(entity);
            await _ctx.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(T entity)
        {
            _ctx.Set<T>().Update(entity);
            await _ctx.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            _ctx.Set<T>().Remove(entity);
            await _ctx.SaveChangesAsync();
        }
    }
}
