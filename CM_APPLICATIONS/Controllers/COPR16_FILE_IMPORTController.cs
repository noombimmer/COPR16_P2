using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.OleDb;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CM_APPLICATIONS;
using CM_APPLICATIONS.Models;
using System.Data.SqlClient;
using System.Data.Common;
using System.IO;
using System.Text;


using System.Text.RegularExpressions;
using System.Globalization;
using System.ComponentModel;
using System.Configuration;

namespace CM_APPLICATIONS.Controllers
{
    public class COPR16_FILE_IMPORTController : Controller
    {
        private COPR16Entities db = new COPR16Entities();
        public string UploadFilePath;
        //PARTLISTUploading
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AsyncTimeout(50000)]
        public async Task<JsonResult> PARTLISTUploading(HttpPostedFileBase FSIM_NAME, string hidden_cOPR16_FILE_IMPORT_FSIM_SEQDT, string UID,string SPC_MODEL_LIST,string SPC_YEAR)
        {
            var rowData = new JsonResult();
            DataTable dt = new DataTable();


            int rowIndex = 0;
            List<Dictionary<string, object>> ColumnOfRows = new List<Dictionary<string, object>>();
            List<string> colNameArr = new List<string>();
            List<string> RowcolNameArr = new List<string>();
            if (FSIM_NAME.ContentLength > 0)
            {
                string rootPath = AppPropModel.rootPath;
                string subPath = UID + "/" + hidden_cOPR16_FILE_IMPORT_FSIM_SEQDT;
                bool exists = System.IO.Directory.Exists(Server.MapPath(rootPath + "/" + subPath));
                if (!exists)
                    System.IO.Directory.CreateDirectory(Server.MapPath(rootPath + "/" + subPath));

                var fileName = Path.GetFileName(FSIM_NAME.FileName);

                var path = Path.Combine(Server.MapPath(rootPath + "/" + subPath), fileName);
                /*
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                */
                FSIM_NAME.SaveAs(path);
                string Ext = Path.GetExtension(path);
                dt = FillGridFromExcelSheetEx(path, Ext, "YES");
            }
            Boolean tableCreate = false;
            Boolean tableDDL = false;
            string table_name = "COPR16_TNS_PART_UPLOAD";
            string SQLDDL = "CREATE TABLE " + table_name + " (";
            string CHECKDDL = "";
            List<Dictionary<string, object>> rows1 = new List<Dictionary<string, object>>();
            Dictionary<string, object> row1;
            foreach (DataRow dr in dt.Rows)
            {
                row1 = new Dictionary<string, object>();
                foreach (DataColumn col in dt.Columns)
                {
                    row1.Add(col.ColumnName, dr[col]);
                    if (tableDDL != true) SQLDDL += "[" + col.ColumnName.Replace('(', '_').Replace(')', '_') + "] nvarchar(512), ";
                }

                if (tableDDL != true)
                {
                    SQLDDL = SQLDDL.Substring(0, SQLDDL.Length - 2) + " )";
                    //SQLDDL += ",UPDATE_BY nvarchar(512), )";
                    CHECKDDL = "DECLARE @TABLENAME NVARCHAR(50) = '" + table_name + "';\r\n" +
                                "DECLARE @SQL_CREATE NVARCHAR(max) = '" + SQLDDL + "';\r\n" +
                                "DECLARE @SQL_TRUCATE NVARCHAR(max) = 'TRUNCATE TABLE ' + @TABLENAME;\r\n" +
                                "DECLARE @SQL_DROP NVARCHAR(max) = 'DROP TABLE ' + @TABLENAME;\r\n" +
                                "IF OBJECT_ID('dbo.' + @TABLENAME ) IS NOT NULL\r\n" +
                                "BEGIN\r\n" +
                                "EXECUTE sp_executesql @SQL_DROP\r\n" +
                                "EXECUTE sp_executesql @SQL_CREATE\r\n" +
                                "END;\r\n" +
                                 "ELSE\r\n" +
                                 "BEGIN\r\n" +
                                 "EXECUTE sp_executesql @SQL_CREATE\r\n" +
                                "END;\r\n";
                    //var rtn = await db.Database.ExecuteSqlCommandAsync(CHECKDDL);
                    if (db.Database.Connection.State != ConnectionState.Open)
                    {
                        db.Database.Connection.Open();
                    }
                    string SQLCMD2 = CHECKDDL;

                    using (var cmd = db.Database.Connection.CreateCommand())
                    {
                        cmd.CommandText = SQLCMD2;

                        //cmd.Parameters.Add(new SqlParameter("@COPR_ID", COPNO == null ? "" : COPNO));
                        //cmd.Parameters.Add(new SqlParameter("@LINE_ID", LINE_ID == null ? "" : LINE_ID));
                        cmd.ExecuteNonQuery();
                    }
                    tableDDL = true;
                }

                rows1.Add(row1);
            }
            using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DatabaseServer"].ToString()))
            {
                using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
                {
                    //Set the database table name.
                    sqlBulkCopy.DestinationTableName = "dbo." + table_name;
                    foreach (DataColumn col in dt.Columns)
                    {
                        sqlBulkCopy.ColumnMappings.Add(col.ColumnName, col.ColumnName.Replace('(', '_').Replace(')', '_'));
                        //row1.Add(col.ColumnName, dr[col]);
                        //if (tableDDL != true) SQLDDL += "[" + col.ColumnName.Replace('(', '_').Replace(')', '_') + "] nvarchar(512), ";
                    }
                    //[OPTIONAL]: Map the Excel columns with that of the database table
                    //sqlBulkCopy.ColumnMappings.Add("Id", "CustomerId");
                    //sqlBulkCopy.ColumnMappings.Add("Name", "Name");
                    //sqlBulkCopy.ColumnMappings.Add("Country", "Country");
                    con.Open();
                    sqlBulkCopy.WriteToServer(dt);
                    con.Close();
                }
            }
            using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DatabaseServer"].ToString()))
            {

                await con.OpenAsync();
                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = "exec COPR16_UPDATE_TNS_RPT_TEMPLATE @YEAR,@MODEL,@UID;";
                    cmd.Parameters.Add(new SqlParameter("@YEAR", SPC_YEAR));
                    cmd.Parameters.Add(new SqlParameter("@MODEL", SPC_MODEL_LIST));
                    cmd.Parameters.Add(new SqlParameter("@UID", UID));
                    //cmd.ExecuteNonQuery();

                    await cmd.ExecuteNonQueryAsync();
                    //await cmd.ExecuteSqlCommandAsync(sqlCOMMAND_ITEMS, sqlParams.ToArray());
                }
                con.Close();
            }
            rowData.Data = rows1;
            //row.Data = new object[] { 1, 2,5 };
            JsonResult json = Json(rowData, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;
            //return Json(rowData, JsonRequestBehavior.AllowGet);
            return json;
            //return rowData;
        }

