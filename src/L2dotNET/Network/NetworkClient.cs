using L2dotNET.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace L2dotNET.Network
{
    /// <summary>
    /// Represents the method that will handle Client.OnConnected" event.
    /// </summary>
    public delegate void OnConnectedEventHandler(IPEndPoint endPoint, byte connectionID);

    /// <summary>
    /// Represents the method that will handle Client.OnDisconnected" event.
    /// </summary>
    public delegate void OnDisconnectedEventHandler(int errorCode, NetworkClient client, byte connectionID);

    /// <summary>
    /// Abstract class to all client connections.
    /// </summary>
    public abstract class NetworkClient
    {
        /// <summary>
        /// Client <see cref="Socket"/>.
        /// </summary>
        public Socket m_Socket;

        /// <summary>
        /// Send buffers queue.
        /// </summary>
        protected readonly Queue<byte[]> m_SendQueue;

        /// <summary>
        /// Connection receive buffer.
        /// </summary>
        protected byte[] m_ReceiveBuffer;

        /// <summary>
        /// Receive callback.
        /// </summary>
        protected readonly AsyncCallback m_ReceiveCallback;

        /// <summary>
        /// Send callback.
        /// </summary>
        protected readonly AsyncCallback m_SendCallback;

        /// <summary>
        /// Indicates if packet header was received.
        /// </summary>
        protected bool m_HeaderReceived;

        /// <summary>
        /// Packet sending indicator.
        /// </summary>
        protected bool m_SendStackFlag;

        /// <summary>
        /// Indicates if sending packet is aviable.
        /// </summary>
        protected bool m_SendReadyFlag = true;

        /// <summary>
        /// Lock object.
        /// </summary>
        protected readonly object m_Lock = new object();

        /// <summary>
        /// Occurs after <see cref="NetworkClient"/> object was connected (initialized).
        /// </summary>
        public virtual event OnConnectedEventHandler OnConnected;

        /// <summary>
        /// Occurs after <see cref="NetworkClient"/> object was disconnected.
        /// </summary>
        public virtual event OnDisconnectedEventHandler OnDisconnected;

        /// <summary>
        /// Default connection buffer.
        /// </summary>
        protected static readonly byte[] m_DefaultBuffer = new byte[4];

        /// <summary>
        /// Currently received packet capacity.
        /// </summary>
        protected int m_ReceivedLength;

        /// <summary>
        /// Connection id.
        /// </summary>
        public byte ConnectionID = 1;

        /// <summary>
        /// Initializes new instance of <see cref="NetworkClient"/> connection.
        /// </summary>
        public NetworkClient()
        {
            m_ReceiveCallback = new AsyncCallback(ReceiveCallback);
            m_SendCallback = new AsyncCallback(SendCallback);
            m_SendQueue = new Queue<byte[]>();
            m_ReceiveBuffer = m_DefaultBuffer;
        }

        /// <summary>
        /// Initializes new instance of <see cref="NetworkClient"/> connection.
        /// </summary>
        /// <param name="socket">Client <see cref="Socket"/> object.</param>
        public NetworkClient(Socket socket)
            : this()
        {
            Logger.WriteLine(Source.Debug, "Try set m_Socket");
            try
            {
                m_Socket = socket;
            }
            catch (Exception ex)
            {
                Logger.Exception(ex, "Try set m_Socket");
            }

        }

        /// <summary>
        /// Forces <see cref="NetworkClient"/> to begin receive from socket.
        /// </summary>
        public abstract void BeginReceive();

        /// <summary>
        /// Handles received <see cref="Packet"/>.
        /// </summary>
        /// <param name="packet">Received <see cref="Packet"/>.</param>
        protected virtual void Handle(Packet packet)
        {
            Logger.WriteLine("Received (NC) :\r\n{0}", packet.ToString());
        }

        /// <summary>
        /// Receive <see cref="AsyncCallback"/> method.
        /// </summary>
        protected abstract unsafe void ReceiveCallback(IAsyncResult ar);

        /// <summary>
        /// Send <see cref="AsyncCallback"/> method.
        /// </summary>
        protected virtual void SendCallback(IAsyncResult ar)
        {
            try
            {
                lock (m_Lock)
                {
                    m_SendReadyFlag = false;

                    while (!m_SendReadyFlag && m_SendQueue.Count > 0)
                    {
                        byte[] buffer = m_SendQueue.Dequeue();
                        m_Socket.BeginSend(buffer, 0, buffer.Length, 0, m_SendCallback, null);
                    }

                    m_SendReadyFlag = true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                CloseConnection();

                if (OnDisconnected != null)
                    OnDisconnected(-1, this, ConnectionID);
            }
        }

        /// <summary>
        /// Serves for packet capacity validation and received buffer-><see cref="Packet"/> transforming.
        /// </summary>
        /// <param name="buffer">Received buffer.</param>
        /// <param name="length">Received buffer capacity.</param>
        public virtual void ReceiveData(byte[] buffer, int length) { }

        /// <summary>
        /// Sends buffer to client socket.
        /// </summary>
        /// <param name="buffer">Buffer to send.</param>
        public virtual void SendData(byte[] buffer)
        {
            //#if DEBUG_NET_CLIENT
            Console.WriteLine("Sending:\r\n{0}", L2Buffer.ToString(buffer));
            //#endif
            if (m_Socket != null && m_Socket.Connected)
            {
                lock (m_Lock)
                    m_SendQueue.Enqueue(buffer);

                if (m_SendReadyFlag)
                    SendCallback(null);
            }
        }

        /// <summary>
        /// Closes current client connection.
        /// </summary>
        public virtual void CloseConnection()
        {
            if (m_Socket != null && m_Socket.Connected)
            {
                try { m_Socket.Shutdown(SocketShutdown.Both); }
                catch (Exception e) { Console.WriteLine(e); }

                m_Socket = null;
            }
        }

        /// <summary>
        /// Indicates if client socket is connected.
        /// </summary>
        public virtual bool Connected
        {
            get { return m_Socket != null && m_Socket.Connected; }
        }
    }
}
