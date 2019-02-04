using ApiTecnodim;
using DataEF.DataAccess;
using Helper;
using Helper.Enum;
using Model.In;
using Model.Out;
using Model.VM;
using System;
using System.Collections.Generic;
using System.Linq;
using WebSupergoo.ABCpdf11;

namespace Repository
{
    public partial class JobRepository
    {
        private RegisterEventRepository registerEventRepository = new RegisterEventRepository();
        private JobCategoryRepository jobCategoryRepository = new JobCategoryRepository();
        private JobCategoryApi jobCategoryApi = new JobCategoryApi();

        #region .: API :.

        public JobsByRegistrationOut GetJobsByRegistration(JobsByRegistrationIn jobsByRegistrationIn)
        {
            JobsByRegistrationOut jobsByRegistrationOut = new JobsByRegistrationOut();
            registerEventRepository.SaveRegisterEvent(jobsByRegistrationIn.id, jobsByRegistrationIn.key, "Log - Start", "Repository.JobRepository.GetJobsByRegistration", "");

            using (var db = new DBContext())
            {
                jobsByRegistrationOut.result = db.Jobs
                                                .Where(x => x.Active == true
                                                         && x.DeletedDate == null
                                                         && x.Users.Registration == jobsByRegistrationIn.registration
                                                         && x.JobCategories.Count(y => y.Active == true && y.DeletedDate == null && y.Received == false) > 0)
                                                .Select(x => new JobsByRegistrationVM()
                                                {
                                                    JobId = x.JobId,
                                                    Registration = x.Registration,
                                                    Name = x.Name,
                                                    Unity = x.Units.Name,
                                                    Course = x.Course,
                                                    JobCategories = x.JobCategories
                                                                     .Where(y => y.Active == true && y.DeletedDate == null && y.Received == false)
                                                                     .Select(y => new JobCategoriesByRegistrationVM()
                                                                     {
                                                                         JobCategoryId = y.JobCategoryId,
                                                                         Category = y.Categories.Code.Trim() + " - " + y.Categories.Name.Trim(),
                                                                         pb = y.Categories.Pb
                                                                     }).ToList()
                                                })
                                                .ToList();
            }

            registerEventRepository.SaveRegisterEvent(jobsByRegistrationIn.id, jobsByRegistrationIn.key, "Log - End", "Repository.JobRepository.GetJobsByRegistration", "");
            return jobsByRegistrationOut;
        }

        public JobByIdOut GetJobById(JobByIdIn jobByIdIn)
        {
            JobByIdOut jobByIdOut = new JobByIdOut();
            registerEventRepository.SaveRegisterEvent(jobByIdIn.id, jobByIdIn.key, "Log - Start", "Repository.JobRepository.GetJobById", "");

            using (var db = new DBContext())
            {
                jobByIdOut.result = db.Jobs
                                      .Where(x => x.Active == true
                                                  && x.DeletedDate == null
                                                  && x.JobId == jobByIdIn.jobId
                                                  && x.JobCategories.Count(y => y.Active == true && y.DeletedDate == null && y.Received == false) > 0)
                                      .Select(x => new JobByIdVM()
                                      {
                                          JobId = x.JobId,
                                          Registration = x.Registration,
                                          Name = x.Name,
                                          Unity = x.Units.Name,
                                          Course = x.Course,
                                          JobCategories = x.JobCategories
                                                           .Where(y => y.Active == true && y.DeletedDate == null && y.Received == false)
                                                           .Select(y => new JobCategoriesByIdVM()
                                                           {
                                                               JobCategoryId = y.JobCategoryId,
                                                               Category = y.Categories.Code.Trim() + " - " + y.Categories.Name.Trim(),
                                                               pb = y.Categories.Pb
                                                           }).ToList()
                                      })
                                      .FirstOrDefault();
            }

            registerEventRepository.SaveRegisterEvent(jobByIdIn.id, jobByIdIn.key, "Log - End", "Repository.JobRepository.GetJobById", "");
            return jobByIdOut;
        }

