using Serilog; // logging library
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Serialization;
using todoapi.API.model;
using todoapi.API.service;
using todoapi.API.numnum;
using todoapi.API.users;


List<UserInfo> Users = new List<UserInfo>();
ToDoServices toDoServices = new ToDoServices();
var tasks = new (string Title, string Description, Priority ListPriority, DateTime DueDate)[]
{
    ("Finish Report", "Complete the quarterly financial report", Priority.Critical, DateTime.Now.AddDays(2)),
    ("Buy Groceries", "Milk, Eggs, Bread, and Coffee", Priority.Medium, DateTime.Now.AddDays(1)),
    ("Workout", "Go to the gym for cardio and weights", Priority.Low, DateTime.Now.AddDays(3)),
    ("Team Meeting", "Weekly sync with project team", Priority.High, DateTime.Now.AddHours(6)),
    ("Doctor Appointment", "Annual physical checkup", Priority.Medium, DateTime.Now.AddDays(7)),
    ("Clean House", "Vacuum, dust, and mop the floors", Priority.Low, DateTime.Now.AddDays(5)),
    ("Pay Bills", "Electricity and Internet bills", Priority.Critical, DateTime.Now.AddDays(4)),
    ("Call Mom", "Check in and say hi", Priority.Low, DateTime.Now.AddDays(2)),
    ("Prepare Presentation", "Slides for client meeting", Priority.High, DateTime.Now.AddDays(1)),
    ("Study C#", "Review generics and async/await", Priority.Medium, DateTime.Now.AddDays(10)),
    ("Update Resume", "Add recent projects and skills", Priority.High, DateTime.Now.AddDays(6)),
    ("Car Maintenance", "Oil change and tire rotation", Priority.Medium, DateTime.Now.AddDays(8)),
    ("Book Flight", "Reserve tickets for vacation", Priority.Critical, DateTime.Now.AddDays(15)),
    ("Read Book", "Finish reading 'Clean Code'", Priority.Low, DateTime.Now.AddDays(20)),
    ("Write Blog Post", "Draft article about APIs", Priority.Medium, DateTime.Now.AddDays(9)),
    ("Fix Bug #42", "Null reference issue in login module", Priority.Critical, DateTime.Now.AddDays(1)),
    ("Plan Birthday", "Organize surprise party", Priority.High, DateTime.Now.AddDays(12)),
    ("Laundry", "Wash and fold clothes", Priority.Low, DateTime.Now.AddDays(4)),
    ("Refactor Code", "Improve readability of ToDoServices", Priority.Medium, DateTime.Now.AddDays(11)),
    ("Learn SQL", "Practice advanced queries", Priority.Low, DateTime.Now.AddDays(14))
};

// loop to insert into service
foreach (var task in tasks)
{
    toDoServices.AddItem(task.Title, task.Description, task.ListPriority, task.DueDate);
}


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
var jwtConfig = builder.Configuration.GetSection("JwtSettings").Get<JwtConfiguration>();
if(jwtConfig == null)
{
    throw new InvalidOperationException("JwtSettings section is missing in appsettings.json!");
}
builder.Services.AddSingleton(jwtConfig);

builder.Services.AddScoped<IdentityService>();

