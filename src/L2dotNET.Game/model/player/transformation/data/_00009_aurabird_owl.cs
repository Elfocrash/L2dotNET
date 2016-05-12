using System.Collections.Generic;

namespace L2dotNET.GameService.model.player.transformation.data
{
    class _00009_aurabird_owl : TransformTemplate
    {
        public _00009_aurabird_owl()
        {
            id = 9;
            collision_box = new double[] { 40, 18.57};
            MoveMode = 2;
            npcId = 13145;
        }

        public override void onTransformStart(L2Player player)
        {
            //_skills = new List<int[]>();

            //if (player.Level >= 75)
            //    _skills.Add(new int[] { 885, 1 });

            //if (player.Level >= 83)
            //    _skills.Add(new int[] { 895, 1 });

            //int diff = player.Level - 74;
            //if (diff > 0)
            //{

            //}

            base.onTransformStart(player);
        }
    }
}
