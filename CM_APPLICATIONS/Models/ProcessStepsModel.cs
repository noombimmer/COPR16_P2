using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CM_APPLICATIONS.Models
{
    public class ProcessStepsModel
    {
        private COPR16Entities db;
        public List<SelectListItem> MCTYPE_LIST { get; set; }
        public List<SelectListItem> RTV_LIST { get; set; }

        public COPR16_STEP_MSTR cOPR16_STEP_MSTR { get; set; }

        public ProcessStepsModel(COPR16Entities dbModel)
        {
            db = dbModel;
            this.MCTYPE_LIST = new List<SelectListItem>();
            this.RTV_LIST = new List<SelectListItem>();

            foreach (var row in db.COPR16_MANCTYPE_MSTR.Where(l => l.FLGACT == true))
            {
                MCTYPE_LIST.Add(new SelectListItem { Text = row.MTYPE_ID + ":" + row.MTYPE_NAME , Value = row.MTYPE_ID });
            }
            foreach (var row in db.COPR16_RETURNTYPE_MSTR.Where(l => l.FLGACT == true))
            {
                RTV_LIST.Add(new SelectListItem { Text = row.RTYPE_ID + ":" + row.RTYPE_NAME, Value = row.RTYPE_ID });
            }

        }
    }
}