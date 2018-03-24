namespace L2dotNET.Network.serverpackets
{
    class RecipeItemMakeInfo : GameserverPacket
    {
        public override void Write()
        {
            WriteByte(0xdd);
        }
    }
}