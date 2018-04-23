using System.Collections.Generic;

namespace L2dotNET.Models.Items
{
    public class ItemSlots
    {
        public static Dictionary<string, int> ToDictionary()
        {
            Dictionary<string, int> slots = new Dictionary<string, int>
            {
                {"chest", ItemTemplate.SlotChest},
                {"fullarmor", ItemTemplate.SlotFullArmor},
                {"alldress", ItemTemplate.SlotAlldress},
                {"head", ItemTemplate.SlotHead},
                {"hair", ItemTemplate.SlotHair},
                {"face", ItemTemplate.SlotFace},
                {"dhair", ItemTemplate.SlotHairall},
                {"underwear", ItemTemplate.SlotUnderwear},
                {"back", ItemTemplate.SlotBack},
                {"neck", ItemTemplate.SlotNeck},
                {"legs", ItemTemplate.SlotLegs},
                {"feet", ItemTemplate.SlotFeet},
                {"gloves", ItemTemplate.SlotGloves},
                {"chest,legs", ItemTemplate.SlotChest | ItemTemplate.SlotLegs},
                {"rhand", ItemTemplate.SlotRHand},
                {"lhand", ItemTemplate.SlotLHand},
                {"lrhand", ItemTemplate.SlotLrHand},
                {"rear,lear", ItemTemplate.SlotREar | ItemTemplate.SlotLEar},
                {"rfinger,lfinger", ItemTemplate.SlotRFinger | ItemTemplate.SlotLFinger},
                {"none", ItemTemplate.SlotNone},
                {"wolf", ItemTemplate.SlotWolf},
                {"hatchling", ItemTemplate.SlotHatchling},
                {"strider", ItemTemplate.SlotStrider},
                {"babypet", ItemTemplate.SlotBabypet}
            };
            return slots;
        }
    }
}