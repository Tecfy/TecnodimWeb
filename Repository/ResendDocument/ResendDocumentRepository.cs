using ApiTecnodim;
using DataEF.DataAccess;
using Helper;
using Helper.Enum;
using Helper.ServerMap;
using Model.In;
using Model.Out;
using Model.VM;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web.Configuration;
using WebSupergoo.ABCpdf11;

namespace Repository
{
    public partial class ResendDocumentRepository
    {
        private RegisterEventRepository registerEventRepository = new RegisterEventRepository();
        private ResendDocumentApi resendDocumentApi = new ResendDocumentApi();

        #region .: API :.

        public ResendDocumentsOut GetResendDocuments(ResendDocumentsIn resendDocumentsIn)
        {
            ResendDocumentsOut resendDocumentsOut = new ResendDocumentsOut();

            registerEventRepository.SaveRegisterEvent(resendDocumentsIn.id, resendDocumentsIn.key, "Log - Start", "Repository.ResendDocumentRepository.GetResendDocuments", "");

            string uninyCode = string.Empty;

            using (var db = new DBContext())
            {
                Units unit = db.Units.Where(x => x.UnityId == resendDocumentsIn.unityId).FirstOrDefault();

                if (unit == null)
                {
                    throw new Exception(i18n.Resource.RegisterNotFound);
                }

                uninyCode = unit.ExternalId;
            }

            resendDocumentsOut = resendDocumentApi.GetResendDocuments(resendDocumentsIn.registration, uninyCode);
            
            registerEventRepository.SaveRegisterEvent(resendDocumentsIn.id, resendDocumentsIn.key, "Log - End", "Repository.ResendDocumentRepository.GetResendDocuments", "");

            return resendDocumentsOut;
        }

        #endregion
    }
}
