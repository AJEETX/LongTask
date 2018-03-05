using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Serilog;
using MarkdownLog;
namespace LongTask.Engine
{
    public class LongRunningTask
    {
        static string currentPath = AppDomain.CurrentDomain.BaseDirectory+ $"Data\\Log-{DateTime.Now.ToString("ddMMMyy-HH-mm-ss")}.log";
        ILogger logger;
        public Task Process(CancellationToken token)
        {
            logger = new LoggerConfiguration().MinimumLevel.Information() .WriteTo.Console() .WriteTo.File($"{currentPath}",
                rollingInterval: RollingInterval.Hour,restrictedToMinimumLevel:Serilog.Events.LogEventLevel.Information,
                fileSizeLimitBytes:4096000).CreateLogger();
            Console.WriteLine("The Start of the LongRunningTask".ToMarkdownHeader());
            int i = 0;
            Task task = Task.Run(() => {
            while (!token.IsCancellationRequested) {
                    int worker = 0,io = 0;
                    ThreadPool.GetAvailableThreads(out worker, out io);
                    Functions.StartProgress($"ThreadID {Thread.CurrentThread.ManagedThreadId}  Worker threads: {worker:N0}  I/O threads: {io:N0}", i);
                    var log = new []{ new { SerialNo = i+1, ThreadID = Thread.CurrentThread.ManagedThreadId, Workers = worker, IOWorker = io,
                        Time = DateTime.Now.ToString("ddMMMyy") } };
                    Console.WriteLine(log.ToMarkdownTable());
                    logger.Information($"\n{i} ThreadID {Thread.CurrentThread.ManagedThreadId}  Worker threads: {worker:N0}  I/O threads: {io:N0}", "LongRunningTask");
                    Thread.Sleep(2000);i++;if (i == 100) i = 0;
                } Cancel(token);}, token);
            return task;
        }
        private void Cancel(CancellationToken token)
        {
            logger.Information($"Cancellation Requested: {token.IsCancellationRequested}");
            token.ThrowIfCancellationRequested();
        }
    }
}
