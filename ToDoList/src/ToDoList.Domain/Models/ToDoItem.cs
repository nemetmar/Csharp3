using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoList.Domain.Models
{
    public class ToDoItem
    {
        public int ToDoItemId { get; set;}

        public string Name { get; set;}

        public string Description { get; set;}

        public bool IsCompleted { get; set;}
    }
}
