using System.Collections.Generic;
using L2dotNET.GameService.Templates;

namespace L2dotNET.GameService.network.serverpackets
{
    class CharTemplates : GameServerNetworkPacket
    {
        private readonly List<PcTemplate> _templates;

        public CharTemplates(List<PcTemplate> templates)
        {
            _templates = templates;
        }

        protected internal override void write()
        {
            writeC(0x17);
            writeD(_templates.Count);

            foreach (PcTemplate t in _templates)
            {
                if (t == null)
                    break;

                writeD((int)t.ClassId.ClassRace); //race id
                writeD((int)t.ClassId.Id);
                writeD(0x46);
                writeD(t.BaseSTR);
                writeD(0x0a);
                writeD(0x46);
                writeD(t.BaseDEX);
                writeD(0x0a);
                writeD(0x46);
                writeD(t.BaseCON);
                writeD(0x0a);
                writeD(0x46);
                writeD(t.BaseINT);
                writeD(0x0a);
                writeD(0x46);
                writeD(t.BaseWIT);
                writeD(0x0a);
                writeD(0x46);
                writeD(t.BaseMEN);
                writeD(0x0a);
            }
        }
    }
}