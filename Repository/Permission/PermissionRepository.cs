using ApiTecnodim;
using DataEF.DataAccess;
using Helper.Enum;
using Model.In;
using Model.Out;
using Model.VM;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Configuration;

namespace Repository
{
    public partial class PermissionRepository
    {
        private RegisterEventRepository registerEventRepository = new RegisterEventRepository();
        private PermissionApi permissionApi = new PermissionApi();
        private AttributeApi attributeApi = new AttributeApi();

        #region .: Api :.

        public PermissionsOut GetPermissions(PermissionsIn permissionsIn)
        {
            PermissionsOut permissionsOut = new PermissionsOut();

            registerEventRepository.SaveRegisterEvent(permissionsIn.id, permissionsIn.key, "Log - Start", "Repository.PermissionRepository.GetPermissions", "");

            #region .: Search Permissions :.

            permissionsOut = permissionApi.GetPermissions();

            #endregion

            #region .: Process Queue :.

            foreach (var item in permissionsOut.result)
            {
                try
                {
                    PermissionsProcess(item, permissionsIn.id, permissionsIn.key);
                }
                catch (Exception ex)
                {
                    registerEventRepository.SaveRegisterEvent(permissionsIn.id, permissionsIn.key, "Erro", "Repository.PermissionRepository.GetPermissions", ex.Message);

                    permissionsOut.messages.Add(ex.Message);
                }
            }

            #endregion

            registerEventRepository.SaveRegisterEvent(permissionsIn.id, permissionsIn.key, "Log - End", "Repository.PermissionRepository.GetPermissions", "");

            return permissionsOut;
        }

        #endregion

        #region .: Helper :.

