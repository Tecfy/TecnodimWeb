using ApiTecnodim;
using DataEF.DataAccess;
using Model.In;
using Model.Out;
using Model.VM;
using System;
using System.Collections.Generic;
using System.Linq;
using WebSupergoo.ABCpdf11;

namespace Repository
{
    public partial class JobCategoryRepository
    {
        RegisterEventRepository registerEventRepository = new RegisterEventRepository();
        JobCategoryApi jobCategoryApi = new JobCategoryApi();

        #region .: API :.

        public JobCategoryArchiveOut SetJobCategorySave(JobCategoryArchiveIn jobCategorySaveIn)
        {
            JobCategoryArchiveOut jobCategoryOut = new JobCategoryArchiveOut();

            registerEventRepository.SaveRegisterEvent(jobCategorySaveIn.id, jobCategorySaveIn.key, "Log - Start", "Repository.JobCategoryRepository.SetJobCategorySave", "");

            #region .: Job Category :.

            ECMJobCategorySaveIn ecmJobCategorySaveIn = new ECMJobCategorySaveIn();

            using (var db = new DBContext())
            {
                ecmJobCategorySaveIn = db.JobCategories
                                          .Where(x => x.JobCategoryId == jobCategorySaveIn.jobCategoryId)
                                          .Select(x => new ECMJobCategorySaveIn()
                                          {
                                              registration = x.Jobs.Registration,
                                              code = x.Code,
                                              categoryId = x.Categories.Code,
                                              archive = jobCategorySaveIn.archive,
                                              title = x.Categories.Name + ".pdf",
                                              dataJob = x.Jobs.CreatedDate,
                                              user = x.Jobs.Users.Registration
                                          })
                                          .FirstOrDefault();

                if (ecmJobCategorySaveIn == null)
                {
                    throw new Exception(i18n.Resource.NoDataFound);
                }
            }

            #endregion

            #region .: Sent New Document :.

            ECMJobCategorySaveOut ecmJobCategorySaveOut = jobCategoryApi.SetECMJobCategorySave(ecmJobCategorySaveIn);

            if (!ecmJobCategorySaveOut.success)
            {
                throw new Exception(ecmJobCategorySaveOut.messages.FirstOrDefault());
            }

            #endregion

            #region .: JobCategoryPage :.

            Doc theDoc = new Doc();
            theDoc.Read(Convert.FromBase64String(jobCategorySaveIn.archive));

            using (var db = new DBContext())
            {
                List<JobCategoryPages> jobCategoryPages = db.JobCategoryPages.Where(x => x.Active == true && x.DeletedDate == null && x.JobCategoryId == jobCategorySaveIn.jobCategoryId).ToList();

                if (jobCategoryPages != null && jobCategoryPages.Count > 0)
                {
                    foreach (var item in jobCategoryPages)
                    {
                        item.Active = false;
                        item.DeletedDate = DateTime.Now;

                        db.Entry(item).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                    }
                }

                for (int i = 1; i <= theDoc.PageCount; i++)
                {
                    JobCategoryPages jobCategoryPage = new JobCategoryPages
                    {
                        JobCategoryId = jobCategorySaveIn.jobCategoryId,
                        Page = i
                    };

                    db.JobCategoryPages.Add(jobCategoryPage);
                    db.SaveChanges();
                }
            }

            #endregion

            #region .: Update Job Category :.

            using (var db = new DBContext())
            {
                JobCategories jobCategory = db.JobCategories.Where(x => x.JobCategoryId == jobCategorySaveIn.jobCategoryId).FirstOrDefault();

                jobCategory.Received = true;
                jobCategory.EditedDate = DateTime.Now;

                db.Entry(jobCategory).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }

            #endregion

            registerEventRepository.SaveRegisterEvent(jobCategorySaveIn.id, jobCategorySaveIn.key, "Log - End", "Repository.JobCategoryRepository.SetJobCategorySave", "");
            return jobCategoryOut;
        }

        public JobCategoryDisapproveOut SetJobCategoryDisapprove(JobCategoryDisapproveIn jobCategoryDisapproveIn)
        {
            JobCategoryDisapproveOut jobCategoryDisapproveOut = new JobCategoryDisapproveOut();

            registerEventRepository.SaveRegisterEvent(jobCategoryDisapproveIn.id, jobCategoryDisapproveIn.key, "Log - Start", "Repository.JobCategoryRepository.SetJobCategoryDisapprove", "");

            #region .: Job Category :.

            using (var db = new DBContext())
            {
                JobCategories jobCategory = db.JobCategories.Where(x => x.JobCategoryId == jobCategoryDisapproveIn.jobCategoryId && x.Jobs.Users.AspNetUserId == jobCategoryDisapproveIn.id).FirstOrDefault();

                if (jobCategory == null)
                {
                    throw new Exception(i18n.Resource.NoDataFound);
                }

                jobCategory.EditedDate = DateTime.Now;
                jobCategory.Received = false;

                db.Entry(jobCategory).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }

            #endregion

            registerEventRepository.SaveRegisterEvent(jobCategoryDisapproveIn.id, jobCategoryDisapproveIn.key, "Log - End", "Repository.JobCategoryRepository.SetJobCategoryDisapprove", "");
            return jobCategoryDisapproveOut;
        }
 
