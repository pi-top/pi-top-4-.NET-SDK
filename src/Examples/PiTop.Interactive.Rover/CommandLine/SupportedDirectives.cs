using System.Collections.Generic;
using System.CommandLine;

namespace PiTop.Interactive.Rover.CommandLine
{
    public class SupportedDirectives
    {
        public string KernelName { get; }

        public SupportedDirectives(string kernelName)
        {
            KernelName = kernelName;
        }

        public List<ICommand> Commands { get; } = new List<ICommand>();
    }
}