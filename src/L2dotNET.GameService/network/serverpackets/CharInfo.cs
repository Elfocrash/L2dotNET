using L2dotNET.GameService.Model.Inventory;
using L2dotNET.GameService.Model.Npcs.Cubic;
using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Model.Skills2;

namespace L2dotNET.GameService.Network.Serverpackets
{
    class CharInfo : GameServerNetworkPacket
    {
        private readonly L2Player player;

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
            writeD(player.ObjId);
            writeS(player.Name);

            writeD((int)player.BaseClass.ClassId.ClassRace);
            writeD(player.Sex);
            writeD((int)player.ActiveClass.ClassId.Id);

            writeD(player.Inventory.Paperdoll[Inventory.PaperdollHair].Template.ItemID);
            writeD(player.Inventory.Paperdoll[Inventory.PaperdollHead].Template.ItemID);
            writeD(player.Inventory.Paperdoll[Inventory.PaperdollRhand].Template.ItemID);
            writeD(player.Inventory.Paperdoll[Inventory.PaperdollLhand].Template.ItemID);
            writeD(player.Inventory.Paperdoll[Inventory.PaperdollGloves].Template.ItemID);
            writeD(player.Inventory.Paperdoll[Inventory.PaperdollChest].Template.ItemID);
            writeD(player.Inventory.Paperdoll[Inventory.PaperdollLegs].Template.ItemID);
            writeD(player.Inventory.Paperdoll[Inventory.PaperdollFeet].Template.ItemID);
            writeD(player.Inventory.Paperdoll[Inventory.PaperdollBack].Template.ItemID);
            writeD(player.Inventory.Paperdoll[Inventory.PaperdollRhand].Template.ItemID);
            writeD(player.Inventory.Paperdoll[Inventory.PaperdollHairall].Template.ItemID);
            writeD(player.Inventory.Paperdoll[Inventory.PaperdollFace].Template.ItemID);

            writeH(0x00);
            writeH(0x00);
            writeH(0x00);
            writeH(0x00);
            writeD(0x00);//player.Inventory.getPaperdollAugmentId(InvPC.EQUIPITEM_RHand));
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
            writeD(0x00);//player.Inventory.getPaperdollAugmentId(InvPC.EQUIPITEM_LHand));
            writeH(0x00);
            writeH(0x00);
            writeH(0x00);
            writeH(0x00);

            writeD(player.PvPStatus);
            writeD(player.Karma);

            writeD(player.CharacterStat.getStat(TEffectType.b_attack_spd)); //matkspeed
            writeD(player.CharacterStat.getStat(TEffectType.b_casting_spd));

            writeD(player.PvPStatus);
            writeD(player.Karma);

            double spd = player.CharacterStat.getStat(TEffectType.p_speed);
            double anim = spd * 1f / 130;
            double anim2 = (1.1) * player.CharacterStat.getStat(TEffectType.b_attack_spd) / 300;
            double runSpd = spd / anim;
            double walkSpd = spd * .8 / anim;

            writeD(runSpd);
            writeD(walkSpd);
            writeD(50); // swimspeed
            writeD(50); // swimspeed
            writeD(runSpd);
            writeD(walkSpd);
            writeD(runSpd);
            writeD(walkSpd);
            writeF(anim);
            writeF(anim2);

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

            writeD(0);

            writeC(player.isSitting() ? 0 : 1); // standing = 1  sitting = 0
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

            writeD(player.MaxCp); //max cp here
            writeD((int)player.CurCp);
            writeC(player.GetEnchantValue());
            writeC(player.TeamId);
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

            writeD(0x00);

            writeD(player.ClanRank());
            writeD(player.ClanType);

            writeD(player.getTitleColor());
            writeD(0x00); //titlecolor
        }
    }
}