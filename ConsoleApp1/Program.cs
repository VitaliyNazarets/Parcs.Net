using System;
using System.Reflection;
using System.Threading;
using Parcs;


namespace ConsoleApp1
{
	class Program : MainModule
    {
        private static int workers;
        private static int n;
        public static void Main(string[] args)
        {
            if (args == null || args.Length < 2)
            {
                throw new ArgumentException("Options is not correct");
            }
            Int32.TryParse(args[0], out workers);
            Int32.TryParse(args[1], out n);

            Console.Write($"{workers}  + {n}");
            var job = new Job();
            job.AddFile(Assembly.GetExecutingAssembly().Location);

            (new Program()).Run(new ModuleInfo(job, null));
            Console.WriteLine("Press ESC to exit");
            while (Console.ReadKey().Key != ConsoleKey.Escape) ;
        }

        public override void Run(ModuleInfo info, CancellationToken token = default(CancellationToken))
        {
            var points = new IPoint[workers];
            var channels = new IChannel[workers];
            for (int i = 0; i < workers; ++i)
            {
                points[i] = info.CreatePoint();
                channels[i] = points[i].CreateChannel();
                points[i].ExecuteClass("Queens.QueensModule");
            }

            int step = n / workers;
            for (int i = 0; i < workers - 1; ++i)
            {
                channels[i].WriteData(i * step);
                channels[i].WriteData(i * step + step);
                channels[i].WriteData(n);
            }
            channels[workers - 1].WriteData((workers - 1) * step);
            channels[workers - 1].WriteData(n);
            channels[workers - 1].WriteData(n);

            DateTime time = DateTime.Now;
            Console.WriteLine("Waiting for result...");

            double res = 0;
            for (int i = 0; i <= workers; ++i)
            {
                res += channels[i].ReadDouble();
            }

            Console.WriteLine("Result found: res = {0}, time = {1}", res, Math.Round((DateTime.Now - time).TotalSeconds, 3));

        }
    }
}

