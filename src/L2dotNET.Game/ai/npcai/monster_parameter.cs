using L2dotNET.GameService.ai.template;
using L2dotNET.GameService.model.playable;

namespace L2dotNET.GameService.ai.npcai
{
    public class monster_parameter : AI
    {
        public int AttackLowLevel = 0;
        public int RunAway = 1;
        public int SetCurse = 0;
        public int AttackLowHP = 0;
        public int HelpHeroSilhouette = 0;
        public string HelpHeroAI = "warrior_hero";
        public int SetAggressiveTime = -1;
        public int HalfAggressive = 0;
        public int RandomAggressive = 0;
        public int SetHateGroup = -1;
        public int SetHateGroupRatio = 0;
        public int SetHateOccupation = -1;
        public int SetHateOccupationRatio = 0;
        public int SetHateRace = -1;
        public int SetHateRaceRatio = 0;
        public int IsTransform = 0;
        public int step1 = 20130;
        public int step2 = 20006;
        public int step3 = 20853;
        public int DaggerBackAttack = 458752001;
        public int IsVs = 0;
        public int SpecialSkill = 458752001;
        public int MoveAroundSocial = 0;
        public int MoveAroundSocial1 = 0;
        public int MoveAroundSocial2 = 0;
        public int IsSay = 0;
        public int ShoutMsg1 = 0;
        public int ShoutMsg2 = 0;
        public int ShoutMsg3 = 0;
        public int ShoutMsg4 = 0;
        public int SSQLoserTeleport = 0;
        public int SSQTelPosX = 0;
        public int SSQTelPosY = 0;
        public int SSQTelPosZ = 0;
        public int SwapPosition = 0;
        public int FriendShip = 0;
        public int DungeonType = 0;
        public int DungeonTypeAI = 0;
        public string DungeonTypePrivate = "";
        public int ShoutTarget = 0;
        public int AcceptShoutTarget = 0;
        public int SelfExplosion = 0;
        public int FriendShip1 = 0;
        public int FriendShip2 = 0;
        public int FriendShip3 = 0;
        public int FriendShip4 = 0;
        public int FriendShip5 = 0;
        public int SoulShot = 0;
        public int SoulShotRate = 0;
        public int SpiritShot = 0;
        public int SpiritShotRate = 0;
        public int SpeedBonus = 0;
        public int HealBonus = 0;
        public int CreviceOfDiminsion = 0;
        public int LongRangeGuardRate = -1;
        public int SeeCreatureAttackerTime = -1;

        public override void AttackFinished(world.L2Character target)
        {
            if (target is L2Summon)
            {
                if (((L2Summon)target).Owner != null)
                    AddAttackDesire(((L2Summon)target).Owner, 500);
            }
        }
    }
}