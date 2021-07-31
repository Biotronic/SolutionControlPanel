using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

// ReSharper disable InconsistentNaming

namespace Volmax.ControlPanel.App.Utils
{
    public class UsedPort
    {
        public int ProcessId { get; set; }

        public string Protocol { get; set; }

        public int Port { get; set; }

        public override string ToString()
        {
            return $"{Protocol} {Port}";
        }
    }

    // ref: http://www.microsoft.com/whdc/system/Sysinternals/MoreThan64proc.mspx
    public enum PROCESSINFOCLASS
    {
        ProcessBasicInformation = 0, // 0, q: PROCESS_BASIC_INFORMATION, PROCESS_EXTENDED_BASIC_INFORMATION
        ProcessWow64Information = 26, // q: ULONG_PTR
    }
    [Flags]
    public enum PEB_OFFSET
    {
        CurrentDirectory,
        //DllPath,
        //ImagePathName,
        CommandLine,
        //WindowTitle,
        //DesktopInfo,
        //ShellInfo,
        //RuntimeData,
        //TypeMask = 0xffff,
        //Wow64 = 0x10000,
    };

    public class Is64BitChecker
    {
        [DllImport("kernel32.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool IsWow64Process(
            [In] IntPtr hProcess,
            [Out] out bool wow64Process
        );

        public static bool GetProcessIsWow64(IntPtr hProcess)
        {
            if ((Environment.OSVersion.Version.Major != 5 || Environment.OSVersion.Version.Minor < 1) &&
                Environment.OSVersion.Version.Major < 6) return false;

            return IsWow64Process(hProcess, out var retVal) && retVal;

        }

        public static bool InternalCheckIsWow64()
        {
            if ((Environment.OSVersion.Version.Major != 5 || Environment.OSVersion.Version.Minor < 1) &&
                Environment.OSVersion.Version.Major < 6) return false;

            using var p = Process.GetCurrentProcess();
            return IsWow64Process(p.Handle, out var retVal) && retVal;

        }
    }

    // All offset values below have been tested on Windows 7 & 8 only
    // but you can use WinDbg "dt ntdll!_PEB" command and search for ProcessParameters offset to find the truth, depending on the OS version
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Local")]
    public static class ProcessUtilities
    {
        public static readonly bool Is64BitProcess = IntPtr.Size > 4;
        public static readonly bool Is64BitOperatingSystem = Is64BitProcess || Is64BitChecker.InternalCheckIsWow64();

        public static string GetCurrentDirectory(int processId)
        {
            return GetProcessParametersString(processId, PEB_OFFSET.CurrentDirectory);
        }

        private static string GetProcessParametersString(int processId, PEB_OFFSET Offset)
        {
            var handle = OpenProcess(PROCESS_QUERY_INFORMATION | PROCESS_VM_READ, false, processId);
            if (handle == IntPtr.Zero)
                throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error());

            var isWow64Process = Is64BitChecker.InternalCheckIsWow64();
            var isTargetWow64Process = Is64BitChecker.GetProcessIsWow64(handle);
            var isTarget64BitProcess = Is64BitOperatingSystem && !isTargetWow64Process;

            long offset;
            long processParametersOffset = isTarget64BitProcess ? 0x20 : 0x10;
            switch (Offset)
            {
                case PEB_OFFSET.CurrentDirectory:
                    offset = isTarget64BitProcess ? 0x38 : 0x24;
                    break;
                case PEB_OFFSET.CommandLine:
                default:
                    return null;
            }

            try
            {
                long pebAddress;
                if (isTargetWow64Process) // OS : 64Bit, Cur : 32 or 64, Tar: 32bit
                {
                    var peb32 = new IntPtr();

                    var hr = NtQueryInformationProcess(handle, (int)PROCESSINFOCLASS.ProcessWow64Information, ref peb32, IntPtr.Size, IntPtr.Zero);
                    if (hr != 0) throw new System.ComponentModel.Win32Exception(hr);
                    pebAddress = peb32.ToInt64();

                    var pp = new IntPtr();
                    if (!ReadProcessMemory(handle, new IntPtr(pebAddress + processParametersOffset), ref pp, new IntPtr(Marshal.SizeOf(pp)), IntPtr.Zero))
                        throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error());

                    var us = new UNICODE_STRING_32();
                    if (!ReadProcessMemory(handle, new IntPtr(pp.ToInt64() + offset), ref us, new IntPtr(Marshal.SizeOf(us)), IntPtr.Zero))
                        throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error());

                    if ((us.Buffer == 0) || (us.Length == 0))
                        return null;

