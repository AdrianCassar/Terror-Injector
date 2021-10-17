using System.Drawing;
using System.Windows.Forms;

namespace Terror_Injector
{
    public class ToolStripRenderer : ToolStripProfessionalRenderer
    {
        public ToolStripRenderer() : base() { }

        protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
        {
            if (!(e.ToolStrip is ToolStrip))
                base.OnRenderToolStripBorder(e);
        }

        protected override void OnRenderButtonBackground(ToolStripItemRenderEventArgs e)
        {
            if (!e.Item.Selected)
            {
                base.OnRenderButtonBackground(e);
            }
            else
            {
                Rectangle rectangle = new Rectangle(0, 0, e.Item.Size.Width - 1, e.Item.Size.Height - 1);
                //e.Graphics.FillRectangle(Brushes.White, rectangle);
                e.Graphics.DrawRectangle(Pens.Red, rectangle);
            }
        }
    }
}
