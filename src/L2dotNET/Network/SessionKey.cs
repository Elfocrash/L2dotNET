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
    }
}