using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using L2dotNET.Game.logger;
using Microsoft.CSharp;

namespace L2dotNET.Game.scripting
{
    /// <summary>
    /// Idea L2cemu
    /// </summary>
    public class ScriptCompiler
    {
        private static ScriptCompiler instance = new ScriptCompiler();
        public static ScriptCompiler getController()
        {
            return instance;
        }

        private CSharpCodeProvider provider;

        public ScriptCompiler()
        {
            provider = new CSharpCodeProvider();
        }

        public object[] CompileFolder(string path)
        {
            CompilerParameters cp = new CompilerParameters();
            cp.ReferencedAssemblies.Add("L2dotNET.Game.exe");
            cp.GenerateInMemory = true;
            cp.CompilerOptions = "/t:library";

            List<object> objectList = new List<object>();
            foreach (string fname in Directory.GetFiles(path, "*.cs"))
            {
                FileInfo info = new FileInfo(fname);
                CompilerResults result = provider.CompileAssemblyFromFile(cp, fname);

                if (result.Errors.Count > 0)
                    CLogger.error($"ScriptCompiler: Failed to compile { fname }.");
                else
                    objectList.Add(result.CompiledAssembly.CreateInstance(info.Name.Remove(info.Name.Length - 3)));
            }
            CLogger.info($"Script Compiler: Compiled {objectList.Count} scripted quests.");

            return objectList.ToArray();
        }
    }
}
