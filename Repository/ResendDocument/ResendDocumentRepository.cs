using ApiTecnodim;
using DataEF.DataAccess;
using Helper;
using Helper.Enum;
using Helper.ServerMap;
using iTextSharp.text.pdf;
using Model.In;
using Model.Out;
using Model.VM;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web.Configuration;
using WebSupergoo.ABCpdf11;

namespace Repository
{
    public partial class ResendDocumentRepository
    {
        private RegisterEventRepository registerEventRepository = new RegisterEventRepository();
        private ResendDocumentApi resendDocumentApi = new ResendDocumentApi();
        private bool _proxy = ConfigurationManager.AppSettings["Proxy"] == "true" ? true : false;
        private string _proxyUrl = ConfigurationManager.AppSettings["ProxyUrl"];

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

        public ResendDocumentOut SaveResendDocument(ResendDocumentIn resendDocumentIn)
        {
            ResendDocumentOut resendDocumentOut = new ResendDocumentOut();
            ResendDocumentSEIn resendDocumentSEIn = new ResendDocumentSEIn();

            registerEventRepository.SaveRegisterEvent(resendDocumentIn.id, resendDocumentIn.key, "Log - Start", "Repository.ResendDocumentRepository.SaveResendDocument", "");

            string path = ServerMapHelper.GetServerMap(ConfigurationManager.AppSettings["Path.Files.Resend"]);
            string name = string.Empty;
            string pathFile = string.Empty;
            string user = string.Empty;
            Units unit = new Units();

            foreach (var item in resendDocumentIn.itens)
            {
                name = resendDocumentIn.registration + ".pdf";
                pathFile = Path.Combine(path, name);

                DownloadFile(path, item.uri, pathFile, 1, resendDocumentIn.id, resendDocumentIn.key);

                if (!File.Exists(pathFile))
                {
                    int sleep = 3000;
                    int.TryParse(ConfigurationManager.AppSettings["SLEEP"], out sleep);

                    Thread.Sleep(sleep);
                }

                if (File.Exists(pathFile))
                {
                    try
                    {
                        byte[] fileBinary = File.ReadAllBytes(pathFile);

                        int pageCount = 0;

                        using (PdfReader reader = new PdfReader(fileBinary))
                        {
                            pageCount = reader.NumberOfPages;
                        }

                        using (var db = new DBContext())
                        {
                            unit = db.Units.Where(x => x.UnityId == resendDocumentIn.unityId).FirstOrDefault();

                            if (unit == null)
                            {
                                throw new Exception(i18n.Resource.RegisterNotFound);
                            }

                            user = db.Users.Where(x => x.AspNetUserId == resendDocumentIn.id).FirstOrDefault().Registration;
                        }

                        resendDocumentSEIn = new ResendDocumentSEIn()
                        {
                            id = resendDocumentIn.id,
                            key = resendDocumentIn.key,
                            FileBinary = fileBinary,
                            CategoryPrimary = ConfigurationManager.AppSettings["Category_Primary"],
                            CategoryOwner = ConfigurationManager.AppSettings["Category_Owner"],
                            Registration = resendDocumentIn.registration,
                            User = user,
                            Extension = ".pdf",
                            Now = DateTime.Now,
                            Pages = pageCount,
                            UnityCode = unit.ExternalId,
                            UnityName = unit.Name
                        };

                        registerEventRepository.SaveRegisterEvent(resendDocumentIn.id, resendDocumentIn.key, "Log - Start", "Repository.ResendDocumentRepository.SaveResendDocument", string.Format("**** Arquivo sendo enviado para o SE: {0}. ****", resendDocumentSEIn.Registration));

                        #region .: Query :.

                        string connectionString = ConfigurationManager.ConnectionStrings["DefaultSer"].ConnectionString;

                        #endregion

                        #region .: Synchronization :.

                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            using (SqlCommand command = new SqlCommand("p_Dossier_Document", connection))
                            {
                                command.CommandType = CommandType.StoredProcedure;

                                command.Parameters.Add("@Registration", SqlDbType.VarChar).Value = resendDocumentSEIn.Registration;
                                command.Parameters.Add("@User", SqlDbType.VarChar).Value = resendDocumentSEIn.User;
                                command.Parameters.Add("@Pages", SqlDbType.Decimal).Value = resendDocumentSEIn.Pages;
                                command.Parameters.Add("@UnityCode", SqlDbType.VarChar).Value = resendDocumentSEIn.UnityCode;
                                command.Parameters.Add("@UnityName", SqlDbType.VarChar).Value = resendDocumentSEIn.UnityName;
                                command.Parameters.Add("@Title", SqlDbType.VarChar).Value = ConfigurationManager.AppSettings["Category_Primary_Title"].ToString();

                                connection.Open();
                                SqlDataReader reader = command.ExecuteReader();

                                reader.Read();

                                resendDocumentSEIn.DocumentIdPrimary = reader["DocumentId"].ToString().Trim();
                            }
                        }

                        UploadFile(resendDocumentSEIn);

                        if (File.Exists(pathFile))
                        {
                            File.Delete(pathFile);
                        }

                        #endregion

                        registerEventRepository.SaveRegisterEvent(resendDocumentIn.id, resendDocumentIn.key, "Log - End", "Repository.ResendDocumentRepository.SaveResendDocument", string.Format("**** Arquivo sendo enviado para o SE: {0}. ****", resendDocumentSEIn.Registration));
                    }
                    catch (Exception ex)
                    {
                        registerEventRepository.SaveRegisterEvent(resendDocumentIn.id, resendDocumentIn.key, "Erro", "Repository.ResendDocumentRepository.SaveResendDocument", string.Format("**** Arquivo sendo enviado para o SE (Erro {0}): {1}. ****", ex.Message, resendDocumentSEIn.Registration));
                    }
                }
            }

