using System;
using L2dotNET.Logging.Abstraction;
using L2dotNET.Models.Npcs;
using L2dotNET.Templates;

namespace L2dotNET.Tables
{
    public class L2Spawn
    {
        private readonly ILog _log = LogProvider.GetCurrentClassLogger();

        public NpcTemplate Template { get; set; }
        public SpawnLocation Location { get; set; }

        public L2Npc Npc { get; set; }

        private readonly IdFactory _idFactory;
        private readonly SpawnTable _spawnTable;

        public L2Spawn(NpcTemplate template, IdFactory idFactory, SpawnTable spawnTable)
        {
            Template = template ?? throw new ArgumentNullException(nameof(template));
            _idFactory = idFactory;
            _spawnTable = spawnTable;
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
                //TODO this is shit. Change it
                npc = (L2Npc) Activator.CreateInstance(Type.GetType("L2dotNET.Models.Npcs." + Template.Type),
                        _spawnTable, _idFactory.NextId(), Template, this);
                npc.X = Location.X;
                npc.Y = Location.Y;
                npc.Z = Location.Z;
                npc.Heading = Location.Heading;
            }
            else
            {
                npc = new L2Npc(_spawnTable, _idFactory.NextId(), Template, this)
                {
                    X = Location.X,
                    Y = Location.Y,
                    Z = Location.Z,
                    Heading = Location.Heading
                };
            }
            
            npc.SpawnMeAsync(notifyOthers);
            return npc;
        }
    }
}