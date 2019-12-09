using System;
using System.Runtime.InteropServices;

namespace FIS.AppClient.Utils
{
    public class Win32
    {
        public const int HWND_BROADCAST = 0xffff;
        public const int WM_COPYDATA = 0x004A;
    
        public struct COPYDATASTRUCT
        {
            public IntPtr m_DwData;
            public int m_CbData;
            [MarshalAs(UnmanagedType.LPStr)]
            public string m_LpData;
        }

        [DllImport("User32.dll", EntryPoint = "SendMessage")]
        public static extern int SendMessage(IntPtr hWnd, int msg, IntPtr wParam, ref COPYDATASTRUCT lParam);
    }
}