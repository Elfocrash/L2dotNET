namespace L2dotNET.LoginService.managers
{
    class NetRedClass
    {
        public string mask,
                      redirect;
        public short serverId;
        public byte[] redirectBits;

        public void setRedirect(string p)
        {
            redirect = p;
            redirectBits = new byte[4];

            string[] w = redirect.Split('.');
            redirectBits[0] = byte.Parse(w[0]);
            redirectBits[1] = byte.Parse(w[1]);
            redirectBits[2] = byte.Parse(w[2]);
            redirectBits[3] = byte.Parse(w[3]);
        }
    }
}