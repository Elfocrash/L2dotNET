using L2dotNET.model.items;
using L2dotNET.model.player;
using L2dotNET.world;

namespace L2dotNET.Models.Stats
{
    public class Env
    {
        public L2Character Character { get; set; }
        public L2Character Target { get; set; }
        public L2Item Item { get; set; }

        public double Value { get; set; }
        public double BaseValue { get; set; }

        public bool SkillMastery { get; set; } = false;
        public byte Shield { get; set; }

        public bool SoulShot { get; set; } = false;
        public bool SpiritShot { get; set; } = false;
        public bool BlessedSpiritShor { get; set; } = false;

        public void AddValue(double value)
        {
            Value += value;
        }

        public void SubValue(double value)
        {
            Value -= value;
        }

        public void MulValue(double value)
        {
            Value *= value;
        }

        public void DivValue(double value)
        {
            Value /= value;
        }
    }
}