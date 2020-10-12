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
    public class COPR16_RTVALUE_TRController : Controller
    {
        private COPR16Entities db = new COPR16Entities();

        // GET: COPR16_RTVALUE_TR
        public async Task<ActionResult> Index()
        {
            return View(await db.COPR16_RTVALUE_TR.ToListAsync());
        }

        // GET: COPR16_RTVALUE_TR/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            COPR16_RTVALUE_TR cOPR16_RTVALUE_TR = await db.COPR16_RTVALUE_TR.FindAsync(id);
            if (cOPR16_RTVALUE_TR == null)
            {
                return HttpNotFound();
            }
            return View(cOPR16_RTVALUE_TR);
        }

        // GET: COPR16_RTVALUE_TR/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: COPR16_RTVALUE_TR/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "RTV_ID,RTV_NAME,ADATE,CRE_BY,MOD_BY,MOD_DATE,RTYPE_ID,MANC_ID,DEF_VALUE")] COPR16_RTVALUE_TR cOPR16_RTVALUE_TR)
        {
            if (ModelState.IsValid)
            {
                db.COPR16_RTVALUE_TR.Add(cOPR16_RTVALUE_TR);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(cOPR16_RTVALUE_TR);
        }

        // GET: COPR16_RTVALUE_TR/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            COPR16_RTVALUE_TR cOPR16_RTVALUE_TR = await db.COPR16_RTVALUE_TR.FindAsync(id);
            if (cOPR16_RTVALUE_TR == null)
            {
                return HttpNotFound();
            }
            return View(cOPR16_RTVALUE_TR);
        }

        // POST: COPR16_RTVALUE_TR/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "RTV_ID,RTV_NAME,ADATE,CRE_BY,MOD_BY,MOD_DATE,RTYPE_ID,MANC_ID,DEF_VALUE")] COPR16_RTVALUE_TR cOPR16_RTVALUE_TR)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cOPR16_RTVALUE_TR).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(cOPR16_RTVALUE_TR);
        }

        // GET: COPR16_RTVALUE_TR/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            COPR16_RTVALUE_TR cOPR16_RTVALUE_TR = await db.COPR16_RTVALUE_TR.FindAsync(id);
            if (cOPR16_RTVALUE_TR == null)
            {
                return HttpNotFound();
            }
            return View(cOPR16_RTVALUE_TR);
        }

        // POST: COPR16_RTVALUE_TR/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            COPR16_RTVALUE_TR cOPR16_RTVALUE_TR = await db.COPR16_RTVALUE_TR.FindAsync(id);
            db.COPR16_RTVALUE_TR.Remove(cOPR16_RTVALUE_TR);
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
