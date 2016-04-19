using L2dotNET.Game.managers;
using L2dotNET.Game.model.items;

namespace L2dotNET.Game.network.l2send
{
    class ExShowSentPost : GameServerNetworkPacket
    {
        private MailMessage mail;
        public ExShowSentPost(MailMessage mm)
        {
            this.mail = mm;
        }

        protected internal override void write()
        {
            writeC(0xfe);
            writeH(0xad);
            writeD(mail.MailID);
            writeD(mail.Trade);
            writeS(mail.ReceiverName);
            writeS(mail.Title);
            writeS(mail.Content);

            writeD(mail.WithItem > 0 ? mail.Inventory.Items.Count : 0);

            foreach (L2Item item in mail.Inventory.Items.Values)
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

            writeQ(mail.PaymentAdena);
            writeD(mail.SentBySystem);
        }
    }
}
