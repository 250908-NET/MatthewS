# C# Challenges for Minimal API Practice

## Challenge 1: Basic Calculator
**Goal**: Practice basic operations and parameter handling
- Create endpoint `/calculator/add/{a}/{b}` that returns sum of two numbers
- Add endpoints for subtract, multiply, and divide
- Handle division by zero with proper error messages
- Return results as JSON: `{"operation": "add", "result": 15}`

## Challenge 2: String Manipulator
**Goal**: Work with string methods and transformations
- Create `/text/reverse/{text}` - returns reversed string
- Add `/text/uppercase/{text}` and `/text/lowercase/{text}`
- Create `/text/count/{text}` - returns character count, word count, vowel count
- Add `/text/palindrome/{text}` - checks if text is a palindrome

## Challenge 3: Number Games
**Goal**: Practice loops, conditionals, and number operations
- Create `/numbers/fizzbuzz/{count}` - returns FizzBuzz sequence up to count
- Add `/numbers/prime/{number}` - checks if number is prime
- Create `/numbers/fibonacci/{count}` - returns first N Fibonacci numbers
- Add `/numbers/factors/{number}` - returns all factors of a number

## Challenge 4: Date and Time Fun
**Goal**: Work with DateTime and formatting
- Create `/date/today` - returns current date in different formats
- Add `/date/age/{birthYear}` - calculates age from birth year
- Create `/date/daysbetween/{date1}/{date2}` - calculates days between dates
- Add `/date/weekday/{date}` - returns day of week for given date

## Challenge 5: Simple Collections
**Goal**: Practice working with lists and basic LINQ
- Create `/colors` endpoint that returns a predefined list of favorite colors
- Add `/colors/random` - returns a random color from the list
- Create `/colors/search/{letter}` - returns colors starting with that letter
- Add `/colors/add/{color}` (POST) - adds new color to the list

## Challenge 6: Temperature Converter
**Goal**: Practice calculations and different data formats
- Create `/temp/celsius-to-fahrenheit/{temp}` 
- Add `/temp/fahrenheit-to-celsius/{temp}`
- Create `/temp/kelvin-to-celsius/{temp}` and reverse
- Add `/temp/compare/{temp1}/{unit1}/{temp2}/{unit2}` - compares temperatures

## Challenge 7: Password Generator
**Goal**: Work with random generation and string building
- Create `/password/simple/{length}` - generates random letters/numbers
- Add `/password/complex/{length}` - includes special characters
- Create `/password/memorable/{words}` - generates passphrase with N words
- Add `/password/strength/{password}` - rates password strength

## Challenge 8: Simple Validator
**Goal**: Practice validation logic and boolean operations
- Create `/validate/email/{email}` - basic email format validation
- Add `/validate/phone/{phone}` - validates phone number format
- Create `/validate/creditcard/{number}` - Luhn algorithm validation
- Add `/validate/strongpassword/{password}` - checks password rules

## Challenge 9: Unit Converter
**Goal**: Work with different measurement systems
- Create `/convert/length/{value}/{fromUnit}/{toUnit}` (meters, feet, inches)
- Add `/convert/weight/{value}/{fromUnit}/{toUnit}` (kg, lbs, ounces)
- Create `/convert/volume/{value}/{fromUnit}/{toUnit}` (liters, gallons, cups)
- Add `/convert/list-units/{type}` - returns available units for each type

## Challenge 10: Weather History
**Goal**: Add persistence and CRUD operations
- Create a simple in-memory list to store weather forecasts
- Add POST endpoint to save a weather forecast
- Modify GET to return saved forecasts instead of random ones
- Add DELETE endpoint to remove forecasts by date

## Challenge 11: Simple Games
**Goal**: Combine multiple concepts in mini-games
- Create `/game/guess-number` (POST) - number guessing game with session
- Add `/game/rock-paper-scissors/{choice}` - play against computer
- Create `/game/dice/{sides}/{count}` - roll N dice with X sides
- Add `/game/coin-flip/{count}` - flip coins and return results

## Sample Implementation Pattern

Each challenge should follow this basic structure:

```csharp
// Example for Challenge 1
app.MapGet("/calculator/add/{a}/{b}", (double a, double b) => 
{
    var result = a + b;
    return new { operation = "add", input1 = a, input2 = b, result = result };
});

app.MapGet("/calculator/divide/{a}/{b}", (double a, double b) => 
{
    if (b == 0)
        return Results.BadRequest(new { error = "Cannot divide by zero" });
    
    var result = a / b;
    return Results.Ok(new { operation = "divide", input1 = a, input2 = b, result = result });
});
```

## Challenge Progression

**Challenges 1-3**: Basic C# syntax, operations, and control flow
**Challenges 4-6**: Working with built-in types and classes
**Challenges 7-9**: String manipulation and data validation
**Challenge 10/11**: Combining concepts and adding simple state