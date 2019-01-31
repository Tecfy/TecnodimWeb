using Helper.Enum;
using Helper.Utility;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Model;
using Model.In;
using Model.Out;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Site.Adm.Controllers
{
    [Authorize(Roles = "Administrador")]
    [RoutePrefix("Users")]
    public class UsersController : Controller
    {
        private readonly UserRepository userRepository = new UserRepository();
        private readonly UnityRepository unityRepository = new UnityRepository();
        private readonly UserUnityRepository userUnityRepository = new UserUnityRepository();
        private readonly AspNetRoleRepository aspNetRoleRepository = new AspNetRoleRepository();
        private ApplicationUserManager _userManager;

        public UsersController()
        {
        }

        public UsersController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
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

        public ActionResult Index()
        {
            UsersIn usersIn = new UsersIn();

            UsersOut usersOut = userRepository.GetAll(usersIn);

            var qtdPage = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(usersOut.totalCount) / Convert.ToDecimal(usersIn.qtdEntries)));

            ViewBag.qtdEntries = usersIn.qtdEntries;
            ViewBag.amount = usersOut.totalCount;
            ViewBag.qtdPage = qtdPage;
            ViewBag.currentPage = 1;
            ViewBag.qtdActionNumber = usersIn.qtdActionNumber;

            return View(usersOut.result);
        }

        public ActionResult PartialList(int page, int qtdEntries, string filter = "", string sort = "CreatedDate", string sortdirection = "asc")
        {
            UsersIn usersIn = new UsersIn() { currentPage = page, qtdEntries = qtdEntries, filter = filter, sort = sort, sortdirection = sortdirection };

            UsersOut usersOut = userRepository.GetAll(usersIn);

            var qtdPage = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(usersOut.totalCount) / Convert.ToDecimal(usersIn.qtdEntries)));

            ViewBag.qtdEntries = usersIn.qtdEntries;
            ViewBag.amount = usersOut.totalCount;
            ViewBag.qtdPage = qtdPage;
            ViewBag.currentPage = usersIn.currentPage;
            ViewBag.qtdActionNumber = usersIn.qtdActionNumber;

            var jss = new System.Web.Script.Serialization.JavaScriptSerializer();
            Response.AddHeader("ViewBagHeader", jss.Serialize(new
            {
                usersIn.qtdEntries,
                amount = usersOut.totalCount,
                qtdPage,
                usersIn.currentPage,
                usersIn.qtdActionNumber,
                search = filter,
                info = string.Format(i18n.Resource.PaginationInfo, (ViewBag.qtdEntries * (ViewBag.currentPage - 1)) + 1, ((ViewBag.currentPage * ViewBag.qtdEntries) > ViewBag.amount ? ViewBag.amount : ViewBag.currentPage * ViewBag.qtdEntries), ViewBag.amount)
            }));

            return PartialView("_PartialList", usersOut.result);
        }

        public ActionResult Details(int id = 0)
        {
            UserOut userOut = userRepository.GetById(new UserIn { UserId = id });

            if (userOut.result == null)
            {
                return HttpNotFound();
            }

            if (Request.IsAjaxRequest())
            {
                ViewBag.HideLayout = true;
            }

            return View(userOut.result);
        }

        public ActionResult Create()
        {
            #region Roles

            AspNetRolesOut aspNetRolesOut = aspNetRoleRepository.GetRoles();

            ViewBag.RolesId = new SelectList(aspNetRolesOut.result, "RoleId", "Name");

            ViewBag.RoleUser = Ready.AppSettings["Site.Areas.Adm.Controllers.Role.Usuario"];

            #endregion

            #region Claims

            ViewBag.Claims = Enum.GetValues(typeof(EClaims)).Cast<EClaims>().ToList();

            #endregion

            #region Units

            UnitsDDLOut unitsDDLOut = unityRepository.GetDDLAll();

            ViewBag.Units = new SelectList(unitsDDLOut.result, "UnityId", "Name");

            #endregion

            return View(new UserCreateIn());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(UserCreateIn userCreateIn, string[] claims, int[] units)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = userCreateIn.Email, Email = userCreateIn.Email };

                var result = await UserManager.CreateAsync(user, userCreateIn.Password);
                if (result.Succeeded)
                {
                    user.Roles.Add(new Microsoft.AspNet.Identity.EntityFramework.IdentityUserRole()
                    {
                        RoleId = userCreateIn.RoleId,
                        UserId = user.Id
                    });

                    if (claims != null && claims.Count() > 0)
                    {
                        foreach (var item in claims)
                        {
                            await UserManager.AddClaimAsync(user.Id, new System.Security.Claims.Claim(item, item));
                        }
                    }

                    await UserManager.UpdateAsync(user);

                    userCreateIn.AspNetUserId = user.Id;

                    UserOut userOut = new UserOut();
                    userOut = userRepository.Insert(userCreateIn);

                    if (!userOut.success)
                    {
                        AddErrors(userOut.messages);
                    }
                    else
                    {
                        if (units != null && units.Count() > 0)
                        {
                            foreach (var item in units)
                            {
                                userUnityRepository.Insert(new UserUnityCreateIn { UnityId = item, UserId = userOut.result.UserId });
                            }
                        }
                    }
                }
                else
                {
                    AddErrors(result);
                }

                if (ModelState.IsValid)
                {
                    return RedirectToAction("Index");
                }
            }

            #region Roles

            AspNetRolesOut aspNetRolesOut = aspNetRoleRepository.GetRoles();

            ViewBag.RolesId = new SelectList(aspNetRolesOut.result, "RoleId", "Name");

            ViewBag.RoleUser = Ready.AppSettings["Site.Areas.Adm.Controllers.Role.Usuario"];

            #endregion

            #region Claims

            ViewBag.Claims = Enum.GetValues(typeof(EClaims)).Cast<EClaims>().ToList();

            #endregion

            #region Units

            UnitsDDLOut unitsDDLOut = unityRepository.GetDDLAll();

            ViewBag.Units = new SelectList(unitsDDLOut.result, "UnityId", "Name");

            #endregion

            return View(userCreateIn);
        }

        public ActionResult Edit(int id = 0)
        {
            UserEditOut userEditOut = userRepository.GetEditById(new UserIn { UserId = id });

            if (userEditOut.result == null)
            {
                return HttpNotFound();
            }

            UserEditIn userEditIn = new UserEditIn
            {
                UserId = userEditOut.result.UserId,
                AspNetUserId = userEditOut.result.AspNetUserId,
                RoleId = userEditOut.result.RoleId,
                FirstName = userEditOut.result.FirstName,
                LastName = userEditOut.result.LastName,
                Email = userEditOut.result.Email,
                Claims = userEditOut.result.Claims,
                Units = userEditOut.result.Units,
                Registration = userEditOut.result.Registration,
            };

            #region Roles

            AspNetRolesOut aspNetRolesOut = aspNetRoleRepository.GetRoles();

            ViewBag.RolesId = new SelectList(aspNetRolesOut.result, "RoleId", "Name");

            ViewBag.RoleUser = Ready.AppSettings["Site.Areas.Adm.Controllers.Role.Usuario"];

            #endregion

            #region Claims

            ViewBag.Claims = Enum.GetValues(typeof(EClaims)).Cast<EClaims>().ToList();

            #endregion

            #region Units

            UnitsDDLOut unitsDDLOut = unityRepository.GetDDLAll();

            ViewBag.Units = new SelectList(unitsDDLOut.result, "UnityId", "Name");

            #endregion

            return View(userEditIn);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(UserEditIn userEditIn, string[] claims, int[] units)
        {
            if (ModelState.IsValid)
            {
                if (userEditIn.NewPassword)
                {
                    var validate = await UserManager.PasswordValidator.ValidateAsync(userEditIn.Password);

                    if (validate.Succeeded)
                    {
                        await UserManager.RemovePasswordAsync(userEditIn.AspNetUserId);

                        await UserManager.AddPasswordAsync(userEditIn.AspNetUserId, userEditIn.Password);
                    }
                    else
                    {
                        AddErrors(validate);
                    }
                }

                if (claims != null && claims.Count() > 0)
                {
                    var claimsOld = await UserManager.GetClaimsAsync(userEditIn.AspNetUserId);

                    foreach (var item in claimsOld)
                    {
                        if (!claims.Contains(item.ValueType))
                        {
                            await UserManager.RemoveClaimAsync(userEditIn.AspNetUserId, item);
                        }
                    }

                    foreach (var item in claims)
                    {
                        if (!claimsOld.Any(x => x.ValueType == item))
                        {
                            await UserManager.AddClaimAsync(userEditIn.AspNetUserId, new System.Security.Claims.Claim(item, item));
                        }
                    }
                }
                else
                {
                    var claimsOld = await UserManager.GetClaimsAsync(userEditIn.AspNetUserId);

                    foreach (var item in claimsOld)
                    {
                        await UserManager.RemoveClaimAsync(userEditIn.AspNetUserId, item);
                    }
                }

                if (ModelState.IsValid)
                {
                    var user = UserManager.FindById(userEditIn.AspNetUserId);

                    if (!user.Roles.Any(x => x.RoleId == userEditIn.RoleId))
                    {
                        var roles = await UserManager.GetRolesAsync(userEditIn.AspNetUserId);
                        await UserManager.RemoveFromRolesAsync(userEditIn.AspNetUserId, roles.ToArray());

                        user.Roles.Add(new Microsoft.AspNet.Identity.EntityFramework.IdentityUserRole()
                        {
                            RoleId = userEditIn.RoleId,
                            UserId = user.Id
                        });
                    }

                    var result = await UserManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        userUnityRepository.DeleteUnits(userEditIn.UserId);

                        if (units != null && units.Count() > 0)
                        {
                            foreach (var item in units)
                            {
                                userUnityRepository.Insert(new UserUnityCreateIn { UnityId = item, UserId = userEditIn.UserId });
                            }
                        }

                        UserOut userOut = new UserOut();
                        userOut = userRepository.Update(userEditIn);

                        if (!userOut.success)
                        {
                            AddErrors(userOut.messages);
                        }
                    }
                    else
                    {
                        AddErrors(result);
                    }

                    if (ModelState.IsValid)
                    {
                        return RedirectToAction("Index");
                    }
                }
            }

            #region Roles

            AspNetRolesOut aspNetRolesOut = aspNetRoleRepository.GetRoles();

            ViewBag.RolesId = new SelectList(aspNetRolesOut.result, "RoleId", "Name");

            ViewBag.RoleUser = Ready.AppSettings["Site.Areas.Adm.Controllers.Role.Usuario"];

            #endregion

            #region Claims

            ViewBag.Claims = Enum.GetValues(typeof(EClaims)).Cast<EClaims>().ToList();

            #endregion

            #region Units

            UnitsDDLOut unitsDDLOut = unityRepository.GetDDLAll();

            ViewBag.Units = new SelectList(unitsDDLOut.result, "UnityId", "Name");

            #endregion

            return View(userEditIn);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                userRepository.Delete(id);

                if (Request.IsAjaxRequest())
                {
                    return Json(new { status = "OK", id }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                string msg; try { msg = ex.InnerException.InnerException.Message; }
                catch { msg = ex.Message; }
                if (msg.Contains("register(s) relationated"))
                {
                    msg = i18n.Resource.HaveDependentsRegisters;
                }
                if (Request.IsAjaxRequest())
                {
                    return Json(new { status = "ERROR", id, msg }, JsonRequestBehavior.AllowGet);
                }
            }

            return RedirectToAction("Index");
        }

        #region .: Helper :.

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }
            }

            base.Dispose(disposing);
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private void AddErrors(List<string> messages)
        {
            foreach (var error in messages)
            {
                ModelState.AddModelError("", error);
            }
        }

        #endregion
    }
}