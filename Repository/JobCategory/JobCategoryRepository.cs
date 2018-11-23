using ApiTecnodim;
using DataEF.DataAccess;
using Model.In;
using Model.Out;
using System;
using System.Linq;

namespace Repository
{
    public partial class JobCategoryRepository
    {
        RegisterEventRepository registerEventRepository = new RegisterEventRepository();
        JobCategoryApi jobCategoryApi = new JobCategoryApi();

        #region .: API :.

        public JobCategoryOut SaveJobCategory(JobCategoryIn jobCategoryIn)
        {
            JobCategoryOut jobCategoryOut = new JobCategoryOut();

            registerEventRepository.SaveRegisterEvent(jobCategoryIn.userId, jobCategoryIn.key, "Log - Start", "Repository.JobCategoryRepository.SaveJobCategory", "");

            #region .: Job Category :.

            ECMJobCategorySaveIn ecmJobCategorySaveIn = new ECMJobCategorySaveIn();

            using (var db = new DBContext())
            {
                ecmJobCategorySaveIn = db.JobCategories
                                          .Where(x => x.JobCategoryId == jobCategoryIn.jobCategoryId)
                                          .Select(x => new ECMJobCategorySaveIn()
                                          {
                                              registration = x.Jobs.Registration,
                                              categoryId = x.Categories.Code,
                                              category = x.Categories.Name,
                                              archive = jobCategoryIn.archive,
                                              title = x.Categories.Name + ".pdf"
                                          })
                                          .FirstOrDefault();

                if (ecmJobCategorySaveIn == null)
                {
                    throw new Exception(i18n.Resource.NoDataFound);
                }
            }

            #endregion

            #region .: Sent New Document :.

            ECMJobCategorySaveOut ecmJobCategorySaveOut = jobCategoryApi.PostECMJobCategorySave(ecmJobCategorySaveIn);

            if (!ecmJobCategorySaveOut.success)
            {
                throw new Exception(ecmJobCategorySaveOut.messages.FirstOrDefault());
            }

            #endregion

            #region .: Update Job Category :.

            using (var db = new DBContext())
            {
                JobCategories jobCategory = db.JobCategories.Where(x => x.JobCategoryId == jobCategoryIn.jobCategoryId).FirstOrDefault();

                jobCategory.EditedDate = DateTime.Now;

                db.Entry(jobCategory).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }

            #endregion

            registerEventRepository.SaveRegisterEvent(jobCategoryIn.userId, jobCategoryIn.key, "Log - End", "Repository.JobCategoryRepository.SaveJobCategory", "");
            return jobCategoryOut;
        }

        #endregion
    }
}
