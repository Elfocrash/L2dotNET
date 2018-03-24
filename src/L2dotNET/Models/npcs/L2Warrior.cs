using System;
using System.Linq;
using L2dotNET.Models.player;
using L2dotNET.Network.serverpackets;
using L2dotNET.tables;
using L2dotNET.templates;

namespace L2dotNET.Models.npcs
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
            if (player.Target == null)
            {
                player.Target = this;
                newtarget = true;
            }
            else
            {
                if (player.Target.ObjId != ObjId)
                {
                    player.Target = this;
                    newtarget = true;
                }
            }

            if (newtarget)
            {
                player.SendPacket(new MyTargetSelected(ObjId, player.Level - Template.Level));

                StatusUpdate su = new StatusUpdate(this);
                su.Add(StatusUpdate.CurHp, (int)CurHp);
                su.Add(StatusUpdate.MaxHp, (int)MaxHp);
                player.SendPacket(su);
            }
            
        }

        private readonly Random _rnd = new Random();

        public override void OnSpawn(bool notifyOthers = true)
        {
            base.OnSpawn(notifyOthers);

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
        }

        public override void OnForcedAttack(L2Player player)
        {
            player.AttackingId = ObjId;
        }

        public override void BroadcastUserInfo()
        {
            foreach (L2Player obj in KnownObjects.Values.OfType<L2Player>())
                obj.SendPacket(new NpcInfo(this));
        }

        public override void DoDie(L2Character killer)
        {
            base.DoDie(killer);

            if (killer is L2Player)
                ((L2Player)killer).RedistExp(this);
            else
            {

            }

            //Template.roll_drops(this, killer);

            //if (TerritorySpawn != null)
             //   TerritorySpawn.OnDie(this, killer);

            //socialTask.Enabled = false;
        }

        public override void OnActionShift(L2Player player)
        {
            string text = string.Empty;
            //text += $"pdef: {CharacterStat.GetStat(EffectType.PPhysicalDefense)}<br>";
            //text += $"patk: {CharacterStat.GetStat(EffectType.PPhysicalAttack)}<br>";
            //text += $"curhp: {CurHp}<br>";
            //text += $"maxhp: {CharacterStat.GetStat(EffectType.BMaxHp)}<br>";
            //text += $"mdef: {CharacterStat.GetStat(EffectType.PMagicalAttack)}<br>";
            //text += $"matk: {CharacterStat.GetStat(EffectType.PMagicalDefense)}<br>";

            player.ShowHtmPlain(text, null);
            player.SendActionFailed();
        }

        public override int Attackable => 1;
    }
}