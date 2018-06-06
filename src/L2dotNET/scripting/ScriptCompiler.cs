using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using L2dotNET.Logging.Abstraction;
using Microsoft.CSharp;

namespace L2dotNET.Scripting
{
    /// <summary>
    /// Idea L2cemu
    /// </summary>
    public class ScriptCompiler
    {
        private static readonly ILog Log = LogProvider.GetCurrentClassLogger();

        private readonly CSharpCodeProvider _provider;

        private static volatile ScriptCompiler _instance;
        private static readonly object SyncRoot = new object();

        public static ScriptCompiler Instance
        {
            get
            {
                if (_instance != null)
                    return _instance;

                lock (SyncRoot)
                {
                    if (_instance == null)
                        _instance = new ScriptCompiler();
                }

                return _instance;
            }
        }

        public ScriptCompiler()
        {
            _provider = new CSharpCodeProvider();
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
                CompilerResults result = _provider.CompileAssemblyFromFile(cp, fname);

                if (result.Errors.Count > 0)
                    Log.Error($"ScriptCompiler: Failed to compile {fname}.");
                else
                    objectList.Add(result.CompiledAssembly.CreateInstance(Path.GetFileNameWithoutExtension(info.Name)));
            }

            Log.Info($"Compiled {objectList.Count} scripted quests.");

            return objectList.ToArray();
        }
    }
}