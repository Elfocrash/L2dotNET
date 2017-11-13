using L2dotNET.model.player;
using L2dotNET.world;

namespace L2dotNET.Models.Status
{
    public class PlayerStatus : CharStatus
    {
        public double CurrentCp { get; set; } = 0;

        public PlayerStatus(L2Player player) : base(player)
        {
        }

        public void ReduceCp(int value)
        {
            if (CurrentCp > value)
                CurrentCp = CurrentHp - value;
            else
                CurrentCp = 0;
        }

    }
}