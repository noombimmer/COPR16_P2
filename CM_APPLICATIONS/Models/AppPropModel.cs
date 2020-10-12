

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.WebPages;


namespace CM_APPLICATIONS.Models
{
    public static class AppPropModel
    {
        public static string username { get { return GetUserNameEx(); } set { } }
        public static string usernameStr { get; set; }
        public static System.DateTime today { get { return System.DateTime.Now; } set { } }
        public static string sDate { get { return System.DateTime.Now.ToString("yyyy-MMM-dd",DateTimeFormatInfo.InvariantInfo); } set { } }
        public static string GetUserName()
        {
            System.Security.Principal.IPrincipal user = System.Web.HttpContext.Current.User;
            System.Security.Principal.IIdentity identity = user.Identity;
            usernameStr = identity.Name.Substring(identity.Name.IndexOf(@"\") + 1);
            return identity.Name.Substring(identity.Name.IndexOf(@"\") + 1);
        }
        public static string GetUserNameEx()
        {
            
            string usr = System.Web.HttpContext.Current.User.Identity.Name;
            usernameStr = usr.Substring(usr.IndexOf(@"\") + 1);
            return usr.Substring(usr.IndexOf(@"\") + 1);
        }
        public static string rootPath { get { return "~/App_Data/uploads"; } set { } }


    }

}