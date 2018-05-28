using L2dotNET.Logging.Abstraction;
using L2dotNET.Templates;
using L2dotNET.Network.serverpackets;

using L2dotNET.World;
using L2dotNET.Models.Player;
using L2dotNET.Network;
using L2dotNET.Tables;

namespace L2dotNET.Models.Npcs
{
    class L2Trainer : L2Npc
    {
        private readonly ILog Log = LogProvider.GetCurrentClassLogger();

        public L2Trainer(SpawnTable spawnTable, int objectId, NpcTemplate template, L2Spawn spawn) : base(spawnTable, objectId, template, spawn)
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

            player.ShowHtm($"trainer/{NpcId}.htm",this);
        }
    }
}
