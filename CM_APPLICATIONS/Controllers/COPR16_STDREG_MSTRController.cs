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
    public class COPR16_STDREG_MSTRController : Controller
    {
        private COPR16Entities db = new COPR16Entities();

        // GET: COPR16_STDREG_MSTR
        public async Task<ActionResult> Index()
        {
            return View(await db.COPR16_STDREG_MSTR.ToListAsync());
        }

        // GET: COPR16_STDREG_MSTR/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            COPR16_STDREG_MSTR cOPR16_STDREG_MSTR = await db.COPR16_STDREG_MSTR.FindAsync(id);
            if (cOPR16_STDREG_MSTR == null)
            {
                return HttpNotFound();
            }
            return View(cOPR16_STDREG_MSTR);
        }

        // GET: COPR16_STDREG_MSTR/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: COPR16_STDREG_MSTR/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Create([Bind(Include = "STDREG_ID,STDREG_NAME,STDREG_DESC,ADATE,CRE_BY,MOD_BY,MOD_DATE,FLGACT")] COPR16_STDREG_MSTR cOPR16_STDREG_MSTR)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.COPR16_STDREG_MSTR.Add(cOPR16_STDREG_MSTR);
        //        await db.SaveChangesAsync();
        //        return RedirectToAction("Index");
        //    }

        //    return View(cOPR16_STDREG_MSTR);
        //}

        // GET: COPR16_STDREG_MSTR/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            COPR16_STDREG_MSTR cOPR16_STDREG_MSTR = await db.COPR16_STDREG_MSTR.FindAsync(id);
            if (cOPR16_STDREG_MSTR == null)
            {
                return HttpNotFound();
            }
            return View(cOPR16_STDREG_MSTR);
        }

        // POST: COPR16_STDREG_MSTR/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Edit([Bind(Include = "STDREG_ID,STDREG_NAME,STDREG_DESC,ADATE,CRE_BY,MOD_BY,MOD_DATE,FLGACT")] COPR16_STDREG_MSTR cOPR16_STDREG_MSTR)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        cOPR16_STDREG_MSTR.MOD_BY = System.Environment.UserName.ToLower();
        //        cOPR16_STDREG_MSTR.MOD_DATE = System.DateTime.Now;
        //        db.Entry(cOPR16_STDREG_MSTR).State = EntityState.Modified;
        //        await db.SaveChangesAsync();
        //        return RedirectToAction("Index");
        //    }
        //    return View(cOPR16_STDREG_MSTR);
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateJson(
            string STDREG_ID,
            string STDREG_NAME,
            string STDREG_DESC,
            string FLGACT,
            string username
        )
        {
            COPR16_STDREG_MSTR cOPR16_STDREG_MSTR = new COPR16_STDREG_MSTR();
            cOPR16_STDREG_MSTR.STDREG_ID = STDREG_ID;
            cOPR16_STDREG_MSTR.STDREG_NAME = STDREG_NAME;
            cOPR16_STDREG_MSTR.STDREG_DESC = STDREG_DESC;
            cOPR16_STDREG_MSTR.FLGACT = FLGACT.ToLower() == "true" ? true : false;
            cOPR16_STDREG_MSTR.CRE_BY = username;
            cOPR16_STDREG_MSTR.ADATE = System.DateTime.Now;
            //db.Entry(cOPR16_STDREG_MSTR).State = EntityState.Modified;
            db.COPR16_STDREG_MSTR.Add(cOPR16_STDREG_MSTR);
            await db.SaveChangesAsync();
            return View(cOPR16_STDREG_MSTR);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveJson(
            string STDREG_ID,
            string STDREG_NAME,
            string STDREG_DESC,
            string FLGACT,
            string username
        )
        {
            COPR16_STDREG_MSTR cOPR16_STDREG_MSTR = await db.COPR16_STDREG_MSTR.FindAsync(STDREG_ID);
            if (cOPR16_STDREG_MSTR == null)
            {
                return HttpNotFound();
            }

            if (ModelState.IsValid)
            {
                cOPR16_STDREG_MSTR.STDREG_NAME = STDREG_NAME;
                cOPR16_STDREG_MSTR.STDREG_DESC = STDREG_DESC;
                cOPR16_STDREG_MSTR.FLGACT = FLGACT.ToLower() == "true" ? true : false;
                cOPR16_STDREG_MSTR.MOD_BY = username;
                cOPR16_STDREG_MSTR.MOD_DATE = System.DateTime.Now;
                db.Entry(cOPR16_STDREG_MSTR).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(cOPR16_STDREG_MSTR);
        }

        // GET: COPR16_STDREG_MSTR/Delete/5
        //public async Task<ActionResult> Delete(string id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    COPR16_STDREG_MSTR cOPR16_STDREG_MSTR = await db.COPR16_STDREG_MSTR.FindAsync(id);
        //    if (cOPR16_STDREG_MSTR == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(cOPR16_STDREG_MSTR);
        //}

        // GET: COPR16_STDREG_MSTR/Deactive/5
        public async Task<ActionResult> Deactive(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            COPR16_STDREG_MSTR cOPR16_STDREG_MSTR = await db.COPR16_STDREG_MSTR.FindAsync(id);
            if (cOPR16_STDREG_MSTR == null)
            {
                return HttpNotFound();
            }
            cOPR16_STDREG_MSTR.MOD_BY = System.Environment.UserName.ToLower();
            cOPR16_STDREG_MSTR.MOD_DATE = System.DateTime.Now;
            cOPR16_STDREG_MSTR.FLGACT = false;
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // GET: COPR16_STDREG_MSTR/Active/5
        public async Task<ActionResult> Active(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            COPR16_STDREG_MSTR cOPR16_STDREG_MSTR = await db.COPR16_STDREG_MSTR.FindAsync(id);
            if (cOPR16_STDREG_MSTR == null)
            {
                return HttpNotFound();
            }
            cOPR16_STDREG_MSTR.MOD_BY = System.Environment.UserName.ToLower();
            cOPR16_STDREG_MSTR.MOD_DATE = System.DateTime.Now;
            cOPR16_STDREG_MSTR.FLGACT = true;
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        // POST: COPR16_STDREG_MSTR/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> DeleteConfirmed(string id)
        //{
        //    COPR16_STDREG_MSTR cOPR16_STDREG_MSTR = await db.COPR16_STDREG_MSTR.FindAsync(id);
        //    db.COPR16_STDREG_MSTR.Remove(cOPR16_STDREG_MSTR);
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
