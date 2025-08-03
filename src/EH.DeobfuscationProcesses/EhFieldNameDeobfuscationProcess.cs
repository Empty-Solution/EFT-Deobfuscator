using DI.Services.Scheme.Abstraction;
using DI.Services.Scheme.Attributes;
using dnlib.DotNet;
using EH.BaseDeobfuscationProcess;
using EH.DeobfuscationCore.Abstraction;
using EH.Logging.Abstraction;
using EH.StringValidation.Abstraction;
namespace EH.DeobfuscationProcesses;
[DiDescript(Order = 2, Lifetime = EDiServiceLifetime.Singleton, ServiceType = typeof(IEhDeobfuscationProcess), Key = "FieldName")]
public class EhFieldNameDeobfuscationProcess(IEhStringValidator stringValidator, IEhLogger logger) : EhBaseDeobfuscationProcess
{
    protected override void Deobfuscate(ModuleDefMD module)
    {
        foreach(TypeDef typeDef in module.GetTypes())
        {
            foreach(FieldDef fieldDef in typeDef.Fields)
            {
                if(!stringValidator.Validate(fieldDef.Name) || !fieldDef.FieldType.IsTypeDefOrRef) continue;

                UTF8String? loweredFieldName = fieldDef.Name.ToLower();
                if(loweredFieldName == "instance" || loweredFieldName == "default" || loweredFieldName == "value" || loweredFieldName.Contains("<") ||
                   loweredFieldName.Contains(">"))
                    continue;

                TypeDef? returnType = fieldDef.FieldType.TryGetTypeDef();
                if(returnType == null || stringValidator.Validate(returnType.Name)) continue;

                logger.Log($"Renaming {returnType.Name} to {fieldDef.Name} [Token: 0x{fieldDef.MDToken.Raw:X8}]");
                returnType.Name = fieldDef.Name;
            }
        }
    }
}