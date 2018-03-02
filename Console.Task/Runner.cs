using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Engine;

namespace Consoles.Tasks
{
    static class Runner
    {
        static Task t;
        internal static void Run(CancellationTokenSource cts)
        {
            try
            {
                var token = cts.Token;
                var longTask = new LongRunningTask();
                t = Task.Factory.StartNew(() => { longTask.Process(token); }, token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
                t.Wait();
            }
            catch (AggregateException ae)
            {
                ae.Flatten().Handle(ex => {
                    if (ex is Exception)
                    {
                        Task.Run(() => {
                            Console.WriteLine(ex.Message);
                        }); throw ex;
                    }
                    else
                    {
                        return false;
                    }
                });
            }
        }
    }
}
