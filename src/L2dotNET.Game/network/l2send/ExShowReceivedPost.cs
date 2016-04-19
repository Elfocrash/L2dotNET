using L2dotNET.Game.managers;
using L2dotNET.Game.model.items;

namespace L2dotNET.Game.network.l2send
{
    class ExShowReceivedPost : GameServerNetworkPacket
    {
        private MailMessage Mail;
        public ExShowReceivedPost(MailMessage mm)
        {
            Mail = mm;
        }

        protected internal override void write()
        {
            writeC(0xfe);
            writeH(0xab);
            writeD(Mail.MailID);
            writeD(Mail.Trade);
            writeD(0x00); // unk
            writeS(Mail.SenderName);
            writeS(Mail.Title);
            writeS(Mail.Content);

            writeD(Mail.WithItem > 0 ? Mail.Inventory.Items.Count : 0);

            foreach (L2Item item in Mail.Inventory.Items.Values)
            {
                writeD(0);
                writeD(item.Template.ItemID);
                writeD(0);
                writeQ(item.Count);

                writeH(item.Template.Type2());
                writeH(0);
                writeH(0);

                writeD(item.Template.BodyPartId());
                writeH(item.Enchant);
                writeH(0);

                writeD(item.AugmentationID);
                writeD(item.Durability);
                writeD(item.LifeTimeEnd());

                writeH(item.AttrAttackType);
                writeH(item.AttrAttackValue);
                writeH(item.AttrDefenseValueFire);
                writeH(item.AttrDefenseValueWater);
                writeH(item.AttrDefenseValueWind);
                writeH(item.AttrDefenseValueEarth);
                writeH(item.AttrDefenseValueHoly);
                writeH(item.AttrDefenseValueUnholy);

                writeH(0x00);
                writeH(0x00);
                writeH(0x00);

                writeD(item.ObjID);
            }

            writeQ(Mail.PaymentAdena);
            writeD(Mail.ReturnAble);
            writeD(Mail.SentBySystem);
        }
    }
}
