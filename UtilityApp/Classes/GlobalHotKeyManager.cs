using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;

namespace UtilityApp.Classes {
    public class GlobalHotKeyManager {
        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        private const int HOTKEY_ID = 9000;

        //Modifiers:
        private const uint MOD_SHIFT    = 0x0004;   // SHIFT
        private const uint MOD_WIN      = 0x0008;   // WINDOWS

        private Window _window;
        private IntPtr _windowHandle;
        private HwndSource _source;

        public bool IsHooking { get; private set; } = false;

        public GlobalHotKeyManager(Window window) {
            _window = window;

            _windowHandle = new WindowInteropHelper(window).EnsureHandle();
            _source = HwndSource.FromHwnd(_windowHandle);
        }

        public void Start() {
            if (!IsHooking) {
                _source.AddHook(HwndHook);
                RegisterHotKey(_windowHandle, HOTKEY_ID, (MOD_WIN | MOD_SHIFT), 0x5A); // WIN + SHIFT + B
                IsHooking = true;
            }
        }

        public void Stop() {
            if (IsHooking) {
                _source.RemoveHook(HwndHook);
                UnregisterHotKey(_windowHandle, HOTKEY_ID);
                IsHooking = false;
            }
        }

        private IntPtr HwndHook(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled) {
            const int WM_HOTKEY = 0x0312;
            switch (msg) {
                case WM_HOTKEY:
                    switch (wParam.ToInt32()) { 
                        case HOTKEY_ID:
                            int vkey = (((int)lParam >> 16) & 0xFFFF);
                            if (vkey == 0x5A) {
                                if (_window.IsVisible) {
                                    _window.Hide();
                                }
                                else {
                                    _window.Show();
                                    _window.Focus();
                                }
                            }
                            handled = true;
                            break;
                    }
                    break;
            }
            return IntPtr.Zero;
        }
    }
}
