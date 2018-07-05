using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using L2dotNET.Logging.Abstraction;
using L2dotNET.Repositories.Utils;
using NLog;
using PeregrineDb;

namespace L2dotNET.Repositories.Abstract
{
    public class CrudRepositoryBase<T> : ICrudRepository<T> where T: class
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public async Task<IEnumerable<T>> GetAll()
        {
            try
            {
                using (IDatabase database = ConnectionFactory.Open())
                {
                    return await database.GetAllAsync<T>();
                }
            }
            catch (Exception ex)
            {
                Log.Halt($"Method: {nameof(GetAll)}. Message: '{ex.Message}'");
                throw;
            }
        }

        public async Task<T> GetById(object id)
        {
            try
            {
                using (IDatabase database = ConnectionFactory.Open())
                {
                    return await database.GetAsync<T>(id);
                }
            }
            catch (Exception ex)
            {
                Log.Halt($"Method: {nameof(GetById)}. Message: '{ex.Message}'");
                throw;
            }
        }

        public async Task<int?> Add(T model)
        {
            try
            {
                using (IDatabase database = ConnectionFactory.Open())
                {
                    return await database.InsertAsync<int?>(model);
                }
            }
            catch (Exception ex)
            {
                Log.Halt($"Method: {nameof(Add)}. Message: '{ex.Message}'");
                throw;
            }
        }

        public async void Update(T model)
        {
            try
            {
                using (IDatabase database = ConnectionFactory.Open())
                {
                    await database.UpdateAsync(model);
                }
            }
            catch (Exception ex)
            {
                Log.Halt($"Method: {nameof(Update)}. Message: '{ex.Message}'");
                throw;
            }
        }

        public async void Delete(T model)
        {
            try
            {
                using (IDatabase database = ConnectionFactory.Open())
                {
                    await database.DeleteAsync(model);
                }
            }
            catch (Exception ex)
            {
                Log.Halt($"Method: {nameof(Delete)}. Message: '{ex.Message}'");
                throw;
            }
        }
    }
}
