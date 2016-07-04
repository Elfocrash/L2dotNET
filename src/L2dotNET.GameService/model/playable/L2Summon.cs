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
        public int StatusSP;
        public long StatusExp;
        public L2Item ControlItem;

        public L2Summon()
        {
            ObjectSummonType = 1;
            ObjId = IdFactory.Instance.nextId();
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

        public virtual void setTemplate(NpcTemplate template)
        {
            Template = template;
            CStatsInit();
            //CharacterStat.setTemplate(template);
            CurHp = CharacterStat.getStat(TEffectType.b_max_hp);
            MaxTime = 1200; //20 минут
            CurrentTime = MaxTime;
            Level = template.Level;
        }

        public int NpcId
        {
            get { return Template.NpcId; }
        }

        public int NpcHashId
        {
            get { return Template.NpcId + 1000000; }
        }

        public override void BroadcastUserInfo()
        {
            foreach (L2Player obj in KnownObjects.Values.OfType<L2Player>())
                obj.SendPacket(new PetInfo(this));
        }

        public byte getPvPStatus()
        {
            return Owner == null ? (byte)0 : Owner.PvPStatus;
        }

        public int getKarma()
        {
            return Owner == null ? 0 : Owner.Karma;
        }

        public virtual long getExpToLevelUp()
        {
            return 0;
        }

        public virtual long getExpCurrentLevel()
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

        public virtual int getForm()
        {
            return 0;
        }

        private bool IsSpawned;

        public void SpawmMe()
        {
            X = Owner.X;
            Y = Owner.Y;
            Z = Owner.Z;
            Heading = Owner.Heading;

            Owner.SendPacket(new PetStatusUpdate(this));

            L2World.Instance.AddObject(this); //to add pet
            IsSpawned = true;
            OnSpawn();

            StartRegeneration();

            AiCharacter = new SA_Standart(this);
            AiCharacter.Enable();
        }

        public void setOwner(L2Player owner)
        {
            Owner = owner;
            owner.Summon = this;

            if (owner.Party != null)
                owner.Party.broadcastToMembers(new ExPartyPetWindowAdd(this));

            Title = owner.Name;
        }

        public virtual void unSummon()
        {
            AiCharacter.Disable();

            Owner.SendPacket(new PetDelete(ObjectSummonType, ObjId));

            if (Owner.Party != null)
                Owner.Party.broadcastToMembers(new ExPartyPetWindowDelete(ObjId, Owner.ObjId, Name));

            Owner.Summon = null;
            DeleteMe();
        }

        public bool isTeleporting = false;
        public double ConsumeExp = 30.0;
        // 0 - teleport, 1 - default, 2 - summoned
        public byte AppearMethod()
        {
            if (!IsSpawned)
                return 2;

            if (isTeleporting)
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

            double dis = Calcs.calculateDistance(this, Owner.CurrentTarget, true);

            if ((dis > 40) && (dis < 2300))
                if (!CantMove())
                    MoveTo(Owner.CurrentTarget.X, Owner.CurrentTarget.Y, Owner.CurrentTarget.Z);
        }

        public override L2Character[] GetPartyCharacters()
        {
            List<L2Character> chars = new List<L2Character>();
            chars.Add(this);
            if (Owner != null)
                chars.Add(Owner);

            if ((Owner != null) && (Owner.Party != null))
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
            if ((Owner == null) || (Owner.Party == null))
                return;

            if (Effects.Count == 0)
                return;

            PartySpelled p = new PartySpelled(this);
            List<AbnormalEffect> nulled = new List<AbnormalEffect>();
            foreach (AbnormalEffect ei in Effects.Where(ei => ei != null))
                if (ei.active == 1)
                    p.addIcon(ei.id, ei.lvl, ei.getTime());
                else
                    nulled.Add(ei);

            lock (Effects)
                foreach (AbnormalEffect ei in nulled)
                    Effects.Remove(ei);

            nulled.Clear();
            Owner.Party.broadcastToMembers(p);
        }

        public override string AsString()
        {
            return "L2Summon:" + ObjId + "";
        }
    }
}