using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CM_APPLICATIONS;
using CM_APPLICATIONS.Models;
using System.Data.SqlClient;
using System.Data.Entity.Infrastructure;
using System.Data.Common;
using System.Data.Entity.Core.Objects;
using FastMember;
using System.Configuration;

namespace CM_APPLICATIONS.Controllers
{
    public class COPR16_COPRUNNINGController : Controller
    {
        private COPR16Entities db = new COPR16Entities();

        // GET: COPR16_COPRUNNING
        public async Task<ActionResult> Index()
        {
            Models.CopRunningModel model = new CopRunningModel(db);
            DateTime now = DateTime.Now;
            int dayOfWeek = (int)now.DayOfWeek;
            dayOfWeek = dayOfWeek == 0 ? 7 : dayOfWeek;
            DateTime startOfWeek = now.AddDays(0 - (int)now.DayOfWeek);
            DateTime endOfWeek = now.AddDays(6 - (int)now.DayOfWeek);

            
            string startDate = startOfWeek.ToString("yyyy-MM-dd");
            string endDate = endOfWeek.ToString("yyyy-MM-dd");
            string SQLCMD = "SELECT * FROM COPR16_COPRUNNING A WHERE ADATE between '" + startDate + "' AND '"+ endDate + "' AND WRK_ID like 'COP-ATH%' ORDER BY ADATE DESC";
            //model.cOPR16_COPRUNNING_List = await db.COPR16_COPRUNNING.Where(l=>l.ADATE.Value.ToString("yyyy-mm-dd") == Models.AppPropModel.today.ToString("yyyy-mm-dd")).OrderBy(l=>l.ADATE).ToListAsync();
            model.cOPR16_COPRUNNING_List = await db.COPR16_COPRUNNING.SqlQuery(SQLCMD).ToListAsync();

            return View(model);
        }
        // GET: COPR16_COPRUNNING
        public async Task<ActionResult> IndexApproval()
        {
            Models.CopRunningModel model = new CopRunningModel(db);
            DateTime now = DateTime.Now;
            int dayOfWeek = (int)now.DayOfWeek;
            dayOfWeek = dayOfWeek == 0 ? 7 : dayOfWeek;
            DateTime startOfWeek = now.AddDays(0 - (int)now.DayOfWeek);
            DateTime endOfWeek = now.AddDays(6 - (int)now.DayOfWeek);


            string startDate = startOfWeek.ToString("yyyy-MM-dd");
            string endDate = endOfWeek.ToString("yyyy-MM-dd");
            string SQLCMD = "SELECT * FROM COPR16_COPRUNNING A WHERE COP_STATUS = 'COMPLETED' AND ADATE between '" + startDate + "' and '" + endDate + "'  ORDER BY ADATE DESC";
            //model.cOPR16_COPRUNNING_List = await db.COPR16_COPRUNNING.Where(l=>l.ADATE.Value.ToString("yyyy-mm-dd") == Models.AppPropModel.today.ToString("yyyy-mm-dd")).OrderBy(l=>l.ADATE).ToListAsync();
            model.cOPR16_COPRUNNING_List = await db.COPR16_COPRUNNING.SqlQuery(SQLCMD).ToListAsync();

            return View(model);
        }
        public ActionResult TensileUpload()
        {

            return View();
        }
        public async Task<ActionResult> PartListTemplateUpload()
        {
            CopRunningModel model = new CopRunningModel(db);
            model.cOPR16_COPRUNNING = new COPR16_COPRUNNING();
            model.cOPR16_COPRUNNING_DT = new COPR16_COPRUNNING_DT();
            string SQL_CMD = "SELECT distinct SPCC_YEAR FROM dbo.COPR16_SPCC_DIM_YEAR ORDER BY SPCC_YEAR DESC";

            using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DatabaseServer"].ToString()))
            {
                if (con.State != System.Data.ConnectionState.Open)
                {
                    await con.OpenAsync();
                }
                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = SQL_CMD;
                    using (DbDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        var lmodel = Utils.Serialize((SqlDataReader)reader);
                        foreach (var row in lmodel)
                        {
                            string value1 = row["SPCC_YEAR"].ToString();
                            model.SPC_YEAR.Add(new SelectListItem { Text = value1, Value = value1 });
                        }
                        reader.Close();
                    }

                }

                using (var cmd2 = con.CreateCommand())
                {
                    cmd2.CommandText = "SELECT distinct SPCC_MODEL FROM dbo.COPR16_SPCC_DIM_YEAR ORDER BY SPCC_MODEL";
                    using (System.Data.Common.DbDataReader reader2 = await cmd2.ExecuteReaderAsync())
                    {
                        var lmodel = Utils.Serialize((SqlDataReader)reader2);
                        foreach (var row in lmodel)
                        {
                            string value1 = row["SPCC_MODEL"].ToString();
                            model.SPC_MODEL_LIST.Add(new SelectListItem { Text = value1, Value = value1 });
                        }
                        reader2.Close();
                    }
                }

                con.Close();
            }
            return View(model);
        }
        public async Task<ActionResult> IndexAID()
        {
            Models.CopRunningModel model = new CopRunningModel(db);
            DateTime now = DateTime.Now;
            int dayOfWeek = (int)now.DayOfWeek;
            dayOfWeek = dayOfWeek == 0 ? 7 : dayOfWeek;
            DateTime startOfWeek = now.AddDays(0 - (int)now.DayOfWeek);
            DateTime endOfWeek = now.AddDays(6 - (int)now.DayOfWeek);


            string startDate = startOfWeek.ToString("yyyy-MM-dd");
            string endDate = endOfWeek.ToString("yyyy-MM-dd");
            string SQLCMD = "SELECT * FROM COPR16_COPRUNNING A WHERE ADATE between '" + startDate + "' and '" + endDate + "' AND WRK_ID like 'COP-AID%' ORDER BY ADATE DESC";
            //model.cOPR16_COPRUNNING_List = await db.COPR16_COPRUNNING.Where(l=>l.ADATE.Value.ToString("yyyy-mm-dd") == Models.AppPropModel.today.ToString("yyyy-mm-dd")).OrderBy(l=>l.ADATE).ToListAsync();
            model.cOPR16_COPRUNNING_List = await db.COPR16_COPRUNNING.SqlQuery(SQLCMD).ToListAsync();

            return View(model);
        }
        public async Task<ActionResult> IndexNONEC()
        {
            Models.CopRunningModel model = new CopRunningModel(db);
            DateTime now = DateTime.Now;
            int dayOfWeek = (int)now.DayOfWeek;
            dayOfWeek = dayOfWeek == 0 ? 7 : dayOfWeek;
            DateTime startOfWeek = now.AddDays(0 - (int)now.DayOfWeek);
            DateTime endOfWeek = now.AddDays(6 - (int)now.DayOfWeek);


            string startDate = startOfWeek.ToString("yyyy-MM-dd");
            string endDate = endOfWeek.ToString("yyyy-MM-dd");
            string SQLCMD = "SELECT * FROM COPR16_COPRUNNING A WHERE ADATE between '" + startDate + "' and '" + endDate + "' AND WRK_ID like 'NON-EC%' ORDER BY ADATE DESC";
            //model.cOPR16_COPRUNNING_List = await db.COPR16_COPRUNNING.Where(l=>l.ADATE.Value.ToString("yyyy-mm-dd") == Models.AppPropModel.today.ToString("yyyy-mm-dd")).OrderBy(l=>l.ADATE).ToListAsync();
            model.cOPR16_COPRUNNING_List = await db.COPR16_COPRUNNING.SqlQuery(SQLCMD).ToListAsync();

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> FilterIndex(string COPNO, string statusCode, string FROM_DATE, string TO_DATE)
        {
            Models.CopRunningModel model = new CopRunningModel(db);


            DateTime now = DateTime.Now;
            int dayOfWeek = (int)now.DayOfWeek;
            dayOfWeek = dayOfWeek == 0 ? 7 : dayOfWeek;
            DateTime startOfWeek = now.AddDays(0 - (int)now.DayOfWeek);
            DateTime endOfWeek = now.AddDays(6 - (int)now.DayOfWeek);


            string startDate = FROM_DATE;
            string endDate = TO_DATE;
            model.stDate = startDate;
            model.enDate = TO_DATE;
            model.statusCode = statusCode;
            model.COPNO = COPNO;

            string SQLCMD = "SELECT * FROM COPR16_COPRUNNING A WHERE A.ADATE between '" + startDate + "' and DATEADD(day,1,'" + endDate + "') ";
            if(COPNO != null && COPNO != "" && COPNO.Length > 0)
            {
                SQLCMD = SQLCMD + " AND COPR_ID like '%" + COPNO + "%'";
            }
            if (statusCode != null && statusCode != "" && statusCode.Length > 0 && !(statusCode.Contains("ALL")))
            {
                SQLCMD = SQLCMD + " AND COP_STATUS like '%" + statusCode + "%'";
            }

            SQLCMD = SQLCMD + " AND WRK_ID like 'COP-ATH%' ORDER BY ADATE DESC"; 
            //model.cOPR16_COPRUNNING_List = await db.COPR16_COPRUNNING.Where(l=>l.ADATE.Value.ToString("yyyy-mm-dd") == Models.AppPropModel.today.ToString("yyyy-mm-dd")).OrderBy(l=>l.ADATE).ToListAsync();
            model.cOPR16_COPRUNNING_List = await db.COPR16_COPRUNNING.SqlQuery(SQLCMD).ToListAsync();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> FilterIndexApproval(string COPNO, string statusCode, string FROM_DATE, string TO_DATE)
        {
            Models.CopRunningModel model = new CopRunningModel(db);


            DateTime now = DateTime.Now;
            int dayOfWeek = (int)now.DayOfWeek;
            dayOfWeek = dayOfWeek == 0 ? 7 : dayOfWeek;
            DateTime startOfWeek = now.AddDays(0 - (int)now.DayOfWeek);
            DateTime endOfWeek = now.AddDays(6 - (int)now.DayOfWeek);


            string startDate = FROM_DATE;
            string endDate = TO_DATE;
            model.stDate = startDate;
            model.enDate = TO_DATE;
            model.statusCode = statusCode;
            model.statusCode = "COMPLETED";
            model.COPNO = COPNO;

            string SQLCMD = "SELECT * FROM COPR16_COPRUNNING A WHERE A.ADATE between '" + startDate + "' and DATEADD(day,1,'" + endDate + "') ";
            if (COPNO != null && COPNO != "" && COPNO.Length > 0)
            {
                SQLCMD = SQLCMD + " AND COPR_ID like '%" + COPNO + "%'";
            }
            if (model.statusCode != null && model.statusCode != "" && model.statusCode.Length > 0 && !(model.statusCode.Contains("ALL")))
            {
                SQLCMD = SQLCMD + " AND COP_STATUS like '%" + model.statusCode + "%'";
            }

            SQLCMD = SQLCMD + " ORDER BY ADATE DESC";
            //model.cOPR16_COPRUNNING_List = await db.COPR16_COPRUNNING.Where(l=>l.ADATE.Value.ToString("yyyy-mm-dd") == Models.AppPropModel.today.ToString("yyyy-mm-dd")).OrderBy(l=>l.ADATE).ToListAsync();
            model.cOPR16_COPRUNNING_List = await db.COPR16_COPRUNNING.SqlQuery(SQLCMD).ToListAsync();

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> FilterIndexAID(string COPNO, string statusCode, string FROM_DATE, string TO_DATE)
        {
            Models.CopRunningModel model = new CopRunningModel(db);


            DateTime now = DateTime.Now;
            int dayOfWeek = (int)now.DayOfWeek;
            dayOfWeek = dayOfWeek == 0 ? 7 : dayOfWeek;
            DateTime startOfWeek = now.AddDays(0 - (int)now.DayOfWeek);
            DateTime endOfWeek = now.AddDays(6 - (int)now.DayOfWeek);


            string startDate = FROM_DATE;
            string endDate = TO_DATE;
            model.stDate = startDate;
            model.enDate = TO_DATE;
            model.statusCode = statusCode;
            model.COPNO = COPNO;

            string SQLCMD = "SELECT * FROM COPR16_COPRUNNING A WHERE A.ADATE between '" + startDate + "' and DATEADD(day,1,'" + endDate + "') ";
            if (COPNO != null && COPNO != "" && COPNO.Length > 0)
            {
                SQLCMD = SQLCMD + " AND COPR_ID like '%" + COPNO + "%'";
            }
            if (statusCode != null && statusCode != "" && statusCode.Length > 0 && !(statusCode.Contains("ALL")))
            {
                SQLCMD = SQLCMD + " AND COP_STATUS like '%" + statusCode + "%'";
            }

            SQLCMD = SQLCMD + " AND WRK_ID like 'COP-AID%' ORDER BY ADATE DESC";
            //model.cOPR16_COPRUNNING_List = await db.COPR16_COPRUNNING.Where(l=>l.ADATE.Value.ToString("yyyy-mm-dd") == Models.AppPropModel.today.ToString("yyyy-mm-dd")).OrderBy(l=>l.ADATE).ToListAsync();
            model.cOPR16_COPRUNNING_List = await db.COPR16_COPRUNNING.SqlQuery(SQLCMD).ToListAsync();

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> FilterIndexNONEC(string COPNO, string statusCode, string FROM_DATE, string TO_DATE)
        {
            Models.CopRunningModel model = new CopRunningModel(db);


            DateTime now = DateTime.Now;
            int dayOfWeek = (int)now.DayOfWeek;
            dayOfWeek = dayOfWeek == 0 ? 7 : dayOfWeek;
            DateTime startOfWeek = now.AddDays(0 - (int)now.DayOfWeek);
            DateTime endOfWeek = now.AddDays(6 - (int)now.DayOfWeek);


            string startDate = FROM_DATE;
            string endDate = TO_DATE;
            model.stDate = startDate;
            model.enDate = TO_DATE;
            model.statusCode = statusCode;
            model.COPNO = COPNO;

            string SQLCMD = "SELECT * FROM COPR16_COPRUNNING A WHERE A.ADATE between '" + startDate + "' and DATEADD(day,1,'" + endDate + "') ";
            if (COPNO != null && COPNO != "" && COPNO.Length > 0)
            {
                SQLCMD = SQLCMD + " AND COPR_ID like '%" + COPNO + "%'";
            }
            if (statusCode != null && statusCode != "" && statusCode.Length > 0 && !(statusCode.Contains("ALL")))
            {
                SQLCMD = SQLCMD + " AND COP_STATUS like '%" + statusCode + "%'";
            }

            SQLCMD = SQLCMD + " AND WRK_ID like 'NON-EC%' ORDER BY ADATE DESC";
            //model.cOPR16_COPRUNNING_List = await db.COPR16_COPRUNNING.Where(l=>l.ADATE.Value.ToString("yyyy-mm-dd") == Models.AppPropModel.today.ToString("yyyy-mm-dd")).OrderBy(l=>l.ADATE).ToListAsync();
            model.cOPR16_COPRUNNING_List = await db.COPR16_COPRUNNING.SqlQuery(SQLCMD).ToListAsync();

            return View(model);
        }

        // POST: COPR16_COPRUNNING_RT
        [HttpPost]
        public JsonResult GetCOPID()
        {

            //List<GetSeqNextValue1_Result> row = db.GetSeqNextValue("COPRUNNING").ToList();
            List<GetSeqNextValue1_Result> row = new List<GetSeqNextValue1_Result>();
            row.Add(new GetSeqNextValue1_Result());
            row[0].COPRUN = "TBD";
            return Json(row, JsonRequestBehavior.AllowGet);
        }

        // POST: COPR16_COPRUNNING_RT
        [HttpPost]
        public async Task<JsonResult> GetItems(itemParams param1)

        {
            //SqlParameter sQLCMD = new SqlParameter();
            //sQLCMD.ParameterName = "@SQLCMD";
            //sQLCMD.Direction = ParameterDirection.Output;
            //sQLCMD.DbType = DbType.String;
            //sQLCMD.Size = 4000;
            //var parameter = new List<object>();
            //var param = new SqlParameter("@fg_no", param1.fg_no);
            //param.DbType = DbType.String;
            //param.Size = 25;
            //param.Direction = ParameterDirection.Input;
            //parameter.Add(param);
            //param = new SqlParameter("@model_no", param1.model_no);
            //param.DbType = DbType.String;
            //param.Direction = ParameterDirection.Input;
            //param.Size = 25;
            //parameter.Add(param);
            //param = new SqlParameter("@brand_no", param1.brand_no);
            //param.DbType = DbType.String;
            //param.Direction = ParameterDirection.Input;
            //param.Size = 25;
            //parameter.Add(param);
            //parameter.Add(sQLCMD);

            //List<TEST_RST> row = db.GetItemWithParams(param1.fg_no, param1.model_no, param1.brand_no).ToList();

            string SQLCMD = "exec dbo.GetItemWithParams @p0,@p1,@p2";
            var data = await db.Database.SqlQuery<ItemsList>(SQLCMD, param1.fg_no, param1.model_no, param1.line_no).ToListAsync();

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> getLotDateAVAB(string MODEL,string LINE,string ZONE)
        {
            JsonResult rowData = new JsonResult();
            string SQLCMD2 = "exec dbo.COPR16_GET_LOTDATE_AVAB_BYMODEL @MODEL,@LINE,@ZONE";

            List<SqlParameter> param2 = new List<SqlParameter>();
            //param2.Add(new SqlParameter("@MODEL_ID", model_id == null ? "" : model_id));
            //await db.Database.ExecuteSqlCommandAsync(SQLCMD2, param2.ToArray());

            using (var cmd = db.Database.Connection.CreateCommand())
            {
                await db.Database.Connection.OpenAsync();
                cmd.CommandText = SQLCMD2;
                cmd.Parameters.Add(new SqlParameter("@MODEL", MODEL == null ? "" : MODEL));
                cmd.Parameters.Add(new SqlParameter("@LINE", LINE == null ? "" : LINE));
                cmd.Parameters.Add(new SqlParameter("@ZONE", ZONE == null ? "" : ZONE));
                DbDataReader reader = await cmd.ExecuteReaderAsync();
                {
                    var model = Serialize((SqlDataReader)reader);
                    rowData.Data = model;
                }
            }
            return Json(rowData, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> getLotDateAVAB_BYPOSID(string MODEL, string LINE, string POSID,string WRKID)
        {
            JsonResult rowData = new JsonResult();
            string SQLCMD2 = "exec dbo.COPR16_GET_LOTDATE_AVAB_BYMODEL_WITH_POSID @MODEL,@LINE,@POS_ID,@WRKID";

            List<SqlParameter> param2 = new List<SqlParameter>();
            //param2.Add(new SqlParameter("@MODEL_ID", model_id == null ? "" : model_id));
            //await db.Database.ExecuteSqlCommandAsync(SQLCMD2, param2.ToArray());

            using (var cmd = db.Database.Connection.CreateCommand())
            {
                await db.Database.Connection.OpenAsync();
                cmd.CommandText = SQLCMD2;
                cmd.Parameters.Add(new SqlParameter("@MODEL", MODEL == null ? "" : MODEL));
                cmd.Parameters.Add(new SqlParameter("@LINE", LINE == null ? "" : LINE));
                cmd.Parameters.Add(new SqlParameter("@POS_ID", POSID == null ? "" : POSID));
                cmd.Parameters.Add(new SqlParameter("@WRKID", WRKID == null ? "" : WRKID));
                DbDataReader reader = await cmd.ExecuteReaderAsync();
                {
                    var model = Serialize((SqlDataReader)reader);
                    rowData.Data = model;
                }
            }
            return Json(rowData, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> getBuckleListModel(itemParams param1)
        {
            JsonResult rowData = new JsonResult();
            string SQLCMD2 = "exec dbo.COPR16_GetBuckleWithParams @item_no,@model_no,@line_no,@position_no";

            List<SqlParameter> param2 = new List<SqlParameter>();
            //param2.Add(new SqlParameter("@MODEL_ID", model_id == null ? "" : model_id));
            //await db.Database.ExecuteSqlCommandAsync(SQLCMD2, param2.ToArray());

            using (var cmd = db.Database.Connection.CreateCommand())
            {
                await db.Database.Connection.OpenAsync();
                cmd.CommandText = SQLCMD2;
                cmd.Parameters.Add(new SqlParameter("@item_no", param1.fg_no == null ? "" : param1.fg_no));
                cmd.Parameters.Add(new SqlParameter("@model_no", param1.model_no == null ? "" : param1.model_no));
                cmd.Parameters.Add(new SqlParameter("@line_no", param1.line_no == null ? "" : param1.line_no));
                cmd.Parameters.Add(new SqlParameter("@position_no", param1.pos_no == null ? "" : param1.pos_no));
                DbDataReader reader = await cmd.ExecuteReaderAsync();
                {
                    var model = Serialize((SqlDataReader)reader);
                    rowData.Data = model;
                }
            }
            return Json(rowData, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> getSBListModel(itemParams param1)
        {
            JsonResult rowData = new JsonResult();
            string SQLCMD2 = "exec dbo.COPR16_GetSBWithParams @item_no,@model_no,@line_no,@position_no";

            List<SqlParameter> param2 = new List<SqlParameter>();
            //param2.Add(new SqlParameter("@MODEL_ID", model_id == null ? "" : model_id));
            //await db.Database.ExecuteSqlCommandAsync(SQLCMD2, param2.ToArray());

            using (var cmd = db.Database.Connection.CreateCommand())
            {
                await db.Database.Connection.OpenAsync();
                cmd.CommandText = SQLCMD2;
                cmd.Parameters.Add(new SqlParameter("@item_no", param1.fg_no == null ? "" : param1.fg_no));
                cmd.Parameters.Add(new SqlParameter("@model_no", param1.model_no == null ? "" : param1.model_no));
                cmd.Parameters.Add(new SqlParameter("@line_no", param1.line_no == null ? "" : param1.line_no));
                cmd.Parameters.Add(new SqlParameter("@position_no", param1.pos_no == null ? "" : param1.pos_no));
                DbDataReader reader = await cmd.ExecuteReaderAsync();
                {
                    var model = Serialize((SqlDataReader)reader);
                    rowData.Data = model;
                }
            }
            return Json(rowData, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> getSBListModelwithXPOS(itemParamsXPOS param1)
        {
            JsonResult rowData = new JsonResult();
            string SQLCMD2 = "exec dbo.COPR16_GetSBWithParamsWithXPOS @item_no,@model_no,@line_no,@xpos";

            List<SqlParameter> param2 = new List<SqlParameter>();
            //param2.Add(new SqlParameter("@MODEL_ID", model_id == null ? "" : model_id));
            //await db.Database.ExecuteSqlCommandAsync(SQLCMD2, param2.ToArray());

            using (var cmd = db.Database.Connection.CreateCommand())
            {
                await db.Database.Connection.OpenAsync();
                cmd.CommandText = SQLCMD2;
                cmd.Parameters.Add(new SqlParameter("@item_no", param1.fg_no == null ? "" : param1.fg_no));
                cmd.Parameters.Add(new SqlParameter("@model_no", param1.model_no == null ? "" : param1.model_no));
                cmd.Parameters.Add(new SqlParameter("@line_no", param1.line_no == null ? "" : param1.line_no));
                cmd.Parameters.Add(new SqlParameter("@xpos", param1.xpos == null ? "" : param1.xpos));
                DbDataReader reader = await cmd.ExecuteReaderAsync();
                {
                    var model = Serialize((SqlDataReader)reader);
                    rowData.Data = model;
                }
            }
            return Json(rowData, JsonRequestBehavior.AllowGet);
        }
        // POST: COPR16_COPRUNNING/GetAllItems
        [HttpPost]
        public async Task<JsonResult> GetAllItems(itemParams param1)

        {
            System.Data.Entity.Core.Objects.ObjectResult<GetAllItems_Result> row = db.GetAllItems ();
            //List<GetSeqNextValue1_Result> row = db.GetSeqNextValue1("COPRUNNING").ToList();
            DbCommand CMD = db.Database.Connection.CreateCommand();
            CMD.CommandText = "exec GetAllItems";
            CMD.CommandType = System.Data.CommandType.StoredProcedure;
            DbDataReader reader = await CMD.ExecuteReaderAsync();
            //List< GetAllItems_Result > row  =  await reader.

            return Json(row, JsonRequestBehavior.AllowGet);
        }

        // GET: COPR16_COPRUNNING/DashBoard
        public async Task<ActionResult> DashBoard()
        {
            Models.CopRunningModel model = new CopRunningModel(db);
            model.cOPR16_COPRUNNING_List = await db.COPR16_COPRUNNING.ToListAsync();
            string SQLCMD = "SELECT * FROM COPR16_MACHINE_MSTR ORDER BY MTYPE_ID ";
            model.cOPR16_MACHINE_MSTR_List = await db.COPR16_MACHINE_MSTR.SqlQuery(SQLCMD).ToListAsync();
            return View(model);
            //return View(await db.COPR16_COPRUNNING.ToListAsync());
        }
        
        // GET: COPR16_COPRUNNING/Tracking
        public async Task<ActionResult> Tracking()
        {
            return View(await db.COPR16_COPRUNNING.ToListAsync());
        }
        public async Task<ActionResult> Tracking2(string ID)
        {

            List<SelectListItem> MODEL_LIST= new List<SelectListItem>(); ;

            foreach (var row in await db.COPR16_MODEL_MSTR.Where(l => l.FLGACT == true).ToListAsync())
            {
                MODEL_LIST.Add(new SelectListItem { Text = row.MODEL_DESC, Value = row.MODEL_ID });
            }
            ViewBag.ModelList = MODEL_LIST;
            ViewBag.ModelSet = ID;
            //return View(await db.COPR16_COPRUNNING.ToListAsync());
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> getAllMachine()
        {
            List<object> dataRst = new List<object>();
            JsonResult rowData = new JsonResult();
            string SQLCMD2 = "SELECT A.MANC_ID,A.MANC_DESC,C.MTYPE_NAME,B.COP_ID,B.STATUS " +
                "FROM COPR16_MACHINE_MSTR A " +
                "LEFT OUTER JOIN COPR16_MANCHINE_LOCK B ON A.MANC_ID = B.MANC_ID " +
                "JOIN COPR16_MANCTYPE_MSTR C ON A.MTYPE_ID = C.MTYPE_ID";

            //List<SqlParameter> param2 = new List<SqlParameter>();
            //param2.Add(new SqlParameter("@MANC_ID", MANC_ID == null ? "" : MANC_ID));
            //await db.Database.ExecuteSqlCommandAsync(SQLCMD2, param2.ToArray());

            using (var cmd = db.Database.Connection.CreateCommand())
            {
                if (db.Database.Connection.State != ConnectionState.Open)
                {
                    await db.Database.Connection.OpenAsync();
                }
                cmd.CommandText = SQLCMD2;
                DbDataReader reader = await cmd.ExecuteReaderAsync();
                {
                    var model = Serialize((SqlDataReader)reader);
                    dataRst.Add( model);
                }
            }

            SQLCMD2 = "exec sp_getAllMachineStatus";
            using (var cmd = db.Database.Connection.CreateCommand())
            {
                if (db.Database.Connection.State != ConnectionState.Open)
                {
                    await db.Database.Connection.OpenAsync();
                }
                cmd.CommandText = SQLCMD2;
                DbDataReader reader = await cmd.ExecuteReaderAsync();
                {
                    var model = Serialize((SqlDataReader)reader);
                    dataRst.Add(model);
                }
            }

            rowData.Data = dataRst;
            return Json(rowData, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> getAllCopSTatus()
        {
            JsonResult rowData = new JsonResult();
            string SQLCMD2 = "SELECT A.COP_STATUS,COUNT(1) STATUS FROM COPR16_COPRUNNING A GROUP BY A.COP_STATUS " +
                "UNION SELECT 'ALL' [COP_STATUS],COUNT(*) [STATUS] FROM COPR16_COPRUNNING ";

            //List<SqlParameter> param2 = new List<SqlParameter>();
            //param2.Add(new SqlParameter("@MANC_ID", MANC_ID == null ? "" : MANC_ID));
            //await db.Database.ExecuteSqlCommandAsync(SQLCMD2, param2.ToArray());

            using (var cmd = db.Database.Connection.CreateCommand())
            {
                await db.Database.Connection.OpenAsync();
                cmd.CommandText = SQLCMD2;
                DbDataReader reader = await cmd.ExecuteReaderAsync();
                {
                    var model = Serialize((SqlDataReader)reader);
                    rowData.Data = model;
                }
            }
            return Json(rowData, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> getAllModelStatus()
        {
            JsonResult rowData = new JsonResult();
            string SQLCMD2 = "exec sp_getModelQty";

            //List<SqlParameter> param2 = new List<SqlParameter>();
            //param2.Add(new SqlParameter("@MANC_ID", MANC_ID == null ? "" : MANC_ID));
            //await db.Database.ExecuteSqlCommandAsync(SQLCMD2, param2.ToArray());

            using (var cmd = db.Database.Connection.CreateCommand())
            {
                await db.Database.Connection.OpenAsync();
                cmd.CommandText = SQLCMD2;
                DbDataReader reader = await cmd.ExecuteReaderAsync();
                {
                    var model = Serialize((SqlDataReader)reader);
                    rowData.Data = model;
                }
            }
            return Json(rowData, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> getLineListByModel (string model_id)
        {
            JsonResult rowData = new JsonResult();
            string SQLCMD2 = "exec dbo.sp_getLine @MODEL_ID";

            List<SqlParameter> param2 = new List<SqlParameter>();
            //param2.Add(new SqlParameter("@MODEL_ID", model_id == null ? "" : model_id));
            //await db.Database.ExecuteSqlCommandAsync(SQLCMD2, param2.ToArray());

            using (var cmd = db.Database.Connection.CreateCommand())
            {
                await db.Database.Connection.OpenAsync();
                cmd.CommandText = SQLCMD2;
                cmd.Parameters.Add(new SqlParameter("@MODEL_ID", model_id == null ? "" : model_id));
                DbDataReader reader = await cmd.ExecuteReaderAsync();
                {
                    var model = Serialize((SqlDataReader)reader);
                    rowData.Data = model;
                }
            }
            return Json(rowData, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> getPosListByModel(string model_id)
        {
            JsonResult rowData = new JsonResult();
            string SQLCMD2 = "exec dbo.sp_getPos @MODEL_ID";

            List<SqlParameter> param2 = new List<SqlParameter>();
            //param2.Add(new SqlParameter("@MODEL_ID", model_id == null ? "" : model_id));
            //await db.Database.ExecuteSqlCommandAsync(SQLCMD2, param2.ToArray());

            using (var cmd = db.Database.Connection.CreateCommand())
            {
                await db.Database.Connection.OpenAsync();
                cmd.CommandText = SQLCMD2;
                cmd.Parameters.Add(new SqlParameter("@MODEL_ID", model_id == null ? "" : model_id));
                DbDataReader reader = await cmd.ExecuteReaderAsync();
                {
                    var model = Serialize((SqlDataReader)reader);
                    rowData.Data = model;
                }
            }
            return Json(rowData, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> getPosListByModelLine(string model_id,string line_id)
        {
            JsonResult rowData = new JsonResult();
            //string SQLCMD2 = "exec dbo.sp_getPos @MODEL_ID";
            string SQLCMD2 = "exec dbo.COPR16_SP_GETPOS @MODEL_ID, @LINE_ID";

            List<SqlParameter> param2 = new List<SqlParameter>();
            //param2.Add(new SqlParameter("@MODEL_ID", model_id == null ? "" : model_id));
            //await db.Database.ExecuteSqlCommandAsync(SQLCMD2, param2.ToArray());

            using (var cmd = db.Database.Connection.CreateCommand())
            {
                await db.Database.Connection.OpenAsync();
                cmd.CommandText = SQLCMD2;
                cmd.Parameters.Add(new SqlParameter("@MODEL_ID", model_id == null ? "" : model_id));
                cmd.Parameters.Add(new SqlParameter("@LINE_ID", line_id == null ? "" : line_id));
                DbDataReader reader = await cmd.ExecuteReaderAsync();
                {
                    var model = Serialize((SqlDataReader)reader);
                    rowData.Data = model;
                }
            }
            return Json(rowData, JsonRequestBehavior.AllowGet);
        }

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
        // GET: COPR16_COPRUNNING/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            COPR16_COPRUNNING cOPR16_COPRUNNING = await db.COPR16_COPRUNNING.FindAsync(id);
            if (cOPR16_COPRUNNING == null)
            {
                return HttpNotFound();
            }
            return View(cOPR16_COPRUNNING);
        }

        // GET: COPR16_COPRUNNING/Create
        public ActionResult Create()
        {
            CopRunningModel model = new CopRunningModel(db);
            model.cOPR16_COPRUNNING = new COPR16_COPRUNNING();
            model.cOPR16_COPRUNNING_DT = new COPR16_COPRUNNING_DT();
            model.WRK_LIST = new List<SelectListItem>();
            foreach (var row in db.COPR16_WORKFLOW.SqlQuery("SELECT * FROM COPR16_WORKFLOW WHERE WRK_ID LIKE 'COP-ATH-%'"))
            {
                model.WRK_LIST.Add(new SelectListItem { Text = row.WRK_NAME, Value = row.WRK_ID });
            }

            return View(model);
        }

        // GET: COPR16_COPRUNNING/Create
        public ActionResult CreateAID()
        {
            CopRunningModel model = new CopRunningModel(db);
            model.cOPR16_COPRUNNING = new COPR16_COPRUNNING();
            model.cOPR16_COPRUNNING_DT = new COPR16_COPRUNNING_DT();
            model.WRK_LIST = new List<SelectListItem>();
            foreach (var row in db.COPR16_WORKFLOW.SqlQuery("SELECT * FROM COPR16_WORKFLOW WHERE WRK_ID LIKE 'COP-AID-%'"))
            {
                model.WRK_LIST.Add(new SelectListItem { Text = row.WRK_NAME, Value = row.WRK_ID });
            }

            return View(model);
        }
        // GET: COPR16_COPRUNNING/Create
        public ActionResult CreateNONEC()
        {
            CopRunningModel model = new CopRunningModel(db);
            model.cOPR16_COPRUNNING = new COPR16_COPRUNNING();
            model.cOPR16_COPRUNNING_DT = new COPR16_COPRUNNING_DT();
            model.WRK_LIST = new List<SelectListItem>();
            foreach (var row in db.COPR16_WORKFLOW.SqlQuery("SELECT * FROM COPR16_WORKFLOW WHERE WRK_ID LIKE 'NON-EC-%'"))
            {
                model.WRK_LIST.Add(new SelectListItem { Text = row.WRK_NAME, Value = row.WRK_ID });
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateMacro(string xModel,string xLine,string WRK_ID,string btnDate,string xPos,string PROC_ID,string xACCTT)
        {


            CopRunningModel model = new CopRunningModel(db);
            model.cOPR16_COPRUNNING_WRK_ID = WRK_ID;
            model.xModel = xModel;
            model.xLine = xLine;
            model.xPos = xPos;
            model.selectDate = btnDate;
            model.xProc = PROC_ID;
            model.xACCTT = xACCTT;
            model.cOPR16_COPRUNNING = new COPR16_COPRUNNING();
            model.cOPR16_COPRUNNING_DT = new COPR16_COPRUNNING_DT();
            model.POSITION_LIST = new List<SelectListItem>();
            foreach (var row in db.COPR16_POSITION_MSTR.SqlQuery("SELECT * FROM COPR16_POSITION_MSTR WHERE FRNT_REAR = '"+ xPos + "'"))
            {
                model.POSITION_LIST.Add(new SelectListItem { Text = row.POS_DESC, Value = row.POS_ID });
            }
            return View(model);
        }


        [HttpPost]
        public async Task<JsonResult> getPositionName(string posID)
        {
            JsonResult row = new JsonResult();
            string SQLCMD2 = "exec dbo.sp_getPosDesc @POS_ID";

            using (var cmd = db.Database.Connection.CreateCommand())
            {
                await db.Database.Connection.OpenAsync();
                cmd.CommandText = SQLCMD2;
                cmd.Parameters.Add(new SqlParameter("@POS_ID", posID == null ? "" : posID));
                DbDataReader reader = await cmd.ExecuteReaderAsync();
                {
                    var model = Serialize((SqlDataReader)reader);
                    row.Data = model;
                }
            }
            return Json(row, JsonRequestBehavior.AllowGet);
        }

        //getRunningReport
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> getRunningReport(string fromDate, string toDate)
        {
            JsonResult row = new JsonResult();
            List<object> dataRst = new List<object>();

            string SQLCMD2 = "exec dbo.COPR16_SP_GET_RUNNING @FROM_DT,@TO_DT";
            if (db.Database.Connection.State != ConnectionState.Open)
            {
                await db.Database.Connection.OpenAsync();
            }
            using (var cmd = db.Database.Connection.CreateCommand())
            {
                cmd.CommandText = SQLCMD2;
                cmd.Parameters.Add(new SqlParameter("@FROM_DT", fromDate == null ? "" : fromDate));
                cmd.Parameters.Add(new SqlParameter("@TO_DT", toDate == null ? "" : toDate));
                DbDataReader reader = await cmd.ExecuteReaderAsync();
                {
                    var model = Serialize((SqlDataReader)reader);
                    row.Data = model;
                }
            }
            return Json(row, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> getFinalReport(string ModelID,String fromDate,String toDate)
        {
            JsonResult row = new JsonResult();
            List<object> dataRst = new List<object>();

            string SQLCMD2 = "exec dbo.COPR16_SP_GET_COP_COUNT_BY_MODEL_ATH @MODEL_ID,@FROM_DT,@TO_DT";
            using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DatabaseServer"].ToString()))
            {
                if (con.State != ConnectionState.Open)
                {
                    await con.OpenAsync();
                }
                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = SQLCMD2;
                    cmd.Parameters.Add(new SqlParameter("@MODEL_ID", ModelID == null ? "" : ModelID));
                    cmd.Parameters.Add(new SqlParameter("@FROM_DT", fromDate == null ? "" : fromDate));
                    cmd.Parameters.Add(new SqlParameter("@TO_DT", toDate == null ? "" : toDate));
                    DbDataReader reader = await cmd.ExecuteReaderAsync();
                    {
                        var model = Serialize((SqlDataReader)reader);
                        dataRst.Add(model);
                    }
                    reader.Close();
                }

                SQLCMD2 = "exec dbo.COPR16_SP_GET_COP_PARTS_LIST_BY_MODEL_ATH @MODEL_ID,@FROM_DT,@TO_DT";

                using (var cmd = con.CreateCommand())
                {

                    cmd.CommandText = SQLCMD2;
                    cmd.Parameters.Add(new SqlParameter("@MODEL_ID", ModelID == null ? "" : ModelID));
                    cmd.Parameters.Add(new SqlParameter("@FROM_DT", fromDate == null ? "" : fromDate));
                    cmd.Parameters.Add(new SqlParameter("@TO_DT", toDate == null ? "" : toDate));
                    DbDataReader reader = await cmd.ExecuteReaderAsync();
                    {
                        var model = Serialize((SqlDataReader)reader);
                        dataRst.Add(model);
                    }
                    reader.Close();
                }
                SQLCMD2 = "exec dbo.COPR16_SP_GET_COP_RESULT_BY_MODEL_ATH @MODEL_ID,@FROM_DT,@TO_DT";

                using (var cmd = con.CreateCommand())
                {

                    cmd.CommandText = SQLCMD2;
                    cmd.Parameters.Add(new SqlParameter("@MODEL_ID", ModelID == null ? "" : ModelID));
                    cmd.Parameters.Add(new SqlParameter("@FROM_DT", fromDate == null ? "" : fromDate));
                    cmd.Parameters.Add(new SqlParameter("@TO_DT", toDate == null ? "" : toDate));
                    DbDataReader reader = await cmd.ExecuteReaderAsync();
                    {
                        var model = Serialize((SqlDataReader)reader);
                        dataRst.Add(model);
                    }
                    reader.Close();
                }
                con.Close();
            }

            row.Data = dataRst;
            return Json(row, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> getFinalReportAID(string ModelID, String fromDate, String toDate)
        {
            JsonResult row = new JsonResult();
            List<object> dataRst = new List<object>();

            string SQLCMD2 = "exec dbo.COPR16_SP_GET_COP_COUNT_BY_MODEL_AID @MODEL_ID,@FROM_DT,@TO_DT";
            using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DatabaseServer"].ToString()))
            {
                if (con.State != ConnectionState.Open)
                {
                    await con.OpenAsync();
                }
                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = SQLCMD2;
                    cmd.Parameters.Add(new SqlParameter("@MODEL_ID", ModelID == null ? "" : ModelID));
                    cmd.Parameters.Add(new SqlParameter("@FROM_DT", fromDate == null ? "" : fromDate));
                    cmd.Parameters.Add(new SqlParameter("@TO_DT", toDate == null ? "" : toDate));
                    DbDataReader reader = await cmd.ExecuteReaderAsync();
                    {
                        var model = Serialize((SqlDataReader)reader);
                        dataRst.Add(model);
                    }
                    reader.Close();
                }

                SQLCMD2 = "exec dbo.COPR16_SP_GET_COP_PARTS_LIST_BY_MODEL_AID @MODEL_ID,@FROM_DT,@TO_DT";

                using (var cmd = con.CreateCommand())
                {

                    cmd.CommandText = SQLCMD2;
                    cmd.Parameters.Add(new SqlParameter("@MODEL_ID", ModelID == null ? "" : ModelID));
                    cmd.Parameters.Add(new SqlParameter("@FROM_DT", fromDate == null ? "" : fromDate));
                    cmd.Parameters.Add(new SqlParameter("@TO_DT", toDate == null ? "" : toDate));
                    DbDataReader reader = await cmd.ExecuteReaderAsync();
                    {
                        var model = Serialize((SqlDataReader)reader);
                        dataRst.Add(model);
                    }
                    reader.Close();
                }
                SQLCMD2 = "exec dbo.COPR16_SP_GET_COP_RESULT_BY_MODEL_AID @MODEL_ID,@FROM_DT,@TO_DT";

                using (var cmd = con.CreateCommand())
                {

                    cmd.CommandText = SQLCMD2;
                    cmd.Parameters.Add(new SqlParameter("@MODEL_ID", ModelID == null ? "" : ModelID));
                    cmd.Parameters.Add(new SqlParameter("@FROM_DT", fromDate == null ? "" : fromDate));
                    cmd.Parameters.Add(new SqlParameter("@TO_DT", toDate == null ? "" : toDate));
                    DbDataReader reader = await cmd.ExecuteReaderAsync();
                    {
                        var model = Serialize((SqlDataReader)reader);
                        dataRst.Add(model);
                    }
                    reader.Close();
                }
                con.Close();
            }
            
            row.Data = dataRst;
            return Json(row, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> getFinalReportNONEC(string ModelID, String fromDate, String toDate)
        {
            JsonResult row = new JsonResult();
            List<object> dataRst = new List<object>();

            string SQLCMD2 = "exec dbo.COPR16_SP_GET_COP_COUNT_BY_MODEL_NONEC @MODEL_ID,@FROM_DT,@TO_DT";
            using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DatabaseServer"].ToString()))
            {
                if (con.State != ConnectionState.Open)
                {
                    await con.OpenAsync();
                }
                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = SQLCMD2;
                    cmd.Parameters.Add(new SqlParameter("@MODEL_ID", ModelID == null ? "" : ModelID));
                    cmd.Parameters.Add(new SqlParameter("@FROM_DT", fromDate == null ? "" : fromDate));
                    cmd.Parameters.Add(new SqlParameter("@TO_DT", toDate == null ? "" : toDate));
                    DbDataReader reader = await cmd.ExecuteReaderAsync();
                    {
                        var model = Serialize((SqlDataReader)reader);
                        dataRst.Add(model);
                    }
                    reader.Close();
                }

                SQLCMD2 = "exec dbo.COPR16_SP_GET_COP_PARTS_LIST_BY_MODEL_NONEC @MODEL_ID,@FROM_DT,@TO_DT";

                using (var cmd = con.CreateCommand())
                {

                    cmd.CommandText = SQLCMD2;
                    cmd.Parameters.Add(new SqlParameter("@MODEL_ID", ModelID == null ? "" : ModelID));
                    cmd.Parameters.Add(new SqlParameter("@FROM_DT", fromDate == null ? "" : fromDate));
                    cmd.Parameters.Add(new SqlParameter("@TO_DT", toDate == null ? "" : toDate));
                    DbDataReader reader = await cmd.ExecuteReaderAsync();
                    {
                        var model = Serialize((SqlDataReader)reader);
                        dataRst.Add(model);
                    }
                    reader.Close();
                }
                SQLCMD2 = "exec dbo.COPR16_SP_GET_COP_RESULT_BY_MODEL_NONEC @MODEL_ID,@FROM_DT,@TO_DT";

                using (var cmd = con.CreateCommand())
                {

                    cmd.CommandText = SQLCMD2;
                    cmd.Parameters.Add(new SqlParameter("@MODEL_ID", ModelID == null ? "" : ModelID));
                    cmd.Parameters.Add(new SqlParameter("@FROM_DT", fromDate == null ? "" : fromDate));
                    cmd.Parameters.Add(new SqlParameter("@TO_DT", toDate == null ? "" : toDate));
                    DbDataReader reader = await cmd.ExecuteReaderAsync();
                    {
                        var model = Serialize((SqlDataReader)reader);
                        dataRst.Add(model);
                    }
                    reader.Close();
                }
                con.Close();
            }

            row.Data = dataRst;
            return Json(row, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public async Task<JsonResult> getModelName(string MODEL_ID)
        {
            JsonResult row = new JsonResult();
            //row.Data = await db.COPR16_MODEL_MSTR.Where(l => l.MODEL_ID.Contains(posID)).FirstAsync();

            string SQLCMD2 = "exec dbo.sp_getModelDesc @POS_ID";

            using (var cmd = db.Database.Connection.CreateCommand())
            {
                await db.Database.Connection.OpenAsync();
                cmd.CommandText = SQLCMD2;
                cmd.Parameters.Add(new SqlParameter("@POS_ID", MODEL_ID == null ? "" : MODEL_ID));
                DbDataReader reader = await cmd.ExecuteReaderAsync();
                {
                    var model = Serialize((SqlDataReader)reader);
                    row.Data = model;
                }
            }
            return Json(row, JsonRequestBehavior.AllowGet);
        }
        // POST: COPR16_COPRUNNING/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "COPR_ID,MODEL_ID,POSITION_ID,FGTYPE_ID,WRK_ID,DESC,ADATE,CRE_BY,MOD_DATE,MOD_BY,COP_STATUS")] COPR16_COPRUNNING cOPR16_COPRUNNING)
        {
            if (ModelState.IsValid)
            {
                db.COPR16_COPRUNNING.Add(cOPR16_COPRUNNING);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(cOPR16_COPRUNNING);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateJson(
            string COPR_ID,
            string WRK_ID,
            string PROC_ID,
            string MODEL_ID,
            string POSITION_ID,
            string DESC,
            string COP_STATUS,
            string username,
            List<ITEMSROW> jsonString)
        {
            List<GetSeqNextValue1_Result> row = db.GetSeqNextValue("COPRUNNING").ToList();

            string lCOPR_ID =  row.FirstOrDefault().COPRUN + "-" + PROC_ID + "-00";

            COPR16_COPRUNNING cOPR16_COPRUNNING = new COPR16_COPRUNNING();
            cOPR16_COPRUNNING.COPR_ID = lCOPR_ID;
            cOPR16_COPRUNNING.WRK_ID = WRK_ID;
            cOPR16_COPRUNNING.PROC_ID = PROC_ID;
            cOPR16_COPRUNNING.MODEL_ID = MODEL_ID;
            cOPR16_COPRUNNING.POSITION_ID = POSITION_ID;
            cOPR16_COPRUNNING.DESC = DESC;
            cOPR16_COPRUNNING.COP_STATUS = COP_STATUS;
            cOPR16_COPRUNNING.CRE_BY = username;
            cOPR16_COPRUNNING.ADATE = AppPropModel.today;
            if (ModelState.IsValid)
            {
                db.COPR16_COPRUNNING.Add(cOPR16_COPRUNNING);
                await db.SaveChangesAsync();
            }
            foreach (ITEMSROW item in jsonString)
            {

                COPR16_COPRUNNING_DT dtdt = new COPR16_COPRUNNING_DT();
                dtdt.COPR_ID = lCOPR_ID;
                dtdt.DESC = "";
                dtdt.FGTYPE_ID = item.FGTYPE_ID;
                dtdt.LOTNO = item.LOTNO;
                dtdt.PNO = item.PNO;
                if (ModelState.IsValid)
                {
                    db.COPR16_COPRUNNING_DT.Add(dtdt);
                    await db.SaveChangesAsync();
                }
            }
            List<COPR16_WORKFLOW_DT> cOPR16_WORKFLOW_DT_List = await db.COPR16_WORKFLOW_DT.Where(l => l.WRK_ID.Equals(WRK_ID) && l.WRKD_WITH_ID != l.WRKD_SEQ).ToListAsync();

            foreach (COPR16_WORKFLOW_DT row1 in cOPR16_WORKFLOW_DT_List)
            {
                List<COPR16_STEP_MSTR> cOPR16_STEP_MSTR_List = await db.COPR16_STEP_MSTR.Where(l => l.STEP_ID.Equals(row1.STEP_ID)).ToListAsync();
                foreach (COPR16_STEP_MSTR row2 in cOPR16_STEP_MSTR_List)
                {
                    COPR16_MANCTYPE_MSTR cOPR16_MANCTYPE_MSTR = await db.COPR16_MANCTYPE_MSTR.FindAsync(row2.MANC_ID);
                    List<COPR16_RTDT_MSTR> cOPR16_RTDT_MSTR_List = await db.COPR16_RTDT_MSTR.Where(l => l.RTTYPE_ID.Equals(cOPR16_MANCTYPE_MSTR.RTYPE_ID)).ToListAsync();
                    foreach (COPR16_RTDT_MSTR row3 in cOPR16_RTDT_MSTR_List)
                    {
                        COPR16_MEASURETYPE_MSTR cOPR16_MEASURETYPE_MSTR = await db.COPR16_MEASURETYPE_MSTR.FindAsync(row3.MSTYPE_ID);
                        //COPR16_COPRUNNING_RT dtdt = new COPR16_COPRUNNING_RT();
                        List<SqlParameter> VarParams = new List<SqlParameter>();

                        VarParams.Add(new SqlParameter("@COPR_ID", lCOPR_ID));
                        VarParams.Add(new SqlParameter("@PNO", ""));
                        VarParams.Add(new SqlParameter("@FGTYPE_ID", cOPR16_MANCTYPE_MSTR.FGT_ID));
                        VarParams.Add(new SqlParameter("@WRK_ID", WRK_ID));
                        VarParams.Add(new SqlParameter("@WRKD_ID", row1.WRKD_ID));
                        VarParams.Add(new SqlParameter("@MACHINETYPE_ID", cOPR16_MANCTYPE_MSTR.MTYPE_ID));
                        VarParams.Add(new SqlParameter("@RETURNTYPE_ID", row3.RTTYPE_ID));
                        VarParams.Add(new SqlParameter("@SEQ_NO", row1.WRKD_SEQ));
                        VarParams.Add(new SqlParameter("@REV", "00"));
                        VarParams.Add(new SqlParameter("@RTDT_ID", row3.RTDT_ID));
                        VarParams.Add(new SqlParameter("@RTDT_NAME", row3.RTDT_NAME));
                        VarParams.Add(new SqlParameter("@RTDT_REF_ID", row3.REF_ID));


                        //dtdt.COPR_ID = COPR_ID;
                        //    dtdt.FGTYPE_ID = cOPR16_MANCTYPE_MSTR.FGT_ID;
                        //    dtdt.WRK_ID = WRK_ID;
                        //    dtdt.WRKD_ID = row1.WRKD_ID;
                        //    dtdt.MACHINETYPE_ID = cOPR16_MANCTYPE_MSTR.MTYPE_ID;
                        //    dtdt.RETURNTYPE_ID = row3.RTTYPE_ID;
                        //    dtdt.SEQ_NO = row1.WRKD_SEQ;
                        //    dtdt.RTDT_ID = row3.RTDT_ID;
                        //    dtdt.REV = "00";
                        //    dtdt.RTDT_NAME = row3.RTDT_NAME;
                        //    dtdt.RTDT_REF_ID = row3.REF_ID;

                        //if (ModelState.IsValid) {
                        //    db.COPR16_COPRUNNING_RT.Add(dtdt);
                        //    await db.SaveChangesAsync();
                        //}
                        //db.Database.ExecuteSqlCommand("exec GenerateJob @COPR_ID, @PNO, @FGTPE_ID, @WRK_ID, @WRKD_ID, @MACHINETYPE_ID, @RETURNTYPE_ID, @SEQ_NO, @REV, @RTDT_ID, @RTDT_NAME, @RTDT_REF_ID", VarParams.ToList());
                        await db.Database.ExecuteSqlCommandAsync("exec GenerateJob @COPR_ID, @PNO, @FGTYPE_ID, @WRK_ID, @WRKD_ID, @MACHINETYPE_ID, @RETURNTYPE_ID, @SEQ_NO, @REV, @RTDT_ID, @RTDT_NAME, @RTDT_REF_ID", 
                            VarParams[0]
                            , VarParams[1]
                            , VarParams[2]
                            , VarParams[3]
                            , VarParams[4]
                            , VarParams[5]
                            , VarParams[6]
                            , VarParams[7]
                            , VarParams[8]
                            , VarParams[9]
                            , VarParams[10]
                            , VarParams[11]
                            );
                    }
                }

            }

            return RedirectToAction("Index");
        }

        public async Task<ActionResult> CreateJsonWithLine(
            string COPR_ID,
            string WRK_ID,
            string PROC_ID,
            string MODEL_ID,
            string POSITION_ID,
            string DESC,
            string COP_STATUS,
            string LINE_ID,
            string SELECT_DATE,
            string username,
            List<ITEMSROW> jsonString)
        {
            /*
            COPR16_COPRUNNING cOPR16_COPRUNNING = new COPR16_COPRUNNING();
            cOPR16_COPRUNNING.COPR_ID = COPR_ID;
            cOPR16_COPRUNNING.WRK_ID = WRK_ID;
            cOPR16_COPRUNNING.PROC_ID = PROC_ID;
            cOPR16_COPRUNNING.MODEL_ID = MODEL_ID;
            cOPR16_COPRUNNING.POSITION_ID = POSITION_ID;
            cOPR16_COPRUNNING.DESC = DESC;
            cOPR16_COPRUNNING.COP_STATUS = COP_STATUS;
            cOPR16_COPRUNNING.LINE_ID = LINE_ID;
            cOPR16_COPRUNNING.CRE_BY = username;
            cOPR16_COPRUNNING.ADATE = AppPropModel.today;
            if (ModelState.IsValid)
            {
                db.COPR16_COPRUNNING.Add(cOPR16_COPRUNNING);
                await db.SaveChangesAsync();
            }
            */
            /// Create COP Header
            List<GetSeqNextValue1_Result> row = db.GetSeqNextValue("COPRUNNING").ToList();
            string lCOPR_ID =  row.FirstOrDefault().COPRUN + "-" + PROC_ID + "-00";

            if (db.Database.Connection.State != ConnectionState.Open)
            {
                await db.Database.Connection.OpenAsync();
            }
            string SQLCMD2 = "exec dbo.sp_CreateNewCop @COPR_ID,@WRK_ID,@PROC_ID,@MODEL_ID,@POSITION_ID,@DESC,@COP_STATUS,@LINE_ID,@CRE_BY";

            using (var cmd = db.Database.Connection.CreateCommand())
            {
                //await db.Database.Connection.OpenAsync();
                cmd.CommandText = SQLCMD2;
                cmd.Parameters.Add(new SqlParameter("@COPR_ID", lCOPR_ID == null ? "" : lCOPR_ID));
                cmd.Parameters.Add(new SqlParameter("@WRK_ID", WRK_ID == null ? "" : WRK_ID));
                cmd.Parameters.Add(new SqlParameter("@PROC_ID", PROC_ID == null ? "" : PROC_ID));
                cmd.Parameters.Add(new SqlParameter("@MODEL_ID", MODEL_ID == null ? "" : MODEL_ID));
                cmd.Parameters.Add(new SqlParameter("@POSITION_ID", POSITION_ID == null ? "" : POSITION_ID));
                cmd.Parameters.Add(new SqlParameter("@DESC", DESC == null ? "" : DESC));
                cmd.Parameters.Add(new SqlParameter("@COP_STATUS", COP_STATUS == null ? "" : COP_STATUS));
                cmd.Parameters.Add(new SqlParameter("@LINE_ID", LINE_ID == null ? "" : LINE_ID));
                cmd.Parameters.Add(new SqlParameter("@CRE_BY", username == null ? "" : username));

                var reader = await cmd.ExecuteNonQueryAsync();
                
            }
            /** Create COP FG PART**/
            foreach (ITEMSROW item in jsonString)
            {

                COPR16_COPRUNNING_DT dtdt = new COPR16_COPRUNNING_DT();
                dtdt.COPR_ID = lCOPR_ID;
                dtdt.DESC = "";
                dtdt.FGTYPE_ID = item.FGTYPE_ID;
                dtdt.LOTNO = item.LOTNO;
                dtdt.PNO = item.PNO;
                if (ModelState.IsValid)
                {
                    db.COPR16_COPRUNNING_DT.Add(dtdt);
                    await db.SaveChangesAsync();
                }
            }


            //** Create COP WORKFLOW DETAILS**//
            List<COPR16_WORKFLOW_DT> cOPR16_WORKFLOW_DT_List = await db.COPR16_WORKFLOW_DT.Where(l => l.WRK_ID.Equals(WRK_ID) && l.WRKD_WITH_ID != l.WRKD_SEQ).ToListAsync();

            foreach (COPR16_WORKFLOW_DT row1 in cOPR16_WORKFLOW_DT_List)
            {
                List<COPR16_STEP_MSTR> cOPR16_STEP_MSTR_List = await db.COPR16_STEP_MSTR.Where(l => l.STEP_ID.Equals(row1.STEP_ID)).ToListAsync();
                foreach (COPR16_STEP_MSTR row2 in cOPR16_STEP_MSTR_List)
                {
                    COPR16_MANCTYPE_MSTR cOPR16_MANCTYPE_MSTR = await db.COPR16_MANCTYPE_MSTR.FindAsync(row2.MANC_ID);
                    List<COPR16_RTDT_MSTR> cOPR16_RTDT_MSTR_List = await db.COPR16_RTDT_MSTR.Where(l => l.RTTYPE_ID.Equals(cOPR16_MANCTYPE_MSTR.RTYPE_ID)).ToListAsync();
                    foreach (COPR16_RTDT_MSTR row3 in cOPR16_RTDT_MSTR_List)
                    {
                        COPR16_MEASURETYPE_MSTR cOPR16_MEASURETYPE_MSTR = await db.COPR16_MEASURETYPE_MSTR.FindAsync(row3.MSTYPE_ID);
                        //COPR16_COPRUNNING_RT dtdt = new COPR16_COPRUNNING_RT();
                        List<SqlParameter> VarParams = new List<SqlParameter>();

                        VarParams.Add(new SqlParameter("@COPR_ID", lCOPR_ID));
                        VarParams.Add(new SqlParameter("@PNO", ""));
                        VarParams.Add(new SqlParameter("@FGTYPE_ID", cOPR16_MANCTYPE_MSTR.FGT_ID));
                        VarParams.Add(new SqlParameter("@WRK_ID", WRK_ID));
                        VarParams.Add(new SqlParameter("@WRKD_ID", row1.WRKD_ID));
                        VarParams.Add(new SqlParameter("@MACHINETYPE_ID", cOPR16_MANCTYPE_MSTR.MTYPE_ID));
                        VarParams.Add(new SqlParameter("@RETURNTYPE_ID", row3.RTTYPE_ID));
                        VarParams.Add(new SqlParameter("@SEQ_NO", row1.WRKD_SEQ));
                        VarParams.Add(new SqlParameter("@REV", "00"));
                        VarParams.Add(new SqlParameter("@RTDT_ID", row3.RTDT_ID));
                        VarParams.Add(new SqlParameter("@RTDT_NAME", row3.RTDT_NAME));
                        VarParams.Add(new SqlParameter("@RTDT_REF_ID", row3.REF_ID));
                        await db.Database.ExecuteSqlCommandAsync("exec GenerateJob @COPR_ID, @PNO, @FGTYPE_ID, @WRK_ID, @WRKD_ID, @MACHINETYPE_ID, @RETURNTYPE_ID, @SEQ_NO, @REV, @RTDT_ID, @RTDT_NAME, @RTDT_REF_ID",
                            VarParams[0]
                            , VarParams[1]
                            , VarParams[2]
                            , VarParams[3]
                            , VarParams[4]
                            , VarParams[5]
                            , VarParams[6]
                            , VarParams[7]
                            , VarParams[8]
                            , VarParams[9]
                            , VarParams[10]
                            , VarParams[11]
                            );
                    }
                }

            }

            if (db.Database.Connection.State != ConnectionState.Open)
            {
                await db.Database.Connection.OpenAsync();
            }

            using (var cmd = db.Database.Connection.CreateCommand())
            {

                cmd.CommandText = "exec [dbo].[sp_save_selectdate] @COPR_ID,@MODEL_ID,@POSITION_ID,@LINE_ID,@PROC_ID,@SELECT_DATE"; 
                cmd.Parameters.Add(new SqlParameter("@COPR_ID", lCOPR_ID == null ? "" : lCOPR_ID));
                cmd.Parameters.Add(new SqlParameter("@MODEL_ID", MODEL_ID == null ? "" : MODEL_ID));
                cmd.Parameters.Add(new SqlParameter("@POSITION_ID", POSITION_ID == null ? "" : POSITION_ID));
                cmd.Parameters.Add(new SqlParameter("@LINE_ID", LINE_ID == null ? "" : LINE_ID));
                cmd.Parameters.Add(new SqlParameter("@PROC_ID", PROC_ID == null ? "" : PROC_ID));
                cmd.Parameters.Add(new SqlParameter("@SELECT_DATE", SELECT_DATE == null ? "" : SELECT_DATE));
                var reader = await cmd.ExecuteNonQueryAsync();

            }

            return RedirectToAction("Index");
        }
        public async Task<ActionResult> CreateCopFromMacro(
            string COPR_ID,
            string WRK_ID,
            string PROC_ID,
            string MODEL_ID,
            string POSITION_ID,
            string DESC,
            string COP_STATUS,
            string LINE_ID,
            string SELECT_DATE,
            string username,
            List<ITEMSROW> jsonString)
        {
            /*
            COPR16_COPRUNNING cOPR16_COPRUNNING = new COPR16_COPRUNNING();
            cOPR16_COPRUNNING.COPR_ID = COPR_ID;
            cOPR16_COPRUNNING.WRK_ID = WRK_ID;
            cOPR16_COPRUNNING.PROC_ID = PROC_ID;
            cOPR16_COPRUNNING.MODEL_ID = MODEL_ID;
            cOPR16_COPRUNNING.POSITION_ID = POSITION_ID;
            cOPR16_COPRUNNING.DESC = DESC;
            cOPR16_COPRUNNING.COP_STATUS = COP_STATUS;
            cOPR16_COPRUNNING.LINE_ID = LINE_ID;
            cOPR16_COPRUNNING.CRE_BY = username;
            cOPR16_COPRUNNING.ADATE = AppPropModel.today;
            if (ModelState.IsValid)
            {
                db.COPR16_COPRUNNING.Add(cOPR16_COPRUNNING);
                await db.SaveChangesAsync();
            }
            */
            /// Create COP Header

            if (db.Database.Connection.State != ConnectionState.Open)
            {
                await db.Database.Connection.OpenAsync();
            }
            
            List<GetSeqNextValue1_Result> row = db.GetSeqNextValue("COPRUNNING").ToList();

            string lCOPR_ID = row.FirstOrDefault().COPRUN + "-" + PROC_ID + "-00";

            string SQLCMD2 = "exec dbo.sp_CreateNewCop @COPR_ID,@WRK_ID,@PROC_ID,@MODEL_ID,@POSITION_ID,@DESC,@COP_STATUS,@LINE_ID,@CRE_BY";

            using (var cmd = db.Database.Connection.CreateCommand())
            {
                //await db.Database.Connection.OpenAsync();
                cmd.CommandText = SQLCMD2;
                cmd.Parameters.Add(new SqlParameter("@COPR_ID", lCOPR_ID == null ? "" : lCOPR_ID));
                cmd.Parameters.Add(new SqlParameter("@WRK_ID", WRK_ID == null ? "" : WRK_ID));
                cmd.Parameters.Add(new SqlParameter("@PROC_ID", PROC_ID == null ? "" : PROC_ID));
                cmd.Parameters.Add(new SqlParameter("@MODEL_ID", MODEL_ID == null ? "" : MODEL_ID));
                cmd.Parameters.Add(new SqlParameter("@POSITION_ID", POSITION_ID == null ? "" : POSITION_ID));
                cmd.Parameters.Add(new SqlParameter("@DESC", DESC == null ? "" : DESC));
                cmd.Parameters.Add(new SqlParameter("@COP_STATUS", COP_STATUS == null ? "" : COP_STATUS));
                cmd.Parameters.Add(new SqlParameter("@LINE_ID", LINE_ID == null ? "" : LINE_ID));
                cmd.Parameters.Add(new SqlParameter("@CRE_BY", username == null ? "" : username));

                var reader = await cmd.ExecuteNonQueryAsync();

            }
            string strLOTNO = "";
            /** Create COP FG PART**/
            foreach (ITEMSROW item in jsonString)
            {

                COPR16_COPRUNNING_DT dtdt = new COPR16_COPRUNNING_DT();
                dtdt.COPR_ID = lCOPR_ID;
                dtdt.DESC = "";
                dtdt.FGTYPE_ID = item.FGTYPE_ID;
                dtdt.LOTNO = item.LOTNO;
                dtdt.PNO = item.PNO;
                if (item.FGTYPE_ID.Contains("SEATBELT"))
                {
                    strLOTNO = item.LOTNO;
                }
                if (ModelState.IsValid)
                {
                    db.COPR16_COPRUNNING_DT.Add(dtdt);
                    await db.SaveChangesAsync();
                }
            }


            //** Create COP WORKFLOW DETAILS**//
            List<COPR16_WORKFLOW_DT> cOPR16_WORKFLOW_DT_List = await db.COPR16_WORKFLOW_DT.Where(l => l.WRK_ID.Equals(WRK_ID) && l.WRKD_WITH_ID != l.WRKD_SEQ).ToListAsync();

            foreach (COPR16_WORKFLOW_DT row1 in cOPR16_WORKFLOW_DT_List)
            {
                List<COPR16_STEP_MSTR> cOPR16_STEP_MSTR_List = await db.COPR16_STEP_MSTR.Where(l => l.STEP_ID.Equals(row1.STEP_ID)).ToListAsync();
                foreach (COPR16_STEP_MSTR row2 in cOPR16_STEP_MSTR_List)
                {
                    COPR16_MANCTYPE_MSTR cOPR16_MANCTYPE_MSTR = await db.COPR16_MANCTYPE_MSTR.FindAsync(row2.MANC_ID);
                    List<COPR16_RTDT_MSTR> cOPR16_RTDT_MSTR_List = await db.COPR16_RTDT_MSTR.Where(l => l.RTTYPE_ID.Equals(cOPR16_MANCTYPE_MSTR.RTYPE_ID)).ToListAsync();
                    foreach (COPR16_RTDT_MSTR row3 in cOPR16_RTDT_MSTR_List)
                    {
                        COPR16_MEASURETYPE_MSTR cOPR16_MEASURETYPE_MSTR = await db.COPR16_MEASURETYPE_MSTR.FindAsync(row3.MSTYPE_ID);
                        //COPR16_COPRUNNING_RT dtdt = new COPR16_COPRUNNING_RT();
                        List<SqlParameter> VarParams = new List<SqlParameter>();

                        VarParams.Add(new SqlParameter("@COPR_ID", lCOPR_ID));
                        VarParams.Add(new SqlParameter("@PNO", ""));
                        VarParams.Add(new SqlParameter("@FGTYPE_ID", cOPR16_MANCTYPE_MSTR.FGT_ID));
                        VarParams.Add(new SqlParameter("@WRK_ID", WRK_ID));
                        VarParams.Add(new SqlParameter("@WRKD_ID", row1.WRKD_ID));
                        VarParams.Add(new SqlParameter("@MACHINETYPE_ID", cOPR16_MANCTYPE_MSTR.MTYPE_ID));
                        VarParams.Add(new SqlParameter("@RETURNTYPE_ID", row3.RTTYPE_ID));
                        VarParams.Add(new SqlParameter("@SEQ_NO", row1.WRKD_SEQ));
                        VarParams.Add(new SqlParameter("@REV", "00"));
                        VarParams.Add(new SqlParameter("@RTDT_ID", row3.RTDT_ID));
                        VarParams.Add(new SqlParameter("@RTDT_NAME", row3.RTDT_NAME));
                        VarParams.Add(new SqlParameter("@RTDT_REF_ID", row3.REF_ID));


                        //dtdt.COPR_ID = COPR_ID;
                        //    dtdt.FGTYPE_ID = cOPR16_MANCTYPE_MSTR.FGT_ID;
                        //    dtdt.WRK_ID = WRK_ID;
                        //    dtdt.WRKD_ID = row1.WRKD_ID;
                        //    dtdt.MACHINETYPE_ID = cOPR16_MANCTYPE_MSTR.MTYPE_ID;
                        //    dtdt.RETURNTYPE_ID = row3.RTTYPE_ID;
                        //    dtdt.SEQ_NO = row1.WRKD_SEQ;
                        //    dtdt.RTDT_ID = row3.RTDT_ID;
                        //    dtdt.REV = "00";
                        //    dtdt.RTDT_NAME = row3.RTDT_NAME;
                        //    dtdt.RTDT_REF_ID = row3.REF_ID;

                        //if (ModelState.IsValid) {
                        //    db.COPR16_COPRUNNING_RT.Add(dtdt);
                        //    await db.SaveChangesAsync();
                        //}
                        //db.Database.ExecuteSqlCommand("exec GenerateJob @COPR_ID, @PNO, @FGTPE_ID, @WRK_ID, @WRKD_ID, @MACHINETYPE_ID, @RETURNTYPE_ID, @SEQ_NO, @REV, @RTDT_ID, @RTDT_NAME, @RTDT_REF_ID", VarParams.ToList());
                        await db.Database.ExecuteSqlCommandAsync("exec GenerateJob @COPR_ID, @PNO, @FGTYPE_ID, @WRK_ID, @WRKD_ID, @MACHINETYPE_ID, @RETURNTYPE_ID, @SEQ_NO, @REV, @RTDT_ID, @RTDT_NAME, @RTDT_REF_ID",
                            VarParams[0]
                            , VarParams[1]
                            , VarParams[2]
                            , VarParams[3]
                            , VarParams[4]
                            , VarParams[5]
                            , VarParams[6]
                            , VarParams[7]
                            , VarParams[8]
                            , VarParams[9]
                            , VarParams[10]
                            , VarParams[11]
                            );
                    }
                }

            }

            if (db.Database.Connection.State != ConnectionState.Open)
            {
                await db.Database.Connection.OpenAsync();
            }

            using (var cmd = db.Database.Connection.CreateCommand())
            {

                cmd.CommandText = "exec [dbo].[sp_save_selectdate] @COPR_ID,@MODEL_ID,@POSITION_ID,@LINE_ID,@PROC_ID,@SELECT_DATE";
                cmd.Parameters.Add(new SqlParameter("@COPR_ID", lCOPR_ID == null ? "" : lCOPR_ID));
                cmd.Parameters.Add(new SqlParameter("@MODEL_ID", MODEL_ID == null ? "" : MODEL_ID));
                cmd.Parameters.Add(new SqlParameter("@POSITION_ID", POSITION_ID == null ? "" : POSITION_ID));
                cmd.Parameters.Add(new SqlParameter("@LINE_ID", LINE_ID == null ? "" : LINE_ID));
                cmd.Parameters.Add(new SqlParameter("@PROC_ID", PROC_ID == null ? "" : PROC_ID));
                cmd.Parameters.Add(new SqlParameter("@SELECT_DATE", SELECT_DATE == null ? "" : SELECT_DATE));
                var reader = await cmd.ExecuteNonQueryAsync();

            }

            using (var cmd = db.Database.Connection.CreateCommand())
            {
                int ProcID=0;
                if (PROC_ID.Contains("01")) ProcID = 1;
                else ProcID = 2;


                cmd.CommandText = "exec [dbo].[COPR16_UPDATE_COPNO_TO_TRACKING] @DATE,@MODEL,@LINE,@POS,@COP_TYPE,@COPNO,@COPLOT";
                cmd.Parameters.Add(new SqlParameter("@DATE", SELECT_DATE == null ? "" : SELECT_DATE));
                cmd.Parameters.Add(new SqlParameter("@MODEL", MODEL_ID == null ? "" : MODEL_ID));
                cmd.Parameters.Add(new SqlParameter("@LINE", LINE_ID == null ? "" : LINE_ID));
                cmd.Parameters.Add(new SqlParameter("@POS", POSITION_ID == null ? "" : POSITION_ID));
                cmd.Parameters.Add(new SqlParameter("@COP_TYPE", ProcID));
                cmd.Parameters.Add(new SqlParameter("@COPNO", lCOPR_ID == null ? "" : lCOPR_ID));
                cmd.Parameters.Add(new SqlParameter("@COPLOT", strLOTNO == null ? "" : strLOTNO));
                await cmd.ExecuteNonQueryAsync();

            }

            return RedirectToAction("Index");
        }
        public async Task<ActionResult> CreateCopAID(
            string COPR_ID,
            string WRK_ID,
            string PROC_ID,
            string MODEL_ID,
            string POSITION_ID,
            string DESC,
            string COP_STATUS,
            string LINE_ID,
            string SELECT_DATE,
            string username,
            List<ITEMSROW> jsonString)
        {
            /// Create COP Header
            /// 
            JsonResult rowData = new JsonResult();
            try
            {
                if (db.Database.Connection.State != ConnectionState.Open)
                {
                    await db.Database.Connection.OpenAsync();
                }

                //List<GetSeqNextValue1_Result> row = db.GetSeqNextValue("COPRUNNING").ToList();

                //string lCOPR_ID = row.FirstOrDefault().COPRUN + "-" + PROC_ID + "-00";
                string lCOPR_ID = COPR_ID;

                string SQLCMD2 = "exec dbo.sp_CreateNewCop @COPR_ID,@WRK_ID,@PROC_ID,@MODEL_ID,@POSITION_ID,@DESC,@COP_STATUS,@LINE_ID,@CRE_BY";

                using (var cmd = db.Database.Connection.CreateCommand())
                {
                    //await db.Database.Connection.OpenAsync();
                    cmd.CommandText = SQLCMD2;
                    cmd.Parameters.Add(new SqlParameter("@COPR_ID", lCOPR_ID == null ? "" : lCOPR_ID));
                    cmd.Parameters.Add(new SqlParameter("@WRK_ID", WRK_ID == null ? "" : WRK_ID));
                    cmd.Parameters.Add(new SqlParameter("@PROC_ID", PROC_ID == null ? "" : PROC_ID));
                    cmd.Parameters.Add(new SqlParameter("@MODEL_ID", MODEL_ID == null ? "" : MODEL_ID));
                    cmd.Parameters.Add(new SqlParameter("@POSITION_ID", POSITION_ID == null ? "" : POSITION_ID));
                    cmd.Parameters.Add(new SqlParameter("@DESC", DESC == null ? "" : DESC));
                    cmd.Parameters.Add(new SqlParameter("@COP_STATUS", COP_STATUS == null ? "" : COP_STATUS));
                    cmd.Parameters.Add(new SqlParameter("@LINE_ID", LINE_ID == null ? "" : LINE_ID));
                    cmd.Parameters.Add(new SqlParameter("@CRE_BY", username == null ? "" : username));

                    var reader = await cmd.ExecuteNonQueryAsync();

                }
                string strLOTNO = "";
                /** Create COP FG PART**/
                foreach (ITEMSROW item in jsonString)
                {

                    COPR16_COPRUNNING_DT dtdt = new COPR16_COPRUNNING_DT();
                    dtdt.COPR_ID = lCOPR_ID;
                    dtdt.DESC = "";
                    dtdt.FGTYPE_ID = item.FGTYPE_ID;
                    dtdt.LOTNO = item.LOTNO;
                    dtdt.PNO = item.PNO;
                    if (item.FGTYPE_ID.Contains("SEATBELT"))
                    {
                        strLOTNO = item.LOTNO;
                    }
                    if (ModelState.IsValid)
                    {
                        db.COPR16_COPRUNNING_DT.Add(dtdt);
                        await db.SaveChangesAsync();
                    }
                }


                //** Create COP WORKFLOW DETAILS**//
                List<COPR16_WORKFLOW_DT> cOPR16_WORKFLOW_DT_List = await db.COPR16_WORKFLOW_DT.Where(l => l.WRK_ID.Equals(WRK_ID) && l.WRKD_WITH_ID != l.WRKD_SEQ).ToListAsync();

                foreach (COPR16_WORKFLOW_DT row1 in cOPR16_WORKFLOW_DT_List)
                {
                    List<COPR16_STEP_MSTR> cOPR16_STEP_MSTR_List = await db.COPR16_STEP_MSTR.Where(l => l.STEP_ID.Equals(row1.STEP_ID)).ToListAsync();
                    foreach (COPR16_STEP_MSTR row2 in cOPR16_STEP_MSTR_List)
                    {
                        COPR16_MANCTYPE_MSTR cOPR16_MANCTYPE_MSTR = await db.COPR16_MANCTYPE_MSTR.FindAsync(row2.MANC_ID);
                        List<COPR16_RTDT_MSTR> cOPR16_RTDT_MSTR_List = await db.COPR16_RTDT_MSTR.Where(l => l.RTTYPE_ID.Equals(cOPR16_MANCTYPE_MSTR.RTYPE_ID)).ToListAsync();
                        foreach (COPR16_RTDT_MSTR row3 in cOPR16_RTDT_MSTR_List)
                        {
                            COPR16_MEASURETYPE_MSTR cOPR16_MEASURETYPE_MSTR = await db.COPR16_MEASURETYPE_MSTR.FindAsync(row3.MSTYPE_ID);
                            //COPR16_COPRUNNING_RT dtdt = new COPR16_COPRUNNING_RT();
                            List<SqlParameter> VarParams = new List<SqlParameter>();

                            VarParams.Add(new SqlParameter("@COPR_ID", lCOPR_ID));
                            VarParams.Add(new SqlParameter("@PNO", ""));
                            VarParams.Add(new SqlParameter("@FGTYPE_ID", cOPR16_MANCTYPE_MSTR.FGT_ID));
                            VarParams.Add(new SqlParameter("@WRK_ID", WRK_ID));
                            VarParams.Add(new SqlParameter("@WRKD_ID", row1.WRKD_ID));
                            VarParams.Add(new SqlParameter("@MACHINETYPE_ID", cOPR16_MANCTYPE_MSTR.MTYPE_ID));
                            VarParams.Add(new SqlParameter("@RETURNTYPE_ID", row3.RTTYPE_ID));
                            VarParams.Add(new SqlParameter("@SEQ_NO", row1.WRKD_SEQ));
                            VarParams.Add(new SqlParameter("@REV", "00"));
                            VarParams.Add(new SqlParameter("@RTDT_ID", row3.RTDT_ID));
                            VarParams.Add(new SqlParameter("@RTDT_NAME", row3.RTDT_NAME));
                            VarParams.Add(new SqlParameter("@RTDT_REF_ID", row3.REF_ID));
                            await db.Database.ExecuteSqlCommandAsync("exec GenerateJob @COPR_ID, @PNO, @FGTYPE_ID, @WRK_ID, @WRKD_ID, @MACHINETYPE_ID, @RETURNTYPE_ID, @SEQ_NO, @REV, @RTDT_ID, @RTDT_NAME, @RTDT_REF_ID",
                                VarParams[0]
                                , VarParams[1]
                                , VarParams[2]
                                , VarParams[3]
                                , VarParams[4]
                                , VarParams[5]
                                , VarParams[6]
                                , VarParams[7]
                                , VarParams[8]
                                , VarParams[9]
                                , VarParams[10]
                                , VarParams[11]
                                );
                        }
                    }

                }

                if (db.Database.Connection.State != ConnectionState.Open)
                {
                    await db.Database.Connection.OpenAsync();
                }

                using (var cmd = db.Database.Connection.CreateCommand())
                {

                    cmd.CommandText = "exec [dbo].[sp_save_selectdate] @COPR_ID,@MODEL_ID,@POSITION_ID,@LINE_ID,@PROC_ID,@SELECT_DATE";
                    cmd.Parameters.Add(new SqlParameter("@COPR_ID", lCOPR_ID == null ? "" : lCOPR_ID));
                    cmd.Parameters.Add(new SqlParameter("@MODEL_ID", MODEL_ID == null ? "" : MODEL_ID));
                    cmd.Parameters.Add(new SqlParameter("@POSITION_ID", POSITION_ID == null ? "" : POSITION_ID));
                    cmd.Parameters.Add(new SqlParameter("@LINE_ID", LINE_ID == null ? "" : LINE_ID));
                    cmd.Parameters.Add(new SqlParameter("@PROC_ID", PROC_ID == null ? "" : PROC_ID));
                    cmd.Parameters.Add(new SqlParameter("@SELECT_DATE", SELECT_DATE == null ? "" : SELECT_DATE));
                    var reader = await cmd.ExecuteNonQueryAsync();

                }

                using (var cmd = db.Database.Connection.CreateCommand())
                {
                    int ProcID = 0;
                    if (PROC_ID.Contains("01")) ProcID = 1;
                    else ProcID = 2;
                    cmd.CommandText = "exec [dbo].[COPR16_UPDATE_COPNO_TO_TRACKING] @DATE,@MODEL,@LINE,@POS,@COP_TYPE,@COPNO,@COPLOT";
                    cmd.Parameters.Add(new SqlParameter("@DATE", SELECT_DATE == null ? "" : SELECT_DATE));
                    cmd.Parameters.Add(new SqlParameter("@MODEL", MODEL_ID == null ? "" : MODEL_ID));
                    cmd.Parameters.Add(new SqlParameter("@LINE", LINE_ID == null ? "" : LINE_ID));
                    cmd.Parameters.Add(new SqlParameter("@POS", POSITION_ID == null ? "" : POSITION_ID));
                    cmd.Parameters.Add(new SqlParameter("@COP_TYPE", ProcID));
                    cmd.Parameters.Add(new SqlParameter("@COPNO", lCOPR_ID == null ? "" : lCOPR_ID));
                    cmd.Parameters.Add(new SqlParameter("@COPLOT", strLOTNO == null ? "" : strLOTNO));
                    await cmd.ExecuteNonQueryAsync();

                }
                rowData.Data = "0";
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                rowData.Data = e.Message;
            }
            return Json(rowData, JsonRequestBehavior.AllowGet);
            //return RedirectToAction("IndexAID");
        }
        public async Task<ActionResult> CreateCopFromBeforePrint(
            string COPR_ID,
            string WRK_ID,
            string PROC_ID,
            string MODEL_ID,
            string POSITION_ID,
            string DESC,
            string COP_STATUS,
            string LINE_ID,
            string SELECT_DATE,
            string username,
            List<ITEMSROW> jsonString)
        {
            /*
            COPR16_COPRUNNING cOPR16_COPRUNNING = new COPR16_COPRUNNING();
            cOPR16_COPRUNNING.COPR_ID = COPR_ID;
            cOPR16_COPRUNNING.WRK_ID = WRK_ID;
            cOPR16_COPRUNNING.PROC_ID = PROC_ID;
            cOPR16_COPRUNNING.MODEL_ID = MODEL_ID;
            cOPR16_COPRUNNING.POSITION_ID = POSITION_ID;
            cOPR16_COPRUNNING.DESC = DESC;
            cOPR16_COPRUNNING.COP_STATUS = COP_STATUS;
            cOPR16_COPRUNNING.LINE_ID = LINE_ID;
            cOPR16_COPRUNNING.CRE_BY = username;
            cOPR16_COPRUNNING.ADATE = AppPropModel.today;
            if (ModelState.IsValid)
            {
                db.COPR16_COPRUNNING.Add(cOPR16_COPRUNNING);
                await db.SaveChangesAsync();
            }
            */
            /// Create COP Header
            JsonResult rowData = new JsonResult();
            if (db.Database.Connection.State != ConnectionState.Open)
            {
                await db.Database.Connection.OpenAsync();
            }

            List<GetSeqNextValue1_Result> row = db.GetSeqNextValue("COPRUNNING").ToList();

            string lCOPR_ID = row.FirstOrDefault().COPRUN + "-" + PROC_ID + "-00";

            string SQLCMD2 = "exec dbo.sp_CreateNewCop @COPR_ID,@WRK_ID,@PROC_ID,@MODEL_ID,@POSITION_ID,@DESC,@COP_STATUS,@LINE_ID,@CRE_BY";

            using (var cmd = db.Database.Connection.CreateCommand())
            {
                //await db.Database.Connection.OpenAsync();
                cmd.CommandText = SQLCMD2;
                cmd.Parameters.Add(new SqlParameter("@COPR_ID", lCOPR_ID == null ? "" : lCOPR_ID));
                cmd.Parameters.Add(new SqlParameter("@WRK_ID", WRK_ID == null ? "" : WRK_ID));
                cmd.Parameters.Add(new SqlParameter("@PROC_ID", PROC_ID == null ? "" : PROC_ID));
                cmd.Parameters.Add(new SqlParameter("@MODEL_ID", MODEL_ID == null ? "" : MODEL_ID));
                cmd.Parameters.Add(new SqlParameter("@POSITION_ID", POSITION_ID == null ? "" : POSITION_ID));
                cmd.Parameters.Add(new SqlParameter("@DESC", DESC == null ? "" : DESC));
                cmd.Parameters.Add(new SqlParameter("@COP_STATUS", COP_STATUS == null ? "" : COP_STATUS));
                cmd.Parameters.Add(new SqlParameter("@LINE_ID", LINE_ID == null ? "" : LINE_ID));
                cmd.Parameters.Add(new SqlParameter("@CRE_BY", username == null ? "" : username));

                var reader = await cmd.ExecuteNonQueryAsync();

            }
            string strLOTNO = "";
            /** Create COP FG PART**/
            foreach (ITEMSROW item in jsonString)
            {

                COPR16_COPRUNNING_DT dtdt = new COPR16_COPRUNNING_DT();
                dtdt.COPR_ID = lCOPR_ID;
                dtdt.DESC = "";
                dtdt.FGTYPE_ID = item.FGTYPE_ID == null ? "": item.FGTYPE_ID;
                dtdt.LOTNO = item.LOTNO == null ? "" : item.LOTNO;
                dtdt.PNO = item.PNO == null ? "" : item.PNO;
                if (item.FGTYPE_ID.Contains("SEATBELT"))
                {
                    strLOTNO = item.LOTNO == null ? "" : item.LOTNO;
                }
                if (ModelState.IsValid) 
                {
                    db.COPR16_COPRUNNING_DT.Add(dtdt);
                    await db.SaveChangesAsync();
                }
            }


            //** Create COP WORKFLOW DETAILS**//
            List<COPR16_WORKFLOW_DT> cOPR16_WORKFLOW_DT_List = await db.COPR16_WORKFLOW_DT.Where(l => l.WRK_ID.Equals(WRK_ID) && l.WRKD_WITH_ID != l.WRKD_SEQ).ToListAsync();

            foreach (COPR16_WORKFLOW_DT row1 in cOPR16_WORKFLOW_DT_List)
            {
                List<COPR16_STEP_MSTR> cOPR16_STEP_MSTR_List = await db.COPR16_STEP_MSTR.Where(l => l.STEP_ID.Equals(row1.STEP_ID)).ToListAsync();
                foreach (COPR16_STEP_MSTR row2 in cOPR16_STEP_MSTR_List)
                {
                    COPR16_MANCTYPE_MSTR cOPR16_MANCTYPE_MSTR = await db.COPR16_MANCTYPE_MSTR.FindAsync(row2.MANC_ID);
                    List<COPR16_RTDT_MSTR> cOPR16_RTDT_MSTR_List = await db.COPR16_RTDT_MSTR.Where(l => l.RTTYPE_ID.Equals(cOPR16_MANCTYPE_MSTR.RTYPE_ID)).ToListAsync();
                    foreach (COPR16_RTDT_MSTR row3 in cOPR16_RTDT_MSTR_List)
                    {
                        COPR16_MEASURETYPE_MSTR cOPR16_MEASURETYPE_MSTR = await db.COPR16_MEASURETYPE_MSTR.FindAsync(row3.MSTYPE_ID);
                        //COPR16_COPRUNNING_RT dtdt = new COPR16_COPRUNNING_RT();
                        List<SqlParameter> VarParams = new List<SqlParameter>();

                        VarParams.Add(new SqlParameter("@COPR_ID", lCOPR_ID));
                        VarParams.Add(new SqlParameter("@PNO", ""));
                        VarParams.Add(new SqlParameter("@FGTYPE_ID", cOPR16_MANCTYPE_MSTR.FGT_ID));
                        VarParams.Add(new SqlParameter("@WRK_ID", WRK_ID));
                        VarParams.Add(new SqlParameter("@WRKD_ID", row1.WRKD_ID));
                        VarParams.Add(new SqlParameter("@MACHINETYPE_ID", cOPR16_MANCTYPE_MSTR.MTYPE_ID));
                        VarParams.Add(new SqlParameter("@RETURNTYPE_ID", row3.RTTYPE_ID));
                        VarParams.Add(new SqlParameter("@SEQ_NO", row1.WRKD_SEQ));
                        VarParams.Add(new SqlParameter("@REV", "00"));
                        VarParams.Add(new SqlParameter("@RTDT_ID", row3.RTDT_ID));
                        VarParams.Add(new SqlParameter("@RTDT_NAME", row3.RTDT_NAME));
                        VarParams.Add(new SqlParameter("@RTDT_REF_ID", row3.REF_ID));


                        //dtdt.COPR_ID = COPR_ID;
                        //    dtdt.FGTYPE_ID = cOPR16_MANCTYPE_MSTR.FGT_ID;
                        //    dtdt.WRK_ID = WRK_ID;
                        //    dtdt.WRKD_ID = row1.WRKD_ID;
                        //    dtdt.MACHINETYPE_ID = cOPR16_MANCTYPE_MSTR.MTYPE_ID;
                        //    dtdt.RETURNTYPE_ID = row3.RTTYPE_ID;
                        //    dtdt.SEQ_NO = row1.WRKD_SEQ;
                        //    dtdt.RTDT_ID = row3.RTDT_ID;
                        //    dtdt.REV = "00";
                        //    dtdt.RTDT_NAME = row3.RTDT_NAME;
                        //    dtdt.RTDT_REF_ID = row3.REF_ID;

                        //if (ModelState.IsValid) {
                        //    db.COPR16_COPRUNNING_RT.Add(dtdt);
                        //    await db.SaveChangesAsync();
                        //}
                        //db.Database.ExecuteSqlCommand("exec GenerateJob @COPR_ID, @PNO, @FGTPE_ID, @WRK_ID, @WRKD_ID, @MACHINETYPE_ID, @RETURNTYPE_ID, @SEQ_NO, @REV, @RTDT_ID, @RTDT_NAME, @RTDT_REF_ID", VarParams.ToList());
                        await db.Database.ExecuteSqlCommandAsync("exec GenerateJob @COPR_ID, @PNO, @FGTYPE_ID, @WRK_ID, @WRKD_ID, @MACHINETYPE_ID, @RETURNTYPE_ID, @SEQ_NO, @REV, @RTDT_ID, @RTDT_NAME, @RTDT_REF_ID",
                            VarParams[0]
                            , VarParams[1]
                            , VarParams[2]
                            , VarParams[3]
                            , VarParams[4]
                            , VarParams[5]
                            , VarParams[6]
                            , VarParams[7]
                            , VarParams[8]
                            , VarParams[9]
                            , VarParams[10]
                            , VarParams[11]
                            );
                    }
                }

            }

            if (db.Database.Connection.State != ConnectionState.Open)
            {
                await db.Database.Connection.OpenAsync();
            }

            using (var cmd = db.Database.Connection.CreateCommand())
            {

                cmd.CommandText = "exec [dbo].[sp_save_selectdate] @COPR_ID,@MODEL_ID,@POSITION_ID,@LINE_ID,@PROC_ID,@SELECT_DATE";
                cmd.Parameters.Add(new SqlParameter("@COPR_ID", lCOPR_ID == null ? "" : lCOPR_ID));
                cmd.Parameters.Add(new SqlParameter("@MODEL_ID", MODEL_ID == null ? "" : MODEL_ID));
                cmd.Parameters.Add(new SqlParameter("@POSITION_ID", POSITION_ID == null ? "" : POSITION_ID));
                cmd.Parameters.Add(new SqlParameter("@LINE_ID", LINE_ID == null ? "" : LINE_ID));
                cmd.Parameters.Add(new SqlParameter("@PROC_ID", PROC_ID == null ? "" : PROC_ID));
                cmd.Parameters.Add(new SqlParameter("@SELECT_DATE", SELECT_DATE == null ? "" : SELECT_DATE));
                var reader = await cmd.ExecuteNonQueryAsync();

            }

            using (var cmd = db.Database.Connection.CreateCommand())
            {
                int ProcID = 0;
                if (PROC_ID.Contains("01")) ProcID = 1;
                else ProcID = 2;


                cmd.CommandText = "exec [dbo].[COPR16_UPDATE_COPNO_TO_TRACKING] @DATE,@MODEL,@LINE,@POS,@COP_TYPE,@COPNO,@COPLOT";
                cmd.Parameters.Add(new SqlParameter("@DATE", SELECT_DATE == null ? "" : SELECT_DATE));
                cmd.Parameters.Add(new SqlParameter("@MODEL", MODEL_ID == null ? "" : MODEL_ID));
                cmd.Parameters.Add(new SqlParameter("@LINE", LINE_ID == null ? "" : LINE_ID));
                cmd.Parameters.Add(new SqlParameter("@POS", POSITION_ID == null ? "" : POSITION_ID));
                cmd.Parameters.Add(new SqlParameter("@COP_TYPE", ProcID));
                cmd.Parameters.Add(new SqlParameter("@COPNO", lCOPR_ID == null ? "" : lCOPR_ID));
                cmd.Parameters.Add(new SqlParameter("@COPLOT", strLOTNO == null ? "" : strLOTNO));
                await cmd.ExecuteNonQueryAsync();

            }

            //return RedirectToAction("Index");
            //return row;
            rowData.Data = lCOPR_ID;
            return Json(rowData, JsonRequestBehavior.AllowGet);
        }
        public async Task<ActionResult> CreateCopAIDFromBeforePrint(
            string COPR_ID,
            string WRK_ID,
            string PROC_ID,
            string MODEL_ID,
            string POSITION_ID,
            string DESC,
            string COP_STATUS,
            string LINE_ID,
            string SELECT_DATE,
            string username,
            List<ITEMSROW> jsonString)
        {
            /*
            COPR16_COPRUNNING cOPR16_COPRUNNING = new COPR16_COPRUNNING();
            cOPR16_COPRUNNING.COPR_ID = COPR_ID;
            cOPR16_COPRUNNING.WRK_ID = WRK_ID;
            cOPR16_COPRUNNING.PROC_ID = PROC_ID;
            cOPR16_COPRUNNING.MODEL_ID = MODEL_ID;
            cOPR16_COPRUNNING.POSITION_ID = POSITION_ID;
            cOPR16_COPRUNNING.DESC = DESC;
            cOPR16_COPRUNNING.COP_STATUS = COP_STATUS;
            cOPR16_COPRUNNING.LINE_ID = LINE_ID;
            cOPR16_COPRUNNING.CRE_BY = username;
            cOPR16_COPRUNNING.ADATE = AppPropModel.today;
            if (ModelState.IsValid)
            {
                db.COPR16_COPRUNNING.Add(cOPR16_COPRUNNING);
                await db.SaveChangesAsync();
            }
            */
            /// Create COP Header
            JsonResult rowData = new JsonResult();
            if (db.Database.Connection.State != ConnectionState.Open)
            {
                await db.Database.Connection.OpenAsync();
            }

            List<GetSeqNextValue1_Result> row = db.GetSeqNextValue("COPRUNNING").ToList();

            string lCOPR_ID = row.FirstOrDefault().COPRUN + "-" + PROC_ID + "-00";

            string SQLCMD2 = "exec dbo.sp_CreateNewCop @COPR_ID,@WRK_ID,@PROC_ID,@MODEL_ID,@POSITION_ID,@DESC,@COP_STATUS,@LINE_ID,@CRE_BY";

            using (var cmd = db.Database.Connection.CreateCommand())
            {
                //await db.Database.Connection.OpenAsync();
                cmd.CommandText = SQLCMD2;
                cmd.Parameters.Add(new SqlParameter("@COPR_ID", lCOPR_ID == null ? "" : lCOPR_ID));
                cmd.Parameters.Add(new SqlParameter("@WRK_ID", WRK_ID == null ? "" : WRK_ID));
                cmd.Parameters.Add(new SqlParameter("@PROC_ID", PROC_ID == null ? "" : PROC_ID));
                cmd.Parameters.Add(new SqlParameter("@MODEL_ID", MODEL_ID == null ? "" : MODEL_ID));
                cmd.Parameters.Add(new SqlParameter("@POSITION_ID", POSITION_ID == null ? "" : POSITION_ID));
                cmd.Parameters.Add(new SqlParameter("@DESC", DESC == null ? "" : DESC));
                cmd.Parameters.Add(new SqlParameter("@COP_STATUS", COP_STATUS == null ? "" : COP_STATUS));
                cmd.Parameters.Add(new SqlParameter("@LINE_ID", LINE_ID == null ? "" : LINE_ID));
                cmd.Parameters.Add(new SqlParameter("@CRE_BY", username == null ? "" : username));

                var reader = await cmd.ExecuteNonQueryAsync();

            }
            string strLOTNO = "";
            /** Create COP FG PART**/
            foreach (ITEMSROW item in jsonString)
            {

                COPR16_COPRUNNING_DT dtdt = new COPR16_COPRUNNING_DT();
                dtdt.COPR_ID = lCOPR_ID;
                dtdt.DESC = "";
                dtdt.FGTYPE_ID = item.FGTYPE_ID == null ? "" : item.FGTYPE_ID;
                dtdt.LOTNO = item.LOTNO == null ? "" : item.LOTNO;
                dtdt.PNO = item.PNO == null ? "" : item.PNO;
                if (item.FGTYPE_ID.Contains("SEATBELT"))
                {
                    strLOTNO = item.LOTNO == null ? "" : item.LOTNO;
                }
                if (ModelState.IsValid)
                {
                    db.COPR16_COPRUNNING_DT.Add(dtdt);
                    await db.SaveChangesAsync();
                }
            }


            //** Create COP WORKFLOW DETAILS**//
            List<COPR16_WORKFLOW_DT> cOPR16_WORKFLOW_DT_List = await db.COPR16_WORKFLOW_DT.Where(l => l.WRK_ID.Equals(WRK_ID) && l.WRKD_WITH_ID != l.WRKD_SEQ).ToListAsync();

            foreach (COPR16_WORKFLOW_DT row1 in cOPR16_WORKFLOW_DT_List)
            {
                List<COPR16_STEP_MSTR> cOPR16_STEP_MSTR_List = await db.COPR16_STEP_MSTR.Where(l => l.STEP_ID.Equals(row1.STEP_ID)).ToListAsync();
                foreach (COPR16_STEP_MSTR row2 in cOPR16_STEP_MSTR_List)
                {
                    COPR16_MANCTYPE_MSTR cOPR16_MANCTYPE_MSTR = await db.COPR16_MANCTYPE_MSTR.FindAsync(row2.MANC_ID);
                    List<COPR16_RTDT_MSTR> cOPR16_RTDT_MSTR_List = await db.COPR16_RTDT_MSTR.Where(l => l.RTTYPE_ID.Equals(cOPR16_MANCTYPE_MSTR.RTYPE_ID)).ToListAsync();
                    foreach (COPR16_RTDT_MSTR row3 in cOPR16_RTDT_MSTR_List)
                    {
                        COPR16_MEASURETYPE_MSTR cOPR16_MEASURETYPE_MSTR = await db.COPR16_MEASURETYPE_MSTR.FindAsync(row3.MSTYPE_ID);
                        //COPR16_COPRUNNING_RT dtdt = new COPR16_COPRUNNING_RT();
                        List<SqlParameter> VarParams = new List<SqlParameter>();

                        VarParams.Add(new SqlParameter("@COPR_ID", lCOPR_ID));
                        VarParams.Add(new SqlParameter("@PNO", ""));
                        VarParams.Add(new SqlParameter("@FGTYPE_ID", cOPR16_MANCTYPE_MSTR.FGT_ID));
                        VarParams.Add(new SqlParameter("@WRK_ID", WRK_ID));
                        VarParams.Add(new SqlParameter("@WRKD_ID", row1.WRKD_ID));
                        VarParams.Add(new SqlParameter("@MACHINETYPE_ID", cOPR16_MANCTYPE_MSTR.MTYPE_ID));
                        VarParams.Add(new SqlParameter("@RETURNTYPE_ID", row3.RTTYPE_ID));
                        VarParams.Add(new SqlParameter("@SEQ_NO", row1.WRKD_SEQ));
                        VarParams.Add(new SqlParameter("@REV", "00"));
                        VarParams.Add(new SqlParameter("@RTDT_ID", row3.RTDT_ID));
                        VarParams.Add(new SqlParameter("@RTDT_NAME", row3.RTDT_NAME));
                        VarParams.Add(new SqlParameter("@RTDT_REF_ID", row3.REF_ID));


                        //dtdt.COPR_ID = COPR_ID;
                        //    dtdt.FGTYPE_ID = cOPR16_MANCTYPE_MSTR.FGT_ID;
                        //    dtdt.WRK_ID = WRK_ID;
                        //    dtdt.WRKD_ID = row1.WRKD_ID;
                        //    dtdt.MACHINETYPE_ID = cOPR16_MANCTYPE_MSTR.MTYPE_ID;
                        //    dtdt.RETURNTYPE_ID = row3.RTTYPE_ID;
                        //    dtdt.SEQ_NO = row1.WRKD_SEQ;
                        //    dtdt.RTDT_ID = row3.RTDT_ID;
                        //    dtdt.REV = "00";
                        //    dtdt.RTDT_NAME = row3.RTDT_NAME;
                        //    dtdt.RTDT_REF_ID = row3.REF_ID;

                        //if (ModelState.IsValid) {
                        //    db.COPR16_COPRUNNING_RT.Add(dtdt);
                        //    await db.SaveChangesAsync();
                        //}
                        //db.Database.ExecuteSqlCommand("exec GenerateJob @COPR_ID, @PNO, @FGTPE_ID, @WRK_ID, @WRKD_ID, @MACHINETYPE_ID, @RETURNTYPE_ID, @SEQ_NO, @REV, @RTDT_ID, @RTDT_NAME, @RTDT_REF_ID", VarParams.ToList());
                        await db.Database.ExecuteSqlCommandAsync("exec GenerateJob @COPR_ID, @PNO, @FGTYPE_ID, @WRK_ID, @WRKD_ID, @MACHINETYPE_ID, @RETURNTYPE_ID, @SEQ_NO, @REV, @RTDT_ID, @RTDT_NAME, @RTDT_REF_ID",
                            VarParams[0]
                            , VarParams[1]
                            , VarParams[2]
                            , VarParams[3]
                            , VarParams[4]
                            , VarParams[5]
                            , VarParams[6]
                            , VarParams[7]
                            , VarParams[8]
                            , VarParams[9]
                            , VarParams[10]
                            , VarParams[11]
                            );
                    }
                }

            }

            if (db.Database.Connection.State != ConnectionState.Open)
            {
                await db.Database.Connection.OpenAsync();
            }

            using (var cmd = db.Database.Connection.CreateCommand())
            {

                cmd.CommandText = "exec [dbo].[sp_save_selectdate] @COPR_ID,@MODEL_ID,@POSITION_ID,@LINE_ID,@PROC_ID,@SELECT_DATE";
                cmd.Parameters.Add(new SqlParameter("@COPR_ID", lCOPR_ID == null ? "" : lCOPR_ID));
                cmd.Parameters.Add(new SqlParameter("@MODEL_ID", MODEL_ID == null ? "" : MODEL_ID));
                cmd.Parameters.Add(new SqlParameter("@POSITION_ID", POSITION_ID == null ? "" : POSITION_ID));
                cmd.Parameters.Add(new SqlParameter("@LINE_ID", LINE_ID == null ? "" : LINE_ID));
                cmd.Parameters.Add(new SqlParameter("@PROC_ID", PROC_ID == null ? "" : PROC_ID));
                cmd.Parameters.Add(new SqlParameter("@SELECT_DATE", SELECT_DATE == null ? "" : SELECT_DATE));
                var reader = await cmd.ExecuteNonQueryAsync();

            }

            using (var cmd = db.Database.Connection.CreateCommand())
            {
                int ProcID = 0;
                if (PROC_ID.Contains("01")) ProcID = 1;
                else ProcID = 2;


                cmd.CommandText = "exec [dbo].[COPR16_UPDATE_COPNO_TO_TRACKING] @DATE,@MODEL,@LINE,@POS,@COP_TYPE,@COPNO,@COPLOT";
                cmd.Parameters.Add(new SqlParameter("@DATE", SELECT_DATE == null ? "" : SELECT_DATE));
                cmd.Parameters.Add(new SqlParameter("@MODEL", MODEL_ID == null ? "" : MODEL_ID));
                cmd.Parameters.Add(new SqlParameter("@LINE", LINE_ID == null ? "" : LINE_ID));
                cmd.Parameters.Add(new SqlParameter("@POS", POSITION_ID == null ? "" : POSITION_ID));
                cmd.Parameters.Add(new SqlParameter("@COP_TYPE", ProcID));
                cmd.Parameters.Add(new SqlParameter("@COPNO", lCOPR_ID == null ? "" : lCOPR_ID));
                cmd.Parameters.Add(new SqlParameter("@COPLOT", strLOTNO == null ? "" : strLOTNO));
                await cmd.ExecuteNonQueryAsync();

            }

            //return RedirectToAction("Index");
            //return row;
            rowData.Data = lCOPR_ID;
            return Json(rowData, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> checkCOPNO(string SELECTED_COPNO)
        {
            /*
            string SQLCMD = "SELECT * FROM COPR16_COPRUNNING WHERE COP_STATUS IN ('READY','TESTING') ORDER BY COPR_ID DESC";
            //model.cOPR16_COPRUNNING_List = await db.COPR16_COPRUNNING.Where(l => l.COP_STATUS.Equals("READY") || l.COP_STATUS.Equals("TESTING")).ToListAsync();
            model.cOPR16_COPRUNNING_List = await db.COPR16_COPRUNNING.SqlQuery(SQLCMD).ToListAsync();
            foreach (var rows in model.cOPR16_COPRUNNING_List)
            {
                COPR16_POSITION_MSTR posName = await db.COPR16_POSITION_MSTR.Where(l => l.POS_ID == rows.POSITION_ID).FirstAsync();
                COPR16_MODEL_MSTR modelName = await db.COPR16_MODEL_MSTR.Where(l => l.MODEL_ID == rows.MODEL_ID).FirstAsync();

                rows.POSITION_ID = posName.POS_DESC;
                rows.MODEL_ID = modelName.MODEL_DESC;

            }
            //model.cOPR16_COPRUNNING_List = model.cOPR16_COPRUNNING_List.OrderByDescending(l => l.COPR_ID).ToList();
            model.statusCode = "";
             */
            if (db.Database.Connection.State != ConnectionState.Open)
            {
                await db.Database.Connection.OpenAsync();
            }

            JsonResult rowData = new JsonResult();

            COPR16_COPRUNNING cOPR16_COPRUNNING = new COPR16_COPRUNNING();
            using (var cmd = db.Database.Connection.CreateCommand())
            {
                cmd.CommandText = "SELECT * FROM COPR16_COPRUNNING WHERE COP_STATUS IN ('READY','TESTING') AND COPR_ID = @COPR_ID";
                cmd.Parameters.Add(new SqlParameter("@COPR_ID", SELECTED_COPNO == null ? "" : SELECTED_COPNO));
                DbDataReader readerEx = await cmd.ExecuteReaderAsync();
                if (readerEx.HasRows)
                {
                    Type type = typeof(COPR16_COPRUNNING);
                    var accessor = TypeAccessor.Create(type);
                    while (readerEx.Read())
                    {
                        for (int i = 0; i < readerEx.FieldCount; i++)
                        {
                            object value = readerEx[readerEx.GetName(i)];
                            Type valueType = readerEx[readerEx.GetName(i)].GetType();
                            if (value != DBNull.Value)
                            {
                                if (!Utils.IsNullableType(valueType))
                                {
                                    accessor[cOPR16_COPRUNNING, readerEx.GetName(i)] = readerEx[readerEx.GetName(i)];
                                }
                            }


                        }

                    }
                }
                readerEx.Close();

            }

            rowData.Data = cOPR16_COPRUNNING;
            return Json(rowData, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public async Task<ActionResult> SPC_PARTS_LIST(string MODEL, string YEAR, string MONTH, string SEC)
        {
            //-- exec COPR16_SPC_GET_PARTS_28MAR2021 '2GM',11,2020,'51'
            JsonResult rowData = new JsonResult();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DatabaseServer"].ToString()))
            {
                if (con.State != ConnectionState.Open)
                {
                    await con.OpenAsync();
                }
                
                string SQLCMD = "exec dbo.COPR16_SPC_GET_PARTS_28MAR2021 @MODEL,@MONTH,@YEAR,@SEC";

                using (var cmd = con.CreateCommand())
                {

                    cmd.CommandText = SQLCMD;
                    cmd.Parameters.Add(new SqlParameter("@MODEL", MODEL ));
                    cmd.Parameters.Add(new SqlParameter("@MONTH", Convert.ToInt16(MONTH ?? "")));
                    cmd.Parameters.Add(new SqlParameter("@YEAR", Convert.ToInt16( YEAR ??"")));
                    cmd.Parameters.Add(new SqlParameter("@SEC", SEC));
                    DbDataReader reader = await cmd.ExecuteReaderAsync();
                    {
                        var model = Serialize((SqlDataReader)reader);
                        rowData.Data = model;
                    }
                    reader.Close();
                }
                con.Close();
            }
            //rowData.Data = cOPR16_COPRUNNING;
            return Json(rowData, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        // GET: COPR16_COPRUNNING/Edit/5
        public async Task<ActionResult> Edit(string SELECTED_COPNO, string COPNO, string statusCode, string FROM_DATE, string TO_DATE)
        {
            if (SELECTED_COPNO == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            COPR16_COPRUNNING cOPR16_COPRUNNING = new COPR16_COPRUNNING();
            

            CopRunningModel model = new CopRunningModel(db);

            using (var cmd = db.Database.Connection.CreateCommand())
            {
                if (db.Database.Connection.State != ConnectionState.Open)
                {
                    await db.Database.Connection.OpenAsync();
                }
                
                cmd.CommandText = "SELECT * FROM COPR16_COPRUNNING WHERE COPR_ID = @COPR_ID";
                cmd.Parameters.Add(new SqlParameter("@COPR_ID", SELECTED_COPNO == null ? "" : SELECTED_COPNO));
                DbDataReader readerEx = await cmd.ExecuteReaderAsync();
                if (readerEx.HasRows)
                {
                    Type type = typeof(COPR16_COPRUNNING);
                    var accessor = TypeAccessor.Create(type);
                    while (readerEx.Read())
                    {
                        for (int i = 0; i < readerEx.FieldCount; i++)
                        {
                            object value = readerEx[readerEx.GetName(i)];
                            Type valueType = readerEx[readerEx.GetName(i)].GetType();
                            if (value != DBNull.Value)
                            {
                                if (!Utils.IsNullableType(valueType))
                                {
                                    accessor[cOPR16_COPRUNNING, readerEx.GetName(i)] = readerEx[readerEx.GetName(i)];
                                }
                            }


                        }

                    }
                }
                readerEx.Close();
                
            }

            model.stDate = FROM_DATE;
            model.enDate = TO_DATE;
            model.statusCode = statusCode;
            model.COPNO = SELECTED_COPNO;

            model.cOPR16_COPRUNNING = cOPR16_COPRUNNING;
            model.cOPR16_COPRUNNING_DT_List = await db.COPR16_COPRUNNING_DT.Where(l => l.COPR_ID.Equals(SELECTED_COPNO)).ToListAsync();
            model.cOPR16_COPRUNNING_DT_SB = model.cOPR16_COPRUNNING_DT_List.Where(l => l.FGTYPE_ID.Equals("SEATBELT")).FirstOrDefault();
            model.cOPR16_COPRUNNING_DT_BKL = model.cOPR16_COPRUNNING_DT_List.Where(l => l.FGTYPE_ID.Equals("BUNKLE")).FirstOrDefault();
            string SQLCMD = "select * from ( SELECT A.COPR_ID, A.PNO, A.FGTYPE_ID, A.WRK_ID,A.WRKD_ID," + 
            "CONCAT(A.MACHINETYPE_ID, ': ', C.MTYPE_NAME) MACHINETYPE_ID, " +
            "A.RETURNTYPE_ID, A.[VALUE], A.STATUS_ID,  A.TEST_START,  A.TEST_FINISH, " +
            "A.TEST_BY, A.REV, A.SEQ_NO, " +
            "CONCAT(A.MACHINE_ID, ': ', B.MANC_DESC) MACHINE_ID, A.CLOSE_BY, A.RTDT_ID, A.RTDT_NAME, D.UNIT_TEXT AS RTDT_REF_ID " +
            "FROM COPR16_COPRUNNING_RT A " +
            "LEFT OUTER JOIN COPR16_MACHINE_MSTR B ON A.MACHINE_ID = B.MANC_ID AND A.MACHINE_ID is not null " +
            "LEFT OUTER JOIN COPR16_MANCTYPE_MSTR C ON A.MACHINETYPE_ID = C.MTYPE_ID " +
            "LEFT OUTER JOIN [dbo].[COPR16_UNITS_MSTR] D ON A.RTDT_REF_ID = D.UNIT_ID " +
            "WHERE COPR_ID='" + SELECTED_COPNO + "' " +
            ") x " +
            "" +
            "ORDER BY CAST(x.SEQ_NO AS INT);";
            //model.cOPR16_COPRUNNING_RT_List = db.COPR16_COPRUNNING_RT.SqlQuery(SQLCMD).ToList();
            //model.cOPR16_COPRUNNING_RT_List = await db.COPR16_COPRUNNING_RT.Where(l => l.COPR_ID.Equals(id)).ToListAsync();
            model.cOPR16_COPRUNNING_RT_List = db.Database.SqlQuery<COPR16_COPRUNNING_RT>(SQLCMD).ToList();
            model.SelectedCOPNO = SELECTED_COPNO;
            if (cOPR16_COPRUNNING == null)
            {
                return HttpNotFound();
            }

            string SQLCMD2 = "exec [dbo].[sp_list_rst_header_by_copr_no] @COPR_ID";

            using (var cmd = db.Database.Connection.CreateCommand())
            {
                if (db.Database.Connection.State != ConnectionState.Open)
                {
                    await db.Database.Connection.OpenAsync();
                }
                cmd.CommandText = SQLCMD2;
                cmd.Parameters.Add(new SqlParameter("@COPR_ID", SELECTED_COPNO == null ? "" : SELECTED_COPNO));
                DbDataReader reader = await cmd.ExecuteReaderAsync();
                {
                    var rst_head = Serialize((SqlDataReader)reader);
                    if (rst_head.Count() > 0)
                    {
                        var temp = rst_head.FirstOrDefault();
                        model.rST_HEADER = GetObject<RST_HEADER>(temp);
                    }
                    
                }
                reader.Close();
            }
            return View(model);
        }
        public async Task<ActionResult> EditAID(string SELECTED_COPNO, string COPNO, string statusCode, string FROM_DATE, string TO_DATE)
        {
            if (SELECTED_COPNO == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            COPR16_COPRUNNING cOPR16_COPRUNNING = new COPR16_COPRUNNING();


            CopRunningModel model = new CopRunningModel(db);

            using (var cmd = db.Database.Connection.CreateCommand())
            {
                if (db.Database.Connection.State != ConnectionState.Open)
                {
                    await db.Database.Connection.OpenAsync();
                }

                cmd.CommandText = "SELECT * FROM COPR16_COPRUNNING WHERE COPR_ID = @COPR_ID";
                cmd.Parameters.Add(new SqlParameter("@COPR_ID", SELECTED_COPNO == null ? "" : SELECTED_COPNO));
                DbDataReader readerEx = await cmd.ExecuteReaderAsync();
                if (readerEx.HasRows)
                {
                    Type type = typeof(COPR16_COPRUNNING);
                    var accessor = TypeAccessor.Create(type);
                    while (readerEx.Read())
                    {
                        for (int i = 0; i < readerEx.FieldCount; i++)
                        {
                            object value = readerEx[readerEx.GetName(i)];
                            Type valueType = readerEx[readerEx.GetName(i)].GetType();
                            if (value != DBNull.Value)
                            {
                                if (!Utils.IsNullableType(valueType))
                                {
                                    accessor[cOPR16_COPRUNNING, readerEx.GetName(i)] = readerEx[readerEx.GetName(i)];
                                }
                            }


                        }

                    }
                }
                readerEx.Close();

            }

            model.stDate = FROM_DATE;
            model.enDate = TO_DATE;
            model.statusCode = statusCode;
            model.COPNO = SELECTED_COPNO;

            model.cOPR16_COPRUNNING = cOPR16_COPRUNNING;
            model.cOPR16_COPRUNNING_DT_List = await db.COPR16_COPRUNNING_DT.Where(l => l.COPR_ID.Equals(SELECTED_COPNO)).ToListAsync();
            model.cOPR16_COPRUNNING_DT_SB = model.cOPR16_COPRUNNING_DT_List.Where(l => l.FGTYPE_ID.Equals("SEATBELT")).FirstOrDefault();
            model.cOPR16_COPRUNNING_DT_BKL = model.cOPR16_COPRUNNING_DT_List.Where(l => l.FGTYPE_ID.Equals("BUNKLE")).FirstOrDefault();
            model.cOPR16_COPRUNNING_DT_BKL2 = model.cOPR16_COPRUNNING_DT_List.Where(l => l.FGTYPE_ID.Equals("BUNKLE2")).FirstOrDefault();

            string SQLCMD = "select * from ( SELECT A.COPR_ID, A.PNO, A.FGTYPE_ID, A.WRK_ID,A.WRKD_ID," +
            "CONCAT(A.MACHINETYPE_ID, ': ', C.MTYPE_NAME) MACHINETYPE_ID, " +
            "A.RETURNTYPE_ID, A.[VALUE], A.STATUS_ID,  A.TEST_START,  A.TEST_FINISH, " +
            "A.TEST_BY, A.REV, A.SEQ_NO, " +
            "CONCAT(A.MACHINE_ID, ': ', B.MANC_DESC) MACHINE_ID, A.CLOSE_BY, A.RTDT_ID, A.RTDT_NAME, D.UNIT_TEXT AS RTDT_REF_ID " +
            "FROM COPR16_COPRUNNING_RT A " +
            "LEFT OUTER JOIN COPR16_MACHINE_MSTR B ON A.MACHINE_ID = B.MANC_ID AND A.MACHINE_ID is not null " +
            "LEFT OUTER JOIN COPR16_MANCTYPE_MSTR C ON A.MACHINETYPE_ID = C.MTYPE_ID " +
            "LEFT OUTER JOIN [dbo].[COPR16_UNITS_MSTR] D ON A.RTDT_REF_ID = D.UNIT_ID " +
            "WHERE COPR_ID='" + SELECTED_COPNO + "' " +
            ") x " +
            "" +
            "ORDER BY CAST(x.SEQ_NO AS INT);";
            //model.cOPR16_COPRUNNING_RT_List = db.COPR16_COPRUNNING_RT.SqlQuery(SQLCMD).ToList();
            //model.cOPR16_COPRUNNING_RT_List = await db.COPR16_COPRUNNING_RT.Where(l => l.COPR_ID.Equals(id)).ToListAsync();
            model.cOPR16_COPRUNNING_RT_List = db.Database.SqlQuery<COPR16_COPRUNNING_RT>(SQLCMD).ToList();
            model.SelectedCOPNO = SELECTED_COPNO;
            if (cOPR16_COPRUNNING == null)
            {
                return HttpNotFound();
            }

            string SQLCMD2 = "exec [dbo].[sp_list_rst_header_by_copr_no] @COPR_ID";

            using (var cmd = db.Database.Connection.CreateCommand())
            {
                if (db.Database.Connection.State != ConnectionState.Open)
                {
                    await db.Database.Connection.OpenAsync();
                }
                cmd.CommandText = SQLCMD2;
                cmd.Parameters.Add(new SqlParameter("@COPR_ID", SELECTED_COPNO == null ? "" : SELECTED_COPNO));
                DbDataReader reader = await cmd.ExecuteReaderAsync();
                {
                    var rst_head = Serialize((SqlDataReader)reader);
                    if (rst_head.Count() > 0)
                    {
                        var temp = rst_head.FirstOrDefault();
                        model.rST_HEADER = GetObject<RST_HEADER>(temp);
                    }

                }
                reader.Close();
            }
            return View(model);
        }
        public async Task<ActionResult> EditNONEC(string SELECTED_COPNO, string COPNO, string statusCode, string FROM_DATE, string TO_DATE)
        {
            if (SELECTED_COPNO == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            COPR16_COPRUNNING cOPR16_COPRUNNING = new COPR16_COPRUNNING();


            CopRunningModel model = new CopRunningModel(db);

            using (var cmd = db.Database.Connection.CreateCommand())
            {
                if (db.Database.Connection.State != ConnectionState.Open)
                {
                    await db.Database.Connection.OpenAsync();
                }

                cmd.CommandText = "SELECT * FROM COPR16_COPRUNNING WHERE COPR_ID = @COPR_ID";
                cmd.Parameters.Add(new SqlParameter("@COPR_ID", SELECTED_COPNO == null ? "" : SELECTED_COPNO));
                DbDataReader readerEx = await cmd.ExecuteReaderAsync();
                if (readerEx.HasRows)
                {
                    Type type = typeof(COPR16_COPRUNNING);
                    var accessor = TypeAccessor.Create(type);
                    while (readerEx.Read())
                    {
                        for (int i = 0; i < readerEx.FieldCount; i++)
                        {
                            object value = readerEx[readerEx.GetName(i)];
                            Type valueType = readerEx[readerEx.GetName(i)].GetType();
                            if (value != DBNull.Value)
                            {
                                if (!Utils.IsNullableType(valueType))
                                {
                                    accessor[cOPR16_COPRUNNING, readerEx.GetName(i)] = readerEx[readerEx.GetName(i)];
                                }
                            }


                        }

                    }
                }
                readerEx.Close();

            }

            model.stDate = FROM_DATE;
            model.enDate = TO_DATE;
            model.statusCode = statusCode;
            model.COPNO = SELECTED_COPNO;

            model.cOPR16_COPRUNNING = cOPR16_COPRUNNING;
            model.cOPR16_COPRUNNING_DT_List = await db.COPR16_COPRUNNING_DT.Where(l => l.COPR_ID.Equals(SELECTED_COPNO)).ToListAsync();
            model.cOPR16_COPRUNNING_DT_SB = model.cOPR16_COPRUNNING_DT_List.Where(l => l.FGTYPE_ID.Equals("SEATBELT")).FirstOrDefault();
            model.cOPR16_COPRUNNING_DT_BKL = model.cOPR16_COPRUNNING_DT_List.Where(l => l.FGTYPE_ID.Equals("BUNKLE")).FirstOrDefault();
            string SQLCMD = "select * from ( SELECT A.COPR_ID, A.PNO, A.FGTYPE_ID, A.WRK_ID,A.WRKD_ID," +
            "CONCAT(A.MACHINETYPE_ID, ': ', C.MTYPE_NAME) MACHINETYPE_ID, " +
            "A.RETURNTYPE_ID, A.[VALUE], A.STATUS_ID,  A.TEST_START,  A.TEST_FINISH, " +
            "A.TEST_BY, A.REV, A.SEQ_NO, " +
            "CONCAT(A.MACHINE_ID, ': ', B.MANC_DESC) MACHINE_ID, A.CLOSE_BY, A.RTDT_ID, A.RTDT_NAME, D.UNIT_TEXT AS RTDT_REF_ID " +
            "FROM COPR16_COPRUNNING_RT A " +
            "LEFT OUTER JOIN COPR16_MACHINE_MSTR B ON A.MACHINE_ID = B.MANC_ID AND A.MACHINE_ID is not null " +
            "LEFT OUTER JOIN COPR16_MANCTYPE_MSTR C ON A.MACHINETYPE_ID = C.MTYPE_ID " +
            "LEFT OUTER JOIN [dbo].[COPR16_UNITS_MSTR] D ON A.RTDT_REF_ID = D.UNIT_ID " +
            "WHERE COPR_ID='" + SELECTED_COPNO + "' " +
            ") x " +
            "" +
            "ORDER BY CAST(x.SEQ_NO AS INT);";
            //model.cOPR16_COPRUNNING_RT_List = db.COPR16_COPRUNNING_RT.SqlQuery(SQLCMD).ToList();
            //model.cOPR16_COPRUNNING_RT_List = await db.COPR16_COPRUNNING_RT.Where(l => l.COPR_ID.Equals(id)).ToListAsync();
            model.cOPR16_COPRUNNING_RT_List = db.Database.SqlQuery<COPR16_COPRUNNING_RT>(SQLCMD).ToList();
            model.SelectedCOPNO = SELECTED_COPNO;
            if (cOPR16_COPRUNNING == null)
            {
                return HttpNotFound();
            }

            string SQLCMD2 = "exec [dbo].[sp_list_rst_header_by_copr_no] @COPR_ID";

            using (var cmd = db.Database.Connection.CreateCommand())
            {
                if (db.Database.Connection.State != ConnectionState.Open)
                {
                    await db.Database.Connection.OpenAsync();
                }
                cmd.CommandText = SQLCMD2;
                cmd.Parameters.Add(new SqlParameter("@COPR_ID", SELECTED_COPNO == null ? "" : SELECTED_COPNO));
                DbDataReader reader = await cmd.ExecuteReaderAsync();
                {
                    var rst_head = Serialize((SqlDataReader)reader);
                    if (rst_head.Count() > 0)
                    {
                        var temp = rst_head.FirstOrDefault();
                        model.rST_HEADER = GetObject<RST_HEADER>(temp);
                    }

                }
                reader.Close();
            }
            return View(model);
        }
        public T GetObject<T>(Dictionary<string, object> dict)
        {
            Type type = typeof(T);
            var obj = Activator.CreateInstance(type);

            foreach (var kv in dict)
            {
                if (DBNull.Value.Equals(kv.Value))
                {
                    type.GetProperty(kv.Key).SetValue(obj, null);
                }
                else
                {
                    type.GetProperty(kv.Key).SetValue(obj, kv.Value);
                }
                
            }
            return (T)obj;
        }
        // POST: COPR16_COPRUNNING/SaveJson/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //        public async Task<ActionResult> Edit([Bind(Include = "COPR_ID,MODEL_ID,POSITION_ID,FGTYPE_ID,WRK_ID,DESC,ADATE,CRE_BY,MOD_DATE,MOD_BY,COP_STATUS")] COPR16_COPRUNNING cOPR16_COPRUNNING)
        public async Task<ActionResult> SaveJson(
            string COPR_ID,
            string WRK_ID,
            string PROC_ID,
            string MODEL_ID,
            string POSITION_ID,
            string DESC,
            string COP_STATUS,
            string LINE_ID,
            string username,
            List<ITEMSROW> jsonString
        )
        {
            //COPR16_COPRUNNING cOPR16_COPRUNNING = new COPR16_COPRUNNING();
            COPR16_COPRUNNING cOPR16_COPRUNNING = await db.COPR16_COPRUNNING.FindAsync(COPR_ID);
            //cOPR16_COPRUNNING.COPR_ID = COPR_ID;
            cOPR16_COPRUNNING.WRK_ID = WRK_ID;
            //cOPR16_COPRUNNING.PROC_ID = PROC_ID;
            cOPR16_COPRUNNING.MODEL_ID = MODEL_ID;
            cOPR16_COPRUNNING.POSITION_ID = POSITION_ID;
            //cOPR16_COPRUNNING.LINEL_ID = LINEL_ID;
            cOPR16_COPRUNNING.DESC = DESC;
            cOPR16_COPRUNNING.COP_STATUS = COP_STATUS;
            cOPR16_COPRUNNING.CRE_BY = username;
            cOPR16_COPRUNNING.ADATE = AppPropModel.today;

            //db.COPR16_COPRUNNING.Add(cOPR16_COPRUNNING);
            db.Entry(cOPR16_COPRUNNING).State = EntityState.Modified;
            await db.SaveChangesAsync();

            foreach (ITEMSROW item in jsonString)
            {
                //COPR16_COPRUNNING_DT dtdt = new COPR16_COPRUNNING_DT();
                COPR16_COPRUNNING_DT dtdt = await db.COPR16_COPRUNNING_DT.FindAsync(COPR_ID, item.FGTYPE_ID);
                //COPR16_COPRUNNING_DT dtdt = db.COPR16_COPRUNNING_DT.Where(l => l.COPR_ID.Equals(COPR_ID) && l.FGTYPE_ID.Equals(item.FGTYPE_ID)).FirstOrDefault();
                dtdt.COPR_ID = COPR_ID;
                dtdt.DESC = "";
                dtdt.FGTYPE_ID = item.FGTYPE_ID;
                dtdt.LOTNO = item.LOTNO;
                dtdt.PNO = item.PNO;
                db.Entry(dtdt).State = EntityState.Modified;
                //db.COPR16_COPRUNNING_DT.Add(dtdt);
                await db.SaveChangesAsync();
            }
            SaveLineID(COPR_ID, LINE_ID);
            return RedirectToAction("Index");

            //if (ModelState.IsValid)
            //{
            //    db.Entry(cOPR16_COPRUNNING).State = EntityState.Modified;
            //    await db.SaveChangesAsync();
            //    return RedirectToAction("Index");
            //}
            //return View(cOPR16_COPRUNNING);
        }
        [HttpPost]
        public async Task<ActionResult> saveLot(string copno,string sbLot,string bkl1Lot, string bkl2Lot)
        {
            /*
            "copno": copno,
            "sbLot": sbLot,
            "bkl1Lot": bkl1Lot,
            "bkl2Lot": bkl2Lot
             */
            JsonResult rowData = new JsonResult();
            string SQLCMD2 = "exec dbo.COPR16_SAVE_LOT @COPNO,@SBLOT,@BKL1LOT,@BKL2LOT";
            try
            {
                if (db.Database.Connection.State != ConnectionState.Open)
                {
                    await db.Database.Connection.OpenAsync();
                }
                using (var cmd = db.Database.Connection.CreateCommand())
                {

                    cmd.CommandText = SQLCMD2;
                    cmd.Parameters.Add(new SqlParameter("@COPNO", copno == null ? "" : copno));
                    cmd.Parameters.Add(new SqlParameter("@SBLOT", sbLot == null ? "" : sbLot));
                    cmd.Parameters.Add(new SqlParameter("@BKL1LOT", bkl1Lot == null ? "" : bkl1Lot));
                    cmd.Parameters.Add(new SqlParameter("@BKL2LOT", bkl2Lot == null ? "" : bkl2Lot));
                    await cmd.ExecuteNonQueryAsync();
                }
                rowData.Data = "0";
            }
            catch (Exception e)
            {
                rowData.Data = e.Message;
            }

            return Json(rowData, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeletingCOPNO(string COPNO)
        {
            JsonResult rowData = new JsonResult();
            string SQLCMD2 = "exec dbo.COPR16_CANCEL_COPNO @COPNO";

            try
            {
                if(db.Database.Connection.State != ConnectionState.Open)
                {
                    await db.Database.Connection.OpenAsync();
                }
                using (var cmd = db.Database.Connection.CreateCommand())
                {

                    cmd.CommandText = SQLCMD2;
                    cmd.Parameters.Add(new SqlParameter("@COPNO", COPNO == null ? "" : COPNO));
                    await cmd.ExecuteNonQueryAsync();
                }
                rowData.Data = "0";
            }
            catch(Exception e)
            {
                rowData.Data = e.Message;
            }
            return Json(rowData, JsonRequestBehavior.AllowGet);
        }
        public async Task<ActionResult> SaveJsonAID(
            string COPR_ID,
            string WRK_ID,
            string PROC_ID,
            string MODEL_ID,
            string POSITION_ID,
            string DESC,
            string COP_STATUS,
            string LINE_ID,
            string username,
            List<ITEMSROW> jsonString
        )
        {
            //COPR16_COPRUNNING cOPR16_COPRUNNING = new COPR16_COPRUNNING();
            COPR16_COPRUNNING cOPR16_COPRUNNING = await db.COPR16_COPRUNNING.FindAsync(COPR_ID);
            //cOPR16_COPRUNNING.COPR_ID = COPR_ID;
            cOPR16_COPRUNNING.WRK_ID = WRK_ID;
            //cOPR16_COPRUNNING.PROC_ID = PROC_ID;
            cOPR16_COPRUNNING.MODEL_ID = MODEL_ID;
            cOPR16_COPRUNNING.POSITION_ID = POSITION_ID;
            //cOPR16_COPRUNNING.LINEL_ID = LINEL_ID;
            cOPR16_COPRUNNING.DESC = DESC;
            cOPR16_COPRUNNING.COP_STATUS = COP_STATUS;
            cOPR16_COPRUNNING.CRE_BY = username;
            cOPR16_COPRUNNING.ADATE = AppPropModel.today;

            //db.COPR16_COPRUNNING.Add(cOPR16_COPRUNNING);
            db.Entry(cOPR16_COPRUNNING).State = EntityState.Modified;
            await db.SaveChangesAsync();

            foreach (ITEMSROW item in jsonString)
            {
                //COPR16_COPRUNNING_DT dtdt = new COPR16_COPRUNNING_DT();
                COPR16_COPRUNNING_DT dtdt = await db.COPR16_COPRUNNING_DT.FindAsync(COPR_ID, item.FGTYPE_ID);
                //COPR16_COPRUNNING_DT dtdt = db.COPR16_COPRUNNING_DT.Where(l => l.COPR_ID.Equals(COPR_ID) && l.FGTYPE_ID.Equals(item.FGTYPE_ID)).FirstOrDefault();
                dtdt.COPR_ID = COPR_ID;
                dtdt.DESC = "";
                dtdt.FGTYPE_ID = item.FGTYPE_ID;
                dtdt.LOTNO = item.LOTNO;
                dtdt.PNO = item.PNO;
                db.Entry(dtdt).State = EntityState.Modified;
                //db.COPR16_COPRUNNING_DT.Add(dtdt);
                await db.SaveChangesAsync();
            }
            SaveLineID(COPR_ID, LINE_ID);
            return RedirectToAction("IndexAID");

            //if (ModelState.IsValid)
            //{
            //    db.Entry(cOPR16_COPRUNNING).State = EntityState.Modified;
            //    await db.SaveChangesAsync();
            //    return RedirectToAction("Index");
            //}
            //return View(cOPR16_COPRUNNING);
        }
        public async Task<ActionResult> SaveJsonNONEC(
            string COPR_ID,
            string WRK_ID,
            string PROC_ID,
            string MODEL_ID,
            string POSITION_ID,
            string DESC,
            string COP_STATUS,
            string LINE_ID,
            string username,
            List<ITEMSROW> jsonString
        )
        {
            //COPR16_COPRUNNING cOPR16_COPRUNNING = new COPR16_COPRUNNING();
            COPR16_COPRUNNING cOPR16_COPRUNNING = await db.COPR16_COPRUNNING.FindAsync(COPR_ID);
            //cOPR16_COPRUNNING.COPR_ID = COPR_ID;
            cOPR16_COPRUNNING.WRK_ID = WRK_ID;
            //cOPR16_COPRUNNING.PROC_ID = PROC_ID;
            cOPR16_COPRUNNING.MODEL_ID = MODEL_ID;
            cOPR16_COPRUNNING.POSITION_ID = POSITION_ID;
            //cOPR16_COPRUNNING.LINEL_ID = LINEL_ID;
            cOPR16_COPRUNNING.DESC = DESC;
            cOPR16_COPRUNNING.COP_STATUS = COP_STATUS;
            cOPR16_COPRUNNING.CRE_BY = username;
            cOPR16_COPRUNNING.ADATE = AppPropModel.today;

            //db.COPR16_COPRUNNING.Add(cOPR16_COPRUNNING);
            db.Entry(cOPR16_COPRUNNING).State = EntityState.Modified;
            await db.SaveChangesAsync();

            foreach (ITEMSROW item in jsonString)
            {
                //COPR16_COPRUNNING_DT dtdt = new COPR16_COPRUNNING_DT();
                COPR16_COPRUNNING_DT dtdt = await db.COPR16_COPRUNNING_DT.FindAsync(COPR_ID, item.FGTYPE_ID);
                //COPR16_COPRUNNING_DT dtdt = db.COPR16_COPRUNNING_DT.Where(l => l.COPR_ID.Equals(COPR_ID) && l.FGTYPE_ID.Equals(item.FGTYPE_ID)).FirstOrDefault();
                dtdt.COPR_ID = COPR_ID;
                dtdt.DESC = "";
                dtdt.FGTYPE_ID = item.FGTYPE_ID;
                dtdt.LOTNO = item.LOTNO;
                dtdt.PNO = item.PNO;
                db.Entry(dtdt).State = EntityState.Modified;
                //db.COPR16_COPRUNNING_DT.Add(dtdt);
                await db.SaveChangesAsync();
            }
            SaveLineID(COPR_ID, LINE_ID);
            return RedirectToAction("IndexNONEC");

            //if (ModelState.IsValid)
            //{
            //    db.Entry(cOPR16_COPRUNNING).State = EntityState.Modified;
            //    await db.SaveChangesAsync();
            //    return RedirectToAction("Index");
            //}
            //return View(cOPR16_COPRUNNING);
        }
        // GET: COPR16_COPRUNNING/Delete/5
        //public async Task<ActionResult> Delete(string id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    COPR16_COPRUNNING cOPR16_COPRUNNING = await db.COPR16_COPRUNNING.FindAsync(id);
        //    if (cOPR16_COPRUNNING == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(cOPR16_COPRUNNING);
        //}
        // GET: COPR16_COPRUNNING/Dispatch/5
        public void SaveLineID(string COPNO, string LINE_ID)
        {
            if (db.Database.Connection.State != ConnectionState.Open)
            {
                db.Database.Connection.Open();
            }
            string SQLCMD2 = "exec dbo.sp_SaveLineToCOPNO @COPR_ID,@LINE_ID";

            using (var cmd = db.Database.Connection.CreateCommand())
            {
                cmd.CommandText = SQLCMD2;
                cmd.Parameters.Add(new SqlParameter("@COPR_ID", COPNO == null ? "" : COPNO));
                cmd.Parameters.Add(new SqlParameter("@LINE_ID", LINE_ID == null ? "" : LINE_ID));
                cmd.ExecuteNonQuery();
            }
        }
        public async Task<ActionResult> Dispatch(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            COPR16_COPRUNNING cOPR16_COPRUNNING = await db.COPR16_COPRUNNING.FindAsync(id);

            if (cOPR16_COPRUNNING == null)
            {
                return HttpNotFound();
            }

            string SQLCMD2 = "exec dbo.sp_save_teststep_to_queue @COPR_ID";

            List<SqlParameter> param2 = new List<SqlParameter>();
            param2.Add(new SqlParameter("@COPR_ID", id == null ? "" : id));
            await db.Database.ExecuteSqlCommandAsync(SQLCMD2, param2.ToArray());

            SQLCMD2 = "exec dbo.sp_save_teststep_to_wait @COPR_ID,@SEQ_NO ";
            param2.Clear();
            param2.Add(new SqlParameter("@COPR_ID", id == null ? "" : id));
            param2.Add(new SqlParameter("@SEQ_NO", "1"));
            await db.Database.ExecuteSqlCommandAsync(SQLCMD2, param2.ToArray());

            cOPR16_COPRUNNING.COP_STATUS = "READY";
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<ActionResult> DispatchAID(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            COPR16_COPRUNNING cOPR16_COPRUNNING = await db.COPR16_COPRUNNING.FindAsync(id);

            if (cOPR16_COPRUNNING == null)
            {
                return HttpNotFound();
            }

            string SQLCMD2 = "exec dbo.sp_save_teststep_to_queue @COPR_ID";

            List<SqlParameter> param2 = new List<SqlParameter>();
            param2.Add(new SqlParameter("@COPR_ID", id == null ? "" : id));
            await db.Database.ExecuteSqlCommandAsync(SQLCMD2, param2.ToArray());

            SQLCMD2 = "exec dbo.sp_save_teststep_to_wait @COPR_ID,@SEQ_NO ";
            param2.Clear();
            param2.Add(new SqlParameter("@COPR_ID", id == null ? "" : id));
            param2.Add(new SqlParameter("@SEQ_NO", "1"));
            await db.Database.ExecuteSqlCommandAsync(SQLCMD2, param2.ToArray());

            cOPR16_COPRUNNING.COP_STATUS = "READY";
            await db.SaveChangesAsync();
            return RedirectToAction("IndexAID");
        }
        public async Task<ActionResult> DispatchNONEC(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            COPR16_COPRUNNING cOPR16_COPRUNNING = await db.COPR16_COPRUNNING.FindAsync(id);

            if (cOPR16_COPRUNNING == null)
            {
                return HttpNotFound();
            }

            string SQLCMD2 = "exec dbo.sp_save_teststep_to_queue @COPR_ID";

            List<SqlParameter> param2 = new List<SqlParameter>();
            param2.Add(new SqlParameter("@COPR_ID", id == null ? "" : id));
            await db.Database.ExecuteSqlCommandAsync(SQLCMD2, param2.ToArray());

            SQLCMD2 = "exec dbo.sp_save_teststep_to_wait @COPR_ID,@SEQ_NO ";
            param2.Clear();
            param2.Add(new SqlParameter("@COPR_ID", id == null ? "" : id));
            param2.Add(new SqlParameter("@SEQ_NO", "1"));
            await db.Database.ExecuteSqlCommandAsync(SQLCMD2, param2.ToArray());

            cOPR16_COPRUNNING.COP_STATUS = "READY";
            await db.SaveChangesAsync();
            return RedirectToAction("IndexNONEC");
        }

        // POST: COPR16_COPRUNNING/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            COPR16_COPRUNNING cOPR16_COPRUNNING = await db.COPR16_COPRUNNING.FindAsync(id);
            db.COPR16_COPRUNNING.Remove(cOPR16_COPRUNNING);
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
        public  ActionResult REPORTS()
        {
            return  View();
        }
        public async Task<ActionResult> FINAL_REPORTS()
        {
            //return View(await db.COPR16_COPRUNNING.ToListAsync());

            CopRunningModel model = new CopRunningModel(db);
            model.cOPR16_COPRUNNING = new COPR16_COPRUNNING();
            model.cOPR16_COPRUNNING_DT = new COPR16_COPRUNNING_DT();

            return View(model);
        }
        public async Task<ActionResult> FINAL_REPORTS_AID()
        {
            //return View(await db.COPR16_COPRUNNING.ToListAsync());

            CopRunningModel model = new CopRunningModel(db);
            model.cOPR16_COPRUNNING = new COPR16_COPRUNNING();
            model.cOPR16_COPRUNNING_DT = new COPR16_COPRUNNING_DT();
            string SQL_CMD = "SELECT distinct MODEL_ID FROM COPR16_COPRUNNING where WRK_ID like 'COP-AID%'";
            model.MODEL_LIST = new List<SelectListItem>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DatabaseServer"].ToString()))
            {
                if (con.State != System.Data.ConnectionState.Open)
                {
                    await con.OpenAsync();
                }
                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = SQL_CMD;
                    using (DbDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        var rowModel = Utils.Serialize((SqlDataReader)reader);
                        foreach (var row in rowModel)
                        {
                            string value1 = row["MODEL_ID"].ToString();
                            model.MODEL_LIST.Add(new SelectListItem { Text = value1, Value = value1 });
                        }
                        reader.Close();
                    }

                }
            }
            return View(model);
        }
        public async Task<ActionResult> FINAL_REPORTS_NONEC()
        {
            //return View(await db.COPR16_COPRUNNING.ToListAsync());

            CopRunningModel model = new CopRunningModel(db);
            model.cOPR16_COPRUNNING = new COPR16_COPRUNNING();
            model.cOPR16_COPRUNNING_DT = new COPR16_COPRUNNING_DT();
            string SQL_CMD = "SELECT distinct MODEL_ID FROM COPR16_COPRUNNING where WRK_ID like 'COP-NONEC%'";
            model.MODEL_LIST = new List<SelectListItem>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DatabaseServer"].ToString()))
            {
                if (con.State != System.Data.ConnectionState.Open)
                {
                    await con.OpenAsync();
                }
                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = SQL_CMD;
                    using (DbDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        var rowModel = Utils.Serialize((SqlDataReader)reader);
                        foreach (var row in rowModel)
                        {
                            string value1 = row["MODEL_ID"].ToString();
                            model.MODEL_LIST.Add(new SelectListItem { Text = value1, Value = value1 });
                        }
                        reader.Close();
                    }

                }
            }
            return View(model);
        }
        public async Task<ActionResult> FINAL_REPORTS_TENSILE()
        {
            //return View(await db.COPR16_COPRUNNING.ToListAsync());

            CopRunningModel model = new CopRunningModel(db);
            model.cOPR16_COPRUNNING = new COPR16_COPRUNNING();
            model.cOPR16_COPRUNNING_DT = new COPR16_COPRUNNING_DT();
            string SQL_CMD = "SELECT distinct SPCC_YEAR FROM dbo.COPR16_SPCC_DIM_YEAR ORDER BY SPCC_YEAR DESC";

            using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DatabaseServer"].ToString()))
            {
                if (con.State != System.Data.ConnectionState.Open)
                {
                    await con.OpenAsync();
                }
                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = SQL_CMD;
                    using (DbDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        var lmodel = Utils.Serialize((SqlDataReader)reader);
                        foreach (var row in lmodel)
                        {
                            string value1 = row["SPCC_YEAR"].ToString();
                            model.SPC_YEAR.Add(new SelectListItem { Text = value1, Value = value1 });
                        }
                        reader.Close();
                    }

                }

                using (var cmd2 = con.CreateCommand())
                {
                    cmd2.CommandText = "SELECT distinct MODLE_ID AS SPCC_MODEL FROM dbo.COPR16_ITEMS_MSTR ORDER BY MODLE_ID";
                    using (System.Data.Common.DbDataReader reader2 = await cmd2.ExecuteReaderAsync())
                    {
                        var lmodel = Utils.Serialize((SqlDataReader)reader2);
                        foreach (var row in lmodel)
                        {
                            string value1 = row["SPCC_MODEL"].ToString();
                            model.SPC_MODEL_LIST.Add(new SelectListItem { Text = value1, Value = value1 });
                        }
                        reader2.Close();
                    }
                }

                con.Close();
            }

            return View(model);
        }
        public ActionResult COPR16_Running_Report()
        {
            //return View(await db.COPR16_COPRUNNING.ToListAsync());

            //CopRunningModel model = new CopRunningModel(db);
            //model.cOPR16_COPRUNNING = new COPR16_COPRUNNING();
            //model.cOPR16_COPRUNNING_DT = new COPR16_COPRUNNING_DT();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> getPartListTNS(string ModelID,string YEAR,string USERNAME)
        {
            /*
            string sqlCmd2 = "SELECT ColIdx,F6,F7,F8,F9,F10,F11,F12,F13,F14,F15,F16,F17,F18,F19,F20,F21,F22,F23,F24,F25,F26,F27,F28,F29\n" +
                "FROM [dbo].[COPR16_TNS_PART_UPLOAD]\n" +
                "WHERE cast(ColIdx as int) in (4,7,10,13)\n" +
                "\n" +
                "ORDER BY cast(ColIdx as int) asc";
            */
            string sqlCmd2 = "exec dbo.COPR16_GET_TNS_RPT @YEAR,@MODEL";

            var rowData = new JsonResult();
            
            if (db.Database.Connection.State != ConnectionState.Open)
            {
                await db.Database.Connection.OpenAsync();
            }
            using (var cmd = db.Database.Connection.CreateCommand())
            {
                cmd.CommandText = sqlCmd2;
                cmd.Parameters.Add(new SqlParameter("@YEAR", YEAR));
                cmd.Parameters.Add(new SqlParameter("@MODEL", ModelID == null ? "" : ModelID));
                System.Data.Common.DbDataReader reader = await cmd.ExecuteReaderAsync();
                {
                    var model = Utils.Serialize((SqlDataReader)reader);
                    rowData.Data = model;
                }
            }
            return Json(rowData, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> getSPCDATA(
            string ModelID,
            List<SPCDATA> SEC51,
            List<SPCDATA> SEC52,
            List<SPCDATA> SEC6,
            List<SPCDATA> SEC7,
            string YEAR
            )
        {
            SPCDATAS listData = new SPCDATAS();
            string sqlCmd2 = "exec COPR16_GETSPCDATA @part,@month,@year,@sec,@model";

            var rowData = new JsonResult();
            if (db.Database.Connection.State != ConnectionState.Open)
            {
                await db.Database.Connection.OpenAsync();
            }
            List<SPCDATA> localSEC51 = new List<SPCDATA>();
            List<SPCDATA> localSEC52 = new List<SPCDATA>();
            List<SPCDATA> localSEC6 = new List<SPCDATA>();
            List<SPCDATA> localSEC7 = new List<SPCDATA>();

            int n = 0;
            var prevPart = "";
            foreach (SPCDATA row in SEC51)
            {
                
                using (var cmd = db.Database.Connection.CreateCommand())
                {
                    cmd.CommandText = sqlCmd2;
                    cmd.Parameters.Add(new SqlParameter("@part", row.PART == null ? "" : row.PART));
                    cmd.Parameters.Add(new SqlParameter("@month", row.MONTH == null ? "" : row.MONTH));
                    cmd.Parameters.Add(new SqlParameter("@year", YEAR == null ? "" : YEAR));
                    cmd.Parameters.Add(new SqlParameter("@sec", "51"));
                    cmd.Parameters.Add(new SqlParameter("@model", ModelID));
                    using (DbDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        var model = Utils.Serialize((SqlDataReader)reader);
                        SPCDATA data = new SPCDATA();
                        data = row;
                        foreach (var dict in model)
                        {
                            foreach(var dt in dict)
                            {
                                if (int.Parse( row.POS) == 2)
                                {
                                    if (prevPart == row.PART)
                                    {
                                        if (dt.Key == "TNS_LOT2")
                                        {
                                            data.LOT = dt.Value.ToString();
                                        }
                                        if (dt.Key == "TNS_VALUE2")
                                        {
                                            data.VAL = dt.Value.ToString();
                                        }
                                    }
                                    else
                                    {
                                        if (dt.Key == "TNS_LOT1")
                                        {
                                            data.LOT = dt.Value.ToString();
                                        }
                                        if (dt.Key == "TNS_VALUE1")
                                        {
                                            data.VAL = dt.Value.ToString();
                                        }
                                    }
                                }
                                else {
                                    if (dt.Key == "TNS_LOT1")
                                    {
                                        data.LOT = dt.Value.ToString();
                                    }
                                    if (dt.Key == "TNS_VALUE1")
                                    {
                                        data.VAL = dt.Value.ToString();
                                    }
                                }
                            }

                        }
                            
                        
                        localSEC51.Add(data);
                        
                        reader.Close();
                    }
                }
                prevPart = row.PART;
                n++;
            }
            listData.SEC51 = localSEC51;

            n = 0;
            foreach (SPCDATA row in SEC52)
            {
                using (var cmd = db.Database.Connection.CreateCommand())
                {
                    cmd.CommandText = sqlCmd2;
                    cmd.Parameters.Add(new SqlParameter("@part", row.PART == null ? "" : row.PART));
                    cmd.Parameters.Add(new SqlParameter("@month", row.MONTH == null ? "" : row.MONTH));
                    cmd.Parameters.Add(new SqlParameter("@year", YEAR == null ? "" : YEAR));
                    cmd.Parameters.Add(new SqlParameter("@sec", "52"));
                    cmd.Parameters.Add(new SqlParameter("@model", ModelID));
                    using (DbDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        var model = Utils.Serialize((SqlDataReader)reader);
                        SPCDATA data = new SPCDATA();
                        data = row;
                        foreach (var dict in model)
                        {
                            /*
                            foreach (var dt in dict)
                            {
                                if (dt.Key == "TNS_LOT1")
                                {
                                    data.LOT = dt.Value.ToString();
                                }
                                if (dt.Key == "TNS_VALUE1")
                                {
                                    data.VAL = dt.Value.ToString();
                                }

                            }
                            */
                            foreach (var dt in dict)
                            {
                                if (int.Parse(row.POS) == 2)
                                {
                                    if (prevPart == row.PART)
                                    {
                                        if (dt.Key == "TNS_LOT2")
                                        {
                                            data.LOT = dt.Value.ToString();
                                        }
                                        if (dt.Key == "TNS_VALUE2")
                                        {
                                            data.VAL = dt.Value.ToString();
                                        }
                                    }
                                    else
                                    {
                                        if (dt.Key == "TNS_LOT1")
                                        {
                                            data.LOT = dt.Value.ToString();
                                        }
                                        if (dt.Key == "TNS_VALUE1")
                                        {
                                            data.VAL = dt.Value.ToString();
                                        }
                                    }
                                }
                                else
                                {
                                    if (dt.Key == "TNS_LOT1")
                                    {
                                        data.LOT = dt.Value.ToString();
                                    }
                                    if (dt.Key == "TNS_VALUE1")
                                    {
                                        data.VAL = dt.Value.ToString();
                                    }
                                }
                            }
                        }


                        localSEC52.Add(data);

                        reader.Close();
                    }
                }
                n++;
            }
            listData.SEC52 = localSEC52;

            n = 0;
            foreach (SPCDATA row in SEC6)
            {
                using (var cmd = db.Database.Connection.CreateCommand())
                {
                    cmd.CommandText = sqlCmd2;
                    cmd.Parameters.Add(new SqlParameter("@part", row.PART == null ? "" : row.PART));
                    cmd.Parameters.Add(new SqlParameter("@month", row.MONTH == null ? "" : row.MONTH));
                    cmd.Parameters.Add(new SqlParameter("@year", YEAR == null ? "" : YEAR));
                    cmd.Parameters.Add(new SqlParameter("@sec", "6"));
                    cmd.Parameters.Add(new SqlParameter("@model", ModelID));
                    using (DbDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        var model = Utils.Serialize((SqlDataReader)reader);
                        SPCDATA data = new SPCDATA();
                        data = row;
                        foreach (var dict in model)
                        {
                            foreach (var dt in dict)
                            {
                                if (dt.Key == "TNS_LOT1")
                                {
                                    data.LOT = dt.Value.ToString();
                                }
                                if (dt.Key == "TNS_VALUE1")
                                {
                                    data.VAL = dt.Value.ToString();
                                }

                            }
                        }


                        localSEC6.Add(data);

                        reader.Close();
                    }
                }
                n++;
            }
            listData.SEC6 = localSEC6;

            n = 0;
            foreach (SPCDATA row in SEC7)
            {
                using (var cmd = db.Database.Connection.CreateCommand())
                {
                    cmd.CommandText = sqlCmd2;
                    cmd.Parameters.Add(new SqlParameter("@part", row.PART == null ? "" : row.PART));
                    cmd.Parameters.Add(new SqlParameter("@month", row.MONTH == null ? "" : row.MONTH));
                    cmd.Parameters.Add(new SqlParameter("@year", YEAR == null ? "" : YEAR));
                    cmd.Parameters.Add(new SqlParameter("@sec", "7"));
                    cmd.Parameters.Add(new SqlParameter("@model", ModelID));
                    using (DbDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        var model = Utils.Serialize((SqlDataReader)reader);
                        SPCDATA data = new SPCDATA();
                        data = row;
                        foreach (var dict in model)
                        {
                            foreach (var dt in dict)
                            {
                                if (dt.Key == "TNS_LOT1")
                                {
                                    data.LOT = dt.Value.ToString();
                                }
                                if (dt.Key == "TNS_VALUE1")
                                {
                                    data.VAL = dt.Value.ToString();
                                }

                            }
                        }


                        localSEC7.Add(data);

                        reader.Close();
                    }
                }
                n++;
            }
            listData.SEC7 = localSEC7;

            rowData.Data = listData;
            return Json(rowData, JsonRequestBehavior.AllowGet);
        }
    }
}
