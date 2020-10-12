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
    public class COPR16_BRAND_MSTRController : Controller
    {
        private COPR16Entities db = new COPR16Entities();

        // GET: COPR16_BRAND_MSTR
        public async Task<ActionResult> Index()
        {
            return View(await db.COPR16_BRAND_MSTR.ToListAsync());
        }

        // GET: COPR16_BRAND_MSTR/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            COPR16_BRAND_MSTR cOPR16_BRAND_MSTR = await db.COPR16_BRAND_MSTR.FindAsync(id);
            if (cOPR16_BRAND_MSTR == null)
            {
                return HttpNotFound();
            }
            return View(cOPR16_BRAND_MSTR);
        }

        // GET: COPR16_BRAND_MSTR/Create
        public ActionResult Create()
        {
            
            return View();
        }

        // POST: COPR16_BRAND_MSTR/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "BRAND_ID,BRAND_NAME,ADATE,CRE_BY,MOD_BY,MOD_DATE,FLGACT")] COPR16_BRAND_MSTR cOPR16_BRAND_MSTR)
        {
            if (ModelState.IsValid)
            {
                db.COPR16_BRAND_MSTR.Add(cOPR16_BRAND_MSTR);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(cOPR16_BRAND_MSTR);
        }

        // GET: COPR16_BRAND_MSTR/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            COPR16_BRAND_MSTR cOPR16_BRAND_MSTR = await db.COPR16_BRAND_MSTR.FindAsync(id);
            if (cOPR16_BRAND_MSTR == null)
            {
                return HttpNotFound();
            }
            return View(cOPR16_BRAND_MSTR);
        }

        // POST: COPR16_BRAND_MSTR/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "BRAND_ID,BRAND_NAME,ADATE,CRE_BY,MOD_BY,MOD_DATE,FLGACT")] COPR16_BRAND_MSTR cOPR16_BRAND_MSTR)
        {
            if (ModelState.IsValid)
            {
                cOPR16_BRAND_MSTR.MOD_BY = System.Environment.UserName.ToLower();
                cOPR16_BRAND_MSTR.MOD_DATE = System.DateTime.Now;

                db.Entry(cOPR16_BRAND_MSTR).State = EntityState.Modified;

                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(cOPR16_BRAND_MSTR);
        }

        // GET: COPR16_BRAND_MSTR/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            COPR16_BRAND_MSTR cOPR16_BRAND_MSTR = await db.COPR16_BRAND_MSTR.FindAsync(id);
            if (cOPR16_BRAND_MSTR == null)
            {
                return HttpNotFound();
            }
            return View(cOPR16_BRAND_MSTR);
        }

        // GET: COPR16_BRAND_MSTR/Deactive/5
        public async Task<ActionResult> Deactive(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            COPR16_BRAND_MSTR cOPR16_BRAND_MSTR = await db.COPR16_BRAND_MSTR.FindAsync(id);
            if (cOPR16_BRAND_MSTR == null)
            {
                return HttpNotFound();
            }
            cOPR16_BRAND_MSTR.FLGACT = false;
            cOPR16_BRAND_MSTR.MOD_BY = System.Environment.UserName.ToLower();
            cOPR16_BRAND_MSTR.MOD_DATE = System.DateTime.Now;
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<ActionResult> Active(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            COPR16_BRAND_MSTR cOPR16_BRAND_MSTR = await db.COPR16_BRAND_MSTR.FindAsync(id);
            if (cOPR16_BRAND_MSTR == null)
            {
                return HttpNotFound();
            }
            cOPR16_BRAND_MSTR.FLGACT = true;
            cOPR16_BRAND_MSTR.MOD_BY = System.Environment.UserName.ToLower();
            cOPR16_BRAND_MSTR.MOD_DATE = System.DateTime.Now;
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // POST: COPR16_BRAND_MSTR/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> DeleteConfirmed(string id)
        //{
        //    COPR16_BRAND_MSTR cOPR16_BRAND_MSTR = await db.COPR16_BRAND_MSTR.FindAsync(id);
        //    db.COPR16_BRAND_MSTR.Remove(cOPR16_BRAND_MSTR);
        //    await db.SaveChangesAsync();
        //    return RedirectToAction("Index");
        //}

        [HttpPost, ActionName("Deactivate")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Deactivate(string id)
        {
            COPR16_BRAND_MSTR cOPR16_BRAND_MSTR = await db.COPR16_BRAND_MSTR.FindAsync(id);
            //db.COPR16_BRAND_MSTR.Remove(cOPR16_BRAND_MSTR);
            cOPR16_BRAND_MSTR.FLGACT = false;
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        [HttpPost, ActionName("Activate")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Activate(string id)
        {
            COPR16_BRAND_MSTR cOPR16_BRAND_MSTR = await db.COPR16_BRAND_MSTR.FindAsync(id);
            //db.COPR16_BRAND_MSTR.Remove(cOPR16_BRAND_MSTR);
            cOPR16_BRAND_MSTR.FLGACT = true;
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
