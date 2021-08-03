using System;

namespace PiTop
{
    public sealed class  PlatePortInUseException : Exception{
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="port">The Port that is already used.</param>
        public PlatePortInUseException(PlatePort port): base($"Port {port.Name} already in use by (another) {port.Device}")
        {
            
        }
    }
}