using L2dotNET.Templates;
using L2dotNET.Network.serverpackets;

using L2dotNET.World;
using System.Timers;
using L2dotNET.Logging.Abstraction;
using L2dotNET.Models.Player;
using L2dotNET.Tables;

namespace L2dotNET.Models.Npcs
{
    class L2Monster : L2Npc
    {
        private readonly ILog Log = LogProvider.GetCurrentClassLogger();

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

        public override void OnAction(L2Player player)
        {
            if (player.Target != this)
            {
                player.SetTarget(this);
                return;
            }

            player.TryMoveTo(X, Y, Z);
            player.SendPacket(new MoveToPawn(player, this, 150));

            player.DoAttack(this);
        }

        public override void OnForcedAttack(L2Player player)
        {
            if (player.Target != this)
            {
                player.SetTarget(this);
                return;
            }

            player.TryMoveTo(X, Y, Z);
            
            player.SendPacket(new MoveToPawn(player, this, (int)player.GetPlanDistanceSq(X,Y)));

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
            _spawnTable.RegisterRespawn(spawn);
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