builder.Services.AddAuthorization();
builder.Services.AddJwtAuthentication(jwtConfig);
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "My API", Version = "v1" });



    // 🔑 Add JWT Bearer Auth
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Enter 'Bearer' [space] and then your valid token.\n\nExample: **Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6...**"
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });

});
    builder.Services.ConfigureHttpJsonOptions(options =>
    {
        options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
    builder.Services.Configure<JsonOptions>(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });



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





app.MapPost("/login", async (LoginRequest request,HttpContext httpContext ,IdentityService identityService, IConfiguration config, ILogger<Program> logger) =>
{
    // Retrieve admin credentials from configuration (Optional for flexibility)
    var adminUsername = config["Auth:AdminUsername"] ?? "admin";
    var adminPassword = config["Auth:AdminPassword"] ?? "password";

    // Validate user credentials
    if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
    {
        logger.LogWarning("Login failed: Empty username or password.");
        return Results.BadRequest(new { message = "Username and password are required." });
    }
    var userIsAuthenticated = ((request.Username == adminUsername && request.Password == adminPassword) || (Users.Any(o => o.UserName == request.Username && o.Password == HashPassword(request.Password))));

    if (!userIsAuthenticated)
    {
        logger.LogWarning("Login failed for user: {Username}", request.Username);
        return Results.Unauthorized();
    }

    // Generate JWT token
    var token = await identityService.GenerateToken(Users.Find(o => o.UserName == request.Username));
    
    logger.LogInformation("User {Username} authenticated successfully.", request.Username);

    return Results.Ok(new
    {
        message = "Login successful",
        token
    });
})
.AllowAnonymous();
static string HashPassword(string password)
{
    using (var sha256 = SHA256.Create())
    {
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
    }
}
UserInfo AdminUser = new UserInfo(Users.Count,"admin",HashPassword("password"));
Users.Add(AdminUser);
app.MapPost("/api/register", (string username, string password) =>
{
    
    List<string> Error = new List<string>();

    // Check if username already exists
    if (Users.Any(u => u.UserName == username))
    {
        Error.Add("Username already exists");
        return Results.Ok(new { success = false, errors = Error, message = "Operation Failed" });
    }

    // Hash password before storing
    string hashedPassword = HashPassword(password);

    UserInfo newUser = new UserInfo(Users.Count,username,hashedPassword);
    Users.Add(newUser);

    return Results.Ok(new { success = true, data = newUser, message = "User Registered" });
});

// desplaying all inputs
app.MapGet("/api/tasks", (ILogger<Program> logger,HttpContext httpContext,string? title, string? description,bool? completed = null,Priority? priority = null,DateTime? dueBefore = null, int page = 0, int pageSize = 0, SortedBy? sorting = null) => {
    //var userIdClaim = httpContext
    //if (userIdClaim == null)
        //Console.WriteLine();
        //return Results.Unauthorized();
    List<string> Error = new List<string>(); 
    try // try the call to filteredDisplay
    {
        return Results.Ok(new { success = true, data = toDoServices.filteredDisplay(title, description, completed, priority, dueBefore, page, pageSize, sorting ?? SortedBy.none), message = page == 0  ? "Tasks Retrieved" : $"Tasks Retrieved. Page: {page}" });
    }
    catch (Exception e) // catches errors that happen in the FilterDisplay class and sends the error to the client
    {
        Error.Add(e.Message.ToString());
        return Results.Ok(new { success = false, errors = Error, message = "Operation Failed" }); // sending a error within the filter
    }
}).RequireAuthorization();
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
}).RequireAuthorization();;


// create a new task
app.MapPost("/api/tasks", (ILogger<Program> logger, HttpContext httpContext, [FromBody] string Title, Priority? ListPriority = null, string? Description = "", DateTime? DueDate = null) =>
{
    // var userIdClaim = httpContext.User.FindFirst(JwtRegisteredClaimNames.Sub);
    // if (userIdClaim == null)
    //     return Results.Unauthorized();
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
}).RequireAuthorization();
// update changes of a seleted task, must have the id
app.MapPut("/api/tasks/{id}", (ILogger<Program> logger,[FromBody] int id, string? title = null,string? Description = "",bool? completed = null,Priority? ListPriority = null,DateTime? DueDate = null) =>
{
    List<string> Error = new List<string>();
    if (Description.Length > 500) // Description can not be over 500 characters
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
    
}).RequireAuthorization();
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
}).RequireAuthorization();


app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.Run();

public record JwtConfiguration
{
    public string Secret { get; set; } = string.Empty;
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public int ExpireDays { get; set; } = 7;
}
public record LoginRequest
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public partial class Program { };