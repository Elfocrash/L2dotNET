using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L2dotNET.Auth.network
{
    /// <summary>
    /// Queued inner network request structure.
    /// </summary>
    internal struct QueuedRequest
    {
        /// <summary>
        /// Request not found identifier.
        /// </summary>
        internal static readonly QueuedRequest NotFound = new QueuedRequest(long.MinValue, null);

        /// <summary>
        /// Queued request id.
        /// </summary>
        internal readonly long RequestID;

        /// <summary>
        /// <see cref="UserConnection"/> object.
        /// </summary>
        internal readonly UserConnection UserConnection;

        /// <summary>
        /// Initializes new instance of <see cref="QueuedRequest"/>.
        /// </summary>
        /// <param name="requestID"><see cref="QueuedRequest"/> unique id.</param>
        /// <param name="connection"><see cref="UserConnection"/> object.</param>
        internal QueuedRequest(long requestID, UserConnection connection)
        {
            RequestID = requestID;
            UserConnection = connection;
        }

        /// <summary>
        /// Determines two <see cref="QueuedRequest"/> objects equality.
        /// </summary>
        /// <param name="one">First <see cref="QueuedRequest"/> object.</param>
        /// <param name="other">Second <see cref="QueuedRequest"/> object.</param>
        /// <returns>True, if first <see cref="QueuedRequest"/> equals to second, otherwise false.</returns>
        public static bool operator ==(QueuedRequest one, QueuedRequest other)
        {
            return one.RequestID == other.RequestID;
        }

        /// <summary>
        /// Determines two <see cref="QueuedRequest"/> inequality.
        /// </summary>
        /// <param name="one">First <see cref="QueuedRequest"/> object.</param>
        /// <param name="other">Second <see cref="QueuedRequest"/> object.</param>
        /// <returns>True, if first <see cref="QueuedRequest"/> object doesn'thread equal to second, otherwise false.</returns>
        public static bool operator !=(QueuedRequest one, QueuedRequest other)
        {
            return !(one == other);
        }

        /// <summary>
        /// Validates provided <see cref="QueuedRequest"/>.
        /// </summary>
        /// <param name="request"><see cref="QueuedRequest"/> to validate.</param>
        /// <returns>True, if provided <see cref="QueuedRequest"/> is valid, otherwise false.</returns>
        internal static bool IsValid(QueuedRequest request)
        {
            return request != QueuedRequest.NotFound && request.UserConnection != null && request.UserConnection.Connected;
        }

        /// <summary>
        /// Sends provided <see cref="Packet"/> to requester.
        /// </summary>
        /// <param name="p"><see cref="Packet"/> to send.</param>
        internal void Send(Packet p)
        {
            if (UserConnection != null && UserConnection.Connected)
                UserConnection.Send(p);
        }

        /// <summary>
        /// Returns a value indicating whether this instance is equal to a specified <see cref="QueuedRequest"/>.
        /// </summary>
        /// <param name="obj"> An object to compare with this instance.</param>
        /// <returns>True if obj is an instance of <see cref="QueuedRequest"/> and equals the value of this instance, otherwise false.</returns>
        public override bool Equals(object obj)
        {
            return obj is QueuedRequest && ((QueuedRequest)obj == this);
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
