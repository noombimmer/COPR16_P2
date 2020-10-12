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
    public class COPR16_STD_MSTRController : Controller
    {
        private COPR16Entities db = new COPR16Entities();

        // GET: COPR16_STD_MSTR
        public async Task<ActionResult> Index()
        {
            return View(await db.COPR16_STD_MSTR.ToListAsync());
        }

        // GET: COPR16_STD_MSTR/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            COPR16_STD_MSTR cOPR16_STD_MSTR = await db.COPR16_STD_MSTR.FindAsync(id);
            if (cOPR16_STD_MSTR == null)
            {
                return HttpNotFound();
            }
            return View(cOPR16_STD_MSTR);
        }

        // GET: COPR16_STD_MSTR/Create
        public ActionResult Create()
        {
            ProcessModel model = new ProcessModel(db);
            model.cOPR16_STD_MSTR = new COPR16_STD_MSTR();
            return View(model);
        }

        // POST: COPR16_STD_MSTR/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "STD_ID,STD_NAME,STD_DESC,ADATE,CRE_BY,MOD_BY,NOD_DATE,FLGACT,PROC_ID,STDREG_ID")] COPR16_STD_MSTR cOPR16_STD_MSTR)
        {
            if (ModelState.IsValid)
            {
                db.COPR16_STD_MSTR.Add(cOPR16_STD_MSTR);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(cOPR16_STD_MSTR);
        }

        // GET: COPR16_STD_MSTR/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            COPR16_STD_MSTR cOPR16_STD_MSTR = await db.COPR16_STD_MSTR.FindAsync(id);
            if (cOPR16_STD_MSTR == null)
            {
                return HttpNotFound();
            }
            ProcessModel model = new ProcessModel(db);
            model.cOPR16_STD_MSTR = cOPR16_STD_MSTR;
            return View(model);
        }

        // POST: COPR16_STD_MSTR/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "STD_ID,STD_NAME,STD_DESC,ADATE,CRE_BY,MOD_BY,NOD_DATE,FLGACT,PROC_ID,STDREG_ID")] COPR16_STD_MSTR cOPR16_STD_MSTR)
        {
            if (ModelState.IsValid)
            {
                cOPR16_STD_MSTR.NOD_DATE = System.DateTime.Now;
                cOPR16_STD_MSTR.MOD_BY = System.Environment.UserName.ToLower();
                db.Entry(cOPR16_STD_MSTR).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(cOPR16_STD_MSTR);
        }

        // GET: COPR16_STD_MSTR/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            COPR16_STD_MSTR cOPR16_STD_MSTR = await db.COPR16_STD_MSTR.FindAsync(id);
            if (cOPR16_STD_MSTR == null)
            {
                return HttpNotFound();
            }
            return View(cOPR16_STD_MSTR);
        }
        // GET: COPR16_STD_MSTR/Delete/5
        public async Task<ActionResult> Deactive(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            COPR16_STD_MSTR cOPR16_STD_MSTR = await db.COPR16_STD_MSTR.FindAsync(id);
            if (cOPR16_STD_MSTR == null)
            {
                return HttpNotFound();
            }
            cOPR16_STD_MSTR.FLGACT = false;
            cOPR16_STD_MSTR.NOD_DATE = System.DateTime.Now;
            cOPR16_STD_MSTR.MOD_BY = System.Environment.UserName.ToLower();
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        // GET: COPR16_STD_MSTR/Delete/5
        public async Task<ActionResult> Active(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            COPR16_STD_MSTR cOPR16_STD_MSTR = await db.COPR16_STD_MSTR.FindAsync(id);
            if (cOPR16_STD_MSTR == null)
            {
                return HttpNotFound();
            }
            cOPR16_STD_MSTR.FLGACT = true;
            cOPR16_STD_MSTR.NOD_DATE = System.DateTime.Now;
            cOPR16_STD_MSTR.MOD_BY = System.Environment.UserName.ToLower();
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        //// POST: COPR16_STD_MSTR/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> DeleteConfirmed(string id)
        //{
        //    COPR16_STD_MSTR cOPR16_STD_MSTR = await db.COPR16_STD_MSTR.FindAsync(id);
        //    db.COPR16_STD_MSTR.Remove(cOPR16_STD_MSTR);
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
