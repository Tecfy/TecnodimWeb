using ApiTecnodim;
using DataEF.DataAccess;
using Model.In;
using Model.Out;
using System;
using System.Linq;

namespace Repository
{
    public partial class WorkCategoryRepository
    {
        RegisterEventRepository registerEventRepository = new RegisterEventRepository();
        WorkCategoryApi workCategoryApi = new WorkCategoryApi();

        #region .: API :.

        public WorkCategoryOut SaveWorkCategory(WorkCategoryIn workCategoryIn)
        {
            WorkCategoryOut workCategoryOut = new WorkCategoryOut();

            registerEventRepository.SaveRegisterEvent(workCategoryIn.userId, workCategoryIn.key, "Log - Start", "Repository.WorkCategoryRepository.SaveWorkCategory", "");

            #region .: Work Category :.

            ECMWorkCategorySaveIn ecmWorkCategorySaveIn = new ECMWorkCategorySaveIn();

            using (var db = new DBContext())
            {
                ecmWorkCategorySaveIn = db.WorkCategories
                                          .Where(x => x.WorkCategoryId == workCategoryIn.workCategoryId)
                                          .Select(x => new ECMWorkCategorySaveIn()
                                          {
                                              registration = x.Works.Registration,
                                              categoryId = x.Categories.Code,
                                              category = x.Categories.Name,
                                              archive = workCategoryIn.archive,
                                              title = x.Categories.Name + ".pdf"
                                          })
                                          .FirstOrDefault();

                if (ecmWorkCategorySaveIn == null)
                {
                    throw new Exception(i18n.Resource.NoDataFound);
                }
            }

            #endregion

            #region .: Sent New Document :.

            ECMWorkCategorySaveOut ecmWorkCategorySaveOut = workCategoryApi.PostECMWorkCategorySave(ecmWorkCategorySaveIn);

            if (!ecmWorkCategorySaveOut.success)
            {
                throw new Exception(ecmWorkCategorySaveOut.messages.FirstOrDefault());
            }

            #endregion

            #region .: Update Work Category :.

            using (var db = new DBContext())
            {
                WorkCategories workCategory = db.WorkCategories.Where(x => x.WorkCategoryId == workCategoryIn.workCategoryId).FirstOrDefault();

                workCategory.EditedDate = DateTime.Now;

                db.Entry(workCategory).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }

            #endregion

            registerEventRepository.SaveRegisterEvent(workCategoryIn.userId, workCategoryIn.key, "Log - End", "Repository.WorkCategoryRepository.SaveWorkCategory", "");
            return workCategoryOut;
        }

        #endregion
    }
}
