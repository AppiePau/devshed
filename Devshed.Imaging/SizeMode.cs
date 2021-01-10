namespace Devshed.Imaging
{
    /// <summary>
    /// The method to resize the image.
    /// </summary>
    public enum SizeMode
    {
        /// <summary>
        /// Fit the image to the ratio.
        /// </summary>
        FitRatio = 0,

        /// <summary>
        /// Crop the image.
        /// </summary>
        Crop = 1,

        /// <summary>
        /// Stretch image to full extent of the new dimensions.
        /// </summary>
        Stretch = 2,

        /// <summary>
        /// Fit the image, while respecting the ratio of the new dimensions.
        /// </summary>
        FitRatioFilled = 3
    }
}