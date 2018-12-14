using ApiTecnodim;
using DataEF.DataAccess;
using Model.In;
using Model.Out;
using System;
using System.Configuration;
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

            documentDetailsByRegistrationOut = documentDetailApi.GetDocumentDetailsByRegistration(documentsDetailIn.Registration, documentsDetailIn.Unity);

            registerEventRepository.SaveRegisterEvent(documentsDetailIn.id, documentsDetailIn.key, "Log - End", "Repository.DocumentDetailRepository.GetDocumentsDetail", "");

            return documentDetailsByRegistrationOut;
        }

        public DocumentDetailOut GetDocumentDetail(DocumentDetailIn documentDetailIn)
        {
            DocumentDetailOut documentDetailOut = new DocumentDetailOut();
            registerEventRepository.SaveRegisterEvent(documentDetailIn.id, documentDetailIn.key, "Log - Start", "Repository.DocumentDetailRepository.GetDocumentDetail", "");

            string registration = string.Empty;

            using (var db = new DBContext())
            {
                Documents document = db.Documents.Where(x => x.DocumentId == documentDetailIn.documentId).FirstOrDefault();

                if (document == null)
                {
                    throw new System.Exception(i18n.Resource.RegisterNotFound);
                }

                registration = document.Registration;
            }

            documentDetailOut = documentDetailApi.GetDocumentDetail(registration);

            SlicesOut slicesOut = new SlicesOut();

            slicesOut = sliceRepository.GetSlices(new SlicesIn { documentId = documentDetailIn.documentId, id = documentDetailIn.id, key = documentDetailIn.key, classificated = true });

            documentDetailOut.result.Classificated = slicesOut.result.Count;

            slicesOut = sliceRepository.GetSlices(new SlicesIn { documentId = documentDetailIn.documentId, id = documentDetailIn.id, key = documentDetailIn.key, classificated = false });

            documentDetailOut.result.NotClassificated = slicesOut.result.Count;

            registerEventRepository.SaveRegisterEvent(documentDetailIn.id, documentDetailIn.key, "Log - End", "Repository.DocumentDetailRepository.GetDocumentDetail", "");
            return documentDetailOut;
        }

        public DocumentDetailsOut GetDocumentDetails(DocumentDetailsIn documentDetailsIn)
        {
            DocumentDetailsOut documentDetailsOut = new DocumentDetailsOut();
            ECMDocumentDetailSaveOut eCMDocumentDetailSaveOut = new ECMDocumentDetailSaveOut();
            ECMDocumentDetailSaveIn eCMDocumentDetailSaveIn = new ECMDocumentDetailSaveIn();

            registerEventRepository.SaveRegisterEvent(documentDetailsIn.id, documentDetailsIn.key, "Log - Start", "Repository.DocumentDetailRepository.GetDocumentDetails", "");

            #region .: Query :.

            string queryString = @"SELECT TOP({0}) _key AS StudentId,
                                    FILIAL AS unity, 
                                    UNIDADE AS unityCode, 
                                    RA AS registration, 
                                    NOME AS name, 
                                    CPF AS cpf, 
                                    RG AS rg, 
                                    CURSO AS course, 
                                    SITUACAO AS status
                                 FROM BASE_ALUNOS_GESTAODOCUMENTOS WHERE CONTROLE IS NULL AND CPF IS NOT NULL AND RG IS NOT NULL AND ('{1}'='' OR UNIDADE='{1}')";
            string queryStringUpdate = @"UPDATE BASE_ALUNOS_GESTAODOCUMENTOS SET CONTROLE='{0}' WHERE _key={1}";
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultSer"].ConnectionString;

            #endregion

            #region .: Synchronization :.

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                registerEventRepository.SaveRegisterEvent(documentDetailsIn.id, documentDetailsIn.key, "Log - Start Query", "Repository.DocumentDetailRepository.GetDocumentDetails", "");

                var querySelect = String.Format(queryString, WebConfigurationManager.AppSettings["Repository.GetDocumentDetailSER.TOPLimit"].ToString(), WebConfigurationManager.AppSettings["Repository.GetDocumentDetailSER.Unity"].ToString());
                SqlCommand command = new SqlCommand(querySelect, connection);

                registerEventRepository.SaveRegisterEvent(documentDetailsIn.id, documentDetailsIn.key, "Log - End Query", "Repository.DocumentDetailRepository.GetDocumentDetails", "");

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        try
                        {
                            eCMDocumentDetailSaveIn = new ECMDocumentDetailSaveIn
                            {
                                studentId = int.Parse(reader["studentId"].ToString()),
                                unityCode = reader["unityCode"].ToString().Trim(),
                                cpf = HelperFormatCnpjCpf.FormatCPF(reader["cpf"].ToString().Trim()),
                                rg = reader["rg"].ToString().Trim(),
                                course = reader["course"].ToString().Trim(),
                                registration = reader["registration"].ToString().Trim(),
                                name = reader["name"].ToString().Trim(),
                                status = reader["status"].ToString().Trim(),
                                unity = reader["unity"].ToString().Trim()
                            };

                            registerEventRepository.SaveRegisterEvent(documentDetailsIn.id, documentDetailsIn.key, "Log - Synchronization Start SE", "Repository.DocumentDetailRepository.GetDocumentDetails", "");

                            eCMDocumentDetailSaveOut = documentDetailApi.PostECMDocumentDetailSave(eCMDocumentDetailSaveIn);

                            registerEventRepository.SaveRegisterEvent(documentDetailsIn.id, documentDetailsIn.key, "Log - Synchronization End SE", "Repository.DocumentDetailRepository.GetDocumentDetails", "");

                            using (SqlConnection connectionUpdate = new SqlConnection(connectionString))
                            {
                                registerEventRepository.SaveRegisterEvent(documentDetailsIn.id, documentDetailsIn.key, "Log - Start Update", "Repository.DocumentDetailRepository.GetDocumentDetails", "");

                                var queryUpdate = string.Format(queryStringUpdate, eCMDocumentDetailSaveOut.result.registration, eCMDocumentDetailSaveIn.studentId);
                                SqlCommand commandUpdate = new SqlCommand(queryUpdate, connectionUpdate);
                                connectionUpdate.Open();
                                commandUpdate.ExecuteNonQuery();

                                registerEventRepository.SaveRegisterEvent(documentDetailsIn.id, documentDetailsIn.key, "Log - End Update", "Repository.DocumentDetailRepository.GetDocumentDetails", "");
                            }
                        }
                        catch (Exception ex)
                        {
                            registerEventRepository.SaveRegisterEvent(documentDetailsIn.id, documentDetailsIn.key, "Erro", "Repository.DocumentDetailRepository.GetDocumentDetails", ex.Message);
                        }
                    }
                }
                finally
                {
                    reader.Close();
                }
            }

            #endregion

            registerEventRepository.SaveRegisterEvent(documentDetailsIn.id, documentDetailsIn.key, "Log - End", "Repository.DocumentDetailRepository.GetDocumentDetails", "");

            return documentDetailsOut;
        }

        #endregion
    }
}
