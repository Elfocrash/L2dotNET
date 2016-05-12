using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L2dotNET
{
    /// <summary>
    /// Represents <see cref="UserSession"/> structure.
    /// </summary>
    public struct UserSession
    {
        /// <summary>
        /// Null <see cref="UserSession"/> object.
        /// </summary>
        public static readonly UserSession Null = new UserSession()
        {
            ID = 0,
            AccountID = -1,
            AccountName = null
        };

        /// <summary>
        /// User <see cref="UserSession"/> unique id.
        /// </summary>
        public int ID;

        /// <summary>
        /// User ip-address.
        /// </summary>
        public string IPAddress;

        /// <summary>
        /// Play 1 key.
        /// </summary>
        public int Play1;

        /// <summary>
        /// Play 2 key.
        /// </summary>
        public int Play2;

        /// <summary>
        /// Login 1 key.
        /// </summary>
        public int Login1;

        /// <summary>
        /// Login 2 key.
        /// </summary>
        public int Login2;

        /// <summary>
        /// User account name.
        /// </summary>
        public string AccountName;

        /// <summary>
        /// User account unique id.
        /// </summary>
        public int AccountID;

        /// <summary>
        /// <see cref="UserSession"/> blowfish key.
        /// </summary>
        public byte[] BlowfishKey;

        /// <summary>
        /// <see cref="UserSession"/> creation time.
        /// </summary>
        public DateTime StartTime;

        /// <summary>
        /// World, in which user is playing now.
        /// </summary>
        public byte LastWorld;

        /// <summary>
        /// Returns string representation of <see cref="UserSession"/> structure.
        /// </summary>
        /// <returns>String representation of <see cref="UserSession"/> structure.</returns>
        public override string ToString()
        {
            return String.IsNullOrEmpty(AccountName) ?
                String.Format("Session id: {0} ({1})", ID, IPAddress) :
                String.Format("Session id: {0} ({1}), Account id: {2}, Account name {3}", ID, IPAddress, AccountID, AccountName);
        }

        /// <summary>
        /// Determines two sessions equality.
        /// </summary>
        /// <param name="one">One <see cref="UserSession"/> object.</param>
        /// <param name="other">Other <see cref="UserSession"/> object.</param>
        /// <returns>True, if compared <see cref="UserSession"/>s are equal, otherwise false.</returns>
        public static bool operator ==(UserSession one, UserSession other)
        {
            return one.ID == other.ID &&
                   one.AccountID == other.AccountID &&
                   one.AccountName == other.AccountName;
        }

        /// <summary>
        /// Determines two sessions inequality.
        /// </summary>
        /// <param name="one">One <see cref="UserSession"/> object.</param>
        /// <param name="other">Other <see cref="UserSession"/> object.</param>
        /// <returns>True, if compared <see cref="UserSession"/>s are not equal, otherwise false.</returns>
        public static bool operator !=(UserSession one, UserSession other)
        {
            return !(one == other);
        }

        /// <summary>
        /// Returns a value indicating whether this instance is equal to a specified <see cref="UserSession"/>.
        /// </summary>
        /// <param name="obj"> An object to compare with this instance.</param>
        /// <returns>True if obj is an instance of <see cref="UserSession"/> and equals the value of this instance, otherwise false.</returns>
        public override bool Equals(object obj)
        {
            return obj is UserSession && ((UserSession)obj == this);
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
