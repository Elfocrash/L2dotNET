using System;
using System.Linq;
using L2dotNET.GameService.Model.Npcs.Ai;
using L2dotNET.GameService.Model.Playable;
using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Model.Skills2;
using L2dotNET.GameService.Network.Serverpackets;
using L2dotNET.GameService.Tables;
using L2dotNET.GameService.World;

namespace L2dotNET.GameService.Model.Npcs
{
    public class L2Warrior : L2Npc
    {
        public bool spoilActive = false;
        public DateTime dtstart;
        public L2Spawn TerritorySpawn;
        public System.Timers.Timer socialTask;

        public override string AsString()
        {
            return base.AsString().Replace("L2Npc", "L2Warrior");
        }

        public override void OnAction(L2Player player)
        {
            player.SendMessage(AsString());
            //    TimeSpan ts = dtstart - DateTime.Now;
            //    player.sendMessage("timems "+(ts.TotalMilliseconds));
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
                su.add(StatusUpdate.CUR_HP, (int)CurHp);
                su.add(StatusUpdate.MAX_HP, (int)CharacterStat.getStat(TEffectType.b_max_hp));
                player.SendPacket(su);
            }
            else
            {
                player.AiCharacter.Attack(this);
            }
        }

        private readonly Random rnd = new Random();

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

            MoveTo(rnd.Next(SpawnX - 90, SpawnX + 90), rnd.Next(SpawnY - 90, SpawnY + 90), Z);

            // broadcastPacket(new SocialAction(ObjID, rnd.Next(8)));
        }

        public override void StartAi()
        {
            AiCharacter = new WarriorAI(this);
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
            else if (killer is L2Pet)
                ((L2Pet)killer).Owner.RedistExp(this);
            else if (killer is L2Summon)
                ((L2Summon)killer).Owner.RedistExp(this);

            //Template.roll_drops(this, killer);

            if (TerritorySpawn != null)
                TerritorySpawn.onDie(this, killer);

            //socialTask.Enabled = false;
        }

        public override void OnActionShift(L2Player player)
        {
            string text = "";

            text += "pdef: " + CharacterStat.getStat(TEffectType.p_physical_defense) + "<br>";
            text += "patk: " + CharacterStat.getStat(TEffectType.p_physical_attack) + "<br>";
            text += "curhp: " + CurHp + "<br>";
            text += "maxhp: " + CharacterStat.getStat(TEffectType.b_max_hp) + "<br>";
            text += "mdef: " + CharacterStat.getStat(TEffectType.p_magical_attack) + "<br>";
            text += "matk: " + CharacterStat.getStat(TEffectType.p_magical_defense) + "<br>";

            player.ShowHtmPlain(text, null);
            player.SendActionFailed();
        }

        public override int Attackable
        {
            get { return 1; }
        }
    }
}