using L2.Net.LoginService.Crypt;
using L2dotNET.Auth.Utils;
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
    /// Represents user connection class.
    /// </summary>
    internal sealed class UserConnection : NetworkClient
    {
        /// <summary>
        /// Provides access to user session data.
        /// </summary>
        internal UserSession Session;

        /// <summary>
        /// Crypt object.
        /// </summary>
        private NewCrypt m_Crypt;

        /// <summary>
        /// RSA object.
        /// </summary>
        private RSAManaged m_RSADecryptor;

        /// <summary>
        /// User login.
        /// </summary>
        internal string Login = String.Empty;

        /// <summary>
        /// User password.
        /// </summary>
        internal string Password = String.Empty;

        /// <summary>
        /// Initializes new instance of <see cref="UserConnection"/> class.
        /// </summary>
        /// <param name="socket">Connection <see cref="Socket"/>.</param>
        internal UserConnection(Socket socket)
            : base(socket)
        {
            Session = InitializeSession(((IPEndPoint)socket.RemoteEndPoint).Address.ToString());
            m_Crypt = new NewCrypt(Session.BlowfishKey);
        }

        /// <summary>
        /// Begins receiving data from client.
        /// </summary>
        public override void BeginReceive()
        {
            m_Socket.BeginReceive(m_ReceiveBuffer, 0, 2, 0, m_ReceiveCallback, null);
        }

        /// <summary>
        /// Handles incoming packet.
        /// </summary>
        /// <param name="packet">Incoming packet.</param>
        protected override void Handle(Packet packet)
        {
            //Logger.WriteLine(Source.OuterNetwork, "Received: {0}", packet.ToString());

            //ELFO to be uncommented once everything is in place
            //if (!CacheServiceConnection.Active) // validate if login service is active
            //{
            //    Send(LoginFailed.ToPacket(UserAuthenticationResponseType.ServerMaintenance));
            //    UserConnectionsListener.CloseActiveConnection(this);
            //    return;
            //}

            if (QueuedRequestsPool.HasRequest(this, true)) // validate if user is awaiting response from cache service
                return;

            switch (packet.FirstOpcode)
            {
                //ELFO to be uncommented once everything is in place
                //case 0x07:
                //    {
                //        Send(ResponseAuthGameGuard.Static);
                //        return;
                //    }
                //case 0x00:
                //    {
                //        m_RSADecryptor = new RSAManaged();
                //        m_RSADecryptor.ImportParameters(UserConnectionsListener.PrivateKey);

                //        // get login and password
                //        unsafe
                //        {
                //            byte[] bytes = new byte[0x80];

                //            fixed (byte* buf = bytes, src = packet.GetBuffer())
                //                L2Buffer.Copy(src, 0x01, buf, 0x00, 0x80);

                //            fixed (byte* buf = m_RSADecryptor.DecryptValue(bytes))
                //            {
                //                L2Buffer.GetTrimmedString(buf, 0x03, ref Login, 0x0e);
                //                L2Buffer.GetTrimmedString(buf, 0x11, ref Password, 0x10);
                //            }
                //        }

                //        // validate user login
                //        if (!Utils.IsValidUserLogin(Login))
                //        {
                //            Send(LoginFailed.ToPacket(UserAuthenticationResponseType.UserOrPasswordWrong));
                //            return;
                //        }

                //        //Password = Utils.HashPassword(Password);

                //        Session.AccountName = Login;

                //        Logger.WriteLine(Session.ToString());

                //        long requestId = long.MinValue;

                //        // request cache to auth user
                //        if (QueuedRequestsPool.Enqueue(this, ref requestId))
                //            CacheServiceConnection.Send(new UserAuthenticationRequest(requestId, Login, Password, Session.ID).ToPacket());
                //        else
                //        {
                //            Logger.WriteLine(Source.InnerNetwork, "Failed to send UserAuthenticationRequest to cache service, request was not enqueued by QueuedRequestsPool ?...");
                //            Send(LoginFailed.ToPacket(UserAuthenticationResponseType.SystemError));
                //            UserConnectionsListener.CloseActiveConnection(this);
                //        }

                //        return;
                //    }
                //case 0x05:
                //    {
                //        int login1 = packet.ReadInt();
                //        int login2 = packet.ReadInt();

                //        if (login1 != Session.Login1 || login2 != Session.Login2)
                //        {
                //            Logger.WriteLine(Source.OuterNetwork, "Invalid UserSession data: {0}. BAN!", Session.ToString());
                //            CacheServiceConnection.Send(new UnCacheUser(Session.ID).ToPacket());
                //            UserConnectionsListener.CloseActiveConnection(this);
                //        }
                //        else
                //        {
                //            long requestID = long.MinValue;

                //            if (QueuedRequestsPool.Enqueue(this, ref requestID))
                //                CacheServiceConnection.Send(new WorldsListRequest(requestID).ToPacket());
                //            else
                //            {
                //                Logger.WriteLine(Source.InnerNetwork, "Failed to send WorldsListRequest to cache service, request was not enqueued by QueuedRequestsPool ?...");
                //                UserConnectionsListener.CloseActiveConnection(this);
                //            }
                //        }

                //        return;
                //    }
                //case 0x02:
                //    {
                //        // skip not needed data
                //        packet.MoveOffset(8);

                //        long requestID = long.MinValue;

                //        if (QueuedRequestsPool.Enqueue(this, ref requestID))
                //            CacheServiceConnection.Send(new JoinWorldRequest(requestID, Session.ID, packet.ReadByte()).ToPacket());
                //        else
                //        {
                //            Logger.WriteLine(Source.InnerNetwork, "Failed to send JionWorldRequest to cache service, request was not enqueued by QueuedRequestsPool ?...");
                //            UserConnectionsListener.CloseActiveConnection(this);
                //        }

                //        return;
                //    }
            }

            //Logger.WriteLine(Source.OuterNetwork, "Unknown packet received: {0}", packet.ToString());
            UserConnectionsListener.CloseActiveConnection(this);
        }

        /// <summary>
        /// Not needed here.
        /// </summary>
        /// <param name="buffer">Received buffer.</param>
        /// <param name="length">Received buffer length.</param>
        public override unsafe void ReceiveData(byte[] buffer, int length)
        {
            throw new System.InvalidOperationException();
        }

        /// <summary>
        /// Sends packet to client.
        /// </summary>
        /// <param name="p"><see cref="Packet"/> to send.</param>
        internal void Send(Packet p)
        {
            p.Prepare(sizeof(short));
            //Logger.WriteLine(Source.OuterNetwork, "Sending: \r\n{0}", p.ToString());
            SendData(p.GetBuffer());
        }

        /// <summary>
        /// Receive method.
        /// </summary>
        protected override unsafe void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                m_ReceivedLength += m_Socket.EndReceive(ar);

                fixed (byte* buf = m_ReceiveBuffer)
                {
                    if (!m_HeaderReceived) //get packet capacity
                    {
                        L2Buffer.Extend(ref m_ReceiveBuffer, 0, *((short*)(buf)) - sizeof(short));
                        m_ReceivedLength = 0;
                        m_HeaderReceived = true;
                    }

                    if (m_ReceivedLength == m_ReceiveBuffer.Length) // all data received
                    {
                        m_ReceiveBuffer = m_Crypt.Decrypt(m_ReceiveBuffer);
                        Handle(new Packet(1, m_ReceiveBuffer));
                        m_ReceivedLength = 0;
                        m_ReceiveBuffer = m_DefaultBuffer;
                        m_HeaderReceived = false;

                        m_Socket.BeginReceive(m_ReceiveBuffer, 0, 2, 0, ReceiveCallback, null);
                    }
                    else if (m_ReceivedLength < m_ReceiveBuffer.Length) // not all data received
                        m_Socket.BeginReceive(m_ReceiveBuffer, m_ReceivedLength, m_ReceiveBuffer.Length - m_ReceivedLength, 0, m_ReceiveCallback, null);
                    else
                        throw new InvalidOperationException();
                }
            }
            catch (Exception e)
            {
                if (e is NullReferenceException)  // user closed connection
                {
                    UserConnectionsListener.RemoveFromActiveConnections(this);
                    return;
                }

                //Logger.Exception(e);
            }
        }

        /// <summary>
        /// Initializes new <see cref="UserSession"/> object.
        /// </summary>
        /// <returns>New <see cref="UserSession"/> object.</returns>
        private static UserSession InitializeSession(string ipAddress)
        {
            return new UserSession()
            {
                ID = L2Random.Next(),
                IPAddress = ipAddress,
                Login1 = L2Random.Next(),
                Login2 = L2Random.Next(),
                Play1 = L2Random.Next(),
                Play2 = L2Random.Next(),
                BlowfishKey = L2Random.NextBytes(16),
                StartTime = DateTime.Now,
                LastWorld = 1
            };
        }
    }
}
