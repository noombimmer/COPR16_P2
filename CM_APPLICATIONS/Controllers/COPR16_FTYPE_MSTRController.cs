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

namespace CM_APPLICATIONS.Controllers
{
    public class COPR16_FTYPE_MSTRController : Controller
    {
        private COPR16Entities db = new COPR16Entities();

        // GET: COPR16_FTYPE_MSTR
        public async Task<ActionResult> Index()
        {
            return View(await db.COPR16_FTYPE_MSTR.ToListAsync());
        }

        // GET: COPR16_FTYPE_MSTR/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            COPR16_FTYPE_MSTR cOPR16_FTYPE_MSTR = await db.COPR16_FTYPE_MSTR.FindAsync(id);
            if (cOPR16_FTYPE_MSTR == null)
            {
                return HttpNotFound();
            }
            return View(cOPR16_FTYPE_MSTR);
        }

        // GET: COPR16_FTYPE_MSTR/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: COPR16_FTYPE_MSTR/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "FT_ID,FT_NAME,FLGACT,ADATE,CRE_BY,MOD_DATE,MOD_BY")] COPR16_FTYPE_MSTR cOPR16_FTYPE_MSTR)
        {
            if (ModelState.IsValid)
            {
                db.COPR16_FTYPE_MSTR.Add(cOPR16_FTYPE_MSTR);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(cOPR16_FTYPE_MSTR);
        }

        // GET: COPR16_FTYPE_MSTR/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            COPR16_FTYPE_MSTR cOPR16_FTYPE_MSTR = await db.COPR16_FTYPE_MSTR.FindAsync(id);
            if (cOPR16_FTYPE_MSTR == null)
            {
                return HttpNotFound();
            }
            return View(cOPR16_FTYPE_MSTR);
        }

        // POST: COPR16_FTYPE_MSTR/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "FT_ID,FT_NAME,FLGACT,ADATE,CRE_BY,MOD_DATE,MOD_BY")] COPR16_FTYPE_MSTR cOPR16_FTYPE_MSTR)
        {
            if (ModelState.IsValid)
            {
                cOPR16_FTYPE_MSTR.MOD_BY = System.Environment.UserName.ToLower();
                cOPR16_FTYPE_MSTR.MOD_DATE = System.DateTime.Now;
                db.Entry(cOPR16_FTYPE_MSTR).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(cOPR16_FTYPE_MSTR);
        }

        //// GET: COPR16_FTYPE_MSTR/Delete/5
        //public async Task<ActionResult> Delete(string id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    COPR16_FTYPE_MSTR cOPR16_FTYPE_MSTR = await db.COPR16_FTYPE_MSTR.FindAsync(id);
        //    if (cOPR16_FTYPE_MSTR == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(cOPR16_FTYPE_MSTR);
        //}
        // GET: COPR16_FTYPE_MSTR/Deactive/5
        public async Task<ActionResult> Deactive(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            COPR16_FTYPE_MSTR cOPR16_FTYPE_MSTR = await db.COPR16_FTYPE_MSTR.FindAsync(id);
            if (cOPR16_FTYPE_MSTR == null)
            {
                return HttpNotFound();
            }
            cOPR16_FTYPE_MSTR.MOD_BY = System.Environment.UserName.ToLower();
            cOPR16_FTYPE_MSTR.MOD_DATE = System.DateTime.Now;
            cOPR16_FTYPE_MSTR.FLGACT = false;
            await db.SaveChangesAsync();
            return RedirectToAction("Index");

        }

        // GET: COPR16_FTYPE_MSTR/Active/5
        public async Task<ActionResult> Active(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            COPR16_FTYPE_MSTR cOPR16_FTYPE_MSTR = await db.COPR16_FTYPE_MSTR.FindAsync(id);
            if (cOPR16_FTYPE_MSTR == null)
            {
                return HttpNotFound();
            }
            cOPR16_FTYPE_MSTR.MOD_BY = System.Environment.UserName.ToLower();
            cOPR16_FTYPE_MSTR.MOD_DATE = System.DateTime.Now;
            cOPR16_FTYPE_MSTR.FLGACT = true;
            await db.SaveChangesAsync();
            return RedirectToAction("Index");

        }

        //// POST: COPR16_FTYPE_MSTR/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> DeleteConfirmed(string id)
        //{
        //    COPR16_FTYPE_MSTR cOPR16_FTYPE_MSTR = await db.COPR16_FTYPE_MSTR.FindAsync(id);
        //    db.COPR16_FTYPE_MSTR.Remove(cOPR16_FTYPE_MSTR);
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
