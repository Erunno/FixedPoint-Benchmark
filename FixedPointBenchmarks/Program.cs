//#define ctor

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cuni.Arithmetics.FixedPoint;
using System.Diagnostics;

namespace FixedPointBenchmarks
{
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch watch = new Stopwatch();
            int NumOfCycles = 10_000_000;
            int NumOfCyclesForToString = 1_000_000;

            //Jiting methods
            Fixed<Q8_24> a = new Fixed<Q8_24>(42);
            Fixed<Q8_24> b = new Fixed<Q8_24>(32);

            a.Add(b);
            a.Divide(b);
            a.Multiply(b);
            a.Subtract(b);

            a.ToString();

#if ctor
            System.Console.WriteLine("Testing with contructor");
            System.Console.WriteLine();
#endif

            //Add
            Console.WriteLine("Testing .Add:");
            watch.Start();
            for (int i = 0; i < NumOfCycles; i++)
            {
#if ctor
                b = new Fixed<Q8_24>(21);
#endif
                a.Add(b);
            }
            watch.Stop();
            Console.WriteLine("\tResult: " + watch.Elapsed);
            Console.WriteLine();
            watch.Reset();


            //Subract
            Console.WriteLine("Testing .Subtract:");
            watch.Start();
            for (int i = 0; i < NumOfCycles; i++)
            {
#if ctor
                b = new Fixed<Q8_24>(21);
#endif
                a.Subtract(b);
            }
            watch.Stop();
            Console.WriteLine("\tResult: " + watch.Elapsed);
            Console.WriteLine();
            watch.Reset();

            //Multiply
            Console.WriteLine("Testing .Multiply:");
            watch.Start();
            for (int i = 0; i < NumOfCycles; i++)
            {
#if ctor
                b = new Fixed<Q8_24>(21);
#endif
                a.Multiply(b);
            }
            watch.Stop();
            Console.WriteLine("\tResult: " + watch.Elapsed);
            Console.WriteLine();
            watch.Reset();

            //Multiply
            Console.WriteLine("Testing .Divide:");
            watch.Start();
            for (int i = 0; i < NumOfCycles; i++)
            {
#if ctor
                b = new Fixed<Q8_24>(21);
#endif
                a.Divide(b);
            }
            watch.Stop();
            Console.WriteLine("\tResult: " + watch.Elapsed);
            Console.WriteLine();
            watch.Reset();

            //ToString
            Console.WriteLine("Testing .ToString:");
            watch.Start();
            for (int i = 0; i < NumOfCyclesForToString; i++)
            {
                string s = a.ToString();
            }
            watch.Stop();
            Console.WriteLine("\tResult: " + watch.Elapsed);
            Console.WriteLine();
            watch.Reset();

        }
    }
}
