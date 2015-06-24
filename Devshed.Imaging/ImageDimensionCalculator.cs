using System;
using System.Drawing;

namespace Devshed.Imaging
{
    public sealed class ImageDimensionCalculator
    {
        private readonly Size originalSize;

        public ImageDimensionCalculator(Size originalSize)
        {
            this.originalSize = originalSize;
        }

        public SizeDimension GetDimensions(int width, int height, SizeMode mode)
        {
            if (mode == SizeMode.Crop)
            {
                return this.GetCroppedDimensions(width, height);
            }

            if (mode == SizeMode.Stretch)
            {
                return this.GetStretchedDimensions(width, height);
            }

            if (mode == SizeMode.FitRatioFilled)
            {
                return this.GetRatioFittedFilledDimensions(width, height);
            }

            return this.GetRatioFittedDimensions(width, height);
        }

        private SizeDimension GetCroppedDimensions(int width, int height)
        {
            return new SizeDimension
            {
                Canvas = this.GetCroppedCanvasSize(width, height),
                Render = this.GetCroppedCanvasSize(width, height)
            };
        }

        private SizeDimension GetStretchedDimensions(int width, int height)
        {
            return new SizeDimension
            {
                Canvas = new Size(width, height),
                Render = new Size(width, height)
            };
        }

        private SizeDimension GetRatioFittedDimensions(int width, int height)
        {
            return new SizeDimension
            {
                Canvas = this.GetScaledCanvasSize(width, height),
                Render = this.GetScaledCanvasSize(width, height)
            };
        }

        private SizeDimension GetRatioFittedFilledDimensions(int width, int height)
        {
            return new SizeDimension
            {
                Canvas = new Size(width, height),
                Render = this.GetScaledCanvasSize(width, height)
            };
        }

        private Size GetScaledCanvasSize(int maxWidth, int maxHeight)
        {
            var ratio = Math.Min((decimal)maxWidth / (decimal)this.originalSize.Width, (decimal)maxHeight / (decimal)this.originalSize.Height);

            return new Size((int)Math.Round(this.originalSize.Width * ratio), (int)Math.Round(this.originalSize.Height * ratio));
        }


        private Size GetCroppedCanvasSize(int maxWidth, int maxHeight)
        {
            var size = new Size(this.originalSize.Width, this.originalSize.Height);

            if (size.Width > maxWidth)
            {
                size = new Size(maxWidth, size.Height);
            }

            if (size.Height > maxHeight)
            {
                size = new Size(size.Width, maxHeight);
            }

            return size;
        }

        private int ResizeByAspect(decimal width, decimal height, int maxHeight)
        {
            return (int)((width / height) * maxHeight);
        }
    }
}