using DI.Services.Scheme.Abstraction;
using DI.Services.Scheme.Attributes;
using dnlib.DotNet;
using EH.BaseDeobfuscationProcess;
using EH.DeobfuscationCore.Abstraction;
using EH.Logging.Abstraction;
namespace EH.ClassDeobfuscationProcess;
[DiDescript(Order = 0, Lifetime = EDiServiceLifetime.Singleton, ServiceType = typeof(IEhDeobfuscationProcess))]
public class EhClassDeobfuscationProcess(IEhLogger logger) : EhBaseDeobfuscationProcess
{
    protected override void Deobfuscate(ModuleDefMD module)
    {
        foreach(TypeDef typeDef in module.GetTypes())
        {
            foreach(MethodDef methodDef in typeDef.Methods)
            {
                foreach(Parameter parameter in methodDef.Parameters)
                {
                    if(string.IsNullOrEmpty(parameter.Name) || string.IsNullOrWhiteSpace(parameter.Name)) continue;
                    if(!parameter.Type.IsTypeDefOrRef) continue;

                    TypeDef? parameterTypeDef = parameter.Type.TryGetTypeDef();
                    if(parameterTypeDef == null) continue;

                    logger.Log($"Renaming {parameterTypeDef.Name} to {parameter.Name}");
                    parameterTypeDef.Name = parameter.Name;
                }
            }
        }
    }
}