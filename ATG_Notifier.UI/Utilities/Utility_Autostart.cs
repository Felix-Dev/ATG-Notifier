using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

namespace ATG_Notifier.Utilities
{
    public enum Autostart
    {
        Disable,
        Enable
    }

    public class Utility_Autostart
    {
        private const string shortcutName = "ATG-Notifier";
        private const string shortcutDescription = "ATG-Notifier-Shortcut";

        public static void SetAutostart(Autostart setting)
        {
            var startupFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
            switch (setting)
            {
                case Autostart.Disable:
                    DeleteShortcut(startupFolderPath);
                    break;
                case Autostart.Enable:
                    CreateShortcut(startupFolderPath);
                    break;
            }
        }

        private static void DeleteShortcut(string path)
        {
            var scPath = Path.Combine(path, $"{shortcutName}.lnk");
            if (File.Exists(scPath))
            {
                File.Delete(scPath);
                return;
            }           
        }

        private static void CreateShortcut(string path)
        {
            IShellLink link = (IShellLink)new ShellLink();

            link.SetDescription(shortcutDescription);
            link.SetPath(Assembly.GetEntryAssembly().Location);

            // save it
            IPersistFile file = (IPersistFile)link;
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            file.Save(Path.Combine(path, $"{shortcutName}.lnk"), false);
        }

        [ComImport]
        [Guid("00021401-0000-0000-C000-000000000046")]
        internal class ShellLink
        {
        }

        [ComImport]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        [Guid("000214F9-0000-0000-C000-000000000046")]
        internal interface IShellLink
        {
            void GetPath([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszFile, int cchMaxPath, out IntPtr pfd, int fFlags);
            void GetIDList(out IntPtr ppidl);
            void SetIDList(IntPtr pidl);
            void GetDescription([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszName, int cchMaxName);
            void SetDescription([MarshalAs(UnmanagedType.LPWStr)] string pszName);
            void GetWorkingDirectory([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszDir, int cchMaxPath);
            void SetWorkingDirectory([MarshalAs(UnmanagedType.LPWStr)] string pszDir);
            void GetArguments([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszArgs, int cchMaxPath);
            void SetArguments([MarshalAs(UnmanagedType.LPWStr)] string pszArgs);
            void GetHotkey(out short pwHotkey);
            void SetHotkey(short wHotkey);
            void GetShowCmd(out int piShowCmd);
            void SetShowCmd(int iShowCmd);
            void GetIconLocation([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszIconPath, int cchIconPath, out int piIcon);
            void SetIconLocation([MarshalAs(UnmanagedType.LPWStr)] string pszIconPath, int iIcon);
            void SetRelativePath([MarshalAs(UnmanagedType.LPWStr)] string pszPathRel, int dwReserved);
            void Resolve(IntPtr hwnd, int fFlags);
            void SetPath([MarshalAs(UnmanagedType.LPWStr)] string pszFile);
        }
    }
}
