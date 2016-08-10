using System;
using log4net;
using L2dotNET.model.npcs.ai;
using L2dotNET.model.zones;
using L2dotNET.model.zones.classes;
using L2dotNET.model.zones.forms;
using L2dotNET.world;

namespace L2dotNET.model.structures
{
    public class Hideout : HideoutTemplate
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Hideout));

        public int RentCost = 5000000;
        public bool NoTeleports = false;
        public DateTime PayTime = new DateTime(2011, 2, 1, 12, 00, 00);
        public int ownerId;

        public override void init()
        {
            GenZone();
            SpawnNpcs();

            log.Info($"Hideout #{ID} ({Name}) loaded. {npcs.Count} npcs.");
        }

        public void Banish()
        {
            //foreach (L2Player player in zone.ObjectsInside.Values.OfType<L2Player>().Where(player => player.ClanId != ownerId)) { }
        }

        private hideout_zone zone;

        public void GenZone()
        {
            ZoneTemplate template = new ZoneTemplate
            {
                Name = $"hideout #{ID}",
                Type = ZoneTemplate.ZoneType.Hideout
            };
            template.SetRange(zoneLoc);

            zone = new hideout_zone
            {
                hideout = this,
                Name = template.Name,
                Template = template,
                Territory = new ZoneNPoly(template.X, template.Y, template.Z1, template.Z2)
            };

            for (int i = 0; i < template.X.Length; i++)
            {
                L2WorldRegion region = L2World.Instance.GetRegion(template.X[i], template.Y[i]);
                if (region != null)
                {
                    // region._zoneManager.addZone(zone);
                }
                else
                    log.Error($"AreaTable[hideout]: null region at {template.X[i]} {template.Y[i]} for zone {zone.Name}");
            }
        }

        public int[] Decoration = new int[13];

        public byte MofidyFunc(int decoId, int level)
        {
            if (Decoration[decoId] == level)
                return 1;

            //if (Decoration[decoId] > 0 && Decoration[decoId] < level)
            //    return 2;

            Decoration[decoId] = level;

            //SQL_Block sqb = new SQL_Block("st_hideouts");
            //sqb.param($"func_{decoId}", level);
            //sqb.where("id", ID);
            //sqb.sql_update(false);

            return 5;
        }

        public int GetFuncDepth(int decoId)
        {
            int level = Decoration[decoId];
            int val;
            switch (decoId)
            {
                case AgitManagerAi.DecotypeHpregen:
                    val = level * 20;
                    break;
                case AgitManagerAi.DecotypeMpregen:
                case AgitManagerAi.DecotypeXprestore:
                    val = level * 5;
                    break;
                default:
                    val = level;
                    break;
            }

            return val;
        }

        public int GetFuncLevel(int decoId)
        {
            return Decoration[decoId];
        }

        public int GetDecoCost(int id, int lvl)
        {
            return 0;
        }

        public int GetCurrentDecoCost(int id)
        {
            return 0;
        }

        public object GetDecoEffect(int id, int lvl)
        {
            return 0;
        }
    }
}