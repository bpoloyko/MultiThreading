/*
 * 2.	Write a program, which creates a chain of four Tasks.
 * First Task – creates an array of 10 random integer.
 * Second Task – multiplies this array with another random integer.
 * Third Task – sorts this array by ascending.
 * Fourth Task – calculates the average value. All this tasks should print the values to console.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MultiThreading.Task2.Chaining
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(".Net Mentoring Program. MultiThreading V1 ");
            Console.WriteLine("2.	Write a program, which creates a chain of four Tasks.");
            Console.WriteLine("First Task – creates an array of 10 random integer.");
            Console.WriteLine("Second Task – multiplies this array with another random integer.");
            Console.WriteLine("Third Task – sorts this array by ascending.");
            Console.WriteLine("Fourth Task – calculates the average value. All this tasks should print the values to console");
            Console.WriteLine();

            Task.Run(() =>
            {
                var random = new Random();
                var array = new int[10];
                for (var i = 0; i < 10; i++)
                {
                    array[i] = random.Next(-100, 100);
                }
                Console.Write("First Task: ");
                Print(array);
                return array;
            }).ContinueWith(ant =>
            {
                var array = ant.Result;
                var random = new Random();
                var number = random.Next(-100, 100);
                for (var i = 0; i < array.Length; i++)
                {
                    array[i] *= number;
                }
                Console.Write("Second Task: ");
                Console.WriteLine(number);
                Print(array);
                return array;
            }).ContinueWith(ant =>
            {
                var sorted = ant.Result.OrderBy(x => x).ToArray();
                Console.Write("Third Task: ");
                Print(sorted);
                return sorted;
            }).ContinueWith(ant =>
            {
                Console.Write("Fourth Task: ");
                var avg = ant.Result.Average();
                Console.WriteLine(avg);
                return avg;
            });

            Console.ReadLine();
        }

        private static void Print(IEnumerable<int> collection)
        {
            Console.WriteLine(string.Join(",", collection));
        }
    }
}
