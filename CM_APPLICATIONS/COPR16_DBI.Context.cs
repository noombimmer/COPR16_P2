﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class COPR16Entities : DbContext
    {
        public COPR16Entities()
            : base("name=COPR16Entities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<COPR16_BRAND_MSTR> COPR16_BRAND_MSTR { get; set; }
        public virtual DbSet<COPR16_FG_MSTR> COPR16_FG_MSTR { get; set; }
        public virtual DbSet<COPR16_LINE_MSTR> COPR16_LINE_MSTR { get; set; }
        public virtual DbSet<COPR16_MANCTYPE_MSTR> COPR16_MANCTYPE_MSTR { get; set; }
        public virtual DbSet<COPR16_MEASURETYPE_MSTR> COPR16_MEASURETYPE_MSTR { get; set; }
        public virtual DbSet<COPR16_OPTIONVAL_MSTR> COPR16_OPTIONVAL_MSTR { get; set; }
        public virtual DbSet<COPR16_POSITION_MSTR> COPR16_POSITION_MSTR { get; set; }
        public virtual DbSet<COPR16_RTVALUE_TR> COPR16_RTVALUE_TR { get; set; }
        public virtual DbSet<COPR16_UNITS_MSTR> COPR16_UNITS_MSTR { get; set; }
        public virtual DbSet<COPR16_ITEMS_MSTR> COPR16_ITEMS_MSTR { get; set; }
        public virtual DbSet<COPR16_ITEMS_VOLUME> COPR16_ITEMS_VOLUME { get; set; }
        public virtual DbSet<COPR16_MODEL_MSTR> COPR16_MODEL_MSTR { get; set; }
        public virtual DbSet<COPR16_MACHINE_MSTR> COPR16_MACHINE_MSTR { get; set; }
        public virtual DbSet<COPR16_PROC_MSTR> COPR16_PROC_MSTR { get; set; }
        public virtual DbSet<COPR16_STD_MSTR> COPR16_STD_MSTR { get; set; }
        public virtual DbSet<COPR16_STDREG_MSTR> COPR16_STDREG_MSTR { get; set; }
        public virtual DbSet<COPR16_FGTDET_MSTR> COPR16_FGTDET_MSTR { get; set; }
        public virtual DbSet<COPR16_FGTEST> COPR16_FGTEST { get; set; }
        public virtual DbSet<COPR16_FGTYPE_MSTR> COPR16_FGTYPE_MSTR { get; set; }
        public virtual DbSet<COPR16_GROUPTEST_MSTR> COPR16_GROUPTEST_MSTR { get; set; }
        public virtual DbSet<COPR16_STDDET> COPR16_STDDET { get; set; }
        public virtual DbSet<COPR16_STEP_MSTR> COPR16_STEP_MSTR { get; set; }
        public virtual DbSet<COPR16_SUBGRPTEST_MSTR> COPR16_SUBGRPTEST_MSTR { get; set; }
        public virtual DbSet<COPR16_REPORT> COPR16_REPORT { get; set; }
        public virtual DbSet<COPR16_FILES_MSTR> COPR16_FILES_MSTR { get; set; }
        public virtual DbSet<COPR16_FTYPE_MSTR> COPR16_FTYPE_MSTR { get; set; }
        public virtual DbSet<COPR16_OPTDTVALUE_MSTR> COPR16_OPTDTVALUE_MSTR { get; set; }
        public virtual DbSet<COPR16_RETURNTYPE_MSTR> COPR16_RETURNTYPE_MSTR { get; set; }
        public virtual DbSet<COPR16_RTDT_MSTR> COPR16_RTDT_MSTR { get; set; }
        public virtual DbSet<COPR16_WORKFLOW> COPR16_WORKFLOW { get; set; }
        public virtual DbSet<COPR16_WORKFLOW_DT> COPR16_WORKFLOW_DT { get; set; }
        public virtual DbSet<COPR16_COPRUNNING> COPR16_COPRUNNING { get; set; }
        public virtual DbSet<COPR16_COPRUNNING_DT> COPR16_COPRUNNING_DT { get; set; }
        public virtual DbSet<COPR16_COPRUNNING_RT> COPR16_COPRUNNING_RT { get; set; }
        public virtual DbSet<COPR16_FILE_IMPORT> COPR16_FILE_IMPORT { get; set; }
        public virtual DbSet<COPR16_IMPORT_CONTENTS> COPR16_IMPORT_CONTENTS { get; set; }
    
        public virtual ObjectResult<GetSeqNextValue1_Result> GetSeqNextValue(string seqName)
        {
            var seqNameParameter = seqName != null ?
                new ObjectParameter("SeqName", seqName) :
                new ObjectParameter("SeqName", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetSeqNextValue1_Result>("GetSeqNextValue", seqNameParameter);
        }
    
        public virtual ObjectResult<getItems_Result> getItems(string fg_no, string brand_no, string model_no)
        {
            var fg_noParameter = fg_no != null ?
                new ObjectParameter("fg_no", fg_no) :
                new ObjectParameter("fg_no", typeof(string));
    
            var brand_noParameter = brand_no != null ?
                new ObjectParameter("brand_no", brand_no) :
                new ObjectParameter("brand_no", typeof(string));
    
            var model_noParameter = model_no != null ?
                new ObjectParameter("model_no", model_no) :
                new ObjectParameter("model_no", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<getItems_Result>("getItems", fg_noParameter, brand_noParameter, model_noParameter);
        }
    
        public virtual ObjectResult<GetAllItems_Result> GetAllItems()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetAllItems_Result>("GetAllItems");
        }
    
        public virtual int GetItemWithParams(string item_no, string model_no, string brand_no)
        {
            var item_noParameter = item_no != null ?
                new ObjectParameter("item_no", item_no) :
                new ObjectParameter("item_no", typeof(string));
    
            var model_noParameter = model_no != null ?
                new ObjectParameter("model_no", model_no) :
                new ObjectParameter("model_no", typeof(string));
    
            var brand_noParameter = brand_no != null ?
                new ObjectParameter("brand_no", brand_no) :
                new ObjectParameter("brand_no", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("GetItemWithParams", item_noParameter, model_noParameter, brand_noParameter);
        }
    
        public virtual ObjectResult<string> sp_insert_excel_volume(string fSIM_NAME, string fSIM_EXT, string fSIM_PATH, string fSIM_SIZE, string fSIM_FORMAT, ObjectParameter fSIM_UNIQ_ID, string cRE_BY, string fSIM_SEQDT)
        {
            var fSIM_NAMEParameter = fSIM_NAME != null ?
                new ObjectParameter("FSIM_NAME", fSIM_NAME) :
                new ObjectParameter("FSIM_NAME", typeof(string));
    
            var fSIM_EXTParameter = fSIM_EXT != null ?
                new ObjectParameter("FSIM_EXT", fSIM_EXT) :
                new ObjectParameter("FSIM_EXT", typeof(string));
    
            var fSIM_PATHParameter = fSIM_PATH != null ?
                new ObjectParameter("FSIM_PATH", fSIM_PATH) :
                new ObjectParameter("FSIM_PATH", typeof(string));
    
            var fSIM_SIZEParameter = fSIM_SIZE != null ?
                new ObjectParameter("FSIM_SIZE", fSIM_SIZE) :
                new ObjectParameter("FSIM_SIZE", typeof(string));
    
            var fSIM_FORMATParameter = fSIM_FORMAT != null ?
                new ObjectParameter("FSIM_FORMAT", fSIM_FORMAT) :
                new ObjectParameter("FSIM_FORMAT", typeof(string));
    
            var cRE_BYParameter = cRE_BY != null ?
                new ObjectParameter("CRE_BY", cRE_BY) :
                new ObjectParameter("CRE_BY", typeof(string));
    
            var fSIM_SEQDTParameter = fSIM_SEQDT != null ?
                new ObjectParameter("FSIM_SEQDT", fSIM_SEQDT) :
                new ObjectParameter("FSIM_SEQDT", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("sp_insert_excel_volume", fSIM_NAMEParameter, fSIM_EXTParameter, fSIM_PATHParameter, fSIM_SIZEParameter, fSIM_FORMATParameter, fSIM_UNIQ_ID, cRE_BYParameter, fSIM_SEQDTParameter);
        }
    
        public virtual int sp_insert_volume_contents_stg(string fSIM_ID, string iTEM_ID, string qTY, string cRE_BY)
        {
            var fSIM_IDParameter = fSIM_ID != null ?
                new ObjectParameter("FSIM_ID", fSIM_ID) :
                new ObjectParameter("FSIM_ID", typeof(string));
    
            var iTEM_IDParameter = iTEM_ID != null ?
                new ObjectParameter("ITEM_ID", iTEM_ID) :
                new ObjectParameter("ITEM_ID", typeof(string));
    
            var qTYParameter = qTY != null ?
                new ObjectParameter("QTY", qTY) :
                new ObjectParameter("QTY", typeof(string));
    
            var cRE_BYParameter = cRE_BY != null ?
                new ObjectParameter("CRE_BY", cRE_BY) :
                new ObjectParameter("CRE_BY", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_insert_volume_contents_stg", fSIM_IDParameter, iTEM_IDParameter, qTYParameter, cRE_BYParameter);
        }
    }
}
