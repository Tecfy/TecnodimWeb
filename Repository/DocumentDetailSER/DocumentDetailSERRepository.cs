using ApiTecnodim;
using Model.In;
using Model.Out;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Configuration;

namespace Repository
{
    public partial class DocumentDetailSERRepository
    {
        DocumentDetailApi documentDetailApi = new DocumentDetailApi();

        #region .: Api :.

        public void GetDocumentDetailSER()
        {
            ECMDocumentDetailSaveOut eCMDocumentDetailSaveOut = new ECMDocumentDetailSaveOut();
            ECMDocumentDetailSaveIn eCMDocumentDetailSaveIn = new ECMDocumentDetailSaveIn();

            #region .: Query :.

            string queryString = @"SELECT TOP({0}) CODCOLIGADA AS affiliateCode, 
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
            string queryStringUpdate = @"UPDATE BASE_ALUNOS_GESTAODOCUMENTOS SET CONTROLE='{0}' WHERE CODCOLIGADA={1} AND CODCURSO='{2}' AND CODFILIAL={3} AND CODHABILITACAO='{4}' AND RA='{5}' AND SITUACAO='{6}'";
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultSer"].ConnectionString;

            #endregion

            #region .: Synchronization :.

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                var querySelect = String.Format(queryString, WebConfigurationManager.AppSettings["Repository.GetDocumentDetailSER.TOPLimit"].ToString());
                SqlCommand command = new SqlCommand(querySelect, connection);
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

                            eCMDocumentDetailSaveOut = documentDetailApi.PostECMDocumentDetailSave(eCMDocumentDetailSaveIn);

                            using (SqlConnection connectionUpdate = new SqlConnection(connectionString))
                            {
                                var queryUpdate = string.Format(queryStringUpdate,
                                    eCMDocumentDetailSaveIn.registration,
                                    eCMDocumentDetailSaveIn.affiliateCode,
                                    eCMDocumentDetailSaveIn.courseCode,
                                    eCMDocumentDetailSaveIn.branchCode,
                                    eCMDocumentDetailSaveIn.habilitationCode,
                                    eCMDocumentDetailSaveIn.registration,
                                    eCMDocumentDetailSaveIn.status);
                                SqlCommand commandUpdate = new SqlCommand(queryUpdate, connectionUpdate);
                                connectionUpdate.Open();
                                commandUpdate.ExecuteNonQuery();
                            }
                        }
                        catch { }
                    }
                }
                finally
                {
                    reader.Close();
                }
            }

            #endregion
        }

        #endregion
    }
}
