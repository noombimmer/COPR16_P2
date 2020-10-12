using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CM_APPLICATIONS.Models
{
    public class FileModel
    {
        private COPR16Entities db;
        public List<SelectListItem> FTYPE_LIST { get; set; }

        public COPR16_FILES_MSTR cOPR16_FILES_MSTR { get; set; }
        public FileModel(COPR16Entities dbModel)
        {
            db = dbModel;

            this.FTYPE_LIST = new List<SelectListItem>();

            foreach (var row in db.COPR16_FTYPE_MSTR.Where(l => l.FLGACT == true))
            {
                FTYPE_LIST.Add(new SelectListItem { Text = row.FT_NAME, Value = row.FT_ID });
            }
        }
    }
}