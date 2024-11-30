using Thuai.Server.Utility;

namespace Thuai.Server.Test.UnitTest;

public class TaskManagerTests
{
    private readonly StringWriter _consoleOutput = new();

    public TaskManagerTests()
    {
        Tools.LogHandler.Initialize(
            new Config.LogSettings()
            {
                Target = Config.LogSettings.LogTarget.Console,
                MinimumLevel = Config.LogSettings.LogLevel.Verbose,
            }
        );
        Console.SetOut(_consoleOutput);
    }

    [Theory]
    [ClassData(typeof(TestParameters<DataGroup<TestActionData, TestActionWithExceptionData>, TestStringData>))]
    public void CreateTask_ShouldCreateTask(Action action, string description)
    {
        // Act
        Task task = Tools.TaskManager.CreateTask(action, description);

        // Assert
        Assert.NotNull(task);
        Assert.Equal(TaskStatus.Created, task.Status);

        var consoleOutput = _consoleOutput.ToString();
        Assert.Contains("Task created", consoleOutput);
        if (description != "")
        {
            if (description.Length > 256)
            {
                Assert.Contains(description[..256], consoleOutput);
            }
            else
            {
                Assert.Contains(description, consoleOutput);
            }
        }
    }

    [Theory]
    [ClassData(typeof(TestActionData))]
    public void CreateTask_ShouldExecuteAction(Action action)
    {
        // Arrange
        bool actionCompleted = false;
        Action expectedAction = () =>
        {
            action();
            actionCompleted = true;
        };
        string description = "Test task";

        // Act
        Task task = Tools.TaskManager.CreateTask(expectedAction, description);
        task.Start();
        task.Wait();

        // Assert
        Assert.True(actionCompleted);
    }

    [Theory]
    [ClassData(typeof(TestActionWithExceptionData))]
    public void CreateTask_ShouldLogException(Action action)
    {
        // Arrange
        string description = "Test task";

        // Act
        Task task = Tools.TaskManager.CreateTask(action, description);
        task.Start();
        try
        {
            task.Wait();
        }
        catch (AggregateException)
        {
            // Ignore the exception
        }

        // Assert
        var consoleOutput = _consoleOutput.ToString();
        Assert.Contains("Task crashed", consoleOutput);
    }

    [Theory]
    [ClassData(typeof(TestActionData))]
    public void CreateTask_ShouldCancelTask(Action action)
    {
        // Arrange
        string description = "Test task";
        Action<CancellationToken> expectedAction = (token) =>
        {
            while (!token.IsCancellationRequested)
            {
                action();
            }
        };

        // Act
        TaskWithCancellation taskWithCancellation = Tools.TaskManager.CreateTaskWithCancellation(
            expectedAction, description
        );

        taskWithCancellation.Start();
        Task.Delay(100).Wait();
        taskWithCancellation.Cancel();
        Task.Delay(100).Wait();

        // Assert
        Assert.Contains("Task cancelled", _consoleOutput.ToString());
    }
}
