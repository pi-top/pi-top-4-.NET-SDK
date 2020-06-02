using System.Threading.Tasks;
using Microsoft.DotNet.Interactive;

namespace PiTopMakerArchitecture.Foundation.PSI.InteractiveExtension
{
    public class KernelExtension : IKernelExtension
    {
        public Task OnLoadAsync(IKernel kernel)
        {
            return Task.CompletedTask;
        }
    }
}
