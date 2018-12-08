using DataEF.DataAccess;
using Model.In;
using Model.Out;
using System;
using System.Linq;

namespace Repository
{
    public partial class JobCategoryAdditionalFieldRepository
    {
        RegisterEventRepository registerEventRepository = new RegisterEventRepository();

        public JobCategoryAdditionalFieldOut SaveJobCategoryAdditionalField(JobCategoryAdditionalFieldIn jobCategoryAdditionalFieldIn)
        {
            JobCategoryAdditionalFieldOut jobCategoryAdditionalFieldOut = new JobCategoryAdditionalFieldOut();

            registerEventRepository.SaveRegisterEvent(jobCategoryAdditionalFieldIn.id, jobCategoryAdditionalFieldIn.key, "Log - Start", "Repository.JobCategoryAdditionalFieldRepository.SaveJobCategoryAdditionalField", "");

            using (var db = new DBContext())
            {
                JobCategoryAdditionalFields jobCategoryAdditionalField = new JobCategoryAdditionalFields
                {
                    Active = true,
                    CreatedDate = DateTime.Now,
                    JobCategoryId = jobCategoryAdditionalFieldIn.jobCategoryId,
                    CategoryAdditionalFieldId = jobCategoryAdditionalFieldIn.categoryAdditionalFieldId
                };

                db.JobCategoryAdditionalFields.Add(jobCategoryAdditionalField);
                db.SaveChanges();
            }

            registerEventRepository.SaveRegisterEvent(jobCategoryAdditionalFieldIn.id, jobCategoryAdditionalFieldIn.key, "Log - End", "Repository.JobCategoryAdditionalFieldRepository.SaveJobCategoryAdditionalField", "");
            return jobCategoryAdditionalFieldOut;
        }

        public JobCategoryAdditionalFieldUpdateOut UpdateJobCategoryAdditionalField(JobCategoryAdditionalFieldUpdateIn jobCategoryAdditionalFieldUpdateIn)
        {
            JobCategoryAdditionalFieldUpdateOut jobCategoryAdditionalFieldUpdateOut = new JobCategoryAdditionalFieldUpdateOut();

            registerEventRepository.SaveRegisterEvent(jobCategoryAdditionalFieldUpdateIn.id, jobCategoryAdditionalFieldUpdateIn.key, "Log - Start", "Repository.JobCategoryAdditionalFieldRepository.UpdateJobCategoryAdditionalField", "");

            using (var db = new DBContext())
            {
                JobCategoryAdditionalFields jobCategoryAdditionalFields = db.JobCategoryAdditionalFields.Where(x => x.JobCategoryAdditionalFieldId == jobCategoryAdditionalFieldUpdateIn.jobCategoryAdditionalFieldId).FirstOrDefault();

                if (jobCategoryAdditionalFields == null)
                {
                    throw new Exception(i18n.Resource.NoDataFound);
                }

                jobCategoryAdditionalFields.EditedDate = DateTime.Now;
                jobCategoryAdditionalFields.Value = jobCategoryAdditionalFieldUpdateIn.value;

                db.Entry(jobCategoryAdditionalFields).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }

            registerEventRepository.SaveRegisterEvent(jobCategoryAdditionalFieldUpdateIn.id, jobCategoryAdditionalFieldUpdateIn.key, "Log - End", "Repository.JobCategoryAdditionalFieldRepository.UpdateJobCategoryAdditionalField", "");
            return jobCategoryAdditionalFieldUpdateOut;
        }
    }
}
