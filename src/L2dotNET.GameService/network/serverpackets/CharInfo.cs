using L2dotNET.GameService.Model.Inventory;
using L2dotNET.GameService.Model.Npcs.Cubic;
using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Model.Skills2;

namespace L2dotNET.GameService.Network.Serverpackets
{
    class CharInfo : GameServerNetworkPacket
    {
        private readonly L2Player _player;

        public CharInfo(L2Player player)
        {
            this._player = player;
        }

        protected internal override void Write()
        {
            WriteC(0x03);

            WriteD(_player.X);
            WriteD(_player.Y);
            WriteD(_player.Z);
            WriteD(_player.Heading);
            WriteD(_player.ObjId);
            WriteS(_player.Name);

            WriteD((int)_player.BaseClass.ClassId.ClassRace);
            WriteD(_player.Sex);
            WriteD((int)_player.ActiveClass.ClassId.Id);

            WriteD(_player.Inventory.Paperdoll[Inventory.PaperdollHair].Template.ItemId);
            WriteD(_player.Inventory.Paperdoll[Inventory.PaperdollHead].Template.ItemId);
            WriteD(_player.Inventory.Paperdoll[Inventory.PaperdollRhand].Template.ItemId);
            WriteD(_player.Inventory.Paperdoll[Inventory.PaperdollLhand].Template.ItemId);
            WriteD(_player.Inventory.Paperdoll[Inventory.PaperdollGloves].Template.ItemId);
            WriteD(_player.Inventory.Paperdoll[Inventory.PaperdollChest].Template.ItemId);
            WriteD(_player.Inventory.Paperdoll[Inventory.PaperdollLegs].Template.ItemId);
            WriteD(_player.Inventory.Paperdoll[Inventory.PaperdollFeet].Template.ItemId);
            WriteD(_player.Inventory.Paperdoll[Inventory.PaperdollBack].Template.ItemId);
            WriteD(_player.Inventory.Paperdoll[Inventory.PaperdollRhand].Template.ItemId);
            WriteD(_player.Inventory.Paperdoll[Inventory.PaperdollHairall].Template.ItemId);
            WriteD(_player.Inventory.Paperdoll[Inventory.PaperdollFace].Template.ItemId);

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

            WriteD(_player.PvPStatus);
            WriteD(_player.Karma);

            WriteD(_player.CharacterStat.GetStat(EffectType.BAttackSpd)); //matkspeed
            WriteD(_player.CharacterStat.GetStat(EffectType.BCastingSpd));

            WriteD(_player.PvPStatus);
            WriteD(_player.Karma);

            double spd = _player.CharacterStat.GetStat(EffectType.PSpeed);
            double anim = spd * 1f / 130;
            double anim2 = (1.1) * _player.CharacterStat.GetStat(EffectType.BAttackSpd) / 300;
            double runSpd = spd / anim;
            double walkSpd = spd * .8 / anim;

            WriteD(runSpd);
            WriteD(walkSpd);
            WriteD(50); // swimspeed
            WriteD(50); // swimspeed
            WriteD(runSpd);
            WriteD(walkSpd);
            WriteD(runSpd);
            WriteD(walkSpd);
            WriteF(anim);
            WriteF(anim2);

            WriteF(_player.Radius);
            WriteF(_player.Height);

            WriteD(_player.HairStyle);
            WriteD(_player.HairColor);
            WriteD(_player.Face);

            WriteS(_player.Title);

            WriteD(_player.ClanId);
            WriteD(_player.ClanCrestId);
            WriteD(_player.AllianceId);
            WriteD(_player.AllianceCrestId);

            WriteD(0);

            WriteC(_player.IsSitting() ? 0 : 1); // standing = 1  sitting = 0
            WriteC(_player.IsRunning);
            WriteC(_player.isInCombat() ? 1 : 0);
            WriteC(_player.IsAlikeDead() ? 1 : 0); //if (_activeChar.isInOlympiadMode()) 0 TODO
            WriteC(_player.Visible ? 0 : 1);

            WriteC(_player.MountType);
            WriteC(_player.GetPrivateStoreType());

            WriteH(_player.Cubics.Count);
            foreach (Cubic cub in _player.Cubics)
                WriteH(cub.Template.Id);

            WriteC(0x00); //1-_activeChar.isInPartyMatchRoom()

            WriteD(_player.AbnormalBitMask);

            WriteC(0); //_activeChar.isFlyingMounted() ? 2 : 0);
            WriteH(_player.RecHave);
            WriteD((int)_player.ActiveClass.ClassId.Id);

            WriteD(_player.MaxCp); //max cp here
            WriteD((int)_player.CurCp);
            WriteC(_player.GetEnchantValue());
            WriteC(_player.TeamId);
            WriteD(_player.GetClanCrestLargeId());
            WriteC(_player.Noblesse);

            byte hero = _player.Heroic;
            if (_player.TransformId != 0)
                hero = 0;

            WriteC(hero);

            WriteC(_player.IsFishing() ? 0x01 : 0x00); //Fishing Mode
            WriteD(_player.GetFishx()); //fishing x
            WriteD(_player.GetFishy()); //fishing y
            WriteD(_player.GetFishz()); //fishing z
            WriteD(_player.GetNameColor());

            WriteD(0x00);

            WriteD(_player.ClanRank());
            WriteD(_player.ClanType);

            WriteD(_player.GetTitleColor());
            WriteD(0x00); //titlecolor
        }
    }
}