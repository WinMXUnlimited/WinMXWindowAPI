using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinMXWindowApi.Tools;

namespace WinMXWindowApi.Api
{
    internal class MXMonitor
    {
        public IntPtr GetMXMonitorWindow()
        {
            var topwindow = Imports.FindWindowEx(IntPtr.Zero, IntPtr.Zero, null, null);

            while (topwindow != IntPtr.Zero)
            {
                var title = new StringBuilder(100);
                Imports.GetWindowText(topwindow, title, 100);

                if (title.ToString().StartsWith("MX Monitor 1"))
                {
                    return topwindow;
                }

                topwindow = Imports.FindWindowEx(IntPtr.Zero, topwindow, null, null);
            }

            return IntPtr.Zero;
        }
    }
}
