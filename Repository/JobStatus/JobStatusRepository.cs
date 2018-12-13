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

        public JobSatusOut SatusJob(JobSatusIn jobSatusIn)
        {
            JobSatusOut jobSatusOut = new JobSatusOut();

            registerEventRepository.SaveRegisterEvent(jobSatusIn.id, jobSatusIn.key, "Log - Start", "Repository.JobRepository.SetJobSatus", "");

            #region .: Job :.

            using (var db = new DBContext())
            {
                Jobs job = db.Jobs.Where(x => x.JobId == jobSatusIn.jobId && x.Users.AspNetUserId == jobSatusIn.id).FirstOrDefault();

                if (job == null)
                {
                    throw new Exception(i18n.Resource.NoDataFound);
                }

                job.EditedDate = DateTime.Now;
                job.JobStatusId = jobSatusIn.jobStatusId;

                db.Entry(job).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }

            #endregion

            registerEventRepository.SaveRegisterEvent(jobSatusIn.id, jobSatusIn.key, "Log - End", "Repository.JobRepository.SetJobSatus", "");
            return jobSatusOut;
        }

        #endregion
    }
}
