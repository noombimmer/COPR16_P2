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

    public partial class COPR16_MEASURETYPE_MSTR
    {

        [DisplayName("ID")]
        public string MSTYPE_ID { get; set; }

        [DisplayName("NAME")]
        public string MSTYPE_NAME { get; set; }

        [DisplayName("UNIT")]
        public string UNIT_ID { get; set; }

        [DisplayName("DEFAULT VALUE")]
        public Nullable<decimal> DEF_VALUE { get; set; }

        [DisplayName("CREATED DATE")]
        public Nullable<System.DateTime> ADATE { get; set; }

        [DisplayName("CREATED BY")]
        public string CRE_BY { get; set; }

        [DisplayName("MODIFIED BY")]
        public string MOD_BY { get; set; }

        [DisplayName("MODIFIED DATE")]
        public Nullable<System.DateTime> MOD_DATE { get; set; }

        [DisplayName("IS UNIT")]
        public Nullable<bool> OPTION_UNIT { get; set; }

        [DisplayName("IS FROM FILE")]
        public Nullable<bool> FROM_FILE { get; set; }

        [DisplayName("ACTIVE")]
        public Nullable<bool> FLGACT { get; set; }

        [DisplayName("OPTION VALUE")]
        public string OPT_ID { get; set; }

        [DisplayName("FILE")]
        public string FILE_ID { get; set; }

        [DisplayName("MIN VALUE")]
        public Nullable<decimal> MIN_VALUE { get; set; }

        [DisplayName("MAX VALUE")]
        public Nullable<decimal> MAX_VALUE { get; set; }

        [DisplayName("DESCRIPTION")]
        public string DESC { get; set; }
    }
}
