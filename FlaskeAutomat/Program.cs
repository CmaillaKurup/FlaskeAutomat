using System;
using System.Threading;

namespace Flaskeautomat
{
    class Program
    {
        static int[] drinkArray = new int[2];
        private static int _id = 0;
        static object _lock = new object();
        static Random _random = new Random();


        static void Main()
        {

            Thread producer = new Thread(Producer);
            Thread consumer = new Thread(Consumer);

            producer.Start();
            consumer.Start();
        }

        static void Producer()
        {
            while (true)
            {
                Monitor.Enter(_lock);
                try
                {
                    if (drinkArray[0] == 0 || drinkArray[1] == 0)
                    {
                        _id = _random.Next(1, 3);
                        if (_id == 1)
                        {
                            drinkArray.SetValue(1, 0);
                            Console.WriteLine("PRODUCE id " + _id);
                        }

                        if (_id == 2)
                        {
                            drinkArray.SetValue(2, 1);
                            Console.WriteLine("PRODUCE id " + _id);
                        }
                        else
                        {
                            Console.WriteLine("Producer Waits");
                        }
                    }

                    Monitor.PulseAll(_lock);
                    Console.WriteLine(string.Join(", ", drinkArray) + "\n");
                    Thread.Sleep(_random.Next(200, 1000));
                }
                finally
                {
                    Monitor.Exit(_lock);
                }
            }
        }

        static void Consumer()
        {
            while (true)
            {
                Monitor.Enter(_lock);
                try
                {
                    if (drinkArray[0] == 1 || drinkArray[1] == 2)
                    {
                        if (drinkArray[0] == 1)
                        {
                            drinkArray.SetValue(0, 0);
                            Console.WriteLine("CONSUME id " + _id);
                            Monitor.PulseAll(_lock);
                        }

                        if (drinkArray[0] == 2)
                        {
                            drinkArray.SetValue(0, 1);
                            Console.WriteLine("CONSUME id " + _id);
                            Monitor.PulseAll(_lock);
                        }
                        else
                        {
                            Console.WriteLine("Consumer is wating");
                        }
                    }
                }
                finally
                {
                    Monitor.Exit(_lock);
                }
                Thread.Sleep(_random.Next(200, 1000));
            }
        }
    }
}