using todoapi.API.numnum;


namespace todoapi.API.model;

public abstract class ToDoItemBase
{
    // fields
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public bool IsCompleted { get; set; }
    public Priority ListPriority { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public abstract void UpdateTask(string? title = null, string? Description = null, bool? IsCompleted = null, Priority? ListPriority = null, DateTime? DueDate = null);
    // constructor
    public override string ToString()
    {
        return $"ID: {Id}, Title: {Title}, Description: {Description}, Complete: {IsCompleted}, Priority: {ListPriority}, Date of Creation: {CreatedDate}, Last Updated: {UpdatedAt} ";
    }
}