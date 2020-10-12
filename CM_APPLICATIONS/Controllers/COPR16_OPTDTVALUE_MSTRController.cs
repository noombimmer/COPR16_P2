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
    public class COPR16_OPTDTVALUE_MSTRController : Controller
    {
        private COPR16Entities db = new COPR16Entities();

        // GET: COPR16_OPTDTVALUE_MSTR
        public async Task<ActionResult> Index()
        {
            return View(await db.COPR16_OPTDTVALUE_MSTR.ToListAsync());
        }

        // GET: COPR16_OPTDTVALUE_MSTR/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            COPR16_OPTDTVALUE_MSTR cOPR16_OPTDTVALUE_MSTR = await db.COPR16_OPTDTVALUE_MSTR.FindAsync(id);
            if (cOPR16_OPTDTVALUE_MSTR == null)
            {
                return HttpNotFound();
            }
            return View(cOPR16_OPTDTVALUE_MSTR);
        }

        // GET: COPR16_OPTDTVALUE_MSTR/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: COPR16_OPTDTVALUE_MSTR/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "OPTDT_ID,OPTION_ID,OPTDT_VALUE,DESC,FLGDEF,FLGACT,ADATE,CRE_BY,MOD_DATE,MOD_BY")] COPR16_OPTDTVALUE_MSTR cOPR16_OPTDTVALUE_MSTR)
        {
            if (ModelState.IsValid)
            {
                db.COPR16_OPTDTVALUE_MSTR.Add(cOPR16_OPTDTVALUE_MSTR);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(cOPR16_OPTDTVALUE_MSTR);
        }

        // GET: COPR16_OPTDTVALUE_MSTR/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            COPR16_OPTDTVALUE_MSTR cOPR16_OPTDTVALUE_MSTR = await db.COPR16_OPTDTVALUE_MSTR.FindAsync(id);
            if (cOPR16_OPTDTVALUE_MSTR == null)
            {
                return HttpNotFound();
            }
            return View(cOPR16_OPTDTVALUE_MSTR);
        }

        // POST: COPR16_OPTDTVALUE_MSTR/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "OPTDT_ID,OPTION_ID,OPTDT_VALUE,DESC,FLGDEF,FLGACT,ADATE,CRE_BY,MOD_DATE,MOD_BY")] COPR16_OPTDTVALUE_MSTR cOPR16_OPTDTVALUE_MSTR)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cOPR16_OPTDTVALUE_MSTR).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(cOPR16_OPTDTVALUE_MSTR);
        }

        // GET: COPR16_OPTDTVALUE_MSTR/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            COPR16_OPTDTVALUE_MSTR cOPR16_OPTDTVALUE_MSTR = await db.COPR16_OPTDTVALUE_MSTR.FindAsync(id);
            if (cOPR16_OPTDTVALUE_MSTR == null)
            {
                return HttpNotFound();
            }
            return View(cOPR16_OPTDTVALUE_MSTR);
        }

        // POST: COPR16_OPTDTVALUE_MSTR/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            COPR16_OPTDTVALUE_MSTR cOPR16_OPTDTVALUE_MSTR = await db.COPR16_OPTDTVALUE_MSTR.FindAsync(id);
            db.COPR16_OPTDTVALUE_MSTR.Remove(cOPR16_OPTDTVALUE_MSTR);
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
