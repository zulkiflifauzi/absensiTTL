using AbsensiTTL.Models;
using AbsensiTTL.Repositories;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AbsensiTTL.Controllers
{

    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            using (var db = new AbsensiEntities())
            {
                var today = db.CHECKINOUTs.Where(c => c.CHECKTIME < DateTime.Now).OrderByDescending(c => c.CHECKTIME).FirstOrDefault();

                var users = db.USERINFOes.ToList();

                List<SelectListItem> userIds = new List<SelectListItem>();

                foreach (var user in users)
                {
                    SelectListItem userid = new SelectListItem { Text = user.NAME, Value = user.BADGENUMBER };
                    userIds.Add(userid);
                }

                ViewData["userId"] = userIds;
                ViewData["LastUpdate"] = today.CHECKTIME.ToString("MMM dd, yyy HH:mm:ss");
            }
            return View();
        }

        

        [HttpPost]
        public ActionResult GetPresenceList(int year, int month, string badgeNumber = "")
        {

            string user = User.Identity.Name;

            if (badgeNumber != "undefined")
                user = badgeNumber;

            var daysInMonth = DateTime.DaysInMonth(year, month);

            List<ViewModelCheckInOut> result = new List<ViewModelCheckInOut>();

            for (var i = 1; i <= daysInMonth; i++)
            {
                ViewModelCheckInOut a = new ViewModelCheckInOut();
                a.DATETIME = new DateTime(year, month, i);

                if (a.DATETIME > DateTime.Today) continue;
                result.Add(a);
            }

            foreach (var item in result)
            {
                item.CHECKOUT = "";
                item.CHECKIN = "";
                item.DESCRIPTION = "";


                item.DATE = item.DATETIME.ToString("dd MMM yy");
                item.DAY = item.DATETIME.ToString("dddd");
                if (item.DATETIME.ToString("ddd").Equals("Sat")) item.DESCRIPTION = "Saturday";
                if (item.DATETIME.ToString("ddd").Equals("Sun")) item.DESCRIPTION = "Sunday";

                using (var db = new AbsensiEntities()) 
                {
                    var today = db.ViewCheckInOuts.Where(c => c.BADGENUMBER.Equals(user) && c.CHECKTIME.Year == item.DATETIME.Year && c.CHECKTIME.Month == item.DATETIME.Month && c.CHECKTIME.Day == item.DATETIME.Day).ToList();
                    
                    if (today.Count > 0 && (item.DATETIME.DayOfWeek!=DayOfWeek.Saturday&&item.DATETIME.DayOfWeek!=DayOfWeek.Sunday))
                    {
                        var min = today.Min(c => c.CHECKTIME);
                        if (min != null)
                        {
                            item.CHECKIN = min.ToString("H:mm:ss");
                        }

                        var max = today.Max(c => c.CHECKTIME);
                        if (max != null)
                        {
                            item.CHECKOUT = max.ToString("H:mm:ss");
                        }
                        item.DESCRIPTION = "Sesuai Waktu Kerja";
                        var cekDate = max - min;
                        if (cekDate.Hours < 1 && cekDate.Minutes <= 1)
                        {
                            min = max;
                        }
                        if (min != max)
                        {
                            if (min.Hour >= 8 && min.Minute >= 10)
                            {
                                item.DESCRIPTION = "Tidak Sesuai Waktu Kerja";
                            }
                            if (max.Hour < 17)
                            {
                                item.DESCRIPTION = "Tidak Sesuai Waktu Kerja";
                            }
                        }
                        else {
                            if (max.Hour < 12)
                            {
                                item.DESCRIPTION = "Absen Sekali";
                            }
                            if (min.Hour >= 12)
                            {
                                item.DESCRIPTION = "Absen Sekali";
                            }
                        }
                    }
                    else if (today.Count > 0 && (item.DATETIME.DayOfWeek == DayOfWeek.Saturday || item.DATETIME.DayOfWeek == DayOfWeek.Sunday))
                    {
                        var min = today.Min(c => c.CHECKTIME);
                        if (min != null)
                        {
                            item.CHECKIN = min.ToString("H:mm:ss");
                        }

                        var max = today.Max(c => c.CHECKTIME);
                        if (max != null)
                        {
                            item.CHECKOUT = max.ToString("H:mm:ss");
                        }
                        item.DESCRIPTION = "Masuk Weekend";
                    }
                }
            }                

            return Json(result);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult EmailList()
        {
            return View();
        }
    }
}