            registerEventRepository.SaveRegisterEvent(resendDocumentIn.id, resendDocumentIn.key, "Log - End", "Repository.ResendDocumentRepository.SaveResendDocument", "");
            return resendDocumentOut;
        }

        #endregion

        #region .: HELPER :.

        private void DownloadFile(string path, string archive, string pathFile, int exec, string userId, string key)
        {
            try
            {
                WebClient wc = new WebClient();

                if (_proxy)
                {
                    WebProxy wp = new WebProxy(_proxyUrl);
                    wc.Proxy = wp;
                }

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                if (File.Exists(pathFile))
                {
                    File.Delete(pathFile);
                }

                wc.DownloadFile(archive, pathFile);
            }
            catch (Exception ex)
            {
                registerEventRepository.SaveRegisterEvent(userId, key, "Erro", "Repository.ResendDocumentRepository.DownloadFile", string.Format("Arquivo: {0}. Erro: {1}", archive, ex.Message));

                if (exec < 5)
                {
                    exec++;
                    int sleep = 3000;
                    int.TryParse(ConfigurationManager.AppSettings["SLEEP"], out sleep);

                    Thread.Sleep(sleep);
                    DownloadFile(path, archive, pathFile, exec, userId, key);
                }
                else
                {
                    throw new Exception(i18n.Resource.DownloadFailed);
                }
            }
        }

