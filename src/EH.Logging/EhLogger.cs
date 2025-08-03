using DI.Services.Scheme.Abstraction;
using DI.Services.Scheme.Attributes;
using EH.Logging.Abstraction;
using System;
namespace EH.Logging;
[DiDescript(Order = -100, Lifetime = EDiServiceLifetime.Singleton, ServiceType = typeof(IEhLogger))]
public class EhLogger : IEhLogger
{
    public EhLogger() => Log("EhLogger initialized");
    public bool Log(string message, char c = '~')
    {
        Console.WriteLine($"[ ~ ] {message}");
        return true;
    }
    public bool Log(object value, char c = '~') => Log(value.ToString(), c);
}