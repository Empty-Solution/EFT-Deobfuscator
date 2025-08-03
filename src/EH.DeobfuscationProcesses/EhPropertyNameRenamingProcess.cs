using DI.Services.Scheme.Abstraction;
using DI.Services.Scheme.Attributes;
using dnlib.DotNet;
using EH.BaseDeobfuscationProcess;
using EH.DeobfuscationCore.Abstraction;
using EH.Logging.Abstraction;
using EH.StringValidation.Abstraction;
namespace EH.DeobfuscationProcesses;
[DiDescript(Order = 2, Lifetime = EDiServiceLifetime.Singleton, ServiceType = typeof(IEhDeobfuscationProcess), Key = "PropertyName")]
public class EhPropertyNameRenamingProcess(IEhStringValidator stringValidator, IEhLogger logger) : EhBaseDeobfuscationProcess
{
    protected override void Deobfuscate(ModuleDefMD module)
    {
        foreach(TypeDef typeDef in module.GetTypes())
        {
            foreach(PropertyDef propertyDef in typeDef.Properties)
            {
                if(!stringValidator.Validate(propertyDef.Name) || !propertyDef.PropertySig.RetType.IsTypeDefOrRef) continue;

                UTF8String? loweredPropertyName = propertyDef.Name.ToLower();
                if(loweredPropertyName == "instance" || loweredPropertyName == "default" || loweredPropertyName == "value" ||
                   loweredPropertyName.Contains("this"))
                    continue;

                TypeDef? returnType = propertyDef.PropertySig.RetType.TryGetTypeDef();
                if(returnType == null || stringValidator.Validate(returnType.Name)) continue;

                logger.Log($"Renaming {returnType.Name} to {propertyDef.Name} [Token: 0x{propertyDef.MDToken.Raw:X8}]");
                returnType.Name = propertyDef.Name;
            }
        }
    }
}