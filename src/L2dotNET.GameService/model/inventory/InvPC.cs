using System.Collections.Generic;
using System.Linq;
using L2dotNET.GameService.Model.Items;
using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Model.Quests;
using L2dotNET.GameService.Network.Serverpackets;
using L2dotNET.GameService.Tables;

namespace L2dotNET.GameService.Model.Inventory
{
    public class InvPC : InvTemplate
    {
        public const byte EQUIPITEM_Underwear = 0;
        public const byte EQUIPITEM_LEar = 1;
        public const byte EQUIPITEM_REar = 2;
        public const byte EQUIPITEM_Neck = 3;
        public const byte EQUIPITEM_LFinger = 4;
        public const byte EQUIPITEM_RFinger = 5;
        public const byte EQUIPITEM_Head = 6;
        public const byte EQUIPITEM_RHand = 7; //14 lrhand
        public const byte EQUIPITEM_LHand = 8;
        public const byte EQUIPITEM_Gloves = 9;
        public const byte EQUIPITEM_Chest = 10;
        public const byte EQUIPITEM_Legs = 11;
        public const byte EQUIPITEM_Feet = 12;
        public const byte EQUIPITEM_Cloak = 13;
        public const byte EQUIPITEM_Hair = 15;
        public const byte EQUIPITEM_Hair2 = 16;
        public const byte EQUIPITEM_Max = 17;

        public const int SLOT_NONE = 0;
        public const int SBT_UNDERWEAR = 1;
        public const int SBT_REAR = 2;
        public const int SBT_LEAR = 4;
        public const int SBT_RLEAR = 6;
        public const int SBT_NECK = 8;
        public const int SBT_RFINGER = 16;
        public const int SBT_LFINGER = 32;
        public const int SBT_RLFINGER = 48;
        public const int SBT_HEAD = 64;
        public const int SBT_RHAND = 128;
        public const int SBT_LHAND = 256;
        public const int SBT_GLOVES = 512;
        public const int SBT_CHEST = 1024;
        public const int SBT_LEGS = 2048;
        public const int SBT_FEET = 4096;
        public const int SBT_BACK = 8192;
        public const int SBT_RLHAND = 16384;
        public const int SBT_ONEPIECE = 32768;
        public const int SBT_HAIR = 65536;
        public const int SBT_ALLDRESS = 131072;
        public const int SBT_HAIR2 = 262144;
        public const int SBT_HAIRALL = 524288;
        public const int SBT_RBracelet = 1048576;
        public const int SBT_LBracelet = 2097152;
        public const int SBT_Deco1 = 4194304;
        public const int SBT_Waist = 268435456;

        public int[][] _paperdoll = new int[EQUIPITEM_Max][];
        public int[] _paperdollVisual = new int[EQUIPITEM_Max];

        private const byte PDOLL_ID = 0;
        private const byte PDOLL_OBJID = 1;
        private const byte PDOLL_AUGMENT = 2;

        public L2Player _owner;

        public InvPC()
        {
            for (byte i = 0; i < EQUIPITEM_Max; i++)
            {
                _paperdoll[i] = new[] { 0, 0, 0 };
                _paperdollVisual[i] = 0;
            }
        }

        public int getPaperdollId(int slot)
        {
            int visualId = _paperdollVisual[slot];

            return visualId > 0 ? visualId : _paperdoll[slot][PDOLL_ID];
        }

        public int getPaperdollObjectId(int slot)
        {
            int visualId = _paperdollVisual[slot];

            return visualId > 0 ? visualId : _paperdoll[slot][PDOLL_OBJID];
        }

        public int getPaperdollAugmentId(int slot)
        {
            int visualId = _paperdollVisual[slot];

            return visualId > 0 ? 0 : _paperdoll[slot][PDOLL_AUGMENT];
        }

        public int getWeaponEnchanment()
        {
            int visualId = _paperdollVisual[EQUIPITEM_RHand];

            if (visualId > 0)
                return 0;

            int objId = _paperdoll[EQUIPITEM_RHand][PDOLL_OBJID];

            if (objId > 0)
            {
                L2Item item = getByObject(objId);
                return item.Enchant;
            }

            return 0;
        }

        public L2Item getWeapon()
        {
            int objId = _paperdoll[EQUIPITEM_RHand][PDOLL_OBJID];

            return objId > 0 ? getByObject(objId) : null;
        }

