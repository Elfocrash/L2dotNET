using System.Collections.Generic;
using System.Threading.Tasks;

namespace L2dotNET.Repositories.Abstract
{
    public interface ICrudRepository<T> where T: class
    {
        Task<IEnumerable<T>> GetAll();

        Task<T> GetById(object id);

        Task<int?> Add(T model);

        void Update(T model);

        void Delete(T model);
    }
}
