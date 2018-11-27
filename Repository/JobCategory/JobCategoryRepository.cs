﻿using ApiTecnodim;
using DataEF.DataAccess;
using Model.In;
using Model.Out;
using System;
using System.Linq;

namespace Repository
{
    public partial class JobCategoryRepository
    {
        RegisterEventRepository registerEventRepository = new RegisterEventRepository();
        JobCategoryApi jobCategoryApi = new JobCategoryApi();

        #region .: API :.

        public JobCategorySaveOut SetJobCategorySave(JobCategorySaveIn jobCategorySaveIn)
        {
            JobCategorySaveOut jobCategoryOut = new JobCategorySaveOut();

            registerEventRepository.SaveRegisterEvent(jobCategorySaveIn.userId, jobCategorySaveIn.key, "Log - Start", "Repository.JobCategoryRepository.SetJobCategorySave", "");

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

            registerEventRepository.SaveRegisterEvent(jobCategorySaveIn.userId, jobCategorySaveIn.key, "Log - End", "Repository.JobCategoryRepository.SetJobCategorySave", "");
            return jobCategoryOut;
        }

        #endregion
    }
}