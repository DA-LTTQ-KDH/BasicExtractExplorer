using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BasicExtractExplorer
{
    public class IconHelper
    {
        static SHFILEINFO shinfo = new SHFILEINFO();
        public static Icon GetIcon(string path)
        {
            IntPtr hImgSmall = Win32.SHGetFileInfo(path, 0, ref shinfo,
                        (uint)Marshal.SizeOf(shinfo),
                       Win32.SHGFI_ICON | Win32.SHGFI_LARGEICON);
            return Icon.FromHandle(shinfo.hIcon);
        }
        public static string GetFileTypeDescription(string fileName)
        {
            if (IntPtr.Zero != Win32.SHGetFileInfo(fileName, 0, ref shinfo, (uint)Marshal.SizeOf(shinfo), Win32.SHGFI_TYPENAME))
            {
                return Convert.ToString(shinfo.szTypeName.Trim());
            }
            return null;
        }
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct SHFILEINFO
        {
            public IntPtr hIcon;
            public int iIcon;
            public uint dwAttributes;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string szDisplayName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
            public string szTypeName;
        };
        class Win32
        {
            public const uint SHGFI_ICON = 0x100;
            public const uint SHGFI_LARGEICON = 0x0;    // 'Large icon
            public const uint SHGFI_SMALLICON = 0x1;    // 'Small icon
            public const uint FILE_ATTRIBUTE_NORMAL = 0x00000080;
            public const uint SHGFI_USEFILEATTRIBUTES = 0x000000010;
            public const uint SHGFI_TYPENAME = 0x000000400;

            [DllImport("shell32.dll",CharSet = CharSet.Auto)]
            public static extern IntPtr SHGetFileInfo(string pszPath,
                                        uint dwFileAttributes,
                                        ref SHFILEINFO psfi,
                                        uint cbSizeFileInfo,
                                        uint uFlags);
        }
    }

    
}
