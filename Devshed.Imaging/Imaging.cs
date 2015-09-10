namespace Devshed.Imaging
{
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Imaging;
    using System.IO;
    using Devshed.Shared;
    using Goheer.EXIF;

    public static class Imaging
    {
        public static byte[] GetImage(this Stream  input, int width, int height, SizeMode mode)
        {
                using (var destination = new MemoryStream())
                {
                    SaveImageTo(input, destination, width, height, mode);

                    destination.Position = 0;
                    return destination.GetBytes();
                }
        }
        
        public static byte[] GetImage(this byte[] input, int width, int height, SizeMode mode)
        {
            using (var source = new MemoryStream(input))
            {
                using (var destination = new MemoryStream())
                {
                    SaveImageTo(source, destination, width, height, mode);

                    destination.Position = 0;
                    return destination.GetBytes();
                }
            }
        }

        public static void SaveImageTo(Stream inputStream, Stream outputStream, int width, int height, SizeMode mode)
        {
            SaveImageTo(inputStream, outputStream, width, height, mode, Color.White);
        }

        public static void SaveImageTo(Stream inputStream, Stream outputStream, int width, int height, SizeMode mode, Color background)
        {
            using (var image = Bitmap.FromStream(inputStream))
            {
                var calculator = new ImageDimensionCalculator(image.Size);
                var imageDimensions = calculator.GetDimensions(width, height, mode);

                //// create a new Bitmap the size of the new image
                using (Bitmap bitmap = new Bitmap(imageDimensions.Canvas.Width, imageDimensions.Canvas.Height))
                {
                    //// create a new graphic from the Bitmap
                    using (Graphics graphic = Graphics.FromImage(bitmap))
                    {
                        graphic.Clear(background);
                        graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        //// draw the newly resized image
                        graphic.DrawImage(image,
                            GetCentered(imageDimensions.Canvas.Width, imageDimensions.Render.Width), 
                            GetCentered(imageDimensions.Canvas.Height, imageDimensions.Render.Height),
                            imageDimensions.Render.Width, 
                            imageDimensions.Render.Height);
                        RotateByExif(bitmap);
                        SaveAsHighQualityJpeg(bitmap, outputStream);
                    }
                }
            }
        }

        private static int GetCentered(int canvas, int render)
        {
            return (canvas / 2) - (render / 2);
        }

        private static int GetCentered(SizeDimension imageDimensions)
        {
            return imageDimensions.Canvas.Width / 2 - (imageDimensions.Render.Width / 2);
        }

        private static void RotateByExif(Bitmap bitmap)
        {
            var exif = new EXIFextractor(ref bitmap, "n");

            if (exif["Orientation"] != null)
            {
                RotateFlipType flip = OrientationToFlipType(exif["Orientation"].ToString());

                if (flip != RotateFlipType.RotateNoneFlipNone) // don't flip of orientation is correct
                {
                    bitmap.RotateFlip(flip);
                    exif.setTag(0x112, "1"); // Optional: reset orientation tag
                }

            }
        }

        public static void RotateAndSaveImage(string source, RotateFlipType flip)
        {
            RotateAndSaveImage(source, source, flip);
        }

        public static void RotateAndSaveImage(string source, string destination, RotateFlipType flip)
        {
            //create an object that we can use to examine an image file
            using (Image img = Image.FromFile(source))
            {
                var jgpEncoder = GetJpegEncoder(ImageFormat.Jpeg);
                var parameters = GetHighQualityParameters();

                img.RotateFlip(flip);
                img.Save(destination, jgpEncoder, parameters);
            }
        }

        private static void SaveAsHighQualityJpeg(Bitmap bitmap, Stream stream)
        {
            var jgpEncoder = GetJpegEncoder(ImageFormat.Jpeg);
            var parameters = GetHighQualityParameters();
            bitmap.Save(stream, jgpEncoder, parameters);
        }

        private static EncoderParameters GetHighQualityParameters()
        {
            var qualityEncoder = Encoder.Quality;        
            var parameters = new EncoderParameters(1);
            var encodingParameter = new EncoderParameter(qualityEncoder, 100L);
            parameters.Param[0] = encodingParameter;
            return parameters;
        }

        private static ImageCodecInfo GetJpegEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();

            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }

            return null;
        }

        private static RotateFlipType OrientationToFlipType(string orientation)
        {
            switch (int.Parse(orientation))
            {
                case 1:
                    return RotateFlipType.RotateNoneFlipNone;
                case 2:
                    return RotateFlipType.RotateNoneFlipX;
                case 3:
                    return RotateFlipType.Rotate180FlipNone;
                case 4:
                    return RotateFlipType.Rotate180FlipX;
                case 5:
                    return RotateFlipType.Rotate90FlipX;
                case 6:
                    return RotateFlipType.Rotate90FlipNone;
                case 7:
                    return RotateFlipType.Rotate270FlipX;
                case 8:
                    return RotateFlipType.Rotate270FlipNone;
                default:
                    return RotateFlipType.RotateNoneFlipNone;
            }
        }
    }

}
