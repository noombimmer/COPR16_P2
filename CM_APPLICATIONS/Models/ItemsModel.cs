using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CM_APPLICATIONS.Models
{
    public class ItemsModel
    {
        private COPR16Entities db;
        public ItemsModel(COPR16Entities dbModel)
        {
            db = dbModel;
            this.FG_LIST = new List<SelectListItem>();
            this.FGTYPE_LIST = new List<SelectListItem>();
            this.BRAND_LIST = new List<SelectListItem>();
            this.MODEL_LIST = new List<SelectListItem>();
            this.LINE_LIST = new List<SelectListItem>();
            this.POSITION_LIST = new List<SelectListItem>();

            foreach (var row in db.COPR16_FG_MSTR.Where(l => l.FLGACT == true))
            {
                FG_LIST.Add(new SelectListItem { Text = row.FG_NO, Value = row.FG_NO });
            }
            foreach (var row in db.COPR16_BRAND_MSTR.Where(l => l.FLGACT == true))
            {
                BRAND_LIST.Add(new SelectListItem { Text = row.BRAND_ID, Value = row.BRAND_ID });
            }
            foreach (var row in db.COPR16_MODEL_MSTR.Where(l => l.FLGACT == true))
            {
                MODEL_LIST.Add(new SelectListItem { Text = row.MODEL_ID, Value = row.MODEL_ID });
            }
            foreach (var row in db.COPR16_LINE_MSTR.Where(l => l.FLGACT == true))
            {
                LINE_LIST.Add(new SelectListItem { Text = row.LINE_ID, Value = row.LINE_ID });
            }
            foreach (var row in db.COPR16_POSITION_MSTR.Where(l => l.FLGACT == true))
            {
                POSITION_LIST.Add(new SelectListItem { Text = row.POS_ID, Value = row.POS_ID });
            }
            foreach (var row in db.COPR16_FGTYPE_MSTR.Where(l => l.FLGACT == true))
            {
                FGTYPE_LIST.Add(new SelectListItem { Text = row.FGT_ID, Value = row.FGT_ID });
            }
        }

        public List<SelectListItem> FG_LIST { get; set; }
        public List<SelectListItem> FGTYPE_LIST { get; set; }
        public List<SelectListItem> BRAND_LIST { get; set; }
        public List<SelectListItem> MODEL_LIST { get; set; }
        public List<SelectListItem> LINE_LIST { get; set; }
        public List<SelectListItem> POSITION_LIST { get; set; }
        public COPR16_ITEMS_MSTR cOPR16_ITEMS_MSTR { get; set; }

        public COPR16_MODEL_MSTR cOPR16_MODEL_MSTR { get; set; }
        public String FG_NO { get; set; }
        public String BRAND_ID { get; set; }
        public String MODELID { get; set; }
        public String LINE_ID { get; set; }
        public String POSITION_ID { get; set; }

    }
    public class VolumeMatrixParams
    {
        public string FROM_DATE { get; set; }
        public string TO_DATE { get; set; }
        public string ITEM_ID { get; set; }
        public string MODEL_ID { get; set; }
    }
    public class ModelSearch
    {
        public string MODEL_ID { get; set; }
        public string LINE_ID { get; set; }
        public string ITEM_ID { get; set; }
        public string FG_NO { get; set; }

    }
    public class ItemMatchModel
    {
        COPR16Entities db;
        public ItemMatchModel(COPR16Entities dbModel)
        {
            db = dbModel;
        }
        public COPR16_ITEMS_MSTR cOPR16_ITEMS_MSTR { get; set; }
        public COPR16_LINE_MSTR cOPR16_LINE_MSTR { get; set; }
        public COPR16_MODEL_MSTR cOPR16_MODEL_MSTR { get; set; }
        public COPR16_POSITION_MSTR cOPR16_POSITION_MSTR { get; set; }
        public COPR16_FG_MSTR cOPR16_FG_MSTR { get; set; }
        public List<COPR16_ITEMS_MSTR> cOPR16_ITEMS_MSTR_LIST { get; set; }
        public IEnumerable<Dictionary<string, object>> Serialize(SqlDataReader reader)
        {
            var results = new List<Dictionary<string, object>>();
            var cols = new List<string>();
            for (var i = 0; i < reader.FieldCount; i++)
                cols.Add(reader.GetName(i));

            while (reader.Read())
                results.Add(SerializeRow(cols, reader));

            return results;
        }
        private Dictionary<string, object> SerializeRow(IEnumerable<string> cols,
                                                        SqlDataReader reader)
        {
            var result = new Dictionary<string, object>();
            foreach (var col in cols)
                result.Add(col, reader[col]);
            return result;
        }
        private static IEnumerable<object[]> Read(SqlDataReader reader)
        {

            while (reader.Read())
            {
                var values = new List<object>();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    values.Add(reader.GetValue(i));
                }
                yield return values.ToArray();
            }
        }
        public async Task<IEnumerable<Dictionary<string, object>>> ExecuteSQLAsync(string SQLCMD,List<SqlParameter> SqlParms)
        {
            using (var cmd = db.Database.Connection.CreateCommand())
            {
                await db.Database.Connection.OpenAsync();
                cmd.CommandText = SQLCMD;
                foreach (var parm in SqlParms)
                {
                    cmd.Parameters.Add(parm);
                }
                DbDataReader reader = await cmd.ExecuteReaderAsync();
                return Serialize((SqlDataReader)reader);
            }
            
        }
    }
    public class FG_MAPPED
    {
        public string POS_ID { get; set; }
        public string FG_NO { get; set; }
        public string ITEM_ID { get; set; }
        public string FGTYPE_ID { get; set; }
        public string BRAND_ID { get; set; }
        public string MODEL_ID { get; set; }
        public string LINE_ID { get; set; }
        public string ADATE { get; set; }
        public string MOD_DATE { get; set; }
        public string MOD_BY { get; set; }
        public string CRE_BY { get; set; }
        public string FLGACT { get; set; }
    }
    public class MATCHING_MODEL
    {
        public COPR16_MODEL_MSTR cOPR16_MODEL_MSTR { get; set; }
        public COPR16_LINE_MSTR cOPR16_LINE_MSTR { get; set; }

        public COPR16_ITEMS_MSTR cOPR16_ITEMS_MSTR { get; set; }
    }
}