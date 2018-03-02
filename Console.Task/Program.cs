using System;
using System.Threading;

namespace Consoles.Tasks
{
    class Program
    {
        static CancellationTokenSource cts;
        static void Main(string[] args)
        {

            Start();
            Console.ReadLine();
        }
        static void  Start()
        {
            Console.WriteLine("Press type [S] to start the task");
            if (Console.ReadLine().Trim().ToUpper() == "S")
            {
                cts = new CancellationTokenSource();
                Runner.Run(cts);
                Console.WriteLine("Press type [C] to cancel the task");
                if (Console.ReadLine().Trim().ToUpper() == "C")  cts.Cancel();
            }
        }

    }
}
