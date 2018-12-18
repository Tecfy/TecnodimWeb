using DataEF.DataAccess;
using Model.In;
using Model.Out;
using System;
using System.Linq;

namespace Repository
{
    public partial class JobStatusRepository
    {
        private RegisterEventRepository registerEventRepository = new RegisterEventRepository();

        #region .: API :.

        public JobStatusOut StatusJob(JobStatusIn jobStatusIn)
        {
            JobStatusOut jobStatusOut = new JobStatusOut();

            registerEventRepository.SaveRegisterEvent(jobStatusIn.id, jobStatusIn.key, "Log - Start", "Repository.JobRepository.StatusJob", "");

            #region .: Job :.

            using (var db = new DBContext())
            {
                Jobs job = db.Jobs.Where(x => x.JobId == jobStatusIn.jobId && x.Users.AspNetUserId == jobStatusIn.id).FirstOrDefault();

                if (job == null)
                {
                    throw new Exception(i18n.Resource.NoDataFound);
                }

                job.EditedDate = DateTime.Now;
                job.JobStatusId = jobStatusIn.jobStatusId;

                db.Entry(job).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }

            #endregion

            registerEventRepository.SaveRegisterEvent(jobStatusIn.id, jobStatusIn.key, "Log - End", "Repository.JobRepository.StatusJob", "");
            return jobStatusOut;
        }

        #endregion
    }
}
