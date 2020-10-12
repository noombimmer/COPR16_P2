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

namespace CM_APPLICATIONS.Controllers
{
    public class COPR16_WORKFLOWController : Controller
    {
        private COPR16Entities db = new COPR16Entities();

        // POST:  /COPR16_WORKFLOW/GetStepInfo/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> GetStepInfo(string STEP_ID,
            string DESC)
        {

            //COPR16_OPTDTVALUE_MSTR cOPR16_OPTDTVALUE_MSTR = await db.COPR16_OPTDTVALUE_MSTR.FindAsync(id);
            //List<COPR16_STEP_MSTR> data = await db.COPR16_STEP_MSTR.Where(l => l.STEP_ID.Contains(STEP_ID)).ToListAsync();
            string SQLCMD = "exec dbo.sp_GetStepLookup @CSTEP_ID";
            List<SqlParameter> param1 = new List<SqlParameter>();
            param1.Add(new SqlParameter("@CSTEP_ID", STEP_ID == null ? "": STEP_ID));

            var data = await db.Database.SqlQuery<StepLookup>(SQLCMD, param1.ToArray()).ToListAsync();
            
            //List <step_lookup> data = await db.step_lookup.Where(l => l.STEP_ID.Equals(STEP_ID)).ToListAsync();
            if (data.Count == 0)
            {
                data.Add(new StepLookup());
                data[0].STEP_ID = "[" + STEP_ID + "]";
                data[0].STEP_DESC = "[" + DESC + "]";
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        // GET: COPR16_WORKFLOW
        public async Task<ActionResult> Index()
        {
            return View(await db.COPR16_WORKFLOW.ToListAsync());
        }

        // GET: COPR16_WORKFLOW/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            COPR16_WORKFLOW cOPR16_WORKFLOW = await db.COPR16_WORKFLOW.FindAsync(id);
            if (cOPR16_WORKFLOW == null)
            {
                return HttpNotFound();
            }
            return View(cOPR16_WORKFLOW);
        }

        // GET: COPR16_WORKFLOW/Create
        public ActionResult Create()
        {
            WrkFlwModel model = new WrkFlwModel(db);
            model.cOPR16_WORKFLOW = new COPR16_WORKFLOW();
            model.cOPR16_WORKFLOW_DT = new COPR16_WORKFLOW_DT();
            model.cOPR16_WORKFLOW_DT_List = new List<COPR16_WORKFLOW_DT>();


            return View(model);
        }
        // POST: COPR16_WORKFLOW/CreateJson
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateJson(
            string WRK_ID,
            string WRK_NAME,
            string WRK_DESC,
            string STDREG_ID,
            string STD_ID,
            string PROC_ID,
            string FLGACT,
            string username,
            List<WRKD_ROW> jsonString
            )
        {
            COPR16_WORKFLOW cOPR16_WORKFLOW = new COPR16_WORKFLOW();
            cOPR16_WORKFLOW.CRE_BY = username;
            cOPR16_WORKFLOW.ADATE = AppPropModel.today;
            cOPR16_WORKFLOW.WRK_ID = WRK_ID;
            cOPR16_WORKFLOW.WRK_NAME = WRK_NAME;
            cOPR16_WORKFLOW.WRK_DESC = WRK_DESC;
            cOPR16_WORKFLOW.STDREG_ID = STDREG_ID;
            cOPR16_WORKFLOW.STD_ID = STD_ID;
            cOPR16_WORKFLOW.PROC_ID = PROC_ID;
            cOPR16_WORKFLOW.FLGACT = FLGACT.ToLower() == "true" ? true : false;

            if (ModelState.IsValid)
            {
                db.COPR16_WORKFLOW.Add(cOPR16_WORKFLOW);
                await db.SaveChangesAsync();
                
            }
            List<SqlParameter> param1 = new List<SqlParameter>();
            param1.Add(new SqlParameter("@WRK_ID", WRK_ID == null ? "" : WRK_ID));
            param1.Add(new SqlParameter("@STD_ID", STD_ID == null ? "" : STD_ID));
            param1.Add(new SqlParameter("@PROC_ID", PROC_ID == null ? "" : PROC_ID));
            await db.Database.ExecuteSqlCommandAsync("exec DBO.sp_save_workflow_head @WRK_ID,@STD_ID,@PROC_ID", param1.ToArray());

            foreach (WRKD_ROW item in jsonString)
            {
                COPR16_WORKFLOW_DT cOPR16_WORKFLOW_DT = db.COPR16_WORKFLOW_DT.Where(row => row.WRK_ID == WRK_ID && row.WRKD_ID == item.WRKD_ID ).FirstOrDefault();
                if (cOPR16_WORKFLOW_DT == null)
                {
                    COPR16_WORKFLOW_DT dtdt = new COPR16_WORKFLOW_DT();
                    dtdt.WRK_ID = item.WRK_ID;
                    //dtdt.WRKD_SEQ = item.WRKD_SEQ;
                    dtdt.WRKD_ID = item.WRKD_ID;
                    dtdt.WRKD_WITH_ID = item.WRKD_WITH_ID;
                    dtdt.WRKD_MAIN = item.WRKD_MAIN.ToLower() == "true" ? true : false;
                    dtdt.WRKD_ORDER = item.WRKD_ORDER;
                    dtdt.WRKD_PARL = item.WRKD_PARL.ToLower() == "true" ? true : false;
                    dtdt.WRKD_ANDCOND = item.WRKD_ANDCOND.ToLower() == "true" ? true : false;
                    dtdt.STEP_ID = item.STEP_ID;
                    dtdt.CRE_BY = username;
                    dtdt.WRKD_DESC = item.WRKD_DESC;
                    dtdt.WRKD_SEQ = item.WRKD_SEQ;
                    dtdt.ADATE = AppPropModel.today;
                    db.COPR16_WORKFLOW_DT.Add(dtdt);
                    await db.SaveChangesAsync();
                }
                else
                {
                    //cOPR16_WORKFLOW_DT.WRK_ID = item.WRK_ID;
                    //cOPR16_WORKFLOW_DT.WRKD_ID = item.WRKD_ID;
                    cOPR16_WORKFLOW_DT.WRKD_MAIN = item.WRKD_MAIN.ToLower() == "true" ? true : false;
                    cOPR16_WORKFLOW_DT.WRKD_ORDER = item.WRKD_ORDER;
                    cOPR16_WORKFLOW_DT.WRKD_PARL = item.WRKD_PARL.ToLower() == "true" ? true : false;
                    cOPR16_WORKFLOW_DT.WRKD_ANDCOND = item.WRKD_ANDCOND.ToLower() == "true" ? true : false;
                    cOPR16_WORKFLOW_DT.WRKD_DESC = item.WRKD_DESC;
                    cOPR16_WORKFLOW_DT.WRKD_WITH_ID = item.WRKD_WITH_ID;
                    cOPR16_WORKFLOW_DT.STEP_ID = item.STEP_ID;
                    cOPR16_WORKFLOW_DT.WRKD_SEQ = item.WRKD_SEQ;
                    //cOPR16_WORKFLOW_DT.FLGDEF = item.FLGDEF == "true" ? true : false;
                    cOPR16_WORKFLOW_DT.MOD_BY = username;
                    cOPR16_WORKFLOW_DT.MOD_DATE = AppPropModel.today;

                    db.Entry(cOPR16_WORKFLOW_DT).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                }
            }

            return View();
        }


        // POST: COPR16_WORKFLOW/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "WRK_ID,WRK_NAME,WRK_DESC,STDREG_ID,WRK_WITH,ADATE,CRE_BY,MOD_DATE,MOD_BY")] COPR16_WORKFLOW cOPR16_WORKFLOW)
        {
            if (ModelState.IsValid)
            {
                db.COPR16_WORKFLOW.Add(cOPR16_WORKFLOW);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(cOPR16_WORKFLOW);
        }
        [HttpPost]
        public async Task<JsonResult> getProcByWRKFLOW(string wrkid)
        {
            var rowData = new JsonResult();
            
            string SQLCMD2 = "SELECT * FROM COPR16_WORKFLOW WHERE WRK_ID ='"+ wrkid + "'";
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
        // GET: COPR16_WORKFLOW/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            COPR16_WORKFLOW cOPR16_WORKFLOW = await db.COPR16_WORKFLOW.FindAsync(id);
            WrkFlwModel model = new WrkFlwModel(db);
            model.cOPR16_WORKFLOW = cOPR16_WORKFLOW;
            model.cOPR16_WORKFLOW_DT = new COPR16_WORKFLOW_DT();
            string SQL = "SELECT * FROM dbo.COPR16_WORKFLOW_DT WHERE WRK_ID = '" +id+ "' ORDER BY CAST(WRKD_SEQ AS INT)";
            //model.cOPR16_WORKFLOW_DT_List = await db.COPR16_WORKFLOW_DT.Where(l => l.WRK_ID.Equals(id)).OrderBy(l=>l.WRKD_SEQ) .ToListAsync();
            model.cOPR16_WORKFLOW_DT_List = await db.COPR16_WORKFLOW_DT.SqlQuery(SQL).ToListAsync();
            model.WRK_DESC = "count : " + model.cOPR16_WORKFLOW_DT_List.Count().ToString();
            if (cOPR16_WORKFLOW == null)
            {
                return HttpNotFound();
            }
            
            return View(model);
        }

        // POST: COPR16_WORKFLOW/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "WRK_ID,WRK_NAME,WRK_DESC,STDREG_ID,WRK_WITH,ADATE,CRE_BY,MOD_DATE,MOD_BY")] COPR16_WORKFLOW cOPR16_WORKFLOW)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cOPR16_WORKFLOW).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(cOPR16_WORKFLOW);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveJson(
            string WRK_ID,
            string WRK_NAME,
            string WRK_DESC,
            string STDREG_ID,
            string STD_ID,
            string PROC_ID,
            string FLGACT,
            string username,
            List<WRKD_ROW> jsonString
            )
        {
            //COPR16_WORKFLOW cOPR16_WORKFLOW = new COPR16_WORKFLOW();
            COPR16_WORKFLOW cOPR16_WORKFLOW = await db.COPR16_WORKFLOW.FindAsync(WRK_ID);
            cOPR16_WORKFLOW.CRE_BY = username;
            cOPR16_WORKFLOW.ADATE = AppPropModel.today;
            //cOPR16_WORKFLOW.WRK_ID = WRK_ID;
            cOPR16_WORKFLOW.WRK_NAME = WRK_NAME;
            cOPR16_WORKFLOW.WRK_DESC = WRK_DESC;
            cOPR16_WORKFLOW.STDREG_ID = STDREG_ID;
            cOPR16_WORKFLOW.PROC_ID = PROC_ID;
            cOPR16_WORKFLOW.STD_ID = STD_ID;
            cOPR16_WORKFLOW.FLGACT = FLGACT.ToLower() == "true" ? true : false;

            if (ModelState.IsValid)
            {
                //db.COPR16_WORKFLOW.Add(cOPR16_WORKFLOW);
                db.Entry(cOPR16_WORKFLOW).State = EntityState.Modified;
                await db.SaveChangesAsync();

                List<SqlParameter> param1 = new List<SqlParameter>();
                param1.Add(new SqlParameter("@WRK_ID", WRK_ID == null ? "" : WRK_ID));
                param1.Add(new SqlParameter("@STD_ID", STD_ID == null ? "" : STD_ID));
                param1.Add(new SqlParameter("@PROC_ID", PROC_ID == null ? "" : PROC_ID));
                await db.Database.ExecuteSqlCommandAsync("exec DBO.sp_save_workflow_head @WRK_ID,@STD_ID,@PROC_ID", param1.ToArray());
            }
            foreach (WRKD_ROW item in jsonString)
            {
                COPR16_WORKFLOW_DT cOPR16_WORKFLOW_DT = db.COPR16_WORKFLOW_DT.Where(row => row.WRK_ID == WRK_ID && row.WRKD_ID == item.WRKD_ID).FirstOrDefault();
                if (cOPR16_WORKFLOW_DT == null)
                {
                    COPR16_WORKFLOW_DT dtdt = new COPR16_WORKFLOW_DT();
                    dtdt.WRK_ID = item.WRK_ID;
                    //dtdt.WRKD_SEQ = item.WRKD_SEQ;
                    dtdt.WRKD_ID = item.WRKD_ID;
                    dtdt.WRKD_WITH_ID = item.WRKD_WITH_ID;
                    dtdt.WRKD_MAIN = item.WRKD_MAIN.ToLower() == "true" ? true : false;
                    dtdt.WRKD_ORDER = item.WRKD_ORDER;
                    dtdt.WRKD_PARL = item.WRKD_PARL.ToLower() == "true" ? true : false;
                    dtdt.WRKD_ANDCOND = item.WRKD_ANDCOND.ToLower() == "true" ? true : false;
                    dtdt.STEP_ID = item.STEP_ID;
                    dtdt.CRE_BY = username;
                    dtdt.WRKD_DESC = item.WRKD_DESC;
                    dtdt.WRKD_SEQ = item.WRKD_SEQ;
                    dtdt.ADATE = AppPropModel.today;
                    db.COPR16_WORKFLOW_DT.Add(dtdt);
                    await db.SaveChangesAsync();
                }
                else
                {
                    //cOPR16_WORKFLOW_DT.WRK_ID = item.WRK_ID;
                    //cOPR16_WORKFLOW_DT.WRKD_ID = item.WRKD_ID;
                    cOPR16_WORKFLOW_DT.WRKD_MAIN = item.WRKD_MAIN.ToLower() == "true" ? true : false;
                    cOPR16_WORKFLOW_DT.WRKD_ORDER = item.WRKD_ORDER;
                    cOPR16_WORKFLOW_DT.WRKD_PARL = item.WRKD_PARL.ToLower() == "true" ? true : false;
                    cOPR16_WORKFLOW_DT.WRKD_ANDCOND = item.WRKD_ANDCOND.ToLower() == "true" ? true : false;
                    cOPR16_WORKFLOW_DT.WRKD_DESC = item.WRKD_DESC;
                    cOPR16_WORKFLOW_DT.WRKD_WITH_ID = item.WRKD_WITH_ID;
                    cOPR16_WORKFLOW_DT.STEP_ID = item.STEP_ID;
                    cOPR16_WORKFLOW_DT.WRKD_SEQ = item.WRKD_SEQ;
                    //cOPR16_WORKFLOW_DT.FLGDEF = item.FLGDEF == "true" ? true : false;
                    cOPR16_WORKFLOW_DT.MOD_BY = username;
                    cOPR16_WORKFLOW_DT.MOD_DATE = AppPropModel.today;

                    db.Entry(cOPR16_WORKFLOW_DT).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                }
            }

            return View();
        }
        // GET: COPR16_OPTIONVAL_MSTR/Deactive/5
        public async Task<ActionResult> Deactive(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            COPR16_WORKFLOW cOPR16_WORKFLOW = await db.COPR16_WORKFLOW.FindAsync(id);
            if (cOPR16_WORKFLOW == null)
            {
                return HttpNotFound();
            }
            cOPR16_WORKFLOW.MOD_BY = System.Web.HttpContext.Current.User.Identity.Name.Substring(System.Web.HttpContext.Current.User.Identity.Name.IndexOf(@"\") + 1);
            cOPR16_WORKFLOW.MOD_DATE = System.DateTime.Now;
            cOPR16_WORKFLOW.FLGACT = false;
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // GET: COPR16_OPTIONVAL_MSTR/Active/5
        public async Task<ActionResult> Active(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            COPR16_WORKFLOW cOPR16_WORKFLOW = await db.COPR16_WORKFLOW.FindAsync(id);
            if (cOPR16_WORKFLOW == null)
            {
                return HttpNotFound();
            }
            cOPR16_WORKFLOW.MOD_BY = System.Web.HttpContext.Current.User.Identity.Name.Substring(System.Web.HttpContext.Current.User.Identity.Name.IndexOf(@"\") + 1);
            cOPR16_WORKFLOW.MOD_DATE = System.DateTime.Now;
            cOPR16_WORKFLOW.FLGACT = true;
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // GET: COPR16_WORKFLOW/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            COPR16_WORKFLOW cOPR16_WORKFLOW = await db.COPR16_WORKFLOW.FindAsync(id);
            if (cOPR16_WORKFLOW == null)
            {
                return HttpNotFound();
            }
            return View(cOPR16_WORKFLOW);
        }

        // POST: COPR16_WORKFLOW/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            COPR16_WORKFLOW cOPR16_WORKFLOW = await db.COPR16_WORKFLOW.FindAsync(id);
            db.COPR16_WORKFLOW.Remove(cOPR16_WORKFLOW);
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
    }
}