        public int getWeaponAugmentation()
        {
            int visualId = _paperdollVisual[EQUIPITEM_RHand];

            if (visualId > 0)
                return 0;

            int objId = _paperdoll[EQUIPITEM_RHand][PDOLL_OBJID];

            if (objId > 0)
            {
                L2Item item = getByObject(objId);
                return item.AugmentationID;
            }

            return 0;
        }

        public int getWeaponObjId()
        {
            return _paperdoll[EQUIPITEM_RHand][PDOLL_OBJID];
        }

        public override void addItem(ItemTemplate template, long count, short enchant, bool msg, bool update)
        {
            foreach (QuestInfo qi in _owner._quests.Where(qi => !qi.completed))
                qi._template.onEarnItem(_owner, qi.stage, template.ItemID);

            InventoryUpdate iu = null;

            if (update)
                iu = new InventoryUpdate();

            if (!template.isStackable())
            {
                bool mass = count > 1;
                for (int i = 0; i < count; i++)
                {
                    L2Item ins = new L2Item(template);
                    ins.Enchant = enchant;
                    ins.Location = L2Item.L2ItemLocation.inventory;
                    Items.Add(ins.ObjID, ins);

                    if (update)
                        iu.addNewItem(ins);

                    ins.sql_insert(_owner.ObjID);
                    if (msg && !mass)
                    {
                        SystemMessage sm = new SystemMessage(SystemMessage.SystemMessageId.EARNED_ITEM_S1);
                        if (enchant > 0)
                        {
                            sm = new SystemMessage(SystemMessage.SystemMessageId.OBTAINED_S1_S2);
                            sm.AddNumber(enchant);
                            sm.AddItemName(template.ItemID);
                        }
                        else
                            sm.AddItemName(template.ItemID);

                        _owner.sendPacket(sm);
                    }
                }

                if (msg && mass)
                {
                    SystemMessage sm = new SystemMessage(SystemMessage.SystemMessageId.EARNED_S2_S1_S);
                    sm.AddItemName(template.ItemID);
                    sm.AddItemCount(count);
                    _owner.sendPacket(sm);
                }
            }
            else
            {
                bool find = false;
                foreach (L2Item it in Items.Values.Where(it => it.Template.ItemID == template.ItemID))
                {
                    it.Count += count;

                    if (update)
                        iu.addModItem(it);

                    it.sql_update();
                    find = true;
                    break;
                }

                if (!find)
                {
                    L2Item ins = new L2Item(template);
                    ins.Count = count;
                    ins.Location = L2Item.L2ItemLocation.inventory;
                    Items.Add(ins.ObjID, ins);

                    if (update)
                        iu.addNewItem(ins);

                    ins.sql_insert(_owner.ObjID);
                }

                if (msg)
                {
                    SystemMessage sm;
                    if (template.ItemID == 57)
                    {
                        sm = new SystemMessage(SystemMessage.SystemMessageId.EARNED_S1_ADENA);
                        sm.AddItemCount(count);
                    }
                    else if (count > 1)
                    {
                        sm = new SystemMessage(SystemMessage.SystemMessageId.EARNED_S2_S1_S);
                        sm.AddItemName(template.ItemID);
                        sm.AddItemCount(count);
                    }
                    else
                    {
                        sm = new SystemMessage(SystemMessage.SystemMessageId.EARNED_ITEM_S1);
                        sm.AddItemName(template.ItemID);
                    }

                    _owner.sendPacket(sm);
                }

                if (update)
                    _owner.sendPacket(iu);
            }

            if (template.Weight > 0)
                _owner.updateWeight();
        }

