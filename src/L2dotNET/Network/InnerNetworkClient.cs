using System;
using System.Net.Sockets;
using L2dotNET.Utility;

namespace L2dotNET.Network
{
    /// <summary>
    /// Delegate for packet handling
    /// </summary>
    /// <param name="packet">Incoming packet</param>
    public delegate void PacketHandleDelegate(Packet packet);

    /// <summary>
    /// Represents inner network client (remote) connection class.
    /// </summary>
    public class InnerNetworkClient : NetworkClient
    {
        /// <summary>
        /// Remote service settings.
        /// </summary>
        public RemoteServiceSettings RemoteServiceSettings;

        /// <summary>
        /// Fires when connection is lost or broken.
        /// </summary>
        public override event OnDisconnectedEventHandler OnDisconnected;

        /// <summary>
        /// Packet handling method.
        /// </summary>
        public PacketHandleDelegate HandleDelegate;

        /// <summary>
        /// Initializes new instance of <see cref="InnerNetworkClient"/> object.
        /// </summary>
        public InnerNetworkClient() { }

        /// <summary>
        /// Initializes new instance of <see cref="InnerNetworkClient"/> object.
        /// </summary>
        /// <param name="serviceId">Remote service unique id.</param>
        /// <param name="serviceType">Remote service type.</param>
        /// <param name="socket"><see cref="Socket"/> used by connection.</param>
        /// <param name="handleDelegate">Service Handle Delegate, if null packet will not be handled</param>
        public InnerNetworkClient(byte serviceId, ServiceType serviceType, Socket socket, PacketHandleDelegate handleDelegate) : base(socket)
        {
            ServiceId = serviceId;
            ServiceType = serviceType;
            HandleDelegate = handleDelegate;
        }

        /// <summary>
        /// Creates new instance of <see cref="InnerNetworkClient"/> object.
        /// </summary>
        /// <param name="serviceId">Service unique id.</param>
        /// <param name="serviceType">Service type.</param>
        /// <param name="socket">Service <see cref="Socket"/> object.</param>
        public InnerNetworkClient(byte serviceId, ServiceType serviceType, Socket socket) : this(serviceId, serviceType, socket, null) { }

        /// <summary>
        /// Handles incoming packet.
        /// </summary>
        /// <param name="packet">Incoming packet.</param>
        protected override void Handle(Packet packet)
        {
            if (HandleDelegate == null)
            {
                Console.WriteLine("Skipping handling");
            }
            else
            {
                HandleDelegate(packet);
            }
        }

        /// <summary>
        /// Begins receive from connection socket.
        /// </summary>
        public override void BeginReceive()
        {
            MSocket.BeginReceive(MReceiveBuffer, 0, 4, 0, MReceiveCallback, null);
        }

        /// <summary>
        /// Receive <see cref="AsyncCallback"/> method.
        /// </summary>
        /// <exception cref="InvalidOperationException" />
        protected override unsafe void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                MReceivedLength += MSocket.EndReceive(ar);

                fixed (byte* buf = MReceiveBuffer)
                {
                    if (!MHeaderReceived) //get packet capacity
                    {
                        L2Buffer.Extend(ref MReceiveBuffer, 0, *(int*)buf - sizeof(int));
                        MReceivedLength = 0;
                        MHeaderReceived = true;
                    }

                    if (MReceivedLength == MReceiveBuffer.Length) // all data received
                    {
                        Handle(new Packet(2, MReceiveBuffer));

                        MReceivedLength = 0;
                        MReceiveBuffer = MDefaultBuffer;
                        MHeaderReceived = false;

                        MSocket.BeginReceive(MReceiveBuffer, 0, 4, 0, ReceiveCallback, null);
                    }
                    else if (MReceivedLength < MReceiveBuffer.Length) // not all data received
                    {
                        MSocket.BeginReceive(MReceiveBuffer, MReceivedLength, MReceiveBuffer.Length - MReceivedLength, 0, MReceiveCallback, null);
                    }
                    else
                    {
                        throw new InvalidOperationException();
                    }
                }
            }
            catch (SocketException se)
            {
                Logger.WriteLine(Source.InnerNetwork, "{0} \r\nError code: {1}", se.ToString(), se.ErrorCode);

                CloseConnection();

                OnDisconnected?.Invoke(se.ErrorCode, this, ConnectionId);
            }
            catch (Exception e)
            {
                Logger.Exception(e);

                CloseConnection();

                OnDisconnected?.Invoke(-1, this, ConnectionId);
            }
        }

        /// <summary>
        /// Sends <see cref="Packet"/> to remote side.
        /// </summary>
        /// <param name="p"><see cref="Packet"/> to send.</param>
        public virtual void Send(Packet p)
        {
            p.Prepare(sizeof(int));
            SendData(p.GetBuffer());
        }

        /// <summary>
        /// Creates packet from received buffer.
        /// </summary>
        /// <param name="buffer">Received buffer.</param>
        /// <param name="length">Received buffer length.</param>
        public override void ReceiveData(byte[] buffer, int length)
        {
            Handle(new Packet(sizeof(int), buffer));
        }

        /// <summary>
        /// Gets or sets connected service unique id.
        /// </summary>
        public byte ServiceId { get; set; }

        /// <summary>
        /// Gets or sets connected <see cref="ServiceType"/>.
        /// </summary>
        public ServiceType ServiceType { get; set; }
    }
}