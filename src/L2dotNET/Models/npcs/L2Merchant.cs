using L2dotNET.Templates;
using L2dotNET.Network.serverpackets;
using log4net;
using L2dotNET.World;
using System.Timers;
using L2dotNET.Models.Player;
using L2dotNET.Network;
using L2dotNET.Tables;

namespace L2dotNET.Models.Npcs
{
    class L2Merchant : L2Npc
    {
        private readonly ILog Log = LogManager.GetLogger(typeof(L2Merchant));

        private Timer CorpseTimer;

        public L2Merchant(int objectId, NpcTemplate template, L2Spawn spawn) : base(objectId, template, spawn)
        {
            Template = template;
            Name = template.Name;
            InitializeCharacterStatus();
            CharStatus.SetCurrentHp(MaxHp);
            CharStatus.SetCurrentMp(MaxMp);
            //Stats = new CharacterStat(this);
        }

        public override void SendPacket(GameserverPacket pk)
        {
            foreach (L2Player pl in L2World.Instance.GetPlayers())
            {
                // TODO: Sends to all players on the server. It is not right
                pl.Gameclient.SendPacket(pk);
            }
        }

        public override void OnAction(L2Player player)
        {
            if (player.Target != this)
            {
                player.SetTarget(this);
                return;
            }
            player.MoveTo(X, Y, Z);
            player.SendPacket(new MoveToPawn(player, this, 150));

        }

        public override void DoDie(L2Character killer)
        {
            lock (this)
            {
                if (Dead)
                    return;

                CharStatus.SetCurrentHp(0);

                Dead = true;
            }

            Target = null;
            NotifyStopMove(true, true);

            if (IsAttacking())
                AbortAttack();

            CharStatus.StopHpMpRegeneration();

            BroadcastPacket(new Die(this));

            SpawnTable.Instance.RegisterRespawn(spawn);

            if (Template.CorpseTime <= 0)
            {
                return;
            }

            CorpseTimer = new Timer(Template.CorpseTime * 1000);
            CorpseTimer.Elapsed += new ElapsedEventHandler(RemoveCorpse);
            CorpseTimer.Start();
        }

        private void RemoveCorpse(object sender, ElapsedEventArgs e)
        {
            CorpseTimer.Stop();
            CorpseTimer.Enabled = false;
            BroadcastPacket(new DeleteObject(ObjId));
            L2World.Instance.RemoveObject(this);
        }
    }
}
