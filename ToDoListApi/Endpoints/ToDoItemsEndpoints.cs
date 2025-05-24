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
            var todoItemsGroup = app.MapGroup("/todoitems");

            todoItemsGroup.MapGet("/", GetAllToDoItems);

            todoItemsGroup.MapGet("/{id}", GetToDoItemById)
            .WithName("GetTodoItemById");

            todoItemsGroup.MapPost("/", CreateToDoItem);

            todoItemsGroup.MapPut("/{id}", UpdateToDoItem);

            todoItemsGroup.MapDelete("/todoitems/{id}", DeleteToDoItem);
        }

        public static Ok<List<ToDoItem>> GetAllToDoItems()
        {
            return TypedResults.Ok(todoItems);
        }

        public static Results<Ok<ToDoItem>, NotFound> GetToDoItemById(int id)
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
        }

        public static CreatedAtRoute<ToDoItem> CreateToDoItem([FromBody] ToDoItem item)
        {
            item.Id = todoItems.Max(item => item.Id) + 1;
            item.IsDone = false;
            todoItems.Add(item);
            return TypedResults.CreatedAtRoute(item, "GetTodoItemById", new { id = item.Id});
        }

        public static Results<NoContent, NotFound> UpdateToDoItem(int id, [FromBody] ToDoItem updatedItem)
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
        }

        public static Results<NoContent, NotFound> DeleteToDoItem(int id)
        {
            int index = todoItems.FindIndex(item => item.Id == id);
            if (index != -1)
            {
                todoItems.RemoveAt(index);
                return TypedResults.NoContent();
            }
            else
            {
                return TypedResults.NotFound();
            }
        }
    }
}
