namespace L2dotNET.GameService.Model.Skills2
{
    public class SkillLevelParam
    {
        public string Pch;
        public byte Type;
        public byte Id;

        public SkillLevelParam(string pch, byte type, byte id)
        {
            this.Pch = pch;
            this.Type = type;
            this.Id = id;
        }
    }
}