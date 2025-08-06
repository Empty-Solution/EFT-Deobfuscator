# EH Deobfuscator

**EH Deobfuscator** is a .NET assembly deobfuscation tool written in C# using dnlib for IL code manipulation.

## 🚀 Features

- **String Decryption**: Automatically detect and decrypt obfuscated strings in .NET assemblies
- **Modular Architecture**: Plugin system for various deobfuscation processes
- **Dependency Injection**: DI container for managing dependencies
- **Logging**: Detailed logging of the deobfuscation process
- **Extensibility**: Easy addition of new deobfuscation processes

## 🛠 Technologies

- **.NET Framework 4.8**
- **C# 14.0**
- **dnlib** - for working with .NET metadata
- **Dependency Injection** - for application architecture

## 📁 Project Structure

```
src/
├── EH.Core/                              # Core application functionality
├── EH.DeobfuscationCore/                # Deobfuscation system core
├── EH.DeobfuscationCore.Abstraction/    # Abstractions for deobfuscation
├── EH.DeobfuscationProcesses/           # Concrete deobfuscation processes
├── EH.BaseDeobfuscationProcess/         # Base class for processes
├── EH.Logging/                          # Logging system
├── EH.Logging.Abstraction/              # Abstractions for logging
├── EH.StringValidation/                 # String validation
├── EH.StringValidation.Abstraction/     # Abstractions for validation
└── EH.Merging/                          # Assembly merging
```

## 🔧 Deobfuscation Processes

### String Decryption
- Automatically detects string decryption method calls
- Replaces encrypted strings with decrypted ones
- Logs all decryption operations with method tokens

## 🚀 Usage

1. Place the target assembly `Assembly-CSharp.dll` in the application directory
2. Run the deobfuscator
3. The system will automatically process the assembly, applying all available deobfuscation processes
4. Check the logs to monitor the deobfuscation process

## 📝 Logging

The system maintains detailed logs of all operations:
- Decrypted strings with their values
- Processed method tokens
- Errors and warnings during operation

## ⚙️ Architecture

The project is built on the principles of:
- **Dependency Inversion** - using abstractions
- **Single Responsibility** - each process performs one task
- **Extensibility** - easy addition of new deobfuscation processes
- **Configurability** - using attributes for DI configuration

## 🤝 Development

To add a new deobfuscation process:
1. Inherit from `EhBaseDeobfuscationProcess`
2. Override the `Deobfuscate` method
3. Add the `DiDescript` attribute for registration in the DI container

```csharp
[DiDescript(Order = 3, Lifetime = EDiServiceLifetime.Singleton, 
    ServiceType = typeof(IEhDeobfuscationProcess), Key = "YourProcess")]
public class YourDeobfuscationProcess : EhBaseDeobfuscationProcess
{
    protected override void Deobfuscate(ModuleDefMD module)
    {
        // Your deobfuscation logic here
    }
}
```

## 📄 License

See the [LICENSE](LICENSE) file for license rights and limitations.
