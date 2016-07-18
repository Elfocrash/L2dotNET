using L2dotNET.GameService.Model.Inventory;
using L2dotNET.GameService.Model.Npcs.Cubic;
using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Model.Skills2;
using L2dotNET.Network;

namespace L2dotNET.GameService.Network.Serverpackets
{
    class CharInfo : GameserverPacket
    {
        private readonly L2Player _player;

        public CharInfo(L2Player player)
        {
            _player = player;
        }

        //TODO: Simplify method body
        public override void Write()
        {
            WriteByte(0x03);

            WriteInt(_player.X);
            WriteInt(_player.Y);
            WriteInt(_player.Z);
            WriteInt(_player.Heading);
            WriteInt(_player.ObjId);
            WriteString(_player.Name);

            WriteInt((int)_player.BaseClass.ClassId.ClassRace);
            WriteInt(_player.Sex);
            WriteInt((int)_player.ActiveClass.ClassId.Id);

            WriteInt(_player.Inventory.Paperdoll[Inventory.PaperdollHair]?.Template.ItemId ?? 0);
            WriteInt(_player.Inventory.Paperdoll[Inventory.PaperdollHead]?.Template.ItemId ?? 0);
            WriteInt(_player.Inventory.Paperdoll[Inventory.PaperdollRhand]?.Template.ItemId ?? 0);
            WriteInt(_player.Inventory.Paperdoll[Inventory.PaperdollLhand]?.Template.ItemId ?? 0);
            WriteInt(_player.Inventory.Paperdoll[Inventory.PaperdollGloves]?.Template.ItemId ?? 0);
            WriteInt(_player.Inventory.Paperdoll[Inventory.PaperdollChest]?.Template.ItemId ?? 0);
            WriteInt(_player.Inventory.Paperdoll[Inventory.PaperdollLegs]?.Template.ItemId ?? 0);
            WriteInt(_player.Inventory.Paperdoll[Inventory.PaperdollFeet]?.Template.ItemId ?? 0);
            WriteInt(_player.Inventory.Paperdoll[Inventory.PaperdollBack]?.Template.ItemId ?? 0);
            WriteInt(_player.Inventory.Paperdoll[Inventory.PaperdollRhand]?.Template.ItemId ?? 0);
            WriteInt(_player.Inventory.Paperdoll[Inventory.PaperdollHairall]?.Template.ItemId ?? 0);
            WriteInt(_player.Inventory.Paperdoll[Inventory.PaperdollFace]?.Template.ItemId ?? 0);

            WriteShort(0x00);
            WriteShort(0x00);
            WriteShort(0x00);
            WriteShort(0x00);
            WriteInt(0x00); //player.Inventory.getPaperdollAugmentId(InvPC.EQUIPITEM_RHand));
            WriteShort(0x00);
            WriteShort(0x00);
            WriteShort(0x00);
            WriteShort(0x00);
            WriteShort(0x00);
            WriteShort(0x00);
            WriteShort(0x00);
            WriteShort(0x00);
            WriteShort(0x00);
            WriteShort(0x00);
            WriteShort(0x00);
            WriteShort(0x00);
            WriteInt(0x00); //player.Inventory.getPaperdollAugmentId(InvPC.EQUIPITEM_LHand));
            WriteShort(0x00);
            WriteShort(0x00);
            WriteShort(0x00);
            WriteShort(0x00);

            WriteInt(_player.PvPStatus);
            WriteInt(_player.Karma);

            WriteInt(_player.CharacterStat.GetStat(EffectType.BAttackSpd)); //matkspeed
            WriteInt(_player.CharacterStat.GetStat(EffectType.BCastingSpd));

            WriteInt(_player.PvPStatus);
            WriteInt(_player.Karma);

            double spd = _player.CharacterStat.GetStat(EffectType.PSpeed);
            double anim = (spd * 1f) / 130;
            double anim2 = (1.1 * _player.CharacterStat.GetStat(EffectType.BAttackSpd)) / 300;
            double runSpd = spd / anim;
            double walkSpd = (spd * .8) / anim;

            WriteInt(runSpd);
            WriteInt(walkSpd);
            WriteInt(50); // swimspeed
            WriteInt(50); // swimspeed
            WriteInt(runSpd);
            WriteInt(walkSpd);
            WriteInt(runSpd);
            WriteInt(walkSpd);
            WriteDouble(1);
            WriteDouble(1);

            WriteDouble(_player.Radius);
            WriteDouble(_player.Height);

            WriteInt(_player.HairStyle);
            WriteInt(_player.HairColor);
            WriteInt(_player.Face);

            WriteString(_player.Title);

            WriteInt(_player.ClanId);
            WriteInt(_player.ClanCrestId);
            WriteInt(_player.AllianceId);
            WriteInt(_player.AllianceCrestId);

            WriteInt(0);

            WriteByte(_player.IsSitting() ? 0 : 1); // standing = 1  sitting = 0
            WriteByte(_player.IsRunning);
            WriteByte(_player.isInCombat() ? 1 : 0);
            WriteByte(_player.IsAlikeDead() ? 1 : 0); //if (_activeChar.isInOlympiadMode()) 0 TODO
            WriteByte(_player.Visible ? 0 : 1);

            WriteByte(_player.MountType);
            WriteByte(_player.GetPrivateStoreType());

            WriteShort(_player.Cubics.Count);
            foreach (Cubic cub in _player.Cubics)
                WriteShort(cub.Template.Id);

            WriteByte(0x00); //1-_activeChar.isInPartyMatchRoom()

            WriteInt(_player.AbnormalBitMask);

            WriteByte(0); //_activeChar.isFlyingMounted() ? 2 : 0);
            WriteShort(_player.RecHave);
            WriteInt((int)_player.ActiveClass.ClassId.Id);

            WriteInt(_player.MaxCp); //max cp here
            WriteInt((int)_player.CurCp);
            WriteByte(_player.GetEnchantValue());
            WriteByte(_player.TeamId);
            WriteInt(_player.GetClanCrestLargeId());
            WriteByte(_player.Noblesse);

            byte hero = _player.Heroic;
            if (_player.TransformId != 0)
                hero = 0;

            WriteByte(hero);

            WriteByte(_player.IsFishing() ? 0x01 : 0x00); //Fishing Mode
            WriteInt(_player.GetFishx()); //fishing x
            WriteInt(_player.GetFishy()); //fishing y
            WriteInt(_player.GetFishz()); //fishing z
            WriteInt(_player.GetNameColor());

            WriteInt(0x00);

            WriteInt(_player.ClanRank());
            WriteInt(_player.ClanType);

            WriteInt(_player.GetTitleColor());
            WriteInt(0x00); //titlecolor
        }
    }
}