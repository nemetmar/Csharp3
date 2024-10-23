using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoList.Domain.Models
{
    public class ToDoItem
    {
        [Key]
        public int ToDoItemId { get; set; } //ef core looks for filed <id> od <nameiId>
        [Length(1, 50)]
        public string Name { get; set; }
        [StringLength(250)]
        public string Description { get; set; }

        public bool IsCompleted { get; set; }
    }
}
