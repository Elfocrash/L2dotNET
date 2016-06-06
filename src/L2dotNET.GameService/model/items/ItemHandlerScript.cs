using System.Collections.Generic;
using log4net;
using L2dotNET.GameService.Model.Playable;
using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Model.Skills2;
using L2dotNET.GameService.Network.Serverpackets;
using L2dotNET.GameService.World;

namespace L2dotNET.GameService.Model.Items
{
    class ItemHandlerScript : ItemEffect
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ItemHandlerScript));
        private readonly int id;

        public int EffectID = -1;
        public int EffectLv;

        public bool Pet = false;
        public bool Player = true;
        public bool Destroy = true;

        public int PetID = -1;
        public int SummonID = -1;
        public int SummonStaticID = -1;

        private SortedList<int, long> ExchangeItems;
        public int SkillID;
        public int SkillLv;

        public ItemHandlerScript(int id)
        {
            this.id = id;
        }

        public void addExchangeItem(int itemId, long count)
        {
            if (ExchangeItems == null)
                ExchangeItems = new SortedList<int, long>();

            ExchangeItems.Add(itemId, count);
        }

        public override void UsePlayer(L2Player player, L2Item item)
        {
            if (!Player)
            {
                base.UsePlayer(player, item);
                return;
            }

            if (Destroy)
                player.Inventory.destroyItem(item, 1, true, true);

            calcSkill(player);
            calcEffect(player);

            if (ExchangeItems != null)
                foreach (int val in ExchangeItems.Keys)
                    player.Inventory.addItem(val, ExchangeItems[val], true, true);

            if (PetID != -1)
                player.PetSummon(item, PetID);

            if (SummonID != -1)
                player.PetSummon(item, SummonID, false);

            if (SummonStaticID != -1)
            {
                //NpcTable.Instance.SpawnNpc(SummonStaticID, player.X, player.Y, player.Z, player.Heading);
            }
        }

        public override void UsePet(L2Pet pet, L2Item item)
        {
            if (!Pet)
            {
                base.UsePet(pet, item);
                return;
            }

            calcSkill(pet);
            calcEffect(pet);

            if (SummonStaticID != -1)
            {
                //NpcTable.Instance.SpawnNpc(SummonStaticID, pet.X, pet.Y, pet.Z, pet.Heading);
            }
        }

        private void calcEffect(L2Character character)
        {
            if (EffectID != -1)
            {
                TSkill skill = TSkillTable.Instance.Get(EffectID, EffectLv);

                if (skill == null)
                {
                    log.Error($"ItemHandler: item {id} with null effect {EffectID}/{EffectLv}");
                    return;
                }

                character.addAbnormal(skill, character, true, false);
                character.broadcastPacket(new MagicSkillUse(character, character, skill, 100));
            }
        }

        private void calcSkill(L2Character character)
        {
            if (SkillID != -1)
            {
                TSkill skill = TSkillTable.Instance.Get(SkillID, SkillLv);

                if (skill == null)
                {
                    log.Error($"ItemHandler: item {id} with null skill {SkillID}/{SkillLv}");
                    return;
                }

                if (character is L2Player)
                    ((L2Player)character).castSkill(skill, false, false);
                else
                    character.castSkill(skill);
            }
        }
    }
}