                    var s = new string('\0', us.Length / 2);
                    if (!ReadProcessMemory(handle, new IntPtr(us.Buffer), s, new IntPtr(us.Length), IntPtr.Zero))
                        throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error());

                    return s;
                }
                else if (isWow64Process)//Os : 64Bit, Cur 32, Tar 64
                {
                    var pbi = new PROCESS_BASIC_INFORMATION_WOW64();
                    var hr = NtWow64QueryInformationProcess64(handle, (int)PROCESSINFOCLASS.ProcessBasicInformation, ref pbi, Marshal.SizeOf(pbi), IntPtr.Zero);
                    if (hr != 0) throw new System.ComponentModel.Win32Exception(hr);
                    pebAddress = pbi.PebBaseAddress;

                    long pp = 0;
                    hr = NtWow64ReadVirtualMemory64(handle, pebAddress + processParametersOffset, ref pp, Marshal.SizeOf(pp), IntPtr.Zero);
                    if (hr != 0)
                        throw new System.ComponentModel.Win32Exception(hr);

                    var us = new UNICODE_STRING_WOW64();
                    hr = NtWow64ReadVirtualMemory64(handle, pp + offset, ref us, Marshal.SizeOf(us), IntPtr.Zero);
                    if (hr != 0)
                        throw new System.ComponentModel.Win32Exception(hr);

                    if ((us.Buffer == 0) || (us.Length == 0))
                        return null;

                    var s = new string('\0', us.Length / 2);
                    hr = NtWow64ReadVirtualMemory64(handle, us.Buffer, s, us.Length, IntPtr.Zero);
                    if (hr != 0)
                        throw new System.ComponentModel.Win32Exception(hr);

                    return s;
                }
                else// Os,Cur,Tar : 64 or 32
                {
                    var pbi = new PROCESS_BASIC_INFORMATION();
                    var hr = NtQueryInformationProcess(handle, (int)PROCESSINFOCLASS.ProcessBasicInformation, ref pbi, Marshal.SizeOf(pbi), IntPtr.Zero);
                    if (hr != 0) throw new System.ComponentModel.Win32Exception(hr);
                    pebAddress = pbi.PebBaseAddress.ToInt64();

                    var pp = new IntPtr();
                    if (!ReadProcessMemory(handle, new IntPtr(pebAddress + processParametersOffset), ref pp, new IntPtr(Marshal.SizeOf(pp)), IntPtr.Zero))
                        throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error());

