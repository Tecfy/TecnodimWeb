using DataEF.DataAccess;
using Model.In;
using Model.Out;
using Model.VM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace Repository
{
    public partial class SyncRuntimeRepository
    {
        private RegisterEventRepository registerEventRepository = new RegisterEventRepository();

        #region .: API :.

        public SyncRuntimesOut GetSyncRuntimes(SyncRuntimesIn syncRuntimesIn)
        {
            SyncRuntimesOut syncRuntimesOut = new SyncRuntimesOut();
            registerEventRepository.SaveRegisterEvent(syncRuntimesIn.id, syncRuntimesIn.key, "Log - Start", "Repository.SyncRuntimeRepository.GetSyncRuntimes", "");

            using (var db = new DBContext())
            {
                syncRuntimesOut.result = db.SyncRuntimes.Where(x => x.Active == true && x.DeletedDate == null)
                                                .Select(x => new SyncRuntimesVM()
                                                {
                                                    SyncRuntimeId = x.SyncRuntimeId,
                                                    URL = x.Url,
                                                    Interval = x.Interval,
                                                    LastExecution = x.LastExecution
                                                })
                                                .ToList();
            }

            registerEventRepository.SaveRegisterEvent(syncRuntimesIn.id, syncRuntimesIn.key, "Log - End", "Repository.SyncRuntimeRepository.GetSyncRuntimes", "");
            return syncRuntimesOut;
        }

        public bool SaveSyncRuntimes(SyncRuntimeSaveIn syncRuntimeSaveIn)
        {
            registerEventRepository.SaveRegisterEvent(syncRuntimeSaveIn.id, syncRuntimeSaveIn.key, "Log - Start", "Repository.SyncRuntimeRepository.SaveSyncRuntimes", "");

            using (var db = new DBContext())
            {
                SyncRuntimes syncRuntime = db.SyncRuntimes.Where(x => x.SyncRuntimeId == syncRuntimeSaveIn.SyncRuntimeId).FirstOrDefault();
                syncRuntime.LastExecution = DateTime.Now;

                db.Entry(syncRuntime).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }

            registerEventRepository.SaveRegisterEvent(syncRuntimeSaveIn.id, syncRuntimeSaveIn.key, "Log - End", "Repository.SyncRuntimeRepository.SaveSyncRuntimes", "");

            return true;
        }

        #endregion
    }
}
