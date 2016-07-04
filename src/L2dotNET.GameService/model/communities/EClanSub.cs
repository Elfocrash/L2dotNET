namespace L2dotNET.GameService.Model.Communities
{
    public class EClanSub
    {
        public EClanType Type;
        public int LeaderId;
        public string LeaderName = "";
        public bool Enabled;
        public string Name;

        public EClanSub(EClanType eClanType)
        {
            Type = eClanType;
            Enabled = true;
        }
    }
}