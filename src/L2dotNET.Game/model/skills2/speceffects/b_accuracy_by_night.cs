using L2dotNET.GameService.model.skills2.effects;
using L2dotNET.GameService.network.l2send;

namespace L2dotNET.GameService.model.skills2.speceffects
{
    public class b_accuracy_by_night : TSpecEffect
    {
        private TEffect effect;
        public b_accuracy_by_night(double value, int skillId, int lvl)
        {
            this.value = value;
            effect = new b_accuracy();
            effect.HashID = skillId * 65536 + lvl;
            effect.SkillId = skillId;
            effect.SkillLv = lvl;
            effect.build("st +" + value);
        }

        public override void OnStartNight(L2Player player)
        {
            player.sendPacket(new SystemMessage(SystemMessage.SystemMessageId.NIGHT_S1_EFFECT_APPLIES).AddSkillName(effect.SkillId, effect.SkillLv));
            player.addStat(effect);
        }

        public override void OnStartDay(L2Player player)
        {
            player.sendPacket(new SystemMessage(SystemMessage.SystemMessageId.DAY_S1_EFFECT_DISAPPEARS).AddSkillName(effect.SkillId, effect.SkillLv));
            player.removeStat(effect);
        }
    }
}
