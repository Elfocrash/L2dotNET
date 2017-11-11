using System;
using System.Linq;
using L2dotNET.model.player;
using L2dotNET.Network.serverpackets;
using L2dotNET.tables;

namespace L2dotNET.Network.clientpackets.RecipeAPI
{
    class RequestRecipeItemMakeSelf : PacketBase
    {
        private readonly GameClient _client;
        private readonly int _id;

        public RequestRecipeItemMakeSelf(Packet packet, GameClient client)
        {
            _client = client;
            _id = packet.ReadInt();
        }

        public override void RunImpl()
        {
            L2Player player = _client.CurrentPlayer;

            if (player.RecipeBook == null)
            {
                player.SendSystemMessage(SystemMessage.SystemMessageId.RecipeIncorrect);
                player.SendActionFailed();
                return;
            }

            L2Recipe rec = player.RecipeBook.FirstOrDefault(r => r.RecipeId == _id);

            if (rec == null)
            {
                player.SendSystemMessage(SystemMessage.SystemMessageId.RecipeIncorrect);
                player.SendActionFailed();
                return;
            }

            if (player.CurMp < rec.MpConsume)
            {
                player.SendSystemMessage(SystemMessage.SystemMessageId.NotEnoughMp);
                player.SendActionFailed();
                return;
            }

            bool next;

            if (rec.Iscommonrecipe == 0)
                next = player.PCreateItem >= rec.Level;
            else
                next = player.PCreateCommonItem >= rec.Level;

            if (!next)
            {
                player.SendSystemMessage(SystemMessage.SystemMessageId.CreateLvlTooLowToRegister);
                player.SendActionFailed();
                return;
            }

            player.CurMp -= rec.MpConsume;
            StatusUpdate su = new StatusUpdate(player);
            su.Add(StatusUpdate.CurMp, (int)player.CurMp);
            player.SendPacket(su);

            rec.Materials.ForEach(material => player.DestroyItemById(material.Item.ItemId, material.Count));

            if (rec.SuccessRate < 100)
            {
                if (new Random().Next(0, 100) > rec.SuccessRate)
                {
                    player.SendPacket(new RecipeItemMakeInfo(player, rec, 0));
                    player.SendActionFailed();
                    return;
                }
            }

            rec.Products.ForEach(prod => player.SendPacket(new RecipeItemMakeInfo(player, rec, 1)));
        }
    }
}