using DI.Services.Scheme.Abstraction;
using DI.Services.Scheme.Attributes;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using EH.BaseDeobfuscationProcess;
using EH.DeobfuscationCore.Abstraction;
using EH.Logging.Abstraction;
using EH.StringValidation.Abstraction;
using System.Reflection;
namespace EH.DeobfuscationProcesses;
[DiDescript(Order = 2, Lifetime = EDiServiceLifetime.Singleton, ServiceType = typeof(IEhDeobfuscationProcess), Key = "StringDecryption")]
public class EhStringDecryptionProcess(IEhStringValidator stringValidator, IEhLogger logger) : EhBaseDeobfuscationProcess
{
    protected override void Deobfuscate(ModuleDefMD module)
    {
        Assembly    assembly         = Assembly.LoadFrom("Assembly-CSharp.dll");
        MethodBase? decryptionMethod = null;
        foreach(TypeDef typeDef in module.GetTypes())
        {
            foreach(MethodDef methodDef in typeDef.Methods)
            {
                if(!methodDef.HasBody) continue;

                for(int i = 0; i < methodDef.Body.Instructions.Count; i++)
                {
                    Instruction? instruction = methodDef.Body.Instructions[i];
                    if(i <= 0 || instruction.OpCode != OpCodes.Call || methodDef.Body.Instructions[i - 1].OpCode != OpCodes.Ldc_I4) continue;
                    if(instruction.Operand is not MethodDef decryptionMethodStatic) continue;
                    if(!decryptionMethodStatic.HasBody || decryptionMethodStatic.Body.Instructions[0].OpCode != OpCodes.Call ||
                       !decryptionMethodStatic.Body.Instructions[0].Operand.ToString().Contains("get_CurrentDomain"))
                        continue;

                    decryptionMethod                          ??= assembly.ManifestModule.ResolveMethod((int)decryptionMethodStatic.MDToken.Raw);
                    instruction.OpCode                        =   OpCodes.Ldstr;
                    instruction.Operand                       =   decryptionMethod.Invoke(null, [methodDef.Body.Instructions[i - 1].GetLdcI4Value()]);
                    methodDef.Body.Instructions[i - 1].OpCode =   OpCodes.Nop;
                    logger.Log($"Decrypted string ({instruction.Operand}) [Token: 0x{methodDef.MDToken.Raw:X8}]");
                }
            }
        }
    }
}