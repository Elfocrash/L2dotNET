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
        private static readonly ILog Log = LogManager.GetLogger(typeof(ItemHandlerScript));
        private readonly int _id;

        public int EffectId = -1;
        public int EffectLv;

        public bool Pet = false;
        public bool Player = true;
        public bool Destroy = true;

        public int PetId = -1;
        public int SummonId = -1;
        public int SummonStaticId = -1;

        private SortedList<int, int> _exchangeItems;
        public int SkillId;
        public int SkillLv;

        public ItemHandlerScript(int id)
        {
            _id = id;
        }

        public void AddExchangeItem(int itemId, int count)
        {
            if (_exchangeItems == null)
            {
                _exchangeItems = new SortedList<int, int>();
            }

            _exchangeItems.Add(itemId, count);
        }

        public override void UsePlayer(L2Player player, L2Item item)
        {
            if (!Player)
            {
                base.UsePlayer(player, item);
                return;
            }

            if (Destroy)
            {
                player.DestroyItem(item, 1);
            }

            CalcSkill(player);
            CalcEffect(player);

            if (_exchangeItems != null)
            {
                foreach (int val in _exchangeItems.Keys)
                {
                    player.AddItem(val, _exchangeItems[val]);
                }
            }

            if (PetId != -1)
            {
                player.PetSummon(item, PetId);
            }

            if (SummonId != -1)
            {
                player.PetSummon(item, SummonId, false);
            }

            if (SummonStaticId != -1)
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

            CalcSkill(pet);
            CalcEffect(pet);

            if (SummonStaticId != -1)
            {
                //NpcTable.Instance.SpawnNpc(SummonStaticID, pet.X, pet.Y, pet.Z, pet.Heading);
            }
        }

        private void CalcEffect(L2Character character)
        {
            if (EffectId == -1)
            {
                return;
            }

            Skill skill = SkillTable.Instance.Get(EffectId, EffectLv);

            if (skill == null)
            {
                Log.Error($"ItemHandler: item {_id} with null effect {EffectId}/{EffectLv}");
                return;
            }

            character.AddAbnormal(skill, character, true, false);
            character.BroadcastPacket(new MagicSkillUse(character, character, skill, 100));
        }

        private void CalcSkill(L2Character character)
        {
            if (SkillId == -1)
            {
                return;
            }

            Skill skill = SkillTable.Instance.Get(SkillId, SkillLv);

            if (skill == null)
            {
                Log.Error($"ItemHandler: item {_id} with null skill {SkillId}/{SkillLv}");
                return;
            }

            if (character is L2Player)
            {
                ((L2Player)character).CastSkill(skill, false, false);
            }
            else
            {
                character.CastSkill(skill);
            }
        }
    }
}