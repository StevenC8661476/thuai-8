using Serilog;

namespace Thuai.Server.Utility;

public class TaskWithCancellation
{
    private readonly Task _task;
    private readonly CancellationTokenSource _cancellationTokenSource;

    public TaskWithCancellation(Task task, CancellationTokenSource cancellationTokenSource)
    {
        _task = task;
        _cancellationTokenSource = cancellationTokenSource;
    }

    public void Start()
    {
        _task.Start();
    }

    public void Cancel()
    {
        _cancellationTokenSource.Cancel();
    }
}

public static partial class Tools
{

    /// <summary>
    /// A class for managing tasks.
    /// </summary>
    public static class TaskManager
    {
        private static int _taskId = 0;                 // The ID of the task.

        /// <summary>
        /// Creates a task.
        /// </summary>
        /// <param name="action">Action to be executed in the task.</param>
        /// <param name="description">Describes what the task does.</param>
        /// <returns></returns>
        public static Task CreateTask(Action action, string description = "")
        {
            _taskId++;

            ILogger logger = LogHandler.CreateLogger($"Task {_taskId}");
            logger.Debug(
                "Task created." + (description == "" ? "" : $" ({LogHandler.Truncate(description, 256)})")
            );

            return new Task(
                () =>
                {
                    try
                    {
                        action();
                    }
                    catch (Exception e)
                    {
                        logger.Error("Task crashed:");
                        LogHandler.LogException(logger, e);
                    }
                }
            );
        }

        public static TaskWithCancellation CreateTaskWithCancellation(
            Action<CancellationToken> action, string description = ""
        )
        {
            _taskId++;

            ILogger logger = LogHandler.CreateLogger($"Task {_taskId}");
            logger.Debug(
                "Task created." + (description == "" ? "" : $" ({LogHandler.Truncate(description, 256)})")
            );

            CancellationTokenSource cancellationTokenSource = new();
            return new TaskWithCancellation(
                new Task(
                    () =>
                    {
                        try
                        {
                            action(cancellationTokenSource.Token);
                            if (cancellationTokenSource.Token.IsCancellationRequested)
                            {
                                logger.Debug("Task cancelled.");
                            }
                        }
                        catch (Exception e)
                        {
                            logger.Error("Task crashed:");
                            LogHandler.LogException(logger, e);
                        }
                    },
                    cancellationTokenSource.Token
                ),
                cancellationTokenSource
            );
        }
    }
}
