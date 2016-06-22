using System;

namespace L2dotNET.Network
{
    /// <summary>
    /// Service shutdown event arguments.
    /// </summary>
    public sealed class ServiceShutdownEventArgs : EventArgs
    {
        /// <summary>
        /// Shutdown message.
        /// </summary>
        public readonly string Message;

        /// <summary>
        /// Last occurred exception.
        /// </summary>
        public readonly Exception LastException;

        /// <summary>
        /// Initializes new instance of <see cref="ServiceShutdownEventArgs"/> object.
        /// </summary>
        /// <param name="message">Shutdown message.</param>
        public ServiceShutdownEventArgs(string message) : this(message, null) { }

        /// <summary>
        /// Initializes new instance of <see cref="ServiceShutdownEventArgs"/> object.
        /// </summary>
        /// <param name="message">Shutdown message.</param>
        /// <param name="e">Occurred <see cref="Exception"/>.</param>
        public ServiceShutdownEventArgs(string message, Exception e)
        {
            Message = message;
            LastException = e;
        }
    }
}