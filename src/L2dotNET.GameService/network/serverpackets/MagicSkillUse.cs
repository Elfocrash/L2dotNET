using L2dotNET.GameService.Model.Skills2;
using L2dotNET.GameService.World;

namespace L2dotNET.GameService.Network.Serverpackets
{
    public class MagicSkillUse : GameServerNetworkPacket
    {
        private readonly int _level;
        private readonly int _id;
        private readonly int _hitTime;
        private readonly int _targetId;
        private readonly int _casterId;
        private readonly int _x;
        private readonly int _tx;
        private readonly int _y;
        private readonly int _ty;
        private readonly int _z;
        private readonly int _tz;
        private readonly int _damageSuccess;

        public MagicSkillUse(L2Character caster, L2Object target, Skill skill, int hitTime, int flag = 0)
        {
            _id = skill.SkillId;
            _level = skill.Level;
            _hitTime = hitTime;
            _targetId = target.ObjId;
            _casterId = caster.ObjId;
            _x = caster.X;
            _y = caster.Y;
            _z = caster.Z;
            _tx = target.X;
            _ty = target.Y;
            _tz = target.Z;
            _damageSuccess = flag;
        }

        public MagicSkillUse(L2Character caster, L2Object target, int id, int lvl, int hitTime, int flag = 0)
        {
            _id = id;
            _level = lvl;
            _hitTime = hitTime;
            _targetId = target.ObjId;
            _casterId = caster.ObjId;
            _x = caster.X;
            _y = caster.Y;
            _z = caster.Z;
            _tx = target.X;
            _ty = target.Y;
            _tz = target.Z;
            _damageSuccess = flag;
        }

        protected internal override void Write()
        {
            WriteC(0x48);
            WriteD(_casterId);
            WriteD(_targetId);
            WriteD(_id);
            WriteD(_level);
            WriteD(_hitTime);
            WriteD(0); //_reuseDelay
            WriteD(_x);
            WriteD(_y);
            WriteD(_z);
            if (_damageSuccess != 0)
            {
                WriteD(_damageSuccess);
                WriteH(0x00);
            }
            else
                WriteD(0x00);
            WriteD(_tx);
            WriteD(_ty);
            WriteD(_tz);
        }
    }
}