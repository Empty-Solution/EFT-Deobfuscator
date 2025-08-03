using DI.Services.Scheme.Abstraction;
using DI.Services.Scheme.Attributes;
using dnlib.DotNet;
using EH.BaseDeobfuscationProcess;
using EH.DeobfuscationCore.Abstraction;
using EH.Logging.Abstraction;
using EH.StringValidationProviding.Abstraction;
namespace EH.ClassDeobfuscationProcess;
[DiDescript(Order = 2, Lifetime = EDiServiceLifetime.Singleton, ServiceType = typeof(IEhDeobfuscationProcess), Key = "MethodName")]
public class EhMethodNameDeobfuscationProcess(IEhStringValidator stringValidator, IEhLogger logger) : EhBaseDeobfuscationProcess
{
    protected override void Deobfuscate(ModuleDefMD module)
    {
        foreach(TypeDef typeDef in module.GetTypes())
        {
            foreach(MethodDef methodDef in typeDef.Methods)
            {
                UTF8String? loweredMethodName = methodDef.Name.ToLower();
                if(!methodDef.ReturnType.IsTypeDefOrRef) continue;
                if(methodDef.IsGetter || methodDef.IsSetter) continue;
                if(loweredMethodName == "get_value" || loweredMethodName == "getvalue" || loweredMethodName == "getinstance" ||
                   loweredMethodName == "getdefault")
                    continue; // hardcoded default patterns
                if(!loweredMethodName.StartsWith("get")) continue;

                TypeDef? returnType = methodDef.ReturnType.TryGetTypeDef();
                if(returnType == null || stringValidator.Validate(returnType.Name)) continue;

                int substringIndex = loweredMethodName.StartsWith("get_") ? 4 : 3;
                logger.Log(
                $"Renaming {returnType.Name} to {methodDef.Name.Substring(substringIndex)} [Token: 0x{methodDef.MDToken.Raw:X8}, Index: {substringIndex}]");
                returnType.Name = methodDef.Name.Substring(substringIndex);
            }
        }
    }
}