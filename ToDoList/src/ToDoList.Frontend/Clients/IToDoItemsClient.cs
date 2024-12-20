using ToDoList.Frontend.Views;

namespace ToDoList.Frontend.Clients
{
    public interface IToDoItemsClient
    {
        public Task<List<ToDoItemView>> ReadItemsAsync();
        public Task<ToDoItemView?> ReadItemByIdAsync(int itemId);
        public Task CreateItemAsync(ToDoItemView itemView);
        public Task UpdateItemAsync(ToDoItemView itemView);
        public Task DeleteItemAsync(ToDoItemView itemView);

    }
}
