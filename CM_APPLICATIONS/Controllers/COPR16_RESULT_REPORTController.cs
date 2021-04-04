using CM_APPLICATIONS.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CM_APPLICATIONS.Controllers
{
    public class COPR16_RESULT_REPORTController : Controller
    {
        private SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DatabaseServer"].ToString());
        // GET: ResultReport
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ViewReport(int Year,int Month,string Model,string MonthName)
        {
            ViewBag.ModelName = Model;
            ViewBag.YearName = Year;
            //ViewBag.MonthName = MonthName;
            var objectModel = GetMemberList();
            viewReport lmodel = new viewReport();
            List<rptReports> lReport = new List<rptReports>();
            string SQL_CMD = "EXEC dbo.COPR16_CUST_REPORT_GETREPORTS @MODEL,@YEAR,@MONTH";

            if (con.State != System.Data.ConnectionState.Open)
            {
                await con.OpenAsync();
            }
            using (var cmd = con.CreateCommand())
            {
                cmd.CommandText = SQL_CMD;
                cmd.Parameters.Add(new SqlParameter("@MODEL", Model ?? ""));
                cmd.Parameters.Add(new SqlParameter("@YEAR", Year));
                cmd.Parameters.Add(new SqlParameter("@MONTH", Month));
                //cmd.Parameters.Add(new SqlParameter("@RPT_TYPE", reportType));

                using (DbDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    var model = Utils.Serialize((SqlDataReader)reader);
                    foreach (var row in model)
                    {
                        var obj = new rptReports();
                        obj.YearId = (int) row["RPT_YEAR"];
                        obj.MonthId = (int) row["RPT_MONTH"];
                        obj.ReportBody = row["RPT_REPORT_BODY"].ToString();
                        obj.Model = row["MODEL_ID"].ToString();
                        obj.ReportType = (int)row["RPT_REPORT_ID"];
                        lReport.Add(obj);
                    }


                    reader.Close();
                }

            }
            con.Close();


            lmodel.reportData = lReport;
            foreach (var row in lReport)
            {
                if(row.ReportType == 0)
                {
                    objectModel[0].TabName = "rpt0";
                    objectModel[0].strBody = row.ReportBody;
                }else if (row.ReportType == 1)
                {
                    objectModel[1].TabName = "rpt1";
                    objectModel[1].strBody = row.ReportBody;
                }
                else if (row.ReportType == 2)
                {
                    objectModel[2].TabName = "rpt2";
                    objectModel[2].strBody = row.ReportBody;
                }
            }
            lmodel.reportTabs = objectModel;
            return View(lmodel);
        }
        private List<ReportTab> GetMemberList()
        {
            UrlHelper u = new UrlHelper(this.ControllerContext.RequestContext);
            List<ReportTab> members = new List<ReportTab>();
            members.Add(new ReportTab { TabName="rpt0", Name = "Volume Tracking Report", strUrl = u.Action("ViewReport", "COPR16_RESULT_REPORT", new RouteValueDictionary(new { reportName = "rpt1",yearSel="2021",monthSet="Jan",modelSel="2GM" })) });
            members.Add(new ReportTab { TabName="rpt1", Name = "Final Reports", strUrl = u.Action("ViewReport", "COPR16_RESULT_REPORT", new RouteValueDictionary(new { reportName = "rpt2", yearSel = "2021", monthSet = "Jan", modelSel = "2GM" })) });
            members.Add(new ReportTab { TabName = "rpt2", Name = "Tensile Report", strUrl = u.Action("ViewReport", "COPR16_RESULT_REPORT", new RouteValueDictionary(new { reportName = "rpt3", yearSel = "2021", monthSet = "Jan", modelSel = "2GM" })) });
            return members;
        }
        public async Task<ActionResult> ViewReportModel()
        {
            List<rptModels> lmodel = new List<rptModels>();
            string SQL_CMD = "EXEC dbo.COPR16_CUSTRPT_LISTMODEL";

            if (con.State != System.Data.ConnectionState.Open)
            {
                await con.OpenAsync();
            }
            using (var cmd = con.CreateCommand())
            {
                cmd.CommandText = SQL_CMD;
                using (DbDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    var model =  Utils.Serialize((SqlDataReader)reader);
                    foreach (var row in model)
                    {
                        var obj = new rptModels();
                        obj.ModelId = row["MODEL_ID"].ToString();
                        obj.ModelName = row["MODEL_NAME"].ToString();
                        lmodel.Add(obj);
                    }
                    reader.Close();
                }

            }
            con.Close();
         
            return View(lmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task< ActionResult> ViewReportYears(string ModelStr)
        {
            ViewBag.ModelName = ModelStr;
            List<rptYear> lmodel = new List<rptYear>();
            string SQL_CMD = "EXEC dbo.COPR16_CUST_REPORT_GETYEARS @MODEL";

            if (con.State != System.Data.ConnectionState.Open)
            {
                await con.OpenAsync();
            }
            using (var cmd = con.CreateCommand())
            {
                cmd.CommandText = SQL_CMD;
                cmd.Parameters.Add(new SqlParameter("@MODEL", ModelStr ?? ""));
                using (DbDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    var model = Utils.Serialize((SqlDataReader)reader);
                    foreach (var row in model)
                    {
                        var obj = new rptYear();
                        obj.YearId = row["RPT_YEAR"].ToString();
                        obj.YearName = row["RPT_YEAR"].ToString();
                        lmodel.Add(obj);
                    }


                    reader.Close();
                }

            }
            con.Close();
            return View(lmodel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ViewReportMonths(string ModelStr,int YearStr)
        {
            List<rptMonths> lmodel = new List<rptMonths>();
            string SQL_CMD = "EXEC dbo.COPR16_CUST_REPORT_GETMONTHS @MODEL,@RPT_YEAR";
            ViewBag.ModelName = ModelStr;
            ViewBag.YearName = YearStr;
            if (con.State != System.Data.ConnectionState.Open)
            {
                await con.OpenAsync();
            }
            using (var cmd = con.CreateCommand())
            {
                cmd.CommandText = SQL_CMD;
                cmd.Parameters.Add(new SqlParameter("@MODEL", ModelStr ?? ""));
                cmd.Parameters.Add(new SqlParameter("@RPT_YEAR", YearStr));
                using (DbDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    var model = Utils.Serialize((SqlDataReader)reader);
                    foreach (var row in model)
                    {
                        var obj = new rptMonths();
                        obj.YearId = row["RPT_YEAR"].ToString();
                        obj.MonthId = row["RPT_MONTH"].ToString();
                        obj.MonthName = row["RPT_MONTH_NAME"].ToString();
                        lmodel.Add(obj);
                    }
                    reader.Close();
                }

            }
            con.Close();
            return View(lmodel);
        }
        [HttpPost, ValidateInput(false)]

        [ValidateAntiForgeryToken]
        public async Task<JsonResult> SaveCustReport(saveDataRpt paramsStr)
        {
            JsonResult rowData = new JsonResult();
            string SQL_CMD = "EXEC dbo.COPR16_CUST_REPORT_INSERTREPORTS @MODEL,@YEAR,@MONTH,@RPT_TYPE,@RPT_BODY,@UID";
            try
            {
                if (con.State != System.Data.ConnectionState.Open)
                {
                    await con.OpenAsync();
                }
                using (var cmd = con.CreateCommand())
                {
       
                    string reportBody = paramsStr.ReportBody.Replace("'", "''");

                    cmd.CommandText = SQL_CMD;
                    cmd.Parameters.Add(new SqlParameter("@MODEL", paramsStr.ModelStr ?? ""));
                    cmd.Parameters.Add(new SqlParameter("@YEAR", paramsStr.YearStr));
                    cmd.Parameters.Add(new SqlParameter("@MONTH", paramsStr.MonthStr));
                    cmd.Parameters.Add(new SqlParameter("@RPT_TYPE", paramsStr.ReportType));
                    cmd.Parameters.Add(new SqlParameter("@RPT_BODY", reportBody ?? ""));
                    cmd.Parameters.Add(new SqlParameter("@UID", paramsStr.UserName ?? ""));
                    await cmd.ExecuteNonQueryAsync();
                }
                con.Close();
                rowData.Data = "OK";
            }catch(Exception ex)
            {
                rowData.Data = "ERROR: " + ex.Message;
            }
            return Json(rowData, JsonRequestBehavior.AllowGet);
        }
    }

}