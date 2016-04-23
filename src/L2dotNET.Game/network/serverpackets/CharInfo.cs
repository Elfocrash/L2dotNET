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
            writeC(0x03);

            writeD(player.X);
            writeD(player.Y);
            writeD(player.Z);
            writeD(player.Heading);
            writeD(player.ObjID);
            writeS(player.Name);

            writeD((int)player.BaseClass.ClassId.ClassRace);
            writeD(player.Sex);
            writeD((int)player.ActiveClass.ClassId.Id);

        

            writeD(player.Inventory._paperdoll[InvPC.EQUIPITEM_Hair2][0]);
            writeD(player.Inventory._paperdoll[InvPC.EQUIPITEM_Head][0]);
            writeD(player.Inventory._paperdoll[InvPC.EQUIPITEM_RHand][0]);
            writeD(player.Inventory._paperdoll[InvPC.EQUIPITEM_LHand][0]);
            writeD(player.Inventory._paperdoll[InvPC.EQUIPITEM_Gloves][0]);
            writeD(player.Inventory._paperdoll[InvPC.EQUIPITEM_Chest][0]);
            writeD(player.Inventory._paperdoll[InvPC.EQUIPITEM_Legs][0]);
            writeD(player.Inventory._paperdoll[InvPC.EQUIPITEM_Feet][0]);
            writeD(player.Inventory._paperdoll[InvPC.EQUIPITEM_Cloak][0]);
            writeD(player.Inventory._paperdoll[InvPC.EQUIPITEM_RHand][0]);
            writeD(player.Inventory._paperdoll[InvPC.EQUIPITEM_Hair][0]);
            writeD(0x00);//face

            writeH(0x00);
            writeH(0x00);
            writeH(0x00);
            writeH(0x00);
            writeD(player.Inventory.getPaperdollAugmentId(InvPC.EQUIPITEM_RHand));
            writeH(0x00);
            writeH(0x00);
            writeH(0x00);
            writeH(0x00);
            writeH(0x00);
            writeH(0x00);
            writeH(0x00);
            writeH(0x00);
            writeH(0x00);
            writeH(0x00);
            writeH(0x00);
            writeH(0x00);
            writeD(player.Inventory.getPaperdollAugmentId(InvPC.EQUIPITEM_LHand));
            writeH(0x00);
            writeH(0x00);
            writeH(0x00);
            writeH(0x00);

            writeD(player.PvPStatus);
            writeD(player.Karma);

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
            writeD(runSpd);
            writeD(walkSpd);
            writeD(0);
            writeD(0);
            writeF(anim); //анимация бега
            writeF(anim2); //анимация атаки

            writeF(player.Radius);//elfo
            writeF(player.Height);

            writeD(player.HairStyle);
            writeD(player.HairColor);
            writeD(player.Face);

            writeS(player.Title);

            writeD(player.ClanId);
            writeD(player.ClanCrestId);
            writeD(player.AllianceId);
            writeD(player.AllianceCrestId);

            writeD(0);

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
            writeH(player.RecHave);
            writeD((int)player.ActiveClass.ClassId.Id);

            writeD(player.CurrentCP);//max cp here
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
        }
    }
}
