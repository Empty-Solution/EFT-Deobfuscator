using DI.Services.Scheme.Abstraction;
using DI.Services.Scheme.Attributes;
using EH.StringValidation.Abstraction;
namespace EH.StringValidation;
[DiDescript(Order = 0, Lifetime = EDiServiceLifetime.Singleton, ServiceType = typeof(IEhStringValidator))]
public class EhUnicodeStringValidator : IEhStringValidator
{
    public bool Validate(string value)
    {
        if(string.IsNullOrEmpty(value)) return true;

        foreach(char c in value)
        {
            if(char.IsControl(c)) return false;
            if(IsPrivateUseArea(c)) return false;
            if(char.IsSurrogate(c)) return false;
            if(IsInvalidUnicodeRange(c)) return false;
        }
        return true;
    }
    private static bool IsPrivateUseArea(char c)
    {
        int code = c;
        if(code >= 0xE000 && code <= 0xF8FF) return true;

        return false;
    }
    private static bool IsInvalidUnicodeRange(char c)
    {
        int code = c;
        if(code >= 0xFDD0 && code <= 0xFDEF) return true;
        if((code & 0xFFFF) >= 0xFFFE) return true;

        return false;
    }
}