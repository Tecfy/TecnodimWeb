using DataEF.DataAccess;
using Helper.Enum;
using Helper.Utility;
using Model.In;
using Model.Out;
using Model.VM;
using Repository;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Site.Areas.Adm.Controllers
{
    [Authorize]
    [RoutePrefix("Adm/User")]
    public class UserController : Controller
    {
        private readonly UserRepository userRepository = new UserRepository();
        private readonly AspNetRoleRepository aspNetRoleRepository = new AspNetRoleRepository();

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

            ViewBag.RoleId = new SelectList(aspNetRolesOut.result, "RoleId", "Name");

            #endregion

            #region Claims

            ViewBag.Claims = Enum.GetValues(typeof(EClaims)).Cast<EClaims>().ToList();

            #endregion

            return View(new UserCreateIn());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UserCreateIn user)
        {
            if (ModelState.IsValid)
            {
                userRepository.Insert(user);

                if (ModelState.IsValid)
                {
                    return RedirectToAction("Index");
                }
            }

            #region Roles

            AspNetRolesOut aspNetRolesOut = aspNetRoleRepository.GetRoles();

            ViewBag.RoleId = new SelectList(aspNetRolesOut.result, "RoleId", "Name");

            #endregion

            #region Claims

            ViewBag.Claims = Enum.GetValues(typeof(EClaims)).Cast<EClaims>().ToList();

            #endregion

            return View(user);
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
                Email = userEditOut.result.Email
            };

            #region Roles

            AspNetRolesOut aspNetRolesOut = aspNetRoleRepository.GetRoles();

            ViewBag.RoleId = new SelectList(aspNetRolesOut.result, "RoleId", "Name");

            #endregion

            #region Claims

            ViewBag.Claims = Enum.GetValues(typeof(EClaims)).Cast<EClaims>().ToList();

            #endregion

            return View(userEditIn);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UserEditIn user)
        {
            if (ModelState.IsValid)
            {
                userRepository.Update(user);

                if (ModelState.IsValid)
                {
                    return RedirectToAction("Index");
                }
            }

            #region Roles

            AspNetRolesOut aspNetRolesOut = aspNetRoleRepository.GetRoles();

            ViewBag.RoleId = new SelectList(aspNetRolesOut.result, "RoleId", "Name");

            #endregion

            #region Claims

            ViewBag.Claims = Enum.GetValues(typeof(EClaims)).Cast<EClaims>().ToList();

            #endregion

            return View(user);
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
    }
}