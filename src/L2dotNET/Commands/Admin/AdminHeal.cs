using System;
using System.Threading.Tasks;
using L2dotNET.Attributes;
using L2dotNET.DataContracts.Shared.Enums;
using L2dotNET.Models.Player;
using L2dotNET.Network.serverpackets;

namespace L2dotNET.Commands.Admin
{
    [Command(CommandName = "heal")]
    class AdminHeal : AAdminCommand
    {
        protected internal override async Task UseAsync(L2Player admin, string alias)
        {
            //healthy -- восстанавливает выбранному чару хп\мп

            L2Player target;
            if (admin.Target is L2Player)
                target = (L2Player)admin.Target;
            else
                target = admin;

            double hpval = target.MaxHp;
            double mpval = target.MaxMp;
            target.CharStatus.SetCurrentHp(target.MaxHp);
            target.CharStatus.SetCurrentMp(target.MaxMp);

            StatusUpdate su = new StatusUpdate(target);
            su.Add(StatusUpdate.CurHp, (int)target.CharStatus.CurrentHp);
            su.Add(StatusUpdate.CurMp, (int)target.CharStatus.CurrentMp);
            await target.SendPacketAsync(su);

            SystemMessage sm = new SystemMessage(SystemMessageId.S2HpRestoredByS1);
            sm.AddPlayerName(admin.Name);
            sm.AddNumber((int)hpval);
            await target.SendPacketAsync(sm);

            sm = new SystemMessage(SystemMessageId.S2MpRestoredByS1);
            sm.AddPlayerName(admin.Name);
            sm.AddNumber((int)mpval);
            await target.SendPacketAsync(sm);
        }

        public AdminHeal(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
    }
}