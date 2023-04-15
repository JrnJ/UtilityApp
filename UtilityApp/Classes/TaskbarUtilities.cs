using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace UtilityApp.Classes {
    internal class TaskbarUtilities {
        private const int ABM_GETTASKBARPOS = 0x00000005;

        [StructLayout(LayoutKind.Sequential)]
        private struct APPBARDATA {
            public uint cbSize;
            public IntPtr hWnd;
            public uint uCallbackMessage;
            public uint uEdge;
            public RECT rc;
            public int lParam;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct RECT {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        [DllImport("shell32.dll")]
        private static extern IntPtr SHAppBarMessage(uint dwMessage, ref APPBARDATA pData);

        public static double GetTaskbarHeight() {
            var data = new APPBARDATA {
                cbSize = (uint)Marshal.SizeOf(typeof(APPBARDATA))
            };
            var result = SHAppBarMessage(ABM_GETTASKBARPOS, ref data);
            if (result == IntPtr.Zero) {
                throw new InvalidOperationException("Failed to get taskbar position.");
            }
            var rect = data.rc;
            return Math.Max(0, rect.bottom - rect.top);
        }
    }
}
