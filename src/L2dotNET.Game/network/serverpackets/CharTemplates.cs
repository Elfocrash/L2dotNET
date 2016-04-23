using System.Collections.Generic;
using L2dotNET.Game.staticf;

namespace L2dotNET.Game.network.l2send
{
    class CharTemplates : GameServerNetworkPacket
    {
        private List<pc_parameter> _templates;
        public CharTemplates(List<pc_parameter> templates)
        {
            _templates = templates;
        }

        protected internal override void write()
        {
            writeC(0x17);
            writeD(_templates.Count);

            foreach (pc_parameter t in _templates)
            {
                writeD(t.base_race_id);
                writeD(t.base_class_id);
                writeD(0x46);
                writeD(t.baseSTR);
                writeD(0x0a);
                writeD(0x46);
                writeD(t.baseDEX);
                writeD(0x0a);
                writeD(0x46);
                writeD(t.baseCON);
                writeD(0x0a);
                writeD(0x46);
                writeD(t.baseINT);
                writeD(0x0a);
                writeD(0x46);
                writeD(t.baseWIT);
                writeD(0x0a);
                writeD(0x46);
                writeD(t.baseMEN);
                writeD(0x0a);
            }
        }
    }
}
