using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoList.Domain.Models;

namespace ToDoList.Persistence.Repositories
{
    public class ToDoItemsRepository : IRepository<ToDoItem>
    {

        private readonly ToDoItemsContext context;

        public ToDoItemsRepository(ToDoItemsContext context)
        {
            this.context = context;
        }


        public void Create(ToDoItem item)
        {
            context.ToDoItems.Add(item);
            context.SaveChanges();
        }

        public ToDoItem ReadById(int id)
        {
            return context.ToDoItems.Find(id);
        }

        public IEnumerable<ToDoItem> Read()
        {
            return context.ToDoItems.ToList();
        }

        public void UpdateById(int id, ToDoItem item)
        {
            var itemToUpdate = context.ToDoItems.Find(id) ?? throw new ArgumentOutOfRangeException($"ToDo item with ID {item.ToDoItemId} not found.");

            itemToUpdate.Name = item.Name;
            itemToUpdate.Description = item.Description;
            itemToUpdate.IsCompleted = item.IsCompleted;
            itemToUpdate.Category = item.Category;

            context.SaveChanges();
        }

        public void DeleteById(int id)
        {
            var item = context.ToDoItems.Find(id) ?? throw new ArgumentOutOfRangeException($"ToDo item with ID {id} not found.");
            context.ToDoItems.Remove(item);
            context.SaveChanges();
        }
    }
}
