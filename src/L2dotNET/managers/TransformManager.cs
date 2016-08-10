using System.Collections.Generic;
using L2dotNET.model.player;
using L2dotNET.model.player.transformation;

namespace L2dotNET.managers
{
    class TransformManager
    {
        private static readonly TransformManager M = new TransformManager();

        public static TransformManager GetInstance()
        {
            return M;
        }

        private readonly SortedList<int, TransformTemplate> _templates = new SortedList<int, TransformTemplate>();

        private void Register(TransformTemplate t)
        {
            _templates.Add(t.Id, t);
        }

        public void TransformTo(int id, L2Player player, int seconds)
        {
            if (!_templates.ContainsKey(id))
            {
                player.SendMessage($"Transform type #{id} is not registered");
                return;
            }

            L2Transform tr = new L2Transform(_templates[id]);
            if (tr.Template.StartFailed(player))
                return;

            if (seconds != -1)
            {
                tr.Timer(seconds);
                player.SendMessage($"transformed for {seconds} sec");
            }

            player.SetTransform(tr);
        }
    }
}