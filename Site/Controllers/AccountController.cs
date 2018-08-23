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

namespace Site.Controllers
{
    [Authorize]
    [RoutePrefix("Api/Account")]
    public class AccountController : ApiController
    {
        RegisterEventRepository registerEventRepository = new RegisterEventRepository();

        [HttpPost, Route("Login"), AllowAnonymous]
        public AccessResultOut Login(AccessIn accessIn)
        {
            AccessResultOut accessResultOut = new AccessResultOut();
            Guid Key = Guid.NewGuid();

            try
            {
                HttpClient client = new HttpClient
                {
                    BaseAddress = new Uri(Url.Content("~/"))
                };

                HttpResponseMessage response = client.PostAsync("Token", new StringContent(string.Format("grant_type=password&username={0}&password={1}", HttpUtility.UrlEncode(accessIn.userName), HttpUtility.UrlEncode(accessIn.password)), System.Text.Encoding.UTF8, "application/x-www-form-urlencoded")).Result;

                string ret = response.Content.ReadAsStringAsync().Result;

                accessResultOut.result = JsonConvert.DeserializeObject<AccessResultVM>(ret.Replace(".issued", "issued").Replace(".expires", "expires"));
            }
            catch (Exception ex)
            {
                registerEventRepository.SaveRegisterEvent(new Guid(User.Identity.GetUserId()), Key, "Erro", "Site.Controllers.AccountController.Login", ex.Message);

                accessResultOut.successMessage = null;
                accessResultOut.messages.Add(ex.Message);
            }

            return accessResultOut;
        }
    }
}
