using DI.Services.Scheme.Abstraction;
using DI.Services.Scheme.Attributes;
using dnlib.DotNet;
using EH.BaseDeobfuscationProcess;
using EH.DeobfuscationCore.Abstraction;
using EH.Logging.Abstraction;
using EH.StringValidationProviding.Abstraction;
using System.Linq;
namespace EH.ClassDeobfuscationProcess;
[DiDescript(Order = 2, Lifetime = EDiServiceLifetime.Singleton, ServiceType = typeof(IEhDeobfuscationProcess), Key = "sonProperty")]
public class EhJsonPropertyDeobfuscationProcess(IEhStringValidator stringValidator, IEhLogger logger) : EhBaseDeobfuscationProcess
{
    protected override void Deobfuscate(ModuleDefMD module)
    {
        foreach(TypeDef typeDef in module.GetTypes())
        {
            foreach(FieldDef fieldDef in typeDef.Fields)
            {
                CustomAttribute? jsonPropertyAttr = fieldDef.CustomAttributes.FirstOrDefault(attr =>
                    attr.AttributeType.Name == "JsonPropertyAttribute" || attr.AttributeType.Name == "JsonProperty");
                TypeDef? returnType = fieldDef.FieldType.TryGetTypeDef();
                if(returnType == null || stringValidator.Validate(returnType.Name)) continue;
                if(jsonPropertyAttr is null) continue;

                string? propertyName = null;
                if(jsonPropertyAttr.ConstructorArguments.Count > 0)
                {
                    CAArgument firstArg                                              = jsonPropertyAttr.ConstructorArguments[0];
                    if(firstArg.Type.ElementType == ElementType.String) propertyName = firstArg.Value as string;
                }
                if(string.IsNullOrEmpty(propertyName))
                {
                    CANamedArgument? nameProperty = jsonPropertyAttr.NamedArguments.FirstOrDefault(na => na.Name == "PropertyName" || na.Name == "Name");
                    if(nameProperty.Argument.Type.ElementType == ElementType.String) propertyName = nameProperty.Argument.Value as string;
                }
                if(string.IsNullOrEmpty(propertyName)) continue;

                logger.Log($"Renaming {returnType.Name} to {propertyName} [Token: 0x{fieldDef.MDToken.Raw:X8}]");
                returnType.Name = propertyName;
            }
        }
    }
}