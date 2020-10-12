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

namespace CM_APPLICATIONS.Controllers
{
    public class COPR16_UNITS_MSTRController : Controller
    {
        private COPR16Entities db = new COPR16Entities();

        // GET: api/QAD_SIOP_AMD_DET/5
        [HttpPost]
        public async Task<JsonResult> GetUNITS(string id)
        {
            COPR16_UNITS_MSTR cOPR16_UNITS_MSTR = await db.COPR16_UNITS_MSTR.FindAsync(id);

            return Json(cOPR16_UNITS_MSTR, JsonRequestBehavior.AllowGet);
        }
        // GET: COPR16_UNITS_MSTR
        public async Task<ActionResult> Index()
        {
            return View(await db.COPR16_UNITS_MSTR.ToListAsync());
        }

        // GET: COPR16_UNITS_MSTR/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            COPR16_UNITS_MSTR cOPR16_UNITS_MSTR = await db.COPR16_UNITS_MSTR.FindAsync(id);
            if (cOPR16_UNITS_MSTR == null)
            {
                return HttpNotFound();
            }
            return View(cOPR16_UNITS_MSTR);
        }

        // GET: COPR16_UNITS_MSTR/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: COPR16_UNITS_MSTR/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "UNIT_ID,UNIT_NAME,ADATE,CRE_BY,MOD_BY,MOD_DATE,MUNIT_ID,FLGACT,UNIT_TEXT")] COPR16_UNITS_MSTR cOPR16_UNITS_MSTR)
        {
            if (ModelState.IsValid)
            {
                string usr = System.Web.HttpContext.Current.User.Identity.Name.Substring(System.Web.HttpContext.Current.User.Identity.Name.IndexOf(@"\") + 1);
                cOPR16_UNITS_MSTR.CRE_BY = usr;

                db.COPR16_UNITS_MSTR.Add(cOPR16_UNITS_MSTR);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(cOPR16_UNITS_MSTR);
        }

        // GET: COPR16_UNITS_MSTR/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            COPR16_UNITS_MSTR cOPR16_UNITS_MSTR = await db.COPR16_UNITS_MSTR.FindAsync(id);
            if (cOPR16_UNITS_MSTR == null)
            {
                return HttpNotFound();
            }
            return View(cOPR16_UNITS_MSTR);
        }

        // POST: COPR16_UNITS_MSTR/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "UNIT_ID,UNIT_NAME,ADATE,CRE_BY,MOD_BY,MOD_DATE,MUNIT_ID,FLGACT,UNIT_TEXT")] COPR16_UNITS_MSTR cOPR16_UNITS_MSTR)
        {
            string usr = System.Web.HttpContext.Current.User.Identity.Name.Substring(System.Web.HttpContext.Current.User.Identity.Name.IndexOf(@"\") + 1);
            if (ModelState.IsValid)
            {
                cOPR16_UNITS_MSTR.MOD_BY = usr;
                cOPR16_UNITS_MSTR.MOD_DATE = System.DateTime.Now;
                db.Entry(cOPR16_UNITS_MSTR).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(cOPR16_UNITS_MSTR);
        }

        //// GET: COPR16_UNITS_MSTR/Delete/5
        //public async Task<ActionResult> Delete(string id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    COPR16_UNITS_MSTR cOPR16_UNITS_MSTR = await db.COPR16_UNITS_MSTR.FindAsync(id);
        //    if (cOPR16_UNITS_MSTR == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(cOPR16_UNITS_MSTR);
        //}
        // GET: COPR16_UNITS_MSTR/Deactive/5
        public async Task<ActionResult> Deactive(string id)
        {
            string usr = System.Web.HttpContext.Current.User.Identity.Name.Substring(System.Web.HttpContext.Current.User.Identity.Name.IndexOf(@"\") + 1);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            COPR16_UNITS_MSTR cOPR16_UNITS_MSTR = await db.COPR16_UNITS_MSTR.FindAsync(id);
            if (cOPR16_UNITS_MSTR == null)
            {
                return HttpNotFound();
            }
            cOPR16_UNITS_MSTR.FLGACT = false;
            cOPR16_UNITS_MSTR.MOD_BY = usr;
            cOPR16_UNITS_MSTR.MOD_DATE = System.DateTime.Now;
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // GET: COPR16_UNITS_MSTR/Active/5
        public async Task<ActionResult> Active(string id)
        {
            string usr = System.Web.HttpContext.Current.User.Identity.Name.Substring(System.Web.HttpContext.Current.User.Identity.Name.IndexOf(@"\") + 1);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            COPR16_UNITS_MSTR cOPR16_UNITS_MSTR = await db.COPR16_UNITS_MSTR.FindAsync(id);
            if (cOPR16_UNITS_MSTR == null)
            {
                return HttpNotFound();
            }
            cOPR16_UNITS_MSTR.FLGACT = true;
            cOPR16_UNITS_MSTR.MOD_BY = usr;
            cOPR16_UNITS_MSTR.MOD_DATE = System.DateTime.Now;
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // POST: COPR16_UNITS_MSTR/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> DeleteConfirmed(string id)
        //{
        //    COPR16_UNITS_MSTR cOPR16_UNITS_MSTR = await db.COPR16_UNITS_MSTR.FindAsync(id);
        //    db.COPR16_UNITS_MSTR.Remove(cOPR16_UNITS_MSTR);
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
    }
}
