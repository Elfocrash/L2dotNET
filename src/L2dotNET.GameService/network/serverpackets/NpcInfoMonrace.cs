namespace L2dotNET.GameService.Network.Serverpackets
{
    class NpcInfoMonrace : GameserverPacket
    {
        //private MonsterRunner runner;
        //public NpcInfoMonrace(MonsterRunner runner)
        //{
        //    this.runner = runner;
        //}

        protected internal override void Write()
        {
            WriteByte(0x0c);
            //writeD(runner.id);
            //writeD(runner.npcId + 1000000);
            //writeD(0);
            //writeD(runner.x);
            //writeD(runner.y);
            //writeD(runner.z);
            //writeD(runner.heading);
            //writeD(0);
            //writeD(0);
            //writeD(0);
            //writeD(runner.cur_speed);
            //writeD(0);
            //writeD(0);  // swimspeed
            //writeD(0);  // swimspeed
            //writeD(runner.cur_speed);
            //writeD(0);
            //writeD(runner.cur_speed);
            //writeD(0);
            //writeF(runner.cur_speed * 1f / 130);
            //writeF(1.0);
            //writeF(runner.npcTemplate.CollisionRadius);
            //writeF(runner.npcTemplate.CollisionHeight);
            //writeD(0); // right hand weapon
            //writeD(0);
            //writeD(0); // left hand weapon
            //writeC(1);	// name above char 1=true ... ??
            //writeC(1);
            //writeC(0);
            //writeC(0);
            //writeC(0); // invisible ?? 0=false  1=true   2=summoned (only works if model has a summon animation)
            //writeS("");//name
            //writeS("");//title
            //writeD(0x00); // Title color 0=client default
            //writeD(0x00); //pvp flag
            //writeD(0x00); // karma

            //writeD(0);  // C2
            //writeD(0); //clan id
            //writeD(0); //crest id
            //writeD(0); // ally id
            //writeD(0); // all crest
            //writeC(0); // C2

            //writeC(0);
            //writeF(runner.npcTemplate.CollisionRadius);
            //writeF(runner.npcTemplate.CollisionHeight);
            //writeD(0); // enchant
            //writeD(0); // C6
            //writeD(0);
            //writeD(0);  //red?
            //writeC(0x01);
            //writeC(0x01);
            //writeD(0);
            //writeD(0);//freya
        }
    }
}