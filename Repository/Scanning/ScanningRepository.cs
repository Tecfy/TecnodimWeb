using DataEF.DataAccess;
using Helper.Enum;
using Model.In;
using Model.Out;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Configuration;

namespace Repository
{
    public partial class ScanningRepository
    {
        private RegisterEventRepository registerEventRepository = new RegisterEventRepository();
        private JobRepository jobRepository = new JobRepository();
        private JobCategoryRepository jobCategoryRepository = new JobCategoryRepository();
        private CategoryAdditionalFieldRepository categoryAdditionalFieldRepository = new CategoryAdditionalFieldRepository();
        private JobCategoryAdditionalFieldRepository jobCategoryAdditionalFieldRepository = new JobCategoryAdditionalFieldRepository();

        #region .: Methods :.

        public ScanningPermissionOut GetPermission(ScanningPermissionIn scanningPermissionIn)
        {
            ScanningPermissionOut scanningPermissionOut = new ScanningPermissionOut();
            registerEventRepository.SaveRegisterEvent(scanningPermissionIn.id, scanningPermissionIn.key, "Log - Start", "Repository.ScanningRepository.GetPermission", "");

            if (!bool.Parse(WebConfigurationManager.AppSettings["Scanning.Multiple"].ToString()))
            {
                using (var db = new DBContext())
                {
                    int jobStatus = (int)EJobStatus.Sent;

                    bool job = db.Jobs.Any(x => x.DeletedDate == null
                                            && x.Active == true
                                            && x.Users.AspNetUserId == scanningPermissionIn.id
                                            && x.JobStatusId != jobStatus);

                    if (job)
                    {
                        scanningPermissionOut.messages.Add(i18n.Resource.MessagePendingSscan);
                    }
                }
            }

            registerEventRepository.SaveRegisterEvent(scanningPermissionIn.id, scanningPermissionIn.key, "Log - End", "Repository.ScanningRepository.GetPermission", "");
            return scanningPermissionOut;
        }

        public ScanningOut SaveScanning(ScanningIn scanningIn)
        {
            ScanningOut scanningOut = new ScanningOut();
            JobCreateOut jobCreateOut = new JobCreateOut();
            JobCategoryCreateOut jobCategoryCreateOut = new JobCategoryCreateOut();

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
                course = scanningIn.course,
                unityId = scanningIn.unityId
            };

            jobCreateOut = jobRepository.CreateJob(jobsCreateIn);

            #endregion

            #region .: JobCategories :.

            foreach (var item in scanningIn.jobCategories)
            {
                string categoryCode = string.Empty;

                using (var db = new DBContext())
                {
                    categoryCode = db.Categories.Where(x => x.CategoryId == item.categoryId).FirstOrDefault().Code;
                }

                var justDigits = new Regex(@"[^\d]");
                categoryCode = justDigits.Replace(categoryCode, "");

                JobCategoryCreateIn jobCategoryCreateIn = new JobCategoryCreateIn()
                {
                    key = scanningIn.id,
                    id = scanningIn.key,
                    jobId = jobCreateOut.result.jobId,
                    categoryId = item.categoryId,
                    code = DateTime.Now.ToString("yyyyMMddHHmmsss") + "-" + scanningIn.registration + "-" + categoryCode
                };

                jobCategoryCreateOut = jobCategoryRepository.CreateJobCategory(jobCategoryCreateIn);

                #region .: AdditionalFields :.

                CategoryAdditionalFieldsIn categoryAdditionalFieldsIn = new CategoryAdditionalFieldsIn { key = scanningIn.key, id = scanningIn.id, categoryId = item.categoryId };

                CategoryAdditionalFieldsOut categoryAdditionalFieldsOut = categoryAdditionalFieldRepository.GetCategoryAdditionalFieldsByCategoryId(categoryAdditionalFieldsIn);

                foreach (var categoryAdditionalField in categoryAdditionalFieldsOut.result)
                {
                    JobCategoryAdditionalFieldIn jobCategoryAdditionalFieldIn = new JobCategoryAdditionalFieldIn()
                    {
                        key = scanningIn.key,
                        id = scanningIn.id,
                        jobCategoryId = jobCategoryCreateOut.result.jobCategoryId,
                        categoryAdditionalFieldId = categoryAdditionalField.categoryAdditionalFieldId,
                    };

                    jobCategoryAdditionalFieldRepository.SaveJobCategoryAdditionalField(jobCategoryAdditionalFieldIn);
                }

                #endregion
            }

            #endregion

            registerEventRepository.SaveRegisterEvent(scanningIn.id, scanningIn.key, "Log - End", "Repository.ScanningRepository.SaveScanning", "");
            return scanningOut;
        }

        #endregion
    }
}
