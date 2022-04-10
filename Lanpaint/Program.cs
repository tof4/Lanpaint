using System;
using System.Diagnostics;
using System.Linq;
using Lanchat.ClientCore;
using Lanchat.Core.FileSystem;

namespace Lanpaint
{
    public static class Program
    {
        public static Storage Storage { get; private set; }

        [STAThread]
        public static void Main(string[] args)
        {
            Storage = new Storage();
            CheckStartArguments(args);
            using var game = new Main();
            game.Run();
        }

        private static void CheckStartArguments(string[] args)
        {
            if (args.Contains("--no-saved") || args.Contains("-a")) Storage.Config.ConnectToSaved = false;

            if (args.Contains("--no-udp") || args.Contains("-b")) Storage.Config.NodesDetection = false;

            if (args.Contains("--no-server") || args.Contains("-n")) Storage.Config.StartServer = false;

            if (args.Contains("--debug") || args.Contains("-d") || Debugger.IsAttached) Storage.Config.DebugMode = true;
        }
    }
}