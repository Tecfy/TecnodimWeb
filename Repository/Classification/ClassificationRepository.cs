using DataEF.DataAccess;
using Model.In;
using Model.Out;

namespace Repository
{
    public partial class ClassificationRepository
    {
        RegisterEventRepository registerEventRepository = new RegisterEventRepository();
        SliceRepository SliceRepository = new SliceRepository();
        SlicePageRepository slicePageRepository = new SlicePageRepository();
        SliceCategoryAdditionalFieldRepository sliceCategoryAdditionalFieldRepository = new SliceCategoryAdditionalFieldRepository();

        #region .: Methods :.

        public ClassificationOut SaveClassification(ClassificationIn classificationIn)
        {
            ClassificationOut classificationOut = new ClassificationOut();

            registerEventRepository.SaveRegisterEvent(classificationIn.userId.Value, classificationIn.key.Value, "Log - Start", "Repository.ClassificationRepository.SaveClassifications", "");

            #region .: Slice :.

            SliceUpdateIn sliceUpdateIn = new SliceUpdateIn()
            {
                userId = classificationIn.userId,
                key = classificationIn.key,
                sliceId = classificationIn.sliceId,
                categoryId = classificationIn.categoryId,
            };

            SliceRepository.UpdateSlice(sliceUpdateIn);

            #endregion

            #region .: Pages :.

            foreach (var item in classificationIn.slicePages)
            {
                SlicePageUpdateIn slicePageUpdateIn = new SlicePageUpdateIn()
                {
                    key = classificationIn.userId,
                    userId = classificationIn.key,
                    slicePageId = item.slicePageId,
                    page = item.page,
                    rotate = item.rotate
                };

                slicePageRepository.UpdateSlicePage(slicePageUpdateIn);
            }

            #endregion

            #region .: AdditionalFields :.

            sliceCategoryAdditionalFieldRepository.DeleteSliceCategoryAdditionalField(new ApiSliceCategoryAdditionalFieldDeleteIn
            {
                key = classificationIn.userId,
                userId = classificationIn.userId,
                sliceId = classificationIn.sliceId,
                categoryId = classificationIn.categoryId,
            });

            foreach (var item in classificationIn.additionalFields)
            {
                ApiSliceCategoryAdditionalFieldIn sliceCategoryAdditionalFieldIn = new ApiSliceCategoryAdditionalFieldIn()
                {
                    key = classificationIn.key,
                    userId = classificationIn.userId,
                    sliceId = classificationIn.sliceId,
                    categoryAdditionalFieldId = item.categoryAdditionalFieldId,
                    categoryId = classificationIn.categoryId,
                    value = item.value
                };

                sliceCategoryAdditionalFieldRepository.SaveSliceCategoryAdditionalField(sliceCategoryAdditionalFieldIn);
            }

            #endregion

            registerEventRepository.SaveRegisterEvent(classificationIn.userId.Value, classificationIn.key.Value, "Log - End", "Repository.ClassificationRepository.SaveClassifications", "");
            return classificationOut;
        }

        #endregion
    }
}
