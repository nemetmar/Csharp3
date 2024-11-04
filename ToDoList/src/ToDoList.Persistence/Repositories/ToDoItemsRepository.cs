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

        public List<ToDoItem> Read()
        {
            return context.ToDoItems.ToList();
        }

        public bool IdExists(int id)
        {
            if(context.ToDoItems.Find(id) is null)
            {
                return false;
            }
            return true;
        }

        public void UpdateById(int id, ToDoItem item)
        {
            var itemToUpdate = context.ToDoItems.Find(id);

            itemToUpdate.Name = item.Name;
            itemToUpdate.Description = item.Description;
            itemToUpdate.IsCompleted = item.IsCompleted;

            context.SaveChanges();
        }

        public void DeleteById(int id)
        {
            context.ToDoItems.Remove(context.ToDoItems.Find(id));
            context.SaveChanges();
        }
    }
}
