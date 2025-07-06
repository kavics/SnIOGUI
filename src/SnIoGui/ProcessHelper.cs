
using System;
using System.Diagnostics;
using System.Text;

namespace SnIoGui
{
    public static class ProcessHelper
    {
        /// <summary>
        /// Opens a file with the default associated application (cross-platform).
        /// </summary>
        /// <param name="filePath">The file to open.</param>
        /// <returns>True if the process was started successfully, false otherwise.</returns>
        public static bool OpenFileWithDefaultApp(string filePath)
        {
            try
            {
                var psi = new ProcessStartInfo
                {
                    FileName = filePath,
                    UseShellExecute = true
                };
                var process = Process.Start(psi);
                return process != null;
            }
            catch
            {
                return false;
            }
        }
        // Runs a PowerShell script file in a new window
        public static bool RunPowerShellScriptFileInWindow(string scriptFilePath, bool keepOpen = true)
        {
            string noExit = keepOpen ? "-NoExit " : string.Empty;
            var psi = new ProcessStartInfo
            {
                FileName = "powershell.exe",
                Arguments = $"{noExit}-ExecutionPolicy Bypass -File \"{scriptFilePath}\"",
                UseShellExecute = true,
                WindowStyle = ProcessWindowStyle.Normal
            };
            var process = Process.Start(psi);
            return process != null;
        }
        public static (int exitCode, string output, string error) RunPowerShellScript(string script)
        {
            var psi = new ProcessStartInfo
            {
                FileName = "powershell.exe",
                Arguments = "-NoProfile -ExecutionPolicy Bypass -Command -",
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (var process = new Process { StartInfo = psi })
            {
                var output = new StringBuilder();
                var error = new StringBuilder();
                process.OutputDataReceived += (s, e) => { if (e.Data != null) output.AppendLine(e.Data); };
                process.ErrorDataReceived += (s, e) => { if (e.Data != null) error.AppendLine(e.Data); };

                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
                process.StandardInput.WriteLine(script);
                process.StandardInput.Close();
                process.WaitForExit();
                return (process.ExitCode, output.ToString(), error.ToString());
            }
        }

        // Runs a PowerShell script in a new window, without saving to file
        public static bool RunPowerShellScriptInWindow(string script)
        {
            // Escape double quotes for PowerShell -Command
            string escapedScript = script.Replace("\"", "`\"");
            var psi = new ProcessStartInfo
            {
                FileName = "powershell.exe",
                Arguments = $"-NoExit -ExecutionPolicy Bypass -Command \"{escapedScript}\"",
                UseShellExecute = true,
                WindowStyle = ProcessWindowStyle.Normal
            };
            var process = Process.Start(psi);
            return process != null;
        }
    }
}
