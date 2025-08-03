using BS.Initialize.Abstraction;
using DI.Services.Scheme.Abstraction;
using DI.Services.Scheme.Attributes;
using dnlib.DotNet;
using EH.DeobfuscationCore.Abstraction;
using EH.Logging.Abstraction;
using System;
[assembly: Di]
[module: Di]
namespace EH.DeobfuscationCore;
[DiDescript(Order = 2, Lifetime = EDiServiceLifetime.Singleton)]
public class EhDeobfuscationCore(IEhDeobfuscationProcessor processor, IEhLogger logger) : IBsInitializable
{
    public void Initialize()
    {
        logger.Log("EhDeobfuscationCore initialized");
        ModuleDefMD module = ModuleDefMD.Load("Assembly-CSharp.dll");
        processor.Process(module);
        module.Write("Assembly-CSharp-Deobfuscated.dll");
        Console.ReadLine();
    }
}