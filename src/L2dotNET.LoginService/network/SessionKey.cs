using System;
//using L2dotNET.Config;

namespace L2dotNET.LoginService.Network
{
    public class SessionKey
    {
        public readonly int SessionId;
        public readonly int LoginOkID1;
        public readonly int LoginOkID2;
        public readonly int PlayOkID1;
        public readonly int PlayOkID2;

        public SessionKey()
        {
            Random rnd = new Random();
            SessionId = rnd.Next();
            LoginOkID1 = rnd.Next();
            LoginOkID2 = rnd.Next();
            PlayOkID1 = rnd.Next();
            PlayOkID2 = rnd.Next();
        }

        public bool CheckLoginOKIdPair(int loginOkID1, int loginOkID2)
        {
            return (LoginOkID1 == loginOkID1) && (LoginOkID2 == loginOkID2);
        }

        /**
         * Only checks the PlayOk part of the session key if server doesnt show the
         * licence when player logs in.
         * 
         * @param key
         * @return true if keys are equal.
         */
        //public bool Equals(SessionKey key)
        //{
        //    // when server doesnt show licence it deosnt send the LoginOk packet,
        //    // client doesnt have this part of the key then.
        //    if (LoginConfig.LoginServer.ShowLicence)
        //        return (PlayOkID1 == key.PlayOkID1) && (LoginOkID1 == key.LoginOkID1) && (PlayOkID2 == key.PlayOkID2) && (LoginOkID2 == key.LoginOkID2);

        //    return (PlayOkID1 == key.PlayOkID1) && (PlayOkID2 == key.PlayOkID2);
        //}
    }
}
