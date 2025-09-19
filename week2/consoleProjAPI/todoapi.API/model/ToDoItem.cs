using todoapi.API.numnum;

namespace todoapi.API.model;

/*
    Name: IToDoServices
    Function: this functions main purpose is to serve as a point of refrence for ToDoServices and other super classes. 
    Children: ToDoServices
*/

public class ToDoItem : ToDoItemBase
{

    public int Id { get; set; }//the main indicator of the class
    public string Title { get; set; } // the name of the task
    public string Description { get; set; } // describing the purpose of the tasks
    public bool IsCompleted { get; set; } // check if the task is completed
    public Priority ListPriority { get; set; }// checks the main priority of the task
    public DateTime DueDate { get; set; } // Due Date of the Task
    public DateTime CreatedAt { get; set; } // When the Task was Created
    public DateTime UpdatedAt { get; set; } // When the Task was last Updated




    // constructor
    public ToDoItem() // defualt task
    {
        this.Id = 0;
        this.Title = "";
        this.IsCompleted = false;
        this.Description = "";
        this.DueDate = DateTime.Now;
        this.ListPriority = Priority.Medium;
        this.CreatedAt = DateTime.Now;
        this.UpdatedAt = DateTime.Now;

    }
    public ToDoItem(int id, string title, string Description, Priority ListPriority, DateTime DueDate) // initializing Item if given all variables needed
    {
        this.Id = id;
        this.Title = title;
        this.IsCompleted = false;
        this.Description = Description;
        this.DueDate = DueDate;
        this.ListPriority = ListPriority;
        this.CreatedAt = DateTime.Now;
        this.UpdatedAt = DateTime.Now;

    }
    /*
        Update the Values of the Tasks
    */
    public override void UpdateTask(string? title = null, string? Description = null, bool? IsCompleted = null, Priority? ListPriority = null, DateTime? DueDate = null)
    {
        this.Title = title ?? this.Title;
        this.IsCompleted = IsCompleted ?? this.IsCompleted;
        this.Description = Description ?? this.Description;
        this.DueDate = DueDate ?? this.DueDate;
        this.ListPriority = ListPriority ?? this.ListPriority;
        this.UpdatedAt = DateTime.Now;
    }
    /*
        Override string print the whole class
    */
    public override string ToString()
    {
        return $"ID: {Id}, Title: {Title}, Description: {Description}, Complete: {IsCompleted}, Priority: {ListPriority}, Date of Creation: {CreatedAt}, Last Updated: {UpdatedAt}, Due Date: {DueDate} ";
    }
}