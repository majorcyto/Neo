using System;
using System.Runtime.InteropServices;

namespace WoWEditor6.IO.MPQ
{
    static class Imports
    {
#if WIN64
        [DllImport("StormDll.dll", EntryPoint = "?OpenArchive@Funcs@StormLibrary@@SA_NPEB_WKKPEAPEAX@Z")]
        public static extern bool SFileOpenArchive(IntPtr mpqName, uint priority, uint flags, out IntPtr handle);

        [DllImport("StormDll.dll", EntryPoint = "?HasFile@Funcs@StormLibrary@@SA_NPEAXPEBD@Z")]
        public static extern bool SFileHasFile(IntPtr mpq, string fileName);

        [DllImport("StormDll.dll", EntryPoint = "?OpenFileEx@Funcs@StormLibrary@@SA_NPEAXPEBDKPEAPEAX@Z")]
        public static extern bool SFileOpenFileEx(IntPtr mpq, string fileName, uint scope, out IntPtr handle);

        [DllImport("StormDll.dll", EntryPoint = "?ReadFile@Funcs@StormLibrary@@SA_NPEAX0KPEAKPEAU_OVERLAPPED@@@Z")]
        public static extern bool SFileReadFile(IntPtr file, byte[] buffer, int toRead, out int numRead, IntPtr lpOverlapped);

        [DllImport("StormDll.dll", EntryPoint = "?GetFileSize@Funcs@StormLibrary@@SAKPEAXPEAK@Z")]
        public static extern uint SFileGetFileSize(IntPtr file, out uint fileSizeHigh);

        [DllImport("StormDll.dll", EntryPoint = "?CloseFile@Funcs@StormLibrary@@SA_NPEAX@Z")]
        public static extern void SFileCloseFile(IntPtr file);
#else
        [DllImport("Stormlib.dll", CharSet = CharSet.Ansi)]
        public static extern bool SFileOpenArchive(string mpqName, uint priority, uint flags, out IntPtr handle);

        [DllImport("Stormlib.dll", CharSet = CharSet.Ansi)]
        public static extern bool SFileHasFile(IntPtr mpq, string fileName);

        [DllImport("Stormlib.dll", CharSet = CharSet.Ansi)]
        public static extern bool SFileOpenFileEx(IntPtr mpq, string fileName, uint scope, out IntPtr handle);

        [DllImport("Stormlib.dll", CharSet = CharSet.Ansi)]
        public static extern bool SFileReadFile(IntPtr file, byte[] buffer, int toRead, out int numRead, IntPtr lpOverlapped);

        [DllImport("Stormlib.dll", CharSet = CharSet.Ansi)]
        public static extern uint SFileGetFileSize(IntPtr file, out uint fileSizeHigh);

        [DllImport("Stormlib.dll", CharSet = CharSet.Ansi)]
        public static extern void SFileCloseFile(IntPtr file);
#endif
    }
}
