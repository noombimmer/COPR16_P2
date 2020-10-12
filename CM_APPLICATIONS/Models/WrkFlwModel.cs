using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CM_APPLICATIONS.Models
{
    public class WrkFlwModel
    {
        private COPR16Entities db;
        public List<SelectListItem> STDREG_LIST { get; set; }
        public List<SelectListItem> STEPS_LIST { get; set; }
        public List<SelectListItem> WRKD_LIST { get; set; }
        public List<SelectListItem> STD_LIST { get; set; }
        public List<SelectListItem> PROC_LIST { get; set; }

        public COPR16_WORKFLOW cOPR16_WORKFLOW { get; set; }
        public List<COPR16_WORKFLOW_DT> cOPR16_WORKFLOW_DT_List { get; set; }
        public COPR16_WORKFLOW_DT cOPR16_WORKFLOW_DT { get; set; }

        public string WRK_DESC { get; set; }
        public WrkFlwModel(COPR16Entities dbModel)
        {
            db = dbModel;

            this.STDREG_LIST = new List<SelectListItem>();
            this.STD_LIST = new List<SelectListItem>();
            this.PROC_LIST = new List<SelectListItem>();
            this.STEPS_LIST = new List<SelectListItem>();
            this.WRKD_LIST = new List<SelectListItem>();

            foreach (var row in db.COPR16_STDREG_MSTR.Where(l => l.FLGACT == true))
            {
                STDREG_LIST.Add(new SelectListItem { Text = row.STDREG_NAME, Value = row.STDREG_ID });
            }
            foreach (var row in db.COPR16_STEP_MSTR.Where(l => l.FLGACT == true))
            {
                STEPS_LIST.Add(new SelectListItem { Text =  row.STEP_NAME, Value = row.STEP_ID });
            }

            foreach (var row in db.COPR16_STD_MSTR.Where(l => l.FLGACT == true))
            {
                STD_LIST.Add(new SelectListItem { Text = row.STD_NAME, Value = row.STD_ID });
            }
            foreach (var row in db.COPR16_PROC_MSTR.Where(l => l.FLGACT == true))
            {
                PROC_LIST.Add(new SelectListItem { Text = row.PROC_NAME, Value = row.PROC_ID });
            }
        }

    }
    public class WRKD_ROW{
        public WRKD_ROW() {
        }

        public string STEP_ID { get; set; }
        public string WRKD_ANDCOND { get; set; }
        public string WRKD_DESC { get; set; }
        public string WRKD_ID { get; set; }

        public string WRKD_PARL { get; set; }
        public string WRKD_SEQ { get; set; }
        public string WRK_ID { get; set; }
        public string WRKD_WITH_ID { get; set; }
        public string WRKD_MAIN { get; set; }
        public string WRKD_ORDER { get; set; }


    }
    public class StepLookup
    {
        public string RowNumber { get; set; }
        public string STEP_ID { get; set; }
        public string STEP_NAME { get; set; }
        public string STEP_DESC { get; set; }
        public string MTYPE_ID { get; set; }
        public string MTYPE_NAME { get; set; }
        public string RTYPE_ID { get; set; }
        public string RTYPE_NAME { get; set; }
        public string REF_ID { get; set; }
    }

    public class ItemsList
    {
        public string FG_NO { get; set; }
        public string ITEM_ID { get; set; }
        public string BRAND_ID { get; set; }
        public string MODLE_ID { get; set; }
        public string FG_NAME { get; set; }
        public string FG_DESC { get; set; }
        public string fgtype_id { get; set; }
        public string LINE_ID { get; set; }

    }
}