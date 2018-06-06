using System;
using L2dotNET.Attributes;
using L2dotNET.Models.Player;
using L2dotNET.Network.serverpackets;

namespace L2dotNET.Commands.Admin
{
    [Command(CommandName = "whisper")]
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
                    admin.SendMessageAsync("Whisper blocking enabled.");
                    break;
                case "off":
                    changed = admin.WhisperBlock = true;
                    admin.WhisperBlock = false;
                    admin.SendMessageAsync("Whisper blocking disabled.");
                    break;
                default:
                    changed = admin.WhisperBlock = true;
                    admin.WhisperBlock = false;
                    admin.SendMessageAsync("Whisper blocking disabled.");
                    break;
            }

            if (changed)
                admin.SendPacketAsync(new EtcStatusUpdate(admin));
        }

        public AdminWhisper(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
    }
}