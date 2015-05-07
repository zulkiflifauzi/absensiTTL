using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace AbsensiTTL.Helpers
{
    public static class HtmlUtility
    {
        public static MvcHtmlString Year(this HtmlHelper htmlHelper, string name, string id)
        {
            var result = new StringBuilder();

            result.Append("<select name=\"" + name + "\" id=\"" + id + "\" class=\"form-control\">");

            for (var i = DateTime.Now.Year; i >= 2014; i--)
            {
                if (1 == DateTime.Now.Year)
                    result.Append("<option selected=\"selected\" value=\"" + i + "\">" + i + "</option>");
                else
                    result.Append("<option value=\"" + i + "\">" + i + "</option>");
            }
            result.Append("</select>");

            return MvcHtmlString.Create(result.ToString());
        }

        public static MvcHtmlString Month(this HtmlHelper htmlHelper, string name, string id)
        {
            var result = new StringBuilder();

            result.Append("<select name=\"" + name + "\" id=\"" + id + "\" class=\"form-control\">");

            List<string> monthNames = DateTimeFormatInfo.CurrentInfo.MonthNames.Take(12).ToList();

            foreach (var item in monthNames)
            {
                if (monthNames.IndexOf(item) == DateTime.Now.Month - 1) result.Append("<option selected=\"selected\" value=\"" + (Convert.ToInt32(monthNames.IndexOf(item)) + 1) + "\">" + item + "</option>");
                else result.Append("<option value=\"" + (Convert.ToInt32(monthNames.IndexOf(item)) + 1) + "\">" + item + "</option>");
            }

            result.Append("</select>");

            return MvcHtmlString.Create(result.ToString());
        }

        public static MvcHtmlString HasPermissions(this HtmlHelper htmlHelper, string permission, string behaviour, string additionalStyle = "")
        {
            String result = string.Empty;

            if (!HttpContext.Current.User.IsInRole(permission) && !HttpContext.Current.User.IsInRole("Administrator"))
            {
                if (behaviour == Constant.ControlBehaviour.DISABLED)
                {
                    if (additionalStyle != string.Empty)
                        result = "disabled='disabled' style='" + additionalStyle + "'";
                    else
                        result = "disabled='disabled'";
                }
                else if (behaviour == Constant.ControlBehaviour.HIDDEN)
                {
                    if (additionalStyle != string.Empty)
                        result = "style='display:none;" + additionalStyle + "'";
                    else
                        result = "style='display:none;'";
                }
            }
            else
            {
                if (additionalStyle != string.Empty)
                    result += "style='" + additionalStyle + "'";
            }

            return MvcHtmlString.Create(result);
        }
    }
}