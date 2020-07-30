using System;

namespace PiTop.Abstractions
{
    public class DisplayAcquisitionException : Exception
    {
        public DisplayAcquisitionException() : base("Error during display acquisition.")
        {
        }
        public DisplayAcquisitionException(Exception exception) : base("Error during display acquisition.", exception)
        {
        }
    }
}