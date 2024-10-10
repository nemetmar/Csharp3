using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoList.Domain.Models;

namespace ToDoList.Domain.DTOs
{
    public record ToDoItemUpdateRequestDto(string Name, string Description, bool IsCompleted)
    {
        public ToDoItem ToDomain()
        {
            return new ToDoItem
            {
                Name = this.Name,
                Description = this.Description,
                IsCompleted = this.IsCompleted
            };
        }

    }
}
