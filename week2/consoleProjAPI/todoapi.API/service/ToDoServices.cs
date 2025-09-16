using todoapi.API.model;
using todoapi.API.numnum;
using System.Linq;
namespace todoapi.API.service;


public class ToDoServices : IToDoServices
{
    private List<ToDoItem> toDoItems;

    private int id = 0;

    public ToDoServices()
    {
        toDoItems = new List<ToDoItem>();
    }
    public List<ToDoItem> filteredDisplay(bool? completed = null, Priority? priority = null, DateTime? dueBefore = null)
    {
        List<ToDoItem> FilterItems = toDoItems.Where((x) =>
        {
            if ((completed == null || completed == x.IsCompleted) && (priority == null || priority == x.ListPriority) && (dueBefore == null || x.DueDate < dueBefore))
            {
                return true;
            }
            else
            {
                return false;
            }
        }).ToList();
        return FilterItems;
        
    }
    public int AddItem(string title, string Description, Priority ListPriority, DateTime DueDate)
    {
        id++;

        ToDoItem entry = new ToDoItem(id, title, Description, ListPriority, DueDate);
        toDoItems.Add(entry);

        return id;
    }
    public bool UpdateTodo(int id, string? title = null, string? Description = null, bool? IsCompleted = null, Priority? ListPriority = null, DateTime? DueDate = null)
    {
        
        for (int i = 0; i < toDoItems.Count; i++)
        {
            if (toDoItems[i].Id == id)
            {
                toDoItems[i].UpdateTask(title, Description, IsCompleted, ListPriority, DueDate);
                return true;
            }
        }
        return false;
    }
    public bool MarkItemAsCompleted(int id)
    {
        for (int i = 0; i < toDoItems.Count; i++)
        {
            if (toDoItems[i].Id == id)
            {
                toDoItems[i].IsCompleted = true;
                return true;
            }
        }
        return false;
    }
    public bool MarkItemAsNotCompleted(int id)
    {
        for (int i = 0; i < toDoItems.Count; i++)
        {
            if (toDoItems[i].Id == id)
            {
                toDoItems[i].IsCompleted = false;
                return true;
            }
        }
        return false;
    }
    public ToDoItem DeleteItem(int id)
    {
        toDoItems removedItem = new toDoItems();
        for (int i = 0; i < toDoItems.Count; i++)
        {
            if (toDoItems[i].Id == id)
            {
                removedItem = toDoItems[i];
                toDoItems.Remove(toDoItems[i]);
                return removedItem;
            }
        }
        return null;
    }
    public ToDoItem GetItemById(int id)
    {
        for (int i = 0; i < toDoItems.Count; i++)
        {
            if (toDoItems[i].Id == id)
            {
                return toDoItems[i];
            }
        }
        return null;
    }
    // return all items ToString Override
    public override string ToString()
    {
        string result = "";
        for (int i = 0; i < toDoItems.Count; i++)
        {
            result += toDoItems[i].ToString() + "\n";
        }
        return result;
    }
}