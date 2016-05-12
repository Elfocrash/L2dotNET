using L2dotNET.Scructs.Services;
using L2dotNET.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace L2dotNET.Network
{
    /// <summary>
    /// Provides some help methods related to inner network activity.
    /// </summary>
    public static class NetworkHelper
    {
        /// <summary>
        /// Represents struct that contains info about remote service while it's attempting to connect.
        /// </summary>
        public struct RemoteServiceInfo
        {
            /// <summary>
            /// Gets remote service unique id.
            /// </summary>
            public readonly byte ServiceId;

            /// <summary>
            /// Gets remote service type;
            /// </summary>
            public readonly ServiceType ServiceType;

            /// <summary>
            /// Initializes new instance of <see cref="RemoteServiceInfo"/> struct.
            /// </summary>
            /// <param name="serviceId">Remote service unique id.</param>
            /// <param name="serviceType">Remote service type.</param>
            internal RemoteServiceInfo(byte serviceId, ServiceType serviceType)
            {
                ServiceId = serviceId;
                ServiceType = serviceType;
            }

            /// <summary>
            /// Invalid <see cref="RemoteServiceInfo"/> equivalent.
            /// </summary>
            public static readonly RemoteServiceInfo Empty = new RemoteServiceInfo(0, ServiceType.Undefined);
        }

        /// <summary>
        /// Gets <see cref="RemoteServiceInfo"/> object for provided <see cref="Socket"/> object.
        /// </summary>
        /// <param name="serviceSocket"><see cref="Socket"/> to get info about.</param>
        /// <returns><see cref="RemoteServiceInfo"/> object if received data is valid.</returns>
        public static RemoteServiceInfo GetServiceInfo(Socket serviceSocket)
        {
            if (serviceSocket != null && serviceSocket.Connected)
            {
                try
                {
                    byte[] buffer = new byte[8];

                    serviceSocket.Receive(buffer);

                    if (buffer[4] == InitializeRequest.Opcodes[0] && buffer[5] == InitializeRequest.Opcodes[1])
                        return new RemoteServiceInfo(buffer[6], (ServiceType)buffer[7]);
                }
                catch (Exception e)
                {
                    Logger.Exception(e);
                }
            }

            return RemoteServiceInfo.Empty;
        }

        /// <summary>
        /// Sets some common socket options, used in communication between both server and client sides.
        /// </summary>
        /// <param name="socket"><see cref="Socket"/> object, for which options must be applied.</param>
        public static void SetCommonSocketOptions(ref Socket socket)
        {
            socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.NoDelay, true);
            socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveBuffer, 0);
            socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendBuffer, 0);
        }

        /// <summary>
        /// Closes provided socket connection.
        /// </summary>
        /// <param name="socket">Socket to be closed.</param>
        public static void CloseSocket(ref Socket socket)
        {
            try
            {
                if (socket.Connected)
                    socket.Shutdown(SocketShutdown.Both);
                socket = null;
            }
            catch
            {
            }
        }
    }
}
