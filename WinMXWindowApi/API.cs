using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinMXWindowApi.Api;

namespace WinMXWindowApi
{
    public class API
    {
        /// <summary>
        /// Determines if WinMX is running.
        /// </summary>
        /// <returns></returns>
        public static bool WinMXIsOpen()
        {
            return WinMX.GetWinMXWindow() != IntPtr.Zero;
        }

        /// <summary>
        /// Determines if the WinMX Channel List window is open.
        /// </summary>
        /// <returns></returns>
        public static bool ChannelListOpen()
        {
            return WinMX.GetChannelListWindow() != IntPtr.Zero;
        }

        /// <summary>
        /// Gets a list of open WinMX chat rooms.
        /// </summary>
        /// <returns></returns>
        public static List<String> GetOpenChatRooms()
        {
            return WinMX.GetOpenChatRooms().Select(c => c.RoomName).ToList();
        }
            
        /// <summary>
        /// Attempts to join the given Chat Room.
        /// </summary>
        /// <param name="ChannelName"></param>
        public static void JoinChatRoom(string ChannelName)
        {
            WinMX.JoinChatRoom(ChannelName);
        }

        /// <summary>
        /// Sends the given text to the given Chat Room.
        /// </summary>
        /// <param name="ChannelName"></param>
        /// <param name="Text"></param>
        public static void SendTextToChatRoom(string ChannelName, string Text)
        {
            WinMX.SendTextToChatRoom(ChannelName, Text);
        }

        /// <summary>
        /// Gets the window handle for the main WinMX window.
        /// </summary>
        /// <returns></returns>
        public static IntPtr GetWinMXWindowHandle()
        {
            return WinMX.GetWinMXWindow();
        }
    }
}
