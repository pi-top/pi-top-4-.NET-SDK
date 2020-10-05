using System.Threading.Tasks;

using Microsoft.DotNet.Interactive;

namespace PiTop.MakerArchitecture.Foundation.Psi.InteractiveExtension
{
    public class KernelExtension : IKernelExtension
    {
        public Task OnLoadAsync(Kernel kernel)
        {
            return Task.CompletedTask;
        }
    }
}
