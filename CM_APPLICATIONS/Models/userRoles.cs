using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CM_APPLICATIONS.Models
{
    public class userRoles
    {
        public string UserName { get; set; }
        public string UserRoles{ get; set; }
        public DateTime ExpireDateTime { get; set; }
        public bool NeverExpire { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateBy { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateBy { get; set; }
    }
}