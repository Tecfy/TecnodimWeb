using DataEF.DataAccess;
using Helper.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Site.Areas.Adm.Controllers
{
    [Authorize]
    [RoutePrefix("Adm/User")]
    public class UserController : Controller
    {
        private int qtdEntries = Ready.AppSettings["Pagination.qtdEntries"].ToInt();

        private DBContext db = new DBContext();

        public ActionResult Index()
        {
            int qtdActionNumber = Ready.AppSettings["Pagination.qtdActionNumber"].ToInt();
            var query = db.Users.Where(e =>
                                        e.Active == false
                                        &&
                                        e.DeletedDate == null
                                      );

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
            ViewBag.qtdActionNumber = 10;

            return View(query.ToList());
        }

        public ActionResult PartialList(int page, int qtdEntries, string filter = "", string sort = "", string sortdirection = "asc")
        {
            var query = db.Users.Where(e =>
                                        e.Active == false
                                        &&
                                        e.DeletedDate == null
                                      );

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
            ViewBag.qtdActionNumber = 10;

            var jss = new System.Web.Script.Serialization.JavaScriptSerializer();
            Response.AddHeader("ViewBagHeader", jss.Serialize(new
            {
                qtdEntries = qtdEntries,
                amount = count,
                qtdPage = qtdPage,
                currentPage = page,
                qtdActionNumber = 10,
                search = filter,
                info = string.Format(i18n.Resource.PaginationInfo, (ViewBag.qtdEntries * (ViewBag.currentPage - 1)) + 1, ((ViewBag.currentPage * ViewBag.qtdEntries) > ViewBag.amount ? ViewBag.amount : ViewBag.currentPage * ViewBag.qtdEntries), ViewBag.amount)

            }));

            return PartialView("_PartialList", query.ToList());
        }
    }
}