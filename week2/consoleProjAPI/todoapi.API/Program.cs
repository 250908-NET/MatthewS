using Serilog; // logging library
using Microsoft.AspNetCore.Mvc;
using todoapi.API.model;
using todoapi.API.service;
using todoapi.API.numnum;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi




builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration).CreateLogger();

builder.Host.UseSerilog();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}


ToDoServices toDoServices = new ToDoServices();
Priority priority = new Priority();
// desplaying all inputs
app.MapGet("/api/tasks", (bool? completed = null,Priority? priority = null,DateTime? dueBefore = null) => {
    return Results.Ok(new { success = true, data = toDoServices.filteredDisplay(completed, priority, dueBefore), message = "Tasks Retrieved" });
});
// returns task with the matching id, informs if there is no Task with the ID
app.MapGet("/api/tasks/{id}", (ILogger<Program> logger,int id) =>
{
    ToDoItem displayId = toDoServices.GetItemById(id);
    List<string> Error = new List<string>();
    if (displayId == null)
    {
        Error.add("Could not find Anything with that ID");
    }
    if (Error.Count == 0)
    {
        return Results.Ok(new { success = true, data = toDoServices.GetItemById(id), message = "Task Found" });
    }
    else
    {
        return Results.Ok(new { success = false, errors = Error, message = "Operation Failed" });
    }
});


// create a new task
app.MapPost("/api/tasks", (ILogger<Program> logger,[FromBody] string Title, string? Description = "",Priority? ListPriority = null, DateTime? DueDate = null) =>
{
    List<string> Error = new List<string>();
    if (Description.Length >= 500)
    {
        Error.add("Length is too long");
        return Results.Ok(new { success = false, errors = Error, message = "Operation Failed" });
    }
    int id = toDoServices.AddItem(Title, Description, ListPriority ?? Priority.Medium, DueDate ?? DateTime.Now);
    if (Error.Count == 0)
    {
        return Results.Ok(new { success = true, data = toDoServices.GetItemById(id), message = "Task Created" });
    }
    else
    {
        return Results.Ok(new { success = false, errors = Error, message = "Operation Failed" });
    }
});
// update changes of a seleted task, must have the id
app.MapPut("/api/tasks/{id}", (ILogger<Program> logger,[FromBody] int id, string? title = null,string? Description = null,bool? completed = null,Priority? ListPriority = null,DateTime? DueDate = null) =>
{
    List<string> Error = new List<string>();
    if (Description.Length < 500)
    {
        Error.add("Description is too long");
    }
    if (Error.Count == 0)
    {
        bool result = toDoServices.UpdateTodo(id, title, Description, completed, ListPriority, DueDate);
        if (Error.Count == 0)
        {
            return Results.Ok(new { success = true, data = toDoServices.GetItemById(id), message = "Task Changed" });
        }
    }
    return Results.Ok(new { success = false, errors = Error, message = "Operation Failed" });
    
});

app.MapDelete("/api/tasks/{id}", (ILogger<Program> logger,int id) => {
    ToDoItems deletedTask = toDoServices.DeleteItem(id);
    if (deletedTask == null)
    {
        Error.add("This is not Task with that ID");
    }
    if (Error.Count == 0)
    {
        return Results.Ok(new { success = true, data = deletedTask, message = "Task Deleted" });
    }
    
    return Results.Ok(new { success = false, errors = Error, message = "Operation Failed" });
});


app.UseHttpsRedirection();


app.Run();


public partial class Program {};