using todoapi.API.model;
using todoapi.API.numnum;
namespace todoapi.API.service;

/*
    Name: IToDoServices
    Function: this functions main purpose is to serve as a point of refrence for ToDoServices and other super classes. 
    Children: ToDoServices
*/

public interface IToDoServices
{
    ToDoItem GetItemById(int id); // grab a item by there id
    /*
        To Filter the List based on title, description,completed,priority,dueBefore.
        Appies Pagination Support and Sorting
    */
    public List<ToDoItem> filteredDisplay(string? title, string? description, bool? completed = null, Priority? priority = null, DateTime? dueBefore = null, int page = 0, int pageSize = 0, SortedBy? sorting = null);
    /*
        Add a New Task
    */
    int AddItem(string title, string Description, Priority ListPriority, DateTime DueDate);
    /*
        update a new item that matches the requested id
    */
    bool UpdateTodo(int id, string? title = null, string? Description = null, bool? IsCompleted = null, Priority? ListPriority = null, DateTime? DueDate = null);
    bool MarkItemAsCompleted(int id); // change Complete status as true, send a error if the id does not exist
    bool MarkItemAsNotCompleted(int id); // change Complete status as false, send a error if the id does not exist
    ToDoItem DeleteItem(int id); // delete a Task of the given id
}