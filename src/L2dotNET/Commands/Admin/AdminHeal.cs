using L2dotNET.Attributes;
using L2dotNET.model.player;
using L2dotNET.Network.serverpackets;

namespace L2dotNET.Commands.Admin
{
    [Command(CommandName = "heal")]
    class AdminHeal : AAdminCommand
    {
        protected internal override void Use(L2Player admin, string alias)
        {
            //healthy -- восстанавливает выбранному чару хп\мп

            L2Player target;
            if (admin.Target is L2Player)
                target = (L2Player)admin.Target;
            else
                target = admin;

            double hpval = target.MaxHp;
            double mpval = target.MaxMp;
            target.CurHp = target.MaxHp;
            target.CurMp = target.MaxMp;

            StatusUpdate su = new StatusUpdate(target.ObjId);
            su.Add(StatusUpdate.CurHp, (int)target.CurHp);
            su.Add(StatusUpdate.CurMp, (int)target.CurMp);
            target.SendPacket(su);

            SystemMessage sm = new SystemMessage(SystemMessage.SystemMessageId.S2HpRestoredByS1);
            sm.AddPlayerName(admin.Name);
            sm.AddNumber((int)hpval);
            target.SendPacket(sm);

            sm = new SystemMessage(SystemMessage.SystemMessageId.S2MpRestoredByS1);
            sm.AddPlayerName(admin.Name);
            sm.AddNumber((int)mpval);
            target.SendPacket(sm);
        }
    }
}