        public JobsByUserOut GetJobsByUser(JobsByUserIn jobsByUserIn)
        {
            JobsByUserOut jobsByUserOut = new JobsByUserOut();
            registerEventRepository.SaveRegisterEvent(jobsByUserIn.id, jobsByUserIn.key, "Log - Start", "Repository.JobRepository.GetJobsByUser", "");

            using (var db = new DBContext())
            {
                jobsByUserOut.result = db.Jobs
                                         .Where(x => x.Active == true
                                                    && x.DeletedDate == null
                                                    && jobsByUserIn.jobStatusIds.Contains(x.JobStatusId)
                                                    && x.Users.AspNetUserId == jobsByUserIn.id)
                                         .Select(x => new JobsByUserVM()
                                         {
                                             JobId = x.JobId,
                                             Registration = x.Registration,
                                             Name = x.Name,
                                             Unity = x.Units.Name,
                                             Course = x.Course,
                                             Status = x.JobStatus.Name
                                         })
                                         .ToList();
            }

            registerEventRepository.SaveRegisterEvent(jobsByUserIn.id, jobsByUserIn.key, "Log - End", "Repository.JobRepository.GetJobsByUser", "");
            return jobsByUserOut;
        }

        public JobCreateOut CreateJob(JobCreateIn jobsCreateIn)
        {
            JobCreateOut jobCreateOut = new JobCreateOut();

            registerEventRepository.SaveRegisterEvent(jobsCreateIn.id, jobsCreateIn.key, "Log - Start", "Repository.JobRepository.CreateJob", "");

            using (var db = new DBContext())
            {
                Jobs job = new Jobs
                {
                    Active = true,
                    CreatedDate = DateTime.Now,
                    UserId = jobsCreateIn.userId,
                    JobStatusId = (int)EJobStatus.New,
                    Registration = jobsCreateIn.registration,
                    Name = jobsCreateIn.name,
                    Sent = false,
                    UnityId = jobsCreateIn.unityId,
                    Course = jobsCreateIn.course
                };

                db.Jobs.Add(job);
                db.SaveChanges();

                jobCreateOut.result.jobId = job.JobId;
            }

            registerEventRepository.SaveRegisterEvent(jobsCreateIn.id, jobsCreateIn.key, "Log - End", "Repository.JobRepository.CreateJob", "");
            return jobCreateOut;
        }

        public JobDeleteOut DeleteJob(JobDeleteIn jobDeleteIn)
        {
            JobDeleteOut jobDeleteOut = new JobDeleteOut();

            registerEventRepository.SaveRegisterEvent(jobDeleteIn.id, jobDeleteIn.key, "Log - Start", "Repository.JobRepository.DeleteJob", "");

            using (var db = new DBContext())
            {
                Jobs job = db.Jobs.Where(x => x.JobId == jobDeleteIn.jobId && x.Users.AspNetUserId == jobDeleteIn.id).FirstOrDefault();

                if (job == null)
                {
                    throw new Exception(i18n.Resource.RegisterNotFound);
                }

                job.Active = false;
                job.DeletedDate = DateTime.Now;

                db.Entry(job).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }

            registerEventRepository.SaveRegisterEvent(jobDeleteIn.id, jobDeleteIn.key, "Log - End", "Repository.JobRepository.DeleteJob", "");
            return jobDeleteOut;
        }

