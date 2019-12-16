using DataEF.DataAccess;
using Model.In;
using Model.Out;
using System;
using System.Linq;

namespace Repository
{
    public class JobCategoryPageRepository
    {
        private RegisterEventRepository registerEventRepository = new RegisterEventRepository();

        public JobCategoryPageDeleteOut DeleteJobCategoryPage(JobCategoryPageDeleteIn jobCategoryPageDeleteIn)
        {
            JobCategoryPageDeleteOut jobCategoryPageDeleteOut = new JobCategoryPageDeleteOut();

            registerEventRepository.SaveRegisterEvent(jobCategoryPageDeleteIn.id, jobCategoryPageDeleteIn.key, "Log - Start", "Repository.JobCategoryPageRepository.DeleteJobCategoryPage", "");

            using (var db = new DBContext())
            {
                JobCategoryPages jobCategoryPages = db.JobCategoryPages.Where(x => x.JobCategoryPageId == jobCategoryPageDeleteIn.jobCategoryPageId).FirstOrDefault();

                if (jobCategoryPages == null)
                {
                    throw new Exception(i18n.Resource.RegisterNotFound);
                }

                jobCategoryPages.DeletedDate = DateTime.Now;
                jobCategoryPages.EditedDate = DateTime.Now;
                jobCategoryPages.Active = false;

                db.Entry(jobCategoryPages).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }

            registerEventRepository.SaveRegisterEvent(jobCategoryPageDeleteIn.id, jobCategoryPageDeleteIn.key, "Log - End", "Repository.JobCategoryPageRepository.DeleteJobCategoryPage", "");
            return jobCategoryPageDeleteOut;
        }
    }
}
