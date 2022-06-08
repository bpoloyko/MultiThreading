/*
*  Create a Task and attach continuations to it according to the following criteria:
   a.    Continuation task should be executed regardless of the result of the parent task.
   b.    Continuation task should be executed when the parent task finished without success.
   c.    Continuation task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation
   d.    Continuation task should be executed outside of the thread pool when the parent task would be cancelled
   Demonstrate the work of the each case with console utility.
*/
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreading.Task6.Continuation
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Create a Task and attach continuations to it according to the following criteria:");
            Console.WriteLine("a.    Continuation task should be executed regardless of the result of the parent task.");
            Console.WriteLine("b.    Continuation task should be executed when the parent task finished without success.");
            Console.WriteLine("c.    Continuation task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation.");
            Console.WriteLine("d.    Continuation task should be executed outside of the thread pool when the parent task would be cancelled.");
            Console.WriteLine("Demonstrate the work of the each case with console utility.");
            Console.WriteLine();

            CaseA();

            CaseB();
            
            CaseC();

            CaseD();

            Console.ReadLine();
        }

        private static void CaseA()
        {
            Console.WriteLine("Case a:");
            var taskA = Task.Factory.StartNew(() => throw new Exception("Exception in parent")).ContinueWith(ant => Console.WriteLine("Continuation"));
            taskA.Wait();
        }

        private static void CaseB()
        {
            Console.WriteLine("Case b:");
            var taskB = Task.Factory.StartNew(() => throw new Exception("Exception in parent")).ContinueWith(ant => Console.WriteLine(ant.Exception.Message), TaskContinuationOptions.OnlyOnFaulted);
            taskB.Wait();
        }

        private static void CaseC()
        {
            Console.WriteLine("Case c:");
            var taskC = Task.Factory.StartNew(() =>
            {
                Console.WriteLine($"Antecedent thread: {Thread.CurrentThread.ManagedThreadId}");
                throw new Exception("Exception in parent");
            }).ContinueWith(ant =>
            {
                Console.WriteLine($"Continuation thread: {Thread.CurrentThread.ManagedThreadId}");
                Console.WriteLine(ant.Exception.Message);
            }, TaskContinuationOptions.OnlyOnFaulted | TaskContinuationOptions.ExecuteSynchronously);
            taskC.Wait();
        }

        private static void CaseD()
        {
            Console.WriteLine("Case d:");
            using (var cts = new CancellationTokenSource())
            {
                var token = cts.Token;

                var taskD = Task.Factory.StartNew(() =>
                {
                    Thread.Sleep(1000);
                    token.ThrowIfCancellationRequested();
                }, token).ContinueWith(ant =>
                {
                    if (!Thread.CurrentThread.IsThreadPoolThread)
                    {
                        Console.WriteLine("Continuation is not from thread pool");
                    }
                }, TaskContinuationOptions.OnlyOnCanceled | TaskContinuationOptions.LongRunning);
                cts.Cancel();
                try
                {
                    taskD.Wait(token);
                }
                catch (OperationCanceledException)
                {
                    Console.WriteLine("Antecedent was cancelled");
                }
            }
        }
    }
}
