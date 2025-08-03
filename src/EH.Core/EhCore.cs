using BS.Core;
using BS.Core.Abstraction;
using System;
using System.IO;
using System.Reflection;
public class EhCore
{
    private static IBsCore? bootstrapCore;
    public static void Main(string[] args)
    {
        try
        {
            Assembly assembly = Assembly.Load(File.ReadAllBytes("eh.dll")); // im too lazy to write smth like normal load logic, so... 
            bootstrapCore = BsCore.Create(assembly);
            bootstrapCore.Initialize();
        }
        catch(Exception e)
        {
            Console.WriteLine(e);
            Console.ReadLine();
        }
    }
}