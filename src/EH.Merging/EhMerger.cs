using ILRepacking;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
namespace EH.Merging;
public class EhMerger
{
    public static void Main(string[] args)
    {
        try
        {
            const string NAME           = "eh";
            Regex        excludePattern = new("^(assembly-csharp|ilrepack|dnlib|di|dk|bs).*|^" + Regex.Escape(NAME) + "$", RegexOptions.IgnoreCase);
            List<string> list = (from file in Directory.GetFiles(Directory.GetCurrentDirectory(), "*.dll", SearchOption.TopDirectoryOnly)
                where !excludePattern.IsMatch(Path.GetFileNameWithoutExtension(file)) select file).ToList<string>();
            if(File.Exists(NAME + ".dll")) File.Delete(NAME + ".dll");
            new ILRepack(new()
            {
                OutputFile      = NAME + ".dll",
                InputAssemblies = list.ToArray(),
                CopyAttributes  = true,
                DebugInfo       = true
            }).Repack();
            Console.WriteLine("assemblies merged successfully!");
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex);
            Console.ReadLine();
        }
    }
}