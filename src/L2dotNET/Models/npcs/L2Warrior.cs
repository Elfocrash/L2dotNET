using System;
using System.Linq;
using System.Threading.Tasks;
using L2dotNET.Models.Player;
using L2dotNET.Network.serverpackets;
using L2dotNET.Tables;
using L2dotNET.Templates;
using L2dotNET.Utility;

namespace L2dotNET.Models.Npcs
{
    public class L2Warrior : L2Npc
    {
        public bool SpoilActive = false;
        public DateTime Dtstart;
        public L2Spawn TerritorySpawn;
        public System.Timers.Timer socialTask;

        public L2Warrior(SpawnTable spawnTable, int objectId, NpcTemplate template, L2Spawn spawn) : base(spawnTable, objectId, template, spawn)
        {

        }

        public override string AsString()
        {
            return base.AsString().Replace("L2Npc", "L2Warrior");
        }

        public override async Task OnActionAsync(L2Player player)
        {
            await player.SendMessageAsync(AsString());
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
                if (player.Target.ObjectId != ObjectId)
                {
                    player.Target = this;
                    newtarget = true;
                }
            }

            if (newtarget)
            {
                await player.SendPacketAsync(new MyTargetSelected(ObjectId, player.Level - Template.Level));

                StatusUpdate su = new StatusUpdate(this);
                su.Add(StatusUpdate.CurHp, (int)CharStatus.CurrentHp);
                su.Add(StatusUpdate.MaxHp, (int)MaxHp);
                await player.SendPacketAsync(su);
            }
            
        }

        public override async Task OnSpawnAsync(bool notifyOthers = true)
        {
            await base.OnSpawnAsync(notifyOthers);

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
            if (!CharMovement.CanMove() || IsAttacking())
                return;

            CharMovement.MoveTo(RandomThreadSafe.Instance.Next(SpawnX - 90, SpawnX + 90), RandomThreadSafe.Instance.Next(SpawnY - 90, SpawnY + 90), Z);

            // broadcastPacket(new SocialAction(ObjID, rnd.Next(8)));
        }

        public override void StartAi()
        {
        }

        public override async Task OnForcedAttackAsync(L2Player player)
        {
            await Task.Run(() =>
            {
                player.AttackingId = ObjectId;
            });
        }

        public override async Task BroadcastUserInfoAsync()
        {
            foreach (L2Player obj in KnownObjects.Values.OfType<L2Player>())
                await obj.SendPacketAsync(new NpcInfo(this));
        }

        public override async Task DoDieAsync(L2Character killer)
        {
            await base.DoDieAsync(killer);

            if (killer is L2Player)
                ((L2Player)killer).RedistExp(this);

            //Template.roll_drops(this, killer);

            //if (TerritorySpawn != null)
             //   TerritorySpawn.OnDie(this, killer);

            //socialTask.Enabled = false;
        }

        public override async Task OnActionShiftAsync(L2Player player)
        {
            string text = string.Empty;
            //text += $"pdef: {CharacterStat.GetStat(EffectType.PPhysicalDefense)}<br>";
            //text += $"patk: {CharacterStat.GetStat(EffectType.PPhysicalAttack)}<br>";
            //text += $"curhp: {CurHp}<br>";
            //text += $"maxhp: {CharacterStat.GetStat(EffectType.BMaxHp)}<br>";
            //text += $"mdef: {CharacterStat.GetStat(EffectType.PMagicalAttack)}<br>";
            //text += $"matk: {CharacterStat.GetStat(EffectType.PMagicalDefense)}<br>";

            player.ShowHtmPlain(text, null);
            await player.SendActionFailedAsync();
        }

        public override int Attackable => 1;
    }
}