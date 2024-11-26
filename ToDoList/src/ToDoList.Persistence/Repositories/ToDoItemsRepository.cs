using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ToDoList.Domain.Models;

namespace ToDoList.Persistence.Repositories
{
    public class ToDoItemsRepository : IRepositoryAsync<ToDoItem>
    {

        private readonly ToDoItemsContext context;

        public ToDoItemsRepository(ToDoItemsContext context)
        {
            this.context = context;
        }


        public async Task CreateAsync(ToDoItem item)
        {
            await context.ToDoItems.AddAsync(item);
            await context.SaveChangesAsync();
        }

        public async Task<ToDoItem> ReadByIdAsync(int id)
        {
            return await context.ToDoItems.FindAsync(id);
        }

        public async Task<IEnumerable<ToDoItem>> ReadAsync()
        {
            return await context.ToDoItems.ToListAsync();
        }

        public async Task UpdateByIdAsync(int id, ToDoItem item)
        {
            var itemToUpdate = await context.ToDoItems.FindAsync(id) ?? throw new ArgumentOutOfRangeException($"ToDo item with ID {item.ToDoItemId} not found.");

            itemToUpdate.Name = item.Name;
            itemToUpdate.Description = item.Description;
            itemToUpdate.IsCompleted = item.IsCompleted;
            itemToUpdate.Category = item.Category;

            await context.SaveChangesAsync();
        }

        public async Task DeleteByIdAsync(int id)
        {
            var item = await context.ToDoItems.FindAsync(id) ?? throw new ArgumentOutOfRangeException($"ToDo item with ID {id} not found.");
            context.ToDoItems.Remove(item);
            await context.SaveChangesAsync();
        }
    }
}
