using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Tecnodim.App_Start;
using Tecnodim.Models;

namespace Tecnodim.Controllers
{
    [Authorize]
    [RoutePrefix("api/account")]
    public class AccountController : ApiController
    {
        private const string LocalLoginProvider = "Local";
        private ApplicationUserManager _userManager;

        public AccountController()
        {
        }

        [HttpPost, Route("Login"), AllowAnonymous]
        public Return<AccessResultModel> Login([FromBody]AccessModel model)
        {
            Return<AccessResultModel> retorno = new Return<AccessResultModel>();
            try
            {
                HttpClient client = new HttpClient();
                AccessResultModel accessModel = new AccessResultModel();
                client.BaseAddress = new Uri(Url.Content("~/"));
                HttpResponseMessage response = client.PostAsync("Token", new StringContent(string.Format("grant_type=password&username={0}&password={1}", HttpUtility.UrlEncode(model.UserName), HttpUtility.UrlEncode(model.Password)), System.Text.Encoding.UTF8, "application/x-www-form-urlencoded")).Result;
                string ret = response.Content.ReadAsStringAsync().Result;
                accessModel = JsonConvert.DeserializeObject<AccessResultModel>(ret.Replace(".issued", "issued").Replace(".expires", "expires"));
                retorno = new Return<AccessResultModel>(accessModel, retorno.Key, true);
            }
            catch (Exception ex)
            {
                retorno = new Return<AccessResultModel>(ex, retorno.Key, "Tecnodim", "", "Account", "Login", model.ToString());
            }
            return retorno;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        [AllowAnonymous]
        [Route("Register")]
        public async Task<IHttpActionResult> Register(RegisterBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new ApplicationUser() { UserName = model.Email, Email = model.Email };

            IdentityResult result = await UserManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }

            base.Dispose(disposing);
        }

        #region Helpers

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }

        public class AccessModel
        {
            public string UserName { get; set; }
            public string Password { get; set; }
        }

        public class AccessResultModel
        {
            public string userName { get; set; }
            public string access_token { get; set; }
            public string token_type { get; set; }
            public string expires { get; set; }
        }

        #endregion
    }
}
