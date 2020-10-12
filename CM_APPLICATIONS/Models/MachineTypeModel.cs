using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CM_APPLICATIONS.Models
{
    public class MachineTypeModel
    {
        private COPR16Entities db;
        public List<SelectListItem> RTYPE_LIST { get; set; }
        public List<SelectListItem> FGT_LIST { get; set; }
        public COPR16_MANCTYPE_MSTR cOPR16_MANCTYPE_MSTR { get; set; }

        public MachineTypeModel(COPR16Entities dbModel)
        {
            db = dbModel;
            this.RTYPE_LIST = new List<SelectListItem>();
            this.FGT_LIST = new List<SelectListItem>();


            foreach (var row in db.COPR16_RETURNTYPE_MSTR.Where(l => l.FLGACT == true))
            {
                RTYPE_LIST.Add(new SelectListItem { Text = row.RTYPE_NAME, Value = row.RTYPE_ID });
            }
            foreach (var row in db.COPR16_FGTYPE_MSTR.Where(l => l.FLGACT == true))
            {
                FGT_LIST.Add(new SelectListItem { Text = row.FGT_NAME, Value = row.FGT_ID });
            }
        }
    }
}