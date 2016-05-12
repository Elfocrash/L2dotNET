using L2dotNET.GameService.model.inventory;
using L2dotNET.GameService.model.npcs.cubic;
using L2dotNET.GameService.model.skills2;

namespace L2dotNET.GameService.network.l2send
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
            writeD(player.Heading);
            writeD(player.ObjID);

            writeS(player.Name);

            writeD((int)player.BaseClass.ClassId.ClassRace);
            writeD(player.Sex);
            writeD((int)player.BaseClass.ClassId.Id);
            writeD(player.Level);
            writeQ(player.Exp);

            writeD(player.STR);
            writeD(player.DEX);
            writeD(player.CON);
            writeD(player.INT);
            writeD(player.WIT);
            writeD(player.MEN);

            writeD(player.CurHP);//max hp
            writeD(player.CurHP);
            writeD(player.CurMP);//max mp
            writeD(player.CurMP);
            writeD(player.SP);
            writeD(player.CurrentWeight);
            writeD(player.CharacterStat.getStat(TEffectType.b_max_weight));

            writeD(player.Inventory.getWeapon() != null ? 40 : 20); // 20 no weapon, 40 weapon equipped

            for (byte id = 0; id < InvPC.EQUIPITEM_Max; id++)
            {
                int result = 0;
                result = player.Inventory._paperdoll[id][0];
                writeD(result);
            }

            for (byte id = 0; id < InvPC.EQUIPITEM_Max; id++)
            {
                int result = 0;
                result = player.Inventory._paperdoll[id][0];
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
            writeF(1); //run speed multiplier
            writeF(1); //atk speed multiplier

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

            writeD(player.sstt); //_relation
            writeC(player.MountType);
            writeC(player.getPrivateStoreType());//
            writeC(player.p_create_item > 0 ? 1 : 0);
            writeD(player.PkKills);
            writeD(player.PvpKills);

            writeH(player.cubics.Count);
            foreach (Cubic cub in player.cubics)
                writeH(cub.template.id);

            writeC(0); //1-isInPartyMatchRoom

            writeD(player.AbnormalBitMask);

            //byte flymode = 0;

            //if (player.TransformID > 0)
            //    flymode = player.Transform.Template.MoveMode;

            writeC(0x00);

            writeD(player.ClanPrivs);

            writeH(player.RecHave); //c2  recommendations remaining
            writeH(player.RecLeft); //c2  recommendations received
            writeD(player.MountType > 0 ? player.MountedTemplate.NpcId + 1000000 : 0);//moun t npcid
            writeH(player.ItemLimit_Inventory);

            writeD((int)player.ActiveClass.ClassId.Id);
            writeD(0); // special effects? circles around player...
            writeD(player.CurCP); //max cp
            writeD(player.CurCP);
            writeC(player.GetEnchantValue());
            writeC(player.TeamID);
            writeD(player.getClanCrestLargeId());
            writeC(player.Noblesse);

            byte hero = player.Heroic;
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
