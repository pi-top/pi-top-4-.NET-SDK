using System;
using System.Threading.Tasks;
using Microsoft.DotNet.Interactive;

namespace PiTop.InteractiveExtension
{
    public class KernelExtension : IKernelExtension
    {
        public Task OnLoadAsync(IKernel kernel)
        {
            return Task.CompletedTask;
        }
    }
}
