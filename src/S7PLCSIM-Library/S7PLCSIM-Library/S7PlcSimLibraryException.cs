using System;

namespace S7PLCSIM_Library
{
    /// Exception thrown when the Mohawk S7PLCSIM Client Library encounters an error.
    public class S7PlcSimLibraryException : Exception
    {
        public S7PlcSimLibraryException() : base() { }
        public S7PlcSimLibraryException(string message) : base(message) { }
        public S7PlcSimLibraryException(string message, Exception inner) : base(message, inner) { }
    }
}