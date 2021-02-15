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
using System.Data.SqlClient;
using CM_APPLICATIONS.Models;

namespace CM_APPLICATIONS.Controllers
{
    public class COPR16_FG_MSTRController : Controller
    {
        private COPR16Entities db = new COPR16Entities();

        // GET: COPR16_FG_MSTR
        public async Task<ActionResult> Index()
        {
            //Task<List<COPR16_FG_MSTR>> model = await new Task<List<COPR16_FG_MSTR>>();
            return View(await db.COPR16_FG_MSTR.Where(l =>l.ADATE == null).ToListAsync());
            //return View();
        }
        // GET: COPR16_FG_MSTR
        public async Task<ActionResult> IndexERP()
        {
            var rowData = new JsonResult();
            if (db.Database.Connection.State != ConnectionState.Open)
            {
                await db.Database.Connection.OpenAsync();
            }
            using (var cmd = db.Database.Connection.CreateCommand())
            {
                cmd.CommandText = "EXEC COPR16_SP_GETMODELMON '';";


                using (System.Data.Common.DbDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    var model = Utils.Serialize((SqlDataReader)reader);
                    rowData.Data = model;
                    reader.Close();
                }
            }
            using (var cmd = db.Database.Connection.CreateCommand())
            {
                cmd.CommandText = "SELECT format(max(ERP_IMPDT),'dd MMMM yyyy hh:mm:ss tt') [LASTUPDATE] FROM COPR16_ERPVOL;";


                using (System.Data.Common.DbDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    var model = Utils.Serialize((SqlDataReader)reader);
                    ViewBag.LastUpdate = model;
                    reader.Close();
                }
            }
            ViewBag.rowData = rowData.Data;
            //Task<List<COPR16_FG_MSTR>> model = await new Task<List<COPR16_FG_MSTR>>();
            return View();
            //return View();
        }
        public async Task<ActionResult> IndexFGNO()
        {
            //Task<List<COPR16_FG_MSTR>> model = await new Task<List<COPR16_FG_MSTR>>();
            return View(await db.COPR16_FG_MSTR.Where(l => l.ADATE == null).ToListAsync());
            //return View();
        }
        public async Task<ActionResult> IndexAID()
        {
            //Task<List<COPR16_FG_MSTR>> model = await new Task<List<COPR16_FG_MSTR>>();
            return View(await db.COPR16_FG_MSTR.Where(l => l.ADATE == null).ToListAsync());
            //return View();
        }

        // GET: COPR16_FG_MSTR/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            COPR16_FG_MSTR cOPR16_FG_MSTR = await db.COPR16_FG_MSTR.FindAsync(id);
            if (cOPR16_FG_MSTR == null)
            {
                return HttpNotFound();
            }
            return View(cOPR16_FG_MSTR);
        }

