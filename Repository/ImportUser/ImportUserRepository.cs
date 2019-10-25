using Model.In;
using Model.Out;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;

namespace Repository
{
    public partial class ImportUserRepository
    {
        private RegisterEventRepository registerEventRepository = new RegisterEventRepository();

        #region .: Api :.

        public ImportUsersOut GetImportUsers(ImportUsersIn ImportUsersIn)
        {
            ImportUsersOut ImportUsersOut = new ImportUsersOut();

            registerEventRepository.SaveRegisterEvent(ImportUsersIn.id, ImportUsersIn.key, "Log - Start", "Repository.ImportUserRepository.GetImportUsers", "");

            #region .: Query :.

            string connectionString = ConfigurationManager.ConnectionStrings["DefaultSer"].ConnectionString;

            #endregion

            #region .: Synchronization :.

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
                    command.Parameters.Add("@Status", SqlDbType.VarChar).Value = WebConfigurationManager.AppSettings["ImportUsers.Status"].ToString();
                    command.Parameters.Add("@Grupocappservice", SqlDbType.VarChar).Value = WebConfigurationManager.AppSettings["ImportUsers.Grupocappservice"].ToString();

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                }
            }

            #endregion

            registerEventRepository.SaveRegisterEvent(ImportUsersIn.id, ImportUsersIn.key, "Log - End", "Repository.ImportUserRepository.GetImportUsers", "");

            return ImportUsersOut;
        }

        #endregion
    }
}