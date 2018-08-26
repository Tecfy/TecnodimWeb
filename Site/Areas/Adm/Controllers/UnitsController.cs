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
namespace Site.Areas.Adm.Controllers
{
    [Authorize(Roles = "Administrador")]
    [RoutePrefix("Adm/Units")]
    public class UnitsController : Controller
    {
        private readonly UnityRepository unityRepository = new UnityRepository();

        public ActionResult Index()
        {
            UnitsIn unitsIn = new UnitsIn();

            UnitsOut unitsOut = unityRepository.GetAll(unitsIn);

            var qtdPage = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(unitsOut.totalCount) / Convert.ToDecimal(unitsIn.qtdEntries)));

            ViewBag.qtdEntries = unitsIn.qtdEntries;
            ViewBag.amount = unitsOut.totalCount;
            ViewBag.qtdPage = qtdPage;
            ViewBag.currentPage = 1;
            ViewBag.qtdActionNumber = unitsIn.qtdActionNumber;

            return View(unitsOut.result);
        }

        public ActionResult PartialList(int page, int qtdEntries, string filter = "", string sort = "CreatedDate", string sortdirection = "asc")
        {
            UnitsIn unitsIn = new UnitsIn() { currentPage = page, qtdEntries = qtdEntries, filter = filter, sort = sort, sortdirection = sortdirection };

            UnitsOut unitsOut = unityRepository.GetAll(unitsIn);

            var qtdPage = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(unitsOut.totalCount) / Convert.ToDecimal(unitsIn.qtdEntries)));

            ViewBag.qtdEntries = unitsIn.qtdEntries;
            ViewBag.amount = unitsOut.totalCount;
            ViewBag.qtdPage = qtdPage;
            ViewBag.currentPage = unitsIn.currentPage;
            ViewBag.qtdActionNumber = unitsIn.qtdActionNumber;

            var jss = new System.Web.Script.Serialization.JavaScriptSerializer();
            Response.AddHeader("ViewBagHeader", jss.Serialize(new
            {
                unitsIn.qtdEntries,
                amount = unitsOut.totalCount,
                qtdPage,
                unitsIn.currentPage,
                unitsIn.qtdActionNumber,
                search = filter,
                info = string.Format(i18n.Resource.PaginationInfo, (ViewBag.qtdEntries * (ViewBag.currentPage - 1)) + 1, ((ViewBag.currentPage * ViewBag.qtdEntries) > ViewBag.amount ? ViewBag.amount : ViewBag.currentPage * ViewBag.qtdEntries), ViewBag.amount)
            }));

            return PartialView("_PartialList", unitsOut.result);
        }

        public ActionResult Details(int id = 0)
        {
            UnityOut unityOut = unityRepository.GetById(new UnityIn { UnityId = id });

            if (unityOut.result == null)
            {
                return HttpNotFound();
            }

            if (Request.IsAjaxRequest())
            {
                ViewBag.HideLayout = true;
            }

            return View(unityOut.result);
        }

        public ActionResult Create()
        {
            return View(new UnityCreateIn());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UnityCreateIn unityCreateIn)
        {
            if (ModelState.IsValid)
            {
                UnityOut unityOut = new UnityOut();
                unityOut = unityRepository.Insert(unityCreateIn);

                if (!unityOut.success)
                {
                    AddErrors(unityOut.messages);
                }

                if (ModelState.IsValid)
                {
                    return RedirectToAction("Index");
                }
            }

            return View(unityCreateIn);
        }

        public ActionResult Edit(int id = 0)
        {
            UnityEditOut unityEditOut = unityRepository.GetEditById(new UnityIn { UnityId = id });

            if (unityEditOut.result == null)
            {
                return HttpNotFound();
            }

            UnityEditIn unityEditIn = new UnityEditIn
            {
                UnityId = unityEditOut.result.UnityId,
                ExternalId = unityEditOut.result.ExternalId,
                Name = unityEditOut.result.Name,
            };

            return View(unityEditIn);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UnityEditIn unityEditIn)
        {
            if (ModelState.IsValid)
            {
                UnityOut unityOut = new UnityOut();
                unityOut = unityRepository.Update(unityEditIn);

                if (!unityOut.success)
                {
                    AddErrors(unityOut.messages);
                }

                if (ModelState.IsValid)
                {
                    return RedirectToAction("Index");
                }
            }

            return View(unityEditIn);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                unityRepository.Delete(id);

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