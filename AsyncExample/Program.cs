using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncExample
{
    class Program
    {
        public static readonly Stopwatch watch = new Stopwatch();

        static void Main(string[] args)
        {
            watch.Start();

            string url1 = "https://www.microsoft.com/zh-cn/";
            string url2 = "http://www.cnblogs.com/liqingwen/";

            ///sync
            //int length1 = CountCharacters(new { Id = 1 }, url1);
            //int length2 = CountCharacters(new { Id = 2 }, url2);

            ///Async
            Task<int> length1 = CountCharacterAsync(new { Id = 1 }, url1);
            Task<int> length2 = CountCharacterAsync(new { Id = 2 }, url2);

            for (int i = 0; i < 3; i++)
            {
                ExtratOperation(new { Id = i });
            }

            //Console.WriteLine($"{url1}的字符个数：{length1}");
            //Console.WriteLine($"{url2}的字符个数：{length2}");

            Console.WriteLine($"{url1}的字符个数：{length1.Result}");
            Console.WriteLine($"{url2}的字符个数：{length2.Result}");

            Console.ReadLine();
        }

        static int CountCharacters(dynamic i, string url)
        {
            int id = (int)i.Id;
            var client = new WebClient();
            Console.WriteLine($"开始调用 Id={id}:{watch.ElapsedMilliseconds} ms. 执行线程:{Thread.CurrentThread.ManagedThreadId}");
            string countStr = client.DownloadString(url);
            Console.WriteLine($"调用完成 Id={id}:{watch.ElapsedMilliseconds} ms. 执行线程:{Thread.CurrentThread.ManagedThreadId}");
            return countStr.Length;
        }

        static async Task<int> CountCharacterAsync(dynamic i, string url)
        {
            int id = (int)i.Id;
            var client = new WebClient();
            Console.WriteLine($"开始调用 Id={id}:{watch.ElapsedMilliseconds} ms. 执行线程:{Thread.CurrentThread.ManagedThreadId}");
            string countStr = await client.DownloadStringTaskAsync(url);
            Console.WriteLine($"调用完成 Id={id}:{watch.ElapsedMilliseconds} ms. 执行线程:{Thread.CurrentThread.ManagedThreadId}");
            return countStr.Length;
        }


        static void ExtratOperation(dynamic i)
        {
            int id = (int)i.Id;
            StringBuilder sb = new StringBuilder();
            for (int j = 0; j < 6000; j++)
            {
                sb.AppendLine("Hello");
            }
            Console.WriteLine($"Id ={id}的ExtratOperation方法完成：{watch.ElapsedMilliseconds} ms. 执行线程:{Thread.CurrentThread.ManagedThreadId}");
        }
    }
}
