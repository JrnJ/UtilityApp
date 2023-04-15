using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace UtilityApp.Classes {
    internal static class ProgramLauncher {
        public static void RunApplication(string path) {
            Process.Start(path);
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
