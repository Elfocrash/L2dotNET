namespace L2dotNET.GameService.Model.Skills2
{
    public class SkillLevelParam
    {
        public string Pch;
        public byte Type;
        public byte Id;

        public SkillLevelParam(string pch, byte type, byte id)
        {
            Pch = pch;
            Type = type;
            Id = id;
        }
    }
}