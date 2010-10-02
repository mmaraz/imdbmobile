using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace ImdbMobile.UI
{
    class GDIPlus
    {
        public enum GpStatus
        {
            Ok = 0,
            GenericError = 1,
            InvalidParameter = 2,
            OutOfMemory = 3,
            ObjectBusy = 4,
            InsufficientBuffer = 5,
            NotImplemented = 6,
            Win32Error = 7,
            WrongState = 8,
            Aborted = 9,
            FileNotFound = 10,
            ValueOverflow = 11,
            AccessDenied = 12,
            UnknownImageFormat = 13,
            FontFamilyNotFound = 14,
            FontStyleNotFound = 15,
            NotTrueTypeFont = 16,
            UnsupportedGdiplusVersion = 17,
            GdiplusNotInitialized = 18,
            PropertyNotFound = 19,
            PropertyNotSupported = 20,
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct GpGraphics
        {
            public static implicit operator IntPtr(GpGraphics obj) { return obj.ptr; }
            IntPtr ptr;
        }

        public enum QualityMode
        {
            QualityModeInvalid = -1,
            QualityModeDefault = 0,
            QualityModeLow = 1, // Best performance
            QualityModeHigh = 2  // Best rendering quality
        };

        public enum SmoothingMode
        {
            SmoothingModeInvalid = QualityMode.QualityModeInvalid,
            SmoothingModeDefault = QualityMode.QualityModeDefault,
            SmoothingModeHighSpeed = QualityMode.QualityModeLow,
            SmoothingModeHighQuality = QualityMode.QualityModeHigh,
            SmoothingModeNone,
            SmoothingModeAntiAlias,
            //#if (GDIPVER >= 0x0110)
            //    SmoothingModeAntiAlias8x4 = SmoothingModeAntiAlias,
            //    SmoothingModeAntiAlias8x8
            //#endif //(GDIPVER >= 0x0110) 
        };

        [StructLayout(LayoutKind.Sequential)]
        public struct HDC
        {
            private HDC(IntPtr v) { val = v; }
            public IntPtr val;
            public static implicit operator IntPtr(HDC hdc) { return hdc.val; }
            public static implicit operator HDC(IntPtr hdc) { return new HDC(hdc); }
        }

        [DllImport("gdiplus")]
        extern static private GpStatus GdipSetSmoothingMode(GpGraphics graphics, SmoothingMode smoothingMode);

        [DllImport("gdiplus")]
        extern static private GpStatus GdipCreateFromHDC(HDC hdc, out GpGraphics graphics);

        public static void SetSmoothingMode(System.Drawing.Graphics g, SmoothingMode sm)
        {
            IntPtr gHdc = g.GetHdc();
            GDIPlus.GpGraphics Graphics = new GDIPlus.GpGraphics();
            GDIPlus.GdipCreateFromHDC(gHdc, out Graphics);

            GDIPlus.GdipSetSmoothingMode(Graphics, sm);
            g.ReleaseHdc(gHdc);
        }
    }
}
