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

    }
}
