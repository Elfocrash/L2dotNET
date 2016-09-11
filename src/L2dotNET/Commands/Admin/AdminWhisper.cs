using L2dotNET.Attributes;
using L2dotNET.model.player;
using L2dotNET.Network.serverpackets;

namespace L2dotNET.Commands.Admin
{
    [AdminCommand(CommandName = "whisper")]
    class AdminWhisper : AAdminCommand
    {
        protected internal override void Use(L2Player admin, string alias)
        {
            //whisper [on|off] -- so when whisper is off means no one can message you when whispers on they can message you.
            bool changed;
            switch (alias.Split(' ')[1])
            {
                case "on":
                    changed = admin.WhisperBlock = false;
                    admin.WhisperBlock = true;
                    admin.SendMessage("Whisper blocking enabled.");
                    break;
                case "off":
                    changed = admin.WhisperBlock = true;
                    admin.WhisperBlock = false;
                    admin.SendMessage("Whisper blocking disabled.");
                    break;
                default:
                    changed = admin.WhisperBlock = true;
                    admin.WhisperBlock = false;
                    admin.SendMessage("Whisper blocking disabled.");
                    break;
            }

            if (changed)
                admin.SendPacket(new EtcStatusUpdate(admin));
        }
    }
}