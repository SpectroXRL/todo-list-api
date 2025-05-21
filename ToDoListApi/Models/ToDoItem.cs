using System.ComponentModel.DataAnnotations;

namespace ToDoListApi.Models
{
    public class ToDoItem
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public required string Title { get; set; }

        [Required]
        public bool IsDone { get; set; }

        public DateOnly? DueDate { get; set; }
    }
}
