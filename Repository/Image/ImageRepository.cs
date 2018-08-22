using ApiTecnodim;
using Model.In;
using Model.Out;
using System;
using WebSupergoo.ABCpdf11;
using WebSupergoo.ABCpdf11.Objects;

namespace Repository
{
    public partial class ImageRepository
    {
        RegisterEventRepository registerEventRepository = new RegisterEventRepository();
        DocumentRepository documentRepository = new DocumentRepository();

        #region .: Methods :.

        public ImageOut GetImage(ImageIn imageIn)
        {
            ImageOut imageOut = new ImageOut();

            ECMDocumentOut documentOut = documentRepository.GetECMDocumentByHash(imageIn.hash);

            Doc theDoc = new Doc();
            theDoc.Read(Convert.FromBase64String(documentOut.result.archive));

            if (theDoc.PageCount < imageIn.page || imageIn.page <= 0)
            {
                throw new Exception(i18n.Resource.PageNotExist);
            }

            Doc singlePagePdf = new Doc();
            singlePagePdf.Rect.String = singlePagePdf.MediaBox.String = theDoc.MediaBox.String;
            singlePagePdf.AddPage();
            singlePagePdf.AddImageDoc(theDoc, imageIn.page, null);
            singlePagePdf.FrameRect();

            if (imageIn.thumb)
            {
                singlePagePdf.Rendering.DotsPerInch = 20;
                Page[] pages = singlePagePdf.ObjectSoup.Catalog.Pages.GetPageArrayAll();
                foreach (Page page in pages)
                {
                    singlePagePdf.Page = page.ID;
                    using (XImage xi = XImage.FromData(singlePagePdf.Rendering.GetData(".jpg"), null))
                        page.Thumbnail = PixMap.FromXImage(singlePagePdf.ObjectSoup, xi);
                }

                imageOut.result.image = singlePagePdf.Rendering.GetData(".jpg");
            }
            else
            {
                imageOut.result.image = singlePagePdf.Rendering.GetData(".jpg");
            }

            return imageOut;
        }

        #endregion
    }
}