        public void addItem(L2Item item, bool msg, bool update)
        {
            item.Location = L2Item.L2ItemLocation.inventory;
            foreach (QuestInfo qi in _owner._quests.Where(qi => !qi.completed))
                qi._template.onEarnItem(_owner, qi.stage, item.Template.ItemID);

            InventoryUpdate iu = null;

            if (update)
                iu = new InventoryUpdate();

            if (!item.Template.isStackable())
            {
                Items.Add(item.ObjID, item);

                if (update)
                    iu.addNewItem(item);

                item.sql_insert(_owner.ObjID);
                if (msg)
                {
                    SystemMessage sm = new SystemMessage(SystemMessage.SystemMessageId.YOU_PICKED_UP_S1);
                    if ((item.Enchant > 0) && (item.Template.ItemID != 4443))
                    {
                        sm = new SystemMessage(SystemMessage.SystemMessageId.OBTAINED_S1_S2);
                        sm.AddNumber(item.Enchant);
                        sm.AddItemName(item.Template.ItemID);
                    }
                    else
                        sm.AddItemName(item.Template.ItemID);

                    _owner.sendPacket(sm);
                }
            }
            else
            {
                bool find = false;
                foreach (L2Item it in Items.Values.Where(it => it.Template.ItemID == item.Template.ItemID))
                {
                    it.Count += item.Count;

                    if (update)
                        iu.addModItem(it);

                    it.sql_update();
                    find = true;
                    break;
                }

                if (!find)
                {
                    Items.Add(item.ObjID, item);

                    if (update)
                        iu.addNewItem(item);

                    item.sql_insert(_owner.ObjID);
                }

                if (msg)
                {
                    SystemMessage sm;
                    if (item.Template.ItemID == 57)
                    {
                        sm = new SystemMessage(SystemMessage.SystemMessageId.YOU_PICKED_UP_S1_ADENA);
                        sm.AddItemCount(item.Count);
                    }
                    else if (item.Count > 1)
                    {
                        sm = new SystemMessage(SystemMessage.SystemMessageId.YOU_PICKED_UP_S2_S1);
                        sm.AddItemName(item.Template.ItemID);
                        sm.AddItemCount(item.Count);
                    }
                    else
                    {
                        sm = new SystemMessage(SystemMessage.SystemMessageId.YOU_PICKED_UP_S1);
                        sm.AddItemName(item.Template.ItemID);
                    }

                    _owner.sendPacket(sm);
                }
            }

            if (update)
                _owner.sendPacket(iu);

            if (item.Template.Weight > 0)
                _owner.updateWeight();
        }

        public void addItem(int id, long count, short enc, bool msg, bool update)
        {
            ItemTemplate template = ItemTable.Instance.GetItem(id);

            if (template == null)
                return;

            addItem(template, count, enc, msg, update);
        }

        public void addItem(int id, long count, bool msg, bool update)
        {
            ItemTemplate template = ItemTable.Instance.GetItem(id);

            if (template == null)
                return;

            addItem(template, count, 0, msg, update);
        }

