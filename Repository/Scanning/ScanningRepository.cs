using DataEF.DataAccess;
using Helper.Enum;
using Model.In;
using Model.Out;
using System;
using System.Linq;

namespace Repository
{
    public partial class ScanningRepository
    {
        RegisterEventRepository registerEventRepository = new RegisterEventRepository();
        JobRepository jobRepository = new JobRepository();
        JobCategoryRepository jobCategoryRepository = new JobCategoryRepository();

        #region .: Methods :.

        public ScanningOut SaveScanning(ScanningIn scanningIn)
        {
            ScanningOut scanningOut = new ScanningOut();
            JobCreateOut jobCreateOut = new JobCreateOut();

            registerEventRepository.SaveRegisterEvent(scanningIn.id, scanningIn.key, "Log - Start", "Repository.ScanningRepository.SaveScanning", "");

            #region .: Job :.

            int userId = 0;

            using (var db = new DBContext())
            {
                userId = db.Users.Where(x => x.AspNetUserId == scanningIn.id).FirstOrDefault().UserId;
            }

            JobCreateIn jobsCreateIn = new JobCreateIn()
            {
                userId = userId,
                jobStatusId = (int)EJobStatus.New,
                registration = scanningIn.registration,
                name = scanningIn.name,
            };

            jobCreateOut = jobRepository.CreateJob(jobsCreateIn);

            #endregion

            #region .: Pages :.

            foreach (var item in scanningIn.jobCategories)
            {
                string categoryCode = string.Empty;

                using (var db = new DBContext())
                {
                    categoryCode = db.Categories.Where(x => x.CategoryId == item.categoryId).FirstOrDefault().Code;
                }

                JobCategoryCreateIn jobCategoryCreateIn = new JobCategoryCreateIn()
                {
                    key = scanningIn.id,
                    id = scanningIn.key,
                    jobId = jobCreateOut.result.jobId,
                    categoryId = item.categoryId,
                    code = DateTime.Now.ToString("yyyyMMddHHmmsss") + "-" + scanningIn.registration + "-" + categoryCode
                };

                jobCategoryRepository.CreateJobCategory(jobCategoryCreateIn);
            }

            #endregion

            registerEventRepository.SaveRegisterEvent(scanningIn.id, scanningIn.key, "Log - End", "Repository.ScanningRepository.SaveScanning", "");
            return scanningOut;
        }

        #endregion
    }
}
