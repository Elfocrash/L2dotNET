using System;
using log4net;
using L2dotNET.Models.npcs;
using L2dotNET.templates;
using System.Timers;

namespace L2dotNET.tables
{
    public class L2Spawn
    {
        private readonly ILog _log = LogManager.GetLogger(typeof(L2Spawn));

        public NpcTemplate Template { get; set; }
        public SpawnLocation Location { get; set; }

        public L2Npc Npc { get; set; }

        private Timer SpawnTimer;

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
            L2Npc npc;
            if (Type.GetType("L2dotNET.Models.npcs."+Template.Type) != null)
            {
                npc = (L2Npc)Activator.CreateInstance( Type.GetType("L2dotNET.Models.npcs." + Template.Type), IdFactory.Instance.NextId(), Template, this);
                npc.X = Location.X;
                npc.Y = Location.Y;
                npc.Z = Location.Z;
                npc.Heading = Location.Heading;

            }
            else
            {
                npc = new L2Npc(IdFactory.Instance.NextId(), Template, this)
                {
                    X = Location.X,
                    Y = Location.Y,
                    Z = Location.Z,
                    Heading = Location.Heading
                };
            }
            
            npc.SpawnMe(notifyOthers);
            return npc;
        }
    }
}