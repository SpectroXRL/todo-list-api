using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
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
            app.MapGet("/todoitems", () => {
                return TypedResults.Ok(todoItems);
                });

            app.MapGet("/todoitems/{id}", Results<Ok<ToDoItem>, NotFound>(int id) =>
            {
                ToDoItem? item = todoItems.Find(item => item.Id == id);

                if(item != null)
                {
                    return TypedResults.Ok(item);
                }
                else
                {
                    return TypedResults.NotFound();
                }
            })
            .WithName("GetTodoItemById");

            app.MapPost("/todoitems", ([FromBody] ToDoItem item) =>
            {
                item.Id = todoItems.Max(item => item.Id) + 1;
                item.IsDone = false;
                todoItems.Add(item);
                return TypedResults.CreatedAtRoute(item, "GetTodoItemById", new { id = item.Id });
            });

            app.MapPut("/todoitems/{id}", Results<NoContent, NotFound>(int id, [FromBody] ToDoItem updatedItem) =>
            {
                ToDoItem? existingItem = todoItems.Find(item => item.Id == id);
                if (existingItem != null)
                {
                    existingItem.Title = updatedItem.Title;
                    existingItem.IsDone = updatedItem.IsDone;
                    existingItem.DueDate = updatedItem.DueDate;
                    return TypedResults.NoContent();
                }
                else
                {
                    return TypedResults.NotFound();
                }
            });

            app.MapDelete("/todoitems/{id}", Results<NoContent, NotFound>(int id) =>
            {
                int index = todoItems.FindIndex(item => item.Id == id);
                if(index != -1)
                {
                    todoItems.RemoveAt(index);
                    return TypedResults.NoContent();
                }
                else
                {
                    return TypedResults.NotFound();
                }
            });
        }
    }
}
