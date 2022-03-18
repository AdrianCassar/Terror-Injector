using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;

namespace Terror_Injector
{
    internal class CustomComboBox : ComboBox
    {
        public CustomComboBox()
        {
            BorderColor = Color.Black;
        }

        private const int WM_PAINT = 0xF;
        private int buttonWidth = SystemInformation.HorizontalScrollBarArrowWidth;
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (m.Msg == WM_PAINT)
            {
                using (var g = Graphics.FromHwnd(Handle))
                {
                    using (var p = new Pen(this.BorderColor, 3))
                    {
                        g.DrawRectangle(p, 1, 1, Width - buttonWidth - 3, Height - 3);
                    }
                }
            }
        }

        protected override void OnDropDownClosed(EventArgs e)
        {
            base.OnDropDownClosed(e);

            this.FindForm().SelectNextControl(this, true, true, true, true);
        }

        [Browsable(true)]
        [Category("Appearance")]
        [DefaultValue(typeof(Color), "Black")]
        public Color BorderColor { get; set; }
    }
}
