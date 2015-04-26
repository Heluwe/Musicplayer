using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace XiMusic
{
    public class NativeMethods
    {
        #region API Import引用

        /// <summary>
        /// 移动无边框窗体声明引用
        /// </summary>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("user32.dll")]
        public static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);

        //移动窗体
        public static int WM_SYSCOMMAND = 0x0112;
        public static int SC_MOVE = 0xF010;
        public static int HTCAPTION = 0x0002;


        public static byte AC_SRC_OVER = 0;
        public static Int32 ULW_ALPHA = 2;
        public static byte AC_SRC_ALPHA = 1;

        /// <summary>
        /// 对窗口进行层次化处理
        /// </summary>
        /// <param name="hWnd">窗口句柄</param>
        /// <param name="hdcDst">目标窗口的屏幕上下文句柄</param>
        /// <param name="pptDst">目标位置</param>
        /// <param name="psize">目标尺寸</param>
        /// <param name="hdcSrc">内存句柄源</param>
        /// <param name="pprSrc">源位置</param>
        /// <param name="crKey">混合标志</param>
        /// <param name="pblend">合成方式</param>
        /// <param name="dwFlags">透明标志</param>
        /// <returns>层次化窗口是否成功</returns>
        [DllImport("user32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern int UpdateLayeredWindow(IntPtr hWnd, IntPtr hdcDst, ref Point pptDst, ref Size psize, IntPtr hdcSrc, ref Point pprSrc, int crKey, ref BLENDFUNCTION pblend, int dwFlags);

        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern IntPtr CreateCompatibleDC(IntPtr hDC);

        [DllImport("user32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern IntPtr GetDC(IntPtr hWnd);

        [DllImport("gdi32.dll", ExactSpelling = true)]
        public static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObj);

        [DllImport("user32.dll", ExactSpelling = true)]
        public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern int DeleteDC(IntPtr hDC);

        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern int DeleteObject(IntPtr hObj);

        #endregion

        [StructLayout(LayoutKind.Sequential)]
        public struct Size
        {
            public Int32 cx;
            public Int32 cy;

            public Size(Int32 x, Int32 y)
            {
                cx = x;
                cy = y;
            }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct BLENDFUNCTION
        {
            public byte BlendOp;
            public byte BlendFlags;
            public byte SourceConstantAlpha;
            public byte AlphaFormat;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Point
        {
            public Int32 x;
            public Int32 y;

            public Point(Int32 x, Int32 y)
            {
                this.x = x;
                this.y = y;
            }
        }

        protected delegate void InvokeCallback(Bitmap bitmap, Form form);

        protected InvokeCallback InvokeCallbackmsgCallback;

        /// <summary>
        /// 调用UpdateLayeredWindow函数实现透明
        /// </summary>
        /// <param name="bitmap">位图</param>
        public void UpdateLayeredWindow(Bitmap bitmap, Form form)
        {
            if (form.InvokeRequired)
            {
                InvokeCallbackmsgCallback = new InvokeCallback(UpdateLayeredWindow);
                form.Invoke(InvokeCallbackmsgCallback, new object[] { bitmap, form });
            }
            else
            {
                if (!Bitmap.IsCanonicalPixelFormat(bitmap.PixelFormat) || !Bitmap.IsAlphaPixelFormat(bitmap.PixelFormat))
                    throw new ApplicationException("图片必须是32位带Alhpa通道的图片。");

                IntPtr oldBits = IntPtr.Zero;
                IntPtr screenDC = NativeMethods.GetDC(IntPtr.Zero);
                IntPtr hBitmap = IntPtr.Zero;
                IntPtr memDc = NativeMethods.CreateCompatibleDC(screenDC);

                try
                {
                    NativeMethods.Point topLoc = new NativeMethods.Point(form.Left, form.Top);
                    NativeMethods.Size bitMapSize = new NativeMethods.Size(bitmap.Width, bitmap.Height);
                    NativeMethods.BLENDFUNCTION blendFunc = new NativeMethods.BLENDFUNCTION();
                    NativeMethods.Point srcLoc = new NativeMethods.Point(0, 0);

                    hBitmap = bitmap.GetHbitmap(Color.FromArgb(0));// 创建 GDI 位图对象
                    oldBits = NativeMethods.SelectObject(memDc, hBitmap);

                    blendFunc.BlendOp = NativeMethods.AC_SRC_OVER;
                    blendFunc.SourceConstantAlpha = 255;
                    blendFunc.AlphaFormat = NativeMethods.AC_SRC_ALPHA;
                    blendFunc.BlendFlags = 0;

                    NativeMethods.UpdateLayeredWindow(form.Handle, screenDC, ref topLoc, ref bitMapSize, memDc, ref srcLoc, 0, ref blendFunc, NativeMethods.ULW_ALPHA);
                }
                finally
                {
                    if (hBitmap != IntPtr.Zero)
                    {
                        NativeMethods.SelectObject(memDc, oldBits);
                        NativeMethods.DeleteObject(hBitmap);
                    }
                    NativeMethods.ReleaseDC(IntPtr.Zero, screenDC);
                    NativeMethods.DeleteDC(memDc);
                }
            }
        }
    }
}

