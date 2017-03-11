using System;
using System.Linq;
using L2dotNET.model.npcs.ai;
using L2dotNET.model.playable;
using L2dotNET.model.player;
using L2dotNET.model.skills2;
using L2dotNET.Network.serverpackets;
using L2dotNET.tables;
using L2dotNET.templates;
using L2dotNET.world;

namespace L2dotNET.model.npcs
{
    public class L2Warrior : L2Npc
    {
        public bool SpoilActive = false;
        public DateTime Dtstart;
        public L2Spawn TerritorySpawn;
        public System.Timers.Timer socialTask;

        public L2Warrior(int objectId, NpcTemplate template) : base(objectId, template)
        {
        }

        public override string AsString()
        {
            return base.AsString().Replace("L2Npc", "L2Warrior");
        }

        public override void OnAction(L2Player player)
        {
            player.SendMessage(AsString());
            //    TimeSpan ts = dtstart - DateTime.Now;
            //    player.sendMessage($"timems {(ts.TotalMilliseconds)}");
            bool newtarget = false;
            if (player.CurrentTarget == null)
            {
                player.CurrentTarget = this;
                newtarget = true;
            }
            else
            {
                if (player.CurrentTarget.ObjId != ObjId)
                {
                    player.CurrentTarget = this;
                    newtarget = true;
                }
            }

            if (newtarget)
            {
                player.SendPacket(new MyTargetSelected(ObjId, player.Level - Template.Level));

                StatusUpdate su = new StatusUpdate(ObjId);
                su.Add(StatusUpdate.CurHp, (int)CurHp);
                su.Add(StatusUpdate.MaxHp, (int)CharacterStat.GetStat(EffectType.BMaxHp));
                player.SendPacket(su);
            }
            else
                player.AiCharacter.Attack(this);
        }

        private readonly Random _rnd = new Random();

        public override void OnSpawn()
        {
            base.OnSpawn();
            if (Template.AggroRange > 0)
                AiCharacter.Enable();

            SpawnX = X;
            SpawnY = Y;
            SpawnZ = Z;

            //socialTask = new System.Timers.Timer();
            //socialTask.Interval = rnd.Next(10, 30) * 1000;
            //socialTask.Elapsed += new System.Timers.ElapsedEventHandler(SocialTask);
            //socialTask.Enabled = true;
        }

        private void SocialTask(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (CantMove() || IsAttacking())
                return;

            MoveTo(_rnd.Next(SpawnX - 90, SpawnX + 90), _rnd.Next(SpawnY - 90, SpawnY + 90), Z);

            // broadcastPacket(new SocialAction(ObjID, rnd.Next(8)));
        }

        public override void StartAi()
        {
            AiCharacter = new WarriorAi(this);
        }

        public override void OnForcedAttack(L2Player player)
        {
            player.AttackingId = ObjId;
            player.AiCharacter.Attack(this);
        }

        public override void BroadcastUserInfo()
        {
            foreach (L2Player obj in KnownObjects.Values.OfType<L2Player>())
                obj.SendPacket(new NpcInfo(this));
        }

        public override void DoDie(L2Character killer, bool bytrigger)
        {
            base.DoDie(killer, bytrigger);

            if (killer is L2Player)
                ((L2Player)killer).RedistExp(this);
            else
            {
                if (killer is L2Pet)
                    ((L2Pet)killer).Owner.RedistExp(this);
                else
                {
                    if (killer is L2Summon)
                        ((L2Summon)killer).Owner.RedistExp(this);
                }
            }

            //Template.roll_drops(this, killer);

            //if (TerritorySpawn != null)
             //   TerritorySpawn.OnDie(this, killer);

            //socialTask.Enabled = false;
        }

        public override void OnActionShift(L2Player player)
        {
            string text = string.Empty;
            text += $"pdef: {CharacterStat.GetStat(EffectType.PPhysicalDefense)}<br>";
            text += $"patk: {CharacterStat.GetStat(EffectType.PPhysicalAttack)}<br>";
            text += $"curhp: {CurHp}<br>";
            text += $"maxhp: {CharacterStat.GetStat(EffectType.BMaxHp)}<br>";
            text += $"mdef: {CharacterStat.GetStat(EffectType.PMagicalAttack)}<br>";
            text += $"matk: {CharacterStat.GetStat(EffectType.PMagicalDefense)}<br>";

            player.ShowHtmPlain(text, null);
            player.SendActionFailed();
        }

        public override int Attackable => 1;
    }
}