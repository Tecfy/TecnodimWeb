using ApiTecnodim;
using Model.In;
using Model.Out;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;

namespace Repository
{
    public partial class ImportUserRepository
    {
        private ImportUserApi importUserApi = new ImportUserApi();
        private RegisterEventRepository registerEventRepository = new RegisterEventRepository();

        #region .: Api :.

        public ImportUsersOut GetImportUsers(ImportUsersIn ImportUsersIn)
        {
            ImportUsersOut importUsersOut = new ImportUsersOut();
            ImportUserOut importUserOut = new ImportUserOut();
            ImportUserIn importUserIn = new ImportUserIn();

            registerEventRepository.SaveRegisterEvent(ImportUsersIn.id, ImportUsersIn.key, "Log - Start", "Repository.ImportUserRepository.GetImportUsers", "");

            #region .: Query :.

            string queryUsersPermissions = @"SELECT CDUSER, IDUSER, NMUSER, UNINASSAU, UNIVERITAS, UNAMA, MAIN, TEAM FROM v_Users_Permissions WHERE (UNINASSAU IS NULL OR UNIVERITAS IS NULL OR UNAMA IS NULL OR TEAM IS NULL)";
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultSer"].ConnectionString;

            #endregion

            #region .: Synchronization :.

            registerEventRepository.SaveRegisterEvent(ImportUsersIn.id, ImportUsersIn.key, "Log - Start - p_Import_Users", "Repository.ImportUserRepository.GetImportUsers", "");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("p_Import_Users", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add("@Login", SqlDbType.VarChar).Value = WebConfigurationManager.AppSettings["SoftExpert.Username"].ToString();
                    command.Parameters.Add("@Cappservice", SqlDbType.VarChar).Value = WebConfigurationManager.AppSettings["ImportUsers.Cappservice"].ToString();
                    command.Parameters.Add("@Slice", SqlDbType.VarChar).Value = WebConfigurationManager.AppSettings["ImportUsers.Slice"].ToString();
                    command.Parameters.Add("@Classify", SqlDbType.VarChar).Value = WebConfigurationManager.AppSettings["ImportUsers.Classify"].ToString();
                    command.Parameters.Add("@Scans", SqlDbType.VarChar).Value = WebConfigurationManager.AppSettings["ImportUsers.Scans"].ToString();
                    command.Parameters.Add("@Resend", SqlDbType.VarChar).Value = WebConfigurationManager.AppSettings["ImportUsers.Resend"].ToString();
                    command.Parameters.Add("@Status", SqlDbType.VarChar).Value = WebConfigurationManager.AppSettings["ImportUsers.Status"].ToString();
                    command.Parameters.Add("@Grupocappservice", SqlDbType.VarChar).Value = WebConfigurationManager.AppSettings["ImportUsers.Grupocappservice"].ToString();

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                }
            }

            registerEventRepository.SaveRegisterEvent(ImportUsersIn.id, ImportUsersIn.key, "Log - End - p_Import_Users", "Repository.ImportUserRepository.GetImportUsers", "");

            registerEventRepository.SaveRegisterEvent(ImportUsersIn.id, ImportUsersIn.key, "Log - Start - v_Users_Permissions", "Repository.ImportUserRepository.GetImportUsers", "");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(queryUsersPermissions, connection))
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    try
                    {
                        while (reader.Read())
                        {
                            try
                            {
                                importUserIn = new ImportUserIn
                                {
                                    cduser = int.Parse(reader["CDUSER"].ToString()),
                                    iduser = reader["IDUSER"].ToString().Trim(),
                                    nmuser = reader["NMUSER"].ToString().Trim(),
                                    uninassau = reader["UNINASSAU"].ToString().Trim(),
                                    univeritas = reader["UNIVERITAS"].ToString().Trim(),
                                    unama = reader["UNAMA"].ToString().Trim(),
                                    main = reader["MAIN"].ToString().Trim(),
                                    team = reader["TEAM"].ToString().Trim()
                                };

                                importUserOut = importUserApi.PostECMUserPermission(importUserIn);
                            }
                            catch (Exception ex)
                            {
                                registerEventRepository.SaveRegisterEvent(ImportUsersIn.id, ImportUsersIn.key, "Erro - v_Users_Permissions", "Repository.ImportUserRepository.GetImportUsers", ex.Message);
                            }
                        }
                    }
                    finally
                    {
                        reader.Close();
                    }
                }
            }

            registerEventRepository.SaveRegisterEvent(ImportUsersIn.id, ImportUsersIn.key, "Log - End - v_Users_Permissions", "Repository.ImportUserRepository.GetImportUsers", "");

            #endregion

            registerEventRepository.SaveRegisterEvent(ImportUsersIn.id, ImportUsersIn.key, "Log - End", "Repository.ImportUserRepository.GetImportUsers", "");

            return importUsersOut;
        }

        #endregion
    }
}