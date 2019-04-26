using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using WebSupergoo.ABCpdf11;

namespace Helper
{
    /// <summary>
    /// Helper class for setting up and applying a color matrix
    /// </summary>
    public class ColorMatrix
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public ColorMatrix()
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Matrix containing the values of the ColorMatrix
        /// </summary>
        public float[][] Matrix { get; set; }

        #endregion

        #region Public Functions

        /// <summary>
        /// Applies the color matrix
        /// </summary>
        /// <param name="OriginalImage">Image sent in</param>
        /// <returns>An image with the color matrix applied</returns>
        public Bitmap Apply(Bitmap OriginalImage)
        {
            Bitmap NewBitmap = new Bitmap(OriginalImage.Width, OriginalImage.Height);
            using (Graphics NewGraphics = Graphics.FromImage(NewBitmap))
            {
                System.Drawing.Imaging.ColorMatrix NewColorMatrix = new System.Drawing.Imaging.ColorMatrix(Matrix);
                using (ImageAttributes Attributes = new ImageAttributes())
                {
                    Attributes.SetColorMatrix(NewColorMatrix);
                    NewGraphics.DrawImage(OriginalImage,
                        new Rectangle(0, 0, OriginalImage.Width, OriginalImage.Height),
                        0, 0, OriginalImage.Width, OriginalImage.Height,
                        GraphicsUnit.Pixel,
                        Attributes);
                }
            }
            return NewBitmap;
        }

        #endregion
    }

    /// <summary>
    /// Converts an image to black and white
    /// </summary>
    /// <param name="Image">Image to change</param>
    /// <returns>A bitmap object of the black and white image</returns>
    public class ConvertBlackAndWhite
    {
        public static Bitmap Convert(Bitmap Image)
        {
            ColorMatrix TempMatrix = new ColorMatrix
            {
                Matrix = new float[][]{
                    new float[] { .75f, .75f, .75f, 0, 0 },
                    new float[] { .7f, .7f, .7f, 0, 0 },
                    new float[] { .75f, .75f, .75f, 0, 0 },
                    new float[] { 0, 0, 0, 1, 0 },
                    new float[] { -1f, -1f, -1f, 0, 1 }
                }
            };

            return TempMatrix.Apply(Image);
        }
    }

    /// <summary>
    /// Converts a doc to black and white
    /// </summary>
    /// <param name="Doc">Doc in PDF</param>
    /// <returns>A Doc object of the black and white</returns>
    public class PB
    {
        public static Doc Converter(Doc docOld)
        {
            Doc docNew = new Doc();
            docNew.Rect.String = docNew.MediaBox.String = docOld.MediaBox.String;

            for (int i = 1; i <= docOld.PageCount; i++)
            {
                Doc singlePagePdf = new Doc();
                singlePagePdf.Rect.String = singlePagePdf.MediaBox.String = docOld.MediaBox.String;
                singlePagePdf.AddPage();
                singlePagePdf.AddImageDoc(docOld, i, null);
                singlePagePdf.FrameRect();

                byte[] img = singlePagePdf.Rendering.GetData(".jpg");
                MemoryStream memoryStream = new MemoryStream(img);

                Bitmap bitmapOld = new Bitmap(memoryStream);                
                Bitmap bitmapNew = ConvertBlackAndWhite.Convert(bitmapOld);

                docNew.AddPage();
                docNew.PageNumber = i;
                docNew.AddImageBitmap(bitmapNew, true);
                singlePagePdf.FrameRect();

                bitmapOld.Dispose();
                bitmapNew.Dispose();
            }

            return docNew;
        }
    }
}