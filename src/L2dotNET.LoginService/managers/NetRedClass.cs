namespace L2dotNET.LoginService.Managers
{
    class NetRedClass
    {
        public string Mask,
                      Redirect;
        public short ServerId;
        public byte[] RedirectBits;

        public void SetRedirect(string p)
        {
            Redirect = p;
            RedirectBits = new byte[4];

            string[] w = Redirect.Split('.');
            RedirectBits[0] = byte.Parse(w[0]);
            RedirectBits[1] = byte.Parse(w[1]);
            RedirectBits[2] = byte.Parse(w[2]);
            RedirectBits[3] = byte.Parse(w[3]);
        }
    }
}