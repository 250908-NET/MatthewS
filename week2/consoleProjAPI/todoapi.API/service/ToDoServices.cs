using todoapi.API.model;
using todoapi.API.numnum;
using System.Linq;
namespace todoapi.API.service;

/*
    Name: ToDoServices
    Function: ToDoServices purpose is to manage the Services of a toDoItems list, providing functions like 
    Filtering, Adding, Updating, Marking Task, Deleting, and Retrieving.
    Parent: IToDoServices
*/

public class ToDoServices : IToDoServices
{
    private List<ToDoItem> toDoItems; // List of toDoItems


    private int id = 0;

    public ToDoServices()
    {
        toDoItems = new List<ToDoItem>(); //Declared List
    }
    /*
        To Filter the List based on title, description,completed,priority,dueBefore.
        Appies Pagination Support and Sorting
    */
    public List<ToDoItem> filteredDisplay(string? title, string? description, bool? completed = null, Priority? priority = null, DateTime? dueBefore = null, int page = 0, int pageSize = 0, SortedBy? sorting = null)
    {
        /*
            Where: this will create a List of items that has the correct parameters
        */
        List <ToDoItem> FilterItems = toDoItems.Where((x) =>
        {
            // if all these variables are true, it is added to the list. Note: null means the user has not put something in meaning, he doesnt want that variable to be filtered
            if ((completed == null || completed == x.IsCompleted) && (priority == null || priority == x.ListPriority) && (dueBefore == null || x.DueDate < dueBefore) && (title == null || x.Title.ToUpper().Contains(title.ToUpper())) && (description == null || x.Description.ToUpper().Contains(description.ToUpper())))
            {
                return true;
            }
            else
            {
                return false; // task is not added to the list
            }
        }).ToList();
        // using enum sorting to sort based off Created Date, Priority, and Due Date
        switch (sorting)
        {
            case SortedBy.createdAt:
                FilterItems = FilterItems.OrderBy(f => f.CreatedAt).ToList();
                break;
            case SortedBy.priority:
                FilterItems = FilterItems.OrderBy(f => f.ListPriority).ToList();
                break;
            case SortedBy.dueDate:
                FilterItems = FilterItems.OrderBy(f => f.DueDate).ToList();
                break;
            default:
                break;
        }
        // if pages are implemented, only display page x that is has a page size of y
        if (pageSize != 0 && page != 0)
        {
            int lastPage;
            if ((page - 1) * pageSize >= FilterItems.Count)
            {
                throw new KeyNotFoundException("Page Size times the number of pages are too high");
            }
            if (page * pageSize > FilterItems.Count)
            {
                lastPage = (FilterItems.Count % pageSize);
                FilterItems = FilterItems.Skip((page - 1) * pageSize).Take(lastPage).ToList();
            }// determining if there are not enough items for there to be variables for the list, throw a error down if thats the case
            else
            {
                FilterItems = FilterItems.Skip((page - 1) * pageSize).Take(pageSize).ToList();// Filter will now change to display only that page
            }
            

        }
        return FilterItems;

    }
    /*
        Add a New Task
    */
    public int AddItem(string title, string Description, Priority ListPriority, DateTime DueDate)
    {
        id++;

        ToDoItem entry = new ToDoItem(id, title, Description, ListPriority, DueDate); //declare a new ToDoItem
        toDoItems.Add(entry);

        return id;
    }
    /*
        update a new item that matches the requested id
    */
    public bool UpdateTodo(int id, string? title = null, string? Description = null, bool? IsCompleted = null, Priority? ListPriority = null, DateTime? DueDate = null)
    {

        for (int i = 0; i < toDoItems.Count; i++)
        {
            if (toDoItems[i].Id == id) //if matching id then update that task with the new parameters
            {
                toDoItems[i].UpdateTask(title, Description, IsCompleted, ListPriority, DueDate);
                return true;
            }
        }
        return false; // this means the id was not found, and is being sent a false
    }
    public bool MarkItemAsCompleted(int id) // change Complete status as true, send a false if the id does not exist
    {
        for (int i = 0; i < toDoItems.Count; i++)
        {
            if (toDoItems[i].Id == id)
            {
                toDoItems[i].IsCompleted = true;
                return true;
            }
        }
        return false; // this means the id was not found, and is being sent a false
    }
    public bool MarkItemAsNotCompleted(int id) // change Complete status as false, send a false if the id does not exist
    {
        for (int i = 0; i < toDoItems.Count; i++)
        {
            if (toDoItems[i].Id == id)
            {
                toDoItems[i].IsCompleted = false;
                return true;
            }
        }
        return false; // this means the id was not found, and is being sent a false
    }
    public ToDoItem DeleteItem(int id) // delete a Task of the given id
    {
        ToDoItem removedItem = new ToDoItem();
        for (int i = 0; i < toDoItems.Count; i++)
        {
            if (toDoItems[i].Id == id)
            {
                /*Removing the item then sending back the removed item*/
                removedItem = toDoItems[i];
                toDoItems.Remove(toDoItems[i]);
                return removedItem; 
            }
        }
        return null; // this means the id was not found, and is being sent a null
    }
    public ToDoItem GetItemById(int id) // grab a item by there id
    {
        for (int i = 0; i < toDoItems.Count; i++)
        {
            if (toDoItems[i].Id == id)
            {
                return toDoItems[i]; // return the selected id back
            }
        }
        return null;// this means the id was not found, and is being sent a null
    }
    // return all items ToString Override
    public override string ToString()
    {
        string result = "";
        for (int i = 0; i < toDoItems.Count; i++) //print the entire list, unfiltered
        {
            result += toDoItems[i].ToString() + "\n";
        }
        return result;
    }
}