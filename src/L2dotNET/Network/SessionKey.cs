namespace L2dotNET.Network
{
    public class SessionKey
    {
        public int PlayOkId1 { get; set; }
        public int PlayOkId2 { get; set; }
        public int LoginOkId1 { get; set; }
        public int LoginOkId2 { get; set; }

        public SessionKey(int loginOk1, int loginOk2, int playOk1, int playOk2)
        {
            PlayOkId1 = playOk1;
            PlayOkId2 = playOk2;
            LoginOkId1 = loginOk1;
            LoginOkId2 = loginOk2;
        }

        public override string ToString()
        {
            return $"PlayOk: {PlayOkId1} {PlayOkId2} LoginOk: {LoginOkId1} {LoginOkId2}";
        }

        public static bool operator ==(SessionKey key1, SessionKey key2)
        {
            return !ReferenceEquals(key1, null) && !ReferenceEquals(key2, null)
                   && key1.LoginOkId1 == key2.LoginOkId1
                   && key1.LoginOkId2 == key2.LoginOkId2
                   && key1.PlayOkId1 == key2.PlayOkId1
                   && key1.PlayOkId2 == key2.PlayOkId2;
        }

        public static bool operator !=(SessionKey key1, SessionKey key2)
        {
            return !(key1 == key2);
        }
    }
}