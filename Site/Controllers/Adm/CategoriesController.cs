using Helper.Enum;
using Model.In;
using Model.Out;
using Repository;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Site.Adm.Controllers
{
    [Authorize(Roles = "Administrador")]
    [RoutePrefix("Categories")]
    public class CategoriesController : Controller
    {
        private readonly CategoryRepository categoryRepository = new CategoryRepository();

        public ActionResult Index()
        {
            CategoriesIn categoriesIn = new CategoriesIn();

            CategoriesOut categoriesOut = categoryRepository.GetAll(categoriesIn);

            var qtdPage = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(categoriesOut.totalCount) / Convert.ToDecimal(categoriesIn.qtdEntries)));

            ViewBag.qtdEntries = categoriesIn.qtdEntries;
            ViewBag.amount = categoriesOut.totalCount;
            ViewBag.qtdPage = qtdPage;
            ViewBag.currentPage = 1;
            ViewBag.qtdActionNumber = categoriesIn.qtdActionNumber;

            return View(categoriesOut.result);
        }

        public ActionResult PartialList(int page, int qtdEntries, string filter = "", string sort = "CreatedDate", string sortdirection = "asc")
        {
            CategoriesIn categoriesIn = new CategoriesIn() { currentPage = page, qtdEntries = qtdEntries, filter = filter, sort = sort, sortdirection = sortdirection };

            CategoriesOut categoriesOut = categoryRepository.GetAll(categoriesIn);

            var qtdPage = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(categoriesOut.totalCount) / Convert.ToDecimal(categoriesIn.qtdEntries)));

            ViewBag.qtdEntries = categoriesIn.qtdEntries;
            ViewBag.amount = categoriesOut.totalCount;
            ViewBag.qtdPage = qtdPage;
            ViewBag.currentPage = categoriesIn.currentPage;
            ViewBag.qtdActionNumber = categoriesIn.qtdActionNumber;

            var jss = new System.Web.Script.Serialization.JavaScriptSerializer();
            Response.AddHeader("ViewBagHeader", jss.Serialize(new
            {
                categoriesIn.qtdEntries,
                amount = categoriesOut.totalCount,
                qtdPage,
                categoriesIn.currentPage,
                categoriesIn.qtdActionNumber,
                search = filter,
                info = string.Format(i18n.Resource.PaginationInfo, (ViewBag.qtdEntries * (ViewBag.currentPage - 1)) + 1, ((ViewBag.currentPage * ViewBag.qtdEntries) > ViewBag.amount ? ViewBag.amount : ViewBag.currentPage * ViewBag.qtdEntries), ViewBag.amount)
            }));

            return PartialView("_PartialList", categoriesOut.result);
        }

        public ActionResult Details(int id = 0)
        {
            CategoryOut categoryOut = categoryRepository.GetById(new CategoryIn { CategoryId = id });

            if (categoryOut.result == null)
            {
                return HttpNotFound();
            }

            if (Request.IsAjaxRequest())
            {
                ViewBag.HideLayout = true;
            }

            return View(categoryOut.result);
        }

        public ActionResult Edit(int id = 0)
        {
            CategoryEditOut categoryEditOut = categoryRepository.GetEditById(new CategoryIn { CategoryId = id });

            if (categoryEditOut.result == null)
            {
                return HttpNotFound();
            }

            CategoryEditIn categoryEditIn = new CategoryEditIn
            {
                CategoryId = categoryEditOut.result.CategoryId,
                Parent = categoryEditOut.result.Parent,
                Code = categoryEditOut.result.Code,
                Name = categoryEditOut.result.Name,
                pb = categoryEditOut.result.pb,
                ShowIdentifier = categoryEditOut.result.ShowIdentifier,
                ShowCompetence = categoryEditOut.result.ShowCompetence,
                ShowValidity = categoryEditOut.result.ShowValidity,
                ShowDocumentView = categoryEditOut.result.ShowDocumentView,
                Identifier = categoryEditOut.result.Identifier != null ? categoryEditOut.result.Identifier : new Model.VM.CategoryAdditionalFieldVM(categoryEditOut.result.CategoryId, (int)EAdditionalField.Identifier),
                Competence = categoryEditOut.result.Competence != null ? categoryEditOut.result.Competence : new Model.VM.CategoryAdditionalFieldVM(categoryEditOut.result.CategoryId, (int)EAdditionalField.Competence),
                Validity = categoryEditOut.result.Validity != null ? categoryEditOut.result.Validity : new Model.VM.CategoryAdditionalFieldVM(categoryEditOut.result.CategoryId, (int)EAdditionalField.Validity),
                DocumentView = categoryEditOut.result.DocumentView != null ? categoryEditOut.result.DocumentView : new Model.VM.CategoryAdditionalFieldVM(categoryEditOut.result.CategoryId, (int)EAdditionalField.DocumentView),
            };

            return View(categoryEditIn);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CategoryEditIn categoryEditIn)
        {
            if (ModelState.IsValid)
            {
                CategoryOut categoryOut = new CategoryOut();
                categoryOut = categoryRepository.Update(categoryEditIn);

                if (!categoryOut.success)
                {
                    AddErrors(categoryOut.messages);
                }

                if (ModelState.IsValid)
                {
                    return RedirectToAction("Index");
                }
            }

            return View(categoryEditIn);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                categoryRepository.Delete(id);

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