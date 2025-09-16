using todoapi.API.numnum;

namespace todoapi.API.model;

public class ToDoItem : ToDoItemBase
{

    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public bool IsCompleted { get; set; }
    public Priority ListPriority { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }




    // constructor
    public ToDoItem()
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
    public ToDoItem(int id, string title, string Description, Priority ListPriority, DateTime DueDate)
    {
        this.Id = id;
        this.Title = title;
        this.IsCompleted = false;
        this.Description = Description;
        this.DueDate = DueDate;
        this.ListPriority = Priority.Medium;
        this.CreatedAt = DateTime.Now;
        this.UpdatedAt = DateTime.Now;

    }

    public override void UpdateTask(string? title = null, string? Description = null, bool? IsCompleted = null, Priority? ListPriority = null, DateTime? DueDate = null)
    {
        this.Title = title ?? this.Title;
        this.IsCompleted = IsCompleted ?? this.IsCompleted;
        this.Description = Description ?? this.Description;
        this.DueDate = DueDate ?? this.DueDate;
        this.ListPriority = ListPriority ?? this.ListPriority;
        this.UpdatedAt = DateTime.Now;
    }

    public override string ToString()
    {
        return $"ID: {Id}, Title: {Title}, Description: {Description}, Complete: {IsCompleted}, Priority: {ListPriority}, Date of Creation: {CreatedAt}, Last Updated: {UpdatedAt}, Due Date: {DueDate} ";
    }
}