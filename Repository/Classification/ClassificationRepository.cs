using Model.In;
using Model.Out;

namespace Repository
{
    public partial class ClassificationRepository
    {
        private RegisterEventRepository registerEventRepository = new RegisterEventRepository();
        private SliceRepository sliceRepository = new SliceRepository();
        private SlicePageRepository slicePageRepository = new SlicePageRepository();
        private SliceCategoryAdditionalFieldRepository sliceCategoryAdditionalFieldRepository = new SliceCategoryAdditionalFieldRepository();

        #region .: Methods :.

        public ClassificationOut SaveClassification(ClassificationIn classificationIn)
        {
            ClassificationOut classificationOut = new ClassificationOut();

            registerEventRepository.SaveRegisterEvent(classificationIn.id, classificationIn.key, "Log - Start", "Repository.ClassificationRepository.SaveClassifications", "");

            #region .: Slice :.

            SliceUpdateIn sliceUpdateIn = new SliceUpdateIn()
            {
                id = classificationIn.id,
                key = classificationIn.key,
                sliceId = classificationIn.sliceId,
                categoryId = classificationIn.categoryId,
            };

            sliceRepository.UpdateSlice(sliceUpdateIn);

            #endregion

            #region .: Pages :.

            foreach (var item in classificationIn.slicePages)
            {
                SlicePageUpdateIn slicePageUpdateIn = new SlicePageUpdateIn()
                {
                    key = classificationIn.id,
                    id = classificationIn.key,
                    slicePageId = item.slicePageId,
                    page = item.page,
                    rotate = item.rotate
                };

                slicePageRepository.UpdateSlicePage(slicePageUpdateIn);
            }

            #endregion

            #region .: AdditionalFields :.

            sliceCategoryAdditionalFieldRepository.DeleteSliceCategoryAdditionalField(new SliceCategoryAdditionalFieldDeleteIn
            {
                key = classificationIn.id,
                id = classificationIn.id,
                sliceId = classificationIn.sliceId,
                categoryId = classificationIn.categoryId,
            });

            foreach (var item in classificationIn.additionalFields)
            {
                SliceCategoryAdditionalFieldIn sliceCategoryAdditionalFieldIn = new SliceCategoryAdditionalFieldIn()
                {
                    key = classificationIn.key,
                    id = classificationIn.id,
                    sliceId = classificationIn.sliceId,
                    categoryAdditionalFieldId = item.categoryAdditionalFieldId,
                    categoryId = classificationIn.categoryId,
                    value = item.value
                };

                sliceCategoryAdditionalFieldRepository.SaveSliceCategoryAdditionalField(sliceCategoryAdditionalFieldIn);
            }

            #endregion

            registerEventRepository.SaveRegisterEvent(classificationIn.id, classificationIn.key, "Log - End", "Repository.ClassificationRepository.SaveClassifications", "");
            return classificationOut;
        }

        #endregion
    }
}
