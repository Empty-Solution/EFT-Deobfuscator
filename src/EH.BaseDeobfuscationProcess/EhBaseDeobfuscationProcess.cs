using DK.Processing.Abstraction;
using DK.Processing.Abstraction.Generic;
using dnlib.DotNet;
using EH.DeobfuscationCore.Abstraction;
namespace EH.BaseDeobfuscationProcess;
public abstract class EhBaseDeobfuscationProcess : IEhDeobfuscationProcess
{
    public void Execute(IDkProcessor<ModuleDefMD> caller, ModuleDefMD target) => Deobfuscate(target);
    public void Execute(IDkProcessor caller, object target)
    {
        if(target is ModuleDefMD module) Execute(caller, module);
    }
    public bool IsActive => true;
    protected abstract void Deobfuscate(ModuleDefMD module);
}