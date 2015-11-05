// ***********************************************************
// ��װ��WINDOWS���ı��༭���йصķ�������
// Creator:YangMingkun  Date:2013-9-3
// Copyright:supconhealth
// ***********************************************************
using System;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Windy.Common.Libraries
{
    public partial struct GlobalMethods
    {
        /// <summary>
        /// ��װ��WINDOWS���ı��༭���йصķ���
        /// </summary>
        public struct RichEdit
        {
            /// <summary>
            /// WINDOWS���û��Զ�����Ϣ����ʼ����
            /// </summary>
            private const Int32 WM_USER = 0x400;

            #region"��ӡ"
            /// <summary>
            /// ���ı���ӡ��ҳ���С�ͱ߾����
            /// </summary>
            [StructLayout(LayoutKind.Sequential)]
            private struct STRUCT_RECT
            {
                public Int32 left;
                public Int32 top;
                public Int32 right;
                public Int32 bottom;
            }

            /// <summary>
            /// ���ı���ӡ����ʼ�ͽ����ַ�λ��
            /// </summary>
            [StructLayout(LayoutKind.Sequential)]
            private struct STRUCT_CHARRANGE
            {
                public Int32 cpMin;
                public Int32 cpMax;
            }

            /// <summary>
            /// ���ı���ӡʱ���͸�WinAPI�Ĳ����ṹ��
            /// </summary>
            [StructLayout(LayoutKind.Sequential)]
            private struct STRUCT_FORMATRANGE
            {
                public IntPtr hdc;
                public IntPtr hdcTarget;
                public STRUCT_RECT rc;
                public STRUCT_RECT rcPage;
                public STRUCT_CHARRANGE chrg;
            }

            /// <summary>
            /// WINDOWS�и��ı���ӡ����Ϣ����
            /// </summary>
            private const Int32 EM_FORMATRANGE = WM_USER + 57;

            /// <summary>
            /// ��ָ���ı����е����ݴ�ӡΪʸ��ͼԪ�����б�
            /// </summary>
            /// <param name="hRichEditHandle">�ı��༭�����</param>
            /// <param name="charFrom">��ӡ��ʼ�ַ�</param>
            /// <param name="charTo">��ӡ�����ַ�</param>
            /// <param name="size">ͼƬ��С</param>
            /// <returns>�޼����ŵ�ͼԪ�����б�</returns>
            public static Image[] Print(IntPtr hRichEditHandle
                , int charFrom, int charTo, Size size)
            {
                List<Image> images = new List<Image>();
                if (hRichEditHandle == IntPtr.Zero)
                    return images.ToArray();
                if (size.Width <= 24 || size.Height <= 24)
                    return images.ToArray();

                Rectangle rect = Rectangle.Empty;
                rect.Width = size.Width;
                rect.Height = size.Height;

                IntPtr hdc = NativeMethods.User32.GetDC(0);

                while (charFrom >= 0 && charFrom < charTo)
                {
                    Metafile wmf = new Metafile(hdc, rect
                        , MetafileFrameUnit.Pixel, EmfType.EmfPlusDual);
                    Graphics graphics = Graphics.FromImage(wmf);
                    charFrom = PrintRichTextRange(hRichEditHandle, graphics, charFrom, charTo, size);
                    graphics.Save();
                    graphics.Dispose();
                    images.Add(wmf);
                }
                NativeMethods.User32.ReleaseDC(0, hdc);
                return images.ToArray();
            }

            /// <summary>
            /// ��ָ���ı����е����ݴ�ӡΪʸ��ͼԪ�ļ������б�
            /// </summary>
            /// <param name="hRichEditHandle">�ı��༭�����</param>
            /// <param name="charFrom">��ӡ��ʼ�ַ�</param>
            /// <param name="charTo">��ӡ�����ַ�</param>
            /// <param name="size">ͼƬ��С</param>
            /// <returns>ʸ��ͼԪ�ļ��б��Ӧ���ֽ������б�</returns>
            public static List<byte[]> PrintToMetafile(IntPtr hRichEditHandle
                , int charFrom, int charTo, Size size)
            {
                Image[] images = Print(hRichEditHandle, charFrom, charTo, size);

                List<byte[]> lstImageData = new List<byte[]>();
                string metafile = GlobalMethods.Misc.GetWorkingPath() + "\\temp.wmf";

                IntPtr hdc = NativeMethods.User32.GetDC(0);
                foreach (Image image in images)
                {
                    Bitmap bmp = new Bitmap(image.Width, image.Height);
                    bmp.Save(metafile, System.Drawing.Imaging.ImageFormat.Emf);
                    bmp.Dispose();

                    Metafile wmf = new Metafile(metafile, hdc);
                    Graphics graphics = Graphics.FromImage(wmf);
                    graphics.DrawImage(image, 0, 0);
                    graphics.Dispose();
                    wmf.Dispose();

                    byte[] byteImageData = null;
                    GlobalMethods.IO.GetFileBytes(metafile, ref byteImageData);
                    if (byteImageData != null)
                        lstImageData.Add(byteImageData);

                    GlobalMethods.IO.DeleteFile(metafile);
                }
                NativeMethods.User32.ReleaseDC(0, hdc);
                return lstImageData;
            }

            /// <summary>
            /// ��ָ����Χ�ڵĸ��ı����ݴ�ӡ�����Graphics����������ͼ���豸��
            /// </summary>
            /// <param name="hTextBoxHandle">�ı�����</param>
            /// <param name="g">��ͼ����</param>
            /// <param name="charFrom">��ʼ�ַ�</param>
            /// <param name="charTo">�����ַ�</param>
            /// <param name="size">���������С</param>
            /// <returns>��ӡ����λ�õ��ַ�</returns>
            private static int PrintRichTextRange(IntPtr hRichEditHandle
                , Graphics g, int charFrom, int charTo, Size size)
            {
                IntPtr hdc = g.GetHdc();

                STRUCT_CHARRANGE cr;
                cr.cpMin = charFrom;
                cr.cpMax = charTo;

                STRUCT_RECT rc;
                rc.top = 0;
                rc.bottom = (int)(size.Height * 14.4f);
                rc.left = 0;
                rc.right = (int)(size.Width * 14.4f);

                STRUCT_RECT rcPage;
                rcPage.top = 0;
                rcPage.bottom = (int)(size.Height * 14.4f);
                rcPage.left = 0;
                rcPage.right = (int)(size.Width * 14.4f);

                STRUCT_FORMATRANGE fr;
                fr.chrg = cr;
                fr.hdc = hdc;
                fr.hdcTarget = hdc;
                fr.rc = rc;
                fr.rcPage = rcPage;

                IntPtr wParam = new IntPtr(1);
                IntPtr lParam = Marshal.AllocCoTaskMem(Marshal.SizeOf(fr));
                Marshal.StructureToPtr(fr, lParam, true);
                int result = NativeMethods.User32.SendMessage(hRichEditHandle, EM_FORMATRANGE, wParam, lParam);
                Marshal.FreeCoTaskMem(lParam);
                g.ReleaseHdc(hdc);
                return result;
            }
            #endregion

            #region "������ʽ"
            /// <summary>
            /// ��ȡ������ʽ��Ϣ����
            /// </summary>
            private const int EM_GETPARAFORMAT = WM_USER + 61;
            /// <summary>
            /// ���ö�����ʽ��Ϣ����
            /// </summary>
            private const int EM_SETPARAFORMAT = WM_USER + 71;
            private const long MAX_TAB_STOPS = 32;
            private const uint PFM_LINESPACING = 0x00000100;

            /// <summary>
            /// ������ʽ�ṹ��
            /// </summary>
            [StructLayout(LayoutKind.Sequential)]
            private struct PARAFORMAT2
            {
                public int cbSize;
                public uint dwMask;
                public short wNumbering;
                public short wReserved;
                public int dxStartIndent;
                public int dxRightIndent;
                public int dxOffset;
                public short wAlignment;
                public short cTabCount;
                [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
                public int[] rgxTabs;
                public int dySpaceBefore;
                public int dySpaceAfter;
                public int dyLineSpacing;
                public short sStyle;
                public byte bLineSpacingRule;
                public byte bOutlineLevel;
                public short wShadingWeight;
                public short wShadingStyle;
                public short wNumberingStart;
                public short wNumberingStyle;
                public short wNumberingTab;
                public short wBorderSpace;
                public short wBorderWidth;
                public short wBorders;
            }

            /// <summary>
            /// ��ȡ��ǰ������ڶ����о�
            /// </summary>
            /// <param name="hRichEditHandle">���ı��༭�����</param>
            /// <returns>�����о�</returns>
            public static float GetLineSpacing(IntPtr hRichEditHandle)
            {
                PARAFORMAT2 fmt = new PARAFORMAT2();
                fmt.cbSize = Marshal.SizeOf(fmt);
                fmt.dwMask = PFM_LINESPACING;

                IntPtr lParam = Marshal.AllocCoTaskMem(fmt.cbSize);
                Marshal.StructureToPtr(fmt, lParam, true);
                NativeMethods.User32.SendMessage(hRichEditHandle, EM_GETPARAFORMAT, IntPtr.Zero, lParam);
                Marshal.FreeCoTaskMem(lParam);
                return fmt.dyLineSpacing / 100f;
            }

            /// <summary>
            /// ����ָ���ĸ��ı��༭����ǰ������ڶ����о�
            /// </summary>
            /// <param name="hRichEditHandle">���ı��༭�����</param>
            /// <param name="lineSpacing">�����о�</param>
            public static void SetLineSpacing(IntPtr hRichEditHandle, float lineSpacing)
            {
                PARAFORMAT2 fmt = new PARAFORMAT2();
                fmt.cbSize = Marshal.SizeOf(fmt);
                fmt.bLineSpacingRule = 4;
                fmt.dyLineSpacing = (int)(lineSpacing * 100);
                fmt.dwMask = PFM_LINESPACING;

                IntPtr lParam = Marshal.AllocCoTaskMem(fmt.cbSize);
                Marshal.StructureToPtr(fmt, lParam, true);
                NativeMethods.User32.SendMessage(hRichEditHandle, EM_SETPARAFORMAT, IntPtr.Zero, lParam);
                Marshal.FreeCoTaskMem(lParam);
            }
            #endregion
        }
    }
}
