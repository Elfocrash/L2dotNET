using System;
using log4net;
using L2dotNET.model.npcs;
using L2dotNET.templates;
using L2dotNET.world;

namespace L2dotNET.tables
{
    public class L2Spawn
    {
        private readonly ILog _log = LogManager.GetLogger(typeof(L2Spawn));

        public NpcTemplate Template { get; set; }
        public SpawnLocation Location { get; set; }

        public L2Npc Npc { get; set; }

        public L2Spawn(NpcTemplate template)
        {
            if (template == null)
                throw new ArgumentNullException(nameof(template));
            Template = template;
        }

        public int GetNpcId()
        {
            return Template.NpcId;
        }

        public L2Npc Spawn()
        {
            L2Npc npc = new L2Npc(IdFactory.Instance.NextId(), Template);
            npc.X = Location.X;
            npc.Y = Location.Y;
            npc.Z = Location.Z;
            npc.Heading = Location.Heading;
            npc.SpawnMe();
            npc.OnSpawn();
            return npc;
        }
    }
}