        public void setPaperdoll(int pdollId, L2Item item, bool update)
        {
            InventoryUpdate iu = null;
            if (update)
                iu = new InventoryUpdate();

            List<object[]> unequipAction = new List<object[]>();
            //int sbt = item != null ? item.Template.BodyPartId() : -1;
            byte uiUpdate = 0;
            switch (pdollId)
            {
                case SBT_RLEAR:
                    if (_paperdoll[EQUIPITEM_LEar][PDOLL_OBJID] == 0)
                        pdollId = EQUIPITEM_LEar;
                    else
                    {
                        pdollId = EQUIPITEM_REar;
                        if (_paperdoll[EQUIPITEM_REar][PDOLL_OBJID] > 0)
                            unequipAction.Add(new object[] { getByObject(_paperdoll[EQUIPITEM_REar][PDOLL_OBJID]), EQUIPITEM_REar });
                    }
                    uiUpdate = 2;
                    break;
                case SBT_RLFINGER:
                    if (_paperdoll[EQUIPITEM_LFinger][PDOLL_OBJID] == 0)
                        pdollId = EQUIPITEM_LFinger;
                    else
                    {
                        pdollId = EQUIPITEM_RFinger;
                        if (_paperdoll[EQUIPITEM_RFinger][PDOLL_OBJID] > 0)
                            unequipAction.Add(new object[] { getByObject(_paperdoll[EQUIPITEM_RFinger][PDOLL_OBJID]), EQUIPITEM_RFinger });
                    }
                    uiUpdate = 2;
                    break;
                case SBT_RLHAND:
                    if (_paperdoll[EQUIPITEM_LHand][PDOLL_OBJID] > 0)
                    {
                        unequipAction.Add(new object[] { getByObject(_paperdoll[EQUIPITEM_LHand][PDOLL_OBJID]), EQUIPITEM_LHand });
                        uiUpdate = 1;
                    }
                    break;
                case SBT_LHAND:
                    if (_paperdoll[EQUIPITEM_RHand][PDOLL_OBJID] > 0)
                    {
                        L2Item temp = getByObject(_paperdoll[EQUIPITEM_RHand][PDOLL_OBJID]);
                        if ((temp != null) && (temp.Template.BodyPartId() == SBT_RLHAND))
                        {
                            unequipAction.Add(new object[] { temp, EQUIPITEM_RHand });
                            uiUpdate = 1;
                        }
                    }
                    break;
                case SBT_ONEPIECE:
                    if (_paperdoll[EQUIPITEM_Legs][PDOLL_OBJID] > 0)
                    {
                        unequipAction.Add(new object[] { getByObject(_paperdoll[EQUIPITEM_Legs][PDOLL_OBJID]), EQUIPITEM_Legs });
                        uiUpdate = 1;
                    }
                    break;
                case SBT_ALLDRESS:
                    if (_paperdoll[EQUIPITEM_Head][PDOLL_OBJID] > 0)
                        unequipAction.Add(new object[] { getByObject(_paperdoll[EQUIPITEM_Head][PDOLL_OBJID]), EQUIPITEM_Head });
                    if (_paperdoll[EQUIPITEM_Gloves][PDOLL_OBJID] > 0)
                        unequipAction.Add(new object[] { getByObject(_paperdoll[EQUIPITEM_Gloves][PDOLL_OBJID]), EQUIPITEM_Gloves });
                    if (_paperdoll[EQUIPITEM_Legs][PDOLL_OBJID] > 0)
                        unequipAction.Add(new object[] { getByObject(_paperdoll[EQUIPITEM_Legs][PDOLL_OBJID]), EQUIPITEM_Legs });
                    if (_paperdoll[EQUIPITEM_Feet][PDOLL_OBJID] > 0)
                        unequipAction.Add(new object[] { getByObject(_paperdoll[EQUIPITEM_Feet][PDOLL_OBJID]), EQUIPITEM_Feet });
                    uiUpdate = 1;
                    break;
                case SBT_HAIRALL:
                    if (_paperdoll[EQUIPITEM_Hair2][PDOLL_OBJID] > 0)
                    {
                        unequipAction.Add(new object[] { getByObject(_paperdoll[EQUIPITEM_Hair2][PDOLL_OBJID]), EQUIPITEM_Hair2 });
                        uiUpdate = 1;
                    }
                    break;

                default:
                    if (_paperdoll[pdollId][PDOLL_OBJID] != 0)
                    {
                        L2Item old = getByObject(_paperdoll[pdollId][PDOLL_OBJID]);
                        unequipAction.Add(new object[] { old, pdollId });
                    }
                    break;
            }

            switch (uiUpdate)
            {
                case 1:
                    _owner.broadcastUserInfo();
                    break;
                case 2:
                    _owner.sendPacket(new UserInfo(_owner));
                    break;
            }

            if (unequipAction.Count > 0)
                foreach (object[] oa in unequipAction)
                {
                    L2Item temp = oa[0] as L2Item;
                    if (temp != null)
                    {
                        temp.unequip(_owner);
                        if (update)
                            iu.addModItem(temp);
                    }

                    int dollId = (int)oa[1];
                    _paperdoll[dollId][PDOLL_ID] = 0;
                    _paperdoll[dollId][PDOLL_OBJID] = 0;
                    _paperdoll[dollId][PDOLL_AUGMENT] = 0;
                }

            if (item == null)
            {
                if (update)
                    _owner.sendPacket(iu);
                return;
            }

            _paperdoll[pdollId][PDOLL_ID] = item.Template.ItemID;
            _paperdoll[pdollId][PDOLL_OBJID] = item.ObjID;
            _paperdoll[pdollId][PDOLL_AUGMENT] = item.AugmentationID;
            item.equip(_owner);
            item._paperdollSlot = pdollId;
            if (update)
            {
                iu.addModItem(item);
                _owner.sendPacket(iu);
            }
        }

        public void setPaperdollDirect(int pdollId, L2Item item)
        {
            _paperdoll[pdollId][PDOLL_ID] = item.Template.ItemID;
            _paperdoll[pdollId][PDOLL_OBJID] = item.ObjID;
            _paperdoll[pdollId][PDOLL_AUGMENT] = item.AugmentationID;
        }

        public L2Item getByObject(int obj)
        {
            return Items[obj];
        }

