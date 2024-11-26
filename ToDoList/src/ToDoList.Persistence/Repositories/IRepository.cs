using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoList.Domain.Models;

namespace ToDoList.Persistence.Repositories
{
    public interface IRepository<T> where T : class
    {
        public void Create(T item);

        public T ReadById(int id);

        public IEnumerable<T> Read();

        public void UpdateById(int id, T item);

        public void DeleteById(int id);

    }

    public interface IRepositoryAsync<T> where T : class
    {

        public Task CreateAsync(T item);

        public Task<T> ReadByIdAsync(int id);

        public Task<IEnumerable<T>> ReadAsync();

        public Task UpdateByIdAsync(int id, T item);

        public Task DeleteByIdAsync(int id);
    }
}
