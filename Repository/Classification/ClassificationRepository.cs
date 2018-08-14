using DataEF.DataAccess;
using Model.In;
using Model.Out;

namespace Repository
{
    public partial class ClassificationRepository
    {
        RegisterEventRepository registerEventRepository = new RegisterEventRepository();
        ClippingRepository ClippingRepository = new ClippingRepository();
        ClippingPageRepository clippingPageRepository = new ClippingPageRepository();

        #region .: Methods :.

        public ClassificationOut SaveClassification(ClassificationIn classificationIn)
        {
            ClassificationOut classificationOut = new ClassificationOut();

            registerEventRepository.SaveRegisterEvent(classificationIn.userId.Value, classificationIn.key.Value, "Log - Start", "Repository.ClassificationRepository.SaveClassifications", "");

            ClippingUpdateIn clippingUpdateIn = new ClippingUpdateIn()
            {
                userId = classificationIn.userId,
                key = classificationIn.key,
                clippingId = classificationIn.clippingId,
                categoryId = classificationIn.clippingId,
                name = classificationIn.name,
                classification = classificationIn.classification
            };

            ClippingRepository.UpdateClipping(clippingUpdateIn);

            foreach (var item in classificationIn.pages)
            {
                ClippingPageUpdateIn clippingPageUpdateIn = new ClippingPageUpdateIn()
                {
                    key = classificationIn.userId,
                    userId = classificationIn.key,
                    clippingPageId = item.clippingPageId,
                    page = item.page,
                    rotate = item.rotate
                };

                clippingPageRepository.UpdateClippingPage(clippingPageUpdateIn);
            }

            registerEventRepository.SaveRegisterEvent(classificationIn.userId.Value, classificationIn.key.Value, "Log - End", "Repository.ClassificationRepository.SaveClassifications", "");
            return classificationOut;
        }

        #endregion
    }
}
