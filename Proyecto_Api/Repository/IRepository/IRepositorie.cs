using System.Linq.Expressions;

namespace Proyecto_Api.Repository.IRepository
{
    public interface IRepositorie<T> where T : class
    {
        Task create(T entity);
        Task delete(T entity);
        Task<T> find(Expression<Func<T, bool>> filtro = null, bool Tracked=true);
        Task<List<T>> findAll(Expression<Func<T,bool>>? filtro =null);
        Task Save();
    }
}
