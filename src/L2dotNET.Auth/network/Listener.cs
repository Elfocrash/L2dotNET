using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace L2dotNET.Auth.network
{
    /// <summary>
    /// Listener start action delegate.
    /// </summary>
    public delegate void OnListenerStartedEventHandler();

    /// <summary>
    /// Listener stop action delegate.
    /// </summary>
    public delegate void OnListenerStoppedEventHandler();

    /// <summary>
    /// Listener termination action delegate.
    /// </summary>
    public delegate void OnListenerTerminatedEventHandler();

    /// <summary>
    /// Accept connection delegate.
    /// </summary>
    /// <param name="socket">Accepted socket.</param>
    public delegate void OnConnectionAcceptedEventHandler(Socket socket);

    /// <summary>
    /// Possible actions after some service error occurred.
    /// </summary>
    public enum OnErrorAction : byte
    {
        /// <summary>
        /// Ignore error.
        /// </summary>
        Throw,

        /// <summary>
        /// Terminate service.
        /// </summary>
        Terminate
    }

    /// <summary>
    /// Represents network listener class.
    /// </summary>
    public class Listener
    {
        /// <summary>
        /// Gets the number of connections, accepted by listener.
        /// </summary>
        public long AcceptedConnections;

        /// <summary>
        /// Local end point.
        /// </summary>
        public IPEndPoint LocalEndPoint;

        /// <summary>
        /// Listening connections count.
        /// </summary>
        protected int Backlog;

        /// <summary>
        /// Service <see cref="Socket"/> object.
        /// </summary>
        protected Socket ServiceSocket;

        /// <summary>
        /// Service <see cref="Firewall"/> object.
        /// </summary>
        protected Firewall Firewall;

        /// <summary>
        /// Indicates if listener is active.
        /// </summary>
        private bool m_Active;

        /// <summary>
        /// Raised after listener was started. 
        /// </summary>
        public event OnListenerStartedEventHandler OnStarted;

        /// <summary>
        /// Raised after listener was stopped.
        /// </summary>
        public event OnListenerStoppedEventHandler OnStopped;

        /// <summary>
        /// Raised after listener was terminated.
        /// </summary>
        public event OnListenerTerminatedEventHandler OnTerminated;

        /// <summary>
        /// Raised after new connection was accepted by listener.
        /// </summary>
        public event OnConnectionAcceptedEventHandler OnConnectionAccepted;

        /// <summary>
        /// Initializes new instance of <see cref="Listener"/> class.
        /// </summary>
        /// <param name="localEndPoint">Service local endpoint.</param>
        /// <param name="backlog">Service backlog.</param>
        public Listener(IPEndPoint localEndPoint, int backlog)
        {
            LocalEndPoint = localEndPoint;
            Backlog = backlog;

            Firewall = new Firewall();
            Firewall.OnEnabled += new OnFirewallEnabledEventHandler(Firewall_OnEnabled);
            Firewall.OnDisabled += new OnFirewallDisabledEventHandler(Firewall_OnDisabled);
        }

        /// <summary>
        /// Executes when firewall rejects socket.
        /// </summary>
        /// <param name="socket">Rejected <see cref="Socket"/> object.</param>
        public virtual void Firewall_OnBypassRejected(Socket socket)
        {
            if (socket != null && socket.Connected)
            {
#if DEBUG_FIREWALL
                Logger.WriteLine(Source.Firewall, "Rejecting connection from {0}", socket.RemoteEndPoint.ToString());
#endif
                socket.Shutdown(SocketShutdown.Both);
                socket = null;
            }
#if DEBUG_FIREWALL
            else
                Logger.WriteLine(Source.Firewall, "Null socket in Firewall_OnBypassRejected( Socket socket ) method.");
#endif
        }

        /// <summary>
        /// Executes when firewall allows socket activity.
        /// </summary>
        /// <param name="socket">Accepted <see cref="Socket"/> object.</param>
        public virtual void Firewall_OnBypassAllowed(Socket socket)
        {
#if DEBUG_FIREWALL
            Logger.WriteLine(Source.Firewall, "Firewall bypassed");
#endif
        }

        /// <summary>
        /// Enables listener firewall.
        /// </summary>
        public virtual void EnableFirewall()
        {
            Firewall.OnBypassAllowed += new OnFirewallBypassAllowedEventHandler(Firewall_OnBypassAllowed);
            Firewall.OnBypassRejected += new OnFirewallBypassRejectedEventHandler(Firewall_OnBypassRejected);
            Firewall.Enabled = true;
        }

        /// <summary>
        /// Disables listener firewall.
        /// </summary>
        public virtual void DisableFirewall()
        {
            Firewall.OnBypassAllowed -= new OnFirewallBypassAllowedEventHandler(Firewall_OnBypassAllowed);
            Firewall.OnBypassRejected -= new OnFirewallBypassRejectedEventHandler(Firewall_OnBypassRejected);
            Firewall.Enabled = false;
        }

        /// <summary>
        /// Executes when firewall was disabled.
        /// </summary>
        public virtual void Firewall_OnDisabled()
        {
            //Logger.WriteLine(Source.Firewall, "Warning: firewall is disabled for {0}", LocalEndPoint.ToString());
        }

        /// <summary>
        /// Executes when firewall was enabled.
        /// </summary>
        public virtual void Firewall_OnEnabled()
        {
            //Logger.WriteLine(Source.Firewall, "Firewall enabled for {0}", LocalEndPoint.ToString());
        }

        /// <summary>
        /// Starts listener activity.
        /// </summary>
        /// <param name="enableFirewall">If true, firewall will be enabled, otherwise not.</param>
        public virtual void Start(object enableFirewall)
        {
            if (LocalEndPoint == null)
                RaiseError(new ListenerErrorEventArgs(new ArgumentNullException(), OnErrorAction.Terminate));
            else if (ServiceSocket != null)
                RaiseError(new ListenerErrorEventArgs(new InvalidOperationException("Service already started.")));
            else
            {
                try
                {
                    if (enableFirewall is Boolean && (Boolean)enableFirewall)
                        EnableFirewall();

                    ServiceSocket = new Socket(LocalEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                    NetworkHelper.SetCommonSocketOptions(ref ServiceSocket);

                    ServiceSocket.Bind(LocalEndPoint);
                    ServiceSocket.Listen(Backlog < 1 || Backlog > 1000 ? 50 : Backlog);

                    DoBeginAccept();

                    m_Active = true;

                    if (OnStarted != null)
                        OnStarted();
                }
                catch (SocketException se)
                {
                    RaiseError(new ListenerErrorEventArgs(se, OnErrorAction.Terminate));
                }
                catch (Exception e)
                {
                    RaiseError(new ListenerErrorEventArgs(e)); // terminate or not ?
                }
            }
        }

        /// <summary>
        /// Start accepting next connection.
        /// </summary>
        protected virtual void DoBeginAccept()
        {
            ServiceSocket.BeginAccept(DoAcceptSocket, null);
        }

        /// <summary>
        /// Accepts new connection.
        /// </summary>
        /// <param name="ar">Begin accept async result.</param>
        protected virtual void DoAcceptSocket(IAsyncResult ar)
        {
            if (OnConnectionAccepted != null && m_Active)
            {
                Socket acceptedSocket = ServiceSocket.EndAccept(ar);

                if (acceptedSocket != null && Firewall.ValidateRequest(acceptedSocket))
                {
                    NetworkHelper.SetCommonSocketOptions(ref acceptedSocket);
                    OnConnectionAccepted(acceptedSocket);
                    IncrementAcceptedConnectionsCount(ref AcceptedConnections);
                }
#if DEBUG_SERVICE
                else
                    Logger.WriteLine(Source.Listener, "Null socket in DoAcceptSocket( IAsyncResult ar ) method.");
#endif
                if (m_Active)
                    DoBeginAccept();
            }
        }

        /// <summary>
        /// Stops listener activity and raises <see cref="OnStopped"/> event.
        /// </summary>
        public virtual void Stop()
        {
            Stop(true);
        }

        /// <summary>
        /// Stops listener activity.
        /// </summary>
        /// <param name="raiseOnStopEvent">If true, <see cref="OnStopped"/> event will be raised, otherwise will not.</param>
        protected virtual void Stop(bool raiseOnStopEvent)
        {
            if (ServiceSocket != null)
            {
                m_Active = false;

                ServiceSocket.Close();
                ServiceSocket = null;

                if (raiseOnStopEvent && OnStopped != null)
                    OnStopped();
            }
#if DEBUG_SERVICE
            else
                Console.WriteLine("Attempt to stop null listening socket in Stop( bool raiseOnStopEvent ) method.");
#endif
        }

        /// <summary>
        /// Terminates listener.
        /// </summary>
        public virtual void Terminate()
        {
            Stop(false);

            if (OnTerminated != null)
                OnTerminated();
        }

        /// <summary>
        /// Handles occurred error. For more information, see <see cref="ListenerErrorEventArgs"/>.
        /// </summary>
        /// <param name="e">For more information, see <see cref="ListenerErrorEventArgs"/>.</param>
        public void RaiseError(ListenerErrorEventArgs e)
        {
            if (e.Message != null)
                Console.WriteLine(e.Message);

            if (e.Exception != null)
            {
                Console.WriteLine(e.Exception.Message);
                Console.WriteLine(e.Exception.StackTrace);
            }

            switch (e.NextAction)
            {
                case OnErrorAction.Terminate:
                    {
                        Terminate();
                        break;
                    }
                default:
                    break;
            }
        }

        /// <summary>
        /// Indicates, if listener is active now.
        /// </summary>
        public bool Active
        {
            get { return m_Active; }
        }

        /// <summary>
        /// Gets string representation of listener status.
        /// </summary>
        public virtual string Status
        {
            get
            {
                return String.Format("Service status: {0}", m_Active ? String.Format("listening on {0}", LocalEndPoint.ToString()) : "inactive");
            }
        }

        /// <summary>
        /// Safely increments accepted connections count.
        /// </summary>
        /// <param name="acceptedConnections">Current accepted connections count.</param>
        private static void IncrementAcceptedConnectionsCount(ref long acceptedConnections)
        {
            if (acceptedConnections + 1 == long.MaxValue)
            {
                acceptedConnections = 0;
                Console.WriteLine("Accepted connections reset");
            }
            acceptedConnections++;
        }
    }

    /// <summary>
    /// Represents listener service error event arguments class.
    /// </summary>
    public class ListenerErrorEventArgs : EventArgs
    {
        /// <summary>
        /// Thrown exception.
        /// </summary>
        public readonly Exception Exception;

        /// <summary>
        /// Error message.
        /// </summary>
        public readonly string Message;

        /// <summary>
        /// <see cref="OnErrorAction"/> step to do after raising current error.
        /// </summary>
        public readonly OnErrorAction NextAction;

        /// <summary>
        /// Initializes new instance of <see cref="ListenerErrorEventArgs"/> class.
        /// </summary>
        /// <param name="message">Error message.</param>
        /// <param name="e">Thrown exception.</param>
        /// <param name="nextAction">Action, that will be done after raising current error.</param>
        public ListenerErrorEventArgs(string message, Exception e, OnErrorAction nextAction)
        {
            Message = message;
            Exception = e;
            NextAction = nextAction;
        }

        /// <summary>
        /// Initializes new instance of <see cref="ListenerErrorEventArgs"/> class. Next action : Throw.
        /// </summary>
        /// <param name="message">Error message.</param>
        public ListenerErrorEventArgs(string message) : this(message, null, OnErrorAction.Throw) { }

        /// <summary>
        /// Initializes new instance of <see cref="ListenerErrorEventArgs"/> class. Next action: Throw.
        /// </summary>
        /// <param name="e">Thrown exception.</param>
        public ListenerErrorEventArgs(Exception e) : this(null, e, OnErrorAction.Throw) { }

        /// <summary>
        /// Initializes new instance of <see cref="ListenerErrorEventArgs"/> class.
        /// </summary>
        /// <param name="message">Error message.</param>
        /// <param name="nextAction">Action, that will be done after raising current error.</param>
        public ListenerErrorEventArgs(string message, OnErrorAction nextAction) : this(message, null, nextAction) { }

        /// <summary>
        /// Initializes new instance of <see cref="ListenerErrorEventArgs"/> class.
        /// </summary>
        /// <param name="e">Thrown exception.</param>
        /// <param name="nextAction">Action, that will be done after raising current error.</param>
        public ListenerErrorEventArgs(Exception e, OnErrorAction nextAction) : this(null, e, nextAction) { }
    }
}
