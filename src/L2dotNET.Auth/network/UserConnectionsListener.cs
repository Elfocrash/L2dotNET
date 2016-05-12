using L2.Net.LoginService.Crypt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace L2dotNET.Auth.network
{
    /// <summary>
    /// Incoming user connections listener.
    /// </summary>
    internal static class UserConnectionsListener
    {
        /// <summary>
        /// Active <see cref="UserConnection"/> objects collection.
        /// </summary>
        private static SortedList<int, UserConnection> m_ActiveConnections = new SortedList<int, UserConnection>();

        /// <summary>
        /// Incoming connections <see cref="Listener"/>.
        /// </summary>
        private static Listener m_ListenerService;

        /// <summary>
        /// <see cref="Listener"/> <see cref="Thread"/>
        /// </summary>
        private static Thread m_ListenerThread;

        /// <summary>
        /// Indicates if <see cref="UserConnectionsListener"/> is active.
        /// </summary>
        private static volatile bool m_Active;

        /// <summary>
        /// Local <see cref="IPEndPoint"/>.
        /// </summary>
        private static IPEndPoint m_LocalEndPoint;

        /// <summary>
        /// Local listener backlog.
        /// </summary>
        private static int m_Backlog;

        /// <summary>
        /// Indicates if <see cref="Firewall"/> must be enabled.
        /// </summary>
        private static bool m_EnableFirewall;

        internal static ScrambledKeyPair ScrambledKeysPair;
        internal static RSAParameters PrivateKey, PublicKey;

        /// <summary>
        /// Initializes user connections listener.
        /// </summary>
        /// <param name="localEndPoint">Local ip endpoint.</param>
        /// <param name="backlog">Backlog.</param>
        /// <param name="enableFirewall">True, if firewall must be enabled, otherwise false.</param>
        internal static void Initialize(IPEndPoint localEndPoint, int backlog, bool enableFirewall)
        {
            m_LocalEndPoint = localEndPoint;
            m_Backlog = backlog;
            m_EnableFirewall = enableFirewall;

            RSAManaged rsa = new RSAManaged(1024);

            PrivateKey = rsa.ExportParameters(true);
            PublicKey = rsa.ExportParameters(false);

            ScrambledKeysPair = new ScrambledKeyPair(ref PrivateKey, ref PublicKey);

            rsa = null;

            Enable();
        }

        /// <summary>
        /// Disables network listener and stops listener thread.
        /// </summary>
        internal static void Disable()
        {
            if (m_Active)
            {
                CloseAllConnections();
                StopListener();
                StopListenerThread();
            }
        }

        /// <summary>
        /// Enables network listener and starts listener thread.
        /// </summary>
        internal static void Enable()
        {
            Disable();

            InitializeListener();
            StartListenerThread();
        }

        /// <summary>
        /// Initializes listener.
        /// </summary>
        private static void InitializeListener()
        {
            m_ListenerService = new Listener(m_LocalEndPoint, m_Backlog);
            m_ListenerService.OnStarted += new OnListenerStartedEventHandler(ListenerService_OnStarted);
            m_ListenerService.OnStopped += new OnListenerStoppedEventHandler(ListenerService_OnStopped);
            m_ListenerService.OnTerminated += new OnListenerTerminatedEventHandler(ListenerService_OnTerminated);
            m_ListenerService.OnConnectionAccepted += new OnConnectionAcceptedEventHandler(ListenerService_OnConnectionAccepted);
        }

        /// <summary>
        /// Tries to start listener thread.
        /// </summary>
        private static void StartListenerThread()
        {
            try
            {
                if (m_ListenerThread != null)
                    m_ListenerThread = null;

                m_ListenerThread = new Thread(new ParameterizedThreadStart(m_ListenerService.Start));
                m_ListenerThread.Name = "ListenerThread";
                m_ListenerThread.Start(m_EnableFirewall);
                m_Active = true;
            }
            catch (Exception e)
            {
             //   Logger.Exception(e, "Failed to start listener thread");
            }
        }

        /// <summary>
        /// Executes after listener accepted new connection.
        /// </summary>
        /// <param name="socket">New connection socket.</param>
        private static void ListenerService_OnConnectionAccepted(Socket socket)
        {
            if (socket != null && socket.Connected)
            {
                UserConnection userConnection = new UserConnection(socket);

                if (m_ActiveConnections.Count >= 100)
                {
                   // userConnection.Send(LoginFailed.ToPacket(UserAuthenticationResponseType.ServerOverloaded));
                    CloseActiveConnection(userConnection);
                    return;
                }

                if (m_ActiveConnections.ContainsKey(userConnection.Session.ID))
                {
                    userConnection = null;
                    ListenerService_OnConnectionAccepted(socket);
                    return;
                }

                m_ActiveConnections.Add(userConnection.Session.ID, userConnection);

                //userConnection.Send(InitializeConnection.ToPacket(userConnection.Session)); // say hello to client

                userConnection.BeginReceive();
            }
        }

        /// <summary>
        /// Executes after listener was terminated.
        /// </summary>
        private static void ListenerService_OnTerminated()
        {
           // Logger.WriteLine(Source.OuterNetwork, "Network listener ({0}) terminated.", m_LocalEndPoint);
        }

        /// <summary>
        /// Executes after listener was stopped.
        /// </summary>
        private static void ListenerService_OnStopped()
        {
           // Logger.WriteLine(Source.OuterNetwork, "Network listener ({0}) stopped.", m_LocalEndPoint);
        }

        /// <summary>
        /// Executes after listener was started.
        /// </summary>
        private static void ListenerService_OnStarted()
        {
            //Logger.WriteLine(Source.OuterNetwork, "User connections listener initialized on {0}", m_LocalEndPoint);
        }

        /// <summary>
        /// Stops listener.
        /// </summary>
        private static void StopListener()
        {
            if (m_ListenerService != null && m_ListenerService.Active)
                m_ListenerService.Stop();

            m_Active = false;
        }

        /// <summary>
        /// Stops listener thread.
        /// </summary>
        private static void StopListenerThread()
        {
            if (m_ListenerThread != null && m_ListenerThread.IsAlive)
                m_ListenerThread.Abort();
        }

        /// <summary>
        /// Removes provided <see cref="UserConnection"/> object from active connections list.
        /// </summary>
        /// <param name="connection"><see cref="UserConnection"/> object to remove from active connections list.</param>
        internal static void RemoveFromActiveConnections(UserConnection connection)
        {
            if (connection != null)
            {
                //if (connection.Session.AccountID > 0)
                //    CacheServiceConnection.Send(new UnCacheUser(connection.Session.ID).ToPacket());

                m_ActiveConnections.Remove(connection.Session.ID);
                connection = null;
            }
        }

        /// <summary>
        ///Closes active <see cref="UserConnection"/> but not requests cache to drop user session ( when going to world ).
        /// </summary>
        /// <param name="connection"><see cref="UserConnection"/> object to remove from active connections list.</param>
        internal static void CloseConnectionWithoutLogout(UserConnection connection)
        {
            if (connection != null)
            {
                connection.CloseConnection();
                m_ActiveConnections.Remove(connection.Session.ID);
                connection = null;
            }
        }

        /// <summary>
        /// Closes active <see cref="UserConnection"/>.
        /// </summary>
        /// <param name="connection"><see cref="UserConnection"/> object, which network must be closed.</param>
        internal static void CloseActiveConnection(UserConnection connection)
        {
            if (connection != null)
            {
                connection.CloseConnection();

                //if (connection.Session.AccountID > 0)
                //    CacheServiceConnection.Send(new UnCacheUser(connection.Session.ID).ToPacket());

                m_ActiveConnections.Remove(connection.Session.ID);

                //Logger.WriteLine(Source.OuterNetwork, "Connection closed for session {0}.", connection.Session.ToString());

                connection = null;
            }
        }

        /// <summary>
        /// Closes all active <see cref="UserConnection"/>s.
        /// </summary>
        internal static void CloseAllConnections()
        {
            foreach (UserConnection connection in m_ActiveConnections.Values)
            {
                connection.CloseConnection();

                //if (connection.Session.AccountID > 0)
                //    CacheServiceConnection.Send(new UnCacheUser(connection.Session.ID).ToPacket());
            }

            m_ActiveConnections.Clear();
        }

        /// <summary>
        /// Gets or sets value, that indicates, if user connections listener is listening now.
        /// </summary>
        internal static bool Active
        {
            get { return m_Active; }
            set { m_Active = value; }
        }
    }
}
