using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using LongTask.Engine.Common;
using Serilog;
using Serilog.Core;
using MarkdownLog;
namespace Tasks.Engine
{
    public class LongRunningTask
    {
        static string currentPath = AppDomain.CurrentDomain.BaseDirectory+ $"App_Data\\Log-{DateTime.Now.ToString("dd-MMM-yyyy")}.log";
        ILogger logger;
        public Task Process(CancellationToken token)
        {
            var d= Path.GetPathRoot(currentPath);
            logger = new LoggerConfiguration()
                         .MinimumLevel.Information()
                         .WriteTo.Console()
                         .WriteTo.File($"{currentPath}",rollingInterval: RollingInterval.Minute,
                         restrictedToMinimumLevel:Serilog.Events.LogEventLevel.Information,fileSizeLimitBytes:4096)
                         .CreateLogger();
            logger.Information("Start log", "LongRunningTask");
            logger.Information("=================================");
            Console.WriteLine("The Origin of the Species".ToMarkdownHeader());

            int i = 0;
            Task task = Task.Run(() => {
            while (!token.IsCancellationRequested) {
                    int worker = 0,io = 0;
                    ThreadPool.GetAvailableThreads(out worker, out io);
                    Functions.StartProgress($"ThreadID {Thread.CurrentThread.ManagedThreadId}  Worker threads: {worker:N0}  I/O threads: {io:N0}", i);
                    var log = new []{ new { SerialNo = i, ThreadID = Thread.CurrentThread.ManagedThreadId, Workers = worker, IOWorker = io, Time = DateTime.Now.ToString("ddMMMyy") } };
                    //Console.WriteLine(log.ToMarkdownTable());
                    //logger.Write(Serilog.Events.LogEventLevel.Information, "LongRunningTask", log.ToPropertyValues<object>());
                    logger.Information("LongRunningTask", log.ToMarkdownTable());
                    //logger.Information($"\n{i} ThreadID {Thread.CurrentThread.ManagedThreadId}  Worker threads: {worker:N0}  I/O threads: {io:N0}", "LongRunningTask");
                    Thread.Sleep(2000);i++;if (i == 100) i = 0;
                }
                Cancel(token);}, token);
            return task;
        }
        private void Cancel(CancellationToken token)
        {
            logger.Information($"Cancellation Requested: {token.IsCancellationRequested}");
            token.ThrowIfCancellationRequested();
        }
    }
}
