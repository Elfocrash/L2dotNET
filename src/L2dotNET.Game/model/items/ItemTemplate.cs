using System;
using System.Collections.Generic;
using L2dotNET.Game.model.inventory;
using L2dotNET.Game.model.skills2;
using L2dotNET.Game.model.skills2.effects;

namespace L2dotNET.Game.model.items
{
    public class ItemTemplate
    {
        public int ItemID;

        public L2ItemBodypart Bodypart;

        public int Durability = -1;

        public L2ItemType Type = L2ItemType.etcitem;
        public L2ItemArmorType _type_armor = L2ItemArmorType.none;
        public L2ItemConsume StackType = L2ItemConsume.normal;
        public L2ItemGrade CrystallGrade = L2ItemGrade.none;
        public L2ItemWeaponType WeaponType = L2ItemWeaponType.sword;

        public int _recipeId = 0;
        public int Weight = 0;
        public long _cryCount = 0;

        public int MpConsume = 0;
        public int LimitedMinutes = 0;
        public bool isAutoSS
        {
            get 
            {
                return default_action == "action_soulshot" || default_action == "action_spiritshot" || default_action == "action_summon_soulshot" || default_action == "action_summon_spiritshot";
            }
        }

        public int SoulshotCount = 0, SpiritshotCount = 0;
        public long Price = 0;
        public int AbnormalMaskEvent = 0;

      //  public TSkill critical_attack_skill, attack_skill, magic_skill;

        public int is_trade = 1, is_drop = 1, is_destruct = 1, enchant_enable = 0;
        public int is_private_store = 1, elemental_enable = 0, is_olympiad_can_use = 1, is_premium = 0;
        public int physical_damage, random_damage, attack_range, attack_speed, magical_damage;
        public int physical_defense, magical_defense, mp_bonus, magic_weapon;

        public int avoid_modify, shield_defense, shield_defense_rate;
        public double hit_modify;

        public int BaseAttrAttackType = -2;
        public int BaseAttrAttackValue;
        public int BaseAttrDefenseValueFire;
        public int BaseAttrDefenseValueWater;
        public int BaseAttrDefenseValueWind;
        public int BaseAttrDefenseValueEarth;
        public int BaseAttrDefenseValueHoly;
        public int BaseAttrDefenseValueUnholy;

        public int getCrystallId()
        {
            switch (CrystallGrade)
            {
                case L2ItemGrade.d:
                    return 1458;
                case L2ItemGrade.c:
                    return 1459;
                case L2ItemGrade.b:
                    return 1460;
                case L2ItemGrade.a:
                    return 1461;
                case L2ItemGrade.s:
                case L2ItemGrade.s80:
                case L2ItemGrade.s84:
                case L2ItemGrade.s86:
                    return 1462;
            }

            return 0;
        }

        public int[] getSoulshots()
        {
            switch (CrystallGrade)
            {
                case L2ItemGrade.none:
                    return new int[] { 1835, 5789 };
                case L2ItemGrade.d:
                    return new int[] { 1463, 22082 };
                case L2ItemGrade.c:
                    return new int[] { 1464, 22083 };
                case L2ItemGrade.b:
                    return new int[] { 1465, 22084 };
                case L2ItemGrade.a:
                    return new int[] { 1466, 22085 };
                case L2ItemGrade.s:
                case L2ItemGrade.s80:
                case L2ItemGrade.s84:
                case L2ItemGrade.s86:
                    return new int[] { 1467, 22086 };
            }

            return null;
        }

        public enum L2ItemType
        {
            weapon, armor, etcitem, questitem, accessary, asset
        }

        public enum L2ItemArmorType
        {
            none, magic, light, heavy
        }

        public enum L2ItemConsume
        {
            normal, stackable
        }

        public enum L2ItemGrade
        {
            none, d, c, b, a, s, s80, s84, s86
        }

        public enum L2ItemWeaponType
        {
            none,
            shield,
            sword,
            blunt,
            dagger,
            pole,
            fist,
            dualfist,
            bow,
            etc,//book
            dual,
            fishingrod,
            rapier,
            crossbow,
            ancientsword,
            dualdagger,
            flag,
            ownthing
        }

        public enum L2ItemBodypart
        {
            none,
            
            
            lhand,

            back, //cloak
            
            
            hair,
            alldress, //suit

