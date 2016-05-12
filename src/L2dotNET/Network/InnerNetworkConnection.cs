using L2dotNET.Utility;
using System;
using System.Net;
using System.Net.Sockets;
using System.Timers;

namespace L2dotNET.Network
{
    /// <summary>
    /// Represents inner network connection class.
    /// </summary>
    public class InnerNetworkConnection : InnerNetworkClient
    {
        /// <summary>
        /// Remote service <see cref="IPEndPoint"/> data.
        /// </summary>
        public IPEndPoint RemoteEndPoint;

        /// <summary>
        /// Reconnect <see cref="Timer"/> object.
        /// </summary>
        protected Timer m_ReconnectTimer;

        /// <summary>
        /// Reconnect interval, 5 seconds by default.
        /// </summary>
        public TimeSpan ReconnectInterval = TimeSpan.FromSeconds(5.0);

        /// <summary>
        /// Occurs after <see cref="InnerNetworkConnection"/> object connected to remote service.
        /// </summary>
        public override event OnConnectedEventHandler OnConnected;

        /// <summary>
        /// Initializes new instance of inner network connection.
        /// </summary>
        /// <param name="remoteEndPoint">Remote endpoint.</param>
        /// <param name="reconnectInterval">Reconnect interval.</param>
        public InnerNetworkConnection(IPEndPoint remoteEndPoint, TimeSpan reconnectInterval)
            : base()
        {
            RemoteEndPoint = remoteEndPoint;
            ReconnectInterval = reconnectInterval;
            m_ReconnectTimer = new Timer(ReconnectInterval.TotalMilliseconds);
            m_ReconnectTimer.Elapsed += new ElapsedEventHandler(TryConnect);
        }

        /// <summary>
        /// Attempts to connect to remote service.
        /// </summary>
        /// <exception cref="InvalidOperationException" />
        public virtual void BeginConnect()
        {
            if (m_ReconnectTimer != null && m_ReconnectTimer.Enabled)
                throw new InvalidOperationException("Already reconnecting");

            TryConnect(null, null);
        }

        /// <summary>
        /// <paramref name="m_ReconnectTimer"/> <see cref="System.Timers.Timer.Elapsed"/> event handler for current connection.
        /// </summary>
        protected virtual void TryConnect(object sender, ElapsedEventArgs e)
        {
            m_ReconnectTimer.Stop();

            try
            {
                Logger.WriteLine(Source.InnerNetwork, "Connecting to {0}...", RemoteEndPoint);

                m_Socket = new Socket(RemoteEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                NetworkHelper.SetCommonSocketOptions(ref m_Socket);
                m_Socket.Connect(RemoteEndPoint);

                if (m_Socket.Connected)
                {
                    if (OnConnected == null)
                        Logger.WriteLine(Source.InnerNetwork, "OnConnected event is null, connection is not aviable.");
                    else
                        OnConnected(RemoteEndPoint, ConnectionID);

                    return;
                }
            }
            catch (SocketException se)
            {
                if (se.ErrorCode == 10061) // No connection could be made because the target machine actively refused it 
                    Logger.WriteLine(se.Message);
                else
                    Logger.WriteLine(se + "Error code: " + se.ErrorCode);
            }
            catch (Exception ex)
            {
                Logger.Exception(ex);
            }

            m_ReconnectTimer.Start();
        }

        /// <summary>
        /// Sets remote service id and <see cref="ServiceType"/>.
        /// </summary>
        /// <param name="remoteServiceID">Remote service id.</param>
        /// <param name="remoteServiceType">Remote <see cref="ServiceType"/>.</param>
        public void SetRemoteService(byte remoteServiceID, ServiceType remoteServiceType)
        {
            ServiceID = remoteServiceID;
            ServiceType = remoteServiceType;
        }
    }
}
