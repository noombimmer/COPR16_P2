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
    public class COPR16_LINE_MSTRController : Controller
    {
        private COPR16Entities db = new COPR16Entities();

        // GET: COPR16_LINE_MSTR
        public async Task<ActionResult> Index()
        {
            return View(await db.COPR16_LINE_MSTR.ToListAsync());
        }

        // GET: COPR16_LINE_MSTR/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            COPR16_LINE_MSTR cOPR16_LINE_MSTR = await db.COPR16_LINE_MSTR.FindAsync(id);
            if (cOPR16_LINE_MSTR == null)
            {
                return HttpNotFound();
            }
            return View(cOPR16_LINE_MSTR);
        }

        // GET: COPR16_LINE_MSTR/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: COPR16_LINE_MSTR/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "LINE_ID,LINE_DESC,ADATE,CRE_BY,MOD_BY,MOD_DATE,FLGACT")] COPR16_LINE_MSTR cOPR16_LINE_MSTR)
        {
            if (ModelState.IsValid)
            {
                db.COPR16_LINE_MSTR.Add(cOPR16_LINE_MSTR);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(cOPR16_LINE_MSTR);
        }

        // GET: COPR16_LINE_MSTR/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            COPR16_LINE_MSTR cOPR16_LINE_MSTR = await db.COPR16_LINE_MSTR.FindAsync(id);
            if (cOPR16_LINE_MSTR == null)
            {
                return HttpNotFound();
            }
            return View(cOPR16_LINE_MSTR);
        }

        // POST: COPR16_LINE_MSTR/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "LINE_ID,LINE_DESC,ADATE,CRE_BY,MOD_BY,MOD_DATE,FLGACT")] COPR16_LINE_MSTR cOPR16_LINE_MSTR)
        {
            if (ModelState.IsValid)
            {
                cOPR16_LINE_MSTR.MOD_BY = System.Environment.UserName.ToLower();
                cOPR16_LINE_MSTR.MOD_DATE = System.DateTime.Now;
                db.Entry(cOPR16_LINE_MSTR).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(cOPR16_LINE_MSTR);
        }

        // GET: COPR16_LINE_MSTR/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            COPR16_LINE_MSTR cOPR16_LINE_MSTR = await db.COPR16_LINE_MSTR.FindAsync(id);
            if (cOPR16_LINE_MSTR == null)
            {
                return HttpNotFound();
            }
            return View(cOPR16_LINE_MSTR);
        }
        // GET: COPR16_LINE_MSTR/Deactive/5
        public async Task<ActionResult> Deactive(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            COPR16_LINE_MSTR cOPR16_LINE_MSTR = await db.COPR16_LINE_MSTR.FindAsync(id);
            if (cOPR16_LINE_MSTR == null)
            {
                return HttpNotFound();
            }
            cOPR16_LINE_MSTR.FLGACT = false;
            cOPR16_LINE_MSTR.MOD_BY = System.Environment.UserName.ToLower();
            cOPR16_LINE_MSTR.MOD_DATE = System.DateTime.Now;
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        // GET: COPR16_LINE_MSTR/Active/5
        public async Task<ActionResult> Active(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            COPR16_LINE_MSTR cOPR16_LINE_MSTR = await db.COPR16_LINE_MSTR.FindAsync(id);
            if (cOPR16_LINE_MSTR == null)
            {
                return HttpNotFound();
            }
            cOPR16_LINE_MSTR.FLGACT = true;
            cOPR16_LINE_MSTR.MOD_BY = System.Environment.UserName.ToLower();
            cOPR16_LINE_MSTR.MOD_DATE = System.DateTime.Now;
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        // POST: COPR16_LINE_MSTR/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            COPR16_LINE_MSTR cOPR16_LINE_MSTR = await db.COPR16_LINE_MSTR.FindAsync(id);
            db.COPR16_LINE_MSTR.Remove(cOPR16_LINE_MSTR);
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
