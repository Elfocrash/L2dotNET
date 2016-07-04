using System.Collections.Generic;
using L2dotNET.GameService.Templates;

namespace L2dotNET.GameService.Network.Serverpackets
{
    class CharTemplates : GameServerNetworkPacket
    {
        private readonly List<PcTemplate> _templates;

        public CharTemplates(List<PcTemplate> templates)
        {
            _templates = templates;
        }

        protected internal override void Write()
        {
            WriteC(0x17);
            WriteD(_templates.Count);

            foreach (PcTemplate t in _templates)
            {
                if (t == null)
                    break;

                WriteD((int)t.ClassId.ClassRace); //race id
                WriteD((int)t.ClassId.Id);
                WriteD(0x46);
                WriteD(t.BaseStr);
                WriteD(0x0a);
                WriteD(0x46);
                WriteD(t.BaseDex);
                WriteD(0x0a);
                WriteD(0x46);
                WriteD(t.BaseCon);
                WriteD(0x0a);
                WriteD(0x46);
                WriteD(t.BaseInt);
                WriteD(0x0a);
                WriteD(0x46);
                WriteD(t.BaseWit);
                WriteD(0x0a);
                WriteD(0x46);
                WriteD(t.BaseMen);
                WriteD(0x0a);
            }
        }
    }
}