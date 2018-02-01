using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncWait
{
    class Program
    {
        static void Main(string[] args)
        {
            //Task<int> t1 = GetRandomAsync(new { Id = 1 });
            //Task<int> t2 = GetRandomAsync(new { Id = 2 });

            //Task[] tasks = new Task[] { t1, t2 };
            //Task.WaitAll(t1, t2);
            //Task.WaitAny(t1, t2);

            //Console.WriteLine($"{nameof(t1.IsCompleted)}:{t1.IsCompleted}");
            //Console.WriteLine($"{nameof(t2.IsCompleted)}:{t2.IsCompleted}");

            Task<int> t = GetRandomAsync();
            Console.WriteLine($"t.{nameof(t.IsCompleted)}:{t.IsCompleted}");
            Console.WriteLine(t.Result);

            Console.ReadLine();
        }

        private static async Task<int> GetRandomAsync(dynamic id)
        {
            int i = (int)id.Id;
            int num = await Task.Run(() =>
              {
                  Thread.Sleep(1000 * i);
                  return new Random().Next();
              });
            Console.WriteLine($"{i} 任务完成");
            return num;
        }

        private static async Task<int> GetRandomAsync()
        {
            Task<int> t1 = Task.Run(() =>
           {
               Thread.Sleep(1000);
               return new Random().Next();
           });

            Task<int> t2 = Task.Run(() =>
           {
               Thread.Sleep(5000);
               return new Random().Next();
           });

            // await Task.WhenAll(t1, t2);
            await Task.WhenAny(t1, t2);

            Console.WriteLine($"t1.{nameof(t1.IsCompleted)}:{t1.IsCompleted}");
            Console.WriteLine($"t2.{nameof(t2.IsCompleted)}:{t2.IsCompleted}");

            return t1.Result + t2.Result;
        }
    }
}
