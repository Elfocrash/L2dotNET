using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Network.Serverpackets;
using L2dotNET.GameService.World;
using L2dotNET.Utility;

namespace L2dotNET.GameService.Model.Skills2.Effects
{
    class a_tele_region : TEffect
    {
        private string region;

        public override void build(string str)
        {
            region = str.Split(' ')[1];
        }

        public override TEffectResult onStart(L2Character caster, L2Character target)
        {
            int[] loc = null; //для городов не буду писать, пусть тащит как нуль
            if (region.EqualsIgnoreCase("hideout"))
            {
                L2Player player = (L2Player)target;
                if ((player.ClanId > 0) && (player.Clan.HideoutID > 0))
                    loc = player.Clan.hideout.ownerLoc;
            }

            //if (loc == null) //ELFOC
            //    loc = MapRegionTable.getInstance().getRespawn(target.X, target.Y, ((L2Player)target).Karma);

            if (loc != null)
                target.Teleport(loc[0], loc[1], loc[2]);
            return nothing;
        }

        public override bool canUse(L2Character caster)
        {
            L2Player player = (L2Player)caster;

            if (player.IsWard())
            {
                caster.SendSystemMessage(SystemMessage.SystemMessageId.CANNOT_TELEPORT_WHILE_POSSESSION_WARD);
                return false;
            }

            return true;
        }
    }
}