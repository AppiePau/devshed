using System.Diagnostics;
using System.Drawing;

namespace Devshed.Imaging
{
    [DebuggerDisplay("Canvas ({Canvas.Width} x {Canvas.Height}), Render ({Render.Width} x {Render.Height})")]
    public sealed class SizeDimension
    {
        public Size Canvas { get; set; }

        public Size Render { get; set; }
    }
}