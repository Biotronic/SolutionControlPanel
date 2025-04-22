using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using SolutionControlPanel.App.Win32;
using SolutionControlPanel.App.Win32.Structures;

// ReSharper disable InconsistentNaming

namespace SolutionControlPanel.App.Utils
{
    public class UsedPort
    {
        public int ProcessId { get; set; }

        public string Protocol { get; set; }

        public int Port { get; set; }

        public override string ToString()
        {
            return $"{ProcessId}: {Protocol} {Port}";
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
        public static bool GetProcessIsWow64(IntPtr hProcess)
        {
            if ((Environment.OSVersion.Version.Major != 5 || Environment.OSVersion.Version.Minor < 1) &&
                Environment.OSVersion.Version.Major < 6) return false;

            return Kernel32.IsWow64Process(hProcess, out var retVal) && retVal;

        }

        public static bool InternalCheckIsWow64()
        {
            if ((Environment.OSVersion.Version.Major != 5 || Environment.OSVersion.Version.Minor < 1) &&
                Environment.OSVersion.Version.Major < 6) return false;

            using var p = Process.GetCurrentProcess();
            return Kernel32.IsWow64Process(p.Handle, out var retVal) && retVal;

        }
    }

    // All offset values below have been tested on Windows 7 & 8 only
    // but you can use WinDbg "dt ntdll!_PEB" command and search for ProcessParameters offset to find the truth, depending on the OS version
    public static class ProcessUtilities
    {
        public static readonly bool Is64BitProcess = IntPtr.Size > 4;
        public static readonly bool Is64BitOperatingSystem = Is64BitProcess || Is64BitChecker.InternalCheckIsWow64();

        public static string GetCurrentDirectory(int processId)
        {
            return GetProcessParametersString(processId);
        }

        private static string GetProcessParametersString(int processId)
        {
            var handle = Kernel32.OpenProcess(PROCESS_QUERY_INFORMATION | PROCESS_VM_READ, false, processId);
            if (handle == IntPtr.Zero)
                throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error());

            var isWow64Process = Is64BitChecker.InternalCheckIsWow64();
            var isTargetWow64Process = Is64BitChecker.GetProcessIsWow64(handle);
            var isTarget64BitProcess = Is64BitOperatingSystem && !isTargetWow64Process;

            long processParametersOffset = isTarget64BitProcess ? 0x20 : 0x10;
            long offset = isTarget64BitProcess ? 0x38 : 0x24;

            try
            {
                long pebAddress;
                if (isTargetWow64Process) // OS : 64Bit, Cur : 32 or 64, Tar: 32bit
                {
                    var peb32 = new IntPtr();

                    var hr = NtDll.NtQueryInformationProcess(handle, (int)PROCESSINFOCLASS.ProcessWow64Information, ref peb32, IntPtr.Size, IntPtr.Zero);
                    if (hr != 0) throw new System.ComponentModel.Win32Exception(hr);
                    pebAddress = peb32.ToInt64();

                    var pp = new IntPtr();
                    if (!Kernel32.ReadProcessMemory(handle, new IntPtr(pebAddress + processParametersOffset), ref pp, new IntPtr(Marshal.SizeOf(pp)), IntPtr.Zero))
                        throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error());

                    var us = new UnicodeString32();
                    if (!Kernel32.ReadProcessMemory(handle, new IntPtr(pp.ToInt64() + offset), ref us, new IntPtr(Marshal.SizeOf(us)), IntPtr.Zero))
                        throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error());

                    if ((us.Buffer == 0) || (us.Length == 0))
                        return null;

                    var s = new string('\0', us.Length / 2);
                    if (!Kernel32.ReadProcessMemory(handle, new IntPtr(us.Buffer), s, new IntPtr(us.Length), IntPtr.Zero))
                        throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error());

                    return s;
                }
                else if (isWow64Process)//Os : 64Bit, Cur 32, Tar 64
                {
                    var pbi = new ProcessBasicInformationWow64();
                    var hr = NtDll.NtWow64QueryInformationProcess64(handle, (int)PROCESSINFOCLASS.ProcessBasicInformation, ref pbi, Marshal.SizeOf(pbi), IntPtr.Zero);
                    if (hr != 0) throw new System.ComponentModel.Win32Exception(hr);
                    pebAddress = pbi.PebBaseAddress;

                    long pp = 0;
                    hr = NtDll.NtWow64ReadVirtualMemory64(handle, pebAddress + processParametersOffset, ref pp, Marshal.SizeOf(pp), IntPtr.Zero);
                    if (hr != 0)
                        throw new System.ComponentModel.Win32Exception(hr);

                    var us = new UnicodeStringWow64();
                    hr = NtDll.NtWow64ReadVirtualMemory64(handle, pp + offset, ref us, Marshal.SizeOf(us), IntPtr.Zero);
                    if (hr != 0)
                        throw new System.ComponentModel.Win32Exception(hr);

                    if ((us.Buffer == 0) || (us.Length == 0))
                        return null;

                    var s = new string('\0', us.Length / 2);
                    hr = NtDll.NtWow64ReadVirtualMemory64(handle, us.Buffer, s, us.Length, IntPtr.Zero);
                    if (hr != 0)
                        throw new System.ComponentModel.Win32Exception(hr);

                    return s;
                }
                else// Os,Cur,Tar : 64 or 32
                {
                    var pbi = new ProcessBasicInformation();
                    var hr = NtDll.NtQueryInformationProcess(handle, (int)PROCESSINFOCLASS.ProcessBasicInformation, ref pbi, Marshal.SizeOf(pbi), IntPtr.Zero);
                    if (hr != 0) throw new System.ComponentModel.Win32Exception(hr);
                    pebAddress = pbi.PebBaseAddress.ToInt64();

                    var pp = new IntPtr();
                    if (!Kernel32.ReadProcessMemory(handle, new IntPtr(pebAddress + processParametersOffset), ref pp, new IntPtr(Marshal.SizeOf(pp)), IntPtr.Zero))
                        throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error());

                    var us = new UnicodeString();
                    if (!Kernel32.ReadProcessMemory(handle, new IntPtr((long)pp + offset), ref us, new IntPtr(Marshal.SizeOf(us)), IntPtr.Zero))
                        throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error());

                    if ((us.Buffer == IntPtr.Zero) || (us.Length == 0))
                        return null;

                    var s = new string('\0', us.Length / 2);
                    if (!Kernel32.ReadProcessMemory(handle, us.Buffer, s, new IntPtr(us.Length), IntPtr.Zero))
                        throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error());

                    return s;
                }
            }
            finally
            {
                Kernel32.CloseHandle(handle);
            }
        }

        private const int PROCESS_QUERY_INFORMATION = 0x400;
        private const int PROCESS_VM_READ = 0x10;

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
            if (((now - _dataTime) ?? TimeSpan.MaxValue) < DataValidAge)
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

                var netStatRows = netStatContent.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var netStatRow in netStatRows)
                {
                    var tokens = netStatRow.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                    if (tokens.Length <= 4 || !tokens[1].Equals("UDP") && !tokens[1].Equals("TCP")) continue;

                    var ipAddress = Regex.Replace(tokens[2], @"\[(.*?)\]", "1.1.1.1");
                    try
                    {
                        Data.Add(new UsedPort
                        {
                            ProcessId = tokens[1] == "UDP" ? Convert.ToInt32(tokens[4]) : Convert.ToInt32(tokens[5]),
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