            rbracelet, //belt? the same as lbracelet
            lbracelet, //bracelet
            deco1, //talisman
            sigil,
            waist, //belt



            //pts
            rhand,
            lrhand,
            chest,
            legs,
            feet,
            head,
            gloves,
            onepiece,
            ears,
            fingers,
            neck,
            underwear, //shirt
            hair2,
            hairall,
        }

        public short can_equip_sex = -1;
        public bool canEquipSex(int _sex)
        {
            if (can_equip_sex == -1)
                return true;

            return _sex == can_equip_sex;
        }

        public short can_equip_agit = -1;
        public bool canEquipAgit(int _agitId)
        {
            if (can_equip_agit == -1)
                return true;

            return _agitId == can_equip_agit;
        }

        public short can_equip_castle = -1;
        public bool canEquipCastle(int _castleId)
        {
            if (can_equip_castle == -1)
                return true;

            return _castleId == can_equip_castle;
        }

        public short can_equip_hero = -1;
        public bool canEquipHeroic(int _heroic)
        {
            if (can_equip_hero == -1)
                return true;

            return _heroic == can_equip_hero;
        }

        public short can_equip_nobless = -1;
        public bool canEquipNobless(int _noble)
        {
            if (can_equip_nobless == -1)
                return true;

            return _noble == can_equip_nobless;
        }

        public short can_equip_chaotic = -1;
        public string _htmFile;

        public bool canEquipChaotic(int _pkkills)
        {
            if (can_equip_chaotic == -1)
                return true;

            return _pkkills == 0;
        }

        public bool isStackable()
        {
            return StackType == L2ItemConsume.stackable;
        }

        public short Type1()
        {
            short val = 0;
            switch (Type)
            {
                case L2ItemType.weapon:
                    switch (WeaponType)
                    {
                        case L2ItemWeaponType.shield:
                            val = 2;
                            break;

                        default:
                            val = 0;
                            break;
                    }
                    break;
                case L2ItemType.armor:
                    val = 1;
                    break;
                case L2ItemType.accessary:
                    val = 3;
                    break;
                case L2ItemType.questitem:
                    val = 4;
                    break;
            }

            return val;
        }

        public short Type2()
        {
            short val = 0;
            switch (Type)
            {
                case L2ItemType.weapon:
                    val = 0;
                    break;
                case L2ItemType.armor:
                    val = 1;
                    break;
                case L2ItemType.accessary:
                    val = 2;
                    break;
                case L2ItemType.questitem:
                    val = 3;
                    break;
                case L2ItemType.asset:
                    val = 4;
                    break;
                case L2ItemType.etcitem:
                    val = 5;
                    break;
            }

            return val;
        }

        public int BodyPartId()
        {
            int id = 0;
            switch (Bodypart)
            {
                case L2ItemBodypart.underwear:
                    id = InvPC.SBT_UNDERWEAR; break;
                case L2ItemBodypart.ears:
                    id = InvPC.SBT_RLEAR; break;
                case L2ItemBodypart.neck:
                    id = 8; break;
                case L2ItemBodypart.fingers:
                    id = 16 | 32; break;
                case L2ItemBodypart.head:
                    id = 64; break;
                case L2ItemBodypart.rhand:
                    id = 128; break;
                case L2ItemBodypart.lhand:
                    id = 256; break;
                case L2ItemBodypart.gloves:
                    id = 512; break;
                case L2ItemBodypart.chest:
                    id = 1024; break;
                case L2ItemBodypart.legs:
                    id = 2048; break;
                case L2ItemBodypart.feet:
                    id = 4096; break;
                case L2ItemBodypart.back:
                    id = InvPC.SBT_BACK; break;
                case L2ItemBodypart.lrhand:
                    id = 16384; break;
                case L2ItemBodypart.onepiece:
                    id = InvPC.SBT_ONEPIECE; break;
                case L2ItemBodypart.hair:
                    id = InvPC.SBT_HAIR; break;
                case L2ItemBodypart.alldress:
                    id = InvPC.SBT_ALLDRESS; break;
                case L2ItemBodypart.hair2:
                    id = InvPC.SBT_HAIR2; break;
                case L2ItemBodypart.hairall:
                    id = InvPC.SBT_HAIRALL; break;
                case L2ItemBodypart.rbracelet:
                    id = InvPC.SBT_RBracelet; break;
                case L2ItemBodypart.deco1:
                    id = InvPC.SBT_Deco1; break;
                case L2ItemBodypart.lbracelet:
                    id = InvPC.SBT_LBracelet; break;
                case L2ItemBodypart.waist:
                    id = InvPC.SBT_Waist; break;
            }

            return id;
        }

