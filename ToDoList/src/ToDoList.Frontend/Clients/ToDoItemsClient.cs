using ToDoList.Domain.DTOs;
using ToDoList.Frontend.Views;

namespace ToDoList.Frontend.Clients
{
    public class ToDoItemsClient : IToDoItemsClient //zjednodušený zápis konstruktoru
    {

        private readonly HttpClient httpClient;
        public ToDoItemsClient(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<List<ToDoItemView>> ReadItemsAsync()
        {
            var toDoItemsView = new List<ToDoItemView>();
            var response = await httpClient.GetFromJsonAsync<List<ToDoItemGetResponseDto>>("api/ToDoItems");

            toDoItemsView = response.Select(dto => new ToDoItemView
            {
                ToDoItemId = dto.ToDoItemId,
                Name = dto.Name,
                Description = dto.Description,
                IsCompleted = dto.IsCompleted,
                Category= dto.Category

            }).ToList();
            return toDoItemsView;
        }

        public async Task<ToDoItemView?> ReadItemByIdAsync(int itemId)
        {
            try
            {
                var response = await httpClient.GetFromJsonAsync<ToDoItemGetResponseDto>($"api/TodoItems/{itemId}");
                if (response is null)
                {
                    Console.WriteLine($"GET request failed: Item with {itemId} id not found.");
                    throw new ArgumentException($"Given id {itemId} does not exist.");
                }

                return new()
                {
                    ToDoItemId = response.ToDoItemId,
                    Name = response.Name,
                    Description = response.Description,
                    IsCompleted = response.IsCompleted,
                    Category = response.Category
                };
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception occured: {e.Message}");
                return null;
            }
        }

        public async Task CreateItemAsync(ToDoItemView itemView)
        {
            var itemRequest = new ToDoItemCreateRequestDto(itemView.Name, itemView.Description, itemView.IsCompleted, itemView.Category);
            try
            {
                var response = await httpClient.PostAsJsonAsync("api/ToDoItems", itemRequest);
                if (response.StatusCode == System.Net.HttpStatusCode.Created)
                {
                    Console.WriteLine($"POST request successful: Created a ToDoItem.");
                    return;
                }
                else
                {
                    Console.WriteLine($"POST request failed with status code: {response.StatusCode}");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception occured: {e.Message}");
            }
        }

        public async Task UpdateItemAsync(ToDoItemView itemView)
        {
            try
            {
                var itemRequest = new ToDoItemUpdateRequestDto(itemView.Name, itemView.Description, itemView.IsCompleted, itemView.Category);
                var response = await httpClient.PutAsJsonAsync($"api/ToDoItems/{itemView.ToDoItemId}", itemRequest);
                if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    Console.WriteLine($"PUT request successful: Updated ToDoItem with id {itemView.ToDoItemId}.");
                    return;
                }
                else
                {
                    Console.WriteLine($"PUT request failed: {response.StatusCode}");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception occured: {e.Message}");
            }
        }


        public async Task DeleteItemAsync(ToDoItemView itemView)
        {
            try
            {
                var response = await httpClient.DeleteAsync($"api/ToDoItems/{itemView.ToDoItemId}");
                if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    Console.WriteLine($"DELETE request successful: Deleted ToDoItem with id {itemView.ToDoItemId}.");
                    return;
                }
                else
                {
                    Console.WriteLine($"DELETE request failed with status code: {response.StatusCode}");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception occured: {e.Message}");
            }
        }

    }
}
