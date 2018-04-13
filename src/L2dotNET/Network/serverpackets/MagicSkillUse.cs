using L2dotNET.Models;
using L2dotNET.World;

namespace L2dotNET.Network.serverpackets
{
    public class MagicSkillUse : GameserverPacket
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

        public MagicSkillUse(L2Character caster, L2Object target, dynamic skill, int hitTime, int flag = 0)
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

        public override void Write()
        {
            WriteByte(0x48);
            WriteInt(_casterId);
            WriteInt(_targetId);
            WriteInt(_id);
            WriteInt(_level);
            WriteInt(_hitTime);
            WriteInt(0); //_reuseDelay
            WriteInt(_x);
            WriteInt(_y);
            WriteInt(_z);
            if (_damageSuccess != 0)
            {
                WriteInt(_damageSuccess);
                WriteShort(0x00);
            }
            else
                WriteInt(0x00);
            WriteInt(_tx);
            WriteInt(_ty);
            WriteInt(_tz);
        }
    }
}