using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CM_APPLICATIONS.Models
{
    public class ProcessModel
    {
        private COPR16Entities db;
        public List<SelectListItem> PROC_LIST { get; set; }
        public List<SelectListItem> REG_LIST { get; set; }
        public COPR16_PROC_MSTR cOPR16_PROC_MSTR { get; set; }
        public COPR16_STD_MSTR cOPR16_STD_MSTR { get; set; }
        public ProcessModel(COPR16Entities dbModel)
        {
            db = dbModel;
            this.PROC_LIST = new List<SelectListItem>();
            this.REG_LIST = new List<SelectListItem>();


            foreach (var row in db.COPR16_PROC_MSTR.Where(l => l.FLGACT == true))
            {
                PROC_LIST.Add(new SelectListItem { Text = row.PROC_NAME, Value = row.PROC_ID });
            }
            foreach (var row in db.COPR16_STDREG_MSTR.Where(l => l.FLGACT == true))
            {
                REG_LIST.Add(new SelectListItem { Text = row.STDREG_NAME, Value = row.STDREG_ID });
            }
        }
    }
}