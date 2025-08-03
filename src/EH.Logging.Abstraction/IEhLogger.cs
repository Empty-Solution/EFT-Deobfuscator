namespace EH.Logging.Abstraction;
public interface IEhLogger
{
    bool Log(string message, char c = '~');
    bool Log(object value, char c = '~');
}