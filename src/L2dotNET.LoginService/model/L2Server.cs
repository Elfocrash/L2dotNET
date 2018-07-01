﻿using L2dotNET.LoginService.GSCommunication;
using L2dotNET.LoginService.Managers;
using L2dotNET.LoginService.Network;

namespace L2dotNET.LoginService.Model
{
    public class L2Server
    {
        public byte[] DefaultAddress { get; set; }
        public ServerThread Thread { get; set; }
        public string Info { get; set; }
        public byte Id { get; set; }

        public byte[] GetIp(LoginClient client)
        {
            if (DefaultAddress == null)
            {
                string ip = "0.0.0.0";
                if (Thread != null)
                    ip = Thread.Wan;

                DefaultAddress = new byte[4];
                string[] w = ip.Split('.');
                DefaultAddress[0] = byte.Parse(w[0]);
                DefaultAddress[1] = byte.Parse(w[1]);
                DefaultAddress[2] = byte.Parse(w[2]);
                DefaultAddress[3] = byte.Parse(w[3]);
            }

            if (Thread == null)
                return DefaultAddress;

            byte[] redirect = NetworkRedirect.Instance.GetRedirect(client, Id);
            return redirect ?? DefaultAddress;
        }

        public byte Connected => Thread != null ? (Thread.Connected ? (byte)1 : (byte)0) : (byte)0;

        public short CurrentPlayers => Thread?.Curp ?? (short)0;

        public short MaxPlayers => Thread?.Maxp ?? (short)0;

        public int Port => Thread?.Port ?? 0;

        public bool TestMode => (Thread != null) && Thread.TestMode;

        public bool GmOnly => (Thread != null) && Thread.GmOnly;
    }
}