using System;
using System.IO;

namespace L2dotNET.Game.logger
{
    class CLogger
    {
        static bool cf = false;
        public static void warning(string text)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(text);
            if(cf)tw.WriteLine(text);
            Console.ResetColor();
        }

        public static void error(string text)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(text);
            if (cf) tw.WriteLine(text);
            Console.ResetColor();
        }

        public static void info(string text)
        {
            Console.WriteLine(text);
            if (cf) tw.WriteLine(text);
        }

        public static void extra_info(string text)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(text);
            if (cf) tw.WriteLine(text);
            Console.ResetColor();
        }

        static string name;
        static TextWriter tw;
        public static void form()
        {
            if (!cf)
                return;

            name = @"log\" + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss") + ".log";
            tw = File.CreateText(name);
            extra_info("Log file created " + name);
        }
    }
}