        private bool UploadFile(ResendDocumentSEIn resendDocumentSEIn)
        {
            try
            {
                registerEventRepository.SaveRegisterEvent(resendDocumentSEIn.id, resendDocumentSEIn.key, "Log - UploadFile Start", "Repository.ResendDocumentRepository.UploadFile", string.Format("**** Arquivo sendo enviado para o SE: {0}. ****", resendDocumentSEIn.Registration));

                string physicalPath = ConfigurationManager.AppSettings["Sesuite.Physical.Path"];
                string physicalPathSE = ConfigurationManager.AppSettings["Sesuite.Physical.Path.SE"];

                #region .: Save File :.

                if (!Directory.Exists(physicalPath))
                {
                    Directory.CreateDirectory(physicalPath);
                }

                registerEventRepository.SaveRegisterEvent(resendDocumentSEIn.id, resendDocumentSEIn.key, "Log - UploadFile Passo 1", "Repository.ResendDocumentRepository.UploadFile", string.Format("**** Arquivo sendo enviado para o SE: {0}. ****", resendDocumentSEIn.Registration));

                if (File.Exists(Path.Combine(physicalPath, resendDocumentSEIn.FileName)))
                {
                    File.Delete(Path.Combine(physicalPath, resendDocumentSEIn.FileName));
                }

                registerEventRepository.SaveRegisterEvent(resendDocumentSEIn.id, resendDocumentSEIn.key, "Log - UploadFile Passo 2", "Repository.ResendDocumentRepository.UploadFile", string.Format("**** Arquivo sendo enviado para o SE: {0}. ****", resendDocumentSEIn.Registration));

                File.WriteAllBytes(Path.Combine(physicalPath, resendDocumentSEIn.FileName), resendDocumentSEIn.FileBinary);

                registerEventRepository.SaveRegisterEvent(resendDocumentSEIn.id, resendDocumentSEIn.key, "Log - UploadFile Passo 3", "Repository.ResendDocumentRepository.UploadFile", string.Format("**** Arquivo sendo enviado para o SE: {0}. ****", resendDocumentSEIn.Registration));

                #endregion

                #region .: Query :.

                string queryStringInsert = @"INSERT INTO ADINTERFACE (CDINTERFACE, FGIMPORT, CDISOSYSTEM, FGOPTION, NMFIELD01, NMFIELD02, NMFIELD03, NMFIELD04, NMFIELD05, NMFIELD07) VALUES((SELECT COALESCE(MAX(CDINTERFACE),0)+1 FROM ADINTERFACE), 1, 73, 97, '{0}','0','{1}','{2}','{3}','{4}')";
                string connectionString = ConfigurationManager.ConnectionStrings["DefaultSer"].ConnectionString;

                #endregion

                #region .: Insert Sesuite :.

                using (SqlConnection connectionInsert = new SqlConnection(connectionString))
                {
                    registerEventRepository.SaveRegisterEvent(resendDocumentSEIn.id, resendDocumentSEIn.key, "Log - UploadFile Passo 4", "Repository.ResendDocumentRepository.UploadFile", string.Format("**** Arquivo sendo enviado para o SE: {0}. ****", resendDocumentSEIn.Registration));

                    string queryInsert = string.Format(queryStringInsert,
                        resendDocumentSEIn.DocumentIdPrimary /*Identificador do Documento*/,
                        resendDocumentSEIn.FileName /*Nome do Arquivo*/,
                        resendDocumentSEIn.User /*Matrícula do Usuário*/,
                        physicalPathSE + resendDocumentSEIn.FileName,
                        resendDocumentSEIn.CategoryPrimary.Trim() /*Identificador da categoria*/);

                    using (SqlCommand commandInsert = new SqlCommand(queryInsert, connectionInsert))
                    {
                        connectionInsert.Open();
                        commandInsert.ExecuteNonQuery();
                    }

                    registerEventRepository.SaveRegisterEvent(resendDocumentSEIn.id, resendDocumentSEIn.key, "Log - UploadFile Passo 5", "Repository.ResendDocumentRepository.UploadFile", string.Format("**** Arquivo sendo enviado para o SE: {0}. ****", resendDocumentSEIn.Registration));
                }

                #endregion                
            }
            catch (Exception ex)
            {
                registerEventRepository.SaveRegisterEvent(resendDocumentSEIn.id, resendDocumentSEIn.key, "Erro", "Repository.ResendDocumentRepository.UploadFile", string.Format("**** Arquivo sendo enviado para o SE (Erro {0}): {1}. ****", ex.Message, resendDocumentSEIn.Registration));
                throw ex;
            }

            registerEventRepository.SaveRegisterEvent(resendDocumentSEIn.id, resendDocumentSEIn.key, "Log -UploadFile  End", "Repository.ResendDocumentRepository.UploadFile", string.Format("**** Arquivo sendo enviado para o SE: {0}. ****", resendDocumentSEIn.Registration));

            return true;
        }

        #endregion
    }
}
