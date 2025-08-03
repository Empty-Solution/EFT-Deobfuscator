using DI.Engine.Attributes;
using DI.Services.Abstraction;
using DI.Services.Scheme.Abstraction;
using DI.Services.Scheme.Attributes;
using DK.Processing.Abstraction;
using DK.Processing.Abstraction.Generic;
using dnlib.DotNet;
using EH.DeobfuscationCore.Abstraction;
using System.Collections.Generic;
namespace EH.DeobfuscationCore;
[DiDescript(Order = 5, Lifetime = EDiServiceLifetime.Singleton, ServiceType = typeof(IEhDeobfuscationProcessor))]
public class EhDeobfuscationProcessor(
    [DiParameter(IgnoreKeys = true, ServiceEquals = EDiServiceEquals.SubClassOrAssignableOrEquals)]
    IEnumerable<IEhDeobfuscationProcess> deobfuscationProcesses) : IEhDeobfuscationProcessor
{
    public void Process(ModuleDefMD target)
    {
        foreach(IEhDeobfuscationProcess? process in deobfuscationProcesses) process.Execute(this, target);
    }
    public void Process(object target)
    {
        if(target is ModuleDefMD module) Process(module);
    }
    IEnumerable<IDkProcess> IDkProcessor.       Processes => Processes;
    public IEnumerable<IDkProcess<ModuleDefMD>> Processes => deobfuscationProcesses;
}