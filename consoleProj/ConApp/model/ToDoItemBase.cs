namespace ConApp.model;

public abstract class ToDoItemBase
{
    // fields
    public int Id { get; set; }
    public string Title { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime CreatedDate { get; set; }
    // constructor
    public override string ToString()
    {
        return $"[ID: {Id}] Title: {Title}, Completed: {IsCompleted}, Created On: {CreatedDate}";
    }
}