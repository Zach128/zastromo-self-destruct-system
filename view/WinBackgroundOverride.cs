using WinGraphicsController.libdll;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinGraphicsController.view
{
    /// <summary>
    /// A background graphics handler for editing the desktop background.
    /// The handler is able to alter a windows located between the desktop wallpaper and the desktop icons.
    /// Source and reference: https://www.codeproject.com/Articles/856020/Draw-Behind-Desktop-Icons-in-Windows
    /// </summary>
    class WinBackgroundOverride
    {

        /// <summary>
        /// The handle to the WorkerW instance located between the desktop and the wallpaper.
        /// </summary>
        private IntPtr workerw;

        public IntPtr WorkerW
        {
            get { return workerw; }
        }

        public WinBackgroundOverride() {
            // Fetch the Progman window
            IntPtr progman = User32.FindWindow("Progman", null);

            IntPtr result = IntPtr.Zero;
            // Send 0x052C to Progman. This message directs Progman to spawn a 
            // WorkerW behind the desktop icons. If it is already there, nothing 
            // happens.
            User32.SendMessageTimeout(progman,
                                   0x052C,
                                   new IntPtr(0),
                                   IntPtr.Zero,
                                   SendMessageTimeoutFlags.SMTO_NORMAL,
                                   1000,
                                   out result);

            workerw = IntPtr.Zero;

            // We enumerate all Windows, until we find one, that has the SHELLDLL_DefView 
            // as a child. 
            // If we found that window, we take its next sibling and assign it to workerw.
            User32.EnumWindows(new EnumWindowsProc((tophandle, topparamhandle) =>
            {
                IntPtr p = User32.FindWindowEx(tophandle,
                                            IntPtr.Zero,
                                            "SHELLDLL_DefView",
                                            String.Empty);

                if (p != IntPtr.Zero)
                {
                    // Gets the WorkerW Window after the current one.
                    workerw = User32.FindWindowEx(IntPtr.Zero,
                                               tophandle,
                                               "WorkerW",
                                               String.Empty);
                }

                return true;
            }), IntPtr.Zero);
        }

        /// <summary>
        /// Fill the WorkerW window with a specific color
        /// </summary>
        /// <param name="color">The color to fill the window with</param>
        public void FillWithColor(Color color)
        {
            UseWorkerWDC(new DeviceContextUsage((dc) =>
            {
                using (Graphics g = Graphics.FromHdc(dc))
                {
                    g.Clear(color);
                }
            }));
        }

        /// <summary>
        /// Demo function for rendering a basic hexagon and X through it.
        /// </summary>
        public void DrawWarningOnBkg()
        {
            UseWorkerWDC(new DeviceContextUsage(dc => {
                using (Graphics g = Graphics.FromHdc(dc))
                {
                    g.PageUnit = GraphicsUnit.Pixel;

                    ScreenGraphics sg = new ScreenGraphics(g);

                    PointF[] octPoints = new PointF[]
                    {
                        sg.NewPointRM(37.5f, 87.5f),
                        sg.NewPointRM(62.5f, 87.5f),
                        sg.NewPointRM(87.5f, 62.5f),
                        sg.NewPointRM(87.5f, 37.5f),
                        sg.NewPointRM(62.5f, 12.5f),
                        sg.NewPointRM(37.5f, 12.5f),
                        sg.NewPointRM(12.5f, 37.5f),
                        sg.NewPointRM(12.5f, 62.5f)
                    };

                    PointF x1Begin = sg.PointAlongLine(octPoints[0], octPoints[7]);
                    PointF x1End = sg.PointAlongLine(octPoints[3], octPoints[4]);
                    PointF x2Begin = sg.PointAlongLine(octPoints[1], octPoints[2]);
                    PointF x2End = sg.PointAlongLine(octPoints[5], octPoints[6]);

                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                    g.DrawLine(new Pen(Color.Orange, 10f), x1Begin, x1End);
                    g.DrawLine(new Pen(Color.Orange, 10f), x2Begin, x2End);

                    g.DrawPolygon(new Pen(Color.OrangeRed, 10f), octPoints);
                }
            }));
        }

        /// <summary>
        /// Clear the WorkerW window
        /// </summary>
        public void ClearBackground()
        {
            FillWithColor(Color.Transparent);
        }

        /// <summary>
        /// Basic wrapper function for simple use of the WorkerW device context.
        /// The pointer is automatically retrieved and handled by the function so the user
        /// focuses on what the dc needs to be used for.
        /// </summary>
        /// <param name="dcUse">The instructions that need to be executed by the user</param>
        public void UseWorkerWDC(DeviceContextUsage dcUse)
        {
            IntPtr dc = GetWorkerWDeviceContext();
            if (dc != IntPtr.Zero)
            {
                dcUse(dc);
                User32.ReleaseDC(workerw, dc);
            }
            else
            {
                throw new ContextMarshalException("The WorkerW device context was not found (0x0 returned)");
            }
        }

        public delegate void DeviceContextUsage(IntPtr dc);

        private IntPtr GetWorkerWDeviceContext()
        {
            // Get the Device Context of the WorkerW
            IntPtr dc = User32.GetDCEx(workerw, IntPtr.Zero, (DeviceContextValues)0x403);
            return dc;
        }
        
    }
}