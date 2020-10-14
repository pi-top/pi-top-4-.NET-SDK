using System.Threading.Tasks;

using Microsoft.DotNet.Interactive;

namespace PiTop.MakerArchitecture.Expansion.InteractiveExtension
{
    public class KernelExtension : IKernelExtension
    {
        public Task OnLoadAsync(Kernel kernel)
        {
            KernelInvocationContext.Current?.Display(
                $@"Added support for the ExpansionPlate.",
                "text/markdown");

            return Task.CompletedTask;
        }
    }
}
