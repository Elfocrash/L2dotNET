namespace L2dotNET.GameService.Model.Communities
{
    public class e_ClanSub
    {
        public e_ClanType Type;
        public int LeaderID;
        public string LeaderName = "";
        public bool Enabled = false;
        public string Name;

        public e_ClanSub(e_ClanType e_ClanType)
        {
            this.Type = e_ClanType;
            Enabled = true;
        }
    }
}