        public ECMJobsSendOut GetECMSendJobs(ECMJobsSendIn eCMJobsSendIn)
        {
            ECMJobsSendOut eCMJobsSendOut = new ECMJobsSendOut();
            registerEventRepository.SaveRegisterEvent(eCMJobsSendIn.id, eCMJobsSendIn.key, "Log - Start", "Repository.JobRepository.GetECMSendJobs", "");

            #region .: Search Documents Finished :.

            JobsFinishedOut jobsFinishedOut = GetJobsFinished(new JobsFinishedIn() { id = eCMJobsSendIn.id, key = eCMJobsSendIn.key });

            #endregion

            #region .: Process Queue :.

            foreach (var item in jobsFinishedOut.result)
            {
                try
                {
                    JobCategoryProcess(item);
                }
                catch (Exception ex)
                {
                    eCMJobsSendOut.messages.Add(ex.Message);
                }
            }

            #endregion

            #region .: Search Jobs Sent :.

            JobsSentOut jobsSentOut = GetJobsSent(new JobsSentIn() { id = eCMJobsSendIn.id, key = eCMJobsSendIn.key });

            #endregion

            #region .: Update Jobs :.

            foreach (var item in jobsSentOut.result)
            {
                using (var db = new DBContext())
                {
                    Jobs job = db.Jobs.Find(item.jobId);

                    job.JobStatusId = (int)EJobStatus.Sent;
                    job.EditedDate = DateTime.Now;

                    db.Entry(job).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
            }

            #endregion

            registerEventRepository.SaveRegisterEvent(eCMJobsSendIn.id, eCMJobsSendIn.key, "Log - End", "Repository.JobRepository.GetECMSendJobs", "");
            return eCMJobsSendOut;
        }

        #endregion

        #region .: Local :.

        public JobsFinishedOut GetJobsFinished(JobsFinishedIn jobsFinishedIn)
        {
            JobsFinishedOut jobsFinishedOut = new JobsFinishedOut();
            registerEventRepository.SaveRegisterEvent(jobsFinishedIn.id, jobsFinishedIn.key, "Log - Start", "Repository.JobRepository.GetJobsFinished", "");

            #region .: Documents Finished :.

            using (var db = new DBContext())
            {
                jobsFinishedOut.result = db.JobCategories
                                           .Where(x => x.Active == true
                                               && x.DeletedDate == null
                                               && x.Sent == false
                                               && x.Sending == false
                                               && x.Jobs.Active == true
                                               && x.Jobs.DeletedDate == null
                                               && x.Jobs.JobStatusId == (int)EJobStatus.Finished)
                                           .Select(x => new JobsFinishedVM()
                                           {
                                               jobId = x.JobId,
                                               jobCategoryId = x.JobCategoryId,
                                               externalId = x.Code,
                                               registration = x.Jobs.Registration,
                                               categoryId = x.Categories.Code,
                                               category = x.Categories.Name,
                                               pb = x.Categories.Pb,
                                               title = x.Categories.Name + ".pdf",
                                               additionalFields = x.JobCategoryAdditionalFields
                                                                   .Where(y => y.Active == true && y.DeletedDate == null)
                                                                   .Select(y => new AdditionalFieldSaveVM()
                                                                   {
                                                                       additionalFieldId = y.CategoryAdditionalFields.AdditionalFieldId,
                                                                       value = y.Value,
                                                                   }).ToList()
                                           })
                                           .ToList();
            }

            #endregion

            registerEventRepository.SaveRegisterEvent(jobsFinishedIn.id, jobsFinishedIn.key, "Log - End", "Repository.JobRepository.GetJobsFinished", "");
            return jobsFinishedOut;
        }

        public JobsSentOut GetJobsSent(JobsSentIn jobsSentIn)
        {
            JobsSentOut jobsSentOut = new JobsSentOut();
            registerEventRepository.SaveRegisterEvent(jobsSentIn.id, jobsSentIn.key, "Log - Start", "Repository.JobRepository.GetJobsSent", "");

            #region .: Documents Finished :.

            using (var db = new DBContext())
            {
                jobsSentOut.result = db.Jobs
                                       .Where(x => x.Active == true
                                               && x.DeletedDate == null
                                               && x.JobStatusId == (int)EJobStatus.Finished
                                               && (x.JobCategories.Count(y => y.Active == true && y.DeletedDate == null) == x.JobCategories.Count(y => y.Active == true && y.DeletedDate == null && y.Sent == true)))
                                       .Select(x => new JobsSentVM()
                                       {
                                           jobId = x.JobId,
                                       })
                                       .ToList();
            }

            #endregion

            registerEventRepository.SaveRegisterEvent(jobsSentIn.id, jobsSentIn.key, "Log - End", "Repository.JobRepository.GetJobsSent", "");
            return jobsSentOut;
        }

        #endregion

        #region .: Helper :.

        private void JobCategoryProcess(JobsFinishedVM jobsFinishedVM)
        {
            JobCategories jobCategories = new JobCategories();

            try
            {
                #region .: Validate Job Categories :.

                using (var db = new DBContext())
                {
                    jobCategories = db.JobCategories
                                      .Where(x => x.Send == true
                                               && x.Sent == false
                                               && x.Sending == false
                                               && x.JobCategoryId == jobsFinishedVM.jobCategoryId)
                                      .FirstOrDefault();

                    if (jobCategories == null)
                    {
                        throw new Exception(string.Format(i18n.Resource.JobCategoryNoProcess, jobsFinishedVM.jobCategoryId));
                    }

                    jobCategories.Sending = true;
                    jobCategories.SendingDate = DateTime.Now;

                    db.Entry(jobCategories).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }

                #endregion

                #region .: Search Job Categories Original :.

                ECMJobCategoryOut eCMJobCategoryOut = jobCategoryApi.GetECMJobCategory(jobsFinishedVM.externalId);

                if (!eCMJobCategoryOut.success)
                {
                    throw new Exception(eCMJobCategoryOut.messages.FirstOrDefault());
                }

                #endregion

                #region .: Archive :.

                PDFIn pdfIn = new PDFIn
                {
                    archive = eCMJobCategoryOut.result.archive,
                    pb = jobsFinishedVM.pb
                };

                string file = HelperDoc(pdfIn);

                if (string.IsNullOrEmpty(file))
                {
                    throw new Exception(i18n.Resource.FileNotFound);
                }

                #endregion

                #region .: Sent Document :.

                List<AdditionalFieldSaveIn> additionalFieldSaveIns = new List<AdditionalFieldSaveIn>();

                if (jobsFinishedVM.additionalFields != null && jobsFinishedVM.additionalFields.Count() > 0)
                {
                    foreach (var item in jobsFinishedVM.additionalFields)
                    {
                        additionalFieldSaveIns.Add(new AdditionalFieldSaveIn { additionalFieldId = item.additionalFieldId, value = item.value });
                    }
                }

                ECMJobSaveIn eCMJobSaveIn = new ECMJobSaveIn
                {
                    registration = jobsFinishedVM.registration,
                    categoryId = jobsFinishedVM.categoryId,
                    category = jobsFinishedVM.category,
                    archive = file,
                    title = jobsFinishedVM.title,
                    additionalFields = additionalFieldSaveIns
                };

                ECMJobSaveOut eCMJobSaveOut = jobCategoryApi.PostECMJobSave(eCMJobSaveIn);

                if (!eCMJobSaveOut.success)
                {
                    throw new Exception(eCMJobCategoryOut.messages.FirstOrDefault());
                }

                #endregion

                #region .: Update Job Category :.

                using (var db = new DBContext())
                {
                    jobCategories.EditedDate = DateTime.Now;
                    jobCategories.Sent = true;
                    jobCategories.Sending = false;
                    jobCategories.SendingDate = DateTime.Now;

                    db.Entry(jobCategories).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }

                try
                {
                    ECMJobDeletedIn eCMJobDeletedIn = new ECMJobDeletedIn() { externalId = jobsFinishedVM.externalId };

                    jobCategoryApi.DeleteECMJobArchive(eCMJobDeletedIn);
                }
                catch { }

                #endregion
            }
            catch (Exception ex)
            {
                if (jobCategories != null)
                {
                    using (var db = new DBContext())
                    {
                        jobCategories.Sending = false;

                        db.Entry(jobCategories).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                    }
                }

                throw new Exception(ex.Message);
            }
        }

        public string HelperDoc(PDFIn pdfIn)
        {
            string archive = string.Empty;

            Doc docOld = new Doc();
            Doc docNew = new Doc();
            docOld.Read(Convert.FromBase64String(pdfIn.archive));

            //if (pdfIn.pb)
            //{
            //    docNew = PB.Converter(docOld);

            //    archive = System.Convert.ToBase64String(docNew.GetData());
            //}
            //else
            //{
            archive = System.Convert.ToBase64String(docOld.GetData());
            //}

            docOld.Clear();
            docNew.Clear();

            return archive;
        }

        #endregion
    }
}
