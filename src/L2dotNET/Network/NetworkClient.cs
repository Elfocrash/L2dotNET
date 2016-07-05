using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using L2dotNET.Utility;

namespace L2dotNET.Network
{
    /// <summary>
    /// Represents the method that will handle Client.OnConnected" event.
    /// </summary>
    public delegate void OnConnectedEventHandler(IPEndPoint endPoint, byte connectionId);

    /// <summary>
    /// Represents the method that will handle Client.OnDisconnected" event.
    /// </summary>
    public delegate void OnDisconnectedEventHandler(int errorCode, NetworkClient client, byte connectionId);

    /// <summary>
    /// Abstract class to all client connections.
    /// </summary>
    public abstract class NetworkClient
    {
        /// <summary>
        /// Client <see cref="Socket"/>.
        /// </summary>
        public Socket MSocket;

        /// <summary>
        /// Send buffers queue.
        /// </summary>
        protected readonly Queue<byte[]> MSendQueue;

        /// <summary>
        /// Connection receive buffer.
        /// </summary>
        protected byte[] MReceiveBuffer;

        /// <summary>
        /// Receive callback.
        /// </summary>
        protected readonly AsyncCallback MReceiveCallback;

        /// <summary>
        /// Send callback.
        /// </summary>
        protected readonly AsyncCallback MSendCallback;

        /// <summary>
        /// Indicates if packet header was received.
        /// </summary>
        protected bool MHeaderReceived;

        /// <summary>
        /// Packet sending indicator.
        /// </summary>
        protected bool MSendStackFlag;

        /// <summary>
        /// Indicates if sending packet is aviable.
        /// </summary>
        protected bool MSendReadyFlag = true;

        /// <summary>
        /// Lock object.
        /// </summary>
        protected readonly object MLock = new object();

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
        protected static readonly byte[] MDefaultBuffer = new byte[4];

        /// <summary>
        /// Currently received packet capacity.
        /// </summary>
        protected int MReceivedLength;

        /// <summary>
        /// Connection id.
        /// </summary>
        public byte ConnectionId = 1;

        /// <summary>
        /// Initializes new instance of <see cref="NetworkClient"/> connection.
        /// </summary>
        public NetworkClient()
        {
            MReceiveCallback = new AsyncCallback(ReceiveCallback);
            MSendCallback = new AsyncCallback(SendCallback);
            MSendQueue = new Queue<byte[]>();
            MReceiveBuffer = MDefaultBuffer;
        }

        /// <summary>
        /// Initializes new instance of <see cref="NetworkClient"/> connection.
        /// </summary>
        /// <param name="socket">Client <see cref="Socket"/> object.</param>
        public NetworkClient(Socket socket) : this()
        {
            Logger.WriteLine(Source.Debug, "Try set m_Socket");
            try
            {
                MSocket = socket;
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
                lock (MLock)
                {
                    MSendReadyFlag = false;

                    while (!MSendReadyFlag && (MSendQueue.Count > 0))
                    {
                        byte[] buffer = MSendQueue.Dequeue();
                        MSocket.BeginSend(buffer, 0, buffer.Length, 0, MSendCallback, null);
                    }

                    MSendReadyFlag = true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                CloseConnection();

                if (OnDisconnected != null)
                {
                    OnDisconnected(-1, this, ConnectionId);
                }
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
            Console.WriteLine($"Sending:\r\n{L2Buffer.ToString(buffer)}");
            //#endif
            if ((MSocket != null) && MSocket.Connected)
            {
                lock (MLock)
                {
                    MSendQueue.Enqueue(buffer);
                }

                if (MSendReadyFlag)
                {
                    SendCallback(null);
                }
            }
        }

        /// <summary>
        /// Closes current client connection.
        /// </summary>
        public virtual void CloseConnection()
        {
            if ((MSocket != null) && MSocket.Connected)
            {
                try
                {
                    MSocket.Shutdown(SocketShutdown.Both);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

                MSocket = null;
            }
        }

        /// <summary>
        /// Indicates if client socket is connected.
        /// </summary>
        public virtual bool Connected
        {
            get { return (MSocket != null) && MSocket.Connected; }
        }
    }
}