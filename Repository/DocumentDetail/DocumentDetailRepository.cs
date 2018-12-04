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
        RegisterEventRepository registerEventRepository = new RegisterEventRepository();
        SliceRepository sliceRepository = new SliceRepository();
        DocumentDetailApi documentDetailApi = new DocumentDetailApi();

        #region .: Api :.

        public DocumentDetailsByRegistrationOut GetDocumentDetailsByRegistration(DocumentDetailsByRegistrationIn documentsDetailIn)
        {
            DocumentDetailsByRegistrationOut documentDetailsByRegistrationOut = new DocumentDetailsByRegistrationOut();

            registerEventRepository.SaveRegisterEvent(documentsDetailIn.userId, documentsDetailIn.key, "Log - Start", "Repository.DocumentDetailRepository.GetDocumentsDetail", "");

            documentDetailsByRegistrationOut = documentDetailApi.GetDocumentDetailsByRegistration(documentsDetailIn.Registration, documentsDetailIn.Unity);

            registerEventRepository.SaveRegisterEvent(documentsDetailIn.userId, documentsDetailIn.key, "Log - End", "Repository.DocumentDetailRepository.GetDocumentsDetail", "");

            return documentDetailsByRegistrationOut;
        }

        public DocumentDetailOut GetDocumentDetail(DocumentDetailIn documentDetailIn)
        {
            DocumentDetailOut documentDetailOut = new DocumentDetailOut();
            registerEventRepository.SaveRegisterEvent(documentDetailIn.userId, documentDetailIn.key, "Log - Start", "Repository.DocumentDetailRepository.GetDocumentDetail", "");

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

            slicesOut = sliceRepository.GetSlices(new SlicesIn { documentId = documentDetailIn.documentId, userId = documentDetailIn.userId, key = documentDetailIn.key, classificated = true });

            documentDetailOut.result.Classificated = slicesOut.result.Count;

            slicesOut = sliceRepository.GetSlices(new SlicesIn { documentId = documentDetailIn.documentId, userId = documentDetailIn.userId, key = documentDetailIn.key, classificated = false });

            documentDetailOut.result.NotClassificated = slicesOut.result.Count;

            registerEventRepository.SaveRegisterEvent(documentDetailIn.userId, documentDetailIn.key, "Log - End", "Repository.DocumentDetailRepository.GetDocumentDetail", "");
            return documentDetailOut;
        }

        public DocumentDetailsOut GetDocumentDetails(DocumentDetailsIn documentDetailsIn)
        {
            DocumentDetailsOut documentDetailsOut = new DocumentDetailsOut();
            ECMDocumentDetailSaveOut eCMDocumentDetailSaveOut = new ECMDocumentDetailSaveOut();
            ECMDocumentDetailSaveIn eCMDocumentDetailSaveIn = new ECMDocumentDetailSaveIn();

            registerEventRepository.SaveRegisterEvent(documentDetailsIn.userId, documentDetailsIn.key, "Log - Start", "Repository.DocumentDetailRepository.GetDocumentDetails", "");

            #region .: Query :.

            string queryString = @"SELECT TOP({0}) _key AS StudentId,
                                    CODCOLIGADA AS affiliateCode, 
                                    COLIGADA, 
                                    CODFILIAL AS branchCode, 
                                    FILIAL AS unity, 
                                    UNIDADE AS unityCode, 
                                    RA AS registration, 
                                    NOME AS name, 
                                    CPF AS cpf, 
                                    RG, 
                                    CODCURSO AS courseCode, 
                                    CODHABILITACAO AS habilitationCode, 
                                    CURSO AS course, 
                                    SITUACAO AS status, 
                                    CODTIPOINGRESSO, 
                                    TIPOINGRESSO, 
                                    RECMODIFIEDON, 
                                    CONTROLE 
                                 FROM BASE_ALUNOS_GESTAODOCUMENTOS WHERE CONTROLE IS NULL";
            string queryStringUpdate = @"UPDATE BASE_ALUNOS_GESTAODOCUMENTOS SET CONTROLE='{0}' WHERE _key={1}";
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultSer"].ConnectionString;

            #endregion

            #region .: Synchronization :.

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                registerEventRepository.SaveRegisterEvent(documentDetailsIn.userId, documentDetailsIn.key, "Log - Start Query", "Repository.DocumentDetailRepository.GetDocumentDetails", "");

                var querySelect = String.Format(queryString, WebConfigurationManager.AppSettings["Repository.GetDocumentDetailSER.TOPLimit"].ToString());
                SqlCommand command = new SqlCommand(querySelect, connection);

                registerEventRepository.SaveRegisterEvent(documentDetailsIn.userId, documentDetailsIn.key, "Log - End Query", "Repository.DocumentDetailRepository.GetDocumentDetails", "");

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
                                unityCode = reader["unityCode"].ToString(),
                                cpf = HelperFormatCnpjCpf.FormatCPF(reader["cpf"].ToString()),
                                course = reader["course"].ToString(),
                                registration = reader["registration"].ToString(),
                                name = reader["name"].ToString(),
                                status = reader["status"].ToString(),
                                unity = reader["unity"].ToString(),
                                affiliateCode = int.Parse(reader["affiliateCode"].ToString()),
                                courseCode = int.Parse(reader["courseCode"].ToString()),
                                branchCode = int.Parse(reader["branchCode"].ToString()),
                                habilitationCode = reader["habilitationCode"].ToString()
                            };

                            registerEventRepository.SaveRegisterEvent(documentDetailsIn.userId, documentDetailsIn.key, "Log - Synchronization Start SE", "Repository.DocumentDetailRepository.GetDocumentDetails", "");

                            eCMDocumentDetailSaveOut = documentDetailApi.PostECMDocumentDetailSave(eCMDocumentDetailSaveIn);

                            registerEventRepository.SaveRegisterEvent(documentDetailsIn.userId, documentDetailsIn.key, "Log - Synchronization End SE", "Repository.DocumentDetailRepository.GetDocumentDetails", "");

                            using (SqlConnection connectionUpdate = new SqlConnection(connectionString))
                            {
                                registerEventRepository.SaveRegisterEvent(documentDetailsIn.userId, documentDetailsIn.key, "Log - Start Update", "Repository.DocumentDetailRepository.GetDocumentDetails", "");

                                var queryUpdate = string.Format(queryStringUpdate, eCMDocumentDetailSaveIn.registration, eCMDocumentDetailSaveIn.studentId);
                                SqlCommand commandUpdate = new SqlCommand(queryUpdate, connectionUpdate);
                                connectionUpdate.Open();
                                commandUpdate.ExecuteNonQuery();

                                registerEventRepository.SaveRegisterEvent(documentDetailsIn.userId, documentDetailsIn.key, "Log - End Update", "Repository.DocumentDetailRepository.GetDocumentDetails", "");
                            }
                        }
                        catch (Exception ex)
                        {
                            registerEventRepository.SaveRegisterEvent(documentDetailsIn.userId, documentDetailsIn.key, "Erro", "Repository.DocumentDetailRepository.GetDocumentDetails", ex.Message);
                        }
                    }
                }
                finally
                {
                    reader.Close();
                }
            }

            #endregion

            registerEventRepository.SaveRegisterEvent(documentDetailsIn.userId, documentDetailsIn.key, "Log - End", "Repository.DocumentDetailRepository.GetDocumentDetails", "");

            return documentDetailsOut;
        }

        #endregion
    }
}
