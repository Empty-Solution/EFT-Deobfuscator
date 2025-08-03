using DI.Services.Scheme.Abstraction;
using DI.Services.Scheme.Attributes;
using dnlib.DotNet;
using EH.BaseDeobfuscationProcess;
using EH.DeobfuscationCore.Abstraction;
using EH.Logging.Abstraction;
using EH.StringValidation.Abstraction;
namespace EH.DeobfuscationProcesses;
[DiDescript(Order = 2, Lifetime = EDiServiceLifetime.Singleton, ServiceType = typeof(IEhDeobfuscationProcess), Key = "Argument")]
public class EhArgumentDeobfuscationProcess(IEhStringValidator stringValidator, IEhLogger logger) : EhBaseDeobfuscationProcess
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
                    if(parameter.Name.Length < 2) continue;
                    if(!parameter.Type.IsTypeDefOrRef) continue;

                    TypeDef? parameterTypeDef = parameter.Type.TryGetTypeDef();
                    if(parameterTypeDef == null) continue;
                    if(stringValidator.Validate(parameterTypeDef.Name)) continue;

                    logger.Log($"Renaming {parameterTypeDef.Name} to {parameter.Name} [Token: 0x{methodDef.MDToken.Raw:X8}, Index: {parameter.Index}]");
                    parameterTypeDef.Name = parameter.Name;
                }
            }
        }
    }
}