        private void PermissionsProcess(PermissionsVM permissionsVM, string id, string key)
        {
            registerEventRepository.SaveRegisterEvent(id, key, "Log - Start", "Repository.PermissionRepository.PermissionsProcess", "");
            DateTime now = DateTime.Now;

            try
            {
                #region .: Synchronization CapService :.

                #region .: Query :.

                string queryStringUser = @"SELECT idUsuario, loginUsuario, senhaUsuario, nomeUsuario, grupoUsuarioId, statusUsuario, autenticacaoActiveDirectory 
                                       FROM CAP_USUARIO 
                                       WHERE loginUsuario='{0}'";
                string queryStringGroup = @"SELECT grupoId, grupoNome FROM CAP_GRUPO
                                            WHERE grupoNome='{0}'";
                string queryStringUpdate = @"UPDATE CAP_USUARIO SET nomeUsuario='{0}', grupoUsuarioId={1}, statusUsuario={2} WHERE loginUsuario='{3}'";
                string queryStringInsert = @"INSERT INTO CAP_USUARIO (loginUsuario, senhaUsuario, nomeUsuario, grupoUsuarioId, statusUsuario, autenticacaoActiveDirectory) VALUES ('{0}','FE5C1FBBC9568682A472BC86201024DB','{1}',{2},1,1)";
                string connectionString = ConfigurationManager.ConnectionStrings["DefaultCap"].ConnectionString;

                #endregion

                #region .: Synchronization :.

                CapUser capUser = new CapUser();
                CapGroup capGroup = new CapGroup();

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    registerEventRepository.SaveRegisterEvent(id, key, "Log - Start Query - Group", "Repository.PermissionRepository.PermissionsProcess", "");

                    var querySelect = String.Format(queryStringGroup, permissionsVM.group);
                    SqlCommand command = new SqlCommand(querySelect, connection);

                    registerEventRepository.SaveRegisterEvent(id, key, "Log - End Query - Group", "Repository.PermissionRepository.PermissionsProcess", "");

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    try
                    {
                        while (reader.Read())
                        {
                            try
                            {
                                capGroup = new CapGroup
                                {
                                    GroupId = int.Parse(reader["grupoId"].ToString()),
                                    GroupName = reader["grupoNome"].ToString().Trim()
                                };
                            }
                            catch (Exception ex)
                            {
                                registerEventRepository.SaveRegisterEvent(id, key, "Erro - Group", "Repository.PermissionRepository.PermissionsProcess", ex.Message);
                            }
                        }
                    }
                    finally
                    {
                        reader.Close();
                    }
                }

                if (capGroup.GroupId > 0)
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        registerEventRepository.SaveRegisterEvent(id, key, "Log - Start Query - User", "Repository.PermissionRepository.PermissionsProcess", "");

                        var querySelect = String.Format(queryStringUser, permissionsVM.registration);
                        SqlCommand command = new SqlCommand(querySelect, connection);

                        registerEventRepository.SaveRegisterEvent(id, key, "Log - End Query - User", "Repository.PermissionRepository.PermissionsProcess", "");

                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();
                        try
                        {
                            if (reader.Read())
                            {
                                while (reader.Read())
                                {
                                    try
                                    {
                                        capUser = new CapUser
                                        {
                                            UserId = int.Parse(reader["idUsuario"].ToString()),
                                            UserLogin = reader["loginUsuario"].ToString().Trim(),
                                            UserPassword = reader["senhaUsuario"].ToString().Trim(),
                                            UserName = reader["nomeUsuario"].ToString().Trim(),
                                            UserGroupId = int.Parse(reader["grupoUsuarioId"].ToString().Trim()),
                                            UserStatus = bool.Parse(reader["statusUsuario"].ToString().Trim()),
                                            AuthenticationAD = bool.Parse(reader["autenticacaoActiveDirectory"].ToString().Trim())
                                        };

                                        if (capUser.UserId > 0)
                                        {
                                            using (SqlConnection connectionUpdate = new SqlConnection(connectionString))
                                            {
                                                registerEventRepository.SaveRegisterEvent(id, key, "Log - Start Update - User", "Repository.PermissionRepository.PermissionsProcess", "");
                                                var queryUpdate = permissionsVM.cappservice ? string.Format(queryStringUpdate, permissionsVM.name /*Nome do Usuário*/, capGroup.GroupId /*Grupo do Usuário*/, 1 /*Status do Usuário*/, permissionsVM.registration /*Login do Usuário*/) : string.Format(queryStringUpdate, permissionsVM.name /*Nome do Usuário*/, capGroup.GroupId /*Grupo do Usuário*/, 0 /*Status do Usuário*/, permissionsVM.registration /*Login do Usuário*/);
                                                SqlCommand commandUpdate = new SqlCommand(queryUpdate, connectionUpdate);
                                                connectionUpdate.Open();
                                                commandUpdate.ExecuteNonQuery();

                                                registerEventRepository.SaveRegisterEvent(id, key, "Log - End Update - User", "Repository.PermissionRepository.PermissionsProcess", "");
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        registerEventRepository.SaveRegisterEvent(id, key, "Erro - User", "Repository.PermissionRepository.PermissionsProcess", ex.Message);
                                    }
                                }
                            }
                            else if (permissionsVM.cappservice)
                            {
                                using (SqlConnection connectionInsert = new SqlConnection(connectionString))
                                {
                                    var queryInsert = string.Format(queryStringInsert,
                                        permissionsVM.registration /*Login do Usuário*/,
                                        permissionsVM.name /*Nome do Usuário*/,
                                        capGroup.GroupId /*Grupo do Usuário*/);

                                    using (SqlCommand commandInsert = new SqlCommand(queryInsert, connectionInsert))
                                    {
                                        connectionInsert.Open();
                                        commandInsert.ExecuteNonQuery();
                                    }
                                }
                            }
                        }
                        finally
                        {
                            reader.Close();
                        }
                    }
                }

                #endregion

                #endregion

                #region .: Synchronization TecnodimWeb :.

                using (var db = new DBContext())
                {
                    Users user = db.Users.Where(x => x.Registration == permissionsVM.registration).FirstOrDefault();

                    if (user != null)
                    {
                        List<AspNetUserClaims> aspNetUserClaims = db.AspNetUserClaims.Where(x => x.UserId == user.AspNetUserId).ToList();

                        foreach (var item in aspNetUserClaims)
                        {
                            db.Entry(item).State = System.Data.Entity.EntityState.Deleted;
                            db.SaveChanges();
                        }

                        if (permissionsVM.classify)
                        {
                            AspNetUserClaims aspNetUserClaim = new AspNetUserClaims
                            {
                                UserId = user.AspNetUserId,
                                ClaimType = EClaims.Classificar.ToString(),
                                ClaimValue = EClaims.Classificar.ToString()
                            };

                            db.AspNetUserClaims.Add(aspNetUserClaim);
                            db.SaveChanges();
                        }

                        if (permissionsVM.slice)
                        {
                            AspNetUserClaims aspNetUserClaim = new AspNetUserClaims
                            {
                                UserId = user.AspNetUserId,
                                ClaimType = EClaims.Recortar.ToString(),
                                ClaimValue = EClaims.Recortar.ToString()
                            };

                            db.AspNetUserClaims.Add(aspNetUserClaim);
                            db.SaveChanges();
                        }
                    }
                }

                #endregion

                #region .: Synchronization  Attribute :.

                List<ECMAttributeItemIn> itens = new List<ECMAttributeItemIn>
                {
                    new ECMAttributeItemIn { attribute = WebConfigurationManager.AppSettings["Repository.DocumentRepository.Attribute.PermissionDate"].ToString(), value = now.ToString("yyyy-MM-dd") },
                    new ECMAttributeItemIn { attribute = WebConfigurationManager.AppSettings["Repository.DocumentRepository.Attribute.PermissionTime"].ToString(), value = now.ToString("HH:mm") },
                    new ECMAttributeItemIn { attribute = WebConfigurationManager.AppSettings["Repository.DocumentRepository.Attribute.PermissionStatus"].ToString(), value = WebConfigurationManager.AppSettings["Repository.DocumentRepository.Attribute.PermissionStatusValue"].ToString() },
                };

                attributeApi.PostECMAttributeUpdate(new ECMAttributeIn(permissionsVM.externalId, itens));

                #endregion

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            registerEventRepository.SaveRegisterEvent(id, key, "Log - End", "Repository.PermissionRepository.PermissionsProcess", "");
        }

        private class CapUser
        {
            public int UserId { get; set; }

            public string UserLogin { get; set; }

            public string UserPassword { get; set; }

            public string UserName { get; set; }

            public int UserGroupId { get; set; }

            public bool UserStatus { get; set; }

            public bool AuthenticationAD { get; set; }
        }

        private class CapGroup
        {
            public int GroupId { get; set; }

            public string GroupName { get; set; }
        }

        #endregion
    }
}
