# Simple To-Do Console App - Code Challenge Requirements

## Overview
Create a console application in C# (.NET) that allows users to manage a simple to-do list. The application should demonstrate basic object-oriented programming principles including the use of at least one interface or abstract class.

## Core Requirements

### 1. Basic Functionality
Your application must support the following operations:
- **Add** a new to-do item
- **List** all to-do items
- **Mark** items as complete/incomplete
- **Delete** a to-do item
- **Exit** the application

### 2. To-Do Item Properties
Each to-do item should have at minimum:
- **ID** (unique identifier)
- **Title** (description of the task)
- **IsCompleted** (completion status)
- **CreatedDate** (when the item was created)

### 3. User Interface
- Simple console menu with numbered options
- Clear instructions for the user
- Display current list of items with their status
- Handle invalid user input gracefully

### 4. Object-Oriented Design Requirements
**You must implement at least ONE of the following:**

**Option A: Interface**
- Create an `IToDoService` interface that defines methods for managing to-do items
- Implement this interface in a `ToDoService` class

**Option B: Abstract Class**
- Create an abstract `ToDoItemBase` class with common properties and at least one abstract method
- Create a concrete class that inherits from this abstract class

**Option C: Both** (for extra credit)

## Sample User Flow
```
=== TO-DO LIST MANAGER ===
1. Add new item
2. View all items
3. Mark item complete
4. Mark item incomplete
5. Delete item
6. Exit

Choose an option (1-6): 1

Enter task description: Buy groceries
Task added successfully! (ID: 1)

Choose an option (1-6): 2

=== YOUR TO-DO ITEMS ===
[1] Buy groceries (Created: 1/15/2024) - ⭕ Not Complete
```

## Technical Requirements

### Must Have:
- Console application
- Use collections to store to-do items (List, Dictionary, etc.)
- Implement at least one interface OR abstract class
- Proper error handling for user input
- Clean, readable code with meaningful variable names

### Code Organization:
- Separate classes for different responsibilities
- Don't put everything in `Program.cs`
- Use proper access modifiers (public, private, etc.)

## Suggested Project Structure
```
ToDoApp/
├── Program.cs                 // Main entry point and user interface
├── Models/
│   ├── ToDoItem.cs           // To-do item class
│   └── ToDoItemBase.cs       // Abstract class (if using Option B)
├── Services/
│   ├── IToDoService.cs       // Interface (if using Option A)
│   └── ToDoService.cs        // Implementation
└── README.md                 // Brief explanation of your design choices
```

## Bonus Features (Optional)
If you finish the basic requirements, consider adding:
- **Priority levels** (High, Medium, Low)
- **Due dates** for tasks
- **Categories** or tags for organizing tasks
- **Search/filter** functionality
- **Save/load** from file (JSON or text file)
- **Data validation** (empty titles, future dates, etc.)

### Documentation
- Brief README explaining your design decisions
- Comments on complex logic (don't over-comment obvious things)

## Getting Started Tips
1. Start with the basic `ToDoItem` class
2. Create a simple console menu that compiles and runs
3. Implement one feature at a time (start with "Add" and "List")
4. Add the interface/abstract class requirement after basic functionality works
5. Test frequently as you build

Good luck and have fun coding! Remember, the goal is to practice C# fundamentals and object-oriented design principles.