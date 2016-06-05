using L2dotNET.GameService.model.skills2;
using L2dotNET.GameService.world;

namespace L2dotNET.GameService.network.l2send
{
    public class MagicSkillUse : GameServerNetworkPacket
    {
        private readonly int _level;
        private readonly int _id;
        private readonly int _hitTime;
        private readonly int _targetId;
        private readonly int _casterId;
        private readonly int _x;
        private readonly int tx;
        private readonly int _y;
        private readonly int ty;
        private readonly int _z;
        private readonly int tz;
        private readonly int _damageSuccess;
        public MagicSkillUse(L2Character caster, L2Object target, TSkill skill, int hitTime, int flag = 0)
        {
            _id = skill.skill_id;
            _level = skill.level;
            _hitTime = hitTime;
            _targetId = target.ObjID;
            _casterId = caster.ObjID;
            _x = caster.X;
            _y = caster.Y;
            _z = caster.Z;
            tx = target.X;
            ty = target.Y;
            tz = target.Z;
            _damageSuccess = flag;
        }

        public MagicSkillUse(L2Character caster, L2Object target, int id, int lvl, int hitTime, int flag = 0)
        {
            _id = id;
            _level = lvl;
            _hitTime = hitTime;
            _targetId = target.ObjID;
            _casterId = caster.ObjID;
            _x = caster.X;
            _y = caster.Y;
            _z = caster.Z;
            tx = target.X;
            ty = target.Y;
            tz = target.Z;
            _damageSuccess = flag;
        }

        protected internal override void write()
        {
            writeC(0x48);
            writeD(_casterId);
            writeD(_targetId);
            writeD(_id);
            writeD(_level);
            writeD(_hitTime);
            writeD(0); //_reuseDelay
            writeD(_x);
            writeD(_y);
            writeD(_z);
            if (_damageSuccess != 0)
            {
                writeD(_damageSuccess);
                writeH(0x00);
            }
            else
                writeD(0x00);
            writeD(tx);
            writeD(ty);
            writeD(tz);
        }
    }
}
