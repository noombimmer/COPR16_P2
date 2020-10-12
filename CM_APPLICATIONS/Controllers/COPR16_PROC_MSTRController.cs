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
    public class COPR16_PROC_MSTRController : Controller
    {
        private COPR16Entities db = new COPR16Entities();

        // GET: COPR16_PROC_MSTR
        public async Task<ActionResult> Index()
        {
            return View(await db.COPR16_PROC_MSTR.ToListAsync());
        }

        // GET: COPR16_PROC_MSTR/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            COPR16_PROC_MSTR cOPR16_PROC_MSTR = await db.COPR16_PROC_MSTR.FindAsync(id);
            if (cOPR16_PROC_MSTR == null)
            {
                return HttpNotFound();
            }
            return View(cOPR16_PROC_MSTR);
        }

        // GET: COPR16_PROC_MSTR/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: COPR16_PROC_MSTR/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "PROC_ID,PROC_NAME,PROC_DESC,ADATE,CRE_BY,MOD_BY,NOD_DATE,FLGACT")] COPR16_PROC_MSTR cOPR16_PROC_MSTR)
        {
            if (ModelState.IsValid)
            {
                db.COPR16_PROC_MSTR.Add(cOPR16_PROC_MSTR);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(cOPR16_PROC_MSTR);
        }

        // GET: COPR16_PROC_MSTR/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            COPR16_PROC_MSTR cOPR16_PROC_MSTR = await db.COPR16_PROC_MSTR.FindAsync(id);
            if (cOPR16_PROC_MSTR == null)
            {
                return HttpNotFound();
            }
            return View(cOPR16_PROC_MSTR);
        }

        // POST: COPR16_PROC_MSTR/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "PROC_ID,PROC_NAME,PROC_DESC,ADATE,CRE_BY,MOD_BY,NOD_DATE,FLGACT")] COPR16_PROC_MSTR cOPR16_PROC_MSTR)
        {
            if (ModelState.IsValid)
            {
                cOPR16_PROC_MSTR.NOD_DATE = System.DateTime.Now;
                cOPR16_PROC_MSTR.MOD_BY = System.Environment.UserName.ToLower();
                db.Entry(cOPR16_PROC_MSTR).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(cOPR16_PROC_MSTR);
        }

        // GET: COPR16_PROC_MSTR/Delete/5
        //public async Task<ActionResult> Delete(string id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    COPR16_PROC_MSTR cOPR16_PROC_MSTR = await db.COPR16_PROC_MSTR.FindAsync(id);
        //    if (cOPR16_PROC_MSTR == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(cOPR16_PROC_MSTR);
        //}

        // GET: COPR16_PROC_MSTR/Deactive/5
        public async Task<ActionResult> Deactive(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            COPR16_PROC_MSTR cOPR16_PROC_MSTR = await db.COPR16_PROC_MSTR.FindAsync(id);
            if (cOPR16_PROC_MSTR == null)
            {
                return HttpNotFound();
            }
            cOPR16_PROC_MSTR.FLGACT = false;
            cOPR16_PROC_MSTR.NOD_DATE = System.DateTime.Now;
            cOPR16_PROC_MSTR.MOD_BY = System.Environment.UserName.ToLower();

            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // GET: COPR16_PROC_MSTR/Activate/5
        public async Task<ActionResult> Active(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            COPR16_PROC_MSTR cOPR16_PROC_MSTR = await db.COPR16_PROC_MSTR.FindAsync(id);
            if (cOPR16_PROC_MSTR == null)
            {
                return HttpNotFound();
            }
            cOPR16_PROC_MSTR.FLGACT = true;
            cOPR16_PROC_MSTR.NOD_DATE = System.DateTime.Now;
            cOPR16_PROC_MSTR.MOD_BY = System.Environment.UserName.ToLower();
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        //// POST: COPR16_PROC_MSTR/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> DeleteConfirmed(string id)
        //{
        //    COPR16_PROC_MSTR cOPR16_PROC_MSTR = await db.COPR16_PROC_MSTR.FindAsync(id);
        //    db.COPR16_PROC_MSTR.Remove(cOPR16_PROC_MSTR);
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
