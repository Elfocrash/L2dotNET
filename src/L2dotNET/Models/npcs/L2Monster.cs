using L2dotNET.templates;
using L2dotNET.Network.serverpackets;
using log4net;
using L2dotNET.world;
using System.Timers;
using L2dotNET.Models.player;
using L2dotNET.Network;
using L2dotNET.tables;

namespace L2dotNET.Models.npcs
{
    class L2Monster : L2Npc
    {
        private readonly ILog Log = LogManager.GetLogger(typeof(L2Monster));

        private Timer CorpseTimer;

        public L2Monster(int objectId, NpcTemplate template, L2Spawn spawn) : base(objectId, template, spawn)
        {
            Template = template;
            Name = template.Name;
            InitializeCharacterStatus();

            CharStatus.SetCurrentHp(Template.BaseHpMax(0));
            CharStatus.SetCurrentMp(Template.BaseMpMax(0));
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
                player.SendPacket(new MyTargetSelected(ObjId, 0));
                return;
            }
            player.MoveTo(X, Y, Z);
            player.SendPacket(new MoveToPawn(player, this, 150));

            player.DoAttack(this);
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
            //Check For Exp
            if (killer is L2Player)
            {
                ((L2Player)killer).AddExpSp(this.Template.Exp, this.Template.Sp, true);
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
