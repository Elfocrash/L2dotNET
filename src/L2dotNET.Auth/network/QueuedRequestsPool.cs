using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L2dotNET.Auth.network
{
    /// <summary>
    /// <see cref="QueuedRequest"/>s collection.
    /// </summary>
    internal static class QueuedRequestsPool
    {
        private static long m_NextRequestID = long.MinValue + 1;
        private static SortedDictionary<int, QueuedRequest> m_RequestsBySession = new SortedDictionary<int, QueuedRequest>();
        private static SortedDictionary<long, int> m_RequestsById = new SortedDictionary<long, int>();

        /// <summary>
        /// Enqueues new <see cref="QueuedRequest"/> to requests pool.
        /// </summary>
        /// <param name="userConnection"><see cref="UserConnection"/> object to create request for.</param>
        /// <param name="requestId">Request unique id reference.</param>
        /// <returns>True, if request was added, otherwise  false.</returns>
        internal static bool Enqueue(UserConnection userConnection, ref long requestId)
        {
            if (m_RequestsBySession.ContainsKey(userConnection.Session.ID))
            {
#if DropUnAnsweredPackets
                m_RequestsBySession.Remove(userConnection.Session.ID);
                Logger.WriteLine(Source.InnerNetwork, "Previous request was dropped for session {0}", userConnection.Session.ToString());
#else
                //Logger.WriteLine(Source.InnerNetwork, "Can't add new request for session {0}, there is other unanswered request.", userConnection.Session.ToString());
                return false;
#endif
            }

            QueuedRequest request = new QueuedRequest(NextRequestID(), userConnection);
            requestId = request.RequestID;

            m_RequestsBySession.Add(userConnection.Session.ID, request);
            m_RequestsById.Add(request.RequestID, userConnection.Session.ID);

            return true;
        }

        /// <summary>
        /// Indicates if <see cref="QueuedRequestsPool"/> contains request from provided <see cref="UserConnection"/> object.
        /// </summary>
        /// <param name="connection"><see cref="UserConnection"/> to search requests from.</param>
        /// <param name="dropIfExists">If true, existing request will be dropped.</param>
        /// <returns>True, if there is <see cref="QueuedRequest"/> from from provided <see cref="UserConnection"/>, otherwise false.</returns>
        internal static bool HasRequest(UserConnection connection, bool dropIfExists)
        {
            if (m_RequestsBySession.ContainsKey(connection.Session.ID))
            {
                if (dropIfExists)
                    Dequeue(connection.Session.ID);
                else
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Dequeues <see cref="QueuedRequest"/> from requests pool.
        /// </summary>
        /// <param name="sessionID">Connection session unique id.</param>
        /// <returns><see cref="QueuedRequest"/> if it was found in pool, otherwise QueuedRequest.NotFound instance.</returns>
        internal static QueuedRequest Dequeue(int sessionID)
        {
            if (m_RequestsBySession.ContainsKey(sessionID))
            {
                QueuedRequest request = m_RequestsBySession[sessionID];
                m_RequestsBySession.Remove(sessionID);
                m_RequestsById.Remove(request.RequestID);
                return request;
            }

            return QueuedRequest.NotFound;
        }

        /// <summary>
        /// Dequeues <see cref="QueuedRequest"/> from requests pool.
        /// </summary>
        /// <param name="requestId"><see cref="QueuedRequest"/> unique id.</param>
        /// <returns><see cref="QueuedRequest"/> if it was found in pool, otherwise <see cref="QueuedRequest.NotFound"/>.</returns>
        internal static QueuedRequest Dequeue(long requestId)
        {
            if (m_RequestsById.ContainsKey(requestId))
            {
                QueuedRequest request = m_RequestsBySession[m_RequestsById[requestId]];
                m_RequestsById.Remove(requestId);
                m_RequestsBySession.Remove(request.UserConnection.Session.ID);
                return request;
            }

            return QueuedRequest.NotFound;
        }

        /// <summary>
        /// Gets next unique id for <see cref="QueuedRequest"/>.
        /// </summary>
        /// <returns>Next request unique id.</returns>
        private static long NextRequestID()
        {
            if (m_NextRequestID == long.MaxValue)
                m_NextRequestID = long.MinValue;

            m_NextRequestID++;

            if (m_RequestsById.ContainsKey(m_NextRequestID)) // verify that id doesn'thread exist in the pool
                return NextRequestID();

            return m_NextRequestID;
        }
    }
}
