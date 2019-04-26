using Model.In;
using Model.Out;
using System;
using System.Drawing;
using System.Web.Configuration;
using WebSupergoo.ABCpdf11;
using WebSupergoo.ABCpdf11.Objects;

namespace Repository
{
    public partial class ScanningImageRepository
    {
        private JobCategoryRepository jobCategoryRepository = new JobCategoryRepository();

        #region .: Methods :.

        public ImageOut GetImageScanning(ImageIn imageIn)
        {
            ImageOut imageOut = new ImageOut();

            ECMJobCategoryOut eCMJobCategoryOut = jobCategoryRepository.GetECMJobCategoryByHash(imageIn.hash);

            Doc theDoc = new Doc();
            theDoc.Read(Convert.FromBase64String(eCMJobCategoryOut.result.archive));

            if (theDoc.PageCount < imageIn.page || imageIn.page <= 0)
            {
                throw new Exception(i18n.Resource.PageNotExist);
            }

            int dpi = 100;
            int.TryParse(WebConfigurationManager.AppSettings["DPI"], out dpi);

            theDoc.PageNumber = imageIn.page;
            theDoc.Rendering.DotsPerInch = dpi;

            if (imageIn.thumb)
            {
                imageOut.result.image = HelperImage.Resize((Image)theDoc.Rendering.GetBitmap(), 200, HelperImage.TypeSize.Width, 342, 80, false, HelperImage.TypeImage.JPG, Color.White);
            }
            else
            {
                imageOut.result.image = theDoc.Rendering.GetData(".jpg");
            }

            return imageOut;
        }

        #endregion
    }
}
