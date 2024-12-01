

namespace ToDoList.Frontend.Views
{
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.CompilerServices;

    public class ToDoItemView
    {
        public int ToDoItemId { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }


        public string Description { get; set; }

        public bool IsCompleted { get; set; }

        [Required(ErrorMessage = "Category is required.")]
        public string? Category {get; set;}
    }
}
