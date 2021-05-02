using CM_APPLICATIONS.Models;
using ExcelLibs_Test.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CM_APPLICATIONS.Controllers
{
    public class COPR16_TS_TOOLSController : Controller
    {
        private COPR16Entities db = new COPR16Entities();
        // GET: COPR16_TS_TOOLS
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> getSPCList(string date_from, string date_to, string partno, string lotno, string filecode) 
        {
            JsonResult jsonReturn = new JsonResult();
            jsonReturn.MaxJsonLength = Int32.MaxValue;
            string SQLCMD2 = "exec dbo.COPR16_TS_GET_SPC_LIST @PARTNO, @DATE_FROM, @DATE_TO, @LOT, @FILE ";

            List<SqlParameter> param2 = new List<SqlParameter>();
            //param2.Add(new SqlParameter("@MODEL_ID", model_id == null ? "" : model_id));
            //await db.Database.ExecuteSqlCommandAsync(SQLCMD2, param2.ToArray());

            using (var cmd = db.Database.Connection.CreateCommand())
            {
                await db.Database.Connection.OpenAsync();
                cmd.CommandText = SQLCMD2;
                cmd.Parameters.Add(new SqlParameter("@PARTNO", partno??""));
                cmd.Parameters.Add(new SqlParameter("@DATE_FROM", date_from ??""));
                cmd.Parameters.Add(new SqlParameter("@DATE_TO", date_to??""));
                cmd.Parameters.Add(new SqlParameter("@LOT", lotno ?? ""));
                cmd.Parameters.Add(new SqlParameter("@FILE ", filecode??""));
                DbDataReader reader = await cmd.ExecuteReaderAsync();
                {
                    var model = Utils.Serialize((SqlDataReader)reader);
                    jsonReturn.Data = model;
                }
            }
            return Json(jsonReturn, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> getTSChartDataList( string filecode)
        {
            JsonResult jsonReturn = new JsonResult();
            jsonReturn.MaxJsonLength = Int32.MaxValue;
            string SQLCMD2 = "EXECUTE [dbo].[COPR16_TS_GET_DATAFROM_FILE] @FILECODE ";

            List<SqlParameter> param2 = new List<SqlParameter>();
            //param2.Add(new SqlParameter("@MODEL_ID", model_id == null ? "" : model_id));
            //await db.Database.ExecuteSqlCommandAsync(SQLCMD2, param2.ToArray());

            using (var cmd = db.Database.Connection.CreateCommand())
            {
                await db.Database.Connection.OpenAsync();
                cmd.CommandText = SQLCMD2;
                cmd.Parameters.Add(new SqlParameter("@FILECODE ", filecode ?? ""));
                DbDataReader reader = await cmd.ExecuteReaderAsync();
                {
                    var model = Utils.Serialize((SqlDataReader)reader);
                    jsonReturn.Data = model;
                }
            }
            return Json(jsonReturn, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<FileResult> getTSChartDataExcel(string filecode)
        {
            JsonResult jsonReturn = new JsonResult();
            string templateName = "TemplateChart.xlsx";
            string temppath = Server.MapPath("~/RPTTemplate");

            string filePath = Path.Combine(temppath, templateName);
            bmsExcelTools xlsx = new bmsExcelTools();

            xlsx.openExcelFile(temppath, templateName);

            jsonReturn.MaxJsonLength = Int32.MaxValue;
            string SQLCMD2 = "EXECUTE [dbo].[COPR16_TS_GET_DATAFROM_FILE] @FILECODE ";

            List<SqlParameter> param2 = new List<SqlParameter>();
            //param2.Add(new SqlParameter("@MODEL_ID", model_id == null ? "" : model_id));
            //await db.Database.ExecuteSqlCommandAsync(SQLCMD2, param2.ToArray());

            using (var cmd = db.Database.Connection.CreateCommand())
            {
                await db.Database.Connection.OpenAsync();
                cmd.CommandText = SQLCMD2;
                cmd.Parameters.Add(new SqlParameter("@FILECODE ", filecode ?? ""));
                //DataTable dt1 = await cmd.ExecuteReaderAsync();

                int _rowStart = 2;
                
                DbDataReader reader = await cmd.ExecuteReaderAsync();
                while(await reader.ReadAsync())
                {
                    //var model = Utils.Serialize((SqlDataReader)reader);
                    //jsonReturn.Data = model;
                    double xVal = 0.00;
                    double yVal = 0.00;
                    double maxY = 0.00;
                    Double.TryParse(reader[1].ToString(),out xVal);
                    Double.TryParse(reader[2].ToString(), out yVal);
                    Double.TryParse(reader[3].ToString(), out maxY);
                    //string formularStr = $"=IF( B{_rowStart}=MAX(B2:B1024),B{_rowStart},#N/A)";
                    //string formularStr = $"=MAX(B2:B1024)";
                    xlsx.setCellValue(_rowStart, 1, xVal);
                    xlsx.setCellValue(_rowStart, 2, yVal);

                    string formularStr = "=NA()";
                    if (maxY == yVal)
                    {

                        xlsx.setCellValue(_rowStart++, 3, maxY);
                    }
                    else
                    {
                        xlsx.setCellFormular(_rowStart++, 3, formularStr);
                    }
                }

            }
            xlsx.updateformular();
            string FileName = $"{filecode}.xlsx";
            xlsx.renameSheet("", filecode);
            xlsx.getLineChart();
            xlsx.save();
            MemoryStream fsStream = xlsx.saveASMemStream();
            byte[] bytes = fsStream.ToArray();
            return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", FileName);
        }
    }
}