using AbsensiTTL.Helpers;
using AbsensiTTL.Models;
using AbsensiTTL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace AbsensiTTL.Controllers
{
    public class LoginController : Controller
    {
        public ActionResult Login()
        {
            if (User.Identity.IsAuthenticated) RedirectToAction("Index", "Home");
            return View();
        }

        public ActionResult ChangePassword()
        {
            if (User.Identity.IsAuthenticated) RedirectToAction("Index", "Home");
            return View();
        }

        [HttpPost]
        public ActionResult ChangePassword(ViewModelChangePassword changePassword)
        {
            using (var db = new AbsensiEntities())
            {
                var userInfo = db.USERINFOes.SingleOrDefault(c => c.BADGENUMBER.Equals(User.Identity.Name));

                if (userInfo != null)
                {
                    var users = db.USERS.SingleOrDefault(c => c.USERID == userInfo.USERID);
                    users.PASSWORD = PasswordHelper.CreatePasswordHash(changePassword.Password, users.SALT);
                    db.Entry(users).CurrentValues.SetValues(users);
                    db.SaveChanges();
                }
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult Login(ViewModelLogin login)
        {
            using (var db = new AbsensiEntities())
            {
                var userInfo = db.USERINFOes.SingleOrDefault(c => c.BADGENUMBER.Equals(login.NIK));

                if (userInfo != null)
                {
                    var user = db.USERS.SingleOrDefault(c => c.USERID == userInfo.USERID);

                    if (PasswordHelper.CreatePasswordHash(login.Password, user.SALT) == user.PASSWORD)
                    {
                        int timeout = Constant.CookieExpiration.Normal;
                        var ticket = new FormsAuthenticationTicket(1, userInfo.BADGENUMBER, DateTime.Now, DateTime.Now.AddMinutes(timeout), true, string.Empty);
                        var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(ticket));

                        if (ticket.IsPersistent)
                        {
                            cookie.Expires = ticket.Expiration;
                        }

                        Response.Cookies.Add(cookie);

                        this.HttpContext.Cache.Add(userInfo.BADGENUMBER + "_Name", userInfo.NAME, null, System.Web.Caching.Cache.NoAbsoluteExpiration, new TimeSpan(0, 525600, 0), System.Web.Caching.CacheItemPriority.Default, null);  
                                               

                        return RedirectToAction("Index", "Home");
                        
                    }                    
                }
            }

            return RedirectToAction("Login", "Login");
        }

        public ActionResult Logoff()
        {
            this.HttpContext.Cache.Remove(User.Identity.Name + "_Name");
            FormsAuthentication.SignOut();
            Session.Clear();
            return RedirectToAction("Login", "Login");
        }
	}
}