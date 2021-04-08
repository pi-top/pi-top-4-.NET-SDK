using System.Threading.Tasks;

using Microsoft.DotNet.Interactive;
using Microsoft.DotNet.Interactive.Commands;

namespace PiTop.MakerArchitecture.Expansion.InteractiveExtension
{
    public class KernelExtension : IKernelExtension
    {
        public async Task OnLoadAsync(Kernel kernel)
        {
            await kernel.SendAsync(
                new DisplayValue(new FormattedValue(
                    "text/markdown", 
                    "Added support for the ExpansionPlate.")));
        }
    }
}
