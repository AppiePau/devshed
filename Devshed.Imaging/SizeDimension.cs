using System.Diagnostics;
using System.Drawing;

namespace Devshed.Imaging
{
    /// <summary>
    /// Holds the dimensions for resizing.
    /// </summary>
    [DebuggerDisplay("Canvas ({Canvas.Width} x {Canvas.Height}), Render ({Render.Width} x {Render.Height})")]
    public sealed class SizeDimension
    {
        /// <summary>
        /// The size of the source canvas.
        /// </summary>
        public Size Canvas { get; set; }

        /// <summary>
        /// The size of the destination canvas.
        /// </summary>
        public Size Render { get; set; }
    }
}