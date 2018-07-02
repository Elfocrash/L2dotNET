using System.Threading.Tasks;
using L2dotNET.Templates;
using L2dotNET.Network.serverpackets;

using L2dotNET.World;
using System.Timers;
using L2dotNET.Models.Player;
using L2dotNET.Network;
using L2dotNET.Tables;
using NLog;

namespace L2dotNET.Models.Npcs
{
    class L2Merchant : L2Npc
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private Timer CorpseTimer;
        private readonly SpawnTable _spawnTable;

        public L2Merchant(SpawnTable spawnTable, int objectId, NpcTemplate template, L2Spawn spawn) : base(spawnTable, objectId, template, spawn)
        {
            _spawnTable = spawnTable;
            Template = template;
            Name = template.Name;
            InitializeCharacterStatus();
            CharStatus.SetCurrentHp(MaxHp);
            CharStatus.SetCurrentMp(MaxMp);
            //Stats = new CharacterStat(this);
        }

        public override async Task SendPacketAsync(GameserverPacket pk)
        {
            foreach (L2Player pl in L2World.GetPlayers())
            {
                // TODO: Sends to all players on the server. It is not right
                await pl.Gameclient.SendPacketAsync(pk);
            }
        }

        public override async Task OnActionAsync(L2Player player)
        {
            if (player.Target != this)
            {
                player.SetTargetAsync(this);
                return;
            }
            await player.MoveToAsync(X, Y, Z);
            await player.SendPacketAsync(new MoveToPawn(player, this, 150));
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
            CorpseTimer.Elapsed += RemoveCorpse;
            CorpseTimer.Start();
        }

        private async void RemoveCorpse(object sender, ElapsedEventArgs e)
        {
            CorpseTimer.Stop();
            CorpseTimer.Enabled = false;
            await BroadcastPacketAsync(new DeleteObject(ObjId));
            L2World.RemoveObject(this);
        }
    }
}
