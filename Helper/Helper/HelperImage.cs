using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

public static class HelperImage
{
    public enum TypeSize
    {
        Height,
        Width
    }

    public enum TypeImage
    {
        BMP = 0,
        JPG = 1,
        GIF = 2,
        TIF = 3,
        PNG = 4
    }

    /// <summary>
    /// Resizes an image and writes the new image to a specified location
    /// </summary>
    /// <param name="image">Image original</param>
    /// <param name="pathDestination">Full physical path of resized image</param>
    /// <param name="newSize">New image size</param>
    /// <param name="typeSize">Type of size</param>
    /// <param name="otherSize">Other size</param>
    /// <param name="quality">Image quality</param>
    /// <param name="squareImage">Sets whether the final image should be square with white background on the left</param>
    /// <param name="typeImage">Image Type </param>
    /// <param name="background">Image background color when it has left over</param>originalWidth
    public static void Resize(Image image, string pathDestination, int newSize, TypeSize typeSize, int? otherSize, int quality, bool squareImage, TypeImage typeImage, Color background)
    {
        int originalWidth = image.Width;
        int originalHeight = image.Height;

        if (squareImage)
        {
            typeSize = (originalWidth > originalHeight ? TypeSize.Width : TypeSize.Height);
        }

        Double proportion;

        proportion = newSize * 100 / (TypeSize.Height.Equals(typeSize) ? originalHeight : originalWidth);

        int newWidth = (TypeSize.Width.Equals(typeSize) ? newSize : double.Parse(originalWidth.ToString()) * (proportion / 100)).ToInt();
        int newHeight = (TypeSize.Height.Equals(typeSize) ? newSize : double.Parse(originalHeight.ToString()) * (proportion / 100)).ToInt();

        int x = 0;
        int y = 0;

        Image imageResize;

        if (squareImage)
        {
            imageResize = new Bitmap(newSize, newSize);
        }
        else if (otherSize != null)
        {
            imageResize = new Bitmap((TypeSize.Width.Equals(typeSize) ? newSize : otherSize.Value), (TypeSize.Height.Equals(typeSize) ? newSize : otherSize.Value));
        }
        else
        {
            imageResize = new Bitmap(newWidth, newHeight);
        }

        Image newImage = image;
        Graphics imageLast = Graphics.FromImage(imageResize);

        //Fill with white background the generated image
        imageLast.Clear(background);

        imageLast.InterpolationMode = InterpolationMode.HighQualityBicubic;
        imageLast.SmoothingMode = SmoothingMode.HighQuality;
        imageLast.PixelOffsetMode = PixelOffsetMode.HighQuality;
        imageLast.CompositingQuality = CompositingQuality.HighQuality;

        if (!otherSize.HasValue)
        {
            x = (squareImage ? (newSize / 2) - ((newWidth > originalWidth ? originalWidth : newWidth) / 2) : (newWidth > originalWidth ? (newWidth / 2) - (originalWidth / 2) : 0));
            y = (squareImage ? (newSize / 2) - ((newHeight > originalHeight ? originalHeight : newHeight) / 2) : (newHeight > originalHeight ? (newHeight / 2) - (originalHeight / 2) : 0));
        }
        else
        {
            x = ((TypeSize.Width.Equals(typeSize) ? newSize : otherSize.Value) / 2) - ((newWidth > originalWidth ? originalWidth : newWidth) / 2);
            y = ((TypeSize.Height.Equals(typeSize) ? newSize : otherSize.Value) / 2) - ((newHeight > originalHeight ? originalHeight : newHeight) / 2);
        }

        if (newSize < (TypeSize.Height.Equals(typeSize) ? originalHeight : originalWidth))
        {
            imageLast.DrawImage(newImage, x, y, newWidth, newHeight);
        }
        else
        {
            imageLast.DrawImage(newImage, x, y, originalWidth, originalHeight);
        }

        ImageCodecInfo[] imageCodecInfos = ImageCodecInfo.GetImageEncoders();

        EncoderParameters encoderParameters;
        encoderParameters = new EncoderParameters(1);
        encoderParameters.Param[0] = new EncoderParameter(Encoder.Quality, quality);
        imageResize.Save(pathDestination, imageCodecInfos[(int)typeImage], encoderParameters);

        image.Dispose();
    }
}
