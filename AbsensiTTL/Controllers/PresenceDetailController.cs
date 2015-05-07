using AbsensiTTL.Models;
using AbsensiTTL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;

namespace AbsensiTTL.Controllers
{
    [Authorize]
    public class PresenceDetailController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /PresenceDetail/
        [HttpPost]
        public ActionResult GetRekap(int id, int year)
        {
            List<ViewModelPresenceDetail> results = GetRekapPrivate(id, year);


            return Json(results);

            
        }

        private List<ViewModelPresenceDetail> GetRekapPrivate(int id, int year)
        {
            int firstDay = 1;
            int daysInMonth = DateTime.DaysInMonth(year, id);
            int hariKerja = daysInMonth;

            List<ViewModelPresenceDetail> results = new List<ViewModelPresenceDetail>();
            List<ViewModelLiburNasional> holidays = new List<ViewModelLiburNasional>();
            List<ViewModelJabatan> Jabatans = new List<ViewModelJabatan>();


            using (var db = new SIUKEntities())
            {
                List<V_LIST_HARI_LIBUR> liburnasional = new List<V_LIST_HARI_LIBUR>();

                liburnasional = db.V_LIST_HARI_LIBUR.Where(c => c.TGL_LIBUR.Month == id && c.TGL_LIBUR.Year == year).ToList();

                foreach (var item in liburnasional)
                {
                    ViewModelLiburNasional libur = new ViewModelLiburNasional();
                    libur.Date = item.TGL_LIBUR;
                    holidays.Add(libur);
                }

                List<V_LIST_SDM> listSdm = new List<V_LIST_SDM>();

                listSdm = db.V_LIST_SDM.ToList();

                foreach (var item in listSdm)
                {
                    ViewModelJabatan jabatan = new ViewModelJabatan();
                    jabatan.NIPP = item.NRK;
                    jabatan.Jabatan = item.NAJAB;
                    Jabatans.Add(jabatan);
                }
            }

            for (int i = firstDay; i <= daysInMonth; i++)
            {
                DateTime date = new DateTime(year, id, i);

                if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
                {
                    hariKerja -= 1;
                }

                var tanggalLibur = holidays.Where(c => c.Date.Month == date.Month && c.Date.Year == date.Year && c.Date.Day == date.Day).SingleOrDefault();
                if (tanggalLibur != null)
                {
                    if (tanggalLibur.Date.DayOfWeek == DayOfWeek.Saturday || tanggalLibur.Date.DayOfWeek == DayOfWeek.Sunday)
                    {
                        hariKerja += 0;
                    }
                    else
                        hariKerja -= 1;

                }
            }

            using (var db = new AbsensiEntities())
            {
                List<USERINFO> users = new List<USERINFO>();
                if (User.IsInRole("Administrator"))
                {
                    users = db.USERINFOes.ToList();
                }
                else
                {
                    users = db.USERINFOes.Where(c => c.BADGENUMBER == User.Identity.Name).ToList();
                }

                foreach (var user in users)
                {
                    ViewModelPresenceDetail result = new ViewModelPresenceDetail();
                    result.NIK = user.BADGENUMBER;
                    result.Name = user.NAME;
                    result.UserId = user.USERID;

                    results.Add(result);
                }

            }

            foreach (var item in results)
            {
                var jabatanx = Jabatans.Where(c => c.NIPP.Contains(item.NIK)).FirstOrDefault();
                if (jabatanx != null)
                {
                    item.Jabatan = jabatanx.Jabatan;
                }

                int tidakAbsenDatang = 0;
                int tidakAbsenPulang = 0;
                int telat = 0;
                int pulangcepat = 0;
                int masuk = 0;
                int tidakmasuk = 0;
                int masukharilibur = 0;
                for (int i = firstDay; i <= daysInMonth; i++)
                {
                    DateTime date = new DateTime(year, id, i);



                    if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
                    {
                        using (var db = new AbsensiEntities())
                        {
                            var checkinouts = db.CHECKINOUTs.Where(c => c.CHECKTIME.Year == year && c.CHECKTIME.Month == id && c.CHECKTIME.Day == i && c.USERID == item.UserId);
                            if (checkinouts.Count() > 0)
                            {
                                masukharilibur += 1;
                            }
                        }
                    }
                    else if (holidays.Any(c => c.Date.Year == date.Year && c.Date.Month == date.Month && c.Date.Day == date.Day))
                    {
                        if (date.DayOfWeek != DayOfWeek.Saturday || date.DayOfWeek != DayOfWeek.Sunday)
                        {
                            using (var db = new AbsensiEntities())
                            {
                                var checkinouts = db.CHECKINOUTs.Where(c => c.CHECKTIME.Year == year && c.CHECKTIME.Month == id && c.CHECKTIME.Day == i && c.USERID == item.UserId);
                                if (checkinouts.Count() > 0)
                                {
                                    masukharilibur += 1;
                                }
                            }
                        }
                    }
                    else if (date.DayOfWeek == DayOfWeek.Monday || date.DayOfWeek == DayOfWeek.Tuesday || date.DayOfWeek == DayOfWeek.Wednesday || date.DayOfWeek == DayOfWeek.Thursday || date.DayOfWeek == DayOfWeek.Friday)
                    {
                        using (var db = new AbsensiEntities())
                        {
                            var checkinouts = db.CHECKINOUTs.Where(c => c.CHECKTIME.Year == year && c.CHECKTIME.Month == id && c.CHECKTIME.Day == i && c.USERID == item.UserId).ToList();
                            if (checkinouts.Count() > 0)
                            {
                                var minDate = checkinouts.Min(c => c.CHECKTIME);
                                var maxDate = checkinouts.Max(c => c.CHECKTIME);
                                masuk += 1;
                                var cekDate = maxDate - minDate;
                                if (cekDate.Hours < 1 && cekDate.Minutes <= 1)
                                {
                                    minDate = maxDate;
                                }
                                if (minDate != maxDate)
                                {
                                    if (minDate.Hour >= 8 && minDate.Minute >= 10)
                                    {
                                        telat += 1;
                                    }
                                    if (maxDate.Hour < 17)
                                    {
                                        pulangcepat += 1;
                                    }
                                }
                                else
                                {

                                    if (maxDate.Hour < 12)
                                    {
                                        tidakAbsenPulang += 1;
                                    }
                                    if (minDate.Hour >= 12)
                                    {
                                        tidakAbsenDatang += 1;
                                    }
                                }

                            }
                            else
                            {
                                tidakmasuk += 1;
                            }
                        }
                    }
                }
                item.TidakAbsenDatang = tidakAbsenDatang;
                item.TidakAbsenPulang = tidakAbsenPulang;
                item.Telat = telat;
                item.PulangCepat = pulangcepat;
                item.Masuk = masuk;
                item.TidakMasuk = tidakmasuk;
                item.MasukHariLibur = masukharilibur;
                item.HariKerja = hariKerja;
                int transport = 0;
                /*41=bibin,36=doddy,17=herman,22=rizki,28=ranu,26=arief,16=ary,19=faiza,48=ika triyana,
                18=hadi saputro,33=samiadji,95=andika pratama,31=betacahya,25=feldy,24=pierre,14=yoyok,
                143=annisaa,141=nurul khafid  */
                if (item.UserId == 41 || item.UserId == 36 || item.UserId == 17 || item.UserId == 22 || item.UserId == 28
                    || item.UserId == 26 || item.UserId == 16 || item.UserId == 19 || item.UserId == 48 || item.UserId == 18
                    || item.UserId == 33 || item.UserId == 95 || item.UserId == 31 || item.UserId == 25 || item.UserId == 24
                    || item.UserId == 14 || item.UserId == 143 || item.UserId == 141)
                {
                    transport = 100000;
                }
                /*58=yarmani,103=witha,67=ruly,51=devi,52=danie,9=wara,85=dicky,34=priyo,44=arief firman
                 * 68=sapta,38=ariful,107=arsina,89=rani,139=ivone,138=putri,142=ghufron,171=filia,
                 * 11=edward,6=denny,7=kartiko,20=anang,5=rudi,10=juju,8=henry,4=firman,153=takhim,
                 * 145=suhaimin,146=mujib,147=hendra,148=rizal,149=welly,150=faizal,151=saiful,152=aan,
                 * 170=sholeh,172=yana
                 */
                else if (item.UserId == 58 || item.UserId == 103 || item.UserId == 67 || item.UserId == 51
                    || item.UserId == 52 || item.UserId == 9 || item.UserId == 85 || item.UserId == 34 || item.UserId == 44
                    || item.UserId == 68 || item.UserId == 38 || item.UserId == 107 || item.UserId == 89 || item.UserId == 139
                    || item.UserId == 138 || item.UserId == 142 || item.UserId == 171 || item.UserId == 11 ||
                    item.UserId == 6 || item.UserId == 7 || item.UserId == 20 || item.UserId == 5 || item.UserId == 10 ||
                    item.UserId == 8 || item.UserId == 4 || item.UserId == 153 || item.UserId == 145 || item.UserId == 146 ||
                    item.UserId == 147 || item.UserId == 148 || item.UserId == 149 || item.UserId == 150
                    || item.UserId == 151 || item.UserId == 152 || item.UserId == 170 || item.UserId == 172)
                {
                    transport = 0;
                }
                else
                {
                    transport = 75000;
                }
                double tempbantuantransport = 0;
                if (item.TidakAbsenDatang == 0 && item.TidakAbsenPulang == 0)
                {
                    tempbantuantransport = transport * item.Masuk;
                    item.BantuanTransport = String.Format("{0:#,0}", tempbantuantransport);
                    item.BantuanTransportCopy = tempbantuantransport;
                }
                else
                {
                    int temptidakabsen = item.TidakAbsenPulang + item.TidakAbsenDatang;
                    int tempmasuk = item.Masuk - temptidakabsen;
                    tempbantuantransport = ((0.5 * transport) * temptidakabsen) + (transport * tempmasuk);
                    item.BantuanTransport = String.Format("{0:#,0}", tempbantuantransport);
                    item.BantuanTransportCopy = tempbantuantransport;
                }

            }
            return results;
        }
        [HttpPost]
        public ActionResult Export(int tahun, int bulan, int id = 0)
        {
            List<ViewModelPresenceDetail> convert = new List<ViewModelPresenceDetail>();
            convert = GetRekapPrivate(bulan, tahun);
            StringBuilder str = new StringBuilder();
            str.Append("<table border=`" + "1px" + "`b>");
            str.Append("<tr>");
            str.Append("<td><b><font face=Arial Narrow size=3>NIK</font></b></td>");
            str.Append("<td><b><font face=Arial Narrow size=3>Name</font></b></td>");
            str.Append("<td><b><font face=Arial Narrow size=3>Telat</font></b></td>");
            str.Append("<td><b><font face=Arial Narrow size=3>Pulang Cepat</font></b></td>");
            str.Append("<td><b><font face=Arial Narrow size=3>Tidak Absen Datang</font></b></td>");
            str.Append("<td><b><font face=Arial Narrow size=3>Tidak Absen Pulang</font></b></td>");
            str.Append("<td><b><font face=Arial Narrow size=3>Masuk</font></b></td>");
            str.Append("<td><b><font face=Arial Narrow size=3>Tidak Masuk</font></b></td>");
            str.Append("<td><b><font face=Arial Narrow size=3>Masuk Hari Libur</font></b></td>");
            str.Append("<td><b><font face=Arial Narrow size=3>Hari Kerja Bulan Ini</font></b></td>");
            str.Append("<td><b><font face=Arial Narrow size=3>Bantuan Transport</font></b></td>");
            str.Append("</tr>");
            foreach (ViewModelPresenceDetail val in convert)
            {
                str.Append("<tr>");
                str.Append("<td><font face=Arial Narrow size=" + "14px" + ">" + val.NIK.ToString() + "</font></td>");
                str.Append("<td><font face=Arial Narrow size=" + "14px" + ">" + val.Name.ToString() + "</font></td>");
                str.Append("<td><font face=Arial Narrow size=" + "14px" + ">" + val.Telat.ToString() + "</font></td>");
                str.Append("<td><font face=Arial Narrow size=" + "14px" + ">" + val.PulangCepat.ToString() + "</font></td>");
                str.Append("<td><font face=Arial Narrow size=" + "14px" + ">" + val.TidakAbsenDatang.ToString() + "</font></td>");
                str.Append("<td><font face=Arial Narrow size=" + "14px" + ">" + val.TidakAbsenPulang.ToString() + "</font></td>");
                str.Append("<td><font face=Arial Narrow size=" + "14px" + ">" + val.Masuk.ToString() + "</font></td>");
                str.Append("<td><font face=Arial Narrow size=" + "14px" + ">" + val.TidakMasuk.ToString() + "</font></td>");
                str.Append("<td><font face=Arial Narrow size=" + "14px" + ">" + val.MasukHariLibur.ToString() + "</font></td>");
                str.Append("<td><font face=Arial Narrow size=" + "14px" + ">" + val.HariKerja.ToString() + "</font></td>");
                str.Append("<td><font face=Arial Narrow size=" + "14px" + ">" + val.BantuanTransportCopy.ToString(new System.Globalization.CultureInfo("en-US").NumberFormat) + "</font></td>");
                str.Append("</tr>");
            }
            str.Append("</table>");
            HttpContext.Response.AddHeader("content-disposition", "attachment; filename=rekapitulasi"+ DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString() + ".xls");
            this.Response.ContentType = "application/vnd.ms-excel";
            byte[] temp = System.Text.Encoding.UTF8.GetBytes(str.ToString());
            return File(temp, "application/vnd.ms-excel");
        }

	}
}