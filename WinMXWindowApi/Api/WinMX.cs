using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinMXWindowApi.Api;
using WinMXWindowApi.Tools;

namespace WinMXWindowApi.Api
{
    internal class WinMX
    {
        public static IntPtr GetWinMXWindow()
        {
            var topwindow = Imports.FindWindowEx(IntPtr.Zero, IntPtr.Zero, null, null);

            while (topwindow != IntPtr.Zero)
            {
                var title = new StringBuilder(100);
                Imports.GetWindowText(topwindow, title, 100);

                if (title.ToString().StartsWith("WinMX v3."))
                {
                    return topwindow;
                }

                topwindow = Imports.FindWindowEx(IntPtr.Zero, topwindow, null, null);
            }

            return IntPtr.Zero;
        }

        public static IntPtr GetChannelListWindow()
        {
            var winmx = WinMX.GetWinMXWindow();
            if (winmx == IntPtr.Zero) return IntPtr.Zero;

            // Check for inner window
            var inner = Imports.FindWindowEx(winmx, IntPtr.Zero, null, null);
            while (inner != IntPtr.Zero)
            {
                var title = new StringBuilder(100);
                Imports.GetWindowText(inner, title, 100);
                if (title.ToString().Contains("WinMX Peer Network") && !title.ToString().Contains("on WinMX Peer Network"))
                {
                    return inner;
                }
                inner = Imports.FindWindowEx(winmx, inner, null, null);
            }


            // Check for floating window
            var channels = Imports.FindWindowEx(IntPtr.Zero, IntPtr.Zero, null, null);
            while (channels != IntPtr.Zero)
            {
                var Title = new StringBuilder(100);
                Imports.GetWindowText(channels, Title, 100);

                if (Title.ToString() == "WinMX Peer Network")
                {
                    return channels;
                }
                channels = Imports.FindWindowEx(IntPtr.Zero, channels, null, null);
            }

            // Failed
            return IntPtr.Zero;
        }

        public static List<ChatRoom> GetOpenChatRooms()
        {
            var results = new List<ChatRoom>();
            var winmx = WinMX.GetWinMXWindow();
            if (winmx == IntPtr.Zero) return results;

            // Check for inner chat windows.
            var inner = Imports.FindWindowEx(winmx, IntPtr.Zero, null, null);
            while (inner != IntPtr.Zero)
            {
                var title = new StringBuilder(100);
                Imports.GetWindowText(inner, title, 100);
                if (title.ToString().Contains("on WinMX Peer Network"))
                {
                    results.Add(new ChatRoom() { RoomName = title.ToString().Replace(" on WinMX Peer Network", ""), Window = inner });
                }
                inner = Imports.FindWindowEx(winmx, inner, null, null);
            }

            // Check for floating chat windows.
            var top = Imports.FindWindowEx(IntPtr.Zero, IntPtr.Zero, null, null);
            while (top != IntPtr.Zero)
            {
                var title = new StringBuilder(100);
                Imports.GetWindowText(top, title, 100);

                if (title.ToString().Contains("on WinMX Peer Network"))
                {
                    results.Add(new ChatRoom() { RoomName = title.ToString().Replace(" on WinMX Peer Network", ""), Window = top });
                }
                top = Imports.FindWindowEx(IntPtr.Zero, top, null, null);
            }

            return results;
        }

        public static void JoinChatRoom(string channelname)
        {
            var channellist = GetChannelListWindow();
            if (channellist == IntPtr.Zero) return;

            var filterbar = Imports.FindWindowEx(channellist, IntPtr.Zero, "edit", null);

            Imports.SendMessage(filterbar, Imports.WM_SETTEXT, new IntPtr(channelname.Length), new StringBuilder(channelname));
            Imports.PostMessage(filterbar, Imports.WM_KEYDOWN, new IntPtr(Imports.VK_RETURN), IntPtr.Zero);
        }

        public static void SendTextToChatRoom(string channelname, string text)
        {
            var room = GetOpenChatRooms().FirstOrDefault(c => c.RoomName == channelname);
            if (room == null) return;

            var input = Imports.FindWindowEx(room.Window, IntPtr.Zero, "edit", null);
            Imports.SendMessage(input, Imports.WM_SETTEXT, new IntPtr(text.Length), new StringBuilder(text));
            Imports.PostMessage(input, Imports.WM_KEYDOWN, new IntPtr(Imports.VK_RETURN), IntPtr.Zero);
        }

        internal class ChatRoom
        {
            public string RoomName { get; set; }
            public IntPtr Window { get; set; }
        }

    }
}
