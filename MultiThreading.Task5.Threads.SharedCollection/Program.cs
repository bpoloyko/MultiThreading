/*
 * 5. Write a program which creates two threads and a shared collection:
 * the first one should add 10 elements into the collection and the second should print all elements
 * in the collection after each adding.
 * Use Thread, ThreadPool or Task classes for thread creation and any kind of synchronization constructions.
 */
using System;
using System.Collections.Generic;
using System.Threading;

namespace MultiThreading.Task5.Threads.SharedCollection
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("5. Write a program which creates two threads and a shared collection:");
            Console.WriteLine("the first one should add 10 elements into the collection and the second should print all elements in the collection after each adding.");
            Console.WriteLine("Use Thread, ThreadPool or Task classes for thread creation and any kind of synchronization constructions.");
            Console.WriteLine();

            var isCompleted = false;
            var waitHandleAdd = new AutoResetEvent(true);
            var waitHandlePrint = new AutoResetEvent(false);
            var collection = new List<int>();

            var threadAdd = new Thread(() =>
            {
                for (var i = 1; i <= 10; i++)
                {
                    waitHandleAdd.WaitOne();

                    collection.Add(i);

                    waitHandlePrint.Set();
                }

                isCompleted = true;
            });

            threadAdd.Start();
            var threadPrint = new Thread(() =>
            {
                while (!isCompleted)
                {
                    waitHandlePrint.WaitOne();

                    Print(collection);

                    waitHandleAdd.Set();
                }
            });
            threadPrint.Start();

            Console.ReadLine();
        }

        private static void Print(IEnumerable<int> collection)
        {
            Console.WriteLine(string.Join(",", collection));
        }
    }
}
