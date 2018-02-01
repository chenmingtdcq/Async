using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncReturn
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine($"执行线程：{Thread.CurrentThread.ManagedThreadId}");
            //Task<int> result = Calcu.AddAsync(1, 1);
            //Task t = ExtratOperation();

            //int i = Calcu.Add(1, 1);

            //Console.WriteLine($"Result:{result.Result}");
            //Console.WriteLine($"Result:{i}");

            //Task task = Calcu.AddAsync1(1, 1);
            //Task t1 = ExtratOperation();
            //task.Wait();
            //Console.WriteLine($"AddAsync1执行完成。");

            //Calcu.AddAsync2(1, 1);
            //Task t2 = ExtratOperation();
            //Console.WriteLine($"AddAsync2执行完成。");

            //Task t = Do.GetGuidAsync();
            //t.Wait();

            //t = Do.GetGuidAsync1();
            //t.Wait();

            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            CancellationToken token = cancellationTokenSource.Token;
            Task t = Excute.ExcuteAsync(token);

            Thread.Sleep(3000);
            cancellationTokenSource.Cancel(); //任务取消

            t.Wait();
            Console.WriteLine($"{nameof(token.IsCancellationRequested)}:{token.IsCancellationRequested}");

            Console.ReadLine();
        }

        static async Task ExtratOperation()
        {
            Console.WriteLine($"执行线程：{Thread.CurrentThread.ManagedThreadId}");
            await Task.Run(() => { Append(); });
            Console.WriteLine($"ExtratOperation 方法执行完成。");
        }

        static void Append()
        {
            Console.WriteLine($"执行线程：{Thread.CurrentThread.ManagedThreadId}");
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < 100; i++)
            {
                stringBuilder.AppendLine("Hello");
                Console.WriteLine($"Working...");
            }
        }

        internal class Calcu
        {
            public static int Add(int m, int n)
            {
                Console.WriteLine($"执行线程：{Thread.CurrentThread.ManagedThreadId}");
                return m + n;
            }

            public static async Task<int> AddAsync(int m, int n)
            {
                Console.WriteLine($"执行线程：{Thread.CurrentThread.ManagedThreadId}");
                int result = await Task.Run(() => { return Add(m, n); });
                return result;
            }

            public static async Task AddAsync1(int m, int n)
            {
                Console.WriteLine($"执行线程：{Thread.CurrentThread.ManagedThreadId}");
                int result = await Task.Run(() => { return Add(m, n); });
                Console.WriteLine($"Return:{result}");
            }

            public static async void AddAsync2(int m, int n)
            {
                Console.WriteLine($"执行线程：{Thread.CurrentThread.ManagedThreadId}");
                int result = await Task.Run(() => { return Add(m, n); });
                Console.WriteLine($"Return:{result}");
            }
        }

        internal class Do
        {
            public static Guid GetGuid()
            {
                return Guid.NewGuid();
            }

            public static async Task GetGuidAsync()
            {
                var func = new Func<Guid>(GetGuid);
                var t1 = await Task.Run(func);

                var t2 = await Task.Run(() => GetGuid());

                var t3 = await Task.Run(() => { return Guid.NewGuid(); });

                var t4 = await Task.Run(new Func<Guid>(GetGuid));

                Console.WriteLine($"{t1}");
                Console.WriteLine($"{t2}");
                Console.WriteLine($"{t3}");
                Console.WriteLine($"{t4}");
            }

            public static async Task GetGuidAsync1()
            {
                await Task.Run(() => { Console.WriteLine(Guid.NewGuid()); });
                Console.WriteLine(await Task.Run(() => GetGuid()));
                await Task.Run(() => Task.Run(() => { Console.WriteLine(Guid.NewGuid()); }));
                Console.WriteLine(await Task.Run(() => Task.Run(() => GetGuid())));
            }
        }

        internal class Excute
        {
            public static async Task ExcuteAsync(CancellationToken token)
            {
                if (token.IsCancellationRequested)
                    return;
                await Task.Run(() => CircleOutput(token), token);
            }

            public static void CircleOutput(CancellationToken token)
            {
                Console.WriteLine($"{nameof(CircleOutput)}方法开始调用");
                int num = 5;
                for (int i = 1; i <= num; i++)
                {
                    if (token.IsCancellationRequested)
                        return;
                    Console.WriteLine($"{i}/{num}完成");
                    Thread.Sleep(1000);
                }
            }
        }
    }
}
