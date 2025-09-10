using ConApp.model;
namespace ConApp.service;

public interface IToDoServices
{
    ToDoItem GetItemById(int id);
    void AddItem(int id, string title);
    bool MarkItemAsCompleted(int id);
    bool MarkItemAsNotCompleted(int id);
    bool DeleteItem(int id);
}