using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CM_APPLICATIONS.Models
{
    public class CopRunningModel
    {
        private COPR16Entities db;

        public List<SelectListItem> WRK_LIST { get; set; }
        public List<cop_status> STATUS_LIST { get; set; }

        public List<SelectListItem> MODEL_LIST { get; set; }
        public List<SelectListItem> LINE_LIST { get; set; }
        public List<SelectListItem> POSITION_LIST { get; set; }

        public List<SelectListItem> PROC_LIST { get; set; }
        public List<SelectListItem> PROC_LIST_Short { get; set; }
        public COPR16_COPRUNNING cOPR16_COPRUNNING { get; set; }
        public List<COPR16_COPRUNNING> cOPR16_COPRUNNING_List { get; set; }
        public List<COPR16_COPRUNNING_DT> cOPR16_COPRUNNING_DT_List { get; set; }
        public List<COPR16_COPRUNNING_RT> cOPR16_COPRUNNING_RT_List { get; set; }
        public COPR16_COPRUNNING_DT cOPR16_COPRUNNING_DT { get; set; }
        public COPR16_COPRUNNING_RT cOPR16_COPRUNNING_RT { get; set; }

        public COPR16_COPRUNNING_DT cOPR16_COPRUNNING_DT_SB { get; set; }
        public COPR16_COPRUNNING_DT cOPR16_COPRUNNING_DT_BKL { get; set; }
        public COPR16_FG_MSTR cOPR16_FG_MSTR { get; set; }
        public COPR16_ITEMS_MSTR cOPR16_ITEMS_MSTR { get; set; }
        public List<SelectListItem> statusList { get; set; }

        public List<SelectListItem> STDREG { get; set; }

        public RST_HEADER rST_HEADER { get; set; }

        public List<RST_DETIAL> sST_DETIAL_List { get; set; }

        public string statusCode { get; set; }
        public string xModel { get; set; }
        public string xLine { get; set; }
        public string xPos { get; set; }
        public string cOPR16_COPRUNNING_WRK_ID { get; set; }

        public string COPNO { get; set; }
        public string SelectedCOPNO { get; set; }
        public string stDate { get; set; }
        public string enDate { get; set; }

        public string selectDate { get; set; }

        public List<COPR16_MACHINE_MSTR> cOPR16_MACHINE_MSTR_List { get; set; }
        public CopRunningModel(COPR16Entities dbModel)
        {
            db = dbModel;

            this.WRK_LIST = new List<SelectListItem>();
            this.MODEL_LIST = new List<SelectListItem>();
            this.LINE_LIST = new List<SelectListItem>();
            this.POSITION_LIST = new List<SelectListItem>();
            this.PROC_LIST = new List<SelectListItem>();
            this.PROC_LIST_Short = new List<SelectListItem>();
            this.STATUS_LIST = new List<cop_status>();
            statusList = new List<SelectListItem>();
            STDREG = new List<SelectListItem>();
            foreach (var row in db.COPR16_STDREG_MSTR.Where(l => l.FLGACT == true))
            {
                STDREG.Add(new SelectListItem { Text = row.STDREG_NAME, Value = row.STDREG_ID });
            }
            foreach (var row in db.COPR16_WORKFLOW.Where(l => l.FLGACT == true))
            {
                WRK_LIST.Add(new SelectListItem { Text = row.WRK_NAME, Value = row.WRK_ID });
            }
            foreach (var row in db.COPR16_MODEL_MSTR.Where(l => l.FLGACT == true))
            {
                MODEL_LIST.Add(new SelectListItem { Text = row.MODEL_DESC, Value = row.MODEL_ID });
            }
            foreach (var row in db.COPR16_LINE_MSTR.Where(l => l.FLGACT == true))
            {
                LINE_LIST.Add(new SelectListItem { Text = row.LINE_DESC, Value = row.LINE_ID });
            }
            foreach (var row in db.COPR16_POSITION_MSTR.Where(l => l.FLGACT == true))
            {
                POSITION_LIST.Add(new SelectListItem { Text = row.POS_DESC, Value = row.POS_ID });
            }
            foreach (var row in db.COPR16_PROC_MSTR.Where(l => l.FLGACT == true))
            {
                PROC_LIST.Add(new SelectListItem { Text = row.PROC_NAME, Value = row.PROC_ID });
            }
            foreach (var row in db.COPR16_PROC_MSTR.Where(l => l.FLGACT == true))
            {
                PROC_LIST_Short.Add(new SelectListItem { Text = row.PROC_NAME, Value = row.PROC_ID });
            }
            statusList.Add(new SelectListItem { Text = "--ALL--", Value = "ALL" });
            this.STATUS_LIST.Add(new cop_status { text = "NEW", value = "NEW", step_status = false, job_status = true, color = "#" });
            this.STATUS_LIST.Add(new cop_status { text = "READY", value = "READY", step_status = false, job_status = true, color = "#5cb85c" });
            this.STATUS_LIST.Add(new cop_status { text = "WAIT", value = "WAIT", step_status = true, job_status = true, color = "#ffe764" });
            this.STATUS_LIST.Add(new cop_status { text = "QUEUE", value = "QUEUE", step_status = true, job_status = false, color = "#ffe764" });
            this.STATUS_LIST.Add(new cop_status { text = "TESTING", value = "TESTING", step_status = true, job_status = true, color = "#15aaf6", fcolor = "#ffffff" });
            this.STATUS_LIST.Add(new cop_status { text = "ABORT", value = "ABORT", step_status = true, job_status = false, color = "#d9534f" });
            this.STATUS_LIST.Add(new cop_status { text = "RE-TEST", value = "RE-TEST", step_status = true, job_status = false, color = "#15aaf6" });
            this.STATUS_LIST.Add(new cop_status { text = "FINISHED", value = "FINISHED", step_status = true, job_status = false, color = "#42dc95" });
            this.STATUS_LIST.Add(new cop_status { text = "COMPLETED", value = "COMPLETED", step_status = false, job_status = true, color = "#42dc95" });
            this.STATUS_LIST.Add(new cop_status { text = "CANCELED", value = "CANCELED", step_status = false, job_status = true, color = "#d9534f" });
            foreach (var item in STATUS_LIST)
            {
                statusList.Add(new SelectListItem { Text = item.text, Value = item.value });
            }
        }

    }

    public class itemParams
    {
        public string fg_no { get; set; }
        public string model_no { get; set; }
        public string line_no { get; set; }
        public string pos_no { get; set; }

    }

    public class cop_status
    {
        public string text { get; set; }
        public string value { get; set; }
        public string color { get; set; }
        public string fcolor { get; set; }
        public int index { get; set; }
        public Boolean step_status { get; set; }
        public Boolean job_status { get; set; }
    }
    public partial class ItemRows
    {
        public string a_FG_NO { get; set; }
        public string a_ITEM_ID { get; set; }
        public string a_BRAND_ID { get; set; }
        public string a_MODLE_ID { get; set; }
        public string b_FG_NAME { get; set; }
        public string b_FG_DESC { get; set; }
        public string a_fgtype_id { get; set; }

        public string a_LINE_ID { get; set; }
    }
    public class ITEMSROW
    {
        public string COPR_ID { get; set; }
        public string FGTYPE_ID { get; set; }
        public string PNO { get; set; }
        public string LOTNO { get; set; }
        public string DESC { get; set; }
    }
    public class pReturnMatrix
    {
        public object QTY { get; set; }
        public object AQTY { get; set; }
        public object ACCQTY { get; set; }
        public object COPNO { get; set; }
        public object COPNOEX { get; set; }
        public object COPQTY { get; set; }
        public object COPQTYEX { get; set; }

        public object COPQTY_SUGGESTION1{ get; set; }
        public object COPQTY_SUMMARY1 { get; set; }
        public object COPQTY_SUGGESTION2 { get; set; }
        public object COPQTY_SUMMARY2 { get; set; }

        public object DIM_DATE { get; set; }

        public object MODEL_REM1 { get; set; }
        public object MODEL_REM2 { get; set; }
    }
    public class ResultModel
    {
        public List<COPR16_MEASURETYPE_MSTR> cOPR16_MEASURETYPE_MSTR_list { get; set; }

        public COPR16_MEASURETYPE_MSTR cOPR16_MEASURETYPE_MSTR { get; set; }
        public COPR16_COPRUNNING_RT cOPR16_COPRUNNING_RT { get; set; }

        public COPR16_RTDT_MSTR cOPR16_RTDT_MSTR { get; set; }
        public List<COPR16_RTDT_MSTR> cOPR16_RTDT_MSTR_list { get; set; }

        public List<COPR16_OPTDTVALUE_MSTR> cOPR16_OPTDTVALUE_MSTR_List { get; set; }
        public List<COPR16_COPRUNNING_RT> cOPR16_COPRUNNING_RT_List { get; set; }
        public bool isOptionList { get; set; }
        public void getResultModel(COPR16Entities dbModel, string COPR_ID, string SEQ_NO)
        {
            cOPR16_COPRUNNING_RT = dbModel.COPR16_COPRUNNING_RT.Where(l => l.COPR_ID.Equals(COPR_ID) && l.SEQ_NO.Equals(SEQ_NO)).FirstOrDefault();
            cOPR16_RTDT_MSTR = dbModel.COPR16_RTDT_MSTR.Where(l => l.RTTYPE_ID.Equals(cOPR16_COPRUNNING_RT.RETURNTYPE_ID)).FirstOrDefault();
            cOPR16_MEASURETYPE_MSTR = dbModel.COPR16_MEASURETYPE_MSTR.Where(l => l.MSTYPE_ID.Equals(cOPR16_RTDT_MSTR.MSTYPE_ID)).FirstOrDefault();
            if (cOPR16_MEASURETYPE_MSTR.OPTION_UNIT == true)
            {
                isOptionList = true;
                cOPR16_OPTDTVALUE_MSTR_List = new List<COPR16_OPTDTVALUE_MSTR>();
                cOPR16_OPTDTVALUE_MSTR_List = dbModel.COPR16_OPTDTVALUE_MSTR.Where(l => l.OPTION_ID.Equals(cOPR16_MEASURETYPE_MSTR.OPT_ID)).ToList();
            }
            else
            {
                isOptionList = false;

            }


        }
    }
    public class searchCOP
    {
        public string fDate {get;set;}
        public string tDate { get; set; }
        public string COPNO { get; set; }
        public string testStatus { get; set; }

    }
}