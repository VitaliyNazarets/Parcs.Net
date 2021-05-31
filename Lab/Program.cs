using CommandLine;
using NLog;
using Parcs;
using System;
using System.Reflection;
using System.Threading;

namespace Lab
{
	class Program : MainModule
    {
        private const string fileName = "resMatrix.mtr";

        private static readonly Logger _log = LogManager.GetLogger(nameof(MonteCarloModule));

        private static CLOptions options;

        public static void Main(string[] args)
        {

            options = null;

            if (args != null)
            {
                Parser.Default.ParseArguments<CLOptions>(args)
                    .WithParsed<CLOptions>(o =>
                    {
                        options = o;
                    })
                    .WithNotParsed(o =>
                    {
                        throw new ArgumentException($@"Cannot parse the arguments.");
                    });          
            }
            if (options == null)
                throw new ArgumentException("Null options");


            var job = new Job();
            job.AddFile(Assembly.GetExecutingAssembly().Location);

            (new Program()).Run(new ModuleInfo(job, null));

            Console.WriteLine("Press ESC to stop the process");
            while (Console.ReadKey().Key != ConsoleKey.Escape) ;
        }


        public override void Run(ModuleInfo info, CancellationToken token = default(CancellationToken))
        {
            string hand = options.Hand;
            int numberOfRetry = options.NumberOfHands;
            int numberOfPoints = options.NumberOfPoints;

            _log.Info("Starting Matrixes Module on {0} points", numberOfRetry);

            var points = new IPoint[numberOfPoints];
            var channels = new IChannel[numberOfPoints];

            for (int i = 0; i < numberOfPoints; ++i)
            {
                points[i] = info.CreatePoint();
                channels[i] = points[i].CreateChannel();
                points[i].ExecuteClass("Lab.MonteCarloModule");
            }


            for (int i = 0; i < numberOfPoints; ++i)
            {
                channels[i].WriteData(hand);
                channels[i].WriteData(numberOfRetry / numberOfPoints);
            }
            DateTime time = DateTime.Now;
            Console.WriteLine("Waiting for result...");

            double res = 0;
            for (int i = 0; i < numberOfPoints; ++i)
            {
                res += channels[i].ReadDouble();
            }

            res /= (double)numberOfPoints;

            Console.WriteLine("Probability of winning = {0}, Time = {1}.", res, Math.Round((DateTime.Now - time).TotalSeconds, 3));
        }


    }
}
