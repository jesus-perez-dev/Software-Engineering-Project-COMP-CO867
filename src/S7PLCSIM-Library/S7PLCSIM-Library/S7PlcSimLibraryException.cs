using System;

namespace S7PLCSIM_Library
{
    public class S7PlcSimLibraryException : Exception
    {
        public S7PlcSimLibraryException() : base() { }
        public S7PlcSimLibraryException(string message) : base(message) { }
        public S7PlcSimLibraryException(string message, Exception inner) : base(message, inner) { }
    }
}