using System.Threading.Tasks;

using Microsoft.DotNet.Interactive;

namespace PiTop.Camera.Psi.InteractiveExtension
{
    public class KernelExtension : IKernelExtension
    {
        public Task OnLoadAsync(IKernel kernel)
        {
            return Task.CompletedTask;
        }
    }
}
