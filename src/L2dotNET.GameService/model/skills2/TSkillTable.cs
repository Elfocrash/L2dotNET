using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using log4net;
using L2dotNET.GameService.Compression;
using L2dotNET.GameService.Enums;

namespace L2dotNET.GameService.Model.Skills2
{
    class TSkillTable
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(TSkillTable));
        private static volatile TSkillTable instance;
        private static readonly object syncRoot = new object();

        public static TSkillTable Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new TSkillTable();
                        }
                    }
                }

                return instance;
            }
        }

        public void Initialize()
        {
            Read();
            loadDLC();
        }

        public TSkillTable() { }

        public TSkill Get(int id, int lvl)
        {
            long hash = id * 65536 + lvl;
            return _skills.ContainsKey(hash) ? _skills[hash] : null;
        }

        public TSkill Get(int skillId)
        {
            return _skills.ContainsKey(skillId) ? _skills[skillId] : null;
        }

        public readonly SortedList<long, TSkillEnchantInfo> enchantInfo = new SortedList<long, TSkillEnchantInfo>();
        public readonly SortedList<long, TSkill> _skills = new SortedList<long, TSkill>();

        public void Read()
        {
            using (FileStream fstream = File.Open(@"dlc\skillenchant.dlc", FileMode.Open, FileAccess.Read))
            {
                byte[] dlcheader = new byte[3];
                fstream.Read(dlcheader, 0, dlcheader.Length);
                if (Encoding.UTF8.GetString(dlcheader) == "DLC")
                {
                    DlcStream dlc = new DlcStream(fstream, CompressionMode.Decompress);
                    int cnt = dlc.readD();
                    for (int a = 0; a < cnt; a++)
                    {
                        TSkillEnchantInfo inf = new TSkillEnchantInfo();
                        inf.id = dlc.readD();
                        inf.lv = dlc.readD();
                        int len = dlc.readD();
                        for (int b = 0; b < len; b++)
                        {
                            TSkillEnchantInfoDetail nfo = new TSkillEnchantInfoDetail();
                            nfo.route_id = dlc.readD();
                            nfo.enchant_id = dlc.readD();
                            nfo.enchanted_skill_level = dlc.readD();
                            nfo.importance = dlc.readC();
                            nfo.r1 = dlc.readD();
                            nfo.r2 = dlc.readD();
                            nfo.r3 = dlc.readD();
                            nfo.r4 = dlc.readD();
                            dlc.readS(10);

                            inf.details.Add(nfo.enchanted_skill_level, nfo);
                        }

                        enchantInfo.Add(inf.id * 65536 + inf.lv, inf);
                    }
                }
            }

            SortedList<int, object> _ids = new SortedList<int, object>();
            Initreg();
            using (FileStream fstream = File.Open(@"dlc\skilldb_edit.dlc", FileMode.Open, FileAccess.Read))
            {
                byte[] dlcheader = new byte[3];
                fstream.Read(dlcheader, 0, dlcheader.Length);
                if (Encoding.UTF8.GetString(dlcheader) == "DLC")
                {
                    DlcStream dlc = new DlcStream(fstream, CompressionMode.Decompress);
                    int cnt = dlc.readD();
                    for (int a = 0; a < cnt; a++)
                    {
                        byte len = dlc.readC();
                        TSkill skill = new TSkill();
                        for (byte p = 0; p < len; p++)
                        {
                            byte keyId = dlc.readC();
                            SkillLevelParam slp = ps[keyId];
                            int lenx;
                            string value;
                            switch (slp.id)
                            {
                                case 1:
                                    skill.skill_id = dlc.readD();
                                    break;
                                case 2:
                                    skill.level = dlc.readD();
                                    break;
                                case 3:
                                    lenx = dlc.readD();
                                    value = dlc.readS(lenx);
                                    skill.OpType = (TSkillOperational)Enum.Parse(typeof(TSkillOperational), value);
                                    break;
                                case 4:
                                    skill.magic_level = dlc.readD();
                                    break;
                                case 6:
                                    lenx = dlc.readD();
                                    value = dlc.readS(lenx);
                                    skill.SetEffect_effect(value);
                                    break;
                                case 8:
                                    lenx = dlc.readD();
                                    value = dlc.readS(lenx);
                                    skill.SetOperateCond(value);
                                    break;
                                case 15:
                                    skill.is_magic = (short)dlc.readD();
                                    break;
                                case 16:
                                    skill.mp_consume1 = dlc.readD();
                                    break;
                                case 17:
                                    skill.mp_consume2 = dlc.readD();
                                    break;
                                case 18:
                                    skill.cast_range = dlc.readD();
                                    break;
                                case 19:
                                    skill.effective_range = dlc.readD();
                                    break;
                                case 20:
                                    skill.skill_hit_time = (short)(dlc.readF() * 1000);
                                    break;
                                case 21:
                                    skill.skill_cool_time = (short)(dlc.readF() * 1000);
                                    break;
                                case 23:
                                    skill.reuse_delay = dlc.readF();
                                    break;
                                case 26:
                                    double rate = dlc.readF();
                                    if (rate != -1)
                                        skill.activate_rate = (short)(rate * 1000);
                                    break;
                                case 29:
                                    skill.abnormal_time = dlc.readD();
                                    break;
                                case 30:
                                    skill.abnormal_lv = dlc.readD();
                                    break;
                                case 31:
                                    lenx = dlc.readD();
                                    skill.abnormal_type = dlc.readS(lenx);
                                    break;
                                case 39: //target_type
                                    lenx = dlc.readD();
                                    value = dlc.readS(lenx);
                                    try
                                    {
                                        skill.target_type = (TSkillTarget)Enum.Parse(typeof(TSkillTarget), value);
                                    }
                                    catch (Exception)
                                    {
                                        skill.target_type = TSkillTarget.target;
                                        log.Error($"skill # {skill.skill_id} invalid target {value}");
                                    }
                                    break;
                                case 40: //affect_scope
                                    lenx = dlc.readD();
                                    value = dlc.readS(lenx);
                                    try
                                    {
                                        skill.affect_scope = (TSkillScope)Enum.Parse(typeof(TSkillScope), value);
                                    }
                                    catch
                                    {
                                        skill.affect_scope = TSkillScope.single;
                                        log.Error($"skill # {skill.skill_id} invalid scope {value}");
                                    }
                                    break;
                                case 49:
                                    skill.hp_consume = dlc.readD();
                                    break;
                                default:
                                    switch (slp.type)
                                    {
                                        case 1:
                                            dlc.readD();
                                            break;
                                        case 2:
                                            dlc.readF();
                                            break;
                                        case 3:
                                            int f = dlc.readD();
                                            dlc.readS(f);
                                            break;
                                    }
                                    break;
                            }
                        }

                        if (enchantInfo.ContainsKey(skill.HashID()))
                            skill.EnchantEnabled = 1;

                        _skills.Add(skill.HashID(), skill);

                        if (!_ids.ContainsKey(skill.skill_id))
                            _ids.Add(skill.skill_id, null);
                    }
                }
            }

            //using (StreamReader sreader = File.OpenText(@"scripts\skills.txt"))
            //{
            //    while (!sreader.EndOfStream)
            //    {
            //        string line = sreader.ReadLine();
            //        if (line.Length == 0 || line.StartsWith("#"))
            //            continue;

            //        string[] pt = line.Split('\t');

            //        TSkill skill = new TSkill();
            //        skill.ClientID = Convert.ToInt32(pt[0].Split('-')[0]);
            //        skill.Level = Convert.ToInt32(pt[0].Split('-')[1]);

            //        if (_idsEnchant.Contains(skill.HashID()))
            //            skill.EnchantEnabled = 1;

            //        skill.OpType = (TSkillOperational)Enum.Parse(typeof(TSkillOperational), pt[1]);

            //        for (byte ord = 2; ord < pt.Length; ord++)
            //        {
            //            string parameter = pt[ord];
            //            switch (parameter.Split('{')[0].ToLower())
            //            {
            //                case "effect":
            //                    skill.parseEffects(parameter.Substring(7));
            //                    break;
            //                case "is_magic":
            //                    skill.IsMagic = Convert.ToInt16(parameter.Remove(parameter.Length - 1).Substring(9));
            //                    break;
            //                case "mpconsume":
            //                    break;
            //                case "hit_time":
            //                    skill.HitTime = Convert.ToInt16(parameter.Remove(parameter.Length - 1).Substring(9));
            //                    break;
            //                case "cool_time":
            //                    skill.CoolTime = Convert.ToInt32(parameter.Remove(parameter.Length - 1).Substring(10));
            //                    break;
            //                case "reuse":
            //                    string reuse = parameter.Remove(parameter.Length - 1).Substring(6);
            //                    if (reuse.Contains("."))
            //                        skill.Reuse = double.Parse(reuse);
            //                    else
            //                        skill.Reuse = int.Parse(reuse);
            //                    break;
            //                case "activate_rate":
            //                    skill.ActivateRate = Convert.ToInt16(parameter.Remove(parameter.Length - 1).Substring(14));
            //                    break;
            //                case "target":
            //                    skill.setTarget(parameter.Substring(7));
            //                    break;
            //                case "cond":
            //                    break;
            //                case "bonus":
            //                    break;
            //                case "next":
            //                    break;

            //                case "abnormal_time":
            //                    skill.AbnormalTime = Convert.ToInt32(parameter.Remove(parameter.Length - 1).Substring(14));
            //                    break;
            //                case "abnormal_id":
            //                    break;
            //            }
            //        }

            //        _skills.Add(skill.HashID(), skill);

            //        if (!_ids.ContainsKey(skill.ClientID))
            //            _ids.Add(skill.ClientID, null);
            //    }
            //}

            log.Info($"SkillTable: loaded {_ids.Count} skills, {enchantInfo.Count} enchants.");
        }

        #region INITREG

        private const byte type_byte = 0;
        private const byte type_int = 1;
        private const byte type_double = 2;
        private const byte type_str = 3;

        public void Initreg()
        {
            if (ps.Count != 0)
                return;

            reg(new SkillLevelParam("skill_id", type_int, 1));
            reg(new SkillLevelParam("level", type_int, 2));
            reg(new SkillLevelParam("operate_type", type_str, 3));
            reg(new SkillLevelParam("magic_level", type_int, 4));
            reg(new SkillLevelParam("self_effect", type_str, 5));
            reg(new SkillLevelParam("effect", type_str, 6));
            reg(new SkillLevelParam("end_effect", type_str, 7));
            reg(new SkillLevelParam("operate_cond", type_str, 8));
            reg(new SkillLevelParam("pvp_effect", type_str, 9));
            reg(new SkillLevelParam("pve_effect", type_str, 10));
            reg(new SkillLevelParam("fail_effect", type_str, 11));
            reg(new SkillLevelParam("start_effect", type_str, 12));
            reg(new SkillLevelParam("tick_effect", type_str, 13));
            reg(new SkillLevelParam("item_consume", type_str, 14));
            reg(new SkillLevelParam("is_magic", type_int, 15));
            reg(new SkillLevelParam("mp_consume1", type_int, 16));
            reg(new SkillLevelParam("mp_consume2", type_int, 17));
            reg(new SkillLevelParam("cast_range", type_int, 18));
            reg(new SkillLevelParam("effective_range", type_int, 19));
            reg(new SkillLevelParam("skill_hit_time", type_double, 20));
            reg(new SkillLevelParam("skill_cool_time", type_double, 21));
            reg(new SkillLevelParam("skill_hit_cancel_time", type_double, 22));
            reg(new SkillLevelParam("reuse_delay", type_double, 23));
            reg(new SkillLevelParam("reuse_delay_lock", type_int, 24));
            reg(new SkillLevelParam("reuse_delay_type", type_str, 25));
            reg(new SkillLevelParam("activate_rate", type_double, 26));
            reg(new SkillLevelParam("lv_bonus_rate", type_int, 27));
            reg(new SkillLevelParam("basic_property", type_str, 28));
            reg(new SkillLevelParam("abnormal_time", type_int, 29));
            reg(new SkillLevelParam("abnormal_lv", type_int, 30));
            reg(new SkillLevelParam("abnormal_type", type_str, 31));
            reg(new SkillLevelParam("abnormal_instant", type_int, 32));
            reg(new SkillLevelParam("irreplaceable_buff", type_int, 33));
            reg(new SkillLevelParam("buff_protect_level", type_int, 34));
            reg(new SkillLevelParam("debuff", type_int, 35));
            reg(new SkillLevelParam("attribute", type_str, 36));
            reg(new SkillLevelParam("trait", type_str, 37));
            reg(new SkillLevelParam("effect_point", type_int, 38));
            reg(new SkillLevelParam("target_type", type_str, 39));
            reg(new SkillLevelParam("affect_scope", type_str, 40));
            reg(new SkillLevelParam("affect_range", type_int, 41));
            reg(new SkillLevelParam("affect_object", type_str, 42));
            reg(new SkillLevelParam("affect_limit", type_str, 43));
            reg(new SkillLevelParam("next_action", type_str, 44));
            reg(new SkillLevelParam("ride_state", type_str, 45));
            reg(new SkillLevelParam("multi_class", type_int, 46));
            reg(new SkillLevelParam("olympiad_use", type_int, 47));
            reg(new SkillLevelParam("abnormal_visual_effect", type_str, 48));
            reg(new SkillLevelParam("hp_consume", type_int, 49));
            reg(new SkillLevelParam("consume_etc", type_str, 50));
            reg(new SkillLevelParam("fan_range", type_str, 51));
            reg(new SkillLevelParam("target_operate_cond", type_str, 52));
            reg(new SkillLevelParam("tick_interval", type_double, 53));
            reg(new SkillLevelParam("attached_skill", type_str, 54));
            reg(new SkillLevelParam("mp_consume_tick", type_int, 55));
            reg(new SkillLevelParam("passive_conditions", type_str, 56));
            reg(new SkillLevelParam("transform_type", type_str, 57));
            reg(new SkillLevelParam("abnormal_delete_leaveworld", type_int, 58));
            reg(new SkillLevelParam("affect_scope_height", type_str, 59));
            reg(new SkillLevelParam("npc_notice", type_int, 60));
            reg(new SkillLevelParam("block_action_use_skill", type_int, 61));
        }

        private readonly SortedList<byte, SkillLevelParam> ps = new SortedList<byte, SkillLevelParam>();

        private void reg(SkillLevelParam s)
        {
            ps.Add(s.id, s);
        }

        #endregion

        public Dictionary<string, TAcquireSkillsEntry> AcquireSkills = new Dictionary<string, TAcquireSkillsEntry>();

        private void loadDLC()
        {
            FileStream fstream = new FileStream(@"dlc\skilltree.dlc", FileMode.Open, FileAccess.Read);
            byte[] dlcheader = new byte[3];
            fstream.Read(dlcheader, 0, dlcheader.Length);
            if (Encoding.UTF8.GetString(dlcheader) != "DLC")
                return;

            AcquireSkills = new Dictionary<string, TAcquireSkillsEntry>();
            DlcStream dlc = new DlcStream(fstream, CompressionMode.Decompress);
            int cnt = dlc.readD(),
                cntTotal = 0;
            for (int a = 0; a < cnt; a++)
            {
                TAcquireSkillsEntry list = new TAcquireSkillsEntry();
                byte len = dlc.readC();
                list.type = dlc.readS(len);
                len = dlc.readC();

                if (len > 0)
                {
                    list.include = dlc.readS(len);
                    List<TAcquireSkill> s = AcquireSkills[list.include].skills;
                    cntTotal += s.Count;
                    list.skills.AddRange(s);
                }

                int skLen = dlc.readD();
                cntTotal += skLen;
                for (int s = 0; s < skLen; s++)
                {
                    TAcquireSkill skill = new TAcquireSkill();
                    skill.id = dlc.readD();
                    skill.lv = dlc.readD();
                    skill.get_lv = dlc.readD();
                    skill.lv_up_sp = dlc.readD();
                    skill.auto_get = dlc.readC() == 1;

                    if (dlc.readC() == 1)
                    {
                        skill.itemid = dlc.readD();
                        skill.itemcount = dlc.readQ();
                    }

                    skill.social_class = dlc.readD();
                    skill.residence_skill = dlc.readC() == 1;
                    len = dlc.readC();
                    if (len > 0)
                        skill.pledge_type = dlc.readS(len);

                    len = dlc.readC();
                    for (byte b = 0; b < len; b++)
                        skill.races.Add(dlc.readC());

                    skill.id_prerequisite_skill = dlc.readD();
                    skill.lv_prerequisite_skill = dlc.readD();

                    int qcn = dlc.readD();
                    for (int i = 0; i < qcn; i++)
                        skill.quests.Add(dlc.readD());

                    list.skills.Add(skill);
                }

                AcquireSkills.Add(list.type, list);
            }

            dlc.close();
            log.Info($"SkillTable: learnable {AcquireSkills.Count} groups, #{cntTotal} skills.");
        }

        public TAcquireSkillsEntry GetAllRegularSkills(ClassIds id)
        {
            return AcquireSkills[id.ToString()];
        }

        public TAcquireSkillsEntry GetSharingSkills(ClassIds id)
        {
            switch (id)
            {
                case ClassIds.SHILLIEN_ELDER:
                    return AcquireSkills["silen_elder_sharing"];
                case ClassIds.ELVEN_ELDER:
                    return AcquireSkills["elder_sharing"];
                case ClassIds.BISHOP:
                    return AcquireSkills["bishop_sharing"];

                default:
                    return null;
            }
        }

        public TAcquireSkillsEntry GetCollectingSkills()
        {
            return AcquireSkills["collect"];
        }

        public TAcquireSkillsEntry GetSubjobSkills()
        {
            return AcquireSkills["subjob"];
        }

        public TAcquireSkillsEntry GetTransformSkills()
        {
            return AcquireSkills["transform"];
        }

        public TAcquireSkillsEntry GetSubpledgeSkills()
        {
            return AcquireSkills["sub_pledge"];
        }

        public TAcquireSkillsEntry GetPledgeSkills()
        {
            return AcquireSkills["pledge"];
        }

        public TAcquireSkillsEntry GetFishingSkills()
        {
            return AcquireSkills["fishing"];
        }
    }
}