using DataEF.DataAccess;
using Model.In;
using Model.Out;
using Model.VM;
using System.Linq;

namespace Repository
{
    public partial class JobRepository
    {
        RegisterEventRepository registerEventRepository = new RegisterEventRepository();

        #region .: API :.

        public JobsRegistrationOut GetJobsByRegistration(JobsRegistrationIn jobsRegistrationIn)
        {
            JobsRegistrationOut jobsRegistrationOut = new JobsRegistrationOut();
            registerEventRepository.SaveRegisterEvent(jobsRegistrationIn.userId, jobsRegistrationIn.key, "Log - Start", "Repository.JobRepository.GetJobsByRegistration", "");

            using (var db = new DBContext())
            {
                jobsRegistrationOut.result = db.Jobs
                                                .Where(x => x.Active == true
                                                         && x.DeletedDate == null
                                                         && x.Users.Registration == jobsRegistrationIn.registration
                                                         && x.JobCategories.Count(y => y.Received == false) > 0)
                                                .Select(x => new JobsRegistrationVM()
                                                {
                                                    JobId = x.JobId,
                                                    Registration = x.Registration,
                                                    Name = x.Name,
                                                    JobCategories = x.JobCategories.Where(y => y.Received == false)
                                                    .Select(y => new JobCategoriesRegistrationVM()
                                                    {
                                                        JobCategoryId = y.JobCategoryId,
                                                        Category = y.Categories.Code + " - " + y.Categories.Name,
                                                    }).ToList()
                                                })
                                                .ToList();
            }

            registerEventRepository.SaveRegisterEvent(jobsRegistrationIn.userId, jobsRegistrationIn.key, "Log - End", "Repository.JobRepository.GetJobsByRegistration", "");
            return jobsRegistrationOut;
        }

        #endregion
    }
}
