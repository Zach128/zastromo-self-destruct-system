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

        public void DemoDrawRect(float x, float y, float width, float height)
        {
            UseWorkerWDC(new DeviceContextUsage((dc) =>
            {
                // Create a Graphics instance from the Device Context
                using (Graphics g = Graphics.FromHdc(dc))
                {
                    // Use the Graphics instance to draw a white rectangle in the upper 
                    // left corner. In case you have more than one monitor think of the 
                    // drawing area as a rectangle that spans across all monitors, and 
                    // the 0,0 coordinate being in the upper left corner.
                    g.FillRectangle(new SolidBrush(Color.White), 0, 0, 500, 500);
                    g.FillRectangle(new SolidBrush(Color.Coral), x, y, width, height);
                }
            }));
        }

        public IntPtr WorkerW
        {
            get { return workerw; }
        }

        /// <summary>
        /// Clear the WorkerW window
        /// </summary>
        public void ClearBackground()
        {
            UseWorkerWDC(new DeviceContextUsage((dc) =>
            {
                using (Graphics g = Graphics.FromHdc(dc))
                {
                    g.Clear(Color.Empty);
                }
            }));
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
