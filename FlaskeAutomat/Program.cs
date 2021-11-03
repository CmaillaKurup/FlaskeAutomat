using System;
using System.Collections.Generic;
using System.Threading;

namespace Flaskeautomat
{
    class Program
    {
        static Queue<int> _liquidDrinks = new Queue<int>();
        static string beer = "Beer";
        static string soda = "Soda";
        private static int id;
        
        static object _lock = new object();
        static Random _random = new Random();
        
        
        static void Main()
        {
            Thread producerBeer = new Thread(ProducerBeer);
            Thread producerSoda = new Thread(ProducerSoda);
            Thread consumerBeer = new Thread(ConsumerBeer);
            Thread consumerSoda = new Thread(ConsumerSoda);
            
            producerBeer.Start();
            producerSoda.Start();
            consumerBeer.Start();
            consumerSoda.Start();
        }
        
        static void ProducerBeer()
        {
            while (true)
            {
                lock (_lock)
                {
                    Monitor.Enter(_lock);
                    try
                    {
                        id = _random.Next(1, 10);
                        if (_liquidDrinks.Count < 10)
                        {
                            _liquidDrinks.Enqueue(beer.Length);
                            Console.WriteLine(beer + " PRODUCE " + id);
                            Monitor.PulseAll(_lock);
                        }
                        else
                        {
                            Monitor.Wait(_lock);
                            Console.WriteLine("ProducerBeer is wating");
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
        static void ProducerSoda()
        {
            while (true)
            {
                Monitor.Enter(_lock);
                try
                {
                    id = _random.Next(1, 10);
                    if (_liquidDrinks.Count < 10)
                    {
                        _liquidDrinks.Enqueue(soda.Length);
                        Console.WriteLine(soda + " PRODUCE " + id);
                        Monitor.PulseAll(_lock);
                    }
                    else
                    {
                        Monitor.Wait(_lock);
                        Console.WriteLine("ProducerSoda is wating");
                    }
                }
                finally
                {
                    Monitor.Exit(_lock);
                }
                Thread.Sleep(_random.Next(200, 1000));
            }
        }

        static void ConsumerBeer()
        {
            while (true)
            {
                Monitor.Enter(_lock);
                try
                {
                    if (_liquidDrinks.Count != 0)
                    {
                        if (_liquidDrinks.Count > 0)
                        {
                            _liquidDrinks.Dequeue();
                            Console.WriteLine(beer + " CONSUMER " + id);
                            Monitor.PulseAll(_lock);
                        }
                        else
                        {
                            Monitor.Wait(_lock);
                            Console.WriteLine("ConsumeBeer is wating");
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

        static void ConsumerSoda()
        {
            while (true)
            {
                Monitor.Enter(_lock);
                try
                {
                    if (_liquidDrinks.Count > 0)
                    {
                        _liquidDrinks.Dequeue();
                        Console.WriteLine(soda + " CONSUME " + id);
                        Monitor.PulseAll(_lock);
                    }
                    else
                    {
                        Monitor.Wait(_lock);
                        Console.WriteLine("ConsumeSoda is wating");
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