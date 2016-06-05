using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using log4net;
using Microsoft.CSharp;

namespace L2dotNET.GameService.Scripting
{
    /// <summary>
    /// Idea L2cemu
    /// </summary>
    public class ScriptCompiler
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ScriptCompiler));

        private readonly CSharpCodeProvider provider;

        private static volatile ScriptCompiler instance;
        private static readonly object syncRoot = new object();

        public static ScriptCompiler Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new ScriptCompiler();
                        }
                    }
                }

                return instance;
            }
        }

        public ScriptCompiler()
        {
            provider = new CSharpCodeProvider();
        }

        public object[] CompileFolder(string path)
        {
            CompilerParameters cp = new CompilerParameters();
            cp.ReferencedAssemblies.Add("L2dotNET.GameService.exe");
            cp.GenerateInMemory = true;
            cp.CompilerOptions = "/t:library";

            List<object> objectList = new List<object>();
            foreach (string fname in Directory.GetFiles(path, "*.cs"))
            {
                FileInfo info = new FileInfo(fname);
                CompilerResults result = provider.CompileAssemblyFromFile(cp, fname);

                if (result.Errors.Count > 0)
                    log.Error($"ScriptCompiler: Failed to compile {fname}.");
                else
                    objectList.Add(result.CompiledAssembly.CreateInstance(Path.GetFileNameWithoutExtension(info.Name)));
            }
            log.Info($"Script Compiler: Compiled {objectList.Count} scripted quests.");

            return objectList.ToArray();
        }
    }
}