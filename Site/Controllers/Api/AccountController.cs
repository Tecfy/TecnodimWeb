using Microsoft.AspNet.Identity;
using Model.In;
using Model.Out;
using Model.VM;
using Newtonsoft.Json;
using Repository;
using System;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Site.Api.Controllers
{
    [Authorize(Roles = "Usuário")]
    [RoutePrefix("Api/Account")]
    public class AccountController : ApiController
    {
        private RegisterEventRepository registerEventRepository = new RegisterEventRepository();
        private UnityRepository unityRepository = new UnityRepository();
        private UserRepository userRepository = new UserRepository();

        [HttpPost, Route("Login"), AllowAnonymous]
        public AccessResultOut Login(AccessIn accessIn)
        {
            AccessResultOut accessResultOut = new AccessResultOut();
            string Key = Guid.NewGuid().ToString();

            try
            {
                HttpClient client = new HttpClient
                {
                    BaseAddress = new Uri(Url.Content("~/"))
                };

                HttpResponseMessage response = client.PostAsync("Token", new StringContent(string.Format("grant_type=password&username={0}&password={1}&client_id={2}", HttpUtility.UrlEncode(accessIn.userName), HttpUtility.UrlEncode(accessIn.password), HttpUtility.UrlEncode("0")), System.Text.Encoding.UTF8, "application/x-www-form-urlencoded")).Result;

                string ret = response.Content.ReadAsStringAsync().Result;

                accessResultOut.result = JsonConvert.DeserializeObject<AccessResultVM>(ret.Replace(".issued", "issued").Replace(".expires", "expires"));

                if (accessResultOut.result != null)
                {
                    accessResultOut.result.Units = unityRepository.GetDDLAllByAspNetUserId(accessResultOut.result.aspNetUserId).result;
                    accessResultOut.result.name = userRepository.GetNameByAspNetUserId(accessResultOut.result.aspNetUserId);
                }

            }
            catch (Exception ex)
            {
                registerEventRepository.SaveRegisterEvent(User.Identity.GetUserId(), Key, "Erro", "Site.Controllers.AccountController.Login", ex.Message);

                accessResultOut.successMessage = null;
                accessResultOut.messages.Add(ex.Message);
            }

            return accessResultOut;
        }

        [HttpPost, Route("LoginExternal"), AllowAnonymous]
        public AccessResultOut LoginExternal(AccessExternalIn accessExternalIn)
        {
            AccessResultOut accessResultOut = new AccessResultOut();
            string Key = Guid.NewGuid().ToString();

            try
            {
                HttpClient client = new HttpClient
                {
                    BaseAddress = new Uri(Url.Content("~/"))
                };

                UserByTokenOut userByTokenOut = userRepository.GetByToken(new UserByTokenIn { Token = accessExternalIn.token });

                if (userByTokenOut.result == null)
                {
                    throw new Exception(i18n.Resource.RegisterNotFound);
                }

                HttpResponseMessage response = client.PostAsync("Token", new StringContent(string.Format("grant_type=password&username={0}&client_id={1}", HttpUtility.UrlEncode(userByTokenOut.result.Registration), HttpUtility.UrlEncode("1")), System.Text.Encoding.UTF8, "application/x-www-form-urlencoded")).Result;

                string ret = response.Content.ReadAsStringAsync().Result;

                accessResultOut.result = JsonConvert.DeserializeObject<AccessResultVM>(ret.Replace(".issued", "issued").Replace(".expires", "expires"));

                if (accessResultOut.result != null)
                {
                    accessResultOut.result.Units = unityRepository.GetDDLAllByAspNetUserId(accessResultOut.result.aspNetUserId).result;
                    accessResultOut.result.name = userRepository.GetNameByAspNetUserId(accessResultOut.result.aspNetUserId);
                }

            }
            catch (Exception ex)
            {
                registerEventRepository.SaveRegisterEvent(User.Identity.GetUserId(), Key, "Erro", "Site.Controllers.AccountController.Login", ex.Message);

                accessResultOut.successMessage = null;
                accessResultOut.messages.Add(ex.Message);
            }

            return accessResultOut;
        }
    }
}
