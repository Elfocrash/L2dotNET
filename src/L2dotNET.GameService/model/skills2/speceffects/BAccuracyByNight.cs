using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Model.Skills2.Effects;
using L2dotNET.GameService.Network.Serverpackets;

namespace L2dotNET.GameService.Model.Skills2.SpecEffects
{
    public class BAccuracyByNight : SpecEffect
    {
        private readonly Effect _effect;

        public BAccuracyByNight(double value, int skillId, int lvl)
        {
            Value = value;
            _effect = new BAccuracy
                      {
                          HashId = (skillId * 65536) + lvl,
                          SkillId = skillId,
                          SkillLv = lvl
                      };
            _effect.Build($"st +{value}");
        }

        public override void OnStartNight(L2Player player)
        {
            player.SendPacket(new SystemMessage(SystemMessage.SystemMessageId.NightS1EffectApplies).AddSkillName(_effect.SkillId, _effect.SkillLv));
            player.AddStat(_effect);
        }

        public override void OnStartDay(L2Player player)
        {
            player.SendPacket(new SystemMessage(SystemMessage.SystemMessageId.DayS1EffectDisappears).AddSkillName(_effect.SkillId, _effect.SkillLv));
            player.RemoveStat(_effect);
        }
    }
}