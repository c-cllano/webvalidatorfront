namespace Process.Domain.Utilities
{
    public static class ThreadExecutionHelper
    {
        public static void ThreadExecution(Func<Task> task)
        {
            _ = Task.Run(async () =>
            {
                try
                {
                    await task();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error ejecutando el hilo para guardar los logs: {ex.Message}");
                }
            });
        }
    }
}