        public int getPaperdollIdByMask(int mask)
        {
            switch (mask)
            {
                case SBT_UNDERWEAR:
                    return EQUIPITEM_Underwear;
                case SBT_BACK:
                    return EQUIPITEM_Cloak;
                case SBT_REAR:
                    return EQUIPITEM_REar;
                case SBT_LEAR:
                    return EQUIPITEM_LEar;
                case SBT_NECK:
                    return EQUIPITEM_Neck;
                case SBT_RFINGER:
                    return EQUIPITEM_RFinger;
                case SBT_LFINGER:
                    return EQUIPITEM_LFinger;
                case SBT_HEAD:
                    return EQUIPITEM_Head;
                case SBT_RHAND:
                case SBT_RLHAND:
                    return EQUIPITEM_RHand;
                case SBT_LHAND:
                    return EQUIPITEM_LHand;
                case SBT_GLOVES:
                    return EQUIPITEM_Gloves;
                case SBT_CHEST:
                case SBT_ONEPIECE:
                case SBT_ALLDRESS:
                    return EQUIPITEM_Chest;
                case SBT_LEGS:
                    return EQUIPITEM_Legs;
                case SBT_FEET:
                    return EQUIPITEM_Feet;
                case SBT_HAIR:
                    return EQUIPITEM_Hair;
                case SBT_HAIR2:
                    return EQUIPITEM_Hair2;
                case SBT_HAIRALL:
                    return EQUIPITEM_Hair2;
            }

            return -1;
        }

        public int getPaperdollId(ItemTemplate item)
        {
            int id = 0;
            switch (item.Bodypart)
            {
                case ItemTemplate.L2ItemBodypart.underwear:
                    id = EQUIPITEM_Underwear;
                    break;
                case ItemTemplate.L2ItemBodypart.neck:
                    id = EQUIPITEM_Neck;
                    break;
                case ItemTemplate.L2ItemBodypart.head:
                    id = EQUIPITEM_Head;
                    break;
                case ItemTemplate.L2ItemBodypart.rhand:
                    id = EQUIPITEM_RHand;
                    break;
                case ItemTemplate.L2ItemBodypart.lhand:
                    id = EQUIPITEM_LHand;
                    break;
                case ItemTemplate.L2ItemBodypart.gloves:
                    id = EQUIPITEM_Gloves;
                    break;
                case ItemTemplate.L2ItemBodypart.chest:
                    id = EQUIPITEM_Chest;
                    break;
                case ItemTemplate.L2ItemBodypart.legs:
                    id = EQUIPITEM_Legs;
                    break;
                case ItemTemplate.L2ItemBodypart.feet:
                    id = EQUIPITEM_Feet;
                    break;
                case ItemTemplate.L2ItemBodypart.lrhand:
                    id = EQUIPITEM_RHand;
                    break;
                case ItemTemplate.L2ItemBodypart.onepiece:
                    id = EQUIPITEM_Chest;
                    break;
                case ItemTemplate.L2ItemBodypart.hair:
                    id = EQUIPITEM_Hair;
                    break;
                case ItemTemplate.L2ItemBodypart.alldress:
                    id = EQUIPITEM_Chest;
                    break;
                case ItemTemplate.L2ItemBodypart.hair2:
                    id = EQUIPITEM_Hair2;
                    break;
                case ItemTemplate.L2ItemBodypart.hairall:
                    id = EQUIPITEM_Hair2;
                    break;
                case ItemTemplate.L2ItemBodypart.ears:
                {
                    if (_paperdoll[EQUIPITEM_REar][PDOLL_OBJID] == 0)
                        id = EQUIPITEM_REar;
                    else if (_paperdoll[EQUIPITEM_LEar][PDOLL_OBJID] == 0)
                        id = EQUIPITEM_LEar;
                    else
                        id = EQUIPITEM_LEar;
                }
                    break;
                case ItemTemplate.L2ItemBodypart.fingers:
                {
                    if (_paperdoll[EQUIPITEM_RFinger][PDOLL_OBJID] == 0)
                        id = EQUIPITEM_RFinger;
                    else if (_paperdoll[EQUIPITEM_LFinger][PDOLL_OBJID] == 0)
                        id = EQUIPITEM_LFinger;
                    else
                        id = EQUIPITEM_LFinger;
                }
                    break;

                case ItemTemplate.L2ItemBodypart.back:
                    id = EQUIPITEM_Cloak;
                    break;
            }

            return id;
        }

        public void removeItem(L2Item item)
        {
            item.sql_delete();
            lock (Items)
            {
                Items.Remove(item.ObjID);
            }

            if (item.Template.Weight > 0)
                _owner.updateWeight();
        }

