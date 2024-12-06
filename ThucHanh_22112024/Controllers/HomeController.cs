using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ThucHanh_22112024.Models;
using PagedList;
using PagedList.Mvc;
using System.Web.UI;

namespace ThucHanh_22112024.Controllers
{
    public class HomeController : Controller
    {
        QLBanXeGanMayEntities qLBanXeGanMayEntities = new QLBanXeGanMayEntities();
        public ActionResult Index(int? page)
        {
            int pageSize = 2;
            int pageNum = (page ?? 1);
            var xeMoi = LayXeMoi(5);
            return View(xeMoi.ToPagedList(pageNum, pageSize));
        }
        private List<XEGANMAY> LayXeMoi(int count)
        {
            return qLBanXeGanMayEntities.XEGANMAYs.OrderByDescending(x => x.Ngaycapnhat).Take(count).ToList();
        }
    }
}