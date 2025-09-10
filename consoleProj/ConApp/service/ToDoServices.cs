using ConApp.model;
namespace ConApp.service;


public class ToDoServices : IToDoServices
{
    private List<ToDoItem> toDoItems;

    public ToDoServices()
    {
        toDoItems = new List<ToDoItem>();
    }
    public void AddItem(int id,string title)
    {
        ToDoItem entry = new ToDoItem(id, title);
        toDoItems.Add(entry);
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
    public bool DeleteItem(int id)
    {
        for (int i = 0; i < toDoItems.Count; i++)
        {
            if (toDoItems[i].Id == id)
            {
                toDoItems.Remove(toDoItems[i]);
                return true;
            }
        }
        return false;
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