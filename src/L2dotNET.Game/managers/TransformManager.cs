using System.Collections.Generic;
using L2dotNET.GameService.Model.player;
using L2dotNET.GameService.Model.player.transformation;
using L2dotNET.GameService.Model.player.transformation.data;

namespace L2dotNET.GameService.Managers
{
    class TransformManager
    {
        private static readonly TransformManager m = new TransformManager();

        public static TransformManager getInstance()
        {
            return m;
        }

        private readonly SortedList<int, TransformTemplate> _templates = new SortedList<int, TransformTemplate>();

        public TransformManager()
        {
            register(new _00009_aurabird_owl());
            register(new _00125_aqua_elf());
            register(new _00126_treykan());

            register(new _00260_final_form_air());

            register(new _20000_kadomas());
            register(new _20001_jet_bike());
            register(new _20002_trejuo());
            register(new _20003_sujin());
        }

        private void register(TransformTemplate t)
        {
            _templates.Add(t.id, t);
        }

        public void transformTo(int id, L2Player player, int seconds)
        {
            if (!_templates.ContainsKey(id))
            {
                player.sendMessage("Transform type #" + id + " is not registered");
                return;
            }

            L2Transform tr = new L2Transform(_templates[id]);
            if (tr.Template.startFailed(player))
                return;

            if (seconds != -1)
            {
                tr.timer(seconds);
                player.sendMessage("transformed for " + seconds + " sec");
            }

            player.setTransform(tr);
        }
    }
}