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
            string SQLCMD = "SELECT * FROM COPR16_COPRUNNING A WHERE ADATE between '" + startDate + "' and '"+ endDate + "' ORDER BY ADATE DESC";
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

            SQLCMD = SQLCMD + " ORDER BY ADATE DESC"; 
            //model.cOPR16_COPRUNNING_List = await db.COPR16_COPRUNNING.Where(l=>l.ADATE.Value.ToString("yyyy-mm-dd") == Models.AppPropModel.today.ToString("yyyy-mm-dd")).OrderBy(l=>l.ADATE).ToListAsync();
            model.cOPR16_COPRUNNING_List = await db.COPR16_COPRUNNING.SqlQuery(SQLCMD).ToListAsync();

            return View(model);
        }

        // POST: COPR16_COPRUNNING_RT
        [HttpPost]
        public JsonResult GetCOPID()
        {

            List<GetSeqNextValue1_Result> row = db.GetSeqNextValue("COPRUNNING").ToList();
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
        public async Task<JsonResult> getBuckleListModel(itemParams param1)
        {
            JsonResult rowData = new JsonResult();
            string SQLCMD2 = "exec dbo.GetBuckleWithParams @item_no,@model_no,@line_no,@position_no";

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
            string SQLCMD2 = "exec dbo.GetSBWithParams @item_no,@model_no,@line_no,@position_no";

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
        public async Task<ActionResult> Create()
        {
            CopRunningModel model = new CopRunningModel(db);
            model.cOPR16_COPRUNNING = new COPR16_COPRUNNING();
            model.cOPR16_COPRUNNING_DT = new COPR16_COPRUNNING_DT();
            
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateMacro(string xModel,string xLine,string WRK_ID,string btnDate)
        {


            CopRunningModel model = new CopRunningModel(db);
            model.cOPR16_COPRUNNING_WRK_ID = WRK_ID;
            model.xModel = xModel;
            model.xLine = xLine;
            model.selectDate = btnDate;
            model.cOPR16_COPRUNNING = new COPR16_COPRUNNING();
            model.cOPR16_COPRUNNING_DT = new COPR16_COPRUNNING_DT();

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


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> getFinalReport(string ModelID,String fromDate,String toDate)
        {
            JsonResult row = new JsonResult();
            List<object> dataRst = new List<object>();

            string SQLCMD2 = "exec dbo.sp_get_cop_count_by_model @MODEL_ID,@FROM_DT,@TO_DT";
            if (db.Database.Connection.State != ConnectionState.Open)
            {
                await db.Database.Connection.OpenAsync();
            }
            using (var cmd = db.Database.Connection.CreateCommand())
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
            }

            db.Database.Connection.Close();
            if (db.Database.Connection.State != ConnectionState.Open)
            {
                await db.Database.Connection.OpenAsync();
            }
            SQLCMD2 = "exec dbo.sp_get_cop_part_list_by_model @MODEL_ID,@FROM_DT,@TO_DT";

            using (var cmd = db.Database.Connection.CreateCommand())
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
            }
            db.Database.Connection.Close();
            if (db.Database.Connection.State != ConnectionState.Open)
            {
                await db.Database.Connection.OpenAsync();
            }
            SQLCMD2 = "exec dbo.sp_get_cop_result_by_model @MODEL_ID,@FROM_DT,@TO_DT";

            using (var cmd = db.Database.Connection.CreateCommand())
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
            }

            db.Database.Connection.Close();
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
            COPR16_COPRUNNING cOPR16_COPRUNNING = new COPR16_COPRUNNING();
            cOPR16_COPRUNNING.COPR_ID = COPR_ID;
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
                dtdt.COPR_ID = COPR_ID;
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

                        VarParams.Add(new SqlParameter("@COPR_ID", COPR_ID));
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

            if (db.Database.Connection.State != ConnectionState.Open)
            {
                await db.Database.Connection.OpenAsync();
            }
            string SQLCMD2 = "exec dbo.sp_CreateNewCop @COPR_ID,@WRK_ID,@PROC_ID,@MODEL_ID,@POSITION_ID,@DESC,@COP_STATUS,@LINE_ID,@CRE_BY";

            using (var cmd = db.Database.Connection.CreateCommand())
            {
                //await db.Database.Connection.OpenAsync();
                cmd.CommandText = SQLCMD2;
                cmd.Parameters.Add(new SqlParameter("@COPR_ID", COPR_ID == null ? "" : COPR_ID));
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
                dtdt.COPR_ID = COPR_ID;
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

                        VarParams.Add(new SqlParameter("@COPR_ID", COPR_ID));
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
                cmd.Parameters.Add(new SqlParameter("@COPR_ID", COPR_ID == null ? "" : COPR_ID));
                cmd.Parameters.Add(new SqlParameter("@MODEL_ID", MODEL_ID == null ? "" : MODEL_ID));
                cmd.Parameters.Add(new SqlParameter("@POSITION_ID", POSITION_ID == null ? "" : POSITION_ID));
                cmd.Parameters.Add(new SqlParameter("@LINE_ID", LINE_ID == null ? "" : LINE_ID));
                cmd.Parameters.Add(new SqlParameter("@PROC_ID", PROC_ID == null ? "" : PROC_ID));
                cmd.Parameters.Add(new SqlParameter("@SELECT_DATE", SELECT_DATE == null ? "" : SELECT_DATE));
                var reader = await cmd.ExecuteNonQueryAsync();

            }

            return RedirectToAction("Index");
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
            COPR16_COPRUNNING cOPR16_COPRUNNING = await db.COPR16_COPRUNNING.FindAsync(SELECTED_COPNO);
            CopRunningModel model = new CopRunningModel(db);

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
                await db.Database.Connection.OpenAsync();
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
            }
            return View(model);
        }
        T GetObject<T>(Dictionary<string, object> dict)
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
            string username,
            List<ITEMSROW> jsonString
        )
        {
            //COPR16_COPRUNNING cOPR16_COPRUNNING = new COPR16_COPRUNNING();
            COPR16_COPRUNNING cOPR16_COPRUNNING = await db.COPR16_COPRUNNING.FindAsync(COPR_ID);
            //cOPR16_COPRUNNING.COPR_ID = COPR_ID;
            cOPR16_COPRUNNING.WRK_ID = WRK_ID;
            cOPR16_COPRUNNING.PROC_ID = PROC_ID;
            cOPR16_COPRUNNING.MODEL_ID = MODEL_ID;
            cOPR16_COPRUNNING.POSITION_ID = POSITION_ID;
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
            return RedirectToAction("Index");

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
    }
}
