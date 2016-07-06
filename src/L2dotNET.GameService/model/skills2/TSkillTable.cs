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
    class SkillTable
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(SkillTable));
        private static volatile SkillTable _instance;
        private static readonly object SyncRoot = new object();

        public static SkillTable Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (SyncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new SkillTable();
                        }
                    }
                }

                return _instance;
            }
        }

        public void Initialize()
        {
            Read();
            LoadDlc();
        }

        public Skill Get(int id, int lvl)
        {
            long hash = (id * 65536) + lvl;
            return Skills.ContainsKey(hash) ? Skills[hash] : null;
        }

        public Skill Get(int skillId)
        {
            return Skills.ContainsKey(skillId) ? Skills[skillId] : null;
        }

        public readonly SortedList<long, SkillEnchantInfo> EnchantInfo = new SortedList<long, SkillEnchantInfo>();
        public readonly SortedList<long, Skill> Skills = new SortedList<long, Skill>();

        public void Read()
        {
            using (FileStream fstream = File.Open(@"dlc\skillenchant.dlc", FileMode.Open, FileAccess.Read))
            {
                byte[] dlcheader = new byte[3];
                fstream.Read(dlcheader, 0, dlcheader.Length);
                if (Encoding.UTF8.GetString(dlcheader) == "DLC")
                {
                    DlcStream dlc = new DlcStream(fstream, CompressionMode.Decompress);
                    int cnt = dlc.ReadD();
                    for (int a = 0; a < cnt; a++)
                    {
                        SkillEnchantInfo inf = new SkillEnchantInfo();
                        inf.Id = dlc.ReadD();
                        inf.Lv = dlc.ReadD();
                        int len = dlc.ReadD();
                        for (int b = 0; b < len; b++)
                        {
                            SkillEnchantInfoDetail nfo = new SkillEnchantInfoDetail();
                            nfo.RouteId = dlc.ReadD();
                            nfo.EnchantId = dlc.ReadD();
                            nfo.EnchantedSkillLevel = dlc.ReadD();
                            nfo.Importance = dlc.ReadC();
                            nfo.R1 = dlc.ReadD();
                            nfo.R2 = dlc.ReadD();
                            nfo.R3 = dlc.ReadD();
                            nfo.R4 = dlc.ReadD();
                            dlc.ReadS(10);

                            inf.Details.Add(nfo.EnchantedSkillLevel, nfo);
                        }

                        EnchantInfo.Add((inf.Id * 65536) + inf.Lv, inf);
                    }
                }
            }

            SortedList<int, object> ids = new SortedList<int, object>();
            Initreg();
            using (FileStream fstream = File.Open(@"dlc\skilldb_edit.dlc", FileMode.Open, FileAccess.Read))
            {
                byte[] dlcheader = new byte[3];
                fstream.Read(dlcheader, 0, dlcheader.Length);
                if (Encoding.UTF8.GetString(dlcheader) == "DLC")
                {
                    DlcStream dlc = new DlcStream(fstream, CompressionMode.Decompress);
                    int cnt = dlc.ReadD();
                    for (int a = 0; a < cnt; a++)
                    {
                        byte len = dlc.ReadC();
                        Skill skill = new Skill();
                        for (byte p = 0; p < len; p++)
                        {
                            byte keyId = dlc.ReadC();
                            SkillLevelParam slp = _ps[keyId];
                            int lenx;
                            string value;
                            switch (slp.Id)
                            {
                                case 1:
                                    skill.SkillId = dlc.ReadD();
                                    break;
                                case 2:
                                    skill.Level = dlc.ReadD();
                                    break;
                                case 3:
                                    lenx = dlc.ReadD();
                                    value = dlc.ReadS(lenx);
                                    skill.OpType = (SkillOperational)Enum.Parse(typeof(SkillOperational), value);
                                    break;
                                case 4:
                                    skill.MagicLevel = dlc.ReadD();
                                    break;
                                case 6:
                                    lenx = dlc.ReadD();
                                    value = dlc.ReadS(lenx);
                                    skill.SetEffect_effect(value);
                                    break;
                                case 8:
                                    lenx = dlc.ReadD();
                                    value = dlc.ReadS(lenx);
                                    skill.SetOperateCond(value);
                                    break;
                                case 15:
                                    skill.IsMagic = (short)dlc.ReadD();
                                    break;
                                case 16:
                                    skill.MpConsume1 = dlc.ReadD();
                                    break;
                                case 17:
                                    skill.MpConsume2 = dlc.ReadD();
                                    break;
                                case 18:
                                    skill.CastRange = dlc.ReadD();
                                    break;
                                case 19:
                                    skill.EffectiveRange = dlc.ReadD();
                                    break;
                                case 20:
                                    skill.SkillHitTime = (short)(dlc.ReadF() * 1000);
                                    break;
                                case 21:
                                    skill.SkillCoolTime = (short)(dlc.ReadF() * 1000);
                                    break;
                                case 23:
                                    skill.ReuseDelay = dlc.ReadF();
                                    break;
                                case 26:
                                    double rate = dlc.ReadF();
                                    if (rate != -1)
                                    {
                                        skill.ActivateRate = (short)(rate * 1000);
                                    }
                                    break;
                                case 29:
                                    skill.AbnormalTime = dlc.ReadD();
                                    break;
                                case 30:
                                    skill.AbnormalLv = dlc.ReadD();
                                    break;
                                case 31:
                                    lenx = dlc.ReadD();
                                    skill.AbnormalType = dlc.ReadS(lenx);
                                    break;
                                case 39: //target_type
                                    lenx = dlc.ReadD();
                                    value = dlc.ReadS(lenx);
                                    try
                                    {
                                        skill.TargetType = (SkillTarget)Enum.Parse(typeof(SkillTarget), value);
                                    }
                                    catch (Exception)
                                    {
                                        skill.TargetType = SkillTarget.Target;
                                        Log.Error($"skill # {skill.SkillId} invalid target {value}");
                                    }
                                    break;
                                case 40: //affect_scope
                                    lenx = dlc.ReadD();
                                    value = dlc.ReadS(lenx);
                                    try
                                    {
                                        skill.AffectScope = (SkillScope)Enum.Parse(typeof(SkillScope), value);
                                    }
                                    catch
                                    {
                                        skill.AffectScope = SkillScope.Single;
                                        Log.Error($"skill # {skill.SkillId} invalid scope {value}");
                                    }
                                    break;
                                case 49:
                                    skill.HpConsume = dlc.ReadD();
                                    break;
                                default:
                                    switch (slp.Type)
                                    {
                                        case 1:
                                            dlc.ReadD();
                                            break;
                                        case 2:
                                            dlc.ReadF();
                                            break;
                                        case 3:
                                            int f = dlc.ReadD();
                                            dlc.ReadS(f);
                                            break;
                                    }

                                    break;
                            }
                        }

                        if (EnchantInfo.ContainsKey(skill.HashId()))
                        {
                            skill.EnchantEnabled = 1;
                        }

                        Skills.Add(skill.HashId(), skill);

                        if (!ids.ContainsKey(skill.SkillId))
                        {
                            ids.Add(skill.SkillId, null);
                        }
                    }
                }
            }

            //using (StreamReader sreader = File.OpenText(@"scripts\skills.txt"))
            //{
            //    while (!sreader.EndOfStream)
            //    {
            //        string line = sreader.ReadLine();
            //        if (line.Length == 0 || line.StartsWithIgnoreCase("#"))
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

            Log.Info($"SkillTable: loaded {ids.Count} skills, {EnchantInfo.Count} enchants.");
        }

        #region INITREG

        private const byte TypeByte = 0;
        private const byte TypeInt = 1;
        private const byte TypeDouble = 2;
        private const byte TypeStr = 3;

        public void Initreg()
        {
            if (_ps.Count != 0)
            {
                return;
            }

            Reg(new SkillLevelParam("skill_id", TypeInt, 1));
            Reg(new SkillLevelParam("level", TypeInt, 2));
            Reg(new SkillLevelParam("operate_type", TypeStr, 3));
            Reg(new SkillLevelParam("magic_level", TypeInt, 4));
            Reg(new SkillLevelParam("self_effect", TypeStr, 5));
            Reg(new SkillLevelParam("effect", TypeStr, 6));
            Reg(new SkillLevelParam("end_effect", TypeStr, 7));
            Reg(new SkillLevelParam("operate_cond", TypeStr, 8));
            Reg(new SkillLevelParam("pvp_effect", TypeStr, 9));
            Reg(new SkillLevelParam("pve_effect", TypeStr, 10));
            Reg(new SkillLevelParam("fail_effect", TypeStr, 11));
            Reg(new SkillLevelParam("start_effect", TypeStr, 12));
            Reg(new SkillLevelParam("tick_effect", TypeStr, 13));
            Reg(new SkillLevelParam("item_consume", TypeStr, 14));
            Reg(new SkillLevelParam("is_magic", TypeInt, 15));
            Reg(new SkillLevelParam("mp_consume1", TypeInt, 16));
            Reg(new SkillLevelParam("mp_consume2", TypeInt, 17));
            Reg(new SkillLevelParam("cast_range", TypeInt, 18));
            Reg(new SkillLevelParam("effective_range", TypeInt, 19));
            Reg(new SkillLevelParam("skill_hit_time", TypeDouble, 20));
            Reg(new SkillLevelParam("skill_cool_time", TypeDouble, 21));
            Reg(new SkillLevelParam("skill_hit_cancel_time", TypeDouble, 22));
            Reg(new SkillLevelParam("reuse_delay", TypeDouble, 23));
            Reg(new SkillLevelParam("reuse_delay_lock", TypeInt, 24));
            Reg(new SkillLevelParam("reuse_delay_type", TypeStr, 25));
            Reg(new SkillLevelParam("activate_rate", TypeDouble, 26));
            Reg(new SkillLevelParam("lv_bonus_rate", TypeInt, 27));
            Reg(new SkillLevelParam("basic_property", TypeStr, 28));
            Reg(new SkillLevelParam("abnormal_time", TypeInt, 29));
            Reg(new SkillLevelParam("abnormal_lv", TypeInt, 30));
            Reg(new SkillLevelParam("abnormal_type", TypeStr, 31));
            Reg(new SkillLevelParam("abnormal_instant", TypeInt, 32));
            Reg(new SkillLevelParam("irreplaceable_buff", TypeInt, 33));
            Reg(new SkillLevelParam("buff_protect_level", TypeInt, 34));
            Reg(new SkillLevelParam("debuff", TypeInt, 35));
            Reg(new SkillLevelParam("attribute", TypeStr, 36));
            Reg(new SkillLevelParam("trait", TypeStr, 37));
            Reg(new SkillLevelParam("effect_point", TypeInt, 38));
            Reg(new SkillLevelParam("target_type", TypeStr, 39));
            Reg(new SkillLevelParam("affect_scope", TypeStr, 40));
            Reg(new SkillLevelParam("affect_range", TypeInt, 41));
            Reg(new SkillLevelParam("affect_object", TypeStr, 42));
            Reg(new SkillLevelParam("affect_limit", TypeStr, 43));
            Reg(new SkillLevelParam("next_action", TypeStr, 44));
            Reg(new SkillLevelParam("ride_state", TypeStr, 45));
            Reg(new SkillLevelParam("multi_class", TypeInt, 46));
            Reg(new SkillLevelParam("olympiad_use", TypeInt, 47));
            Reg(new SkillLevelParam("abnormal_visual_effect", TypeStr, 48));
            Reg(new SkillLevelParam("hp_consume", TypeInt, 49));
            Reg(new SkillLevelParam("consume_etc", TypeStr, 50));
            Reg(new SkillLevelParam("fan_range", TypeStr, 51));
            Reg(new SkillLevelParam("target_operate_cond", TypeStr, 52));
            Reg(new SkillLevelParam("tick_interval", TypeDouble, 53));
            Reg(new SkillLevelParam("attached_skill", TypeStr, 54));
            Reg(new SkillLevelParam("mp_consume_tick", TypeInt, 55));
            Reg(new SkillLevelParam("passive_conditions", TypeStr, 56));
            Reg(new SkillLevelParam("transform_type", TypeStr, 57));
            Reg(new SkillLevelParam("abnormal_delete_leaveworld", TypeInt, 58));
            Reg(new SkillLevelParam("affect_scope_height", TypeStr, 59));
            Reg(new SkillLevelParam("npc_notice", TypeInt, 60));
            Reg(new SkillLevelParam("block_action_use_skill", TypeInt, 61));
        }

        private readonly SortedList<byte, SkillLevelParam> _ps = new SortedList<byte, SkillLevelParam>();

        private void Reg(SkillLevelParam s)
        {
            _ps.Add(s.Id, s);
        }

        #endregion

        public Dictionary<string, AcquireSkillsEntry> AcquireSkills = new Dictionary<string, AcquireSkillsEntry>();

        private void LoadDlc()
        {
            FileStream fstream = new FileStream(@"dlc\skilltree.dlc", FileMode.Open, FileAccess.Read);
            byte[] dlcheader = new byte[3];
            fstream.Read(dlcheader, 0, dlcheader.Length);
            if (Encoding.UTF8.GetString(dlcheader) != "DLC")
            {
                return;
            }

            AcquireSkills = new Dictionary<string, AcquireSkillsEntry>();
            DlcStream dlc = new DlcStream(fstream, CompressionMode.Decompress);
            int cnt = dlc.ReadD(),
                cntTotal = 0;
            for (int a = 0; a < cnt; a++)
            {
                AcquireSkillsEntry list = new AcquireSkillsEntry();
                byte len = dlc.ReadC();
                list.Type = dlc.ReadS(len);
                len = dlc.ReadC();

                if (len > 0)
                {
                    list.Include = dlc.ReadS(len);
                    List<AcquireSkill> s = AcquireSkills[list.Include].Skills;
                    cntTotal += s.Count;
                    list.Skills.AddRange(s);
                }

                int skLen = dlc.ReadD();
                cntTotal += skLen;
                for (int s = 0; s < skLen; s++)
                {
                    AcquireSkill skill = new AcquireSkill();
                    skill.Id = dlc.ReadD();
                    skill.Lv = dlc.ReadD();
                    skill.GetLv = dlc.ReadD();
                    skill.LvUpSp = dlc.ReadD();
                    skill.AutoGet = dlc.ReadC() == 1;

                    if (dlc.ReadC() == 1)
                    {
                        skill.ItemId = dlc.ReadD();
                        skill.ItemCount = dlc.ReadD();
                    }

                    skill.SocialClass = dlc.ReadD();
                    skill.ResidenceSkill = dlc.ReadC() == 1;
                    len = dlc.ReadC();
                    if (len > 0)
                    {
                        skill.PledgeType = dlc.ReadS(len);
                    }

                    len = dlc.ReadC();
                    for (byte b = 0; b < len; b++)
                    {
                        skill.Races.Add(dlc.ReadC());
                    }

                    skill.IdPrerequisiteSkill = dlc.ReadD();
                    skill.LvPrerequisiteSkill = dlc.ReadD();

                    int qcn = dlc.ReadD();
                    for (int i = 0; i < qcn; i++)
                    {
                        skill.Quests.Add(dlc.ReadD());
                    }

                    list.Skills.Add(skill);
                }

                AcquireSkills.Add(list.Type, list);
            }

            dlc.Close();
            Log.Info($"SkillTable: learnable {AcquireSkills.Count} groups, #{cntTotal} skills.");
        }

        public AcquireSkillsEntry GetAllRegularSkills(ClassIds id)
        {
            return AcquireSkills[id.ToString()];
        }

        public AcquireSkillsEntry GetSharingSkills(ClassIds id)
        {
            switch (id)
            {
                case ClassIds.ShillienElder:
                    return AcquireSkills["silen_elder_sharing"];
                case ClassIds.ElvenElder:
                    return AcquireSkills["elder_sharing"];
                case ClassIds.Bishop:
                    return AcquireSkills["bishop_sharing"];

                default:
                    return null;
            }
        }

        public AcquireSkillsEntry GetCollectingSkills()
        {
            return AcquireSkills["collect"];
        }

        public AcquireSkillsEntry GetSubjobSkills()
        {
            return AcquireSkills["subjob"];
        }

        public AcquireSkillsEntry GetTransformSkills()
        {
            return AcquireSkills["transform"];
        }

        public AcquireSkillsEntry GetSubpledgeSkills()
        {
            return AcquireSkills["sub_pledge"];
        }

        public AcquireSkillsEntry GetPledgeSkills()
        {
            return AcquireSkills["pledge"];
        }

        public AcquireSkillsEntry GetFishingSkills()
        {
            return AcquireSkills["fishing"];
        }
    }
}