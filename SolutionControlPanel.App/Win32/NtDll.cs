using System;
using System.Runtime.InteropServices;
using SolutionControlPanel.App.Win32.Structures;

namespace SolutionControlPanel.App.Win32
{
    public static class NtDll
    {

        // for 32-bit process in a 64-bit OS only
        [DllImport("ntdll.dll")]
        public static extern int NtWow64QueryInformationProcess64(IntPtr processHandle, int processInformationClass, ref ProcessBasicInformationWow64 processInformation, int processInformationLength, IntPtr returnLength);

        [DllImport("ntdll.dll")]
        public static extern int NtWow64ReadVirtualMemory64(IntPtr hProcess, long lpBaseAddress, ref long lpBuffer, long dwSize, IntPtr lpNumberOfBytesRead);

        [DllImport("ntdll.dll")]
        public static extern int NtWow64ReadVirtualMemory64(IntPtr hProcess, long lpBaseAddress, ref UnicodeStringWow64 lpBuffer, long dwSize, IntPtr lpNumberOfBytesRead);

        [DllImport("ntdll.dll")]
        public static extern int NtWow64ReadVirtualMemory64(IntPtr hProcess, long lpBaseAddress, [MarshalAs(UnmanagedType.LPWStr)] string lpBuffer, long dwSize, IntPtr lpNumberOfBytesRead);

        [DllImport("ntdll.dll")]
        public static extern int NtQueryInformationProcess(IntPtr processHandle, int processInformationClass, ref ProcessBasicInformation processInformation, int processInformationLength, IntPtr returnLength);

        //ProcessWow64Information, // q: ULONG_PTR
        [DllImport("ntdll.dll")]
        public static extern int NtQueryInformationProcess(IntPtr processHandle, int processInformationClass, ref IntPtr processInformation, int processInformationLength, IntPtr returnLength);
    }
}