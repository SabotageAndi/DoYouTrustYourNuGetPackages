using System.Diagnostics;
using System.Runtime.CompilerServices;
class C
{
    [ModuleInitializer]
    internal static void M1()
    {
        Process.Start(@"c:\windows\system32\notepad.exe");
    }
}