        public ItemTemplate.L2ItemWeaponType getWeaponClass()
        {
            if (_paperdoll[EQUIPITEM_RHand][PDOLL_OBJID] == 0)
                return ItemTemplate.L2ItemWeaponType.none;

            L2Item item = getByObject(_paperdoll[EQUIPITEM_RHand][PDOLL_OBJID]);

            return item.Template.WeaponType;
        }

        public long getItemCount(int id)
        {
            long count = 0;
            foreach (L2Item item in Items.Values.Where(item => item.Template.ItemID == id))
            {
                count += item.Count;

                if (item.Template.isStackable())
                    break;
            }

            return count;
        }

        public L2Item getItemById(int id)
        {
            return Items.Values.FirstOrDefault(item => item.Template.ItemID == id);
        }

        public bool hasAllOfThis(int[] px)
        {
            byte ctx = (byte)Items.Values.Count(item => px.Any(i => item.Template.ItemID == i));
            return (byte)px.Length == ctx;
        }

        public bool hasSomeOfThis(int[] px)
        {
            byte ctx = (byte)Items.Values.Count(item => px.Any(i => item.Template.ItemID == i));
            return ctx > 0;
        }

        public void destroyItem(int id, long count, bool msg, bool update)
        {
            //long reqc = count;
            InventoryUpdate iu = null;
            if (update)
                iu = new InventoryUpdate();

            SystemMessage sm = null;
            if (msg)
                sm = new SystemMessage(count == 1 ? SystemMessage.SystemMessageId.S1_DISAPPEARED : SystemMessage.SystemMessageId.S2_S1_DISAPPEARED);

            bool weightUp = false;
            List<int> nulled = new List<int>();
            bool nonstackmass = false;
            int iditem = 0;
            foreach (L2Item item in Items.Values.Where(item => item.Template.ItemID == id))
            {
                weightUp = item.Template.Weight > 0;

                if (item.Template.isStackable())
                {
                    if (item.Count > count)
                    {
                        item.Count -= count;
                        if (update)
                            iu.addModItem(item);

                        item.sql_update();
                    }
                    else
                    {
                        nulled.Add(item.ObjID);
                        if (update)
                            iu.addDelItem(item);

                        item.sql_delete();
                    }

                    if (msg)
                    {
                        sm.AddItemName(item.Template.ItemID);

                        if (count > 1)
                            sm.AddItemCount(count);
                    }

                    break;
                }

                if (count == 1)
                {
                    nulled.Add(item.ObjID);

                    if (update)
                        iu.addDelItem(item);

                    if (msg)
                        sm.AddItemName(item.Template.ItemID);

                    item.sql_delete();
                    break;
                }

                nonstackmass = true;
                iditem = item.Template.ItemID;
                nulled.Add(item.ObjID);
                if (update)
                    iu.addDelItem(item);

                item.sql_delete();
            }

            lock (Items)
            {
                foreach (int idd in nulled)
                    Items.Remove(idd);
            }

            if (update)
                _owner.sendPacket(iu);

            if (msg)
            {
                if (nonstackmass)
                {
                    sm.AddItemName(iditem);
                    sm.AddItemCount(count);
                }

                _owner.sendPacket(sm);
            }

            if (weightUp)
                _owner.updateWeight();
        }

        public void destroyItem(L2Item item, int count, bool msg, bool update)
        {
            InventoryUpdate iu = null;
            if (update)
                iu = new InventoryUpdate();

            SystemMessage sm = null;
            if (msg)
                sm = new SystemMessage(count == 1 ? SystemMessage.SystemMessageId.S1_DISAPPEARED : SystemMessage.SystemMessageId.S2_S1_DISAPPEARED);

            List<int> nulled = new List<int>();
            bool nonstackmass = false;
            int iditem = 0;

            if (item.Template.isStackable())
            {
                if (item.Count > count)
                {
                    item.Count -= count;
                    if (update)
                        iu.addModItem(item);

                    item.sql_update();
                }
                else
                {
                    nulled.Add(item.ObjID);
                    if (update)
                        iu.addDelItem(item);

                    item.sql_delete();
                }

                if (msg)
                {
                    sm.AddItemName(item.Template.ItemID);

                    if (count > 1)
                        sm.AddNumber(count);
                }
            }
            else
            {
                if (count == 1)
                {
                    nulled.Add(item.ObjID);

                    if (update)
                        iu.addDelItem(item);

                    if (msg)
                        sm.AddItemName(item.Template.ItemID);

                    item.sql_delete();
                }
                else
                {
                    nonstackmass = true;
                    iditem = item.Template.ItemID;
                    nulled.Add(item.ObjID);
                    if (update)
                        iu.addDelItem(item);

                    item.sql_delete();
                }
            }

            lock (Items)
                foreach (int id in nulled)
                    Items.Remove(id);

            if (update)
                _owner.sendPacket(iu);

            if (msg)
            {
                if (nonstackmass)
                {
                    sm.AddItemName(iditem);
                    sm.AddNumber(count);
                }

                _owner.sendPacket(sm);
            }

            if (item.Template.Weight > 0)
                _owner.updateWeight();
        }

