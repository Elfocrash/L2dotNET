namespace L2dotNET.Network.serverpackets
{
    class RecipeBookItemList : GameserverPacket
    {
        public override void Write()
        {
            WriteByte(0xdc);
        }
    }
}