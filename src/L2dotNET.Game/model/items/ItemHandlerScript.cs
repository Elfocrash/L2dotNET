using System.Collections.Generic;
using L2dotNET.Game.model.playable;
using L2dotNET.Game.model.skills2;
using L2dotNET.Game.network.l2send;
using L2dotNET.Game.tables;
using L2dotNET.Game.world;
using log4net;

namespace L2dotNET.Game.model.items
{
    class ItemHandlerScript : ItemEffect
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ItemHandlerScript));
        private int id;

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

        public void addExchangeItem(int id, long count)
        {
            if (ExchangeItems == null)
                ExchangeItems = new SortedList<int, long>();

            ExchangeItems.Add(id, count);
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
                foreach (int id in ExchangeItems.Keys)
                    player.Inventory.addItem(id, ExchangeItems[id], true, true);

            if (PetID != -1)
                player.PetSummon(item, PetID);

            if (SummonID != -1)
                player.PetSummon(item, SummonID, false);

            if (SummonStaticID != -1)
            {
                NpcTable.Instance.spawnNpc(SummonStaticID, player.X, player.Y, player.Z, player.Heading);
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
                NpcTable.Instance.spawnNpc(SummonStaticID, pet.X, pet.Y, pet.Z, pet.Heading);
            }
        }

        private void calcEffect(L2Character character)
        {
            if (EffectID != -1)
            {
                TSkill skill = TSkillTable.Instance.get(EffectID, EffectLv);

                if (skill == null)
                {
                    log.Error($"ItemHandler: item { id } with null effect { EffectID }/{ EffectLv }");
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
                TSkill skill = TSkillTable.Instance.get(SkillID, SkillLv);

                if (skill == null)
                {
                    log.Error($"ItemHandler: item { id } with null skill { SkillID }/{ SkillLv }");
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
