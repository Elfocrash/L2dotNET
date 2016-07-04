using L2dotNET.GameService.Model.Inventory;
using L2dotNET.GameService.Model.Npcs.Cubic;
using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Model.Skills2;

namespace L2dotNET.GameService.Network.Serverpackets
{
    class UserInfo : GameServerNetworkPacket
    {
        private readonly L2Player _player;

        public UserInfo(L2Player player)
        {
            this._player = player;
        }

        protected internal override void Write()
        {
            WriteC(0x04);

            WriteD(_player.X);
            WriteD(_player.Y);
            WriteD(_player.Z);
            WriteD(_player.Heading);
            WriteD(_player.ObjId);

            WriteS(_player.Name);

            WriteD((int)_player.BaseClass.ClassId.ClassRace);
            WriteD(_player.Sex);
            WriteD((int)_player.BaseClass.ClassId.Id);
            WriteD(_player.Level);
            WriteQ(_player.Exp);

            WriteD(_player.Str);
            WriteD(_player.Dex);
            WriteD(_player.Con);
            WriteD(_player.Int);
            WriteD(_player.Wit);
            WriteD(_player.Men);

            WriteD(_player.CurHp); //max hp
            WriteD(_player.CurHp);
            WriteD(_player.CurMp); //max mp
            WriteD(_player.CurMp);
            WriteD(_player.Sp);
            WriteD(_player.CurrentWeight);
            WriteD(_player.CharacterStat.GetStat(EffectType.BMaxWeight));

            WriteD(_player.Inventory.GetPaperdollItem(Inventory.PaperdollRhand) != null ? 40 : 20); // 20 no weapon, 40 weapon equipped

            for (byte id = 0; id < Inventory.PaperdollTotalslots; id++)
            {
                int result = _player.Inventory.Paperdoll[id].Template.ItemId;
                WriteD(result);
            }

            for (byte id = 0; id < Inventory.PaperdollTotalslots; id++)
            {
                int result = _player.Inventory.Paperdoll[id].Template.ItemId;
                WriteD(result);
            }

            // c6 new h's
            WriteH(0x00);
            WriteH(0x00);
            WriteH(0x00);
            WriteH(0x00);
            WriteH(0x00);
            WriteH(0x00);
            WriteH(0x00);
            WriteH(0x00);
            WriteH(0x00);
            WriteH(0x00);
            WriteH(0x00);
            WriteH(0x00);
            WriteH(0x00);
            WriteH(0x00);
            WriteD(0x00);//player.Inventory.getPaperdollAugmentId(InvPC.EQUIPITEM_RHand));
            WriteH(0x00);
            WriteH(0x00);
            WriteH(0x00);
            WriteH(0x00);
            WriteH(0x00);
            WriteH(0x00);
            WriteH(0x00);
            WriteH(0x00);
            WriteH(0x00);
            WriteH(0x00);
            WriteH(0x00);
            WriteH(0x00);
            WriteD(0x00);//player.Inventory.getPaperdollAugmentId(InvPC.EQUIPITEM_LHand));
            WriteH(0x00);
            WriteH(0x00);
            WriteH(0x00);
            WriteH(0x00);

            double atkspd = _player.CharacterStat.GetStat(EffectType.BAttackSpd);
            WriteD(_player.CharacterStat.GetStat(EffectType.PPhysicalAttack));
            WriteD(atkspd);
            WriteD(_player.CharacterStat.GetStat(EffectType.PPhysicalDefense));
            WriteD(_player.CharacterStat.GetStat(EffectType.BEvasion));
            WriteD(_player.CharacterStat.GetStat(EffectType.BAccuracy));
            WriteD(_player.CharacterStat.GetStat(EffectType.BCriticalRate));
            WriteD(_player.CharacterStat.GetStat(EffectType.PMagicalAttack));
            WriteD(_player.CharacterStat.GetStat(EffectType.BCastingSpd));
            WriteD(atkspd); //? еще раз?
            WriteD(_player.CharacterStat.GetStat(EffectType.PMagicalDefense));

            WriteD(_player.PvPStatus);
            WriteD(_player.Karma);

            double spd = _player.CharacterStat.GetStat(EffectType.PSpeed);

            double anim = spd * 1f / 130;
            //double anim2 = (1.1) * atkspd / 300;
            double runSpd = spd / anim;
            double walkSpd = spd * .8 / anim;

            WriteD(runSpd);
            WriteD(walkSpd);
            WriteD(50); // swimspeed
            WriteD(50); // swimspeed
            WriteD(0); //?
            WriteD(0); //?
            WriteD(runSpd); //fly run
            WriteD(walkSpd); //fly walk ?
            WriteF(1); //run speed multiplier
            WriteF(1); //atk speed multiplier

            WriteF(_player.Radius);
            WriteF(_player.Height);

            WriteD(_player.HairStyle);
            WriteD(_player.HairColor);
            WriteD(_player.Face);
            WriteD(_player.Builder);

            WriteS(_player.Title);

            WriteD(_player.ClanId);
            WriteD(_player.ClanCrestId);
            WriteD(_player.AllianceId);
            WriteD(_player.AllianceCrestId);

            WriteD(_player.Sstt); //_relation
            WriteC(_player.MountType);
            WriteC(_player.GetPrivateStoreType()); //
            WriteC(_player.PCreateItem > 0 ? 1 : 0);
            WriteD(_player.PkKills);
            WriteD(_player.PvpKills);

            WriteH(_player.Cubics.Count);
            foreach (Cubic cub in _player.Cubics)
                WriteH(cub.Template.Id);

            WriteC(0); //1-isInPartyMatchRoom

            WriteD(_player.AbnormalBitMask);

            //byte flymode = 0;

            //if (player.TransformID > 0)
            //    flymode = player.Transform.Template.MoveMode;

            WriteC(0x00);

            WriteD(_player.ClanPrivs);

            WriteH(_player.RecHave); //c2  recommendations remaining
            WriteH(_player.RecLeft); //c2  recommendations received
            WriteD(_player.MountType > 0 ? _player.MountedTemplate.NpcId + 1000000 : 0); //moun t npcid
            WriteH(_player.ItemLimitInventory);

            WriteD((int)_player.ActiveClass.ClassId.Id);
            WriteD(0); // special effects? circles around player...
            WriteD(_player.CurCp); //max cp
            WriteD(_player.CurCp);
            WriteC(_player.GetEnchantValue());
            WriteC(_player.TeamId);
            WriteD(_player.GetClanCrestLargeId());
            WriteC(_player.Noblesse);

            byte hero = _player.Heroic;
            WriteC(hero);

            WriteC(_player.IsFishing() ? 0x01 : 0x00); //Fishing Mode
            WriteD(_player.GetFishx()); //fishing x
            WriteD(_player.GetFishy()); //fishing y
            WriteD(_player.GetFishz()); //fishing z
            WriteD(_player.GetNameColor());

            WriteC(_player.IsRunning);

            WriteD(_player.ClanRank());
            WriteD(_player.ClanType);

            WriteD(_player.GetTitleColor());
            WriteD(_player.CursedWeaponLevel);
        }
    }
}