                    var us = new UNICODE_STRING();
                    if (!ReadProcessMemory(handle, new IntPtr((long)pp + offset), ref us, new IntPtr(Marshal.SizeOf(us)), IntPtr.Zero))
                        throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error());

                    if ((us.Buffer == IntPtr.Zero) || (us.Length == 0))
                        return null;

                    var s = new string('\0', us.Length / 2);
                    if (!ReadProcessMemory(handle, us.Buffer, s, new IntPtr(us.Length), IntPtr.Zero))
                        throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error());

                    return s;
                }
            }
            finally
            {
                CloseHandle(handle);
            }
        }

        private const int PROCESS_QUERY_INFORMATION = 0x400;
        private const int PROCESS_VM_READ = 0x10;

        [StructLayout(LayoutKind.Sequential)]
        private readonly struct PROCESS_BASIC_INFORMATION
        {
            public readonly IntPtr Reserved1;
            public readonly IntPtr PebBaseAddress;
            public readonly IntPtr Reserved2_0;
            public readonly IntPtr Reserved2_1;
            public readonly IntPtr UniqueProcessId;
            public readonly IntPtr Reserved3;
        }

        [StructLayout(LayoutKind.Sequential)]
        private readonly struct UNICODE_STRING
        {
            public readonly short Length;
            public readonly short MaximumLength;
            public readonly IntPtr Buffer;
        }

        // for 32-bit process in a 64-bit OS only
        [StructLayout(LayoutKind.Sequential)]
        private readonly struct PROCESS_BASIC_INFORMATION_WOW64
        {
            public readonly long Reserved1;
            public readonly long PebBaseAddress;
            public readonly long Reserved2_0;
            public readonly long Reserved2_1;
            public readonly long UniqueProcessId;
            public readonly long Reserved3;
        }

        // for 32-bit process
        [StructLayout(LayoutKind.Sequential)]
        private readonly struct UNICODE_STRING_WOW64
        {
            public readonly short Length;
            public readonly short MaximumLength;
            public readonly long Buffer;
        }

        [StructLayout(LayoutKind.Sequential)]
        private readonly struct UNICODE_STRING_32
        {
            public readonly short Length;
            public readonly short MaximumLength;
            public readonly int Buffer;
        }

        [DllImport("ntdll.dll")]
        private static extern int NtQueryInformationProcess(IntPtr processHandle, int processInformationClass, ref PROCESS_BASIC_INFORMATION processInformation, int processInformationLength, IntPtr returnLength);

        //ProcessWow64Information, // q: ULONG_PTR
        [DllImport("ntdll.dll")]
        private static extern int NtQueryInformationProcess(IntPtr processHandle, int processInformationClass, ref IntPtr processInformation, int processInformationLength, IntPtr returnLength);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, ref IntPtr lpBuffer, IntPtr dwSize, IntPtr lpNumberOfBytesRead);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, ref UNICODE_STRING lpBuffer, IntPtr dwSize, IntPtr lpNumberOfBytesRead);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, ref UNICODE_STRING_32 lpBuffer, IntPtr dwSize, IntPtr lpNumberOfBytesRead);

        //[DllImport("kernel32.dll", SetLastError = true)]
        //private static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, ref UNICODE_STRING_WOW64 lpBuffer, IntPtr dwSize, IntPtr lpNumberOfBytesRead);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, [MarshalAs(UnmanagedType.LPWStr)] string lpBuffer, IntPtr dwSize, IntPtr lpNumberOfBytesRead);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll")]
        private static extern bool CloseHandle(IntPtr hObject);

        // for 32-bit process in a 64-bit OS only
        [DllImport("ntdll.dll")]
        private static extern int NtWow64QueryInformationProcess64(IntPtr processHandle, int processInformationClass, ref PROCESS_BASIC_INFORMATION_WOW64 processInformation, int processInformationLength, IntPtr returnLength);

        [DllImport("ntdll.dll")]
        private static extern int NtWow64ReadVirtualMemory64(IntPtr hProcess, long lpBaseAddress, ref long lpBuffer, long dwSize, IntPtr lpNumberOfBytesRead);

        [DllImport("ntdll.dll")]
        private static extern int NtWow64ReadVirtualMemory64(IntPtr hProcess, long lpBaseAddress, ref UNICODE_STRING_WOW64 lpBuffer, long dwSize, IntPtr lpNumberOfBytesRead);

        [DllImport("ntdll.dll")]
        private static extern int NtWow64ReadVirtualMemory64(IntPtr hProcess, long lpBaseAddress, [MarshalAs(UnmanagedType.LPWStr)] string lpBuffer, long dwSize, IntPtr lpNumberOfBytesRead);

        private static readonly TimeSpan DataValidAge = TimeSpan.FromMilliseconds(3000);
        private static readonly List<UsedPort> Data = new List<UsedPort>();
        private static DateTime? _dataTime;

        /// <summary>
        /// This method distills the output from netstat -a -n -o into a list of ProcessPorts that provide a mapping between
        /// the process id and the ports that the process is using.
        /// </summary>
        /// <returns></returns>
        public static IReadOnlyCollection<UsedPort> GetPortUsage()
        {
            var now = DateTime.UtcNow;
            if (((now - _dataTime) ?? TimeSpan.MaxValue) > DataValidAge)
            {
                return Data;
            }
            _dataTime = now;
            Data.Clear();

            try
            {
                using var proc = new Process();
                var startInfo = new ProcessStartInfo
                {
                    FileName = "netstat.exe",
                    Arguments = "-a -n -o",
                    WindowStyle = ProcessWindowStyle.Hidden,
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                };

                proc.StartInfo = startInfo;
                proc.Start();

                var standardOutput = proc.StandardOutput;
                var standardError = proc.StandardError;

                var netStatContent = standardOutput.ReadToEnd() + standardError.ReadToEnd();

                if (proc.ExitCode != 0)
                {
                    return Data;
                }

                var netStatRows = Regex.Split(netStatContent, "\r\n");

                foreach (var netStatRow in netStatRows)
                {
                    var tokens = Regex.Split(netStatRow, "\\s+");
                    if (tokens.Length <= 4 || !tokens[1].Equals("UDP") && !tokens[1].Equals("TCP")) continue;

                    var ipAddress = Regex.Replace(tokens[2], @"\[(.*?)\]", "1.1.1.1");
                    try
                    {
                        Data.Add(new UsedPort
                        {
                            ProcessId = tokens[1] == "UDP" ? Convert.ToInt16(tokens[4]) : Convert.ToInt16(tokens[5]),
                            Protocol = ipAddress.Contains("1.1.1.1") ? $"{tokens[1]}v6" : $"{tokens[1]}v4",
                            Port = Convert.ToInt32(ipAddress.Split(':')[1])
                        });
                    }
                    catch
                    {
                        //
                    }
                }
            }
            catch
            {
                //
            }
            return Data;
        }
    }
}
