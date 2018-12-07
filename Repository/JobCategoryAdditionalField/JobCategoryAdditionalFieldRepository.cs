using DataEF.DataAccess;
using Model.In;
using Model.Out;
using System;

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

    }
}
