using System.Threading.Tasks;
using L2dotNET.Templates;
using L2dotNET.Network.serverpackets;

using L2dotNET.World;
using System.Timers;
using L2dotNET.Models.Player;
using L2dotNET.Tables;
using NLog;

namespace L2dotNET.Models.Npcs
{
    class L2Monster : L2Npc
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private Timer CorpseTimer;
        private SpawnTable _spawnTable;
        public override int Attackable => 1;

        public L2Monster(SpawnTable spawnTable, int objectId, NpcTemplate template, L2Spawn spawn) : base(spawnTable, objectId, template, spawn)
        {
            _spawnTable = spawnTable;
            Template = template;
            Name = template.Name;
            InitializeCharacterStatus();
            CharStatus.SetCurrentHp(MaxHp);
            CharStatus.SetCurrentMp(MaxMp);
            //Stats = new CharacterStat(this);
        }

        public override async Task OnActionAsync(L2Player player)
        {
            if (player.Target != this)
            {
                player.SetTargetAsync(this);
                return;
            }

            await player.TryMoveToAsync(X, Y, Z);
            await player.SendPacketAsync(new MoveToPawn(player, this, 150));

            await player.DoAttackAsync(this);
        }

        public override async Task OnForcedAttackAsync(L2Player player)
        {
            if (player.Target != this)
            {
                player.SetTargetAsync(this);
                return;
            }

            await player.TryMoveToAsync(X, Y, Z);
            
            await player.SendPacketAsync(new MoveToPawn(player, this, (int)player.GetPlanDistanceSq(X,Y)));

            await player.DoAttackAsync(this);
        }

        public override async Task DoDieAsync(L2Character killer)
        {         
            lock (this)
            {
                if (Dead)
                    return;

                CharStatus.SetCurrentHp(0);

                Dead = true;
            }
            //Check For Exp
            if (killer is L2Player)
            {
                ((L2Player)killer).AddExpSp(this.Template.Exp, this.Template.Sp, true);
            }

            Target = null;
            await NotifyStopMoveAsync(true, true);

            if (IsAttacking())
                AbortAttack();

            CharStatus.StopHpMpRegeneration();

            await BroadcastPacketAsync(new Die(this));
            _spawnTable.RegisterRespawn(spawn);
            if (Template.CorpseTime <= 0)
            {
                return;
            }
            CorpseTimer = new Timer(Template.CorpseTime * 1000);
            CorpseTimer.Elapsed += new ElapsedEventHandler(RemoveCorpse);
            CorpseTimer.Start();
        }

        private async void RemoveCorpse(object sender, ElapsedEventArgs e)
        {
            CorpseTimer.Stop();
            CorpseTimer.Enabled = false;
            await BroadcastPacketAsync(new DeleteObject(ObjectId));
            L2World.RemoveObject(this);
        }
    }
}
