using System;
using System.Windows.Forms;

namespace Terror_Injector
{
    /// <summary>
    /// This class adds on to the functionality provided in System.Windows.Forms.ToolStrip.
    /// <bar><bar>
    /// - Source <https://gis.stackexchange.com/a/126288>
    /// </summary>
    public class ToolStripEx : ToolStrip
    {
        public ToolStripEx() : base() {
            this.Renderer = new ToolStripRenderer();
        }

        /// <summary>
        /// Gets or sets whether the ToolStripEx honers item clicks when its containing form does not have input focus.
        /// </summary>
        /// <remarks>
        /// Default value is false, which is the same behaviour provided by the base ToolStrip class.
        /// </remarks>
        public bool ClickThrough { get; set; } = false;

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (this.ClickThrough &&
                m.Msg == NativeConstants.WM_MOUSEACTIVATE &&
                m.Result == (IntPtr)NativeConstants.MA_ACTIVATEANDEAT)
            {
                m.Result = (IntPtr)NativeConstants.MA_ACTIVATE;
            }
        }
    }

    internal sealed class NativeConstants
    {
        internal const uint WM_MOUSEACTIVATE = 0x21;
        internal const uint MA_ACTIVATE = 1;
        internal const uint MA_ACTIVATEANDEAT = 2;
        internal const uint MA_NOACTIVATE = 3;
        internal const uint MA_NOACTIVATEANDEAT = 4;
    }
}
