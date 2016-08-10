namespace L2dotNET.model.communities
{
    public class EClanSub
    {
        public EClanType Type;
        public int LeaderId;
        public string LeaderName = string.Empty;
        public bool Enabled;
        public string Name;

        public EClanSub(EClanType eClanType)
        {
            Type = eClanType;
            Enabled = true;
        }
    }
}