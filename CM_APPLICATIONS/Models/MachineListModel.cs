using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CM_APPLICATIONS.Models
{
    public class MachineListModel
    {
        private COPR16Entities db;
        public List<SelectListItem> MANTYPE_LIST { get; set; }
//        public List<SelectListItem> FGT_LIST { get; set; }
        //public COPR16_MANCTYPE_MSTR cOPR16_MANCTYPE_MSTR { get; set; }
        public COPR16_MACHINE_MSTR cOPR16_MACHINE_MSTR { get; set; }
        
        public MachineListModel(COPR16Entities dbModel)
        {
            db = dbModel;
            this.MANTYPE_LIST = new List<SelectListItem>();

            foreach (var row in db.COPR16_MANCTYPE_MSTR.Where(l => l.FLGACT == true))
            {
                MANTYPE_LIST.Add(new SelectListItem { Text = row.MTYPE_ID + " " + row.MTYPE_NAME  , Value = row.MTYPE_ID });
            }
        }
    }
}