        //SPCUploading
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AsyncTimeout(50000)]
        public async Task<JsonResult> SPCUploading(HttpPostedFileBase FSIM_NAME, string hidden_cOPR16_FILE_IMPORT_FSIM_SEQDT, string UID)
        {
            var rowData = new JsonResult();
            DataTable dt = new DataTable();

            int rowIndex = 0;
            List<Dictionary<string, object>> ColumnOfRows = new List<Dictionary<string, object>>();
            List<string> colNameArr = new List<string>();
            List<string> RowcolNameArr = new List<string>();
            if (FSIM_NAME.ContentLength > 0)
            {
                string rootPath = AppPropModel.rootPath;
                string subPath = UID + "/" + hidden_cOPR16_FILE_IMPORT_FSIM_SEQDT;
                bool exists = System.IO.Directory.Exists(Server.MapPath(rootPath + "/" + subPath));
                if (!exists)
                    System.IO.Directory.CreateDirectory(Server.MapPath(rootPath + "/" + subPath));

                var fileName = Path.GetFileName(FSIM_NAME.FileName);

                var path = Path.Combine(Server.MapPath(rootPath + "/" + subPath), fileName);
                /*
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                */
                FSIM_NAME.SaveAs(path);
                string Ext = Path.GetExtension(path);
                dt = FillGridFromExcelSheetForSPC(path, Ext, "YES");

            }
            Boolean tableCreate = false;
            Boolean tableDDL= false;
            string table_name = "COPR16_SPC_UPLOAD";
            string SQLDDL = "CREATE TABLE "+ table_name + " (";
            string CHECKDDL = "";
            List<Dictionary<string, object>> rows1 = new List<Dictionary<string, object>>();
            Dictionary<string, object> row1;
            foreach (DataRow dr in dt.Rows)
            {
                row1 = new Dictionary<string, object>();
                foreach (DataColumn col in dt.Columns)
                {
                    row1.Add(col.ColumnName, dr[col]);
                    if(tableDDL != true) SQLDDL += "[" + col.ColumnName.Replace('(','_').Replace(')', '_') + "] nvarchar(100), ";
                }
                if (tableDDL != true)
                {
                    SQLDDL = SQLDDL.Substring(0, SQLDDL.Length - 2) + " )";
                    //SQLDDL += ",UPDATE_BY nvarchar(512), )";
                    CHECKDDL = "DECLARE @TABLENAME NVARCHAR(50) = '"+ table_name + "';\r\n" +
                                "DECLARE @SQL_CREATE NVARCHAR(MAX) = '" + SQLDDL + "';\r\n" +
                                "DECLARE @SQL_TRUCATE NVARCHAR(MAX) = 'TRUNCATE TABLE ' + @TABLENAME;\r\n" +
                                "DECLARE @SQL_DROP NVARCHAR(MAX) = 'DROP TABLE ' + @TABLENAME;\r\n" +
                                "DECLARE @SQL_INDEX NVARCHAR(MAX) = 'DROP TABLE ' + @TABLENAME;\r\n" +
                                "IF OBJECT_ID('dbo.' + @TABLENAME ) IS NOT NULL\r\n" +
                                "BEGIN\r\n" +
                                "EXECUTE sp_executesql @SQL_DROP\r\n" +
                                "EXECUTE sp_executesql @SQL_CREATE\r\n" +
                                "END;\r\n" +
                                 "ELSE\r\n" +
                                 "BEGIN\r\n" +
                                 "EXECUTE sp_executesql @SQL_CREATE;\r\n" +
                                "END;\r\n";
                    //var rtn = await db.Database.ExecuteSqlCommandAsync(CHECKDDL);
                    if (db.Database.Connection.State != ConnectionState.Open)
                    {
                        db.Database.Connection.Open();
                    }
                    string SQLCMD2 = CHECKDDL;

                    using (var cmd = db.Database.Connection.CreateCommand())
                    {
                        cmd.CommandText = SQLCMD2;

                        //cmd.Parameters.Add(new SqlParameter("@COPR_ID", COPNO == null ? "" : COPNO));
                        //cmd.Parameters.Add(new SqlParameter("@LINE_ID", LINE_ID == null ? "" : LINE_ID));
                        cmd.ExecuteNonQuery();
                    }
                    tableDDL = true;
                }

                rows1.Add(row1);
            }
            using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DatabaseServer"].ToString()))
            {
                
                using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
                {
                    //Set the database table name.
                    sqlBulkCopy.DestinationTableName = "dbo." + table_name;
                    foreach (DataColumn col in dt.Columns)
                    {
                        sqlBulkCopy.ColumnMappings.Add(col.ColumnName, col.ColumnName.Replace('(', '_').Replace(')', '_'));

                    }

                    await con.OpenAsync();
                    //sqlBulkCopy.WriteToServer(dt);
                    sqlBulkCopy.BatchSize = 1000;
                    sqlBulkCopy.BulkCopyTimeout = 5000;
                    sqlBulkCopy.EnableStreaming = true;
                    await sqlBulkCopy.WriteToServerAsync(dt);
                    con.Close();
                }
            }
            rowData.Data = rows1;
            using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DatabaseServer"].ToString()))
            {
                //con.Open();
                
                await con.OpenAsync();
                using (var cmd = db.Database.Connection.CreateCommand())
                {
                    cmd.CommandText = "exec dbo.COPR16_UPDATE_CUBE_3";
                    cmd.CommandTimeout = 50000;
                    //cmd.ExecuteNonQuery();
                    await cmd.ExecuteNonQueryAsync();
                }
                con.Close();
            }
            using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DatabaseServer"].ToString()))
            {
                //con.Open();
                await con.OpenAsync();
                using (var cmd = db.Database.Connection.CreateCommand())
                {
                    cmd.CommandText = "exec dbo.COPR16_UPDATE_CUBE_00";
                    //cmd.ExecuteNonQuery();
                    cmd.CommandTimeout = 50000;
                    await cmd.ExecuteNonQueryAsync();
                }
                con.Close();
            }
            using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DatabaseServer"].ToString()))
            {
                //con.Open();
                await con.OpenAsync();
                using (var cmd = db.Database.Connection.CreateCommand())
                {
                    cmd.CommandText = "exec dbo.COPR16_SPCC_FACT_GENERATE '" + UID + "';";
                    //cmd.ExecuteNonQuery();
                    cmd.CommandTimeout = 50000;
                    
                    await cmd.ExecuteNonQueryAsync();
                }
                con.Close();
            }
            /*
            using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DatabaseServer"].ToString()))
            {
                con.Open();
                using (var cmd = db.Database.Connection.CreateCommand())
                {
                    cmd.CommandText = "exec dbo.COPR16_UPDATE_CUBE_99";
                    cmd.ExecuteNonQuery();
                }
                con.Close();
            }
            */
            JsonResult json = Json(rowData, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;
            return json;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Uploading(HttpPostedFileBase FSIM_NAME,string hidden_cOPR16_FILE_IMPORT_FSIM_SEQDT, string UID)
        {
            var rowData = new JsonResult();
            DataTable dt = new DataTable();

            int rowIndex = 0;
            List<Dictionary<string, object>> ColumnOfRows = new List<Dictionary<string, object>>();
            List<string> colNameArr = new List<string> ();
            List<string> RowcolNameArr = new List<string>();
            if (FSIM_NAME.ContentLength > 0)
            {
                string rootPath = AppPropModel.rootPath;
                string subPath = UID + "/" + hidden_cOPR16_FILE_IMPORT_FSIM_SEQDT;
                bool exists = System.IO.Directory.Exists(Server.MapPath(rootPath + "/"+subPath));
                if (!exists)
                    System.IO.Directory.CreateDirectory(Server.MapPath(rootPath + "/" + subPath));

                var fileName = Path.GetFileName(FSIM_NAME.FileName);
               
                var path = Path.Combine(Server.MapPath(rootPath +"/"+ subPath), fileName);
                /*
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                */
                FSIM_NAME.SaveAs(path);
                string Ext = Path.GetExtension(path);
                dt = FillGridFromExcelSheet(path, Ext,"YES");
            }

            List<Dictionary<string, object>> rows1 = new List<Dictionary<string, object>>();
            Dictionary<string, object> row1;
            foreach (DataRow dr in dt.Rows)
            {
                row1 = new Dictionary<string, object>();
                foreach (DataColumn col in dt.Columns)
                {
                    row1.Add(col.ColumnName, dr[col]);
                }
                rows1.Add(row1);
            }
            rowData.Data = rows1;

            //rowData.Data =  ColumnOfRows;
            //rowData.Data = dt.Columns.Count;

            //var row = await db.Database.SqlQuery<UniqCheck>("dbo.sp_getUniqID @UNIQ_ID = @cUNIQ_ID", new object[] { param }).ToListAsync();
            //var row = await db.Database.SqlQuery<UniqCheck>("exec dbo.sp_getUniqID @p0", id).ToListAsync();
            //var row = db.Database.ExecuteSqlCommand("dbo.sp_getUniqID @UNIQ_ID", parameter);

            //row.Data = new object[] { 1, 2,5 };
            JsonResult json = Json(rowData, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;
            //return Json(rowData, JsonRequestBehavior.AllowGet);
            return json;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AsyncTimeout(50000)]
        public JsonResult UploadingModel(HttpPostedFileBase FSIM_NAME, string hidden_cOPR16_FILE_IMPORT_FSIM_SEQDT, string UID)
        {
            var rowData = new JsonResult();
            DataTable dt = new DataTable();

            if (FSIM_NAME.ContentLength > 0)
            {
                string rootPath = AppPropModel.rootPath;
                string subPath = UID + "/" + hidden_cOPR16_FILE_IMPORT_FSIM_SEQDT;
                bool exists = System.IO.Directory.Exists(Server.MapPath(rootPath + "/" + subPath));
                if (!exists)
                    System.IO.Directory.CreateDirectory(Server.MapPath(rootPath + "/" + subPath));

                var fileName = Path.GetFileName(FSIM_NAME.FileName);

                var path = Path.Combine(Server.MapPath(rootPath + "/" + subPath), fileName);
                FSIM_NAME.SaveAs(path);
                string Ext = Path.GetExtension(path);
                dt = FillGridFromExcelSheet(path, Ext, "YES");
            }

            List<Dictionary<string, object>> rows1 = new List<Dictionary<string, object>>();
            Dictionary<string, object> row1;
            foreach (DataRow dr in dt.Rows)
            {
                row1 = new Dictionary<string, object>();
                foreach (DataColumn col in dt.Columns)
                {
                    row1.Add(col.ColumnName, dr[col]);
                }
                rows1.Add(row1);
            }
            rowData.Data = rows1;

            JsonResult json = Json(rowData, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;
            return json;
            //return Json(json, JsonRequestBehavior.AllowGet);
        }
        private DataTable FillGridFromExcelSheetForSPC(string FilePath, string ext, string HDR)
        {
            bool isCSV = false;
            string connectionString = "";
            if (ext.ToLower() == ".xls")
            {   //For Excel 97-03
                connectionString = "Provider=Microsoft.ACE.OLEDB.16.0; Data Source ='{0}';Extended Properties = 'Excel 8.0;HDR={1}'";
            }
            else if (ext.ToLower() == ".xlsx")
            {    //For Excel 07 and greater
                //connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source = '{0}'; Extended Properties = 'Excel 8.0;HDR={1}'";
                connectionString = "Provider=Microsoft.ACE.OLEDB.16.0;Data Source = '{0}'; Extended Properties = 'Excel 12.0 Xml;HDR={1};IMEX=1;'";
            }
            else if (ext.ToLower() == ".csv")
            {
                //For CSV files
                isCSV = true;
                connectionString = "Provider=Microsoft.ACE.OLEDB.16.0; Data Source='{0}';Extended Properties='TEXT;HDR={1};FMT=Delimited;ImportMixedTypes=Text;IMEX=0;'";
            }

            //Fetch 1st Sheet Name

            DataTable dt = new DataTable();

            if (!isCSV)
            {

                connectionString = String.Format(connectionString, FilePath, HDR);
                OleDbConnection conn = new OleDbConnection(connectionString);

                conn.Open();
                DataTable dtSchema;
                dtSchema = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                string ExcelSheetName = dtSchema.Rows[0]["TABLE_NAME"].ToString();

                OleDbDataAdapter dataAdapter = new OleDbDataAdapter("SELECT * From [" + ExcelSheetName + "]", conn);
                dataAdapter.Fill(dt);
                dataAdapter.Dispose();

                conn.Close();
            }
            else
            {
                //connectionString = String.Format(connectionString, Path.GetDirectoryName(FilePath), HDR);
                connectionString = String.Format(connectionString, Path.GetDirectoryName(FilePath), HDR);
                OleDbConnection conn = new OleDbConnection(connectionString);

                conn.Open();
                OleDbDataAdapter dataAdapter = new OleDbDataAdapter("SELECT * From [" + Path.GetFileName(FilePath) + "] ", conn);

                var cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT * From [" + Path.GetFileName(FilePath) + "] ";
                DbDataReader reader = cmd.ExecuteReader();
                {
                    //var model = Serialize(reader);
                    dt.Load(reader);
                }
                DataTable dt2 = null;
                using (GenericParserAdapter gp = new GenericParserAdapter(FilePath))
                {
                    //gp.FirstRowSetsExpectedColumnCount = true;
                    gp.FirstRowHasHeader = true;
                    dt2 = gp.GetDataTable();
                }
                /*
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    String dateStr = dt2.Rows[i]["Date"].ToString();
                    String dateStr2 = dt2.Rows[i][1].ToString();
                    DateTime dtStr;
                    if (DateTime.TryParse(dateStr, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out dtStr))
                    {
                        //DateTime dtStr = DateTime.Parse(dateStr, System.Globalization.CultureInfo.CurrentUICulture);
                        String newStr = dtStr.ToString("dd/MM/yyyy");
                        CultureInfo cult = new CultureInfo("th-TH");
                        bool flagTry = DateTime.TryParseExact(newStr, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out dtStr);
                        if (flagTry)
                        {
                            //dtStr = DateTime.Parse(newStr, cult);
                            dt.Rows[i]["Date"] = dtStr;
                        }
                    }
                    else
                    {
                        String ID = dt2.Rows[i][0].ToString();
                        dateStr = dt2.Rows[i]["Date"].ToString();
                        Console.WriteLine(ID + ":" + dateStr);
                        bool flagTry = DateTime.TryParseExact(dateStr, "dd/M/yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out dtStr);
                        if (flagTry)
                        {
                            //dtStr = DateTime.Parse(newStr, cult);
                            dt.Rows[i]["Date"] = dtStr;
                        }
                    }
                }
                */
                conn.Close();
            }
            //Bind Sheet Data to GridView
            return dt;
        }
        private DataTable FillGridFromExcelSheet(string FilePath, string ext,string HDR)
        {
            bool isCSV = false;
            string connectionString = "";
            /*
            if (ext.ToLower() == ".xls")
            {   //For Excel 97-03
                connectionString = "Provider=Microsoft.ACE.OLEDB.16.0; Data Source ='{0}';Extended Properties = 'Excel 8.0;HDR={1}'";
            }
            else if (ext.ToLower() == ".xlsx")
            {    //For Excel 07 and greater
                //connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source = '{0}'; Extended Properties = 'Excel 8.0;HDR={1}'";
                connectionString = "Provider=Microsoft.ACE.OLEDB.16.0;Data Source = '{0}'; Extended Properties = 'Excel 12.0 Xml;HDR={1};IMEX=1;'";
            }
            else if(ext.ToLower() == ".csv")
            {
                //For CSV files
                isCSV = true;
                connectionString = "Provider=Microsoft.ACE.OLEDB.16.0; Data Source='{0}';Extended Properties='TEXT;HDR={1};FMT=Delimited;ImportMixedTypes=Text;IMEX=0;'";
            }
            */
            if (ext.ToLower() == ".xls")
            {   //For Excel 97-03
                connectionString = "Provider=Microsoft.ACE.OLEDB.16.0; Data Source ='{0}';Extended Properties = 'Excel 8.0;HDR={1}'";
            }
            else if (ext.ToLower() == ".xlsx")
            {    //For Excel 07 and greater
                //connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source = '{0}'; Extended Properties = 'Excel 8.0;HDR={1}'";
                connectionString = "Provider=Microsoft.ACE.OLEDB.16.0;Data Source = '{0}'; Extended Properties = 'Excel 12.0 Xml;HDR={1};IMEX=1;'";
            }
            else if (ext.ToLower() == ".csv")
            {
                //For CSV files
                isCSV = true;
                connectionString = "Provider=Microsoft.ACE.OLEDB.16.0; Data Source='{0}';Extended Properties='TEXT;HDR={1};FMT=Delimited;ImportMixedTypes=Text;IMEX=0;'";
            }
            //Fetch 1st Sheet Name

            //OleDbCommand cmd = new OleDbCommand();
            //OleDbDataAdapter dataAdapter = new OleDbDataAdapter();
            DataTable dt = new DataTable();
            //DataColumn dc = new DataColumn("ColIdx");
            //dc.AutoIncrement = true;
            //dc.AutoIncrementSeed = 1;
            //dc.DataType = typeof(Int32);
            //dt.Columns.CollectionChanged += new CollectionChangeEventHandler(Columns_CollectionChanged);
            //dt.Columns.Add(dc);

            if (!isCSV)
            {

                connectionString = String.Format(connectionString, FilePath, HDR);
                OleDbConnection conn = new OleDbConnection(connectionString);
                
                //cmd = conn.CreateCommand();
                //cmd.Connection = conn;

                conn.Open();
                DataTable dtSchema;
                dtSchema = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                string ExcelSheetName = dtSchema.Rows[0]["TABLE_NAME"].ToString();
                //conn.Close();
                //Read all data of fetched Sheet to a Data Table
                //conn.Open();
                //cmd.CommandText = "SELECT * From [" + ExcelSheetName + "]";
                OleDbDataAdapter dataAdapter = new OleDbDataAdapter("SELECT * From [" + ExcelSheetName + "]", conn);
                dataAdapter.Fill(dt);
                dataAdapter.Dispose();

                conn.Close();
                //conn.Dispose();
            }
            else
            {
                //connectionString = String.Format(connectionString, Path.GetDirectoryName(FilePath), HDR);
                connectionString = String.Format(connectionString, Path.GetDirectoryName(FilePath), HDR);
                OleDbConnection conn = new OleDbConnection(connectionString);
                
                conn.Open();
                OleDbDataAdapter dataAdapter = new OleDbDataAdapter("SELECT * From [" + Path.GetFileName(FilePath) + "] ", conn);

                var cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT * From [" + Path.GetFileName(FilePath) + "] ";
                DbDataReader reader = cmd.ExecuteReader();
                {
                    //var model = Serialize(reader);
                    dt.Load(reader);
                }
                DataTable dt2 = null;
                using (GenericParserAdapter gp = new GenericParserAdapter(FilePath))
                {
                    //gp.FirstRowSetsExpectedColumnCount = true;
                    gp.FirstRowHasHeader = true;
                    dt2 = gp.GetDataTable();
                }
                //cmd = conn.CreateCommand();
                //cmd.Connection = conn;

                //cmd.CommandText = "SELECT * From [" + Path.GetFileName(FilePath) + "] ";

                //dataAdapter.SelectCommand = cmd;
                //dt = dataAdapter.FillSchema(dt, SchemaType.Source);

                //dt.Columns["Date"].DataType = typeof(String);

                //dt = dataAdapter.FillSchema(dt, SchemaType.Source);

                //dataAdapter.Fill(dt);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    String dateStr = dt2.Rows[i]["Date"].ToString();
                    String dateStr2 = dt2.Rows[i][1].ToString();
                    DateTime dtStr;
                    if (DateTime.TryParse(dateStr, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out dtStr))
                    {
                        //DateTime dtStr = DateTime.Parse(dateStr, System.Globalization.CultureInfo.CurrentUICulture);
                        String newStr = dtStr.ToString("dd/MM/yyyy");
                        CultureInfo cult = new CultureInfo("th-TH");
                        bool flagTry = DateTime.TryParseExact(newStr, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out dtStr);
                        if (flagTry)
                        {
                            //dtStr = DateTime.Parse(newStr, cult);
                            dt.Rows[i]["Date"] = dtStr;
                        }
                    }
                    else
                    {
                        String ID = dt2.Rows[i][0].ToString();
                        dateStr = dt2.Rows[i]["Date"].ToString();
                        Console.WriteLine(ID + ":" + dateStr);
                        bool flagTry = DateTime.TryParseExact(dateStr, "dd/M/yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out dtStr);
                        if (flagTry)
                        {
                            //dtStr = DateTime.Parse(newStr, cult);
                            dt.Rows[i]["Date"] = dtStr;
                        }

                    }


                    /*if (DateTime.TryParseExact(dateStr, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out dtStr))
                    {
                        dt.Rows[i]["Date"] = dtStr.ToString("DD-MMM-yyyy");
                    }
                    */
                }
                
                

                //dataAdapter.Dispose();
                //conn.Dispose();
                //cmd.Dispose();
                //dataAdapter.Dispose();
                //conn.Dispose();
                conn.Close();
                
                //dataAdapter.Dispose();
            }

            
            //Bind Sheet Data to GridView
            return dt;
        }
        private DataTable FillGridFromExcelSheetEx(string FilePath, string ext, string HDR)
        {
            bool isCSV = false;
            string connectionString = "";
            if (ext.ToLower() == ".xls")
            {   //For Excel 97-03
                connectionString = "Provider=Microsoft.ACE.OLEDB.12.0; Data Source ='{0}';Extended Properties = 'Excel 8.0;HDR={1}'";
            }
            else if (ext.ToLower() == ".xlsx")
            {    //For Excel 07 and greater
                connectionString = "Provider=Microsoft.ACE.OLEDB.16.0;Data Source = '{0}'; Extended Properties = 'Excel 12.0 Xml;HDR={1}'";
            }
            else if (ext.ToLower() == ".csv")
            {
                //For CSV files
                isCSV = true;
                connectionString = "Provider=Microsoft.ACE.OLEDB.12.0; Data Source='{0}';Extended Properties='TEXT;HDR={1};FMT=Delimited;ImportMixedTypes=Text;IMEX=0;'";
            }

            //Fetch 1st Sheet Name

            //OleDbCommand cmd = new OleDbCommand();
            //OleDbDataAdapter dataAdapter = new OleDbDataAdapter();
            DataTable dt = new DataTable();
            DataColumn dc = new DataColumn("ColIdx");
            dc.AutoIncrement = true;
            dc.AutoIncrementSeed = 1;
            dc.DataType = typeof(Int32);
            dt.Columns.CollectionChanged += new CollectionChangeEventHandler(Columns_CollectionChanged);
            dt.Columns.Add(dc);

            if (!isCSV)
            {

                connectionString = String.Format(connectionString, FilePath, HDR);
                OleDbConnection conn = new OleDbConnection(connectionString);

                //cmd = conn.CreateCommand();
                //cmd.Connection = conn;

                conn.Open();
                DataTable dtSchema;
                dtSchema = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                string ExcelSheetName = dtSchema.Rows[0]["TABLE_NAME"].ToString();
                //conn.Close();
                //Read all data of fetched Sheet to a Data Table
                //conn.Open();
                //cmd.CommandText = "SELECT * From [" + ExcelSheetName + "]";
                OleDbDataAdapter dataAdapter = new OleDbDataAdapter("SELECT * From [" + ExcelSheetName + "]", conn);
                dataAdapter.Fill(dt);
                dataAdapter.Dispose();

                conn.Close();
                //conn.Dispose();
            }
            else
            {
                //connectionString = String.Format(connectionString, Path.GetDirectoryName(FilePath), HDR);
                connectionString = String.Format(connectionString, Path.GetDirectoryName(FilePath), HDR);
                OleDbConnection conn = new OleDbConnection(connectionString);

                conn.Open();
                OleDbDataAdapter dataAdapter = new OleDbDataAdapter("SELECT * From [" + Path.GetFileName(FilePath) + "] ", conn);

                var cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT * From [" + Path.GetFileName(FilePath) + "] ";
                DbDataReader reader = cmd.ExecuteReader();
                {
                    //var model = Serialize(reader);
                    dt.Load(reader);
                }
                DataTable dt2 = null;
                using (GenericParserAdapter gp = new GenericParserAdapter(FilePath))
                {
                    //gp.FirstRowSetsExpectedColumnCount = true;
                    gp.FirstRowHasHeader = true;
                    dt2 = gp.GetDataTable();
                }
                //cmd = conn.CreateCommand();
                //cmd.Connection = conn;

                //cmd.CommandText = "SELECT * From [" + Path.GetFileName(FilePath) + "] ";

                //dataAdapter.SelectCommand = cmd;
                //dt = dataAdapter.FillSchema(dt, SchemaType.Source);

                //dt.Columns["Date"].DataType = typeof(String);

                //dt = dataAdapter.FillSchema(dt, SchemaType.Source);

                //dataAdapter.Fill(dt);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    String dateStr = dt2.Rows[i]["Date"].ToString();
                    String dateStr2 = dt2.Rows[i][1].ToString();
                    DateTime dtStr;
                    if (DateTime.TryParse(dateStr, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out dtStr))
                    {
                        //DateTime dtStr = DateTime.Parse(dateStr, System.Globalization.CultureInfo.CurrentUICulture);
                        String newStr = dtStr.ToString("dd/MM/yyyy");
                        CultureInfo cult = new CultureInfo("th-TH");
                        bool flagTry = DateTime.TryParseExact(newStr, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out dtStr);
                        if (flagTry)
                        {
                            //dtStr = DateTime.Parse(newStr, cult);
                            dt.Rows[i]["Date"] = dtStr;
                        }
                    }
                    else
                    {
                        String ID = dt2.Rows[i][0].ToString();
                        dateStr = dt2.Rows[i]["Date"].ToString();
                        Console.WriteLine(ID + ":" + dateStr);
                        bool flagTry = DateTime.TryParseExact(dateStr, "dd/M/yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out dtStr);
                        if (flagTry)
                        {
                            //dtStr = DateTime.Parse(newStr, cult);
                            dt.Rows[i]["Date"] = dtStr;
                        }

                    }


                    /*if (DateTime.TryParseExact(dateStr, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out dtStr))
                    {
                        dt.Rows[i]["Date"] = dtStr.ToString("DD-MMM-yyyy");
                    }
                    */
                }



                //dataAdapter.Dispose();
                //conn.Dispose();
                //cmd.Dispose();
                //dataAdapter.Dispose();
                //conn.Dispose();
                conn.Close();

                //dataAdapter.Dispose();
            }


            //Bind Sheet Data to GridView
            return dt;
        }
        private DataTable FillGridFromGlockFile(string FilePath, string ext, string HDR)
        {
            bool isCSV = false;
            string connectionString = "";
            if (ext.ToLower() == ".xls")
            {   //For Excel 97-03
                connectionString = "Provider=Microsoft.ACE.OLEDB.12.0; Data Source ='{0}';Extended Properties = 'Excel 8.0;HDR={1}'";
            }
            else if (ext.ToLower() == ".xlsx")
            {    //For Excel 07 and greater
                connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source = '{0}'; Extended Properties = 'Excel 8.0;HDR={1}'";
            }
            else if (ext.ToLower() == ".csv")
            {
                //For CSV files
                isCSV = true;
                connectionString = "Provider=Microsoft.ACE.OLEDB.12.0; Data Source='{0}';Extended Properties='TEXT;HDR={1};FMT=Delimited;ImportMixedTypes=Text;IMEX=0;TextQualified=true'";
            }

            //Fetch 1st Sheet Name

            //OleDbCommand cmd = new OleDbCommand();
            //OleDbDataAdapter dataAdapter = new OleDbDataAdapter();
            DataTable dt = new DataTable();
            if (!isCSV)
            {

                connectionString = String.Format(connectionString, FilePath, HDR);
                OleDbConnection conn = new OleDbConnection(connectionString);

                //cmd = conn.CreateCommand();
                //cmd.Connection = conn;

                conn.Open();
                DataTable dtSchema;
                dtSchema = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                string ExcelSheetName = dtSchema.Rows[0]["TABLE_NAME"].ToString();

                OleDbDataAdapter dataAdapter = new OleDbDataAdapter("SELECT * From [" + ExcelSheetName + "]", conn);
                dataAdapter.Fill(dt);
                dataAdapter.Dispose();

                conn.Close();
                //conn.Dispose();
            }
            else
            {
                //connectionString = String.Format(connectionString, Path.GetDirectoryName(FilePath), HDR);
                connectionString = String.Format(connectionString, Path.GetDirectoryName(FilePath), HDR);
                OleDbConnection conn = new OleDbConnection(connectionString);

                conn.Open();
                OleDbDataAdapter dataAdapter = new OleDbDataAdapter("SELECT * From [" + Path.GetFileName(FilePath) + "] ", conn);

                var cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT * From [" + Path.GetFileName(FilePath) + "] ";
                DbDataReader reader = cmd.ExecuteReader();
                {
                    //var model = Serialize(reader);
                    dt.Load(reader);
                }
                DataTable dt2 = null;
                using (GenericParserAdapter gp = new GenericParserAdapter(FilePath))
                {
                    //gp.FirstRowSetsExpectedColumnCount = true;
                    gp.FirstRowHasHeader = true;
                    dt2 = gp.GetDataTable();
                }


                conn.Close();
            }

            return dt;
        }

        public IEnumerable<Dictionary<string, object>> Serialize(DbDataReader reader)
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
                                                        DbDataReader reader)
        {
            var result = new Dictionary<string, object>();
            foreach (var col in cols)
                result.Add(col, reader[col]);
            return result;
        }
        public static Dictionary<string, object> addArray(string tkey,object value)
        {
            Dictionary<string, object>  row1 = new Dictionary<string, object>();
            row1.Add(tkey, value);
            return row1;

        }
        public static string GetColumnName(string cellReference)
        {
            // Create a regular expression to match the column name portion of the cell name.
            Regex regex = new Regex("[A-Za-z]+");
            Match match = regex.Match(cellReference);
            return match.Value;
        }
        /// <summary>
        /// Given just the column name (no row index), it will return the zero based column index.
        /// Note: This method will only handle columns with a length of up to two (ie. A to Z and AA to ZZ). 
        /// A length of three can be implemented when needed.
        /// </summary>
        /// <param name="columnName">Column Name (ie. A or AB)</param>
        /// <returns>Zero based index if the conversion was successful; otherwise null</returns>
        public static int? GetColumnIndexFromName(string columnName)
        {

            //return columnIndex;
            string name = columnName;
            int number = 0;
            int pow = 1;
            for (int i = name.Length - 1; i >= 0; i--)
            {
                number += (name[i] - 'A' + 1) * pow;
                pow *= 26;
            }
            return number;
        }
 
        // POST: COPR16_FILE_IMPORT/getUniqID
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<JsonResult> getUniqID(string id)
        {
            //var row = await db.Database.SqlQuery<UniqCheck>("dbo.sp_getUniqID @UNIQ_ID = @cUNIQ_ID", new object[] { param }).ToListAsync();
            var row = await db.Database.SqlQuery<UniqCheck>("exec dbo.sp_getUniqID @p0",id).ToListAsync();
            //var row = db.Database.ExecuteSqlCommand("dbo.sp_getUniqID @UNIQ_ID", parameter);
            return Json(row, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<JsonResult> ConfirmUploadModel(List<newModelData> ModelsData)
        //public JsonResult ConfirmUpload(FileUploadHeader fsHeader)
        {
            var rowData = new JsonResult();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DatabaseServer"].ToString()))
            {
                if (con.State != ConnectionState.Open)
                {
                    await con.OpenAsync();
                }

                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = "exec COPR16_CHECKING_MODEL_STG_TABLES;";
                    await cmd.ExecuteNonQueryAsync();
                }
                foreach (var item in ModelsData)
                {
                    using (var cmd = con.CreateCommand())
                    {
                        cmd.CommandText = "exec COPR16_INSERT_STG_MODEL_IMPORT @MODEL,@LINE,@POSI,@SB,@BCK1,@BCK2";
                        cmd.Parameters.Add(new SqlParameter("@MODEL", item.ModelName == null ? "" : item.ModelName));
                        cmd.Parameters.Add(new SqlParameter("@LINE", item.LineName == null ? "" : item.LineName));
                        cmd.Parameters.Add(new SqlParameter("@POSI", item.PositionName == null ? "" : item.PositionName));
                        cmd.Parameters.Add(new SqlParameter("@SB", item.SBPart == null ? "" : item.SBPart));
                        cmd.Parameters.Add(new SqlParameter("@BCK1", item.Bck1Part == null ? "" : item.Bck1Part));
                        cmd.Parameters.Add(new SqlParameter("@BCK2", item.Bck2Part == null ? "" : item.Bck2Part));
                        await cmd.ExecuteNonQueryAsync();

                    }
                }

                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = "exec COPR16_UPDATE_MODELS_IMP;";
                    await cmd.ExecuteNonQueryAsync();
                }
                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = "exec COPR16_UPDATE_LINE_IMP;";
                    await cmd.ExecuteNonQueryAsync();
                }
                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = "exec COPR16_UPDATE_MATCHING_PARTS_IMP;";
                    await cmd.ExecuteNonQueryAsync();
                }
                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = "EXECUTE [dbo].[COPR16_SP_UPDATE_MODELMON_STEP02] ;";
                    await cmd.ExecuteNonQueryAsync();
                }
                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = "EXECUTE [dbo].[COPR16_SP_UPDATE_COND1]  ;";
                    await cmd.ExecuteNonQueryAsync();
                }
                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = "EXECUTE [dbo].[COPR16_SP_UPDATE_COND2] ;";
                    await cmd.ExecuteNonQueryAsync();
                }
                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = "EXECUTE [dbo].[COPR16_UPDATE_PART_VOL_TRACK_STEP04]  ;";
                    await cmd.ExecuteNonQueryAsync();
                }

                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = "EXECUTE [dbo].[COPR16_UPADTE_MODEL_VOL_TRACK_STEP03]   ;";
                    await cmd.ExecuteNonQueryAsync();
                }
                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = "EXECUTE [dbo].[COPR16_UPDATE_ALL_TRIGGER_MODEL_STEP05] 1, 4500;";
                    await cmd.ExecuteNonQueryAsync();
                }
                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = "EXECUTE [dbo].[COPR16_UPDATE_ALL_TRIGGER_MODEL_STEP05] 1, 9500;";
                    await cmd.ExecuteNonQueryAsync();
                }
            }
            /*
            var rtn = await db.Database.ExecuteSqlCommandAsync(sqlCOMMAND, parameters.ToArray());
            fsHeader.FSIM_UNIQ_ID = p6.Value.ToString();
            List<SqlParameter> sqlParams = new List<SqlParameter>();
            string sqlCOMMAND_ITEMS = "exec [dbo].[sp_insert_import_contents] @FSIM_UNIQ_ID,@ITEM_ID,@BUSI_DATE,@QTY,@CRE_BY ";
            foreach (var item in fgVolumes)
            {
                sqlParams.Clear();
                item.FSIM_UNIQ_ID = fsHeader.FSIM_UNIQ_ID;
                //item.BUSI_DATE = fsHeader.FSIM_SEQID;
                sqlParams.Add(new SqlParameter("@FSIM_UNIQ_ID", item.FSIM_UNIQ_ID));
                sqlParams.Add(new SqlParameter("@ITEM_ID", item.ITEM_ID));
                sqlParams.Add(new SqlParameter("@BUSI_DATE", item.BUSI_DATE));
                sqlParams.Add(new SqlParameter("@QTY", item.QTY ?? "0"));
                sqlParams.Add(new SqlParameter("@CRE_BY", fsHeader.CRE_BY));
                await db.Database.ExecuteSqlCommandAsync(sqlCOMMAND_ITEMS, sqlParams.ToArray());
            }
            fgVolumes[0].FSIM_UNIQ_ID = fsHeader.FSIM_UNIQ_ID;

            List<SqlParameter> PostsqlParams = new List<SqlParameter>();
            string sqlCOMMAND_POST = "exec [dbo].[sp_update_acc_volume] @FSIM_UNIQ_ID ";
            PostsqlParams.Add(new SqlParameter("@FSIM_UNIQ_ID", fsHeader.FSIM_UNIQ_ID));
            await db.Database.ExecuteSqlCommandAsync(sqlCOMMAND_POST, PostsqlParams.ToArray());

            List<SqlParameter> UpdateTrackingWithOutParams = new List<SqlParameter>();
            string sqlCOMMAND_UpdateTrackingWithOut = "EXEC SP_UPDATE_MODEL_TRACKING @VALUE_QTY";
            UpdateTrackingWithOutParams.Add(new SqlParameter("@VALUE_QTY", 4500));
            await db.Database.ExecuteSqlCommandAsync(sqlCOMMAND_UpdateTrackingWithOut, UpdateTrackingWithOutParams.ToArray());

            List<SqlParameter> UpdateTrackingWithConParams = new List<SqlParameter>();
            string sqlCOMMAND_UpdateTrackingWithCon = "EXEC SP_UPDATE_MODEL_TRACKING @VALUE_QTY";
            UpdateTrackingWithConParams.Add(new SqlParameter("@VALUE_QTY", 9500));
            await db.Database.ExecuteSqlCommandAsync(sqlCOMMAND_UpdateTrackingWithCon, UpdateTrackingWithConParams.ToArray());
            */
            rowData.Data = ModelsData;
            return Json(rowData, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<JsonResult> ConfirmUpload(FileUploadHeader fsHeader,List<fgVolume> fgVolumes)
        //public JsonResult ConfirmUpload(FileUploadHeader fsHeader)
        {
            List< SqlParameter> parameters = new List<SqlParameter>();

            SqlParameter p1 = new SqlParameter("@FSIM_NAME", fsHeader.FSIM_NAME);
            p1.SqlDbType = SqlDbType.VarChar;
            p1.Size = 1024;
            
            SqlParameter p2 = new SqlParameter("@FSIM_EXT", fsHeader.FSIM_EXT);
            p2.SqlDbType = SqlDbType.VarChar;
            p2.Size = 25;

            SqlParameter p3 = new SqlParameter("@FSIM_PATH", fsHeader.FSIM_PATH == null?"": fsHeader.FSIM_PATH);
            p3.SqlDbType = SqlDbType.VarChar;
            p3.Size = 1024;

            SqlParameter p4 = new SqlParameter("@FSIM_SIZE", fsHeader.FSIM_SIZE);
            p4.SqlDbType = SqlDbType.VarChar;
            p4.Size = 25;

            SqlParameter p5 = new SqlParameter("@FSIM_FORMAT", fsHeader.FSIM_FORMAT);
            p5.SqlDbType = SqlDbType.VarChar;
            p5.Size = 25;

            SqlParameter p6 = new SqlParameter("@FSIM_UNIQ_ID", SqlDbType.VarChar);
            p6.Direction = ParameterDirection.Output;
            p6.SqlDbType = SqlDbType.VarChar;
            p6.Size = 1024;

            SqlParameter p7 = new SqlParameter("@CRE_BY", fsHeader.CRE_BY);
            p7.SqlDbType = SqlDbType.VarChar;
            p7.Size = 25;

            SqlParameter p8 = new SqlParameter("@FSIM_SEQDT", fsHeader.FSIM_SEQID);
            p8.SqlDbType = SqlDbType.VarChar;
            p8.Size = 25;

            var rowData = new JsonResult();
            parameters.Add(p1);
            parameters.Add(p2);
            parameters.Add(p3);
            parameters.Add(p4);
            parameters.Add(p5);
            parameters.Add(p6);
            parameters.Add(p7);
            parameters.Add(p8);
            //string sqlCOMMAND = "EXEC [dbo].[sp_insert_excel_volume] @FSIM_NAME, @FSIM_EXT, @FSIM_PATH, @FSIM_SIZE, @FSIM_FORMAT, @FSIM_UNIQ_ID OUTPUT, @CRE_BY, @FSIM_SEQDT ";
            string sqlCOMMAND = "exec [dbo].[sp_insert_excel_volume] @FSIM_NAME, @FSIM_EXT, @FSIM_PATH, @FSIM_SIZE, @FSIM_FORMAT, @FSIM_UNIQ_ID OUTPUT, @CRE_BY, @FSIM_SEQDT ";
            //var rtn = await db.Database.ExecuteSqlCommandAsync(sqlCOMMAND, p1,p2,p3,p4,p5,p6,p7,p8);

            //var rtn = db.Database.ExecuteSqlCommand(sqlCOMMAND, p1, p2, p3, p4, p5, p6, p7, p8);
            //var rtn = db.Database.ExecuteSqlCommand(sqlCOMMAND, parameters.ToArray());
            var rtn = await db.Database.ExecuteSqlCommandAsync(sqlCOMMAND, parameters.ToArray());
            fsHeader.FSIM_UNIQ_ID = p6.Value.ToString();
            List<SqlParameter> sqlParams = new List<SqlParameter>();
            string sqlCOMMAND_ITEMS = "exec [dbo].[sp_insert_import_contents] @FSIM_UNIQ_ID,@ITEM_ID,@BUSI_DATE,@QTY,@CRE_BY ";
            foreach (var item in fgVolumes)
            {
                sqlParams.Clear();
                item.FSIM_UNIQ_ID = fsHeader.FSIM_UNIQ_ID;
                //item.BUSI_DATE = fsHeader.FSIM_SEQID;
                sqlParams.Add(new SqlParameter("@FSIM_UNIQ_ID", item.FSIM_UNIQ_ID));
                sqlParams.Add(new SqlParameter("@ITEM_ID", item.ITEM_ID));
                sqlParams.Add(new SqlParameter("@BUSI_DATE", item.BUSI_DATE));
                sqlParams.Add(new SqlParameter("@QTY", item.QTY ?? "0"));
                sqlParams.Add(new SqlParameter("@CRE_BY", fsHeader.CRE_BY));
                await db.Database.ExecuteSqlCommandAsync(sqlCOMMAND_ITEMS, sqlParams.ToArray());
            }
            fgVolumes[0].FSIM_UNIQ_ID = fsHeader.FSIM_UNIQ_ID;

            List<SqlParameter> PostsqlParams = new List<SqlParameter>();
            string sqlCOMMAND_POST = "exec [dbo].[sp_update_acc_volume] @FSIM_UNIQ_ID ";
            PostsqlParams.Add(new SqlParameter("@FSIM_UNIQ_ID", fsHeader.FSIM_UNIQ_ID));
            await db.Database.ExecuteSqlCommandAsync(sqlCOMMAND_POST, PostsqlParams.ToArray());

            List<SqlParameter> UpdateTrackingWithOutParams = new List<SqlParameter>();
            string sqlCOMMAND_UpdateTrackingWithOut = "EXEC SP_UPDATE_MODEL_TRACKING @VALUE_QTY";
            UpdateTrackingWithOutParams.Add(new SqlParameter("@VALUE_QTY", 4500));
            await db.Database.ExecuteSqlCommandAsync(sqlCOMMAND_UpdateTrackingWithOut, UpdateTrackingWithOutParams.ToArray());

            List<SqlParameter> UpdateTrackingWithConParams = new List<SqlParameter>();
            string sqlCOMMAND_UpdateTrackingWithCon = "EXEC SP_UPDATE_MODEL_TRACKING @VALUE_QTY";
            UpdateTrackingWithConParams.Add(new SqlParameter("@VALUE_QTY", 9500));
            await db.Database.ExecuteSqlCommandAsync(sqlCOMMAND_UpdateTrackingWithCon, UpdateTrackingWithConParams.ToArray());

            rowData.Data = fgVolumes;
            return Json(rowData, JsonRequestBehavior.AllowGet);
        }

        // GET: COPR16_FILE_IMPORT
        public async Task<ActionResult> Index()
        {
            return View(await db.COPR16_FILE_IMPORT.ToListAsync());
        }

        // GET: COPR16_FILE_IMPORT/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            COPR16_FILE_IMPORT cOPR16_FILE_IMPORT = await db.COPR16_FILE_IMPORT.FindAsync(id);
            if (cOPR16_FILE_IMPORT == null)
            {
                return HttpNotFound();
            }
            return View(cOPR16_FILE_IMPORT);
        }

        // GET: COPR16_FILE_IMPORT/Create
        public ActionResult Create()
        {
            FileImportModel model = new FileImportModel(db);
            return View(model);
        }
        public ActionResult importModel()
        {
            ImportModels model = new ImportModels(db);
            return View(model);
        }
        public ActionResult gLockUpload()
        {
            FileImportGlock model = new FileImportGlock(db);
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult GlockUploading(HttpPostedFileBase FSIM_NAME, string hidden_cOPR16_FILE_IMPORT_FSIM_SEQDT, string UID)
        {
            var rowData = new JsonResult();
            DataTable dt = new DataTable();

            int rowIndex = 0;
            List<Dictionary<string, object>> ColumnOfRows = new List<Dictionary<string, object>>();
            List<string> colNameArr = new List<string>();
            List<string> RowcolNameArr = new List<string>();
            if (FSIM_NAME.ContentLength > 0)
            {
                string rootPath = AppPropModel.rootPath;
                string subPath = UID + "/" + hidden_cOPR16_FILE_IMPORT_FSIM_SEQDT;
                bool exists = System.IO.Directory.Exists(Server.MapPath(rootPath + "/" + subPath));
                if (!exists)
                    System.IO.Directory.CreateDirectory(Server.MapPath(rootPath + "/" + subPath));

                var fileName = Path.GetFileName(FSIM_NAME.FileName);

                var path = Path.Combine(Server.MapPath(rootPath + "/" + subPath), fileName);
                /*
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                */
                FSIM_NAME.SaveAs(path);
                string Ext = Path.GetExtension(path);
                dt = FillGridFromGlockFile(path, Ext, "YES");
            }

            List<Dictionary<string, object>> rows1 = new List<Dictionary<string, object>>();
            Dictionary<string, object> row1;
            foreach (DataRow dr in dt.Rows)
            {
                row1 = new Dictionary<string, object>();
                foreach (DataColumn col in dt.Columns)
                {
                    row1.Add(col.ColumnName, dr[col]);
                }
                rows1.Add(row1);
            }
            rowData.Data = rows1;

            //rowData.Data =  ColumnOfRows;
            //rowData.Data = dt.Columns.Count;

            //var row = await db.Database.SqlQuery<UniqCheck>("dbo.sp_getUniqID @UNIQ_ID = @cUNIQ_ID", new object[] { param }).ToListAsync();
            //var row = await db.Database.SqlQuery<UniqCheck>("exec dbo.sp_getUniqID @p0", id).ToListAsync();
            //var row = db.Database.ExecuteSqlCommand("dbo.sp_getUniqID @UNIQ_ID", parameter);

            //row.Data = new object[] { 1, 2,5 };
            JsonResult json = Json(rowData, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;
            //return Json(rowData, JsonRequestBehavior.AllowGet);
            return json;
        }
        // POST: COPR16_FILE_IMPORT/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.

        public string StripSlashes(string InputTxt)
        {
            // List of characters handled:
            // \000 null
            // \010 backspace
            // \011 horizontal tab
            // \012 new line
            // \015 carriage return
            // \032 substitute
            // \042 double quote
            // \047 single quote
            // \134 backslash
            // \140 grave accent

            string Result = InputTxt;

            try
            {
                Result = System.Text.RegularExpressions.Regex.Replace(InputTxt, @"(\\)([\000\010\011\012\015\032\042\047\134\140])", "$2");
            }
            catch (Exception Ex)
            {
                // handle any exception here
                Console.WriteLine(Ex.Message);
            }

            return Result;
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> gLockConfirmUpload(RST_HEADER rstHeader, List<RST_DETIAL> rsDetails)
        //public JsonResult ConfirmUpload(FileUploadHeader fsHeader)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();

            SqlParameter p1 = new SqlParameter("@RST_ID", rstHeader.RST_ID);
            p1.SqlDbType = SqlDbType.VarChar;
            p1.Size = 25;

            SqlParameter p2 = new SqlParameter("@COPR_ID", StripSlashes(rstHeader.COPR_ID));
            p2.SqlDbType = SqlDbType.VarChar;
            p2.Size = 25;

            SqlParameter p3 = new SqlParameter("@RST_FILENAME", StripSlashes(rstHeader.RST_FILENAME));
            p3.SqlDbType = SqlDbType.VarChar;
            p3.Size = 255;

            SqlParameter p4 = new SqlParameter("@RST_FILE_DATA", StripSlashes(rstHeader.RST_FILE_DATA));
            p4.SqlDbType = SqlDbType.Text;

            SqlParameter p5 = new SqlParameter("@CRE_BY", StripSlashes(rstHeader.CRE_BY));
            p5.SqlDbType = SqlDbType.VarChar;
            p5.Size = 25;

            var rowData = new JsonResult();
            parameters.Add(p1);
            parameters.Add(p2);
            parameters.Add(p3);
            parameters.Add(p4);
            parameters.Add(p5);

            string sqlCOMMAND = "exec [dbo].[sp_insert_rst_header] @RST_ID, @COPR_ID, @RST_FILENAME, @RST_FILE_DATA, @CRE_BY ";

            var rtn = await db.Database.ExecuteSqlCommandAsync(sqlCOMMAND, parameters.ToArray());
            
            List<SqlParameter> sqlParams = new List<SqlParameter>();
            string sqlCOMMAND_ITEMS = "exec [dbo].[sp_insert_rst_details] @RST_ID,@RST_KEY,@RST_VALUE,@RTDT_ID ";
            foreach (var item in rsDetails)
            {
                sqlParams.Clear();
                item.RST_ID = rstHeader.RST_ID;
                //item.BUSI_DATE = fsHeader.FSIM_SEQID;
                sqlParams.Add(new SqlParameter("@RST_ID", item.RST_ID));
                sqlParams.Add(new SqlParameter("@RST_KEY", item.RST_KEY));
                sqlParams.Add(new SqlParameter("@RST_VALUE", item.RST_VALUE));
                sqlParams.Add(new SqlParameter("@RTDT_ID", item.RTDT_ID));
                await db.Database.ExecuteSqlCommandAsync(sqlCOMMAND_ITEMS, sqlParams.ToArray());
            }


            return Json(rowData, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "FSIM_DT,FSIM_NAME,FSIM_EXT,FSIM_PATH,FSIM_SIZE,FSIM_FORMAT,FSIM_UNIQ_ID,CRE_BY,FSIM_SEQDT")] COPR16_FILE_IMPORT cOPR16_FILE_IMPORT)
        {
            if (ModelState.IsValid)
            {
                db.COPR16_FILE_IMPORT.Add(cOPR16_FILE_IMPORT);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(cOPR16_FILE_IMPORT);
        }

        // GET: COPR16_FILE_IMPORT/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            COPR16_FILE_IMPORT cOPR16_FILE_IMPORT = await db.COPR16_FILE_IMPORT.FindAsync(id);
            if (cOPR16_FILE_IMPORT == null)
            {
                return HttpNotFound();
            }
            return View(cOPR16_FILE_IMPORT);
        }

        // POST: COPR16_FILE_IMPORT/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "FSIM_DT,FSIM_NAME,FSIM_EXT,FSIM_PATH,FSIM_SIZE,FSIM_FORMAT,FSIM_UNIQ_ID,CRE_BY,FSIM_SEQDT")] COPR16_FILE_IMPORT cOPR16_FILE_IMPORT)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cOPR16_FILE_IMPORT).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(cOPR16_FILE_IMPORT);
        }

        // GET: COPR16_FILE_IMPORT/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            COPR16_FILE_IMPORT cOPR16_FILE_IMPORT = await db.COPR16_FILE_IMPORT.FindAsync(id);
            if (cOPR16_FILE_IMPORT == null)
            {
                return HttpNotFound();
            }
            return View(cOPR16_FILE_IMPORT);
        }

        // POST: COPR16_FILE_IMPORT/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            COPR16_FILE_IMPORT cOPR16_FILE_IMPORT = await db.COPR16_FILE_IMPORT.FindAsync(id);
            db.COPR16_FILE_IMPORT.Remove(cOPR16_FILE_IMPORT);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        void Columns_CollectionChanged(object sender, CollectionChangeEventArgs e)
        {
            DataColumn dc = (e.Element as DataColumn);
            if (dc != null && dc.AutoIncrement)
            {
                long i = dc.AutoIncrementSeed;
                foreach (DataRow drow in dc.Table.Rows)
                {
                    drow[dc] = i;
                    i++;
                }
            }
        }
    }
}
