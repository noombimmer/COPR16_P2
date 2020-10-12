using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CM_APPLICATIONS.Models
{
    public class ReturnTypeModel
    {
        private COPR16Entities db;
        public List<SelectListItem> MSTYPE_LIST { get; set; }
        public COPR16_RETURNTYPE_MSTR cOPR16_RETURNTYPE_MSTR { get; set; }
        public List<COPR16_RTDT_MSTR> cOPR16_RTDT_MSTR_LIST { get; set; }
        public COPR16_RTDT_MSTR cOPR16_RTDT_MSTR { get; set; }

        public ReturnTypeModel(COPR16Entities dbModel)
        {
            db = dbModel;
            
            this.MSTYPE_LIST = new List<SelectListItem>();
            this.cOPR16_RTDT_MSTR = new COPR16_RTDT_MSTR();
            foreach (var row in db.COPR16_MEASURETYPE_MSTR.Where(l => l.FLGACT == true))
            {
                MSTYPE_LIST.Add(new SelectListItem { Text = row.MSTYPE_NAME, Value = row.MSTYPE_ID });
            }
        }
    }
    public class ReturnDetial
    {
        public string RTDT_ID { get; set; }
        public string RTDT_NAME { get; set; }
        public string MSTYPE_ID { get; set; }
        public string REF_ID { get; set; }

    }
}