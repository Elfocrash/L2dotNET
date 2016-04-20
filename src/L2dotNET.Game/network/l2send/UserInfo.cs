using L2dotNET.Game.model.inventory;
using L2dotNET.Game.model.npcs.cubic;
using L2dotNET.Game.model.skills2;

namespace L2dotNET.Game.network.l2send
{
    class UserInfo : GameServerNetworkPacket
    {
        private L2Player player;
        public UserInfo(L2Player player)
        {
            this.player = player;
        }

        protected internal override void write()
        {
            writeC(0x04);

            writeD(player.X);
            writeD(player.Y);
            writeD(player.Z);
            writeD(player.VehicleId);
            writeD(player.ObjID);

            writeS(player.Name);

            writeD(player.BaseClass.race);
            writeD(player.Sex);
            writeD(player.BaseClass.id);
            writeD(player.Level);
            writeQ(player.Exp);

            writeD(player.getSTR());
            writeD(player.getDEX());
            writeD(player.getCON());
            writeD(player.getINT());
            writeD(player.getWIT());
            writeD(player.getMEN());

            writeD(player.CharacterStat.getStat(TEffectType.b_max_hp));
            writeD(player.CurHP);
            writeD(player.CharacterStat.getStat(TEffectType.b_max_mp));
            writeD(player.CurMP);
            writeD(player.SP);
            writeD(player.CurrentWeight);
            writeD(player.CharacterStat.getStat(TEffectType.b_max_weight));

            writeD(player.Inventory.getWeapon() != null ? 40 : 20); // 20 no weapon, 40 weapon equipped

            for (byte id = 0; id < InvPC.EQUIPITEM_Max; id++)
            {
                int result = 0;
                if (player.Inventory._paperdollVisual[id] > 0)
                    result = player.Inventory._paperdollVisual[id];
                else
                    result = player.Inventory._paperdoll[id][1];

                writeD(result);
            }

            for (byte id = 0; id < InvPC.EQUIPITEM_Max; id++)
            {
                int result = 0;
                if (player.Inventory._paperdollVisual[id] > 0)
                    result = player.Inventory._paperdollVisual[id];
                else
                    result = player.Inventory._paperdoll[id][0];

                writeD(result);
            }

            for (byte id = 0; id < InvPC.EQUIPITEM_Max; id++)
            {
                int result = 0;
                if (player.Inventory._paperdollVisual[id] == 0)
                    result = player.Inventory._paperdoll[id][2];

                writeD(result);
            }
            // c6 new h's
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
            writeH(0x00);
            writeH(0x00);
            writeD(player.Inventory.getWeaponAugmentation());
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
            writeD(player.Inventory.getWeaponAugmentation());
            writeH(0x00);
            writeH(0x00);
            writeH(0x00);
            writeH(0x00);

            double atkspd = player.CharacterStat.getStat(TEffectType.b_attack_spd);
            writeD(player.CharacterStat.getStat(TEffectType.p_physical_attack));
            writeD(atkspd);
            writeD(player.CharacterStat.getStat(TEffectType.p_physical_defense));
            writeD(player.CharacterStat.getStat(TEffectType.b_evasion));
            writeD(player.CharacterStat.getStat(TEffectType.b_accuracy));
            writeD(player.CharacterStat.getStat(TEffectType.b_critical_rate));
            writeD(player.CharacterStat.getStat(TEffectType.p_magical_attack));
            writeD(player.CharacterStat.getStat(TEffectType.b_casting_spd));
            writeD(atkspd); //? еще раз?
            writeD(player.CharacterStat.getStat(TEffectType.p_magical_defense));

            writeD(player.PvPStatus);
            writeD(player.Karma);

            double spd = player.CharacterStat.getStat(TEffectType.p_speed);

            double anim = spd * 1f / 130;
            double anim2 = (1.1) * atkspd / 300;
            double runSpd = spd / anim;
            double walkSpd = spd * .8 / anim;

            writeD(runSpd);
            writeD(walkSpd);
            writeD(50); // swimspeed
            writeD(50); // swimspeed
            writeD(0); //?
            writeD(0); //?
            writeD(runSpd); //fly run
            writeD(walkSpd); //fly walk ?
            writeF(0); //анимация бега
            writeF(0); //анимация атаки

            writeD(0);
            writeD(0);
            writeF(0);
            writeF(0);

            writeF(player.Radius);
            writeF(player.Height);

            writeD(player.HairStyle);
            writeD(player.HairColor);
            writeD(player.Face);
            writeD(player.Builder);

            writeS(player.Title);

            writeD(player.ClanId);
            writeD(player.ClanCrestId);
            writeD(player.AllianceId);
            writeD(player.AllianceCrestId);
            // 0x40 leader rights
            /*
             *  128, 640 shield
                192, 704 crown
                384, 896 sword blue
                448 flag leader
             * */
            // siege flags: attacker - 0x180 sword over name, defender - 0x80 shield, 0xC0 crown (|leader), 0x1C0 flag (|leader)
            writeD(player.sstt); //_relation
            writeC(player.MountType);
            writeC(player.getPrivateStoreType());//
            writeC(player.p_create_item > 0 ? 1 : 0);
            writeD(player.PkCount);
            writeD(player.PvpCount);

            writeH(player.cubics.Count);
            foreach (Cubic cub in player.cubics)
                writeH(cub.template.id);

            writeC(0); //1-isInPartyMatchRoom

            writeD(player.AbnormalBitMask);

            byte flymode = 0;

            if (player.TransformID > 0)
                flymode = player.Transform.Template.MoveMode;

            writeC(0x00);

            writeD(player.ClanPrivs);

            writeH(player._recHave); //c2  recommendations remaining
            writeH(player._eval); //c2  recommendations received
            writeD(player.MountType > 0 ? player.MountedTemplate.NpcId + 1000000 : 0);//moun t npcid
            writeH(player.ItemLimit_Inventory);

            writeD(player.ActiveClass.id);
            writeD(0); // special effects? circles around player...
            writeD(player.CharacterStat.getStat(TEffectType.b_max_cp));
            writeD(player.CurCP);
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

            writeC(player.IsRunning);

            writeD(player.ClanRank());
            writeD(player.ClanType);

            writeD(player.getTitleColor());
            writeD(player.CursedWeaponLevel);
            
        }
    }
}
