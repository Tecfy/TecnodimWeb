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

            SliceUpdateIn sliceUpdateIn = new SliceUpdateIn()
            {
                userId = classificationIn.userId,
                key = classificationIn.key,
                sliceId = classificationIn.sliceId,
                categoryId = classificationIn.categoryId,
                name = classificationIn.name,
            };

            SliceRepository.UpdateSlice(sliceUpdateIn);

            foreach (var item in classificationIn.pages)
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

            foreach (var item in classificationIn.additionalFields)
            {
                SliceCategoryAdditionalFieldIn sliceCategoryAdditionalFieldIn = new SliceCategoryAdditionalFieldIn()
                {
                    key = classificationIn.userId,
                    userId = classificationIn.key,
                    sliceId = classificationIn.sliceId,
                    categoryAdditionalFieldId = item.categoryAdditionalFieldId,
                    categoryId = classificationIn.categoryId,
                    value = item.value
                };

                sliceCategoryAdditionalFieldRepository.DeleteSliceCategoryAdditionalField(sliceCategoryAdditionalFieldIn);

                sliceCategoryAdditionalFieldRepository.SaveSliceCategoryAdditionalField(sliceCategoryAdditionalFieldIn);
            }

            registerEventRepository.SaveRegisterEvent(classificationIn.userId.Value, classificationIn.key.Value, "Log - End", "Repository.ClassificationRepository.SaveClassifications", "");
            return classificationOut;
        }

        #endregion
    }
}
