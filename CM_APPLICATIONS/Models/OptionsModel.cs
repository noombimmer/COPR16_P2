using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CM_APPLICATIONS.Models
{
    public class OptionsModel
    {
        private COPR16Entities db;
        public COPR16_OPTIONVAL_MSTR cOPR16_OPTIONVAL_MSTR { get; set; }
        public List<COPR16_OPTDTVALUE_MSTR> cOPR16_OPTDTVALUE_MSTR_LIST { get; set; }
        public COPR16_OPTDTVALUE_MSTR cOPR16_OPTDTVALUE_MSTR { get; set; }
        public OptionsModel(COPR16Entities dbModel)
        {
            db = dbModel;
            details = new OptionDetailsModel();
        }
        public OptionDetailsModel details { get; set; }
        public List<OptionDetailsModel> jsonString { get; set; }
        public string option_id { get; set; }
        public string option_name { get; set; }
        public string option_modby { get; set; }
        public string option_crerby { get; set; }
        public Nullable<System.DateTime> option_moddt { get; set; }
        public Nullable<System.DateTime> option_credt { get; set; }

    }
}