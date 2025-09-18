using Serilog; // logging library
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using todoapi.API.model;
using todoapi.API.service;
using todoapi.API.numnum;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

// builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
// .AddJwtBearer(jwtOptions =>
// {
// 	jwtOptions.Authority = "https://{--your-authority--}";
// 	jwtOptions.Audience = "https://{--your-audience--}";
// });


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
app.MapGet("/api/tasks", (string? title, string? description,bool? completed = null,Priority? priority = null,DateTime? dueBefore = null, int page = 0, int pageSize = 0, SortedBy? sorting = null) => {
    List<string> Error = new List<string>(); 
    try // try the call to filteredDisplay
    {
        return Results.Ok(new { success = true, data = toDoServices.filteredDisplay(title, description, completed, priority, dueBefore, page, pageSize, sorting ?? SortedBy.none), message = "Tasks Retrieved" });
    }
    catch (Exception e) // catches errors that happen in the FilterDisplay class and sends the error to the client
    {
        Error.Add(e.Message.ToString());
        return Results.Ok(new { success = false, errors = Error, message = "Operation Failed" }); // sending a error within the filter
    }
});
// returns task with the matching id, informs if there is no Task with the ID
app.MapGet("/api/tasks/{id}", (ILogger<Program> logger,int id) =>
{
    ToDoItem displayId = toDoServices.GetItemById(id); 
    List<string> Error = new List<string>();
    if (displayId == null)// if the id shows nothing after being called
    {
        Error.Add("Could not find Anything with that ID");
    }
    if (Error.Count == 0)
    {

        return Results.Ok(new { success = true, data = toDoServices.GetItemById(id), message = "Task Found" }); // the Task was found
    }
    else
    {
        return Results.Ok(new { success = false, errors = Error, message = "Operation Failed" }); // if there is a error
    }
});


// create a new task
app.MapPost("/api/tasks", (ILogger<Program> logger,[FromBody] string Title, string? Description = "",Priority? ListPriority = null, DateTime? DueDate = null) =>
{
    List<string> Error = new List<string>();
    if (Description.Length >= 500) // Description can not be over 500 characters
    {
        Error.Add("Length is too long");
        return Results.Ok(new { success = false, errors = Error, message = "Operation Failed" });
    }
    int id = toDoServices.AddItem(Title, Description, ListPriority ?? Priority.Medium, DueDate ?? DateTime.Now); // Send to Add Item, default values for DueDate and ListPriority
    if (Error.Count == 0) 
    {
        return Results.Ok(new { success = true, data = toDoServices.GetItemById(id), message = "Task Created" }); //Created a Task, send the created task to the client
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
    if (Description.Length < 500) // Description can not be over 500 characters
    {
        Error.Add("Description is too long");
    }
    if (Error.Count == 0)
    {
        bool result = toDoServices.UpdateTodo(id, title, Description, completed, ListPriority, DueDate); // update the Task, bool checks if its correct
        if (result == false) // if result was not found
        {
            Error.Add("Could not find Task with the corresponding id");
        }
        if (Error.Count == 0)
            {
                return Results.Ok(new { success = true, data = toDoServices.GetItemById(id), message = "Task Changed" });
            }
    }
    return Results.Ok(new { success = false, errors = Error, message = "Operation Failed" });
    
});
// Delete a Task with the given id
app.MapDelete("/api/tasks/{id}", (ILogger<Program> logger,int id) => {
    ToDoItem deletedTask = toDoServices.DeleteItem(id); // deleted task is sent back, if deleted
    List<string> Error = new List<string>();
    if (deletedTask == null) // if deleted task is not found, deletedTask will be null
    {
        Error.Add("This is not Task with that ID");
    }
    if (Error.Count == 0)
    {
        return Results.Ok(new { success = true, data = deletedTask, message = "Task Deleted" }); // comfirm deleted task
    }
    
    return Results.Ok(new { success = false, errors = Error, message = "Operation Failed" });
});


app.UseHttpsRedirection();

//app.UseAuthorization();

app.Run();


public partial class Program {};