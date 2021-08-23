using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace SolutionControlPanel.App.Win32
{
    public static class Ole32
    {
        [DllImport("ole32.dll")]
        public static extern int CreateBindCtx(int reserved, out IBindCtx ppbc);

        [DllImport("ole32.dll")]
        public static extern int GetRunningObjectTable(int reserved, out IRunningObjectTable prot);
    }
}