using DataEF.DataAccess;
using Helper.Enum;
using Helper.Utility;
using Model.In;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Site.Areas.Adm.Controllers
{
    [Authorize]
    [RoutePrefix("Adm/User")]
    public class UserController : Controller
    {
        private int qtdEntries = Ready.AppSettings["Pagination.qtdEntries"].ToInt();
        private int qtdActionNumber = Ready.AppSettings["Pagination.qtdActionNumber"].ToInt();

        private DBContext db = new DBContext();

        public ActionResult Index()
        {
            var query = db.Users.Where(e => e.Active == true && e.DeletedDate == null);

            var count = query.Count();

            if (Session["qtdEntries"] != null)
            {
                qtdEntries = Session["qtdEntries"].ToString().ToInt();
            }

            query = query.OrderBy(e => e.CreatedDate)
                         .Skip((1 - 1) * qtdEntries)
                         .Take(qtdEntries);

            var qtdPage = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(count) / Convert.ToDecimal(qtdEntries)));

            ViewBag.qtdEntries = qtdEntries;
            ViewBag.amount = count;
            ViewBag.qtdPage = qtdPage;
            ViewBag.currentPage = 1;
            ViewBag.qtdActionNumber = qtdActionNumber;

            return View(query.ToList());
        }

        public ActionResult PartialList(int page, int qtdEntries, string filter = "", string sort = "", string sortdirection = "asc")
        {
            var query = db.Users.Where(e => e.Active == true && e.DeletedDate == null);

            var count = query.Count();

            Session["qtdEntries"] = qtdEntries;

            if (string.IsNullOrEmpty(sort))
            {
                sort = "CreatedDate";
            }

            query = query.OrderBy(sort, !sortdirection.Equals("asc"))
                         .Skip((page - 1) * qtdEntries)
                         .Take(qtdEntries);

            var qtdPage = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(count) / Convert.ToDecimal(qtdEntries)));

            ViewBag.qtdEntries = qtdEntries;
            ViewBag.amount = count;
            ViewBag.qtdPage = qtdPage;
            ViewBag.currentPage = page;
            ViewBag.qtdActionNumber = qtdActionNumber;

            var jss = new System.Web.Script.Serialization.JavaScriptSerializer();
            Response.AddHeader("ViewBagHeader", jss.Serialize(new
            {
                qtdEntries,
                amount = count,
                qtdPage,
                currentPage = page,
                qtdActionNumber,
                search = filter,
                info = string.Format(i18n.Resource.PaginationInfo, (ViewBag.qtdEntries * (ViewBag.currentPage - 1)) + 1, ((ViewBag.currentPage * ViewBag.qtdEntries) > ViewBag.amount ? ViewBag.amount : ViewBag.currentPage * ViewBag.qtdEntries), ViewBag.amount)
            }));

            return PartialView("_PartialList", query.ToList());
        }

        public ActionResult Details(int id = 0)
        {
            UserIn user = db.Users
                            .Where(x => x.Active == true && x.DeletedDate == null && x.UserId == id).Select(x => new UserIn()
                            {
                                UserId = x.UserId,
                                AspNetUserId = x.AspNetUserId,
                                RoleId = x.AspNetUsers.AspNetUserRoles.FirstOrDefault().RoleId,
                                FirstName = x.FirstName,
                                LastName = x.LastName,
                                Email = x.AspNetUsers.Email,
                            }).FirstOrDefault();

            if (user == null)
            {
                return HttpNotFound();
            }

            if (Request.IsAjaxRequest())
            {
                ViewBag.HideLayout = true;
            }

            return View(user);
        }

        public ActionResult Create()
        {
            #region Roles

            ViewBag.RoleId = new SelectList(db.AspNetRoles.OrderBy(x => x.Name).ToList(), "Id", "Name");

            #endregion

            #region Claims

            ViewBag.Claims = Enum.GetValues(typeof(EClaims)).Cast<EClaims>().ToList();

            #endregion

            return View(new UserIn());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UserIn user)
        {
            if (ModelState.IsValid)
            {
                /*
                using (var scope = new System.Transactions.TransactionScope())
                {
                    users.CompanyId = user.CustomerId;
                    if (!string.IsNullOrEmpty(users.Password))
                    {
                        users.Password = DataEF.Security.Generator.Password(users.Password);
                    }
                    db.Users.Add(users);

                    if (ProfileEdit == users.ProfileId || ProfileGestor == users.ProfileId)
                    {
                        #region .: Inclusão de Players :.

                        if (playersIds != null && playersIds.Count() > 0)
                        {
                            foreach (var item in playersIds)
                            {
                                UserPlayers userPlayers = new UserPlayers();
                                userPlayers.UserId = users.UserId;
                                userPlayers.PlayerId = item;
                                db.UserPlayers.Add(userPlayers);
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("playersIds", string.Format(i18n.Resource.RequiredMessage, i18n.Resource.Players));
                        }

                        #endregion

                        #region .: Inclusão de Templates :.

                        if (templatesIds != null && templatesIds.Count() > 0)
                        {
                            foreach (var item in templatesIds)
                            {
                                UserTemplates userTemplates = new UserTemplates();
                                userTemplates.UserId = users.UserId;
                                userTemplates.TemplateId = item;
                                db.UserTemplates.Add(userTemplates);
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("templatesIds", string.Format(i18n.Resource.RequiredMessage, i18n.Resource.Templates));
                        }

                        #endregion

                        if (ModelState.IsValid)
                        {
                            db.SaveChanges();
                            scope.Complete();
                        }
                    }
                    else
                    {
                        db.SaveChanges();
                        scope.Complete();
                    }
                }
                */

                if (ModelState.IsValid)
                {
                    return RedirectToAction("Index");
                }
            }

            #region Roles

            ViewBag.RoleId = new SelectList(db.AspNetRoles.OrderBy(x => x.Name).ToList(), "Id", "Name");

            #endregion

            #region Claims

            ViewBag.Claims = Enum.GetValues(typeof(EClaims)).Cast<EClaims>().ToList();

            #endregion

            return View(user);
        }

        public ActionResult Edit(int id = 0)
        {
            UserIn user = db.Users
                            .Where(x => x.Active == true && x.DeletedDate == null && x.UserId == id).Select(x => new UserIn()
                            {
                                UserId = x.UserId,
                                AspNetUserId = x.AspNetUserId,
                                RoleId = x.AspNetUsers.AspNetUserRoles.FirstOrDefault().RoleId,
                                FirstName = x.FirstName,
                                LastName = x.LastName,
                                Email = x.AspNetUsers.Email,
                            }).FirstOrDefault();

            if (user == null)
            {
                return HttpNotFound();
            }

            #region Roles

            ViewBag.RoleId = new SelectList(db.AspNetRoles.OrderBy(x => x.Name).ToList(), "Id", "Name");

            #endregion

            #region Claims

            ViewBag.Claims = Enum.GetValues(typeof(EClaims)).Cast<EClaims>().ToList();

            #endregion

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UserIn user)
        {
            if (ModelState.IsValid)
            {
                /*
                try
                {
                    using (var scope = new System.Transactions.TransactionScope())
                    {
                        if (NewPassword != "")
                        {
                            users.Password = DataEF.Security.Generator.Password(NewPassword);
                        }

                        users.EditedDate = DateTime.Now;
                        db.Entry(users).State = EntityState.Modified;

                        if (ProfileEdit == users.ProfileId || ProfileGestor == users.ProfileId)
                        {
                            #region .: Inclusão de Players :.

                            if (playersIds != null && playersIds.Count() > 0)
                            {
                                var oldPlayers = db.UserPlayers.Where(x => x.UserId == users.UserId).ToList();

                                var removePlayers = oldPlayers.Where(e => playersIds.Contains(e.PlayerId) == false).ToList();

                                foreach (var item in removePlayers)
                                {
                                    db.Entry(item).State = EntityState.Deleted;
                                }

                                var addPlayers = playersIds.Where(e => oldPlayers.Count(ee => ee.PlayerId == e) <= 0).Distinct().ToList();

                                foreach (var item in addPlayers)
                                {
                                    UserPlayers userPlayers = new UserPlayers();
                                    userPlayers.UserId = users.UserId;
                                    userPlayers.PlayerId = item;
                                    db.UserPlayers.Add(userPlayers);
                                }
                            }
                            else
                            {
                                ModelState.AddModelError("playersIds", string.Format(i18n.Resource.RequiredMessage, i18n.Resource.Players));
                            }

                            #endregion

                            #region .: Inclusão de Templates :.

                            if (templatesIds != null && templatesIds.Count() > 0)
                            {
                                var oldTemplates = db.UserTemplates.Where(x => x.UserId == users.UserId).ToList();

                                var removeTemplates = oldTemplates.Where(e => templatesIds.Contains(e.TemplateId) == false).ToList();

                                foreach (var item in removeTemplates)
                                {
                                    db.Entry(item).State = EntityState.Deleted;
                                }

                                var addTemplates = templatesIds.Where(e => oldTemplates.Count(ee => ee.TemplateId == e) <= 0).Distinct().ToList();

                                foreach (var item in addTemplates)
                                {
                                    UserTemplates userTemplates = new UserTemplates();
                                    userTemplates.UserId = users.UserId;
                                    userTemplates.TemplateId = item;
                                    db.UserTemplates.Add(userTemplates);
                                }
                            }
                            else
                            {
                                ModelState.AddModelError("templatesIds", string.Format(i18n.Resource.RequiredMessage, i18n.Resource.Templates));
                            }

                            #endregion

                            if (ModelState.IsValid)
                            {
                                db.SaveChanges();
                                scope.Complete();
                            }
                        }
                        else
                        {
                            #region .: UserPlayers :.

                            var oldPlayers = db.UserPlayers.Where(x => x.UserId == users.UserId).ToList();

                            foreach (var item in oldPlayers)
                            {
                                db.Entry(item).State = EntityState.Deleted;
                            }

                            #endregion

                            #region .: UserTemplates :.

                            var oldTemplates = db.UserTemplates.Where(x => x.UserId == users.UserId).ToList();

                            foreach (var item in oldTemplates)
                            {
                                db.Entry(item).State = EntityState.Deleted;
                            }

                            #endregion

                            db.SaveChanges();
                            scope.Complete();
                        }
                    }

                    if (ModelState.IsValid)
                    {
                        return RedirectToAction("Index");
                    }
                }
                catch (Exception ex)
                {

                }
                */
            }

            #region Roles

            ViewBag.RoleId = new SelectList(db.AspNetRoles.OrderBy(x => x.Name).ToList(), "Id", "Name");

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
                Users users = db.Users.Find(id);
                users.Active = false;
                users.DeletedDate = DateTime.Now;

                db.SaveChanges();

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

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}