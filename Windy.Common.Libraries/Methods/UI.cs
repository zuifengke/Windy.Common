// ***********************************************************
// ��װһЩ������UI������������
// Creator:YangMingkun  Date:2009-6-22
// Copyright:supconhealth
// ***********************************************************
using System;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace Windy.Common.Libraries
{
    public partial struct GlobalMethods
    {
        /// <summary>
        /// ��װ������UI����
        /// </summary>
        public struct UI
        {
            /// <summary>
            /// ���ù����״
            /// </summary>
            /// <param name="cursor">�µĹ����״</param>
            public static void SetCursor(Control control, Cursor cursor)
            {
                if (control == null || control.IsDisposed)
                    return;
                if (cursor != null && control.Cursor != cursor)
                    control.Cursor = cursor;
            }

            /// <summary>
            /// ����ָ���ؼ������뷨״̬Ϊ���
            /// </summary>
            /// <param name="control">�ؼ�</param>
            public static void SetImeAsHalf(Control control)
            {
                if (control == null || control.IsDisposed)
                    return;
                IntPtr hControl = control.Handle;
                IntPtr hIME = NativeMethods.Ime32.ImmGetContext(hControl);
                if (!NativeMethods.Ime32.ImmGetOpenStatus(hIME))
                    return;

                int nMode = 0;
                int nSentence = 0;
                if (!NativeMethods.Ime32.ImmGetConversionStatus(hIME, ref nMode, ref nSentence))
                    return;

                //�����ȫ��ת���ɰ��
                if ((nMode & NativeConstants.IME_CMODE_FULLSHAPE) > 0)
                    NativeMethods.Ime32.ImmSimulateHotKey(hControl, NativeConstants.IME_CHOTKEY_SHAPE_TOGGLE);
            }

            #region"����UpDown�ؼ���ֵ"
            /// <summary>
            /// ����UpDown�ؼ���ֵ
            /// </summary>
            /// <param name="updownControl">UpDown�ؼ�</param>
            /// <param name="value">�����õ�ֵ</param>
            public static void SetUpdownValue(NumericUpDown updownControl, decimal value)
            {
                if (updownControl == null)
                    return;
                if (value > updownControl.Maximum)
                    updownControl.Value = updownControl.Maximum;
                else if (value < updownControl.Minimum)
                    updownControl.Value = updownControl.Minimum;
                else
                    updownControl.Value = value;
            }
            /// <summary>
            /// ����UpDown�ؼ���ֵ
            /// </summary>
            /// <param name="updownControl">UpDown�ؼ�</param>
            /// <param name="value">�����õ�ֵ</param>
            public static void SetUpdownValue(NumericUpDown updownControl, float value)
            {
                SetUpdownValue(updownControl, (decimal)value);
            }
            /// <summary>
            /// ����UpDown�ؼ���ֵ
            /// </summary>
            /// <param name="updownControl">UpDown�ؼ�</param>
            /// <param name="value">�����õ�ֵ</param>
            public static void SetUpdownValue(NumericUpDown updownControl, int value)
            {
                SetUpdownValue(updownControl, (decimal)value);
            }
            /// <summary>
            /// ����UpDown�ؼ���ֵ
            /// </summary>
            /// <param name="updownControl">UpDown�ؼ�</param>
            /// <param name="value">�����õ�ֵ</param>
            public static void SetUpdownValue(NumericUpDown updownControl, short value)
            {
                SetUpdownValue(updownControl, (decimal)value);
            }
            #endregion

            /// <summary>
            /// ��ȡ������Ļ�ܵ���ʾ����
            /// </summary>
            /// <returns>Rectangle</returns>
            public static Rectangle GetScreenBounds()
            {
                Rectangle bounds = Rectangle.Empty;
                foreach (Screen screen in Screen.AllScreens)
                {
                    Rectangle rectScreen = screen.WorkingArea;
                    if (rectScreen.Left < bounds.Left)
                    {
                        bounds.Width += (bounds.Left - rectScreen.Left);
                        bounds.X = rectScreen.X;
                    }
                    if (rectScreen.Right > bounds.Right)
                        bounds.Width += (rectScreen.Right - bounds.Right);
                    if (rectScreen.Top < bounds.Top)
                    {
                        bounds.Height += (bounds.Top - rectScreen.Top);
                        bounds.Y = rectScreen.Y;
                    }
                    if (rectScreen.Bottom > bounds.Bottom)
                        bounds.Height += (rectScreen.Bottom - bounds.Bottom);
                }
                return bounds;
            }

            /// <summary>
            /// ��ȡָ����������Ȧ���ľ�������
            /// </summary>
            /// <param name="x1">x1����</param>
            /// <param name="y1">y1����</param>
            /// <param name="x2">x2����</param>
            /// <param name="y2">y2����</param>
            /// <returns>��������</returns>
            public static RectangleF GetRectangle(float x1, float y1, float x2, float y2)
            {
                RectangleF rect = RectangleF.Empty;
                rect.X = Math.Min(x1, x2);
                rect.Y = Math.Min(y1, y2);
                rect.Width = Math.Abs(Math.Max(x1, x2) - rect.X);
                rect.Height = Math.Abs(Math.Max(y1, y2) - rect.Y);
                return rect;
            }

            /// <summary>
            /// ��ȡָ����������Ȧ���ľ�������
            /// </summary>
            /// <param name="x1">x1����</param>
            /// <param name="y1">y1����</param>
            /// <param name="x2">x2����</param>
            /// <param name="y2">y2����</param>
            /// <returns>��������</returns>
            public static Rectangle GetRectangle(int x1, int y1, int x2, int y2)
            {
                Rectangle rect = Rectangle.Empty;
                rect.X = Math.Min(x1, x2);
                rect.Y = Math.Min(y1, y2);
                rect.Width = Math.Abs(Math.Max(x1, x2) - rect.X);
                rect.Height = Math.Abs(Math.Max(y1, y2) - rect.Y);
                return rect;
            }

            /// <summary>
            /// ��λ���ڵ���Ļ����
            /// </summary>
            /// <param name="form">����</param>
            public static void LocateScreenCenter(Form form)
            {
                GlobalMethods.UI.LocateScreenCenter(form, Size.Empty);
            }

            /// <summary>
            /// ��λ���ڵ���Ļ����,����ָ���Ĵ�С��ʾ
            /// </summary>
            /// <param name="form">����</param>
            /// <param name="size">�µĴ�С</param>
            public static void LocateScreenCenter(Form form, Size size)
            {
                if (form == null || form.IsDisposed)
                    return;

                Screen primaryScreen = null;
                if (form.Owner != null && !form.Owner.IsDisposed)
                    primaryScreen = Screen.FromControl(form.Owner);
                if (primaryScreen == null)
                    primaryScreen = Screen.PrimaryScreen;
                if (primaryScreen == null)
                    return;

                Rectangle rect = primaryScreen.WorkingArea;

                int width = form.Width;
                int height = form.Height;
                if (!size.IsEmpty)
                {
                    width = (size.Width > rect.Width) ? rect.Width : size.Width;
                    height = (size.Height > rect.Height) ? rect.Height : size.Height;
                }
                int x = (rect.Width - width) / 2;
                int y = (rect.Height - height) / 2;
                form.SetBounds(x, y, width, height, BoundsSpecified.All);
            }

            /// <summary>
            /// ��ȡ��ǰ�ؼ������Parent�ؼ�
            /// </summary>
            /// <returns>Control</returns>
            public static Control GetTopLevelParent(Control control)
            {
                if (control == null || control.IsDisposed)
                    return null;
                Control parent = control;
                while (parent.Parent != null && !parent.Parent.IsDisposed)
                    parent = parent.Parent;
                return parent;
            }

            /// <summary>
            /// ��ȡ��ǰ���ھ�������Parent���
            /// </summary>
            /// <returns>Control</returns>
            public static IntPtr GetTopLevelParent(IntPtr hChildHandle)
            {
                if (!NativeMethods.User32.IsWindow(hChildHandle))
                    return IntPtr.Zero;
                while (NativeMethods.User32.GetParent(hChildHandle) != IntPtr.Zero)
                    hChildHandle = NativeMethods.User32.GetParent(hChildHandle);
                return hChildHandle;
            }

            /// <summary>
            /// ����ָ������Ĵ���
            /// </summary>
            /// <param name="hWndHandle"></param>
            public static void ActivateWindow(IntPtr hWndHandle)
            {
                if (!NativeMethods.User32.IsWindow(hWndHandle))
                    return;
                if (NativeMethods.User32.IsIconic(hWndHandle))
                    NativeMethods.User32.ShowWindow(hWndHandle, NativeConstants.SW_RESTORE);
                NativeMethods.User32.SetForegroundWindow(hWndHandle);
                NativeMethods.User32.SetActiveWindow(hWndHandle);
            }

            /// <summary>
            /// ����ָ�����ڱ������ϵĹرղ���
            /// </summary>
            /// <param name="form">����</param>
            public static void DisableCloseButton(Form form)
            {
                if (form == null || form.IsDisposed)
                    return;
                IntPtr hMenu = NativeMethods.User32.GetSystemMenu(form.Handle, 0);
                NativeMethods.User32.RemoveMenu(hMenu, NativeConstants.SC_CLOSE, NativeConstants.MF_REMOVE);
            }

            /// <summary>
            /// ��ȡһ��ָ����С��Բ�Ǿ�������
            /// </summary>
            /// <param name="rect">�����С</param>
            /// <param name="radius">�뾶</param>
            /// <returns>GraphicsPath</returns>
            public static GraphicsPath GetRoundPath(Rectangle rect, int radius, bool arc)
            {
                GraphicsPath graphicsPath = new GraphicsPath();
                if (rect.Width <= 0 || rect.Height <= 0 || radius <= 0)
                    return graphicsPath;

                //���Ͻ�
                if (arc)
                    graphicsPath.AddArc(new Rectangle(rect.X, rect.Y, radius * 2, radius * 2), 180, 90);
                graphicsPath.AddLine(rect.X + radius, rect.Y, rect.Right - radius, rect.Y);
                //���Ͻ�
                if (arc)
                    graphicsPath.AddArc(new Rectangle(rect.Right - radius * 2, rect.Y, radius * 2, radius * 2), 270, 90);
                graphicsPath.AddLine(rect.Right, rect.Y + radius, rect.Right, rect.Bottom - radius);
                //���½�
                if (arc)
                    graphicsPath.AddArc(new Rectangle(rect.Right - radius * 2, rect.Bottom - radius * 2, radius * 2, radius * 2), 0, 90);
                graphicsPath.AddLine(rect.Right - radius, rect.Bottom, rect.X + radius, rect.Bottom);
                //���½�
                if (arc)
                    graphicsPath.AddArc(new Rectangle(rect.X, rect.Bottom - radius * 2, radius * 2, radius * 2), 90, 90);
                graphicsPath.AddLine(rect.X, rect.Bottom - radius, rect.X, rect.Y + radius);
                graphicsPath.CloseAllFigures();
                return graphicsPath;
            }

            /// <summary>
            /// ��ȡһ��ָ����С��Բ�Ǿ�������
            /// </summary>
            /// <param name="rect">�����С</param>
            /// <param name="radius">�뾶</param>
            /// <returns>GraphicsPath</returns>
            public static GraphicsPath GetRoundPath(RectangleF rect, int radius, bool arc)
            {
                GraphicsPath graphicsPath = new GraphicsPath();
                if (rect.Width <= 0 || rect.Height <= 0 || radius <= 0)
                    return graphicsPath;

                //���Ͻ�
                if (arc)
                    graphicsPath.AddArc(new RectangleF(rect.X, rect.Y, radius * 2, radius * 2), 180, 90);
                graphicsPath.AddLine(rect.X + radius, rect.Y, rect.Right - radius, rect.Y);
                //���Ͻ�
                if (arc)
                    graphicsPath.AddArc(new RectangleF(rect.Right - radius * 2, rect.Y, radius * 2, radius * 2), 270, 90);
                graphicsPath.AddLine(rect.Right, rect.Y + radius, rect.Right, rect.Bottom - radius);
                //���½�
                if (arc)
                    graphicsPath.AddArc(new RectangleF(rect.Right - radius * 2, rect.Bottom - radius * 2, radius * 2, radius * 2), 0, 90);
                graphicsPath.AddLine(rect.Right - radius, rect.Bottom, rect.X + radius, rect.Bottom);
                //���½�
                if (arc)
                    graphicsPath.AddArc(new RectangleF(rect.X, rect.Bottom - radius * 2, radius * 2, radius * 2), 90, 90);
                graphicsPath.AddLine(rect.X, rect.Bottom - radius, rect.X, rect.Y + radius);
                graphicsPath.CloseAllFigures();
                return graphicsPath;
            }
        }
    }
}
