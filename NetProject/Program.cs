using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Numerics;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Transactions;
using Microsoft.OpenApi.Writers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();
List<string> FavColors = new List<string> {"green", "black", "grey", "purple", "yellow"};

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
app.MapGet("/colors", () =>
{
    return FavColors;
});
// return a random color
app.MapGet("/colors/random", () =>
{
    return FavColors[Random.Shared.Next(FavColors.Count)]; // using Random to always print a different color
});
// return color starting with that letter
app.MapGet("/colors/search/{letter}", (string letter) =>
{
    List<string> colors = new List<string>();
    if (letter.Length > 1)
    {
        return Results.Ok("enter only one letter");
    }
    for (int i = 0; i < FavColors.Count; i++)
    {
        if (FavColors[i].IndexOf(letter) == 0)
        {
            colors.Add(FavColors[i]);
        }
    }
    return Results.Ok(colors);
});
// Post a new color
app.MapGet("/colors/add/{color}", (string color) =>
{
    FavColors.Add(color);
    return Results.Ok(new { message = "Color added", FavColors });
});
//Challenge 6: Temperature Converter
static double FToC(double temp)
{
    return ((temp - 32) * (5 / 9));
}
static double CToF(double temp)
{
    return ((temp * (9 / 5)) + 32);
}
static double KToC(double temp)
{
    return (temp - 273.15);
}
static double CToK(double temp)
{
    return (temp + 273.15);
}
// Convert celcius to fahrenheit
app.MapGet("/temp/celsius-to-fahrenheit/{temp}", (double temp) =>
{
    return CToF(temp);
});
// Convert fahrenheit to celcius
app.MapGet("/temp/fahrenheit-to-celsius/{temp}", (double temp) =>
{
    return FToC(temp);
});
// Convert kelvin to celsius
app.MapGet("/temp/kelvin-to-celsius/{temp}", (double temp) =>
{
    return KToC(temp);
});
// Convert celsius to kelvin
app.MapGet("/temp/celsius-to-kelvin/{temp}", (double temp) =>
{
    return CToK(temp);
});
app.MapGet("/temp/compare/{temp1}/{unit1}/{temp2}/{unit2}", (double temp1, string unit1, double temp2, string unit2) =>
{
    if (unit1 != "fahrenheit" && unit1 != "celsius" && unit1 != "kelvin" && unit2 != "fahrenheit" && unit2 != "celsius" && unit2 != "kelvin")
    {
        return Results.Ok(new { Error = $"{unit1} and {unit2} is not the correct units, enter fahrenheit, celsius, and kelvin" });
    }
    if (unit1 != "fahrenheit" && unit1 != "celsius" && unit1 != "kelvin")
    {
        return Results.Ok(new { Error = $"{unit1} is not the correct units, enter fahrenheit, celsius, and kelvin" });
    }
    if (unit2 != "fahrenheit" && unit2 != "celsius" && unit2 != "kelvin")
    {
        return Results.Ok(new { Error = $"{unit2} is not the correct units, enter fahrenheit, celsius, and kelvin" });
    }
    double Convert1 = temp1;
    double Convert2 = temp2;

    if (unit1 == "fahrenheit")
    {
        Convert1 = FToC(Convert1);
    }
    if (unit1 == "kelvin")
    {
        Convert1 = KToC(Convert1);
    }
    if (unit2 == "fahrenheit")
    {
        Convert2 = FToC(Convert2);
    }
    if (unit2 == "kelvin")
    {
        Convert2 = KToC(Convert2);
    }

    if (Convert1 > Convert2)
    {
        return Results.Ok(new { Message = $"{unit1} is higher than {unit2}" });
    }
    else if (Convert1 < Convert2)
    {
        return Results.Ok(new { Message = $"{unit1} is less than {unit2}" });
    }
    else
    {
        return Results.Ok(new { Message = $"{unit1} is equal to {unit2}" });
    }
});
//Challenge 7: Password Generator
// List of characters, up and low, and number in the ones
List<char> randomLetNumber = Enumerable.Range(0, 26).Select(x => (char)('a' + x)).ToList();
randomLetNumber.AddRange(Enumerable.Range(0, 26).Select(x => (char)('A' + x)).ToList());
randomLetNumber.AddRange(Enumerable.Range(0, 10).Select(x => (char)('0' + x)).ToList());
// generate a simple password with just letters and numbers
app.MapGet("/password/simple/{length}", (int length) =>
{
    Random random = new Random();
    string password = "";
    for (int i = 0; i < length; i++)
    {
        password += randomLetNumber[random.Next(randomLetNumber.Count)];
    }
    return Results.Ok(new { Message = $"your password is {password}" });

});
// generate a complex password which will include special characters
List<char> randomLetNumSpec = new List<char>(randomLetNumber);
randomLetNumSpec.AddRange(new char[] { '!', '@', '#', '$', '%', '^', '&', '*', '(', ')', '-', '_', '=', '+', '[', ']', '{', '}', ';', ':', ',', '.', '<', '>', '/', '?' });
app.MapGet("/password/complex/{length}", (int length) =>
{
    Random random = new Random();
    string password = "";
    for (int i = 0; i < length; i++)
    {
        password += randomLetNumSpec[random.Next(randomLetNumSpec.Count)];
    }
    return Results.Ok(new { Message = $"your password is {password}" });

});
List<string> wordList = new List<string> { "apple", "banana", "cherry", "date", "elderberry", "fig", "grape", "honeydew", "kiwi", "lemon", "mango", "nectarine", "orange", "papaya", "quince", "raspberry", "strawberry", "tangerine", "ugli", "vanilla", "watermelon" };
// generate a memorable password with just words
app.MapGet("/password/memorable/{words}", (int words) =>
{
    Random random = new Random();
    string phrase = "";
    for (int i = 0; i < words; i++)
    {
        if (i == words - 1)
        {
            phrase += wordList[random.Next(wordList.Count)];
        }
        else
        {
            phrase += wordList[random.Next(wordList.Count)] + " ";
        }
    }
    return Results.Ok(new { Message = $"{phrase}" });
});
// rates password stregnth
List<char> bigNum = Enumerable.Range(0, 26).Select(x => (char)('A' + x)).ToList();
List<char> specialChar = new List<char>();
specialChar.AddRange(new char[] { '!', '@', '#', '$', '%', '^', '&', '*', '(', ')', '-', '_', '=', '+', '[', ']', '{', '}', ';', ':', ',', '.', '<', '>', '/', '?' });
app.MapGet("/password/strength/{password}", (string password) =>
{
    double entropy = 0;
    double passStrength = 0;
    if (password.Any(c => char.IsUpper(c)) && password.Any(c => specialChar.Contains(c)))
    {
        passStrength += 62;
    }
    else
    {
        passStrength += 26;
    }

    entropy = password.Length * Math.Log(passStrength);
    if (entropy < 40)
    {
        return Results.Ok(new { Message = $"your password is weak" });
    }
    else if (entropy < 60)
    {
        return Results.Ok(new { Message = $"your password is moderate" });
    }
    else if (entropy < 80)
    {
        return Results.Ok(new { Message = $"your password is Strong" });
    }
    else if (entropy < 100)
    {
        return Results.Ok(new { Message = $"your password is very strong" });
    }
    else
    {
        return Results.Ok(new { Message = $"your password is unbreakable" });
    }

});
// Challenge 8: Simple Validator
List<String> DNE = new List<String>([".com", ".net", ".org", ".me", ".name", ".email", ".xyz", ".online", ".tech", ".info", ".io", ".co",".us",".ca",".de"]);
// basic email format validation
app.MapGet("/validate/email/{email}", (string email) =>
{
    if (email.Contains('@') && DNE.Any(dne => email.EndsWith(dne)))
    {
        return Results.Ok(new { Message = $"Valid email" });
    }
    else
    {
        return Results.Ok(new { Message = $"Invalid email" });
    }
});
// phone number format
List<char> bigSmallSpecNumber = Enumerable.Range(0, 26).Select(x => (char)('a' + x)).ToList();
bigSmallSpecNumber.AddRange(Enumerable.Range(0, 26).Select(x => (char)('A' + x)).ToList());
bigSmallSpecNumber.AddRange(new char[] { '!', '@', '#', '$', '%', '^', '&', '*', '(', ')', '-', '_', '=', '+', '[', ']', '{', '}', ';', ':', ',', '.', '<', '>', '/', '?' });
string phonePattern = @"^\d{3}-\d{3}-\d{4}$";
app.MapGet("/validate/phone/{phone}", (string phone) =>
{
    if (Regex.IsMatch(phone, phonePattern) && !phone.Any(c => bigSmallSpecNumber.Any(t => (t == c) && (c != '-'))))
    {
        return Results.Ok(new { Message = $"Valid phone number" });
    }
    else
    {
        return Results.Ok(new { Message = $"Invalid phone number" });
    }
});
app.MapGet("/validate/creditcard/{number}", (int number) =>
{

});
app.MapGet("/", () =>
{
    return "Hello World";
});


app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary = "NOTHING")
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
