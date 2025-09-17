﻿using System.Net;
using System.Text;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Newtonsoft.Json;
using FluentAssertions;
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Text.Json;

using todoapi.API.model;
using todoapi.API.service;
using todoapi.API.numnum;

namespace todoapi.Test;

public class DatabaseFixture : IDisposable
{
    public DatabaseFixture()
    {
        Db = new List<ToDoItem>();
        Console.WriteLine("Setting up test database...");

        Db.Add(new ToDoItem(1, "Task 1", "Description 1", Priority.Medium, DateTime.Now.AddDays(1)));
        Db.Add(new ToDoItem(2, "Task 2", "Description 2", Priority.High, DateTime.Now.AddDays(2)));
        Db.Add(new ToDoItem(3, "Task 3", "Description 3", Priority.Low, DateTime.Now.AddDays(3)));
        Db.Add(new ToDoItem(4, "Task 4", "Description 4", Priority.Critical, DateTime.Now.AddDays(4)));
        Db.Add(new ToDoItem(5, "Task 5", "Description 5", Priority.Medium, DateTime.Now.AddDays(5)));
        Db.Add(new ToDoItem(6, "Task 6", "Description 6", Priority.High, DateTime.Now.AddDays(6)));
        Db.Add(new ToDoItem(7, "Task 7", "Description 7", Priority.Low, DateTime.Now.AddDays(7)));
        Db.Add(new ToDoItem(8, "Task 8", "Description 8", Priority.Critical, DateTime.Now.AddDays(8)));
        Db.Add(new ToDoItem(9, "Task 9", "Description 9", Priority.Medium, DateTime.Now.AddDays(9)));
        // ... initialize data in the test database ...
    }

    public void Dispose()
    {
        Db.Clear();
        // ... clean up test data from the database ...
    }

    public List<ToDoItem> Db { get; set; }
}
public class ApiResponse
{
    public bool success { get; set; }
    public List<ToDoItem>? data { get; set; }
    public List<string>? errors { get; set; }
    public string? message { get; set; }
}
[CollectionDefinition("Api Database collection")]
public class DatabaseCollection : ICollectionFixture<DatabaseFixture>
{
    // This class has no code, and is never created. Its purpose is simply
    // to be the place to apply [CollectionDefinition] and all the
    // ICollectionFixture<> interfaces.
}
[Collection("Api Database collection")]
public class APItest : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly DatabaseFixture _fixture;
    private readonly HttpClient _client;

    public APItest(WebApplicationFactory<Program> factory, DatabaseFixture fixture)
    {
        _client = factory.CreateClient();
        _fixture = fixture;

    }
    
    public async Task InitializeTask()
    {
        foreach (var task in _fixture.Db)
        {
            var url = $"/api/tasks?Description={Uri.EscapeDataString(task.Description)}&ListPriority={(int)task.ListPriority}&DueDate={task.DueDate}";
            var postResponse = await _client.PostAsJsonAsync(url, task.Title);
        }
    }


    [Fact]
    public async Task DisplayTasks()
    {
        // Given

        var Tasks = _fixture.Db.Where(t => t.IsCompleted == false).ToList();
        foreach (var task in _fixture.Db)
        {
            var url = $"/api/tasks?Description={Uri.EscapeDataString(task.Description)}&ListPriority={(int)task.ListPriority}&DueDate={task.DueDate}";
            var postResponse = await _client.PostAsJsonAsync(url, task.Title);
            postResponse.EnsureSuccessStatusCode();
        }
        Console.WriteLine("Tasks Count: " + Tasks.Count);
        var response = await _client.GetAsync("/api/tasks");
        var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse>();

        // Then
        Console.WriteLine("Response: " + response);
        Console.WriteLine("Response Body: " + apiResponse.data);

        response.EnsureSuccessStatusCode(); // Status Code 200-299
        apiResponse.data.Should().BeEquivalentTo(Tasks, options => options.Excluding(t => t.Id).Excluding(t => t.CreatedAt).Excluding(t => t.UpdatedAt).Excluding(t => t.DueDate));
        for (int i = 0; i < apiResponse.data.Count; i++)
        {
            Assert.Equal(Tasks[i].DueDate, apiResponse.data[i].DueDate, TimeSpan.FromSeconds(1));
        }
        // Assert that Tasks are match the database


    }
    [Fact]
    public async Task PageTooHigh()
    {

        List<string> ErrorStatus = new List<string>();
        ErrorStatus.Add("Page Size times the number of pages are too high");
        /*
            if the db size is equal or greater than (page-1) * pageSize
            this means there is no enough task for the selected page to exist
        */
        var testResponse = await _client.GetAsync("/api/tasks");
        var testApiResponse = await testResponse.Content.ReadFromJsonAsync<ApiResponse>();
        var response = await _client.GetAsync("/api/tasks?page=4&pageSize=4");
        var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse>();

        response.EnsureSuccessStatusCode();
        apiResponse.errors.Should().BeEquivalentTo(ErrorStatus);
    }
    public Task DisposeAsync() => Task.CompletedTask;

}
