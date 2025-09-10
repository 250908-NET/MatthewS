using System.ComponentModel;
using System.Data;
using System.Numerics;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly",null, "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

// http get Request
app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");
/*
    Challenge 1:
    basic operations
*/
app.MapGet("/calculator/add/{a}/{b}", (int a,int b) =>
{
    return new
    {
        operation = "add",
        result = a + b
    };
    
});
app.MapGet("/calculator/subtract/{a}/{b}", (int a, int b) =>
{
    return new
    {
        operation = "subtract",
        result = a - b
    };

});
app.MapGet("/calculator/multiply/{a}/{b}", (int a, int b) =>
{
    return new
    {
        operation = "multiply",
        result = a * b
    };

});
app.MapGet("/calculator/divide/{a}/{b}", (int a, int b) =>
{
    try
    {
        return Results.Ok(new
        {
            operation = "divide",
            result = a / b
        });
    }
    catch (DivideByZeroException)
    {
        return Results.BadRequest(new
        {
            Status = false,
            Message = "Error: DivideByZeroExecption"
        });
    }

});
/*
Challenge 2: String Manipulator
Work with String Methods and Transformations
*/
app.MapGet("/text/reverse/{text}", (string text) =>
{
    char[] reverseText = text.ToCharArray();
    Array.Reverse(reverseText);
    return new string(reverseText);
});
app.MapGet("/text/uppercase/{text}", (string text) =>
{
    return text.ToUpper();
});
app.MapGet("/text/lowercase/{text}", (string text) =>
{
    return text.ToLower();
});
app.MapGet("/text/count/{text}", (string text) =>
{
    return text.Length;
});
app.MapGet("/text/palindrome/{text}", (string text) =>
{
    bool isPalindrome = true;
    for (int i = 0; i < text.Length; i++)
    {
        if (text[i] != text[text.Length - i - 1])
        {
            isPalindrome = false;
        }
    }
    if (isPalindrome == true)
    {
        return Results.Ok(new
        {
            isPalindrome = "Yes"
        });
    }
    else
    {
        return Results.Ok(new
        {
            isPalindrome = "No"
        });
    }
});
/*
Challenge 3: Loops, Conditionals, and Operations
*/
app.MapGet("/numbers/fizzbuzz/{count}", (int count) =>
{
    List<String> fizzbuzzArray = new List<String>();
    fizzbuzzArray.Add("0");
    for (int i = 1; i < count; i++)
    {
        if (i % 3 == 0 && i % 5 == 0)
        {
            fizzbuzzArray.Add("fizzbuzz");
        }
        else if (i % 3 == 0)
        {
            fizzbuzzArray.Add("fizz");
        }
        else if (i % 5 == 0)
        {
            fizzbuzzArray.Add("buzz");
        }
        else
        {
            fizzbuzzArray.Add(i.ToString());
        }
    }

    return fizzbuzzArray;
});
app.MapGet("/numbers/prime/{count}", (int count) =>
{
    if (count == 0)
    {
        return Results.Ok(false);
    }
    if (count == 1 || count == 2)
    {
        return Results.Ok(true);
    }
    for (int i = 2; i < count; i++)
    {
        if (count % i == 0)
        {
            return Results.Ok(false);
        }
    }
    return Results.Ok(true);
});
app.MapGet("/numbers/fibonacci/{count}", (int count) =>
{
    List<int> fibonacciArray = new List<int>();
    int prevNum = 0;
    int sum = 0;
    int temp = 0;
    for (int i = 0; i < count; i++)
    {
        if (i < 2)
        {
            fibonacciArray.Add(i);
            sum = i;
            prevNum = i;
        }
        else
        {
            temp = sum;
            sum = prevNum + sum;
            prevNum = temp;
            fibonacciArray.Add(sum);
        }
    }
    return fibonacciArray;
});
app.MapGet("/numbers/factors/{count}", (int count) =>
{
    List<int> factors = new List<int>();

    for (int i = 1; i <= count - 1; i++)
    {
        if (count % i == 0)
        {
            factors.Add(i);
        }
    }
    return factors;

});
/*
Challenge 4: Date and Time Fun
*/
app.MapGet("/date/today", () =>
{
    DateTime currentDate = DateTime.Now;
    return $"{currentDate.ToString("MM-dd-yyyy")}";
});
app.MapGet("/date/age/{birthYear}", (int birthYear) =>
{
    DateTime currentDate = DateTime.Now;
    int currentYear = int.Parse(currentDate.ToString("yyyy"));

    return $"It seems like you are {currentYear - birthYear} years old!";
});
app.MapGet("/date/daysbetween/{date1}/{date2}", (string date1, string date2) =>
{
    // format will be MM-dd-yyyy, split will convert this to [MM,dd,yyyy]
    string[] SplitDate1 = date1.Split("-");
    string[] SplitDate2 = date2.Split("-");
    // convert the dates to a DateTime data type, allows use of timeSpan Data type. 
    DateTime startDate = new DateTime(int.Parse(SplitDate1[2]), int.Parse(SplitDate1[0]), int.Parse(SplitDate1[1]));
    DateTime endDate = new DateTime(int.Parse(SplitDate2[2]), int.Parse(SplitDate2[0]), int.Parse(SplitDate2[1]));
    // time span will have the calculated time diffence, this is done because months dont always have the same number of days every year and is beyond simple calculation
    TimeSpan difference = startDate - endDate;

    double dayDifference = Math.Abs(difference.TotalDays);

    return $"The total number of days between {date1} and {date2} is {(int)dayDifference}";

});
app.MapGet("/date/weekday/{date}", (string date) =>
{
    string[] SplitDate = date.Split("-");

    DateTime DateVar = new DateTime(int.Parse(SplitDate[2]), int.Parse(SplitDate[0]), int.Parse(SplitDate[1]));

    string[] days_of_the_week = ["Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday"];

    return $"today is a {days_of_the_week[(int)DateVar.DayOfWeek - 1]}";
});
/*
Challenge 5
*/

app.MapGet("/", () =>
{
    return "Hello World";
});


app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary = "NOTHING")
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
