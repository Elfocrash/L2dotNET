using L2dotNET.Game.network.l2send;
namespace L2dotNET.Game.tables.admin
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
                    changed = admin._whisperBlock = false;
                    admin._whisperBlock = true;
                    admin.sendMessage("Whisper blocking enabled.");
                    break;
                case "off":
                    changed = admin._whisperBlock = true;
                    admin._whisperBlock = false;
                    admin.sendMessage("Whisper blocking disabled.");
                    break;
            }

            if (changed)
                admin.sendPacket(new EtcStatusUpdate(admin));
        }
    }
}
