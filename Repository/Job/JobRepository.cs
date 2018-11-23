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

        public JobOut GetJobByCode(JobIn jobsIn)
        {
            JobOut jobOut = new JobOut();
            registerEventRepository.SaveRegisterEvent(jobsIn.userId, jobsIn.key, "Log - Start", "Repository.JobRepository.GetJobByCode", "");

            using (var db = new DBContext())
            {
                jobOut.result = db.Jobs
                                   .Where(x => x.Active == true
                                            && x.DeletedDate == null
                                            && x.Code == jobsIn.code)
                                   .Select(x => new JobVM()
                                   {
                                       JobId = x.JobId,
                                       Code = x.Code,
                                       Registration = x.Registration,
                                       CreatedDate = x.CreatedDate,
                                       JobCategories = x.JobCategories.Select(y => new JobCategoryVM()
                                       {
                                           JobCategoryId = y.JobCategoryId,
                                           Category = y.Categories.Code + " - " + y.Categories.Name,
                                           Code = y.Code,
                                           CreatedDate = y.CreatedDate
                                       }).ToList()
                                   }).FirstOrDefault();
            }

            registerEventRepository.SaveRegisterEvent(jobsIn.userId, jobsIn.key, "Log - End", "Repository.JobRepository.GetJobByCode", "");
            return jobOut;
        }

        public JobsRegistrationOut GetJobsByRegistration(JobsRegistrationIn jobsRegistrationIn)
        {
            JobsRegistrationOut jobsRegistrationOut = new JobsRegistrationOut();
            registerEventRepository.SaveRegisterEvent(jobsRegistrationIn.userId, jobsRegistrationIn.key, "Log - Start", "Repository.JobRepository.GetJobsByRegistration", "");

            using (var db = new DBContext())
            {
                jobsRegistrationOut.result = db.Jobs
                                                .Where(x => x.Active == true
                                                         && x.DeletedDate == null
                                                         && x.Users.Registration == jobsRegistrationIn.registration)
                                                .Select(x => new JobsRegistrationVM()
                                                {
                                                    JobId = x.JobId,
                                                    Code = x.Code,
                                                    Registration = x.Registration,
                                                    CreatedDate = x.CreatedDate,
                                                    JobCategories = x.JobCategories.Select(y => new JobCategoryVM()
                                                    {
                                                        JobCategoryId = y.JobCategoryId,
                                                        Category = y.Categories.Code + " - " + y.Categories.Name,
                                                        Code = y.Code,
                                                        CreatedDate = y.CreatedDate
                                                    }).ToList()
                                                })
                                                .ToList();
            }

            registerEventRepository.SaveRegisterEvent(jobsRegistrationIn.userId, jobsRegistrationIn.key, "Log - End", "Repository.JobRepository.GetJobsByRegistration", "");
            return jobsRegistrationOut;
        }

        public JobsOut GetJobs(JobsIn jobsIn)
        {
            JobsOut jobsOut = new JobsOut();
            registerEventRepository.SaveRegisterEvent(jobsIn.userId, jobsIn.key, "Log - Start", "Repository.JobRepository.GetJobs", "");

            using (var db = new DBContext())
            {
                var query = db.Jobs.Where(x => x.Active == true && x.DeletedDate == null && x.Code == jobsIn.code);

                jobsOut.totalCount = query.Count();

                jobsOut.result = query
                                      .Select(x => new JobsVM()
                                      {
                                          JobId = x.JobId,
                                          Code = x.Code,
                                          Registration = x.Registration,
                                          CreatedDate = x.CreatedDate,
                                          JobCategories = x.JobCategories.Select(y => new JobCategoryVM()
                                          {
                                              JobCategoryId = y.JobCategoryId,
                                              Category = y.Categories.Code + " - " + y.Categories.Name,
                                              Code = y.Code,
                                              CreatedDate = y.CreatedDate
                                          }).ToList()
                                      })
                                      .OrderBy(jobsIn.sort, !jobsIn.sortdirection.Equals("asc"))
                                      .Skip((jobsIn.currentPage.Value - 1) * jobsIn.qtdEntries.Value)
                                      .Take(jobsIn.qtdEntries.Value)
                                      .ToList();
            }

            registerEventRepository.SaveRegisterEvent(jobsIn.userId, jobsIn.key, "Log - End", "Repository.JobRepository.GetJobs", "");
            return jobsOut;
        }

        #endregion
    }
}