        public void destroyItemAll(int id, bool msg, bool update)
        {
            InventoryUpdate iu = null;
            if (update)
                iu = new InventoryUpdate();

            bool weightUp = false;
            List<int> nulled = new List<int>();
            foreach (L2Item item in Items.Values.Where(item => item.Template.ItemID == id))
            {
                weightUp = item.Template.Weight > 0;
                if (item.Template.isStackable())
                {
                    long c = item.Count;
                    nulled.Add(item.ObjID);
                    if (update)
                        iu.addDelItem(item);

                    item.sql_delete();

                    if (msg)
                    {
                        SystemMessage sm = new SystemMessage(SystemMessage.SystemMessageId.S2_S1_DISAPPEARED);
                        sm.AddItemName(item.Template.ItemID);
                        sm.AddItemCount(c);

                        _owner.sendPacket(sm);
                    }

                    break;
                }

                nulled.Add(item.ObjID);

                if (update)
                    iu.addDelItem(item);

                if (msg)
                {
                    SystemMessage sm = new SystemMessage(SystemMessage.SystemMessageId.S2_S1_DISAPPEARED);
                    sm.AddItemName(item.Template.ItemID);
                    sm.AddNumber(1);

                    _owner.sendPacket(sm);
                }

                item.sql_delete();
                break;
            }

            lock (Items)
                foreach (int ids in nulled)
                    Items.Remove(ids);

            if (update)
                _owner.sendPacket(iu);

            if (weightUp)
                _owner.updateWeight();
        }

        public void transferHere(L2Player target, List<long[]> items, bool update)
        {
            InventoryUpdate iuTarget = update ? new InventoryUpdate() : null;
            InventoryUpdate iuMe = update ? new InventoryUpdate() : null;
            List<int> nulled = new List<int>();
            foreach (L2Item item in target.Inventory.Items.Values)
                foreach (long[] itemd in items)
                    if (item.ObjID == itemd[0])
                    {
                        bool ex = Items.Values.Any(itp => itp.Template.ItemID == item.Template.ItemID);

                        if (item.Template.isStackable())
                        {
                            if (itemd[1] >= item.Count)
                            {
                                nulled.Add((int)itemd[0]);
                                if (ex)
                                {
                                    foreach (L2Item itp in Items.Values.Where(itp => itp.Template.ItemID == item.Template.ItemID))
                                    {
                                        itp.Count += item.Count;
                                        itp.sql_update();
                                        if (update)
                                            iuMe.addModItem(itp);
                                        break;
                                    }

                                    item.sql_delete();
                                }
                                else
                                {
                                    item.Location = L2Item.L2ItemLocation.inventory;
                                    Items.Add(item.ObjID, item);
                                    item.sql_update();

                                    if (update)
                                        iuMe.addNewItem(item);
                                }

                                if (update)
                                    iuTarget.addDelItem(item);
                            }
                            else
                            {
                                item.Count -= itemd[1];

                                if (ex)
                                {
                                    foreach (L2Item itp in Items.Values.Where(itp => itp.Template.ItemID == item.Template.ItemID))
                                    {
                                        itp.Count += itemd[1];
                                        if (update)
                                            iuMe.addModItem(itp);
                                        break;
                                    }
                                }
                                else
                                {
                                    L2Item ins = new L2Item(item.Template);
                                    ins.Count = itemd[1];
                                    ins.Location = L2Item.L2ItemLocation.inventory;
                                    Items.Add(ins.ObjID, ins);

                                    ins.sql_insert(target.ObjID);

                                    if (update)
                                        iuMe.addNewItem(ins);
                                }

                                if (update)
                                    iuTarget.addModItem(item);
                            }
                        }
                        else
                        {
                            nulled.Add((int)itemd[0]);
                            item.Location = L2Item.L2ItemLocation.inventory;
                            Items.Add(item.ObjID, item);

                            item.sql_update();

                            if (update)
                            {
                                iuTarget.addDelItem(item);
                                iuMe.addNewItem(item);
                            }
                        }
                    }

            lock (target.Inventory.Items)
            {
                foreach (int itemd in nulled)
                    target.Inventory.Items.Remove(itemd);
            }

            if (update)
            {
                target.sendPacket(iuTarget);
                _owner.sendPacket(iuMe);
            }
        }

