using McMaster.Extensions.CommandLineUtils;
using System;
using System.Threading;

namespace Bit0.CrunchLog.Cli
{
    public class Program
    {
        static void Main(String[] args)
        {
            CommandLineApplication.Execute<CliOptions>(args);
            
#if DEBUG
            Thread.Sleep(100);
            Console.Write(Environment.NewLine + "Press any key to continue...");
            Console.ReadKey();
#endif
        }
    }
}

