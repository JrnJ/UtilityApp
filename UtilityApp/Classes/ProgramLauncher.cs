using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace UtilityApp.Classes {
    internal static class ProgramLauncher {

        public static async Task<string> RunCmd(string command) {
            ProcessStartInfo processStartInfo = new("cmd", $"/c cd C:\\ && {command}") {
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };
            Process process = new() {
                StartInfo = processStartInfo
            };
            process.Start();
            string output = await process.StandardOutput.ReadToEndAsync();
            await process.WaitForExitAsync();

            // output = Environment.GetEnvironmentVariable("JAVA_HOME");
            return output;
        }

        public static void RunApplication(string path) {
            try {
                Process.Start(path);
            } catch {
                if (!path.Contains(".exe")) {
                    RunApplication(path + ".exe");
                }
            }
        }

        public static void RunApplication(ProcessStartInfo processStartInfo) {
            Process.Start(processStartInfo);
        }

        public static void RunBrowser(string? uri) {
            uri ??= "https://www.google.com/";

            if (!uri.Contains("://")) {
                uri = $"https://www.google.com/search?q={uri}";
            }

            uri = uri.Replace("&", "^&");
            Process.Start(new ProcessStartInfo(uri) { UseShellExecute = true });
        }
    }
}
