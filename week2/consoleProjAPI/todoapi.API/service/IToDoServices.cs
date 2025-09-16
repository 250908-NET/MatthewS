using todoapi.API.model;
using todoapi.API.numnum;
namespace todoapi.API.service;

public interface IToDoServices
{
    ToDoItem GetItemById(int id);
    public List<ToDoItem> filteredDisplay(bool? completed = null, Priority? priority = null, DateTime? dueBefore = null);
    int AddItem(string title,string Description,Priority ListPriority,DateTime DueDate);
    
    bool UpdateTodo(int id, string? title = null, string? Description = null, bool? IsCompleted = null, Priority? ListPriority = null, DateTime? DueDate = null);
    bool MarkItemAsCompleted(int id);
    bool MarkItemAsNotCompleted(int id);
    ToDoItem DeleteItem(int id);
}