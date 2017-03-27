using System;
using System.Runtime.InteropServices;

namespace WoWEditor6.IO.MPQ
{
    static class Imports
    {
        #region imports
        [DllImport("StormDll.dll", EntryPoint = "?OpenArchive@Funcs@StormLibrary@@SA_NPEB_WKKPEAPEAX@Z")]
        private static extern bool SFileOpenArchive64(IntPtr mpqName, uint priority, uint flags, out IntPtr handle);

        [DllImport("StormDll.dll", EntryPoint = "?HasFile@Funcs@StormLibrary@@SA_NPEAXPEBD@Z")]
        private static extern bool SFileHasFile64(IntPtr mpq, string fileName);

        [DllImport("StormDll.dll", EntryPoint = "?OpenFileEx@Funcs@StormLibrary@@SA_NPEAXPEBDKPEAPEAX@Z")]
        private static extern bool SFileOpenFileEx64(IntPtr mpq, string fileName, uint scope, out IntPtr handle);

        [DllImport("StormDll.dll", EntryPoint = "?ReadFile@Funcs@StormLibrary@@SA_NPEAX0KPEAKPEAU_OVERLAPPED@@@Z")]
        private static extern bool SFileReadFile64(IntPtr file, byte[] buffer, int toRead, out int numRead, IntPtr lpOverlapped);

        [DllImport("StormDll.dll", EntryPoint = "?GetFileSize@Funcs@StormLibrary@@SAKPEAXPEAK@Z")]
        private static extern uint SFileGetFileSize64(IntPtr file, out uint fileSizeHigh);

        [DllImport("StormDll.dll", EntryPoint = "?CloseFile@Funcs@StormLibrary@@SA_NPEAX@Z")]
        private static extern void SFileCloseFile64(IntPtr file);

        [DllImport("StormLib.dll", EntryPoint = "SFileOpenArchive", CharSet = CharSet.Ansi)]
        private static extern bool SFileOpenArchive32(string mpqName, uint priority, uint flags, out IntPtr handle);

        [DllImport("StormLib.dll", EntryPoint = "SFileHasFile", CharSet = CharSet.Ansi)]
        private static extern bool SFileHasFile32(IntPtr mpq, string fileName);

        [DllImport("StormLib.dll", EntryPoint = "SFileOpenFileEx", CharSet = CharSet.Ansi)]
        private static extern bool SFileOpenFileEx32(IntPtr mpq, string fileName, uint scope, out IntPtr handle);

        [DllImport("StormLib.dll", EntryPoint = "SFileReadFile", CharSet = CharSet.Ansi)]
        private static extern bool SFileReadFile32(IntPtr file, byte[] buffer, int toRead, out int numRead, IntPtr lpOverlapped);

        [DllImport("StormLib.dll", EntryPoint = "SFileGetFileSize", CharSet = CharSet.Ansi)]
        private static extern uint SFileGetFileSize32(IntPtr file, out uint fileSizeHigh);

        [DllImport("StormLib.dll", EntryPoint = "SFileCloseFile", CharSet = CharSet.Ansi)]
        private static extern void SFileCloseFile32(IntPtr file);
        #endregion

        public static bool SFileOpenArchive(string fileName, uint priority, uint flags, out IntPtr handle)
        {
            if (Is64Bit())
            {
                var fixedFile = fileName.Replace(@"/", @"\").Replace(@"\", @"\\");
                var filePath = Marshal.StringToBSTR(fixedFile);

                return SFileOpenArchive64(filePath, priority, flags, out handle);
            }

            return SFileOpenArchive32(fileName, priority, flags, out handle);
        }

        public static bool SFileHasFile(IntPtr mpq, string fileName)
        {
            if (Is64Bit())
            {
                return SFileHasFile64(mpq, fileName);
            }

            return SFileHasFile32(mpq, fileName);
        }

        public static bool SFileOpenFileEx(IntPtr mpq, string fileName, uint scope, out IntPtr handle)
        {
            if (Is64Bit())
            {
                return SFileOpenFileEx64(mpq, fileName, scope, out handle);
            }

            return SFileOpenFileEx32(mpq, fileName, scope, out handle);
        }

        public static bool SFileReadFile(IntPtr file, byte[] buffer, int toRead, out int numRead, IntPtr lpOverlapped)
        {
            if (Is64Bit())
            {
                return SFileReadFile64(file, buffer, toRead, out numRead, lpOverlapped);
            }

            return SFileReadFile32(file, buffer, toRead, out numRead, lpOverlapped);
        }

        public static uint SFileGetFileSize(IntPtr file, out uint fileSizeHigh)
        {
            if (Is64Bit())
            {
                return SFileGetFileSize64(file, out fileSizeHigh);
            }

            return SFileGetFileSize32(file, out fileSizeHigh);
        }

        public static void SFileCloseFile(IntPtr file)
        {
            if (Is64Bit())
            {
                SFileCloseFile64(file);
            }
            else
            {
                SFileCloseFile32(file);
            }
        }

        private static bool Is64Bit()
        {
            return IntPtr.Size == 8;
        }
    }
}
