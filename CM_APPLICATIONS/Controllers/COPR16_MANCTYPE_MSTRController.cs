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
    public class COPR16_MANCTYPE_MSTRController : Controller
    {
        private COPR16Entities db = new COPR16Entities();

        // GET: COPR16_MANCTYPE_MSTR
        public async Task<ActionResult> Index()
        {
            return View(await db.COPR16_MANCTYPE_MSTR.ToListAsync());
        }

        // GET: COPR16_MANCTYPE_MSTR/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            COPR16_MANCTYPE_MSTR cOPR16_MANCTYPE_MSTR = await db.COPR16_MANCTYPE_MSTR.FindAsync(id);
            if (cOPR16_MANCTYPE_MSTR == null)
            {
                return HttpNotFound();
            }
            return View(cOPR16_MANCTYPE_MSTR);
        }

        // GET: COPR16_MANCTYPE_MSTR/Create
        public ActionResult Create()
        {
            MachineTypeModel model = new MachineTypeModel(db);
            model.cOPR16_MANCTYPE_MSTR = new COPR16_MANCTYPE_MSTR();
            return View(model);
        }

        // POST: COPR16_MANCTYPE_MSTR/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "MTYPE_ID,MTYPE_NAME,ADATE,CRE_BY,MOD_BY,MOD_DATE,FLGACT,DESC,FGT_ID,FLGONLINE,RTYPE_ID")] COPR16_MANCTYPE_MSTR cOPR16_MANCTYPE_MSTR)
        {
            if (ModelState.IsValid)
            {
                db.COPR16_MANCTYPE_MSTR.Add(cOPR16_MANCTYPE_MSTR);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(cOPR16_MANCTYPE_MSTR);
        }

        // GET: COPR16_MANCTYPE_MSTR/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            COPR16_MANCTYPE_MSTR cOPR16_MANCTYPE_MSTR = await db.COPR16_MANCTYPE_MSTR.FindAsync(id);
            if (cOPR16_MANCTYPE_MSTR == null)
            {
                return HttpNotFound();
            }
            MachineTypeModel model = new MachineTypeModel(db);
            model.cOPR16_MANCTYPE_MSTR = cOPR16_MANCTYPE_MSTR;
            return View(model);
        }

        // POST: COPR16_MANCTYPE_MSTR/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "MTYPE_ID,MTYPE_NAME,ADATE,CRE_BY,MOD_BY,MOD_DATE,FLGACT,DESC,FGT_ID,FLGONLINE,RTYPE_ID")] COPR16_MANCTYPE_MSTR cOPR16_MANCTYPE_MSTR)
        {
            if (ModelState.IsValid)
            {
                cOPR16_MANCTYPE_MSTR.MOD_BY = System.Web.HttpContext.Current.User.Identity.Name.Substring(System.Web.HttpContext.Current.User.Identity.Name.IndexOf(@"\") + 1);
                cOPR16_MANCTYPE_MSTR.MOD_DATE = System.DateTime.Now;
                db.Entry(cOPR16_MANCTYPE_MSTR).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(cOPR16_MANCTYPE_MSTR);
        }

        // GET: COPR16_MANCTYPE_MSTR/Delete/5
        //public async Task<ActionResult> Delete(string id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    COPR16_MANCTYPE_MSTR cOPR16_MANCTYPE_MSTR = await db.COPR16_MANCTYPE_MSTR.FindAsync(id);
        //    if (cOPR16_MANCTYPE_MSTR == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(cOPR16_MANCTYPE_MSTR);
        //}
        // GET: COPR16_MANCTYPE_MSTR/Deactive/5
        public async Task<ActionResult> Deactive(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            COPR16_MANCTYPE_MSTR cOPR16_MANCTYPE_MSTR = await db.COPR16_MANCTYPE_MSTR.FindAsync(id);
            if (cOPR16_MANCTYPE_MSTR == null)
            {
                return HttpNotFound();
            }
            cOPR16_MANCTYPE_MSTR.MOD_BY = System.Web.HttpContext.Current.User.Identity.Name.Substring(System.Web.HttpContext.Current.User.Identity.Name.IndexOf(@"\") + 1);
            cOPR16_MANCTYPE_MSTR.MOD_DATE = System.DateTime.Now;
            cOPR16_MANCTYPE_MSTR.FLGACT = false;
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // GET: COPR16_MANCTYPE_MSTR/Active/5
        public async Task<ActionResult> Active(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            COPR16_MANCTYPE_MSTR cOPR16_MANCTYPE_MSTR = await db.COPR16_MANCTYPE_MSTR.FindAsync(id);
            if (cOPR16_MANCTYPE_MSTR == null)
            {
                return HttpNotFound();
            }
            cOPR16_MANCTYPE_MSTR.MOD_BY = System.Web.HttpContext.Current.User.Identity.Name.Substring(System.Web.HttpContext.Current.User.Identity.Name.IndexOf(@"\") + 1);
            cOPR16_MANCTYPE_MSTR.MOD_DATE = System.DateTime.Now;
            cOPR16_MANCTYPE_MSTR.FLGACT = true;
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        //// POST: COPR16_MANCTYPE_MSTR/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> DeleteConfirmed(string id)
        //{
        //    COPR16_MANCTYPE_MSTR cOPR16_MANCTYPE_MSTR = await db.COPR16_MANCTYPE_MSTR.FindAsync(id);
        //    db.COPR16_MANCTYPE_MSTR.Remove(cOPR16_MANCTYPE_MSTR);
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
