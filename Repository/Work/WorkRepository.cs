using DataEF.DataAccess;
using Model.In;
using Model.Out;
using Model.VM;
using System.Linq;

namespace Repository
{
    public partial class WorkRepository
    {
        RegisterEventRepository registerEventRepository = new RegisterEventRepository();

        #region .: API :.

        public WorkOut GetWorkByCode(WorkIn worksIn)
        {
            WorkOut workOut = new WorkOut();
            registerEventRepository.SaveRegisterEvent(worksIn.userId, worksIn.key, "Log - Start", "Repository.WorkRepository.GetWorkByCode", "");

            using (var db = new DBContext())
            {
                workOut.result = db.Works
                                   .Where(x => x.Active == true
                                            && x.DeletedDate == null
                                            && x.Code == worksIn.code)
                                   .Select(x => new WorkVM()
                                   {
                                       WorkId = x.WorkId,
                                       Code = x.Code,
                                       Registration = x.Registration,
                                       CreatedDate = x.CreatedDate,
                                       WorkCategories = x.WorkCategories.Select(y => new WorkCategoryVM()
                                       {
                                           WorkCategoryId = y.WorkCategoryId,
                                           Category = y.Categories.Code + " - " + y.Categories.Name,
                                           Code = y.Code,
                                           CreatedDate = y.CreatedDate
                                       }).ToList()
                                   }).FirstOrDefault();
            }

            registerEventRepository.SaveRegisterEvent(worksIn.userId, worksIn.key, "Log - End", "Repository.WorkRepository.GetWorkByCode", "");
            return workOut;
        }

        public WorksRegistrationOut GetWorksByRegistration(WorksRegistrationIn worksRegistrationIn)
        {
            WorksRegistrationOut worksRegistrationOut = new WorksRegistrationOut();
            registerEventRepository.SaveRegisterEvent(worksRegistrationIn.userId, worksRegistrationIn.key, "Log - Start", "Repository.WorkRepository.GetWorksByRegistration", "");

            using (var db = new DBContext())
            {
                worksRegistrationOut.result = db.Works
                                                .Where(x => x.Active == true
                                                         && x.DeletedDate == null
                                                         && x.Users.Registration == worksRegistrationIn.registration)
                                                .Select(x => new WorksRegistrationVM()
                                                {
                                                    WorkId = x.WorkId,
                                                    Code = x.Code,
                                                    Registration = x.Registration,
                                                    CreatedDate = x.CreatedDate,
                                                    WorkCategories = x.WorkCategories.Select(y => new WorkCategoryVM()
                                                    {
                                                        WorkCategoryId = y.WorkCategoryId,
                                                        Category = y.Categories.Code + " - " + y.Categories.Name,
                                                        Code = y.Code,
                                                        CreatedDate = y.CreatedDate
                                                    }).ToList()
                                                })
                                                .ToList();
            }

            registerEventRepository.SaveRegisterEvent(worksRegistrationIn.userId, worksRegistrationIn.key, "Log - End", "Repository.WorkRepository.GetWorksByRegistration", "");
            return worksRegistrationOut;
        }

        public WorksOut GetWorks(WorksIn worksIn)
        {
            WorksOut worksOut = new WorksOut();
            registerEventRepository.SaveRegisterEvent(worksIn.userId, worksIn.key, "Log - Start", "Repository.WorkRepository.GetWorks", "");

            using (var db = new DBContext())
            {
                var query = db.Works.Where(x => x.Active == true && x.DeletedDate == null && x.Code == worksIn.code);

                worksOut.totalCount = query.Count();

                worksOut.result = query
                                      .Select(x => new WorksVM()
                                      {
                                          WorkId = x.WorkId,
                                          Code = x.Code,
                                          Registration = x.Registration,
                                          CreatedDate = x.CreatedDate,
                                          WorkCategories = x.WorkCategories.Select(y => new WorkCategoryVM()
                                          {
                                              WorkCategoryId = y.WorkCategoryId,
                                              Category = y.Categories.Code + " - " + y.Categories.Name,
                                              Code = y.Code,
                                              CreatedDate = y.CreatedDate
                                          }).ToList()
                                      })
                                      .OrderBy(worksIn.sort, !worksIn.sortdirection.Equals("asc"))
                                      .Skip((worksIn.currentPage.Value - 1) * worksIn.qtdEntries.Value)
                                      .Take(worksIn.qtdEntries.Value)
                                      .ToList();
            }

            registerEventRepository.SaveRegisterEvent(worksIn.userId, worksIn.key, "Log - End", "Repository.WorkRepository.GetWorks", "");
            return worksOut;
        }

        #endregion
    }
}
