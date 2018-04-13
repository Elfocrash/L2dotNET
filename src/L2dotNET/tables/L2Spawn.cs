using System;
using log4net;
using L2dotNET.Models.Npcs;
using L2dotNET.Templates;

namespace L2dotNET.Tables
{
    public class L2Spawn
    {
        private readonly ILog _log = LogManager.GetLogger(typeof(L2Spawn));

        public NpcTemplate Template { get; set; }
        public SpawnLocation Location { get; set; }

        public L2Npc Npc { get; set; }

        public L2Spawn(NpcTemplate template)
        {
            Template = template ?? throw new ArgumentNullException(nameof(template));
        }

        public int GetNpcId()
        {
            return Template.NpcId;
        }

        public L2Npc Spawn(bool notifyOthers = true)
        {
            L2Npc npc;
            if (Type.GetType("L2dotNET.Models.Npcs." + Template.Type) != null)
            {
                npc = (L2Npc)Activator.CreateInstance( Type.GetType("L2dotNET.Models.Npcs." + Template.Type), IdFactory.Instance.NextId(), Template, this);
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