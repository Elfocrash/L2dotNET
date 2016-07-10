using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Network.Serverpackets;
using L2dotNET.GameService.World;
using L2dotNET.Utility;

namespace L2dotNET.GameService.Model.Skills2.Effects
{
    class ATeleRegion : Effect
    {
        private string _region;

        public override void Build(string str)
        {
            _region = str.Split(' ')[1];
        }

        public override EffectResult OnStart(L2Character caster, L2Character target)
        {
            int[] loc = null; //для городов не буду писать, пусть тащит как нуль
            if (_region.EqualsIgnoreCase("hideout"))
            {
                L2Player player = (L2Player)target;
                if ((player.ClanId > 0) && (player.Clan.HideoutId > 0))
                {
                    loc = player.Clan.Hideout.ownerLoc;
                }
            }

            //if (loc == null) //ELFOC
            //    loc = MapRegionTable.getInstance().getRespawn(target.X, target.Y, ((L2Player)target).Karma);

            if (loc != null)
            {
                target.Teleport(loc[0], loc[1], loc[2]);
            }
            return Nothing;
        }

        public override bool CanUse(L2Character caster)
        {
            L2Player player = (L2Player)caster;

            if (!player.IsWard())
            {
                return true;
            }

            caster.SendSystemMessage(SystemMessage.SystemMessageId.CannotTeleportWhilePossessionWard);
            return false;
        }
    }
}