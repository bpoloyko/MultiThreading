/*
 * 4.	Write a program which recursively creates 10 threads.
 * Each thread should be with the same body and receive a state with integer number, decrement it,
 * print and pass as a state into the newly created thread.
 * Use Thread class for this task and Join for waiting threads.
 * 
 * Implement all of the following options:
 * - a) Use Thread class for this task and Join for waiting threads.
 * - b) ThreadPool class for this task and Semaphore for waiting threads.
 */

using System;
using System.Threading;

namespace MultiThreading.Task4.Threads.Join
{
    class Program
    {
        private static readonly SemaphoreSlim Semaphore = new SemaphoreSlim(1);
        static void Main(string[] args)
        {
            Console.WriteLine("4.	Write a program which recursively creates 10 threads.");
            Console.WriteLine(
                "Each thread should be with the same body and receive a state with integer number, decrement it, print and pass as a state into the newly created thread.");
            Console.WriteLine("Implement all of the following options:");
            Console.WriteLine();
            Console.WriteLine("- a) Use Thread class for this task and Join for waiting threads.");
            Console.WriteLine("- b) ThreadPool class for this task and Semaphore for waiting threads.");

            Console.WriteLine();

            CreateThreadsJoin(10);
            CreateThreadsSemaphore(10);

            Console.ReadLine();
        }


        public static void CreateThreadsJoin(int number)
        {
            var thread = new Thread(state =>
            {
                if ((int) state == 0)
                {
                    return;
                }
                var i = (int)state - 1;
                Console.WriteLine(i);
                CreateThreadsJoin(i);
            });

            thread.Start(number);
            thread.Join();
        }

        public static void CreateThreadsSemaphore(int number)
        {
            Semaphore.Wait();
            ThreadPool.QueueUserWorkItem(state =>
            {
                if ((int)state == 0)
                {
                    return;
                }
                var i = (int)state - 1;
                Console.WriteLine(i);
                CreateThreadsSemaphore(i);
                
            }, number);
            Semaphore.Release();
        }
    }
}
