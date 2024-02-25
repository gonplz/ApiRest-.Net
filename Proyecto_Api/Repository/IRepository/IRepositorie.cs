using System.Linq.Expressions;

namespace Proyecto_Api.Repository.IRepository
{
    public interface IRepositorie<T> where T : class
    {

        Task create(T entity);
        Task delete(T entity);
        Task<T> find(Expression<Func<T, bool>> filtro = null, bool Tracked=true);

        //El signo de interrogación (?) en C# se utiliza para denotar que un tipo de dato es nullable, es decir, que puede tener un valor nulo
        //además de los valores válidos del tipo de dato. En el contexto del método findAll, el signo de interrogación
        //se encuentra después del tipo Expression<Func<T,bool>>, lo que significa que el parámetro filtro puede ser nulo.
        Task<List<T>> findAll(Expression<Func<T,bool>>? filtro =null);

        Task Save();


    }
}
