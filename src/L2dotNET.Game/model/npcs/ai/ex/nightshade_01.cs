using L2dotNET.Game.model.skills2;

namespace L2dotNET.Game.model.npcs.ai.ex
{
    class nightshade_01 : AITemplate
    {
        public override void onActionClicked(L2Player player, playable.L2Summon pet, int id)
        {
            string value = null;
            switch (id)
            {
                case 1013:
                    value = "DeBuff1";
                    break;
                case 1014:
                    value = "DeBuff2";
                    break;
                case 1015:
                    value = "Heal";
                    break;

                default:
                    if (id == getValueInt("buff_action3"))
                        value = "buff3";
                    else if (id == getValueInt("buff_action4"))
                        value = "buff4";
                    else if (id == getValueInt("buff_action5"))
                        value = "buff5";
                    break;
            }

            if (value != null)
            {
                int[] skill = getValueSkill(value);
                pet.castSkill(TSkillTable.Instance.get(skill[0], skill[1]));
            }
        }
    }
}