        public bool SetItem = false;
        public List<TEffect> stats = new List<TEffect>();
        public string default_action;
        public int immediate_effect = 1;
        public string armor_type;
        public string etcitem_type;
        public int delay_share_group = -1;
        public int blessed = 0;
        public int ex_immediate_effect;
        public int drop_period = 10;
        public int ex_drop_period;
        public int use_skill_distime;
        public int equip_reuse_delay;
        public int keep_type;
        public int can_move;
        public int critical;
        public int reuse_delay;
        public short enchanted = 0;
        public int for_npc;
        public void buildEffect()
        {
            if(physical_damage > 0)
            {
                p_physical_attack b = new p_physical_attack();
                b.type = TEffectType.p_physical_defense;
                b.HashID = ItemID;
                b.build("p_physical_attack all +" + physical_damage);
                stats.Add(b);
            }

            if (physical_defense > 0)
            {
                p_physical_defence b = new p_physical_defence();
                b.type = TEffectType.p_physical_defense;
                b.HashID = ItemID;
                b.build("p_physical_defense all +" + physical_defense);
                stats.Add(b);
            }

            if (magical_damage > 0)
            {
                p_magical_attack b = new p_magical_attack();
                b.type = TEffectType.p_magical_attack;
                b.HashID = ItemID;
                b.build("p_magical_attack all +" + magical_damage);
                stats.Add(b);
            }

            if (magical_defense > 0)
            {
                p_magical_defence b = new p_magical_defence();
                b.type = TEffectType.p_magical_defense;
                b.HashID = ItemID;
                b.build("p_magical_defense all +" + magical_defense);
                stats.Add(b);
            }

            if (shield_defense > 0)
            {
                p_physical_shield_defence b = new p_physical_shield_defence();
                b.type = TEffectType.p_physical_shield_defence;
                b.HashID = ItemID;
                b.build("p_physical_shield_defence +" + shield_defense);
                stats.Add(b);
            }
        }

        public List<TSkill> multiSkills;
        public void addMultiSkills(string value)
        {
            if (multiSkills == null)
                multiSkills = new List<TSkill>();

            foreach (string v in value.Split(';'))
            {
                TSkill sk = TSkillTable.Instance.Get(int.Parse(v.Split('-')[0]), int.Parse(v.Split('-')[1]));
                if (sk != null)
                    multiSkills.Add(sk);
            }

            if (multiSkills.Count == 0)
                multiSkills = null;
        }

        internal void setReducingSoulShots(string value)
        {
            
        }

        internal void setReducingMpConsume(string value)
        {
            
        }

        internal void setEquipCondition(string value)
        {
            
        }

        internal void setDamageRange(string value)
        {
            
        }

        internal void setAttributeAttack(string value)
        {
            
        }

        internal void setAttributeDefend(string value)
        {
           
        }
        public TSkill unequip_skill;
        public void setUnequipSkill(string value)
        {
            unequip_skill = TSkillTable.Instance.Get(int.Parse(value.Split('-')[0]), int.Parse(value.Split('-')[1]));
        }

        internal void setEquipOption(string value)
        {
           
        }

        internal void setUseCondition(string value)
        {
            
        }

        public TSkill item_skill;
        public void addItemSkill(string value)
        {
            item_skill = TSkillTable.Instance.Get(int.Parse(value.Split('-')[0]), int.Parse(value.Split('-')[1]));
        }

        public TSkill item_skill_ench4;
        public short AttrDefenseValueUnholy;
        public short AttrDefenseValueHoly;
        public short AttrDefenseValueEarth;
        public short AttrDefenseValueWind;
        public short AttrDefenseValueWater;
        public short AttrDefenseValueFire;
        public short AttrAttackValue;
        public short AttrAttackType = -2;
        public void addItemEnch4(string value)
        {
            item_skill_ench4 = TSkillTable.Instance.Get(int.Parse(value.Split('-')[0]), int.Parse(value.Split('-')[1]));
        }
    }
}
