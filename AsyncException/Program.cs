using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncException
{
    class Program
    {
        static void Main(string[] args)
        {
            Task t = ExceptionAsync();
            t.Wait(); //在调用方法中同步等待任务

            Console.WriteLine($"{nameof(t.Status)}:{t.Status}");
            Console.WriteLine($"{nameof(t.IsCompleted)}:{t.IsCompleted}");
            Console.WriteLine($"{nameof(t.IsFaulted)}:{t.IsFaulted}");

            Console.ReadLine();
        }

        private static async Task ExceptionAsync()
        {
            try
            {
                await Task.Run(() => { throw new Exception(); });
            }
            catch (Exception)
            {
                Console.WriteLine($"{nameof(ExceptionAsync)}出现异常");
            }
        }
    }
}
