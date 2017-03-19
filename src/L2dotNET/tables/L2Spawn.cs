using System;
using log4net;
using L2dotNET.model.npcs;
using L2dotNET.templates;

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

        public L2Npc Spawn(bool notifyOthers = true)
        {
            L2Npc npc = new L2Npc(IdFactory.Instance.NextId(), Template)
            {
                X = Location.X,
                Y = Location.Y,
                Z = Location.Z,
                Heading = Location.Heading
            };
            npc.SpawnMe(notifyOthers);
            return npc;
        }
    }
}