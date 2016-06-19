using System.Net.Sockets;

namespace L2dotNET.Network
{
    /// <summary>
    /// <see cref="Firewall"/> enabled action delegate.
    /// .</summary>
    public delegate void OnFirewallEnabledEventHandler();

    /// <summary>
    /// <see cref="Firewall"/> disabled action delegate.
    /// .</summary>
    public delegate void OnFirewallDisabledEventHandler();

    /// <summary>
    /// <see cref="Firewall"/> bypass rejected action delegate.
    /// .</summary>
    /// <param name="socket">Connected <see cref="Socket"/> object.</param>
    public delegate void OnFirewallBypassRejectedEventHandler(Socket socket);

    /// <summary>
    /// <see cref="Firewall"/> bypass allowed action delegate.
    /// .</summary>
    /// <param name="socket">Connected <see cref="Socket"/> object.</param>
    public delegate void OnFirewallBypassAllowedEventHandler(Socket socket);

    /// <summary>
    /// Represents simple firewall class.
    /// .</summary>
    public class Firewall
    {
        /// <summary>
        /// Indicates if <see cref="Firewall"/> is currently enabled.
        /// .</summary>
        private volatile bool m_Enabled;

        /// <summary>
        /// Raises after firewall was enabled.
        /// .</summary>
        public event OnFirewallEnabledEventHandler OnEnabled;

        /// <summary>
        /// Raises after firewall was disabled.
        /// .</summary>
        public event OnFirewallDisabledEventHandler OnDisabled;

        /// <summary>
        /// Raises after firewall allowed socket.
        /// .</summary>
        public event OnFirewallBypassAllowedEventHandler OnBypassAllowed;

        /// <summary>
        /// Raises after firewall rejected socket.
        /// .</summary>
        public event OnFirewallBypassRejectedEventHandler OnBypassRejected;

        /// <summary>
        /// Validates socket connection. Note: if firewall is disabled and provided socket object is not null, always allows connection.
        /// .</summary>
        /// <param name="socket"><see cref="Socket"/> to validate.</param>
        /// <returns>True, if socket is valid, otherwise false.</returns>
        public virtual bool ValidateRequest(Socket socket)
        {
            if ((socket == null) || !socket.Connected)
                return false;

            if (!m_Enabled)
                return true;

            if (ValidateRules(socket))
            {
                if (OnBypassAllowed != null)
                    OnBypassAllowed(socket);
                return true;
            }

            if (OnBypassRejected != null)
                OnBypassRejected(socket);

            return false;
        }

        /// <summary>
        /// Checks that socket is valid for current rules collection.
        /// .</summary>
        /// <param name="socket"><see cref="Socket"/> object to validate.</param>
        /// <returns>True, if socket is valid, otherwise false.</returns>
        protected virtual bool ValidateRules(Socket socket)
        {
            return true;
        }

        /// <summary>
        /// Enables firewall.
        /// .</summary>
        public virtual void Enable()
        {
            m_Enabled = true;

            if (OnEnabled != null)
                OnEnabled();
        }

        /// <summary>
        /// Disables firewall.
        /// .</summary>
        public virtual void Disable()
        {
            m_Enabled = false;

            if (OnDisabled != null)
                OnDisabled();
        }

        /// <summary>
        /// Sets firewall enabled / disabled, gets it current state.
        /// .</summary>
        public bool Enabled
        {
            get { return m_Enabled; }
            set
            {
                if (m_Enabled != value)
                    if (value)
                        Enable();
                    else
                        Disable();
            }
        }
    }
}