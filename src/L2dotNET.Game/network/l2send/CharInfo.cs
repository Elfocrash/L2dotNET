using L2dotNET.Game.model.inventory;
using L2dotNET.Game.model.npcs.cubic;
using L2dotNET.Game.model.skills2;

namespace L2dotNET.Game.network.l2send
{
    class CharInfo : GameServerNetworkPacket
    {
        private L2Player player;
        public CharInfo(L2Player player)
        {
            this.player = player;
        }

        protected internal override void write()
        {
            writeC(0x31);

            writeD(player.X);
            writeD(player.Y);
            writeD(player.Z);
            writeD(0);
            writeD(player.ObjID);
            writeS(player.Name);

            writeD(player.BaseClass.race);
            writeD(player.Sex);
            writeD(player.BaseClass.id);

            for (byte id = 0; id < InvPC.EQUIPITEM_Max; id++)
            {
                if (id > 0 && id < 6)
                    continue;

                int result = 0;
                if (player.Inventory._paperdollVisual[id] > 0)
                    result = player.Inventory._paperdollVisual[id];
                else
                    result = player.Inventory._paperdoll[id][0];

                writeD(result);
            }

            for (byte id = 0; id < InvPC.EQUIPITEM_Max; id++)
            {
                if (id > 0 && id < 6)
                    continue;

                int result = 0;
                if (player.Inventory._paperdollVisual[id] > 0)
                    result = player.Inventory._paperdollVisual[id];
                else
                    result = player.Inventory._paperdoll[id][2];

                writeD(result);
            }

            writeD(0x00);
            writeD(0x01);

            writeD(player.PvPStatus); 
            writeD(player.Karma);

            double atkspd = player.CharacterStat.getStat(TEffectType.b_attack_spd);

            writeD(player.CharacterStat.getStat(TEffectType.b_casting_spd));
            writeD(atkspd);

            writeD(0x00);

            double spd = player.CharacterStat.getStat(TEffectType.p_speed);
            double anim = spd * 1f / 130;
            double anim2 = (1.1) * atkspd / 300;
            double runSpd = spd / anim;
            double walkSpd = spd * .8 / anim;

            writeD(runSpd);
            writeD(walkSpd);
            writeD(50); // swimspeed
            writeD(50); // swimspeed
            writeD(0);
            writeD(0);
            writeD(0);
            writeD(0);
            writeF(anim); //анимация бега
            writeF(anim2); //анимация атаки

            writeF(player.Radius);
            writeF(player.Height);

            writeD(player.HairStyle);
            writeD(player.HairColor);
            writeD(player.Face);

            writeS(player.Title);

            writeD(player.ClanId);
            writeD(player.ClanCrestId);
            writeD(player.AllianceId);
            writeD(player.AllianceCrestId);

            writeC(player.isSitting() ? 0 : 1);	// standing = 1  sitting = 0
            writeC(player.IsRunning);
            writeC(player.isInCombat() ? 1 : 0);
            writeC(player.isAlikeDead() ? 1 : 0); //if (_activeChar.isInOlympiadMode()) 0 TODO
            writeC(player.Visible ? 0 : 1);

            writeC(player.MountType);
            writeC(player.getPrivateStoreType());

            writeH(player.cubics.Count);
            foreach (Cubic cub in player.cubics)
                writeH(cub.template.id);

            writeC(0x00); //1-_activeChar.isInPartyMatchRoom()

            writeD(player.AbnormalBitMask);

            writeC(0); //_activeChar.isFlyingMounted() ? 2 : 0);
            writeH(player._recHave);

            writeD(0 + 1000000);//_activeChar.getMountNpcId() + 1000000
            writeD(player.ActiveClass.id);
            writeD(0);
            writeC(player.GetEnchantValue());
            writeC(player.TeamID);
            writeD(player.getClanCrestLargeId());
            writeC(player.Noblesse);

            byte hero = player.Heroic;
            if (player.TransformID != 0)
                hero = 0;

            writeC(hero);

            writeC(player.isFishing() ? 0x01 : 0x00); //Fishing Mode
            writeD(player.GetFishx()); //fishing x  
            writeD(player.GetFishy()); //fishing y
            writeD(player.GetFishz()); //fishing z
            writeD(player.getNameColor());

            writeD(player.Heading);

            writeD(player.ClanRank()); 
            writeD(player.ClanType);

            writeD(player.getTitleColor());
            writeD(player.CursedWeaponLevel);

            writeD(player.ClanId > 0 ? player.Clan.ClanNameValue : 0);

            writeD(player.TransformID);
            writeD(player.AgationID);
            writeD(1);// Image index ????
            writeD(player.AbnormalBitMaskEx);
            writeD(0); // territory Id
            writeD(0); // is Disguised
            writeD(0); // territory Id
        }
    }
}