        public JobCategoryDeletedOut SetJobCategoryDeleted(JobCategoryDeletedIn jobCategoryDeletedIn)
        {
            JobCategoryDeletedOut jobCategoryDeletedOut = new JobCategoryDeletedOut();

            registerEventRepository.SaveRegisterEvent(jobCategoryDeletedIn.id, jobCategoryDeletedIn.key, "Log - Start", "Repository.JobCategoryRepository.SetJobCategoryDeleted", "");

            #region .: Job Category :.

            using (var db = new DBContext())
            {
                JobCategories jobCategory = db.JobCategories.Where(x => x.JobCategoryId == jobCategoryDeletedIn.jobCategoryId && x.Jobs.Users.AspNetUserId == jobCategoryDeletedIn.id).FirstOrDefault();

                if (jobCategory == null)
                {
                    throw new Exception(i18n.Resource.NoDataFound);
                }

                jobCategory.Active = false;
                jobCategory.DeletedDate = DateTime.Now;

                db.Entry(jobCategory).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }

            #endregion

            registerEventRepository.SaveRegisterEvent(jobCategoryDeletedIn.id, jobCategoryDeletedIn.key, "Log - End", "Repository.JobCategoryRepository.SetJobCategoryDeleted", "");
            return jobCategoryDeletedOut;
        }

        public JobCategoryCreateOut CreateJobCategory(JobCategoryCreateIn jobCategoryCreateIn)
        {
            JobCategoryCreateOut jobCategoryCreateOut = new JobCategoryCreateOut();

            registerEventRepository.SaveRegisterEvent(jobCategoryCreateIn.id, jobCategoryCreateIn.key, "Log - Start", "Repository.JobCategoryRepository.CreateJobCategory", "");

            using (var db = new DBContext())
            {
                JobCategories jobCategory = new JobCategories
                {
                    Active = true,
                    CreatedDate = DateTime.Now,
                    JobId = jobCategoryCreateIn.jobId,
                    CategoryId = jobCategoryCreateIn.categoryId,
                    Code = jobCategoryCreateIn.code,
                    Received = false,
                    Send = false,
                    Sending = false,
                    SendingDate = null
                };

                db.JobCategories.Add(jobCategory);
                db.SaveChanges();

                jobCategoryCreateOut.result.jobCategoryId = jobCategory.JobCategoryId;
            }

            registerEventRepository.SaveRegisterEvent(jobCategoryCreateIn.id, jobCategoryCreateIn.key, "Log - End", "Repository.JobCategoryRepository.CreateJobCategory", "");
            return jobCategoryCreateOut;
        }

        public JobCategoriesByJobIdOut GetJobCategoriesByJobId(JobCategoriesByJobIdIn jobCategoryByIdIn)
        {
            JobCategoriesByJobIdOut jobCategoryByIdOut = new JobCategoriesByJobIdOut();
            registerEventRepository.SaveRegisterEvent(jobCategoryByIdIn.id, jobCategoryByIdIn.key, "Log - Start", "Repository.JobCategoryRepository.GetJobCategoriesByJobId", "");

            using (var db = new DBContext())
            {
                jobCategoryByIdOut.result = db.JobCategories
                                              .Where(x => x.Active == true
                                                       && x.DeletedDate == null
                                                       && x.JobId == jobCategoryByIdIn.jobId
                                                       && x.Jobs.Users.AspNetUserId == jobCategoryByIdIn.id)
                                              .Select(x => new JobCategoriesByJobIdVM()
                                              {
                                                  JobCategoryId = x.JobCategoryId,
                                                  category = x.Categories.Code + " - " + x.Categories.Name,
                                                  received = x.Received,
                                                  send = x.Send,
                                                  jobCategoryPages = x.JobCategoryPages
                                                                      .Where(y => y.Active == true && y.DeletedDate == null)
                                                                      .Select(y => new JobCategoryPagesVM()
                                                                      {
                                                                          jobCategoryPageId = y.JobCategoryPageId,
                                                                          page = y.Page,
                                                                          image = "/Images/GetImageScanning/" + x.Hash + "/" + y.Page,
                                                                          thumb = "/Images/GetImageScanning/" + x.Hash + "/" + y.Page + "/true",
                                                                      }).ToList(),
                                                  additionalFields = x.JobCategoryAdditionalFields
                                                                      .Where(y => y.Active == true && y.DeletedDate == null)
                                                                      .Select(y => new AdditionalFieldVM()
                                                                      {
                                                                          categoryAdditionalFieldId = y.CategoryAdditionalFieldId,
                                                                          name = y.CategoryAdditionalFields.AdditionalFields.Name,
                                                                          type = y.CategoryAdditionalFields.AdditionalFields.Type,
                                                                          value = y.Value,
                                                                          single = y.CategoryAdditionalFields.Single,
                                                                          required = y.CategoryAdditionalFields.Required,
                                                                      }).ToList()
                                              })
                                              .OrderBy(x => x.category)
                                              .ToList();
            }

            registerEventRepository.SaveRegisterEvent(jobCategoryByIdIn.id, jobCategoryByIdIn.key, "Log - End", "Repository.JobCategoryRepository.GetJobCategoriesByJobId", "");
            return jobCategoryByIdOut;
        }

        public ECMJobCategoryOut GetECMJobCategoryByHash(string hash)
        {
            ECMJobCategoryOut eCMJobCategoryOut = new ECMJobCategoryOut();
            Guid guid = Guid.Parse(hash);
            string externalId = string.Empty;

            using (var db = new DBContext())
            {
                externalId = db.JobCategories.Where(x => x.Hash == guid).FirstOrDefault().Code;
            }

            eCMJobCategoryOut = jobCategoryApi.GetECMJobCategory(externalId);
            eCMJobCategoryOut.result.hash = hash;

            return eCMJobCategoryOut;
        }

        #endregion
    }
}