        //public void transferFrom(L2Player target, List<long[]> transfer, bool update)
        //{
        //    InventoryUpdate iuTarget = update ? new InventoryUpdate() : null;
        //    InventoryUpdate iuMe = update ? new InventoryUpdate() : null;
        //    List<int> nulled = new List<int>();
        //    foreach (L2Item item in Items.Values)
        //    {
        //        foreach (long[] itemd in transfer)
        //        {
        //            if (item.ObjID == itemd[0])
        //            {
        //                bool ex = false;
        //                foreach (L2Item itp in target.getAllItems())
        //                    if (itp.Template.ItemID == item.Template.ItemID)
        //                    {
        //                        ex = true;
        //                        break;
        //                    }

        //                if (item.Template.isStackable())
        //                {
        //                    if (itemd[1] >= item.Count)
        //                    {
        //                        nulled.Add((int)itemd[0]);
        //                        if (ex)
        //                        {
        //                            foreach (L2Item itp in target.getAllItems())
        //                                if (itp.Template.ItemID == item.Template.ItemID)
        //                                {
        //                                    itp.Count += item.Count;
        //                                    itp.sql_update();
        //                                    if (update)
        //                                    {
        //                                        iuTarget.addModItem(itp);
        //                                        iuMe.addDelItem(item);
        //                                    }

        //                                    break;
        //                                }

        //                            item.sql_delete();
        //                        }
        //                        else
        //                        {
        //                            item.Location = L2Item.L2ItemLocation.inventory;
        //                            target.Inventory.Items.Add(item.ObjID, item);
        //                            item.sql_update();

        //                            if (update)
        //                            {
        //                                iuTarget.addNewItem(item);
        //                                iuMe.addDelItem(item);
        //                            }
        //                        }
        //                    }
        //                    else
        //                    {
        //                        item.Count -= itemd[1];
        //                        if (ex)
        //                        {
        //                            foreach (L2Item itp in target.getAllItems())
        //                                if (itp.Template.ItemID == item.Template.ItemID)
        //                                {
        //                                    itp.Count += itemd[1];
        //                                    itp.sql_update();
        //                                    if (update)
        //                                    {
        //                                        iuTarget.addModItem(itp);
        //                                        iuMe.addModItem(item);
        //                                    }

        //                                    break;
        //                                }
        //                        }
        //                        else
        //                        {
        //                            L2Item ins = new L2Item(item.Template);
        //                            ins.Count = itemd[1];
        //                            ins.Location = L2Item.L2ItemLocation.inventory;
        //                            target.Inventory.Items.Add(ins.ObjID, ins);
        //                            ins.sql_insert(target.ObjID);

        //                            if (update)
        //                            {
        //                                iuTarget.addNewItem(ins);
        //                                iuMe.addDelItem(item);
        //                            }
        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    nulled.Add((int)itemd[0]);
        //                    item.Location = L2Item.L2ItemLocation.inventory;
        //                    target.Inventory.Items.Add(item.ObjID, item);

        //                    item.sql_update();

        //                    if (update)
        //                    {
        //                        iuTarget.addNewItem(item);
        //                        iuMe.addDelItem(item);
        //                    }
        //                }

        //                foreach (QuestInfo qi in target._quests)
        //                    if (!qi.completed)
        //                        qi._template.onEarnItem(target, qi.stage, item.Template.ItemID);
        //            }
        //        }
        //    }

        //    lock (Items)
        //    {
        //        foreach (int itemd in nulled)
        //        {
        //            Items.Remove(itemd);
        //        }
        //    }

        //    if (update)
        //    {
        //        target.sendPacket(iuTarget);
        //        target.sendPacket(iuMe);
        //    }
        //}
    }
}