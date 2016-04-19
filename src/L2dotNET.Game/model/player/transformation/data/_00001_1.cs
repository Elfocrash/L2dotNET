using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace L2dotNET.Game.model.player.transformation.data
{
    class _00001_1 : TransformTemplate
    {
        public _00001_1()
        {
            id = 1;
            collision_box = new double[] { 12, 14.5 };
            moving_speed = new int[] { 30, 125, 50, 50, 0, 0, 0, 0 };
            skills = new int[] { 584, 1, 585, 1, 5491, 1, TransformDispelId };
            action = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 11, 15, 16, 17, 18, 19, 21, 22, 23, 32, 36, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 50, 52, 53, 54, 55, 56, 57, 63, 64, 65, 70, 1000, 1001, 1003, 1004, 1005, 1006, 1007, 1008, 1009, 1010, 1011, 1012, 1013, 1014, 1015, 1016, 1017, 1018, 1019, 1020, 1021, 1022, 1023, 1024, 1025, 1026, 1027, 1028, 1029, 1030, 1031, 1032, 1033, 1034, 1035, 1036, 1037, 1038, 1039, 1040, 1041, 1042, 1043, 1044, 1045, 1046, 1047, 1048, 1049, 1050, 1051, 1052, 1053, 1054, 1055, 1056, 1057, 1058, 1059, 1060, 1061, 1062, 1063, 1064, 1065, 1066, 1067, 1068, 1069, 1070, 1071, 1072, 1073, 1074, 1075, 1076, 1077, 1078, 1079, 1080, 1081, 1082, 1083, 1084, 1089, 1090, 1091, 1092, 1093, 1094, 1095, 1096, 1097, 1098 };
            base_attack_range = 20;
            base_random_damage = 10;
            base_attack_speed = 300;
            base_critical_prob = 5;
            base_physical_attack = 5;
            base_magical_attack = 5;
        }
    }
}
