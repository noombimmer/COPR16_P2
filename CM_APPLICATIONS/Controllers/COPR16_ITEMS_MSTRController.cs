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
using System.Data.Common;
using System.Configuration;
using System.Text.RegularExpressions;

namespace CM_APPLICATIONS.Controllers
{
    public class COPR16_ITEMS_MSTRController : Controller
    {
        private COPR16Entities db = new COPR16Entities();

        // GET: COPR16_ITEMS_MSTR
        public ActionResult Index()
        {

            return View();
        }
        public async Task<ActionResult> MatchingParts()
        {
            MATCHING_MODEL model = new MATCHING_MODEL();
            return View(model);
        }

        [HttpPost]
        public async Task<JsonResult> GetModel(ModelSearch searchParams)
        {
            var rowData = new JsonResult();
            rowData.Data = searchParams;
            await db.Database.Connection.OpenAsync();
            using (var cmd = db.Database.Connection.CreateCommand())
            {
                cmd.CommandText = "exec [dbo].[COPR16_SP_LIST_MODEL_MAPPING] @MODEL_ID,@LINE_ID,@FG_NO,@ITEM_ID";
                cmd.Parameters.Add(new SqlParameter("@MODEL_ID", searchParams.MODEL_ID == null ? "" : searchParams.MODEL_ID));
                cmd.Parameters.Add(new SqlParameter("@LINE_ID", searchParams.LINE_ID == null ? "" : searchParams.LINE_ID));
                cmd.Parameters.Add(new SqlParameter("@FG_NO", searchParams.FG_NO == null ? "" : searchParams.FG_NO));
                cmd.Parameters.Add(new SqlParameter("@ITEM_ID", searchParams.ITEM_ID == null ? "" : searchParams.ITEM_ID));
                DbDataReader reader = await cmd.ExecuteReaderAsync();
                {
                    var model = Serialize((SqlDataReader)reader);
                    rowData.Data = model;
                }
            }
            return Json(rowData, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getXls()
        {
            CBExcel excel = new CBExcel();
            excel.open("");
            excel.close();
            return View();
        }

        // GET: COPR16_ITEMS_MSTR/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            COPR16_ITEMS_MSTR cOPR16_ITEMS_MSTR = await db.COPR16_ITEMS_MSTR.FindAsync(id);
            if (cOPR16_ITEMS_MSTR == null)
            {
                return HttpNotFound();
            }
            return View(cOPR16_ITEMS_MSTR);
        }

        [HttpPost]
        public async Task<ActionResult> getCarLineList(string LINEID,string LINEDESC)
        {
            var rowData = new JsonResult();
            ItemMatchModel model = new ItemMatchModel(db);
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@LINE_ID", (LINEID ==null?"": LINEID)));
            param.Add(new SqlParameter("@LINE_DESC", (LINEDESC == null ? "" : LINEDESC)));
            rowData.Data = await model.ExecuteSQLAsync("exec sp_list_carline @LINE_ID,@LINE_DESC", param);
            return Json(rowData, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public async Task<ActionResult> getPosList(string POSID, string POSDESC)
        {
            var rowData = new JsonResult();
            ItemMatchModel model = new ItemMatchModel(db);
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@POS_ID", (POSID == null ? "" : POSID)));
            param.Add(new SqlParameter("@POS_DESC", (POSDESC == null ? "" : POSDESC)));
            rowData.Data = await model.ExecuteSQLAsync("exec sp_list_position @POS_ID,@POS_DESC", param);
            return Json(rowData, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public async Task<ActionResult> getFgList(string FGNO, string FGDESC)
        {
            var rowData = new JsonResult();
            ItemMatchModel model = new ItemMatchModel(db);
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@FG_NO", (FGNO == null ? "" : FGNO)));
            param.Add(new SqlParameter("@FG_DESC", (FGDESC == null ? "" : FGDESC)));
            rowData.Data = await model.ExecuteSQLAsync("exec sp_list_fg @FG_NO,@FG_DESC", param);
            return Json(rowData, JsonRequestBehavior.AllowGet);
        }

        // GET: COPR16_ITEMS_MSTR/Create
        public ActionResult Create()
        {
            ItemsModel model = new ItemsModel(db);
            
            model.cOPR16_ITEMS_MSTR = new COPR16_ITEMS_MSTR();
            return View(model);
        }

        // POST: COPR16_ITEMS_MSTR/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ITEM_ID,FG_NO,BRAND_ID,MODLE_ID,LINE_ID,POS_ID,ADATE,CRE_BY,MOD_BY,MOD_DATE,FLGACT")] COPR16_ITEMS_MSTR cOPR16_ITEMS_MSTR)
        {
            if (ModelState.IsValid)
            {
                db.COPR16_ITEMS_MSTR.Add(cOPR16_ITEMS_MSTR);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(cOPR16_ITEMS_MSTR);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveJson(List<FG_MAPPED> FG_MAPPED)
        {
            var rowData = new JsonResult();
            if (FG_MAPPED.Count > 0)
            {
                List<SqlParameter> param1 = new List<SqlParameter>();
                param1.Add(new SqlParameter("@MODLE_ID", FG_MAPPED[0].MODEL_ID == null ? "" : FG_MAPPED[0].MODEL_ID));
                param1.Add(new SqlParameter("@LINE_ID", FG_MAPPED[0].LINE_ID == null ? "" : FG_MAPPED[0].LINE_ID));
                await db.Database.ExecuteSqlCommandAsync("exec delete_item_mapping @MODLE_ID,@LINE_ID", param1.ToArray());

                foreach (var data in FG_MAPPED)
                {
                    string SQLCMD = "exec [dbo].[sp_insert_item_mapping] @ITEM_ID,@FG_NO,@BRAND_ID,@MODLE_ID,@LINE_ID,@POS_ID,@FGTYPE_ID,@FLGACT,@ADATE,@MOD_DATE,@CRE_BY,@MOD_BY";
                    List<SqlParameter> param = new List<SqlParameter>();
                    param.Add(new SqlParameter("@ITEM_ID", data.ITEM_ID == null ? "" : data.ITEM_ID));
                    param.Add(new SqlParameter("@FG_NO", data.FG_NO == null ? "" : data.FG_NO));
                    param.Add(new SqlParameter("@BRAND_ID", data.BRAND_ID == null ? "" : data.BRAND_ID));
                    param.Add(new SqlParameter("@MODLE_ID", data.MODEL_ID == null ? "" : data.MODEL_ID));
                    param.Add(new SqlParameter("@LINE_ID", data.LINE_ID == null ? "" : data.LINE_ID));
                    param.Add(new SqlParameter("@POS_ID", data.POS_ID == null ? "" : data.POS_ID));
                    param.Add(new SqlParameter("@FGTYPE_ID", data.FGTYPE_ID == null ? "" : data.FGTYPE_ID));
                    param.Add(new SqlParameter("@FLGACT", data.FLGACT.ToLower() == "true" ? true : false));
                    param.Add(new SqlParameter("@ADATE", AppPropModel.today));
                    param.Add(new SqlParameter("@MOD_DATE", AppPropModel.today));
                    param.Add(new SqlParameter("@CRE_BY", data.CRE_BY == null ? "" : data.CRE_BY));
                    param.Add(new SqlParameter("@MOD_BY", data.MOD_BY == null ? "" : data.MOD_BY));
                    await db.Database.ExecuteSqlCommandAsync(SQLCMD, param.ToArray());
                }
            }
            rowData.Data = FG_MAPPED;
            return Json(rowData, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        // GET: COPR16_ITEMS_MSTR/Edit/5
        public async Task<ActionResult> EditEx(string id)
        {
            string usr = System.Web.HttpContext.Current.User.Identity.Name.Substring(System.Web.HttpContext.Current.User.Identity.Name.IndexOf(@"\") + 1);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            }
            ItemMatchModel model = new ItemMatchModel(db);
            COPR16_MODEL_MSTR cOPR16_MODEL_MSTR = await db.COPR16_MODEL_MSTR.Where(l => l.MODEL_ID.Equals(id)).FirstAsync();
            COPR16_ITEMS_MSTR cOPR16_ITEMS_MSTR;
            List<COPR16_ITEMS_MSTR> cOPR16_ITEMS_MSTR_LIST;

            try
            {
                cOPR16_ITEMS_MSTR_LIST = await db.COPR16_ITEMS_MSTR.Where(l => l.MODLE_ID.Equals(id)).ToListAsync();
            }catch(Exception e)
            {
                cOPR16_ITEMS_MSTR_LIST = new List<COPR16_ITEMS_MSTR>();
            }

            try
            {
                cOPR16_ITEMS_MSTR = await db.COPR16_ITEMS_MSTR.Where(l => l.MODLE_ID.Equals(id)).FirstAsync();
            }
            catch (Exception e)
            {
                cOPR16_ITEMS_MSTR = new COPR16_ITEMS_MSTR();
                cOPR16_ITEMS_MSTR.ADATE = AppPropModel.today;
                cOPR16_ITEMS_MSTR.MOD_DATE = AppPropModel.today;
                cOPR16_ITEMS_MSTR.CRE_BY = usr;
                cOPR16_ITEMS_MSTR.MOD_BY = usr;
            }


            if (cOPR16_MODEL_MSTR == null)
            {
                return HttpNotFound();
            }
            model.cOPR16_MODEL_MSTR = cOPR16_MODEL_MSTR;
            model.cOPR16_ITEMS_MSTR_LIST = cOPR16_ITEMS_MSTR_LIST;
            model.cOPR16_ITEMS_MSTR = cOPR16_ITEMS_MSTR;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        // GET: COPR16_ITEMS_MSTR/Edit/5
        public async Task<ActionResult> EditModel(string MODEL_ID,string LINE_ID, string UID)
        {
            COPR16_MODEL_MSTR cOPR16_MODEL_MSTR = null;
            string db_MODELID = "";
            string db_LINEID = "";
            string usr = UID;
            string strModelID = "";
            string strLineID = "";
            COPR16_ITEMS_MSTR cOPR16_ITEMS_MSTR;
            List<COPR16_ITEMS_MSTR> cOPR16_ITEMS_MSTR_LIST;
            if (MODEL_ID == null || LINE_ID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            }
            else
            {
                strModelID = Regex.Replace(MODEL_ID, @"\p{C}+", string.Empty);
                strLineID = Regex.Replace(LINE_ID, @"\p{C}+", string.Empty);
            }

            using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DatabaseServer"].ToString()))
            {
                if (con.State != ConnectionState.Open)
                {
                    await con.OpenAsync();
                }
                
                using (var cmd = con.CreateCommand())
                {
                    //cmd.CommandText = "SELECT * FROM COPR16_MODEL_MSTR WHERE MODEL_ID = @MODELID;";
                    cmd.CommandText = "SELECT * FROM COPR16_MODEL_MSTR WHERE MODEL_ID = '"+ strModelID + "';";
                    cmd.Parameters.Add(new SqlParameter("@MODELID", MODEL_ID == null ? "" : MODEL_ID));
                    DbDataReader reader = await cmd.ExecuteReaderAsync();
                    {
                        db_MODELID = Utils.getValueFromReader(reader,"MODEL_ID");
                    }
                    reader.Close();
                }
                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = "SELECT TOP 1 * FROM COPR16_LINE_MSTR WHERE LINE_ID = '" + strLineID + "'";

                    DbDataReader reader = await cmd.ExecuteReaderAsync();
                    {
                        db_LINEID = Utils.getValueFromReader(reader, "LINE_ID");
                    }
                    reader.Close();
                }

                try
                {
                    cOPR16_ITEMS_MSTR = await db.COPR16_ITEMS_MSTR.Where(l => l.MODLE_ID.Equals(db_MODELID) && l.LINE_ID.Equals(db_LINEID)).FirstOrDefaultAsync();
                }
                catch (Exception e)
                {
                    cOPR16_ITEMS_MSTR = new COPR16_ITEMS_MSTR();
                    cOPR16_ITEMS_MSTR.ADATE = AppPropModel.today;
                    cOPR16_ITEMS_MSTR.MOD_DATE = AppPropModel.today;
                    cOPR16_ITEMS_MSTR.CRE_BY = usr;
                    cOPR16_ITEMS_MSTR.MOD_BY = usr;
                }
                try
                {
                    string SQL_CMD = string.Format("SELECT * FROM COPR16_ITEMS_MSTR WHERE MODLE_ID = '{0}' AND LINE_ID = '{1}' ORDER BY POS_ID;", db_MODELID, db_LINEID);
                    cOPR16_ITEMS_MSTR_LIST = await db.Database.SqlQuery<COPR16_ITEMS_MSTR>(SQL_CMD).ToListAsync();// ToList();
                }
                catch (Exception e)
                {
                    cOPR16_ITEMS_MSTR_LIST = new List<COPR16_ITEMS_MSTR>();
                }

                if (db_MODELID == "")
                {
                    return HttpNotFound();
                }
            }

            //string usr = System.Web.HttpContext.Current.User.Identity.Name.Substring(System.Web.HttpContext.Current.User.Identity.Name.IndexOf(@"\") + 1);




            ItemMatchModel model = new ItemMatchModel(db);

            model.cOPR16_MODEL_MSTR = cOPR16_MODEL_MSTR;
            model.cOPR16_ITEMS_MSTR_LIST = cOPR16_ITEMS_MSTR_LIST;
            model.cOPR16_ITEMS_MSTR = cOPR16_ITEMS_MSTR;
            model.xModel = db_MODELID;
            model.xLine = db_LINEID;
            return View(model);
        }

        [HttpPost]
        //public async  Task<JsonResult> GetVolumeMatrix(VolumeMatrixParams parms)
        public async Task<JsonResult> GetVolumeMatrix(VolumeMatrixParams parms)
        {
            var rowData = new JsonResult();
            using (var cmd = db.Database.Connection.CreateCommand())
            {
                await db.Database.Connection.OpenAsync();
                cmd.CommandText = "exec [dbo].[COPR16_LIST_ITEMS_VOLUME] @FROM_DT,@TO_DT,@ITEM_ID";
                cmd.Parameters.Add(new SqlParameter("@FROM_DT", parms.FROM_DATE));
                cmd.Parameters.Add(new SqlParameter("@TO_DT", parms.TO_DATE));
                cmd.Parameters.Add(new SqlParameter("@ITEM_ID", parms.ITEM_ID == null ? "" : parms.ITEM_ID));
                DbDataReader reader = await cmd.ExecuteReaderAsync();
                {
                    var model = Serialize((SqlDataReader)reader);
                    rowData.Data = model;
                }
            }
            return Json(rowData, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        //public async  Task<JsonResult> GetVolumeMatrix(VolumeMatrixParams parms)
        
        public async Task<JsonResult> GetVolumeMatrixWithModel(VolumeMatrixParams parms)
        {
            var rowData = new JsonResult();
            pReturnMatrix rowsReturn = new pReturnMatrix();
            using (var cmd = db.Database.Connection.CreateCommand())
            {
                if (db.Database.Connection.State != ConnectionState.Open )
                {
                    if(db.Database.Connection.State == ConnectionState.Closed)
                    {
                        await db.Database.Connection.OpenAsync();
                    }
                    
                }
                
                cmd.CommandText = "exec [dbo].[sp_list_items_volume_with_model] @FROM_DT,@TO_DT,@ITEM_ID,@MODEL_ID";
                cmd.Parameters.Add(new SqlParameter("@FROM_DT", parms.FROM_DATE));
                cmd.Parameters.Add(new SqlParameter("@TO_DT", parms.TO_DATE));
                cmd.Parameters.Add(new SqlParameter("@ITEM_ID", parms.ITEM_ID == null ? "" : parms.ITEM_ID));
                cmd.Parameters.Add(new SqlParameter("@MODEL_ID", parms.MODEL_ID == null ? "" : parms.MODEL_ID));
                DbDataReader reader = await cmd.ExecuteReaderAsync();
                {
                    var model = Serialize((SqlDataReader)reader);
                    //rowData.Data = model;
                    rowsReturn.QTY = model;
                }
                reader.Close();
            }

            /** Daily Sum BY Model **/
            /** Show with date and Model ***/
            using (var cmd2 = db.Database.Connection.CreateCommand())
            {
                if(db.Database.Connection.State != ConnectionState.Open)
                {
                    if (db.Database.Connection.State == ConnectionState.Closed)
                    {
                        await db.Database.Connection.OpenAsync();
                    }
                }

                //cmd.CommandText = "exec [dbo].[sp_list_items_volume_with_model_aqty] @FROM_DT,@TO_DT,@ITEM_ID,@MODEL_ID";
                cmd2.CommandText = "exec [dbo].[sp_list_items_volume_with_model_sumqty] @FROM_DT,@TO_DT,@MODEL_ID,@ITEM_ID";

                cmd2.Parameters.Add(new SqlParameter("@FROM_DT", parms.FROM_DATE));
                cmd2.Parameters.Add(new SqlParameter("@TO_DT", parms.TO_DATE));
                cmd2.Parameters.Add(new SqlParameter("@MODEL_ID", parms.MODEL_ID == null ? "" : parms.MODEL_ID));
                cmd2.Parameters.Add(new SqlParameter("@ITEM_ID", parms.ITEM_ID == null ? "" : parms.ITEM_ID));
                DbDataReader reader2 = await cmd2.ExecuteReaderAsync();
                {
                    var model = Serialize((SqlDataReader)reader2);
                    //rowData.Data = model;
                    rowsReturn.AQTY = model;
                }
                reader2.Close();

            }


            /** Daily Commulative Sum BY Model **/
            /** Group by Model                 **/
            using (var cmd3 = db.Database.Connection.CreateCommand())
            {
                if (db.Database.Connection.State != ConnectionState.Open)
                {
                    await db.Database.Connection.OpenAsync();
                }

                //cmd.CommandText = "exec [dbo].[sp_list_items_volume_with_model_aqty] @FROM_DT,@TO_DT,@ITEM_ID,@MODEL_ID";
                cmd3.CommandText = "exec [dbo].[sp_list_items_volume_with_model_sumacc] @FROM_DT,@TO_DT,@MODEL_ID,@ITEM_ID";

                cmd3.Parameters.Add(new SqlParameter("@FROM_DT", parms.FROM_DATE));
                cmd3.Parameters.Add(new SqlParameter("@TO_DT", parms.TO_DATE));
                cmd3.Parameters.Add(new SqlParameter("@MODEL_ID", parms.MODEL_ID == null ? "" : parms.MODEL_ID));
                cmd3.Parameters.Add(new SqlParameter("@ITEM_ID", parms.ITEM_ID == null ? "" : parms.ITEM_ID));
                DbDataReader reader = await cmd3.ExecuteReaderAsync();
                {
                    var model = Serialize((SqlDataReader)reader);
                    //rowData.Data = model;
                    rowsReturn.ACCQTY = model;
                }
                reader.Close();

            }
            using (var cmd4 = db.Database.Connection.CreateCommand())
            {
                if (db.Database.Connection.State != ConnectionState.Open)
                {
                    await db.Database.Connection.OpenAsync();
                }

                //cmd.CommandText = "exec [dbo].[sp_list_items_volume_with_model_aqty] @FROM_DT,@TO_DT,@ITEM_ID,@MODEL_ID";
                cmd4.CommandText = "exec [dbo].[sp_list_items_volume_with_model_copno] @FROM_DT,@TO_DT,@MODEL_ID,@ITEM_ID";

                cmd4.Parameters.Add(new SqlParameter("@FROM_DT", parms.FROM_DATE));
                cmd4.Parameters.Add(new SqlParameter("@TO_DT", parms.TO_DATE));
                cmd4.Parameters.Add(new SqlParameter("@MODEL_ID", parms.MODEL_ID == null ? "" : parms.MODEL_ID));
                cmd4.Parameters.Add(new SqlParameter("@ITEM_ID", parms.ITEM_ID == null ? "" : parms.ITEM_ID));
                DbDataReader reader = await cmd4.ExecuteReaderAsync();
                {
                    var model = Serialize((SqlDataReader)reader);
                    //rowData.Data = model;
                    rowsReturn.COPNO = model;
                }
                reader.Close();

            }
            using (var cmd4 = db.Database.Connection.CreateCommand())
            {
                if (db.Database.Connection.State != ConnectionState.Open)
                {
                    await db.Database.Connection.OpenAsync();
                }

                //cmd.CommandText = "exec [dbo].[sp_list_items_volume_with_model_aqty] @FROM_DT,@TO_DT,@ITEM_ID,@MODEL_ID";
                cmd4.CommandText = "exec [dbo].[sp_list_items_volume_with_model_copno_ex] @FROM_DT,@TO_DT,@MODEL_ID,@ITEM_ID";

                cmd4.Parameters.Add(new SqlParameter("@FROM_DT", parms.FROM_DATE));
                cmd4.Parameters.Add(new SqlParameter("@TO_DT", parms.TO_DATE));
                cmd4.Parameters.Add(new SqlParameter("@MODEL_ID", parms.MODEL_ID == null ? "" : parms.MODEL_ID));
                cmd4.Parameters.Add(new SqlParameter("@ITEM_ID", parms.ITEM_ID == null ? "" : parms.ITEM_ID));
                DbDataReader reader = await cmd4.ExecuteReaderAsync();
                {
                    var model = Serialize((SqlDataReader)reader);
                    //rowData.Data = model;
                    rowsReturn.COPNOEX = model;
                }
                reader.Close();

            }
            using (var cmd5 = db.Database.Connection.CreateCommand())
            {
                if (db.Database.Connection.State != ConnectionState.Open)
                {
                    await db.Database.Connection.OpenAsync();
                }

                //cmd.CommandText = "exec [dbo].[sp_list_items_volume_with_model_aqty] @FROM_DT,@TO_DT,@ITEM_ID,@MODEL_ID";
                cmd5.CommandText = "exec [dbo].[sp_list_items_volume_with_model_copqty] @FROM_DT,@TO_DT,@MODEL_ID,@ITEM_ID";

                cmd5.Parameters.Add(new SqlParameter("@FROM_DT", parms.FROM_DATE));
                cmd5.Parameters.Add(new SqlParameter("@TO_DT", parms.TO_DATE));
                cmd5.Parameters.Add(new SqlParameter("@MODEL_ID", parms.MODEL_ID == null ? "" : parms.MODEL_ID));
                cmd5.Parameters.Add(new SqlParameter("@ITEM_ID", parms.ITEM_ID == null ? "" : parms.ITEM_ID));
                DbDataReader reader = await cmd5.ExecuteReaderAsync();
                {
                    var model = Serialize((SqlDataReader)reader);
                    //rowData.Data = model;
                    rowsReturn.COPQTY = model;
                }
                reader.Close();

            }

            using (var cmd6 = db.Database.Connection.CreateCommand())
            {
                if (db.Database.Connection.State != ConnectionState.Open)
                {
                    await db.Database.Connection.OpenAsync();
                }

                //cmd.CommandText = "exec [dbo].[sp_list_items_volume_with_model_aqty] @FROM_DT,@TO_DT,@ITEM_ID,@MODEL_ID";
                cmd6.CommandText = "exec [dbo].[sp_list_items_volume_with_model_copqty_ex] @FROM_DT,@TO_DT,@MODEL_ID,@ITEM_ID";

                cmd6.Parameters.Add(new SqlParameter("@FROM_DT", parms.FROM_DATE));
                cmd6.Parameters.Add(new SqlParameter("@TO_DT", parms.TO_DATE));
                cmd6.Parameters.Add(new SqlParameter("@MODEL_ID", parms.MODEL_ID == null ? "" : parms.MODEL_ID));
                cmd6.Parameters.Add(new SqlParameter("@ITEM_ID", parms.ITEM_ID == null ? "" : parms.ITEM_ID));
                DbDataReader reader = await cmd6.ExecuteReaderAsync();
                {
                    var model = Serialize((SqlDataReader)reader);
                    //rowData.Data = model;
                    rowsReturn.COPQTYEX = model;
                }
                reader.Close();

            }

            using (var cmd6 = db.Database.Connection.CreateCommand())
            {
                if (db.Database.Connection.State != ConnectionState.Open)
                {
                    await db.Database.Connection.OpenAsync();
                }

                //cmd.CommandText = "exec [dbo].[sp_list_items_volume_with_model_aqty] @FROM_DT,@TO_DT,@ITEM_ID,@MODEL_ID";
                cmd6.CommandText = "EXEC SP_SUMMARY_MODEL_TRACKING @VALUE_QTY,@PROC_VALUE_VAR,@MODEL_VALUE,@PARTNO_VALUE ";

                cmd6.Parameters.Add(new SqlParameter("@VALUE_QTY", 5000));
                cmd6.Parameters.Add(new SqlParameter("@PROC_VALUE_VAR", "01"));
                cmd6.Parameters.Add(new SqlParameter("@MODEL_VALUE", parms.MODEL_ID == null ? "" : parms.MODEL_ID));
                cmd6.Parameters.Add(new SqlParameter("@PARTNO_VALUE ", parms.ITEM_ID == null ? "" : parms.ITEM_ID));
                DbDataReader reader = await cmd6.ExecuteReaderAsync();
                {
                    var model = Serialize((SqlDataReader)reader);
                    //rowData.Data = model;
                    rowsReturn.COPQTY_SUMMARY1 = model;
                }
                reader.Close();

            }

            using (var cmd6 = db.Database.Connection.CreateCommand())
            {
                if (db.Database.Connection.State != ConnectionState.Open)
                {
                    await db.Database.Connection.OpenAsync();
                }

                //cmd.CommandText = "exec [dbo].[sp_list_items_volume_with_model_aqty] @FROM_DT,@TO_DT,@ITEM_ID,@MODEL_ID";
                cmd6.CommandText = "EXEC SP_SUMMARY_MODEL_TRACKING @VALUE_QTY,@PROC_VALUE_VAR,@MODEL_VALUE,@PARTNO_VALUE ";

                cmd6.Parameters.Add(new SqlParameter("@VALUE_QTY", 10000));
                cmd6.Parameters.Add(new SqlParameter("@PROC_VALUE_VAR", "02"));
                cmd6.Parameters.Add(new SqlParameter("@MODEL_VALUE", parms.MODEL_ID == null ? "" : parms.MODEL_ID));
                cmd6.Parameters.Add(new SqlParameter("@PARTNO_VALUE ", parms.ITEM_ID == null ? "" : parms.ITEM_ID));

                DbDataReader reader = await cmd6.ExecuteReaderAsync();
                {
                    var model = Serialize((SqlDataReader)reader);
                    //rowData.Data = model;
                    rowsReturn.COPQTY_SUMMARY2 = model;
                }
                reader.Close();

            }
            using (var cmd6 = db.Database.Connection.CreateCommand())
            {
                if (db.Database.Connection.State != ConnectionState.Open)
                {
                    await db.Database.Connection.OpenAsync();
                }
                //cmd.CommandText = "exec [dbo].[sp_list_items_volume_with_model_aqty] @FROM_DT,@TO_DT,@ITEM_ID,@MODEL_ID";
                cmd6.CommandText = "EXEC SP_GET_MODEL_TRACKING @QTY,@MODEL_ID,@FROM_DT,@TO_DT";
                cmd6.Parameters.Add(new SqlParameter("@QTY", 5000));
                cmd6.Parameters.Add(new SqlParameter("@MODEL_ID", parms.MODEL_ID == null ? "" : parms.MODEL_ID));
                cmd6.Parameters.Add(new SqlParameter("@FROM_DT", parms.FROM_DATE));
                cmd6.Parameters.Add(new SqlParameter("@TO_DT", parms.TO_DATE));

                DbDataReader reader = await cmd6.ExecuteReaderAsync();
                {
                    var model = Serialize((SqlDataReader)reader);
                    //rowData.Data = model;
                    rowsReturn.COPQTY_SUGGESTION1 = model;
                }
                reader.Close();

            }
            using (var cmd6 = db.Database.Connection.CreateCommand())
            {
                if (db.Database.Connection.State != ConnectionState.Open)
                {
                    await db.Database.Connection.OpenAsync();
                }
                //cmd.CommandText = "exec [dbo].[sp_list_items_volume_with_model_aqty] @FROM_DT,@TO_DT,@ITEM_ID,@MODEL_ID";
                cmd6.CommandText = "EXEC SP_GET_MODEL_TRACKING @QTY,@MODEL_ID,@FROM_DT,@TO_DT";
                cmd6.Parameters.Add(new SqlParameter("@QTY", 10000));
                cmd6.Parameters.Add(new SqlParameter("@MODEL_ID", parms.MODEL_ID == null ? "" : parms.MODEL_ID));
                cmd6.Parameters.Add(new SqlParameter("@FROM_DT", parms.FROM_DATE));
                cmd6.Parameters.Add(new SqlParameter("@TO_DT", parms.TO_DATE));

                DbDataReader reader = await cmd6.ExecuteReaderAsync();
                {
                    var model = Serialize((SqlDataReader)reader);
                    //rowData.Data = model;
                    rowsReturn.COPQTY_SUGGESTION2 = model;
                }
                reader.Close();

            }
            using (var cmd6 = db.Database.Connection.CreateCommand())
            {
                if (db.Database.Connection.State != ConnectionState.Open)
                {
                    await db.Database.Connection.OpenAsync();
                }

                //cmd.CommandText = "exec [dbo].[sp_list_items_volume_with_model_aqty] @FROM_DT,@TO_DT,@ITEM_ID,@MODEL_ID";
                cmd6.CommandText = "exec [dbo].[SP_GET_DIMDATE] @FROM_DT,@TO_DT ";

                cmd6.Parameters.Add(new SqlParameter("@FROM_DT", parms.FROM_DATE));
                cmd6.Parameters.Add(new SqlParameter("@TO_DT", parms.TO_DATE));
                DbDataReader reader = await cmd6.ExecuteReaderAsync();
                {
                    var model = Serialize((SqlDataReader)reader);
                    //rowData.Data = model;
                    rowsReturn.DIM_DATE = model;
                }
                reader.Close();

            }
            using (var cmd6 = db.Database.Connection.CreateCommand())
            {
                if (db.Database.Connection.State != ConnectionState.Open)
                {
                    await db.Database.Connection.OpenAsync();
                }
                //cmd.CommandText = "exec [dbo].[sp_list_items_volume_with_model_aqty] @FROM_DT,@TO_DT,@ITEM_ID,@MODEL_ID";
                cmd6.CommandText = "EXEC SP_GET_MODEL_REM @QTY,@MODEL_ID,@FROM_DT,@TO_DT";
                cmd6.Parameters.Add(new SqlParameter("@QTY", 5000));
                cmd6.Parameters.Add(new SqlParameter("@MODEL_ID", parms.MODEL_ID == null ? "" : parms.MODEL_ID));
                cmd6.Parameters.Add(new SqlParameter("@FROM_DT", parms.FROM_DATE));
                cmd6.Parameters.Add(new SqlParameter("@TO_DT", parms.TO_DATE));

                DbDataReader reader = await cmd6.ExecuteReaderAsync();
                {
                    var model = Serialize((SqlDataReader)reader);
                    //rowData.Data = model;
                    rowsReturn.MODEL_REM1 = model;
                }
                reader.Close();

            }
            using (var cmd6 = db.Database.Connection.CreateCommand())
            {
                if (db.Database.Connection.State != ConnectionState.Open)
                {
                    await db.Database.Connection.OpenAsync();
                }
                //cmd.CommandText = "exec [dbo].[sp_list_items_volume_with_model_aqty] @FROM_DT,@TO_DT,@ITEM_ID,@MODEL_ID";
                cmd6.CommandText = "EXEC SP_GET_MODEL_REM @QTY,@MODEL_ID,@FROM_DT,@TO_DT";
                cmd6.Parameters.Add(new SqlParameter("@QTY", 10000));
                cmd6.Parameters.Add(new SqlParameter("@MODEL_ID", parms.MODEL_ID == null ? "" : parms.MODEL_ID));
                cmd6.Parameters.Add(new SqlParameter("@FROM_DT", parms.FROM_DATE));
                cmd6.Parameters.Add(new SqlParameter("@TO_DT", parms.TO_DATE));

                DbDataReader reader = await cmd6.ExecuteReaderAsync();
                {
                    var model = Serialize((SqlDataReader)reader);
                    //rowData.Data = model;
                    rowsReturn.MODEL_REM2 = model;
                }
                reader.Close();

            }

            rowData.Data = rowsReturn;
            return Json(rowData, JsonRequestBehavior.AllowGet);
        }
        [AsyncTimeout(600000)]
        public async Task<JsonResult> GetVolTrackingByModel(VolumeMatrixParams parms)
        {
            var rowData = new JsonResult();
            pReturnMatrix rowsReturn = new pReturnMatrix();

            
            using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DatabaseServer"].ToString()))
            {
               

                await con.OpenAsync();

                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandTimeout = 60000;
                    cmd.CommandText = "exec COPR16_GET_PART_VOLUM_TRACKING_BY_MODEL @FROM_DT,@TO_DT,@MODEL_ID";
                    cmd.Parameters.Add(new SqlParameter("@FROM_DT", parms.FROM_DATE));
                    cmd.Parameters.Add(new SqlParameter("@TO_DT", parms.TO_DATE));
//                    cmd.Parameters.Add(new SqlParameter("@ITEM_ID", parms.ITEM_ID == null ? "" : parms.ITEM_ID));
                    cmd.Parameters.Add(new SqlParameter("@MODEL_ID", parms.MODEL_ID == null ? "" : parms.MODEL_ID));
                    DbDataReader reader = await cmd.ExecuteReaderAsync();
                    {
                        var model = Serialize((SqlDataReader)reader);
                        //rowData.Data = model;
                        rowsReturn.QTY = model;
                    }
                    reader.Close();
                }
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandTimeout = 60000;
                    cmd.CommandText = "exec COPR16_GET_SUM_VOLUM_TRACKING_BY_MODEL @FROM_DT,@TO_DT,@MODEL_ID";
                    cmd.Parameters.Add(new SqlParameter("@FROM_DT", parms.FROM_DATE));
                    cmd.Parameters.Add(new SqlParameter("@TO_DT", parms.TO_DATE));
                    //                    cmd.Parameters.Add(new SqlParameter("@ITEM_ID", parms.ITEM_ID == null ? "" : parms.ITEM_ID));
                    cmd.Parameters.Add(new SqlParameter("@MODEL_ID", parms.MODEL_ID == null ? "" : parms.MODEL_ID));
                    DbDataReader reader = await cmd.ExecuteReaderAsync();
                    {
                        var model = Serialize((SqlDataReader)reader);
                        //rowData.Data = model;
                        rowsReturn.AQTY = model;
                    }
                    reader.Close();
                }
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandTimeout = 60000;
                    cmd.CommandText = "exec COPR16_GET_ACC_VOLUM_TRACKING_BY_MODEL @FROM_DT,@TO_DT,@MODEL_ID";
                    cmd.Parameters.Add(new SqlParameter("@FROM_DT", parms.FROM_DATE));
                    cmd.Parameters.Add(new SqlParameter("@TO_DT", parms.TO_DATE));
                    //                    cmd.Parameters.Add(new SqlParameter("@ITEM_ID", parms.ITEM_ID == null ? "" : parms.ITEM_ID));
                    cmd.Parameters.Add(new SqlParameter("@MODEL_ID", parms.MODEL_ID == null ? "" : parms.MODEL_ID));
                    DbDataReader reader = await cmd.ExecuteReaderAsync();
                    {
                        var model = Serialize((SqlDataReader)reader);
                        //rowData.Data = model;
                        rowsReturn.ACCQTY = model;
                    }
                    reader.Close();
                }
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandTimeout = 60000;
                    cmd.CommandText = "exec COPR16_GET_CONDITION_TRIGGER_BY_MODEL @FROM_DT,@TO_DT,@MODEL_ID,@COP_TYPE";
                    cmd.Parameters.Add(new SqlParameter("@FROM_DT", parms.FROM_DATE));
                    cmd.Parameters.Add(new SqlParameter("@TO_DT", parms.TO_DATE));
                    //                    cmd.Parameters.Add(new SqlParameter("@ITEM_ID", parms.ITEM_ID == null ? "" : parms.ITEM_ID));
                    cmd.Parameters.Add(new SqlParameter("@MODEL_ID", parms.MODEL_ID == null ? "" : parms.MODEL_ID));
                    cmd.Parameters.Add(new SqlParameter("@COP_TYPE", 1));
                    DbDataReader reader = await cmd.ExecuteReaderAsync();
                    {
                        var model = Serialize((SqlDataReader)reader);
                        //rowData.Data = model;
                        rowsReturn.COPQTY_SUGGESTION1 = model;
                    }
                    reader.Close();
                }
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandTimeout = 60000;
                    cmd.CommandText = "exec COPR16_GET_CONDITION_TRIGGER_BY_MODEL @FROM_DT,@TO_DT,@MODEL_ID,@COP_TYPE";
                    cmd.Parameters.Add(new SqlParameter("@FROM_DT", parms.FROM_DATE));
                    cmd.Parameters.Add(new SqlParameter("@TO_DT", parms.TO_DATE));
                    cmd.Parameters.Add(new SqlParameter("@MODEL_ID", parms.MODEL_ID == null ? "" : parms.MODEL_ID));
                    cmd.Parameters.Add(new SqlParameter("@COP_TYPE", 2));
                    DbDataReader reader = await cmd.ExecuteReaderAsync();
                    {
                        var model = Serialize((SqlDataReader)reader);
                        //rowData.Data = model;
                        rowsReturn.COPQTY_SUGGESTION2 = model;
                    }
                    reader.Close();
                }
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandTimeout = 60000;
                    cmd.CommandText = "exec COPR16_GET_COPNO_BY_MODEL @FROM_DT,@TO_DT,@MODEL_ID,@COP_TYPE";
                    cmd.Parameters.Add(new SqlParameter("@FROM_DT", parms.FROM_DATE));
                    cmd.Parameters.Add(new SqlParameter("@TO_DT", parms.TO_DATE));
                    cmd.Parameters.Add(new SqlParameter("@MODEL_ID", parms.MODEL_ID == null ? "" : parms.MODEL_ID));
                    cmd.Parameters.Add(new SqlParameter("@COP_TYPE", 1));
                    DbDataReader reader = await cmd.ExecuteReaderAsync();
                    {
                        var model = Serialize((SqlDataReader)reader);
                        //rowData.Data = model;
                        rowsReturn.COPNO = model;
                    }
                    reader.Close();
                }
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandTimeout = 60000;
                    cmd.CommandText = "exec COPR16_GET_COPNO_BY_MODEL @FROM_DT,@TO_DT,@MODEL_ID,@COP_TYPE";
                    cmd.Parameters.Add(new SqlParameter("@FROM_DT", parms.FROM_DATE));
                    cmd.Parameters.Add(new SqlParameter("@TO_DT", parms.TO_DATE));
                    cmd.Parameters.Add(new SqlParameter("@MODEL_ID", parms.MODEL_ID == null ? "" : parms.MODEL_ID));
                    cmd.Parameters.Add(new SqlParameter("@COP_TYPE", 2));
                    DbDataReader reader = await cmd.ExecuteReaderAsync();
                    {
                        var model = Serialize((SqlDataReader)reader);
                        //rowData.Data = model;
                        rowsReturn.COPNOEX = model;
                    }
                    reader.Close();
                }
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandTimeout = 60000;
                    cmd.CommandText = "exec COPR16_GET_COPLOT_BY_MODEL @FROM_DT,@TO_DT,@MODEL_ID,@COP_TYPE";
                    cmd.Parameters.Add(new SqlParameter("@FROM_DT", parms.FROM_DATE));
                    cmd.Parameters.Add(new SqlParameter("@TO_DT", parms.TO_DATE));
                    cmd.Parameters.Add(new SqlParameter("@MODEL_ID", parms.MODEL_ID == null ? "" : parms.MODEL_ID));
                    cmd.Parameters.Add(new SqlParameter("@COP_TYPE", 1));
                    DbDataReader reader = await cmd.ExecuteReaderAsync();
                    {
                        var model = Serialize((SqlDataReader)reader);
                        //rowData.Data = model;
                        rowsReturn.COPQTY_SUMMARY1 = model;
                    }
                    reader.Close();
                }
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandTimeout = 60000;
                    cmd.CommandText = "exec COPR16_GET_COPLOT_BY_MODEL @FROM_DT,@TO_DT,@MODEL_ID,@COP_TYPE";
                    cmd.Parameters.Add(new SqlParameter("@FROM_DT", parms.FROM_DATE));
                    cmd.Parameters.Add(new SqlParameter("@TO_DT", parms.TO_DATE));
                    cmd.Parameters.Add(new SqlParameter("@MODEL_ID", parms.MODEL_ID == null ? "" : parms.MODEL_ID));
                    cmd.Parameters.Add(new SqlParameter("@COP_TYPE", 2));
                    DbDataReader reader = await cmd.ExecuteReaderAsync();
                    {
                        var model = Serialize((SqlDataReader)reader);
                        //rowData.Data = model;
                        rowsReturn.COPQTY_SUMMARY2 = model;
                    }
                    reader.Close();
                }
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandTimeout = 60000;
                    cmd.CommandText = "exec COPR16_GET_LAST_VOL_BY_MODEL @FROM_DT,@MODEL_ID";
                    cmd.Parameters.Add(new SqlParameter("@FROM_DT", parms.FROM_DATE));
                    cmd.Parameters.Add(new SqlParameter("@MODEL_ID", parms.MODEL_ID == null ? "" : parms.MODEL_ID));
                    DbDataReader reader = await cmd.ExecuteReaderAsync();
                    {
                        var model = Serialize((SqlDataReader)reader);
                        //rowData.Data = model;
                        rowsReturn.MODEL_REM1 = model;
                    }
                    reader.Close();
                }
                con.Close();
            }

            /*
            //Daily Sum BY Model
            //Show with date and Model
            using (var cmd2 = db.Database.Connection.CreateCommand())
            {
                if (db.Database.Connection.State != ConnectionState.Open)
                {
                    if (db.Database.Connection.State == ConnectionState.Closed)
                    {
                        await db.Database.Connection.OpenAsync();
                    }
                }

                //cmd.CommandText = "exec [dbo].[sp_list_items_volume_with_model_aqty] @FROM_DT,@TO_DT,@ITEM_ID,@MODEL_ID";
                cmd2.CommandText = "exec [dbo].[sp_list_items_volume_with_model_sumqty] @FROM_DT,@TO_DT,@MODEL_ID,@ITEM_ID";

                cmd2.Parameters.Add(new SqlParameter("@FROM_DT", parms.FROM_DATE));
                cmd2.Parameters.Add(new SqlParameter("@TO_DT", parms.TO_DATE));
                cmd2.Parameters.Add(new SqlParameter("@MODEL_ID", parms.MODEL_ID == null ? "" : parms.MODEL_ID));
                cmd2.Parameters.Add(new SqlParameter("@ITEM_ID", parms.ITEM_ID == null ? "" : parms.ITEM_ID));
                DbDataReader reader2 = await cmd2.ExecuteReaderAsync();
                {
                    var model = Serialize((SqlDataReader)reader2);
                    //rowData.Data = model;
                    rowsReturn.AQTY = model;
                }
                reader2.Close();

            }


            //Daily Commulative Sum BY Model 
            //roup by Model                 
            using (var cmd3 = db.Database.Connection.CreateCommand())
            {
                if (db.Database.Connection.State != ConnectionState.Open)
                {
                    await db.Database.Connection.OpenAsync();
                }

                //cmd.CommandText = "exec [dbo].[sp_list_items_volume_with_model_aqty] @FROM_DT,@TO_DT,@ITEM_ID,@MODEL_ID";
                cmd3.CommandText = "exec [dbo].[sp_list_items_volume_with_model_sumacc] @FROM_DT,@TO_DT,@MODEL_ID,@ITEM_ID";

                cmd3.Parameters.Add(new SqlParameter("@FROM_DT", parms.FROM_DATE));
                cmd3.Parameters.Add(new SqlParameter("@TO_DT", parms.TO_DATE));
                cmd3.Parameters.Add(new SqlParameter("@MODEL_ID", parms.MODEL_ID == null ? "" : parms.MODEL_ID));
                cmd3.Parameters.Add(new SqlParameter("@ITEM_ID", parms.ITEM_ID == null ? "" : parms.ITEM_ID));
                DbDataReader reader = await cmd3.ExecuteReaderAsync();
                {
                    var model = Serialize((SqlDataReader)reader);
                    //rowData.Data = model;
                    rowsReturn.ACCQTY = model;
                }
                reader.Close();

            }
            using (var cmd4 = db.Database.Connection.CreateCommand())
            {
                if (db.Database.Connection.State != ConnectionState.Open)
                {
                    await db.Database.Connection.OpenAsync();
                }

                //cmd.CommandText = "exec [dbo].[sp_list_items_volume_with_model_aqty] @FROM_DT,@TO_DT,@ITEM_ID,@MODEL_ID";
                cmd4.CommandText = "exec [dbo].[sp_list_items_volume_with_model_copno] @FROM_DT,@TO_DT,@MODEL_ID,@ITEM_ID";

                cmd4.Parameters.Add(new SqlParameter("@FROM_DT", parms.FROM_DATE));
                cmd4.Parameters.Add(new SqlParameter("@TO_DT", parms.TO_DATE));
                cmd4.Parameters.Add(new SqlParameter("@MODEL_ID", parms.MODEL_ID == null ? "" : parms.MODEL_ID));
                cmd4.Parameters.Add(new SqlParameter("@ITEM_ID", parms.ITEM_ID == null ? "" : parms.ITEM_ID));
                DbDataReader reader = await cmd4.ExecuteReaderAsync();
                {
                    var model = Serialize((SqlDataReader)reader);
                    //rowData.Data = model;
                    rowsReturn.COPNO = model;
                }
                reader.Close();

            }
            using (var cmd4 = db.Database.Connection.CreateCommand())
            {
                if (db.Database.Connection.State != ConnectionState.Open)
                {
                    await db.Database.Connection.OpenAsync();
                }

                //cmd.CommandText = "exec [dbo].[sp_list_items_volume_with_model_aqty] @FROM_DT,@TO_DT,@ITEM_ID,@MODEL_ID";
                cmd4.CommandText = "exec [dbo].[sp_list_items_volume_with_model_copno_ex] @FROM_DT,@TO_DT,@MODEL_ID,@ITEM_ID";

                cmd4.Parameters.Add(new SqlParameter("@FROM_DT", parms.FROM_DATE));
                cmd4.Parameters.Add(new SqlParameter("@TO_DT", parms.TO_DATE));
                cmd4.Parameters.Add(new SqlParameter("@MODEL_ID", parms.MODEL_ID == null ? "" : parms.MODEL_ID));
                cmd4.Parameters.Add(new SqlParameter("@ITEM_ID", parms.ITEM_ID == null ? "" : parms.ITEM_ID));
                DbDataReader reader = await cmd4.ExecuteReaderAsync();
                {
                    var model = Serialize((SqlDataReader)reader);
                    //rowData.Data = model;
                    rowsReturn.COPNOEX = model;
                }
                reader.Close();

            }
            using (var cmd5 = db.Database.Connection.CreateCommand())
            {
                if (db.Database.Connection.State != ConnectionState.Open)
                {
                    await db.Database.Connection.OpenAsync();
                }

                //cmd.CommandText = "exec [dbo].[sp_list_items_volume_with_model_aqty] @FROM_DT,@TO_DT,@ITEM_ID,@MODEL_ID";
                cmd5.CommandText = "exec [dbo].[sp_list_items_volume_with_model_copqty] @FROM_DT,@TO_DT,@MODEL_ID,@ITEM_ID";

                cmd5.Parameters.Add(new SqlParameter("@FROM_DT", parms.FROM_DATE));
                cmd5.Parameters.Add(new SqlParameter("@TO_DT", parms.TO_DATE));
                cmd5.Parameters.Add(new SqlParameter("@MODEL_ID", parms.MODEL_ID == null ? "" : parms.MODEL_ID));
                cmd5.Parameters.Add(new SqlParameter("@ITEM_ID", parms.ITEM_ID == null ? "" : parms.ITEM_ID));
                DbDataReader reader = await cmd5.ExecuteReaderAsync();
                {
                    var model = Serialize((SqlDataReader)reader);
                    //rowData.Data = model;
                    rowsReturn.COPQTY = model;
                }
                reader.Close();

            }

            using (var cmd6 = db.Database.Connection.CreateCommand())
            {
                if (db.Database.Connection.State != ConnectionState.Open)
                {
                    await db.Database.Connection.OpenAsync();
                }

                //cmd.CommandText = "exec [dbo].[sp_list_items_volume_with_model_aqty] @FROM_DT,@TO_DT,@ITEM_ID,@MODEL_ID";
                cmd6.CommandText = "exec [dbo].[sp_list_items_volume_with_model_copqty_ex] @FROM_DT,@TO_DT,@MODEL_ID,@ITEM_ID";

                cmd6.Parameters.Add(new SqlParameter("@FROM_DT", parms.FROM_DATE));
                cmd6.Parameters.Add(new SqlParameter("@TO_DT", parms.TO_DATE));
                cmd6.Parameters.Add(new SqlParameter("@MODEL_ID", parms.MODEL_ID == null ? "" : parms.MODEL_ID));
                cmd6.Parameters.Add(new SqlParameter("@ITEM_ID", parms.ITEM_ID == null ? "" : parms.ITEM_ID));
                DbDataReader reader = await cmd6.ExecuteReaderAsync();
                {
                    var model = Serialize((SqlDataReader)reader);
                    //rowData.Data = model;
                    rowsReturn.COPQTYEX = model;
                }
                reader.Close();

            }

            using (var cmd6 = db.Database.Connection.CreateCommand())
            {
                if (db.Database.Connection.State != ConnectionState.Open)
                {
                    await db.Database.Connection.OpenAsync();
                }

                //cmd.CommandText = "exec [dbo].[sp_list_items_volume_with_model_aqty] @FROM_DT,@TO_DT,@ITEM_ID,@MODEL_ID";
                cmd6.CommandText = "EXEC SP_SUMMARY_MODEL_TRACKING @VALUE_QTY,@PROC_VALUE_VAR,@MODEL_VALUE,@PARTNO_VALUE ";

                cmd6.Parameters.Add(new SqlParameter("@VALUE_QTY", 5000));
                cmd6.Parameters.Add(new SqlParameter("@PROC_VALUE_VAR", "01"));
                cmd6.Parameters.Add(new SqlParameter("@MODEL_VALUE", parms.MODEL_ID == null ? "" : parms.MODEL_ID));
                cmd6.Parameters.Add(new SqlParameter("@PARTNO_VALUE ", parms.ITEM_ID == null ? "" : parms.ITEM_ID));
                DbDataReader reader = await cmd6.ExecuteReaderAsync();
                {
                    var model = Serialize((SqlDataReader)reader);
                    //rowData.Data = model;
                    rowsReturn.COPQTY_SUMMARY1 = model;
                }
                reader.Close();

            }

            using (var cmd6 = db.Database.Connection.CreateCommand())
            {
                if (db.Database.Connection.State != ConnectionState.Open)
                {
                    await db.Database.Connection.OpenAsync();
                }

                //cmd.CommandText = "exec [dbo].[sp_list_items_volume_with_model_aqty] @FROM_DT,@TO_DT,@ITEM_ID,@MODEL_ID";
                cmd6.CommandText = "EXEC SP_SUMMARY_MODEL_TRACKING @VALUE_QTY,@PROC_VALUE_VAR,@MODEL_VALUE,@PARTNO_VALUE ";

                cmd6.Parameters.Add(new SqlParameter("@VALUE_QTY", 10000));
                cmd6.Parameters.Add(new SqlParameter("@PROC_VALUE_VAR", "02"));
                cmd6.Parameters.Add(new SqlParameter("@MODEL_VALUE", parms.MODEL_ID == null ? "" : parms.MODEL_ID));
                cmd6.Parameters.Add(new SqlParameter("@PARTNO_VALUE ", parms.ITEM_ID == null ? "" : parms.ITEM_ID));

                DbDataReader reader = await cmd6.ExecuteReaderAsync();
                {
                    var model = Serialize((SqlDataReader)reader);
                    //rowData.Data = model;
                    rowsReturn.COPQTY_SUMMARY2 = model;
                }
                reader.Close();

            }
            using (var cmd6 = db.Database.Connection.CreateCommand())
            {
                if (db.Database.Connection.State != ConnectionState.Open)
                {
                    await db.Database.Connection.OpenAsync();
                }
                //cmd.CommandText = "exec [dbo].[sp_list_items_volume_with_model_aqty] @FROM_DT,@TO_DT,@ITEM_ID,@MODEL_ID";
                cmd6.CommandText = "EXEC SP_GET_MODEL_TRACKING @QTY,@MODEL_ID,@FROM_DT,@TO_DT";
                cmd6.Parameters.Add(new SqlParameter("@QTY", 5000));
                cmd6.Parameters.Add(new SqlParameter("@MODEL_ID", parms.MODEL_ID == null ? "" : parms.MODEL_ID));
                cmd6.Parameters.Add(new SqlParameter("@FROM_DT", parms.FROM_DATE));
                cmd6.Parameters.Add(new SqlParameter("@TO_DT", parms.TO_DATE));

                DbDataReader reader = await cmd6.ExecuteReaderAsync();
                {
                    var model = Serialize((SqlDataReader)reader);
                    //rowData.Data = model;
                    rowsReturn.COPQTY_SUGGESTION1 = model;
                }
                reader.Close();

            }
            using (var cmd6 = db.Database.Connection.CreateCommand())
            {
                if (db.Database.Connection.State != ConnectionState.Open)
                {
                    await db.Database.Connection.OpenAsync();
                }
                //cmd.CommandText = "exec [dbo].[sp_list_items_volume_with_model_aqty] @FROM_DT,@TO_DT,@ITEM_ID,@MODEL_ID";
                cmd6.CommandText = "EXEC SP_GET_MODEL_TRACKING @QTY,@MODEL_ID,@FROM_DT,@TO_DT";
                cmd6.Parameters.Add(new SqlParameter("@QTY", 10000));
                cmd6.Parameters.Add(new SqlParameter("@MODEL_ID", parms.MODEL_ID == null ? "" : parms.MODEL_ID));
                cmd6.Parameters.Add(new SqlParameter("@FROM_DT", parms.FROM_DATE));
                cmd6.Parameters.Add(new SqlParameter("@TO_DT", parms.TO_DATE));

                DbDataReader reader = await cmd6.ExecuteReaderAsync();
                {
                    var model = Serialize((SqlDataReader)reader);
                    //rowData.Data = model;
                    rowsReturn.COPQTY_SUGGESTION2 = model;
                }
                reader.Close();

            }
            using (var cmd6 = db.Database.Connection.CreateCommand())
            {
                if (db.Database.Connection.State != ConnectionState.Open)
                {
                    await db.Database.Connection.OpenAsync();
                }

                //cmd.CommandText = "exec [dbo].[sp_list_items_volume_with_model_aqty] @FROM_DT,@TO_DT,@ITEM_ID,@MODEL_ID";
                cmd6.CommandText = "exec [dbo].[SP_GET_DIMDATE] @FROM_DT,@TO_DT ";

                cmd6.Parameters.Add(new SqlParameter("@FROM_DT", parms.FROM_DATE));
                cmd6.Parameters.Add(new SqlParameter("@TO_DT", parms.TO_DATE));
                DbDataReader reader = await cmd6.ExecuteReaderAsync();
                {
                    var model = Serialize((SqlDataReader)reader);
                    //rowData.Data = model;
                    rowsReturn.DIM_DATE = model;
                }
                reader.Close();

            }
            using (var cmd6 = db.Database.Connection.CreateCommand())
            {
                if (db.Database.Connection.State != ConnectionState.Open)
                {
                    await db.Database.Connection.OpenAsync();
                }
                //cmd.CommandText = "exec [dbo].[sp_list_items_volume_with_model_aqty] @FROM_DT,@TO_DT,@ITEM_ID,@MODEL_ID";
                cmd6.CommandText = "EXEC SP_GET_MODEL_REM @QTY,@MODEL_ID,@FROM_DT,@TO_DT";
                cmd6.Parameters.Add(new SqlParameter("@QTY", 5000));
                cmd6.Parameters.Add(new SqlParameter("@MODEL_ID", parms.MODEL_ID == null ? "" : parms.MODEL_ID));
                cmd6.Parameters.Add(new SqlParameter("@FROM_DT", parms.FROM_DATE));
                cmd6.Parameters.Add(new SqlParameter("@TO_DT", parms.TO_DATE));

                DbDataReader reader = await cmd6.ExecuteReaderAsync();
                {
                    var model = Serialize((SqlDataReader)reader);
                    //rowData.Data = model;
                    rowsReturn.MODEL_REM1 = model;
                }
                reader.Close();

            }
            using (var cmd6 = db.Database.Connection.CreateCommand())
            {
                if (db.Database.Connection.State != ConnectionState.Open)
                {
                    await db.Database.Connection.OpenAsync();
                }
                //cmd.CommandText = "exec [dbo].[sp_list_items_volume_with_model_aqty] @FROM_DT,@TO_DT,@ITEM_ID,@MODEL_ID";
                cmd6.CommandText = "EXEC SP_GET_MODEL_REM @QTY,@MODEL_ID,@FROM_DT,@TO_DT";
                cmd6.Parameters.Add(new SqlParameter("@QTY", 10000));
                cmd6.Parameters.Add(new SqlParameter("@MODEL_ID", parms.MODEL_ID == null ? "" : parms.MODEL_ID));
                cmd6.Parameters.Add(new SqlParameter("@FROM_DT", parms.FROM_DATE));
                cmd6.Parameters.Add(new SqlParameter("@TO_DT", parms.TO_DATE));

                DbDataReader reader = await cmd6.ExecuteReaderAsync();
                {
                    var model = Serialize((SqlDataReader)reader);
                    //rowData.Data = model;
                    rowsReturn.MODEL_REM2 = model;
                }
                reader.Close();

            }
            */
            rowData.Data = rowsReturn;
            return Json(rowData, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        // GET: COPR16_ITEMS_MSTR/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            COPR16_ITEMS_MSTR cOPR16_ITEMS_MSTR = await db.COPR16_ITEMS_MSTR.Where(l => l.MODLE_ID.Equals(id)).FirstAsync();
            ItemsModel model = new ItemsModel(db);

            model.cOPR16_ITEMS_MSTR = cOPR16_ITEMS_MSTR;

            if (cOPR16_ITEMS_MSTR == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }

        // POST: COPR16_ITEMS_MSTR/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ITEM_ID,FG_NO,BRAND_ID,MODLE_ID,LINE_ID,POS_ID,ADATE,CRE_BY,MOD_BY,MOD_DATE,FLGACT")] COPR16_ITEMS_MSTR cOPR16_ITEMS_MSTR)
        {
            if (ModelState.IsValid)
            {
                cOPR16_ITEMS_MSTR.MOD_DATE = System.DateTime.Now;
                cOPR16_ITEMS_MSTR.MOD_BY = System.Environment.UserName.ToLower();
                db.Entry(cOPR16_ITEMS_MSTR).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(cOPR16_ITEMS_MSTR);
        }

        // GET: COPR16_ITEMS_MSTR/Delete/5
        //public async Task<ActionResult> Delete(string id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    COPR16_ITEMS_MSTR cOPR16_ITEMS_MSTR = await db.COPR16_ITEMS_MSTR.FindAsync(id);
        //    if (cOPR16_ITEMS_MSTR == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(cOPR16_ITEMS_MSTR);
        //}

        // GET: COPR16_ITEMS_MSTR/Deactive/5
        public async Task<ActionResult> Deactive(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            COPR16_ITEMS_MSTR cOPR16_ITEMS_MSTR = await db.COPR16_ITEMS_MSTR.FindAsync(id);
            if (cOPR16_ITEMS_MSTR == null)
            {
                return HttpNotFound();
            }

            cOPR16_ITEMS_MSTR.FLGACT = false;
            cOPR16_ITEMS_MSTR.MOD_DATE = System.DateTime.Now;
            cOPR16_ITEMS_MSTR.MOD_BY = System.Environment.UserName.ToLower();
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // GET: COPR16_ITEMS_MSTR/Active/5
        public async Task<ActionResult> Active(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            COPR16_ITEMS_MSTR cOPR16_ITEMS_MSTR = await db.COPR16_ITEMS_MSTR.FindAsync(id);
            if (cOPR16_ITEMS_MSTR == null)
            {
                return HttpNotFound();
            }

            cOPR16_ITEMS_MSTR.FLGACT = true;
            cOPR16_ITEMS_MSTR.MOD_DATE = System.DateTime.Now;
            cOPR16_ITEMS_MSTR.MOD_BY = System.Environment.UserName.ToLower();
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        //// POST: COPR16_ITEMS_MSTR/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> DeleteConfirmed(string id)
        //{
        //    COPR16_ITEMS_MSTR cOPR16_ITEMS_MSTR = await db.COPR16_ITEMS_MSTR.FindAsync(id);
        //    db.COPR16_ITEMS_MSTR.Remove(cOPR16_ITEMS_MSTR);
        //    await db.SaveChangesAsync();
        //    return RedirectToAction("Index");
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
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
    }
}

