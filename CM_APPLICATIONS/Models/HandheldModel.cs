using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CM_APPLICATIONS.Models
{
    public class HandheldModel
    {
        private COPR16Entities db;

        //public List<SelectListItem> WRK_LIST { get; set; }

        //public List<SelectListItem> MODEL_LIST { get; set; }

        //public List<SelectListItem> POSITION_LIST { get; set; }

        //public List<SelectListItem> PROC_LIST { get; set; }
        public COPR16_COPRUNNING cOPR16_COPRUNNING { get; set; }
        public List<COPR16_COPRUNNING> cOPR16_COPRUNNING_List { get; set; }
        public List<COPR16_COPRUNNING_DT> cOPR16_COPRUNNING_DT_List { get; set; }
        public COPR16_COPRUNNING_DT cOPR16_COPRUNNING_DT { get; set; }
        public COPR16_COPRUNNING_RT cOPR16_COPRUNNING_RT { get; set; }
        public List<COPR16_COPRUNNING_RT> cOPR16_COPRUNNING_RT_List { get; set; }
        public List<COPR16_WORKFLOW_DT> cOPR16_WORKFLOW_DT_List { get; set; }
        public COPR16_WORKFLOW_DT cOPR16_WORKFLOW_DT { get; set; }
        public COPR16_COPRUNNING_DT cOPR16_COPRUNNING_DT_SB { get; set; }
        public COPR16_COPRUNNING_DT cOPR16_COPRUNNING_DT_BKL { get; set; }
        public COPR16_FG_MSTR cOPR16_FG_MSTR { get; set; }
        public COPR16_ITEMS_MSTR cOPR16_ITEMS_MSTR { get; set; }
        public string FG_DETAILS { get; set; }
        public List<WorkProcess> WorkProcess_List { get; set; }
        public WorkProcess workProcess { get; set; }
        public searchCOP searchOpion { get; set; }
        public CopRunningModel localCopsModel { get; set; }
        public COPR16_MACHINE_MSTR cOPR16_MACHINE_MSTR { get; set; }
        public List<COPR16_MACHINE_MSTR> cOPR16_MACHINE_MSTR_List { get; set; }
        public COPR16_MANCTYPE_MSTR cOPR16_MANCTYPE_MSTR { get; set; }

        public string statusCode { get; set; }
        public string machiune_id { get; set; }
        public List<SelectListItem> statusList { get; set; }
        public HandheldModel(COPR16Entities dbModel)
        {
            db = dbModel;
            localCopsModel = new CopRunningModel(db);
            //this.WRK_LIST = new List<SelectListItem>();
            //this.MODEL_LIST = new List<SelectListItem>();
            //this.POSITION_LIST = new List<SelectListItem>();
            //this.PROC_LIST = new List<SelectListItem>();

            //foreach (var row in db.COPR16_WORKFLOW.Where(l => l.FLGACT == true))
            //{
            //    WRK_LIST.Add(new SelectListItem { Text = row.WRK_NAME, Value = row.WRK_ID });
            //}
            //foreach (var row in db.COPR16_MODEL_MSTR.Where(l => l.FLGACT == true))
            //{
            //    MODEL_LIST.Add(new SelectListItem { Text = row.MODEL_DESC, Value = row.MODEL_ID });
            //}
            //foreach (var row in db.COPR16_POSITION_MSTR.Where(l => l.FLGACT == true))
            //{
            //    POSITION_LIST.Add(new SelectListItem { Text = row.POS_DESC, Value = row.POS_ID });
            //}
            //foreach (var row in db.COPR16_PROC_MSTR.Where(l => l.FLGACT == true))
            //{
            //    PROC_LIST.Add(new SelectListItem { Text = row.PROC_NAME, Value = row.PROC_ID });
            //}
            statusList = new List<SelectListItem>();
            if (this.localCopsModel == null)
            {
                this.localCopsModel = new CopRunningModel(db);
            }
            foreach( var item in this.localCopsModel.STATUS_LIST)
            {
                statusList.Add(new SelectListItem { Text = item.text, Value = item.value });
            }
        }

    }
    public class WorkProcess
    {
        public string WRKD_SEQ { get; set; }
        public string WRKD_DESC { get; set; }
        public string FGTYPE_ID { get; set; }
        public string WRKD_ID { get; set; }
        public string MACHINETYPE_ID { get; set; }
        public string MTYPE_NAME { get; set; }
        public string COPR_ID { get; set; }
        public string STATUS_ID { get; set; }
        public Nullable<System.DateTime> TEST_START { get; set; }
        public string TEST_BY { get; set; }
        public Nullable<System.DateTime> TEST_FINISH { get; set; }
        public string CLOSE_BY { get; set; }
        public string MACHINE_ID { get; set; }
    }
    public class StartSaveTest
    {
        public string COPR_ID { get; set; }
        public string WRKD_ID { get; set; }
        public string WRKD_SEQ { get; set; }
        public string WRK_ID { get; set; }
        public string MACHINETYPE_ID { get; set; }
        public string MACHINE_ID { get; set; }
        public string TEST_BY { get; set; }
        public string TEST_START { get; set; }
    }
    public class ResultSaveTest
    {
        public string COPR_ID { get; set; }
        public string SEQ_NO { get; set; }
        public string CLOSE_BY { get; set; }
        public string ISOPT { get; set; }
        public List<VALUELIST> valueList { get; set; }

    }
    public class VALUELIST
    {
        public string COPR_ID { get; set; }
        public string SEQ_NO { get; set; }
        public string VALUE { get; set; }
        public string RTDT_ID { get; set; }
    }
}