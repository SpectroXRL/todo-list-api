using Microsoft.AspNetCore.Mvc;
using ToDoListApi.Models;

namespace ToDoListApi.Endpoints
{
    public static class ToDoItemsEndpoints
    {
        static List<ToDoItem> todoItems = new()
        {
            new ToDoItem { Id = 1, Title = "Buy groceries", IsDone = false },
            new ToDoItem { Id = 2, Title = "Walk the dog", IsDone = true },
            new ToDoItem { Id = 3, Title = "Read a book", IsDone = false }
        };

        public static void RegisterToDoItemEndpoints(this WebApplication app)
        {
            app.MapGet("/todoitems", () => todoItems);

            app.MapGet("/todoitems/{id}", (int id) =>
            {
                 return todoItems.Find(item => item.Id == id);
            });

            app.MapPost("/todoitems", ([FromBody] ToDoItem item) =>
            {
                item.Id = todoItems.Max(item => item.Id) + 1;
                item.IsDone = false;
                todoItems.Add(item);
            });

            app.MapPut("/todoitems/{id}", (int id, [FromBody] ToDoItem updatedItem) =>
            {
                ToDoItem? existingItem = todoItems.Find(item => item.Id == id);
                if (existingItem != null)
                {
                    existingItem.Title = updatedItem.Title;
                    existingItem.IsDone = updatedItem.IsDone;
                    existingItem.DueDate = updatedItem.DueDate;
                }
                else
                {

                }
            });

            app.MapDelete("/todoitems/{id}", (int id) =>
            {
                int index = todoItems.FindIndex(item => item.Id == id);
                if(index != -1)
                {
                    todoItems.RemoveAt(index);
                }
                else
                {

                }
            });
        }
    }
}
