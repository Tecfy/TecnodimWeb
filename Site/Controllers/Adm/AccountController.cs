using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Model;
using Model.In;
using Model.Out;
using Model.VM;
using Repository;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace Site.Adm.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class AccountController : Controller
    {
        private readonly UserRepository userRepository = new UserRepository();
        private readonly UserUnityRepository userUnityRepository = new UserUnityRepository();
        private readonly UnityRepository unityRepository = new UnityRepository();
        private readonly PermissionRepository permissionRepository = new PermissionRepository();
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginVM loginVM, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(loginVM);
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(loginVM.Email, loginVM.Password, loginVM.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = loginVM.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", i18n.Resource.InvalidLoginAttempt);
                    return View(loginVM);
            }
        }

        [AllowAnonymous]
        public ActionResult ExternalLogin(string provider, string token, string returnUrl)
        {
            //// Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl, Token = token }));
        }

        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl, string token)
        {
            UserCreateExternalIn userCreateExternalIn = new UserCreateExternalIn { Token = token };
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();

            if (loginInfo == null)
            {
                return Redirect(returnUrl + "?type=error&message=Usuário não encontrado!");
            }
            else
            {
                var identity = (ClaimsIdentity)loginInfo.ExternalIdentity;

                if (identity.IsAuthenticated)
                {
                    foreach (Claim claim in identity.Claims)
                    {
                        if (claim.Type == WebConfigurationManager.AppSettings["ADFS.FirstName"])
                        {
                            userCreateExternalIn.FirstName = claim.Value;
                        }
                        else if (claim.Type == WebConfigurationManager.AppSettings["ADFS.LastName"])
                        {
                            userCreateExternalIn.LastName = claim.Value;
                        }
                        else if (claim.Type == WebConfigurationManager.AppSettings["ADFS.Registration"])
                        {
                            userCreateExternalIn.Registration = claim.Value;
                        }
                        else if (claim.Type == WebConfigurationManager.AppSettings["ADFS.Email"])
                        {
                            userCreateExternalIn.Email = claim.Value;
                        }
                        else if (claim.Type == WebConfigurationManager.AppSettings["ADFS.Unit"])
                        {
                            userCreateExternalIn.Unit = claim.Value;
                        }
                    }
                }
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:

                    var applicationUser = await UserManager.FindByNameAsync(userCreateExternalIn.Registration);

                    userRepository.Update(applicationUser.Id, token);

                    return Redirect(returnUrl + "?type=success&message=Sucesso&token=" + token);

                case SignInStatus.Failure:
                default:

                    var user = new ApplicationUser { UserName = userCreateExternalIn.Registration, Email = userCreateExternalIn.Email };
                    var resultUser = await UserManager.CreateAsync(user);
                    if (resultUser.Succeeded)
                    {
                        resultUser = await UserManager.AddLoginAsync(user.Id, loginInfo.Login);
                        if (resultUser.Succeeded)
                        {
                            user.Roles.Add(new Microsoft.AspNet.Identity.EntityFramework.IdentityUserRole()
                            {
                                RoleId = WebConfigurationManager.AppSettings["Site.Areas.Adm.Controllers.Role.Usuario"],
                                UserId = user.Id
                            });

                            await UserManager.UpdateAsync(user);

                            userCreateExternalIn.AspNetUserId = user.Id;

                            UserOut userOut = new UserOut();
                            userOut = userRepository.Insert(userCreateExternalIn);

                            int? unityId = unityRepository.GetByCode(userCreateExternalIn.Unit);

                            userUnityRepository.Insert(new UserUnityCreateIn { UnityId = unityId.Value, UserId = userOut.result.UserId });

                            await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                            PermissionIn permissionIn = new PermissionIn { Registration = userCreateExternalIn.Registration, key = Guid.NewGuid().ToString() };

                            permissionRepository.SetPermission(permissionIn);

                            return Redirect(returnUrl + "?type=success&message=Sucesso&token=" + token);
                        }
                        else
                        {
                            string msg = string.Empty;

                            foreach (var item in resultUser.Errors)
                            {
                                if (string.IsNullOrEmpty(msg))
                                    msg = item.ToString();
                                else
                                    msg += "</br>" + item.ToString();
                            }

                            return Redirect(returnUrl + "?type=error&message=" + msg);
                        }
                    }
                    else
                    {
                        string msg = string.Empty;

                        foreach (var item in resultUser.Errors)
                        {
                            if (string.IsNullOrEmpty(msg))
                                msg = item.ToString();
                            else
                                msg += "</br>" + item.ToString();
                        }

                        return Redirect(returnUrl + "?type=error&message=" + msg);
                    }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        public ActionResult LogOff(string returnUrl)
        {
            AuthenticationManager.SignOut();
            return Redirect(returnUrl);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home", new { area = "Adm" });
        }

        private const string XsrfKey = "XsrfId";

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }

        #endregion
    }
}