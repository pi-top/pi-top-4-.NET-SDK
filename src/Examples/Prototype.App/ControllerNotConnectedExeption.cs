using System;

namespace Prototype
{
    public class ControllerNotConnectedExeption : Exception
    {
        public ControllerNotConnectedExeption(string message) : base(message) { }
        public ControllerNotConnectedExeption(string message, Exception innerException) : base(message, innerException) { }
    }
}