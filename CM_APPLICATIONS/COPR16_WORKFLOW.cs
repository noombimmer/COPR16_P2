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

    public partial class COPR16_WORKFLOW
    {

        [DisplayName("WORKFLOW ID")]
        public string WRK_ID { get; set; }

        [DisplayName("WORKFLOW NAME")]
        public string WRK_NAME { get; set; }

        [DisplayName("DESCRIPTIONS")]
        public string WRK_DESC { get; set; }

        [DisplayName("REGULATORY")]
        public string STDREG_ID { get; set; }

        [DisplayName("STANDARD PROCESS")]
        public string STD_ID { get; set; }

        [DisplayName("PROCESS TEST")]
        public string PROC_ID { get; set; }

        [DisplayName("STEP PARENT")]
        public string WRK_WITH { get; set; }

        [DisplayName("CREATED DATE")]
        public Nullable<System.DateTime> ADATE { get; set; }

        [DisplayName("CREATED BY")]
        public string CRE_BY { get; set; }

        [DisplayName("MODIFIED DATE")]
        public Nullable<System.DateTime> MOD_DATE { get; set; }

        [DisplayName("MODIFIED BY")]
        public string MOD_BY { get; set; }

        [DisplayName("ACTIVE")]
        public Nullable<bool> FLGACT { get; set; }
    }
}
