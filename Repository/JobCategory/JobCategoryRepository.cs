using DataEF.DataAccess;
using Helper.Enum;
using Helper.ServerMap;
using Model.In;
using Model.Out;
using Model.VM;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Configuration;
using WebSupergoo.ABCpdf11;

namespace Repository
{
    public partial class JobCategoryRepository
    {
        private RegisterEventRepository registerEventRepository = new RegisterEventRepository();
        private CategoryAdditionalFieldRepository categoryAdditionalFieldRepository = new CategoryAdditionalFieldRepository();
        private JobCategoryAdditionalFieldRepository jobCategoryAdditionalFieldRepository = new JobCategoryAdditionalFieldRepository();
        private JobStatusRepository jobStatusRepository = new JobStatusRepository();

        #region .: API :.

        public JobCategoriesByJobIdOut GetJobCategoriesByJobId(JobCategoriesByJobIdIn jobCategoryByIdIn)
        {
            JobCategoriesByJobIdOut jobCategoryByIdOut = new JobCategoriesByJobIdOut();
            registerEventRepository.SaveRegisterEvent(jobCategoryByIdIn.id, jobCategoryByIdIn.key, "Log - Start", "Repository.JobCategoryRepository.GetJobCategoriesByJobId", "");
            string path = WebConfigurationManager.AppSettings["UrlBase"];
            string now = DateTime.Now.ToString("yyyyMMddHHmmsss");

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
                                                                          image = path + "/Files/ScanningPages/" + x.Hash + "/Images/" + y.Page + ".jpg?" + now,
                                                                          thumb = path + "/Files/ScanningPages/" + x.Hash + "/Thumbs/" + y.Page + ".jpg?" + now,
                                                                      }).ToList(),
                                                  additionalFields = x.JobCategoryAdditionalFields
                                                                      .Where(y => y.Active == true && y.DeletedDate == null)
                                                                      .Select(y => new JobCategoryAdditionalFieldVM()
                                                                      {
                                                                          jobCategoryAdditionalFieldId = y.JobCategoryAdditionalFieldId,
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

            foreach (var item in jobCategoryByIdOut.result)
            {
                using (var db = new DBContext())
                {
                    JobCategories jobCategory = db.JobCategories.Where(x => x.JobCategoryId == item.JobCategoryId).FirstOrDefault();

                    if (!jobCategory.Download && jobCategory.Received)
                    {
                        ExistPDFs(jobCategory.JobCategoryId, jobCategory.Code, jobCategory.Hash.ToString(), jobCategoryByIdIn.id, jobCategoryByIdIn.key);
                    }
                }
            }

            registerEventRepository.SaveRegisterEvent(jobCategoryByIdIn.id, jobCategoryByIdIn.key, "Log - End", "Repository.JobCategoryRepository.GetJobCategoriesByJobId", "");
            return jobCategoryByIdOut;
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
                    Sent = false,
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

        public JobCategoryArchiveOut SaveJobCategory(JobCategoryArchiveIn jobCategorySaveIn)
        {
            JobCategoryArchiveOut jobCategoryOut = new JobCategoryArchiveOut();

            registerEventRepository.SaveRegisterEvent(jobCategorySaveIn.id, jobCategorySaveIn.key, "Log - Start", "Repository.JobCategoryRepository.SaveJobCategory", "");

            #region .: Job Category :.

            string code = string.Empty;

            using (var db = new DBContext())
            {
                code = db.JobCategories.Where(x => x.DeletedDate == null && x.Active == true && x.JobCategoryId == jobCategorySaveIn.jobCategoryId).FirstOrDefault()?.Code;

                if (string.IsNullOrEmpty(code))
                {
                    throw new Exception(i18n.Resource.NoDataFound);
                }
            }

            #endregion

            #region .: Sent New Document :.

            SaveArchive(Convert.FromBase64String(jobCategorySaveIn.archive), code, jobCategorySaveIn.id, jobCategorySaveIn.key);

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

                #region .: Change job status :.

                Jobs job = db.Jobs.Where(x => x.JobId == jobCategory.JobId).FirstOrDefault();

                if (job.JobStatusId == (int)EJobStatus.New)
                {
                    jobStatusRepository.StatusJob(new JobStatusIn { id = job.Users.AspNetUserId, key = jobCategorySaveIn.key, jobId = job.JobId, jobStatusId = (int)EJobStatus.PartiallyDigitalized });
                }

                bool received = db.Jobs.Any(x => x.JobId == job.JobId && (x.JobCategories.Count(y => y.Active == true && y.DeletedDate == null) == x.JobCategories.Count(y => y.Active == true && y.DeletedDate == null && y.Received == true)));

                if (job.JobStatusId == (int)EJobStatus.PartiallyDigitalized && received)
                {
                    jobStatusRepository.StatusJob(new JobStatusIn { id = job.Users.AspNetUserId, key = jobCategorySaveIn.key, jobId = job.JobId, jobStatusId = (int)EJobStatus.Digitalized });
                }

                #endregion
            }

            #endregion

            #region .: Save file pages :.

            SavePDFs(jobCategorySaveIn.jobCategoryId, jobCategorySaveIn.id, jobCategorySaveIn.key, null, jobCategorySaveIn.archive);

            #endregion

            registerEventRepository.SaveRegisterEvent(jobCategorySaveIn.id, jobCategorySaveIn.key, "Log - End", "Repository.JobCategoryRepository.SaveJobCategory", "");
            return jobCategoryOut;
        }

        public JobCategoryDisapproveOut DisapproveJobCategory(JobCategoryDisapproveIn jobCategoryDisapproveIn)
        {
            JobCategoryDisapproveOut jobCategoryDisapproveOut = new JobCategoryDisapproveOut();
            JobCategories jobCategory = new JobCategories();

            registerEventRepository.SaveRegisterEvent(jobCategoryDisapproveIn.id, jobCategoryDisapproveIn.key, "Log - Start", "Repository.JobCategoryRepository.DisapproveJobCategory", "");

            #region .: Job Category :.

            using (var db = new DBContext())
            {
                #region .: Change job category :.

                jobCategory = db.JobCategories.Where(x => x.JobCategoryId == jobCategoryDisapproveIn.jobCategoryId && x.Jobs.Users.AspNetUserId == jobCategoryDisapproveIn.id).FirstOrDefault();

                if (jobCategory == null)
                {
                    throw new Exception(i18n.Resource.NoDataFound);
                }

                jobCategory.EditedDate = DateTime.Now;
                jobCategory.Received = false;
                jobCategory.Send = false;
                jobCategory.Sent = false;
                jobCategory.Sending = false;
                jobCategory.SendingDate = null;

                db.Entry(jobCategory).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                #endregion

                #region .: Change job category pages :.

                List<JobCategoryPages> jobCategoryPages = db.JobCategoryPages.Where(x => x.DeletedDate == null && x.Active == true && x.JobCategoryId == jobCategory.JobCategoryId).ToList();

                foreach (var item in jobCategoryPages)
                {
                    item.Active = false;
                    item.DeletedDate = DateTime.Now;

                    db.Entry(item).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }

                #endregion

                #region .: Change job status :.

                Jobs job = db.Jobs.Where(x => x.JobId == jobCategory.JobId).FirstOrDefault();

                if (job.JobStatusId == (int)EJobStatus.Digitalized)
                {
                    jobStatusRepository.StatusJob(new JobStatusIn { id = jobCategoryDisapproveIn.id, key = jobCategoryDisapproveIn.key, jobId = job.JobId, jobStatusId = (int)EJobStatus.PartiallyDigitalized });
                }

                #endregion
            }

            #endregion

            #region .: Delete Document :.

            string path = ServerMapHelper.GetServerMap(WebConfigurationManager.AppSettings["Path.Files"]);
            string name = jobCategory.Code + ".pdf";
            string pathImages = Path.Combine(path, "ScanningPages", "{0}");
            string pathFile = Path.Combine(path, "JobCategories", name);

            try
            {
                if (File.Exists(string.Format(pathFile)))
                {
                    File.Delete(string.Format(pathFile));
                }

                if (Directory.Exists(string.Format(pathImages, jobCategory.Hash)))
                {
                    Directory.Delete(string.Format(pathImages, jobCategory.Hash), true);
                }
            }
            catch (Exception ex)
            {
                registerEventRepository.SaveRegisterEvent(jobCategoryDisapproveIn.id, jobCategoryDisapproveIn.key, "Erro", "Repository.JobCategoryRepository.DisapproveJobCategory", string.Format("Delete File Source: {0}.\n InnerException: {1}.\n Message: {2}", ex.Source, ex.InnerException, ex.Message));
            }

            #endregion

            registerEventRepository.SaveRegisterEvent(jobCategoryDisapproveIn.id, jobCategoryDisapproveIn.key, "Log - End", "Repository.JobCategoryRepository.DisapproveJobCategory", "");
            return jobCategoryDisapproveOut;
        }

        public JobCategoryApproveOut ApproveJobCategory(JobCategoryApproveIn jobCategoryApproveIn)
        {
            JobCategoryApproveOut jobCategoryApproveOut = new JobCategoryApproveOut();

            registerEventRepository.SaveRegisterEvent(jobCategoryApproveIn.id, jobCategoryApproveIn.key, "Log - Start", "Repository.JobCategoryRepository.ApproveJobCategory", "");

            #region .: Job Category :.

            using (var db = new DBContext())
            {
                JobCategories jobCategory = db.JobCategories
                                              .Where(x => x.JobCategoryId == jobCategoryApproveIn.jobCategoryId
                                                       && x.Jobs.Users.AspNetUserId == jobCategoryApproveIn.id)
                                              .FirstOrDefault();

                if (jobCategory == null)
                {
                    throw new Exception(i18n.Resource.NoDataFound);
                }

                #region .: AdditionalFields :.

                foreach (var item in jobCategoryApproveIn.additionalFields)
                {
                    JobCategoryAdditionalFieldUpdateIn jobCategoryAdditionalFieldUpdateIn = new JobCategoryAdditionalFieldUpdateIn()
                    {
                        key = jobCategoryApproveIn.key,
                        id = jobCategoryApproveIn.id,
                        jobCategoryAdditionalFieldId = item.jobCategoryAdditionalFieldId,
                        value = item.value
                    };

                    jobCategoryAdditionalFieldRepository.UpdateJobCategoryAdditionalField(jobCategoryAdditionalFieldUpdateIn);
                }

                #endregion

                jobCategory.EditedDate = DateTime.Now;
                jobCategory.Send = true;

                db.Entry(jobCategory).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }

            #endregion

            registerEventRepository.SaveRegisterEvent(jobCategoryApproveIn.id, jobCategoryApproveIn.key, "Log - End", "Repository.JobCategoryRepository.ApproveJobCategory", "");
            return jobCategoryApproveOut;
        }

        public JobCategoryDeletedOut DeletedJobCategory(JobCategoryDeletedIn jobCategoryDeletedIn)
        {
            JobCategoryDeletedOut jobCategoryDeletedOut = new JobCategoryDeletedOut();
            JobCategories jobCategory = new JobCategories();

            registerEventRepository.SaveRegisterEvent(jobCategoryDeletedIn.id, jobCategoryDeletedIn.key, "Log - Start", "Repository.JobCategoryRepository.DeletedJobCategory", "");

            #region .: Job Category :.

            using (var db = new DBContext())
            {
                #region .: Change job category :.

                jobCategory = db.JobCategories.Where(x => x.JobCategoryId == jobCategoryDeletedIn.jobCategoryId && x.Jobs.Users.AspNetUserId == jobCategoryDeletedIn.id).FirstOrDefault();

                if (jobCategory == null)
                {
                    throw new Exception(i18n.Resource.NoDataFound);
                }

                jobCategory.Active = false;
                jobCategory.DeletedDate = DateTime.Now;

                db.Entry(jobCategory).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                #endregion

                #region .: Change job category pages :.

                List<JobCategoryPages> jobCategoryPages = db.JobCategoryPages.Where(x => x.DeletedDate == null && x.Active == true && x.JobCategoryId == jobCategory.JobCategoryId).ToList();

                foreach (var item in jobCategoryPages)
                {
                    item.Active = false;
                    item.DeletedDate = DateTime.Now;

                    db.Entry(item).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }

                #endregion

                #region .: Change job category additional fields :.

                List<JobCategoryAdditionalFields> jobCategoryAdditionalFields = db.JobCategoryAdditionalFields.Where(x => x.DeletedDate == null && x.Active == true && x.JobCategoryId == jobCategory.JobCategoryId).ToList();

                foreach (var item in jobCategoryAdditionalFields)
                {
                    item.Active = false;
                    item.DeletedDate = DateTime.Now;

                    db.Entry(item).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }

                #endregion

                #region .: Change job status :.

                Jobs job = db.Jobs.Where(x => x.JobId == jobCategory.JobId).FirstOrDefault();

                bool received = db.Jobs.Any(x => x.JobId == job.JobId
                                             && (x.JobCategories.Count(y => y.Active == true && y.DeletedDate == null) == x.JobCategories.Count(y => y.Active == true && y.DeletedDate == null && y.Received == true)));

                if ((job.JobStatusId == (int)EJobStatus.PartiallyDigitalized || job.JobStatusId == (int)EJobStatus.New) && received)
                {
                    jobStatusRepository.StatusJob(new JobStatusIn { id = jobCategoryDeletedIn.id, key = jobCategoryDeletedIn.key, jobId = job.JobId, jobStatusId = (int)EJobStatus.Digitalized });
                }

                #endregion
            }

            #endregion

            #region .: Delete Document :.

            string path = ServerMapHelper.GetServerMap(WebConfigurationManager.AppSettings["Path.Files"]);
            string name = jobCategory.Code + ".pdf";
            string pathImages = Path.Combine(path, "ScanningPages", "{0}");
            string pathFile = Path.Combine(path, "JobCategories", name);

            try
            {
                if (File.Exists(string.Format(pathFile)))
                {
                    File.Delete(string.Format(pathFile));
                }

                if (Directory.Exists(string.Format(pathImages, jobCategory.Hash)))
                {
                    Directory.Delete(string.Format(pathImages, jobCategory.Hash), true);
                }
            }
            catch (Exception ex)
            {
                registerEventRepository.SaveRegisterEvent(jobCategoryDeletedIn.id, jobCategoryDeletedIn.key, "Erro", "Repository.JobCategoryRepository.DeletedJobCategory", string.Format("Delete File Source: {0}.\n InnerException: {1}.\n Message: {2}", ex.Source, ex.InnerException, ex.Message));
            }

            #endregion

            registerEventRepository.SaveRegisterEvent(jobCategoryDeletedIn.id, jobCategoryDeletedIn.key, "Log - End", "Repository.JobCategoryRepository.DeletedJobCategory", "");
            return jobCategoryDeletedOut;
        }

        public JobCategoryIncludeOut IncludeJobCategory(JobCategoryIncludeIn jobCategoryIncludeIn)
        {
            JobCategoryIncludeOut jobCategoryIncludeOut = new JobCategoryIncludeOut();
            JobCategoryCreateOut jobCategoryCreateOut = new JobCategoryCreateOut();

            registerEventRepository.SaveRegisterEvent(jobCategoryIncludeIn.id, jobCategoryIncludeIn.key, "Log - Start", "Repository.JobCategoryRepository.IncludeJobCategory", "");

            #region .: Job Category :.

            using (var db = new DBContext())
            {
                JobCategories jobCategory = db.JobCategories
                                              .Where(x => x.Active == true
                                                       && x.DeletedDate == null
                                                       && x.CategoryId == jobCategoryIncludeIn.categoryId
                                                       && x.JobId == jobCategoryIncludeIn.jobId
                                                       && x.Jobs.Users.AspNetUserId == jobCategoryIncludeIn.id)
                                              .FirstOrDefault();

                if (jobCategory != null)
                {
                    throw new Exception(i18n.Resource.ExistingRegistry);
                }

                Jobs job = db.Jobs
                             .Where(x => x.Active == true
                                      && x.DeletedDate == null
                                      && x.JobId == jobCategoryIncludeIn.jobId
                                      && x.Users.AspNetUserId == jobCategoryIncludeIn.id)
                             .FirstOrDefault();

                if (job == null)
                {
                    throw new Exception(i18n.Resource.NoDataFound);
                }

                string categoryCode = string.Empty;

                categoryCode = db.Categories.Where(x => x.CategoryId == jobCategoryIncludeIn.categoryId).FirstOrDefault().Code;

                var justDigits = new Regex(@"[^\d]");
                categoryCode = justDigits.Replace(categoryCode, "");

                JobCategoryCreateIn jobCategoryCreateIn = new JobCategoryCreateIn()
                {
                    key = jobCategoryIncludeIn.id,
                    id = jobCategoryIncludeIn.key,
                    jobId = jobCategoryIncludeIn.jobId,
                    categoryId = jobCategoryIncludeIn.categoryId,
                    code = DateTime.Now.ToString("yyyyMMddHHmmsss") + "-" + job.Registration + "-" + categoryCode
                };

                jobCategoryCreateOut = CreateJobCategory(jobCategoryCreateIn);

                #region .: AdditionalFields :.

                CategoryAdditionalFieldsIn categoryAdditionalFieldsIn = new CategoryAdditionalFieldsIn { key = jobCategoryIncludeIn.key, id = jobCategoryIncludeIn.id, categoryId = jobCategoryIncludeIn.categoryId };

                CategoryAdditionalFieldsOut categoryAdditionalFieldsOut = categoryAdditionalFieldRepository.GetCategoryAdditionalFieldsByCategoryId(categoryAdditionalFieldsIn);

                foreach (var categoryAdditionalField in categoryAdditionalFieldsOut.result)
                {
                    JobCategoryAdditionalFieldIn jobCategoryAdditionalFieldIn = new JobCategoryAdditionalFieldIn()
                    {
                        key = jobCategoryIncludeIn.key,
                        id = jobCategoryIncludeIn.id,
                        jobCategoryId = jobCategoryCreateOut.result.jobCategoryId,
                        categoryAdditionalFieldId = categoryAdditionalField.categoryAdditionalFieldId,
                    };

                    jobCategoryAdditionalFieldRepository.SaveJobCategoryAdditionalField(jobCategoryAdditionalFieldIn);
                }

                #endregion

                #region .: Change job status :.

                bool received = db.Jobs.Any(x => x.JobId == job.JobId
                                             && (x.JobCategories.Count(y => y.Active == true && y.DeletedDate == null) == x.JobCategories.Count(y => y.Active == true && y.DeletedDate == null && y.Received == true)));

                if (job.JobStatusId == (int)EJobStatus.Digitalized && !received)
                {
                    jobStatusRepository.StatusJob(new JobStatusIn { id = jobCategoryIncludeIn.id, key = jobCategoryIncludeIn.key, jobId = job.JobId, jobStatusId = (int)EJobStatus.PartiallyDigitalized });
                }

                #endregion
            }

            #endregion

            registerEventRepository.SaveRegisterEvent(jobCategoryIncludeIn.id, jobCategoryIncludeIn.key, "Log - End", "Repository.JobCategoryRepository.IncludeJobCategory", "");
            return jobCategoryIncludeOut;
        }

        #endregion

        #region .: Helper :.

        private void SavePDFs(int jobCategoryId, string id, string key, string pathArchive = null, string archive = null)
        {
            JobCategories jobCategory = new JobCategories();

            using (var db = new DBContext())
            {
                jobCategory = db.JobCategories.Where(x => x.JobCategoryId == jobCategoryId).FirstOrDefault();

                if (jobCategory == null)
                {
                    registerEventRepository.SaveRegisterEvent(id, key, "Erro", "Repository.JobCategoryRepository.ValidPDFs", string.Format("JobCategoryId: {0}", jobCategoryId));

                    throw new Exception(i18n.Resource.RegisterNotFound);
                }

                jobCategory.Download = true;
                jobCategory.DownloadDate = DateTime.Now;

                db.Entry(jobCategory).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }

            try
            {
                Doc theDoc = new Doc();
                if (!string.IsNullOrEmpty(archive))
                {
                    theDoc.Read(Convert.FromBase64String(archive));
                }
                else
                {
                    theDoc.Read(pathArchive);
                }

                string path = ServerMapHelper.GetServerMap(WebConfigurationManager.AppSettings["Path.Files"]);
                string pathImages = Path.Combine(path, "ScanningPages", jobCategory.Hash.ToString(), "Images");
                string pathThumb = Path.Combine(path, "ScanningPages", jobCategory.Hash.ToString(), "Thumbs");
                int dpi = 100;
                int.TryParse(WebConfigurationManager.AppSettings["DPI"], out dpi);

                if (!Directory.Exists(pathImages))
                {
                    Directory.CreateDirectory(pathImages);
                }

                if (!Directory.Exists(pathThumb))
                {
                    Directory.CreateDirectory(pathThumb);
                }

                jobCategory.Pages = theDoc.PageCount;

                for (int i = 1; i <= theDoc.PageCount; i++)
                {
                    theDoc.PageNumber = i;
                    theDoc.Rect.Resize(theDoc.MediaBox.Width, theDoc.MediaBox.Height);
                    theDoc.Rendering.DotsPerInch = dpi;
                    theDoc.Rendering.GetBitmap().Save(Path.Combine(pathImages, i + ".jpg"));
                    var bmp = theDoc.Rendering.GetBitmap();

                    HelperImage.Resize((Image)bmp, Path.Combine(pathThumb, i + ".jpg"), 200, HelperImage.TypeSize.Width, 342, 80, false, HelperImage.TypeImage.JPG, Color.White);
                }

                theDoc.Clear();

                using (var db = new DBContext())
                {
                    jobCategory.Download = false;

                    db.Entry(jobCategory).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                using (var db = new DBContext())
                {
                    jobCategory.Download = false;

                    db.Entry(jobCategory).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
                registerEventRepository.SaveRegisterEvent(id, key, "Erro", "Repository.JobCategoryRepository.ValidPDFs", string.Format("Source: {0}.\n InnerException: {1}.\n Message: {2}", ex.Source, ex.InnerException, ex.Message));

                throw new Exception(i18n.Resource.UnknownError);
            }
        }

        public bool ExistPDFs(int jobCategoryId, string externalId, string hash, string id, string key)
        {
            bool @return = false;

            #region .: Valid Files :.

            string path = ServerMapHelper.GetServerMap(WebConfigurationManager.AppSettings["Path.Files"]);
            string pathImages = Path.Combine(path, "ScanningPages", hash.ToString(), "Images");
            string pathThumb = Path.Combine(path, "ScanningPages", hash.ToString(), "Thumbs");

            if (Directory.GetFiles(pathImages).Length > 0 && Directory.GetFiles(pathThumb).Length > 0)
            {
                @return = true;
            }
            else
            {
                @return = false;
            }

            #endregion

            return @return;
        }

        private bool SaveArchive(byte[] archive, string externalId, string id, string key)
        {
            try
            {
                registerEventRepository.SaveRegisterEvent(id, key, "Log - Start", "Repository.JobCategoryRepository.SaveJobCategory", "");

                string path = ServerMapHelper.GetServerMap(WebConfigurationManager.AppSettings["Path.Files"]);
                string fileName = externalId + ".pdf";
                string pathFile = Path.Combine(path, "JobCategories", fileName);

                if (File.Exists(pathFile))
                {
                    File.Delete(pathFile);
                }

                File.WriteAllBytes(pathFile, archive);

                registerEventRepository.SaveRegisterEvent(id, key, "Log - End", "Repository.JobCategoryRepository.SaveJobCategory", "");

                return true;
            }
            catch (Exception ex)
            {
                registerEventRepository.SaveRegisterEvent(id, key, "Erro", "Repository.JobCategoryRepository.SaveJobCategory", string.Format("Source: {0}.\n InnerException: {1}.\n Message: {2}", ex.Source, ex.InnerException, ex.Message));

                throw new Exception(i18n.Resource.UnknownError);
            }
        }

        #endregion
    }
}
