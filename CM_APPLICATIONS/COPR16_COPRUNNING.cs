//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CM_APPLICATIONS
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    public partial class COPR16_COPRUNNING
    {

        [DisplayName("COP RUNNING NO.")]
        public string COPR_ID { get; set; }

        [DisplayName("MODEL CODE")]
        public string MODEL_ID { get; set; }

        [DisplayName("POSITION")]
        public string POSITION_ID { get; set; }

        [DisplayName("FG TYPE ID")]
        public string FGTYPE_ID { get; set; }

        [DisplayName("PROCESS TEST")]
        public string WRK_ID { get; set; }

        [DisplayName("REMARK")]
        public string DESC { get; set; }

        [DisplayName("CREATED DATE")]
        public Nullable<System.DateTime> ADATE { get; set; }

        [DisplayName("CREATED BY")]
        public string CRE_BY { get; set; }

        [DisplayName("MODIFIED DATE")]
        public Nullable<System.DateTime> MOD_DATE { get; set; }

        [DisplayName("MODIFIED BY")]
        public string MOD_BY { get; set; }

        [DisplayName("COP STATUS")]
        public string COP_STATUS { get; set; }

        [DisplayName("FUNCTION INTERFACE ID")]
        public string PROC_ID { get; set; }

        [DisplayName("PRODUCT LINE ")]
        public string LINE_ID { get; set; }

    }
}
