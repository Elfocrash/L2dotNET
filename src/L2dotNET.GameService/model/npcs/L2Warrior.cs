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

        public override string asString()
        {
            return base.asString().Replace("L2Npc", "L2Warrior");
        }

        public override void onAction(L2Player player)
        {
            player.sendMessage(asString());
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
                if (player.CurrentTarget.ObjID != ObjID)
                {
                    player.CurrentTarget = this;
                    newtarget = true;
                }
            }

            if (newtarget)
            {
                player.sendPacket(new MyTargetSelected(ObjID, player.Level - Template.Level));

                StatusUpdate su = new StatusUpdate(ObjID);
                su.add(StatusUpdate.CUR_HP, (int)CurHP);
                su.add(StatusUpdate.MAX_HP, (int)CharacterStat.getStat(TEffectType.b_max_hp));
                player.sendPacket(su);
            }
            else
            {
                player.AICharacter.Attack(this);
            }
        }

        private readonly Random rnd = new Random();

        public override void onSpawn()
        {
            base.onSpawn();
            if (Template.AggroRange > 0)
                AICharacter.Enable();

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
            if (cantMove() || isAttacking())
                return;

            MoveTo(rnd.Next(SpawnX - 90, SpawnX + 90), rnd.Next(SpawnY - 90, SpawnY + 90), Z);

            // broadcastPacket(new SocialAction(ObjID, rnd.Next(8)));
        }

        public override void StartAI()
        {
            AICharacter = new WarriorAI(this);
        }

        public override void onForcedAttack(L2Player player)
        {
            player.AttackingId = ObjID;
            player.AICharacter.Attack(this);
        }

        public override void broadcastUserInfo()
        {
            foreach (L2Player obj in knownObjects.Values.OfType<L2Player>())
                obj.sendPacket(new NpcInfo(this));
        }

        public override void doDie(L2Character killer, bool bytrigger)
        {
            base.doDie(killer, bytrigger);

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

        public override void onActionShift(L2Player player)
        {
            string text = "";

            text += "pdef: " + CharacterStat.getStat(TEffectType.p_physical_defense) + "<br>";
            text += "patk: " + CharacterStat.getStat(TEffectType.p_physical_attack) + "<br>";
            text += "curhp: " + CurHP + "<br>";
            text += "maxhp: " + CharacterStat.getStat(TEffectType.b_max_hp) + "<br>";
            text += "mdef: " + CharacterStat.getStat(TEffectType.p_magical_attack) + "<br>";
            text += "matk: " + CharacterStat.getStat(TEffectType.p_magical_defense) + "<br>";

            player.ShowHtmPlain(text, null);
            player.sendActionFailed();
        }

        public override int Attackable
        {
            get { return 1; }
        }
    }
}