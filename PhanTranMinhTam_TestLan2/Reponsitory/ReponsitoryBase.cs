using Microsoft.EntityFrameworkCore;
using PhanTranMinhTam_TestLan2.Data;
using System.Linq.Expressions;
namespace PhanTranMinhTam_TestLan2.Reponsitory
{
    public interface IRepositoryBase<T>
    {
        T Create(T entity);
        T Update(T entity);
        void Delete(T entity);
        void CreateMulti(List<T> entities);
        void DeleteMulti(List<T> entities);
        IQueryable<T> FindAll();
        IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression);
        //Task<Cart> GetCartWithItemsAsync(int userId);
        Task<T> AddAsync(T entity); // Sửa thành Task<T>
        Task<T> SingleOrDefaultAsync(Expression<Func<T, bool>> expression);
        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> expression);
    }
    public class ReponsitoryBase<T> : IRepositoryBase<T> where T : class
    {

        private readonly MyDbContext _context;

        public ReponsitoryBase(MyDbContext context)
        {
            _context = context;
        }

        public void CreateMulti(List<T> entities) => _context.Set<T>().AddRange(entities);

        public T Create(T entity) => _context.Set<T>().Add(entity).Entity;

        public T Update(T entity) => _context.Set<T>().Update(entity).Entity;

        public IQueryable<T> FindAll() => _context.Set<T>().AsNoTracking();

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression) => _context.Set<T>().Where(expression).AsNoTracking();

        public void Delete(T entity) => _context.Set<T>().Remove(entity);

        public void DeleteMulti(List<T> entities) => _context.Set<T>().RemoveRange(entities);

        // Implement phương thức AddAsync
        public async Task<T> AddAsync(T entity)
        {
            Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<T> result = await _context.Set<T>().AddAsync(entity);
            return result.Entity;
        }

        // Implement phương thức SingleOrDefaultAsync
        public async Task<T> SingleOrDefaultAsync(Expression<Func<T, bool>> expression)
        {
            return await _context.Set<T>().SingleOrDefaultAsync(expression);
        }

        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> expression)
        {
            return await _context.Set<T>().FirstOrDefaultAsync(expression);
        }
    }
}
