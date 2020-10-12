using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CM_APPLICATIONS.Models
{
    public class MSTypeModel
    {
        private COPR16Entities db;
        public List<SelectListItem> UNIT_LIST { get; set; }
        public List<SelectListItem> FILES_LIST { get; set; }
        public List<SelectListItem> OPT_LIST { get; set; }

        public COPR16_MEASURETYPE_MSTR cOPR16_MEASURETYPE_MSTR { get; set; }

        public MSTypeModel(COPR16Entities dbModel)
        {
            db = dbModel;

            this.UNIT_LIST = new List<SelectListItem>();
            this.FILES_LIST = new List<SelectListItem>();
            this.OPT_LIST = new List<SelectListItem>();

            foreach (var row in db.COPR16_UNITS_MSTR.Where(l => l.FLGACT.Value == true))
            {
                UNIT_LIST.Add(new SelectListItem { Text = row.UNIT_NAME, Value = row.UNIT_ID });
            }
            foreach (var row in db.COPR16_FILES_MSTR.Where(l => l.FLGACT.Value == true))
            {
                FILES_LIST.Add(new SelectListItem { Text = row.FILE_NAME, Value = row.FILE_ID });
            }
            foreach (var row in db.COPR16_OPTIONVAL_MSTR.Where(l => l.FLGACT.Value == true))
            {
                OPT_LIST.Add(new SelectListItem { Text = row.OPTION_NAME, Value = row.OPTION_ID });
            }
        }

    }
}