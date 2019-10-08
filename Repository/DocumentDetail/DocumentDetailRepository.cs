using ApiTecnodim;
using DataEF.DataAccess;
using Model.In;
using Model.Out;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Configuration;

namespace Repository
{
    public partial class DocumentDetailRepository
    {
        private RegisterEventRepository registerEventRepository = new RegisterEventRepository();
        private SliceRepository sliceRepository = new SliceRepository();
        private DocumentDetailApi documentDetailApi = new DocumentDetailApi();

        #region .: Api :.

        public DocumentDetailsByRegistrationOut GetDocumentDetailsByRegistration(DocumentDetailsByRegistrationIn documentsDetailIn)
        {
            DocumentDetailsByRegistrationOut documentDetailsByRegistrationOut = new DocumentDetailsByRegistrationOut();

            registerEventRepository.SaveRegisterEvent(documentsDetailIn.id, documentsDetailIn.key, "Log - Start", "Repository.DocumentDetailRepository.GetDocumentsDetail", "");

            string uninyCode = string.Empty;

            using (var db = new DBContext())
            {
                Units unit = db.Units.Where(x => x.UnityId == documentsDetailIn.UnityId).FirstOrDefault();

                if (unit == null)
                {
                    throw new Exception(i18n.Resource.RegisterNotFound);
                }

                uninyCode = unit.ExternalId;
            }

            documentDetailsByRegistrationOut = documentDetailApi.GetDocumentDetailsByRegistration(documentsDetailIn.Registration, uninyCode);

            registerEventRepository.SaveRegisterEvent(documentsDetailIn.id, documentsDetailIn.key, "Log - End", "Repository.DocumentDetailRepository.GetDocumentsDetail", "");

            return documentDetailsByRegistrationOut;
        }

        public DocumentDetailJobIdOut GetDocumentDetailByJobId(DocumentDetailByJobIdIn documentDetailIn)
        {
            DocumentDetailJobIdOut documentDetailOut = new DocumentDetailJobIdOut();

            registerEventRepository.SaveRegisterEvent(documentDetailIn.id, documentDetailIn.key, "Log - Start", "Repository.DocumentDetailRepository.GetDocumentDetailByJobId", "");

            string registration = string.Empty;
            string uninyCode = string.Empty;

            using (var db = new DBContext())
            {
                Jobs jobs = db.Jobs.Where(x => x.JobId == documentDetailIn.jobId).FirstOrDefault();

                if (jobs == null)
                {
                    throw new Exception(i18n.Resource.RegisterNotFound);
                }

                registration = jobs.Registration;

                Units unit = db.Units.Where(x => x.UnityId == jobs.UnityId).FirstOrDefault();

                if (unit == null)
                {
                    throw new Exception(i18n.Resource.RegisterNotFound);
                }

                uninyCode = unit.ExternalId;
            }

            documentDetailOut = documentDetailApi.GetECMDocumentDetailByRegistration(registration, uninyCode);

            registerEventRepository.SaveRegisterEvent(documentDetailIn.id, documentDetailIn.key, "Log - End", "Repository.DocumentDetailRepository.GetDocumentDetailByJobId", "");

            return documentDetailOut;
        }

        public DocumentDetailDocumentIdOut GetDocumentDetailByDocumentId(DocumentDetailByDocumentIdIn documentDetailIn)
        {
            DocumentDetailDocumentIdOut documentDetailOut = new DocumentDetailDocumentIdOut();
            registerEventRepository.SaveRegisterEvent(documentDetailIn.id, documentDetailIn.key, "Log - Start", "Repository.DocumentDetailRepository.GetDocumentDetailByDocumentId", "");

            string registration = string.Empty;

            using (var db = new DBContext())
            {
                Documents document = db.Documents.Where(x => x.DocumentId == documentDetailIn.documentId).FirstOrDefault();

                if (document == null)
                {
                    throw new Exception(i18n.Resource.RegisterNotFound);
                }

                registration = document.Registration;
            }

            documentDetailOut = documentDetailApi.GetDocumentDetailDocumentId(registration);

            SlicesOut slicesOut = new SlicesOut();

            slicesOut = sliceRepository.GetSlices(new SlicesIn { documentId = documentDetailIn.documentId, id = documentDetailIn.id, key = documentDetailIn.key, classificated = true });

            documentDetailOut.result.Classificated = slicesOut.result.Count;

            slicesOut = sliceRepository.GetSlices(new SlicesIn { documentId = documentDetailIn.documentId, id = documentDetailIn.id, key = documentDetailIn.key, classificated = false });

            documentDetailOut.result.NotClassificated = slicesOut.result.Count;

            registerEventRepository.SaveRegisterEvent(documentDetailIn.id, documentDetailIn.key, "Log - End", "Repository.DocumentDetailRepository.GetDocumentDetailByDocumentId", "");
            return documentDetailOut;
        }

        public DocumentDetailsOut GetDocumentDetails(DocumentDetailsIn documentDetailsIn)
        {
            DocumentDetailsOut documentDetailsOut = new DocumentDetailsOut();
            string units = string.Empty;

            using (var db = new DBContext())
            {
                units = string.Join(",", db.Units.Where(x => x.Active == true && x.DeletedDate == null).Select(x => x.ExternalId).ToList());
            }

            registerEventRepository.SaveRegisterEvent(documentDetailsIn.id, documentDetailsIn.key, "Log - Start", "Repository.DocumentDetailRepository.GetDocumentDetails", "");

            #region .: Query :.

            string connectionString = ConfigurationManager.ConnectionStrings["DefaultSer"].ConnectionString;

            #endregion

            #region .: Synchronization :.

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("p_Import_Students", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add("@Units", SqlDbType.VarChar).Value = units;
                    command.Parameters.Add("@Top", SqlDbType.Int).Value = WebConfigurationManager.AppSettings["Repository.GetDocumentDetailSER.TOPLimit"].ToString();
                    command.Parameters.Add("@UserSE", SqlDbType.VarChar).Value = WebConfigurationManager.AppSettings["SoftExpert.Username"].ToString();

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                }
            }

            #endregion

            registerEventRepository.SaveRegisterEvent(documentDetailsIn.id, documentDetailsIn.key, "Log - End", "Repository.DocumentDetailRepository.GetDocumentDetails", "");

            return documentDetailsOut;
        }

        #endregion
    }
}
