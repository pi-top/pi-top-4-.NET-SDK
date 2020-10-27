using System.IO;
using Microsoft.DotNet.Interactive.Http;

namespace PiTop.Interactive.Rover.CommandLine
{
    public class StartupOptions
    {
        public StartupOptions(
            DirectoryInfo logPath = null,
            bool verbose = false,
            HttpPort httpPort = null,
            DirectoryInfo workingDir = null)
        {
            LogPath = logPath;
            Verbose = verbose;
            HttpPort = httpPort;
            WorkingDir = workingDir;
        }


        public DirectoryInfo LogPath { get; }

        public bool Verbose { get; }

        public HttpPort HttpPort { get; internal set; }

        public DirectoryInfo WorkingDir { get; internal set; }

    }
}