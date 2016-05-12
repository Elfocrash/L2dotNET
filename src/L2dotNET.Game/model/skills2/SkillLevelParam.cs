
namespace L2dotNET.GameService.model.skills2
{
    public class SkillLevelParam
    {
        public string pch;
        public byte type;
        public byte id;

        public SkillLevelParam(string pch, byte type, byte id)
        {
            this.pch = pch;
            this.type = type;
            this.id = id;
        }
    }
}
