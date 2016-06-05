using L2dotNET.GameService.network.l2send;

namespace L2dotNET.GameService.tables.admin
{
    class AA_whisper : _adminAlias
    {
        public AA_whisper()
        {
            cmd = "whisper";
        }

        protected internal override void use(L2Player admin, string alias)
        {
            //whisper [on|off] -- so when whisper is off means no one can message you when whispers on they can message you.
            bool changed = false;
            switch (alias.Split(' ')[1])
            {
                case "on":
                    changed = admin.WhieperBlock = false;
                    admin.WhieperBlock = true;
                    admin.sendMessage("Whisper blocking enabled.");
                    break;
                case "off":
                    changed = admin.WhieperBlock = true;
                    admin.WhieperBlock = false;
                    admin.sendMessage("Whisper blocking disabled.");
                    break;
            }

            if (changed)
                admin.sendPacket(new EtcStatusUpdate(admin));
        }
    }
}