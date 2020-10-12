using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CM_APPLICATIONS.Models
{
    public class JsonOptionsModel
    {
        public COPR16_OPTIONVAL_MSTR cOPR16_OPTIONVAL_MSTR { get; set; }
        public List<COPR16_OPTDTVALUE_MSTR> cOPR16_OPTDTVALUE_MSTR_LIST { get; set; }
        public COPR16_OPTDTVALUE_MSTR cOPR16_OPTDTVALUE_MSTR { get; set; }
    }
}