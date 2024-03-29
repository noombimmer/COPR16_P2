﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace CM_APPLICATIONS.Models
{
    public class FileImportModel
    {
        private COPR16Entities db;

        public COPR16_FILE_IMPORT cOPR16_FILE_IMPORT;

        public List<COPR16_FILE_IMPORT> cOPR16_FILE_IMPORT_List;

        public COPR16_IMPORT_CONTENTS cOPR16_IMPORT_CONTENTS;

        public List<COPR16_IMPORT_CONTENTS> cOPR16_IMPORT_CONTENTS_List;

        public FileImportModel(COPR16Entities dataCtx)
        {
            db = dataCtx;
        }
    }
    public class ImportModels
    {
        private COPR16Entities db;

        public COPR16_MODEL_MSTR cOPR16_MODEL_MSTR { get; set; }
        public COPR16_LINE_MSTR cOPR16_LINEL_MSTR { get; set; }
        public COPR16_POSITION_MSTR cOPR16_POSITION_MSTR { get; set; }
        public COPR16_FGTYPE_MSTR cOPR16_FGTYPE_MSTR { get; set; }

        public ImportModels(COPR16Entities dataCtx)
        {
            db = dataCtx;
        }
    }
    public class FileImportGlock
    {
        private COPR16Entities db;

        public COPR16_FILE_IMPORT cOPR16_FILE_IMPORT;

        public COPR16_COPRUNNING cOPR16_COPRUNNING;

        public List<COPR16_FILE_IMPORT> cOPR16_FILE_IMPORT_List;

        public COPR16_IMPORT_CONTENTS cOPR16_IMPORT_CONTENTS;

        public List<COPR16_IMPORT_CONTENTS> cOPR16_IMPORT_CONTENTS_List;

        public RST_HEADER rST_HEADER;

        public List<RST_HEADER> rST_HEADER_List;

        public RST_DETIAL rST_DETIAL;
        public List<RST_DETIAL> rST_DETIAL_List;

        public FileImportGlock(COPR16Entities dataCtx)
        {
            db = dataCtx;
        }
    }
    public class SPCDATAS
    {
        public List<SPCDATA> SEC51 { get; set; }
        public List<SPCDATA> SEC52 { get; set; }
        public List<SPCDATA> SEC6 { get; set; }
        public List<SPCDATA> SEC7 { get; set; }
        public string YEAR { get; set; }
    }
    public class SPCDATA
    {
        public string PART { get; set; }
        public string LOT { get; set; }
        public string VAL { get; set; }
        public string POS { get; set; }
        public string MONTH { get; set; }
    }
    public class UniqCheck
    {
        public string UNIQ_ID { get; set; }
        public string GUID { get; set; }
        public string SQLCMD { get; set; }
    }
    public class jSon_UniqID
    {
        public string id { get; set; }
    }
    public class FileUploadHeader
    {
        public string FSIM_DT { get; set; }
        public string FSIM_NAME { get; set; }
        public string FSIM_EXT { get; set; }
        public string FSIM_PATH { get; set; }
        public string FSIM_SIZE { get; set; }
        public string FSIM_FORMAT { get; set; }
        public string FSIM_UNIQ_ID { get; set; }
        public string CRE_BY { get; set; }
        public string FSIM_SEQID { get; set; }
    }
    public class fgVolume
    {
        public string FSIM_UNIQ_ID { get; set; }
        public string ITEM_ID { get; set; }
        public string BUSI_DATE { get; set; }
        public string QTY { get; set; }

    }
    public class newModelData
    {
        public string ModelName { get; set; }
        public string LineName { get; set; }
        public string PositionName { get; set; }
        public string SBPart { get; set; }
        public string Bck1Part { get; set; }
        public string Bck2Part { get; set; }


    }
    public class RST_HEADER
    {
        public string RST_ID { get; set; }

        [DisplayName("COPR16 NO: ")]
        public string COPR_ID { get; set; }
        public string RST_FILENAME { get; set; }

        public string RST_FILE_DATA { get; set; }

        public DateTime ADATE { get; set; }
        public string CRE_BY { get; set; }

        public DateTime MOD_DATE { get; set; }
        public string MOD_BY { get; set; }
    }
    public partial class RST_DETIAL
    {
        public string RST_ID { get; set; }
        public string RST_KEY { get; set; }
        public string RST_VALUE { get; set; }
        public bool RST_SELECT { get; set; }
        public string RTDT_ID { get; set; }
    }
}
