using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rgz
{
    public  class Program
    {
        static void Main(string[] args)
        {
            List<(double, double, double)> result = new List<(double, double, double)>(); 
            for (int i = 0; i < 10; i++)
            {
                BusStop busStop = new BusStop(120, 8, 10);
                result.Add(busStop.Simulate());
            }
            
            
            double avgItem1 = result.Select(r => r.Item1).Average();
            double avgItem2 = result.Select(r => r.Item2).Average();
            double avgItem3 = result.Select(r => r.Item3).Average();

            
            Console.WriteLine($"\n\nСреднее время пребывания на остановке: {avgItem1:F1}");
            Console.WriteLine($"Среднее время ожидания: {avgItem2:F1}");
            Console.WriteLine($"Средняя длина очереди: {avgItem3:F1}");
            Console.ReadLine();
        }
    }
}
