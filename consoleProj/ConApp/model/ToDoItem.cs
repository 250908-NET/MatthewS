namespace ConApp.model;

    public class    ToDoItem : ToDoItemBase
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime CreatedDate { get; set; }

    

    // constructor
    public ToDoItem()
    {
        this.Id = 0;
        this.Title = "";
        this.IsCompleted = false;
        this.CreatedDate = DateTime.Now;
    }
        public ToDoItem(int id, string title)
        {
            this.Id = id;
            this.Title = title;
            this.IsCompleted = false;
            this.CreatedDate = DateTime.Now;
        }

        public override string ToString()
        {
            return $"ID: {Id}, Title: {Title}, Complete: {IsCompleted}, Date of Creation: {CreatedDate}";
        }
    }