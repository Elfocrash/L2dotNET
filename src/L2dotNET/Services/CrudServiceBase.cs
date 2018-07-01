using System.Collections.Generic;
using System.Threading.Tasks;
using L2dotNET.Repositories.Abstract;
using L2dotNET.Services.Contracts;

namespace L2dotNET.Services
{
    public class CrudServiceBase<T> : ICrudService<T> where T : class
    {
        private readonly ICrudRepository<T> _crudRepository;

        public CrudServiceBase(ICrudRepository<T> crudRepository)
        {
            _crudRepository = crudRepository;
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await _crudRepository.GetAll();
        }

        public async Task<T> GetById(object id)
        {
            return await _crudRepository.GetById(id);
        }

        public async Task<int?> Add(T model)
        {
            return await _crudRepository.Add(model);
        }

        public void Update(T model)
        {
            _crudRepository.Update(model);
        }

        public void Delete(T model)
        {
            _crudRepository.Delete(model);
        }
    }
}
