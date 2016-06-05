using System;
using System.Net.Sockets;
using L2dotNET.Utility;

namespace L2dotNET.Network
{
    /// <summary>
    /// Services types.
    /// </summary>
    public enum ServiceType : byte
    {
        /// <summary>
        /// Undefined service type.
        /// </summary>
        Undefined = 0x00,
        /// <summary>
        /// Login service type.
        /// </summary>
        LoginService = 0x01,
        /// <summary>
        /// Cache service type.
        /// </summary>
        CacheService = 0x02,
        /// <summary>
        /// Game service type.
        /// </summary>
        GameService = 0x03,
        /// <summary>
        /// Npc service type.
        /// </summary>
        NpcService = 0x04
    }

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
        /// Service unique id.
        /// </summary>
        private byte m_ServiceId;

        /// <summary>
        /// Service type.
        /// </summary>
        private ServiceType m_ServiceType;

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
        public InnerNetworkClient() : base() { }

        /// <summary>
        /// Initializes new instance of <see cref="InnerNetworkClient"/> object.
        /// </summary>
        /// <param name="serviceId">Remote service unique id.</param>
        /// <param name="serviceType">Remote service type.</param>
        /// <param name="socket"><see cref="Socket"/> used by connection.</param>
        /// <param name="handleDelegate">Service Handle Delegate, if null packet will not be handled</param>
        public InnerNetworkClient(byte serviceId, ServiceType serviceType, Socket socket, PacketHandleDelegate handleDelegate) : base(socket)
        {
            m_ServiceId = serviceId;
            m_ServiceType = serviceType;
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
                Console.WriteLine("Skipping handling");
            else
                HandleDelegate(packet);
        }

        /// <summary>
        /// Begins receive from connection socket.
        /// </summary>
        public override void BeginReceive()
        {
            m_Socket.BeginReceive(m_ReceiveBuffer, 0, 4, 0, m_ReceiveCallback, null);
        }

        /// <summary>
        /// Receive <see cref="AsyncCallback"/> method.
        /// </summary>
        /// <exception cref="InvalidOperationException" />
        protected override unsafe void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                m_ReceivedLength += m_Socket.EndReceive(ar);

                fixed (byte* buf = m_ReceiveBuffer)
                {
                    if (!m_HeaderReceived) //get packet capacity
                    {
                        L2Buffer.Extend(ref m_ReceiveBuffer, 0, *((int*)(buf)) - sizeof(int));
                        m_ReceivedLength = 0;
                        m_HeaderReceived = true;
                    }

                    if (m_ReceivedLength == m_ReceiveBuffer.Length) // all data received
                    {
                        Handle(new Packet(2, m_ReceiveBuffer));

                        m_ReceivedLength = 0;
                        m_ReceiveBuffer = m_DefaultBuffer;
                        m_HeaderReceived = false;

                        m_Socket.BeginReceive(m_ReceiveBuffer, 0, 4, 0, ReceiveCallback, null);
                    }
                    else if (m_ReceivedLength < m_ReceiveBuffer.Length) // not all data received
                        m_Socket.BeginReceive(m_ReceiveBuffer, m_ReceivedLength, m_ReceiveBuffer.Length - m_ReceivedLength, 0, m_ReceiveCallback, null);
                    else
                        throw new InvalidOperationException();
                }
            }
            catch (SocketException se)
            {
                if (OnDisconnected != null)
                    OnDisconnected(se.ErrorCode, this, ConnectionID);
                else
                {
                    Logger.WriteLine(Source.InnerNetwork, "{0} \r\nError code: {1}", se.ToString(), se.ErrorCode);
                    CloseConnection();
                }
            }
            catch (Exception e)
            {
                Logger.Exception(e);

                if (OnDisconnected != null)
                    OnDisconnected(-1, this, ConnectionID);
                else
                    CloseConnection();
            }
        }

        /// <summary>
        /// Sends <see cref="Packet"/> to remote side.
        /// </summary>
        /// <param name="p"><see cref="Packet"/> to send.</param>
        public virtual unsafe void Send(Packet p)
        {
            p.Prepare(sizeof(int));
            SendData(p.GetBuffer());
        }

        /// <summary>
        /// Creates packet from received buffer.
        /// </summary>
        /// <param name="buffer">Received buffer.</param>
        /// <param name="length">Received buffer length.</param>
        public override unsafe void ReceiveData(byte[] buffer, int length)
        {
            Handle(new Packet(sizeof(int), buffer));
        }

        /// <summary>
        /// Gets or sets connected service unique id.
        /// </summary>
        public byte ServiceID
        {
            get { return m_ServiceId; }
            set { m_ServiceId = value; }
        }

        /// <summary>
        /// Gets or sets connected <see cref="ServiceType"/>.
        /// </summary>
        public ServiceType ServiceType
        {
            get { return m_ServiceType; }
            set { m_ServiceType = value; }
        }
    }
}