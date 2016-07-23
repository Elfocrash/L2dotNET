using L2dotNET.GameService.Model.Playable;
using L2dotNET.GameService.World;

namespace L2dotNET.GameService.AI.NpcAI
{
    public class MonsterParameter : Template.Ai
    {
        public int AttackLowLevel = 0;
        public int RunAway = 1;
        public int SetCurse = 0;
        public int AttackLowHp = 0;
        public int HelpHeroSilhouette = 0;
        public string HelpHeroAi = "warrior_hero";
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
        public int Step1 = 20130;
        public int Step2 = 20006;
        public int Step3 = 20853;
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
        public int SsqLoserTeleport = 0;
        public int SsqTelPosX = 0;
        public int SsqTelPosY = 0;
        public int SsqTelPosZ = 0;
        public int SwapPosition = 0;
        public int FriendShip = 0;
        public int DungeonType = 0;
        public int DungeonTypeAi = 0;
        public string DungeonTypePrivate = string.Empty;
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

        public override void AttackFinished(L2Character target)
        {
            if (!(target is L2Summon))
                return;

            if (((L2Summon)target).Owner != null)
                AddAttackDesire(((L2Summon)target).Owner, 500);
        }
    }
}