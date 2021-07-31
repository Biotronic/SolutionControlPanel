using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Volmax.ControlPanel.App.Utils
{
    /// <summary>
    /// Static class that returns the list of processes and the ports those processes use.
    /// </summary>
    public static class ProcessPorts
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
