using System.Runtime.CompilerServices;
using L2dotNET.Models.player;

namespace L2dotNET.Models.Status
{
    public class PlayerStatus : CharStatus
    {
        public double CurrentCp { get; set; } = 0;

        public PlayerStatus(L2Player player) : base(player)
        {
            Character = player;
        }

        public void SetCurrentCp(double newHp)
        {
            SetCurrentHp(newHp, true);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void SetCurrentCp(double newCp, bool broadcastUpdate)
        {
            L2Player player = (L2Player)Character;
            double maxCp = player.MaxCp;

            if (player.Dead)
                return;

            if (newCp < 0)
                newCp = 0;

            if(newCp >= maxCp)
            {
                CurrentCp = maxCp;
                _flagsRegenActive &= RegenFlagCp;

                if(_flagsRegenActive == 0)
                    StopHpMpRegeneration();
            }
            
            if (broadcastUpdate)
                Character.BroadcastStatusUpdate();
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