        // GET: COPR16_FG_MSTR/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: COPR16_FG_MSTR/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "FG_NO,FG_NAME,FG_DESC,ADATE,CT_BY,MOD_BY,MOD_DATE,FLGACT")] COPR16_FG_MSTR cOPR16_FG_MSTR)
        {
            if (ModelState.IsValid)
            {
                db.COPR16_FG_MSTR.Add(cOPR16_FG_MSTR);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(cOPR16_FG_MSTR);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> getFGList(string FGNO,string FGDESC, string FGNAME)
        {
            var rowData = new JsonResult();
            if (db.Database.Connection.State != ConnectionState.Open)
            {
                await db.Database.Connection.OpenAsync();
            }
            using (var cmd = db.Database.Connection.CreateCommand())
            {
                cmd.CommandText = "exec [dbo].[sp_fg_list] @fg_no,@fg_name,@fg_desc";
                cmd.Parameters.Add(new SqlParameter("@fg_no", FGNO == null ? "" : FGNO));
                cmd.Parameters.Add(new SqlParameter("@fg_name", FGNAME == null ? "" : FGNAME));
                cmd.Parameters.Add(new SqlParameter("@fg_desc", FGDESC == null ? "" : FGDESC));
                
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
        public async Task<ActionResult> getFGListAID(string FGNO, string FGDESC, string FGNAME)
        {
            var rowData = new JsonResult();
            if (db.Database.Connection.State != ConnectionState.Open)
            {
                await db.Database.Connection.OpenAsync();
            }
            using (var cmd = db.Database.Connection.CreateCommand())
            {
                cmd.CommandText = "exec [dbo].[sp_fg_list_aid] @fg_no,@fg_name,@fg_desc";
                cmd.Parameters.Add(new SqlParameter("@fg_no", FGNO == null ? "" : FGNO));
                cmd.Parameters.Add(new SqlParameter("@fg_name", FGNAME == null ? "" : FGNAME));
                cmd.Parameters.Add(new SqlParameter("@fg_desc", FGDESC == null ? "" : FGDESC));

                System.Data.Common.DbDataReader reader = await cmd.ExecuteReaderAsync();
                {
                    var model = Utils.Serialize((SqlDataReader)reader);
                    rowData.Data = model;
                }
            }
            return Json(rowData, JsonRequestBehavior.AllowGet);
        }
        // GET: COPR16_FG_MSTR/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            COPR16_FG_MSTR cOPR16_FG_MSTR = await db.COPR16_FG_MSTR.FindAsync(id);
            if (cOPR16_FG_MSTR == null)
            {
                return HttpNotFound();
            }
            return View(cOPR16_FG_MSTR);
        }

        // POST: COPR16_FG_MSTR/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "FG_NO,FG_NAME,FG_DESC,ADATE,CT_BY,MOD_BY,MOD_DATE,FLGACT")] COPR16_FG_MSTR cOPR16_FG_MSTR)
        {
            if (ModelState.IsValid)
            {
                cOPR16_FG_MSTR.MOD_BY = System.Environment.UserName.ToLower();
                cOPR16_FG_MSTR.MOD_DATE = System.DateTime.Now;
                db.Entry(cOPR16_FG_MSTR).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(cOPR16_FG_MSTR);
        }

        // GET: COPR16_FG_MSTR/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            COPR16_FG_MSTR cOPR16_FG_MSTR = await db.COPR16_FG_MSTR.FindAsync(id);
            if (cOPR16_FG_MSTR == null)
            {
                return HttpNotFound();
            }
            return View(cOPR16_FG_MSTR);
        }

        // GET: COPR16_FG_MSTR/Deactive/5
        public async Task<ActionResult> Deactive(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            COPR16_FG_MSTR cOPR16_FG_MSTR = await db.COPR16_FG_MSTR.FindAsync(id);
            if (cOPR16_FG_MSTR == null)
            {
                return HttpNotFound();
            }

            cOPR16_FG_MSTR.FLGACT = false;
            cOPR16_FG_MSTR.MOD_BY = System.Environment.UserName.ToLower();
            cOPR16_FG_MSTR.MOD_DATE = System.DateTime.Now;
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // GET: COPR16_FG_MSTR/Deactive/5
        public async Task<ActionResult> Active(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            COPR16_FG_MSTR cOPR16_FG_MSTR = await db.COPR16_FG_MSTR.FindAsync(id);
            if (cOPR16_FG_MSTR == null)
            {
                return HttpNotFound();
            }

            cOPR16_FG_MSTR.FLGACT = true;
            cOPR16_FG_MSTR.MOD_BY = System.Environment.UserName.ToLower();
            cOPR16_FG_MSTR.MOD_DATE = System.DateTime.Now;
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // POST: COPR16_FG_MSTR/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            COPR16_FG_MSTR cOPR16_FG_MSTR = await db.COPR16_FG_MSTR.FindAsync(id);
            db.COPR16_FG_MSTR.Remove(cOPR16_FG_MSTR);
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
        //public IEnumerable<Dictionary<string, object>> Serialize(SqlDataReader reader)
        //{
        //    var results = new List<Dictionary<string, object>>();
        //    var cols = new List<string>();
        //    for (var i = 0; i < reader.FieldCount; i++)
        //        cols.Add(reader.GetName(i));

        //    while (reader.Read())
        //        results.Add(SerializeRow(cols, reader));

        //    return results;
        //}
        //private Dictionary<string, object> SerializeRow(IEnumerable<string> cols,
        //                                                SqlDataReader reader)
        //{
        //    var result = new Dictionary<string, object>();
        //    foreach (var col in cols)
        //        result.Add(col, reader[col]);
        //    return result;
        //}
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
