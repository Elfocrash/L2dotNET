using System.Collections.Generic;
using System.Linq;
using L2dotNET.GameService.Templates;

namespace L2dotNET.GameService.Network.Serverpackets
{
    class CharTemplates : GameserverPacket
    {
        private readonly List<PcTemplate> _templates;

        public CharTemplates(List<PcTemplate> templates)
        {
            _templates = templates;
        }

        protected internal override void Write()
        {
            WriteByte(0x17);
            WriteInt(_templates.Count);

            foreach (PcTemplate t in _templates.TakeWhile(t => t != null))
            {
                WriteInt((int)t.ClassId.ClassRace); //race id
                WriteInt((int)t.ClassId.Id);
                WriteInt(0x46);
                WriteInt(t.BaseStr);
                WriteInt(0x0a);
                WriteInt(0x46);
                WriteInt(t.BaseDex);
                WriteInt(0x0a);
                WriteInt(0x46);
                WriteInt(t.BaseCon);
                WriteInt(0x0a);
                WriteInt(0x46);
                WriteInt(t.BaseInt);
                WriteInt(0x0a);
                WriteInt(0x46);
                WriteInt(t.BaseWit);
                WriteInt(0x0a);
                WriteInt(0x46);
                WriteInt(t.BaseMen);
                WriteInt(0x0a);
            }
        }
    }
}