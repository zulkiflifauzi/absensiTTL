using AbsensiTTL.Models;
using AbsensiTTL.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AbsensiTTL.Controllers
{
    public class PublicController : Controller
    {
        public ActionResult LatestAbsence()
        {
            return View();        
        }

        [HttpPost]
        public ActionResult GetTopTenLatest()
        {
            List<ViewModelCheckInOut> result = new List<ViewModelCheckInOut>();
            DateTime tomorrow = DateTime.Now.AddDays(1);
            using (var db = new AbsensiEntities())
            {
                var results = db.ViewCheckInOuts.Where(c => c.CHECKTIME < tomorrow).OrderByDescending(c => c.CHECKTIME).Take(10);
                foreach (var item in results)
                {
                    ViewModelCheckInOut i = new ViewModelCheckInOut();
                    i.NAME = item.NAME;
                    i.DAY = item.CHECKTIME.ToString("dddd, dd MMM yyy");
                    i.DESCRIPTION = item.CHECKTIME.ToString("HH:mm");
                    result.Add(i);
                }
            }

            return Json(result);
        }

        public ActionResult Download(string id)
        {
            return File("/Files/Presence_July_2014_All.pdf", "pdf", "Presensi_Juli_2014_All.pdf");
        }
        public ActionResult DownloadPresence()
        {
            return View();
        }
        public ActionResult Today()
        {
            List<ViewModelCheckInOut> result = new List<ViewModelCheckInOut>();
            using (var db = new AbsensiEntities())
            {
                var users = db.USERINFOes.ToList();

                foreach (var item in users)
                {
                    ViewModelCheckInOut cio = new ViewModelCheckInOut();
                    cio.BADGENUMBER = item.BADGENUMBER;
                    cio.NAME = item.NAME.ToUpper();

                    var today = db.ViewCheckInOuts.Where(c => c.BADGENUMBER.Equals(item.BADGENUMBER) && c.CHECKTIME.Year == DateTime.Now.Year && c.CHECKTIME.Month == DateTime.Now.Month && c.CHECKTIME.Day == DateTime.Now.Day).ToList();

                    if (today.Count > 0)
                    {
                        var min = today.Min(c => c.CHECKTIME);
                        if (min != null)
                        {
                            cio.CHECKIN = min.ToString("H:mm:ss");
                            cio.CHECKINDD = min;
                        }

                        var max = today.Max(c => c.CHECKTIME);
                        if (max != null)
                        {
                            cio.CHECKOUT = max.ToString("H:mm:ss");
                            cio.CHECKOUTD = max;
                        }
                    }
                    result.Add(cio);
                }
            }

            var earlyStaff = result.Where(c => c.CHECKINDD != DateTime.MinValue).OrderBy(c => c.CHECKINDD).FirstOrDefault();

            if (earlyStaff != null)
            {
                ViewData["staff"] = earlyStaff.NAME.ToUpper();
            }
            else
            {
                ViewData["staff"] = "No One";
            }

            using (var db = new AbsensiEntities())
            {
                var today = db.CHECKINOUTs.Where(c => c.CHECKTIME < DateTime.Now).OrderByDescending(c => c.CHECKTIME).FirstOrDefault();

                ViewData["LastUpdate"] = today.CHECKTIME.ToString("MMM dd, yyy HH:mm:ss");
            }

            return View(result);
        }
	}
}