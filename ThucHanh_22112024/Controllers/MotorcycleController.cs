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
    public class MotorcycleController : Controller
    {
        QLBanXeGanMayEntities qLBanXeGanMay = new QLBanXeGanMayEntities();

        private List<XEGANMAY> LayXeMoi(int count)
        {
            return qLBanXeGanMay.XEGANMAYs.OrderByDescending(x => x.Ngaycapnhat).Take(count).ToList();
        }
        // GET: Motorcycle
        public ActionResult Index(int ? page)
        {
            int pageSize = 2;
            int pageNum = (page ?? 1);
            var xeMoi = LayXeMoi(5);
            return View(xeMoi.ToPagedList(pageNum, pageSize));
        }


        public ActionResult LoaiXe()
        {
            var loaiXe = from lx in qLBanXeGanMay.LOAIXEs select lx;
            return PartialView(loaiXe);
        }
        public ActionResult SPTheoLoaiXe(int id)
        {
            var xeganmay = from xgm in qLBanXeGanMay.XEGANMAYs where xgm.MaLX == id select xgm;
            return View(xeganmay);
        }

        public ActionResult NhaPhanPhoi()
        {
            var nhaPhanPhoi = from npp in qLBanXeGanMay.NHAPHANPHOIs select npp;
            return PartialView(nhaPhanPhoi);
        }

        public ActionResult SPTheoNhaPhanPhoi(int id)
        {
            var xeganmay = from xgm in qLBanXeGanMay.XEGANMAYs where xgm.MaNPP == id select xgm;
            return PartialView(xeganmay);
        }

        public ActionResult Details(int id)
        {
            var xeganmay = from xgm in qLBanXeGanMay.XEGANMAYs where xgm.MaXe == id select xgm;
            return View(xeganmay.Single()); 
        }


    }
}