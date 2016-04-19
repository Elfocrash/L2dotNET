using System.Collections.Generic;
using L2dotNET.Game.tables;

namespace L2dotNET.Game.model.player
{
    public class PlayerTemplate
    {
        public byte id;
        public byte race;
        public staticf.ClassId pch;
        public byte level;
        public byte transfer;
        public double patk;
        public double pdef;
        public int atkrange;
        public int rndmg;
        public double matk;
        public double mdef;
        public double critical;
        public double atkspd;
        public double runspd;
        public double walkspd;
        public double waterspd;
        public double collr_f = -1;
        public double collh_f = -1;
        public double collr_m;
        public double collh_m;
        public int fall_f;
        public int fall_m;
        public int breath;
        public double[] _hp;
        public double[] _mp;
        public double[] _cp;
        public double[] _regHp;
        public double[] _regMp;
        public List<PC_item> _items;
        public byte _int;
        public byte _str;
        public byte _con;
        public byte _men;
        public byte _dex;
        public byte _wit;

        public double getCollRadius(int sex)
        {
            double val = collr_m;

            if (sex == 1 && collr_f != -1)
                val = collr_f;

            return val;
        }

        public double getCollHeight(int sex)
        {
            double val = collh_m;

            if (sex == 1 && collh_f != -1)
                val = collh_f;

            return val;
        }
    }
}
