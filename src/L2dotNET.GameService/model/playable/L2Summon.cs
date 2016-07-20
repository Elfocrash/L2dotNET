using System.Collections.Generic;
using System.Linq;
using L2dotNET.GameService.Model.Items;
using L2dotNET.GameService.Model.Playable.PetAI;
using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Model.Skills;
using L2dotNET.GameService.Model.Skills2;
using L2dotNET.GameService.Network.Serverpackets;
using L2dotNET.GameService.Tables;
using L2dotNET.GameService.Templates;
using L2dotNET.GameService.Tools;
using L2dotNET.GameService.World;

namespace L2dotNET.GameService.Model.Playable
{
    public class L2Summon : L2Character
    {
        public L2Player Owner;
        public NpcTemplate Template;
        public int CurrentTime;
        public int MaxTime;
        public int StatusSp;
        public long StatusExp;
        public L2Item ControlItem;

        public L2Summon()
        {
            ObjectSummonType = 1;
            ObjId = IdFactory.Instance.NextId();
        }

        public override void OnAction(L2Player player)
        {
            player.SendMessage(AsString());

            player.ChangeTarget(this);
        }

        public override void NotifyAction(L2Player player)
        {
            if ((Owner != null) && (Owner.ObjId == player.ObjId))
                player.SendPacket(new PetStatusShow(ObjectSummonType));
        }

        public virtual void SetTemplate(NpcTemplate template)
        {
            Template = template;
            CStatsInit();
            //CharacterStat.setTemplate(template);
            CurHp = CharacterStat.GetStat(EffectType.BMaxHp);
            MaxTime = 1200; //20 минут
            CurrentTime = MaxTime;
            Level = template.Level;
        }

        public int NpcId => Template.NpcId;

        public int NpcHashId => Template.NpcId + 1000000;

        public override void BroadcastUserInfo()
        {
            foreach (L2Player obj in KnownObjects.Values.OfType<L2Player>())
                obj.SendPacket(new PetInfo(this));
        }

        public byte GetPvPStatus()
        {
            return Owner?.PvPStatus ?? (byte)0;
        }

        public int GetKarma()
        {
            return Owner?.Karma ?? 0;
        }

        public virtual long GetExpToLevelUp()
        {
            return 0;
        }

        public virtual long GetExpCurrentLevel()
        {
            return 0;
        }

        public virtual int CurrentWeight()
        {
            return 0;
        }

        public virtual int MaxWeight()
        {
            return 0;
        }

        public virtual short IsMountable()
        {
            return 0;
        }

        public virtual int GetForm()
        {
            return 0;
        }

        private bool _isSpawned;

        public void SpawmMe()
        {
            X = Owner.X;
            Y = Owner.Y;
            Z = Owner.Z;
            Heading = Owner.Heading;

            Owner.SendPacket(new PetStatusUpdate(this));

            L2World.Instance.AddObject(this); //to add pet
            _isSpawned = true;
            OnSpawn();

            StartRegeneration();

            AiCharacter = new SaStandart(this);
            AiCharacter.Enable();
        }

        public void SetOwner(L2Player owner)
        {
            Owner = owner;
            owner.Summon = this;

            owner.Party?.BroadcastToMembers(new ExPartyPetWindowAdd(this));

            Title = owner.Name;
        }

        public virtual void UnSummon()
        {
            AiCharacter.Disable();

            Owner.SendPacket(new PetDelete(ObjectSummonType, ObjId));

            Owner.Party?.BroadcastToMembers(new ExPartyPetWindowDelete(ObjId, Owner.ObjId, Name));

            Owner.Summon = null;
            DeleteMe();
        }

        public bool IsTeleporting = false;
        public double ConsumeExp = 30.0;

        // 0 - teleport, 1 - default, 2 - summoned
        public byte AppearMethod()
        {
            if (!_isSpawned)
                return 2;

            if (IsTeleporting)
                return 0;

            return 1;
        }

        public virtual void ChangeNode()
        {
            AiCharacter.ChangeFollowStatus();
        }

        public virtual void Attack() { }

        public virtual void Stop()
        {
            NotifyStopMove(false, true);
        }

        public virtual void Move()
        {
            if (Owner.CurrentTarget == null)
                return;

            double dis = Calcs.CalculateDistance(this, Owner.CurrentTarget, true);

            if (!(dis > 40) || !(dis < 2300))
                return;

            if (!CantMove())
                MoveTo(Owner.CurrentTarget.X, Owner.CurrentTarget.Y, Owner.CurrentTarget.Z);
        }

        public override L2Character[] GetPartyCharacters()
        {
            List<L2Character> chars = new List<L2Character>
                                      {
                                          this
                                      };
            if (Owner != null)
                chars.Add(Owner);

            if (Owner?.Party == null)
                return chars.ToArray();

            foreach (L2Player pl in Owner.Party.Members.Where(pl => pl.ObjId != Owner.ObjId))
            {
                chars.Add(pl);

                if (pl.Summon != null)
                    chars.Add(pl.Summon);
            }

            return chars.ToArray();
        }

        public override void UpdateAbnormalEffect()
        {
            if (Owner?.Party == null)
                return;

            if (Effects.Count == 0)
                return;

            PartySpelled p = new PartySpelled(this);
            List<AbnormalEffect> nulled = new List<AbnormalEffect>();
            lock (Effects)
            {
                foreach (AbnormalEffect ei in Effects.Where(ei => ei != null))
                {
                    if (ei.Active == 1)
                        p.AddIcon(ei.Id, ei.Lvl, ei.GetTime());
                    else
                        nulled.Add(ei);
                }

                foreach (AbnormalEffect ei in nulled)
                    Effects.Remove(ei);
            }

            nulled.Clear();
            Owner.Party.BroadcastToMembers(p);
        }

        public override string AsString()
        {
            return "L2Summon:" + ObjId + "";
        }
    }
}