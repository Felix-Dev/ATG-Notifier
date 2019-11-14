using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;
using System.Net;
using System.Threading;
using HtmlAgilityPack;
using ATG_Notifier.Desktop.Native.Win32;
using System.Media;
using System.IO;

namespace ATG_Notifier.Desktop.Utilities
{
    public static class Utility
    {
        public static void PlaySound(Stream stream) 
        {
            using (var soundPlayer = new SoundPlayer(stream))
            {
                soundPlayer.Play();
            }
        }

        public static void DisplayTurnOn()
        {
            Input.INPUT input;

            input = new Input.INPUT
            {
                type = Input.InputType.MOUSE,
            };
            input.U.mi.dwFlags = Input.MOUSEEVENTF.MOVE;

            Input.INPUT[] inputs = new Input.INPUT[] { input };
            Input.SendInput(1, inputs, Input.INPUT.Size);
        }
    }
}
