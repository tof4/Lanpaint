using System;
using System.Linq;
using System.Diagnostics;
using Lanchat.ClientCore;

namespace Lanpaint
{
    public static class Program
    {
        public static Config Config { get; private set; }
        
        [STAThread]
        public static void Main(string[] args)
        {
            Config = Storage.LoadConfig();
            CheckStartArguments(args);
            using var game = new Main();
            game.Run();
        }
        
        private static void CheckStartArguments(string[] args)
        {
            if (args.Contains("--no-saved") || args.Contains("-a"))
            {
                Config.ConnectToSaved = false;
            }

            if (args.Contains("--no-udp") || args.Contains("-b"))
            {
                Config.NodesDetection = false;
            }

            if (args.Contains("--no-server") || args.Contains("-n"))
            {
                Config.StartServer = false;
            }

            if (args.Contains("--debug") || args.Contains("-d") || Debugger.IsAttached)
            {
                Config.DebugMode = true;
            }
        }
    }
}