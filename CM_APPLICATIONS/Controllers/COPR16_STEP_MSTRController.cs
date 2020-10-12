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
    public class COPR16_STEP_MSTRController : Controller
    {
        private COPR16Entities db = new COPR16Entities();

        // GET: COPR16_STEP_MSTR
        public async Task<ActionResult> Index()
        {
            return View(await db.COPR16_STEP_MSTR.ToListAsync());
        }

        // GET: COPR16_STEP_MSTR/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            COPR16_STEP_MSTR cOPR16_STEP_MSTR = await db.COPR16_STEP_MSTR.FindAsync(id);
            if (cOPR16_STEP_MSTR == null)
            {
                return HttpNotFound();
            }
            return View(cOPR16_STEP_MSTR);
        }


        // GET: COPR16_STEP_MSTR/Create
        public ActionResult Create()
        {
            ProcessStepsModel model = new ProcessStepsModel(db);
            model.cOPR16_STEP_MSTR = new COPR16_STEP_MSTR();
            return View(model);
        }

        // GET: COPR16_STEP_MSTR/CreateJson
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateJson(
            string STEP_ID,
            string STEP_NAME,
            string STEP_DESC,
            string RTV_ID,
            string MANC_ID,
            string FLGACT,
            string username
            )
        {
            COPR16_STEP_MSTR cOPR16_STEP_MSTR = new COPR16_STEP_MSTR();
            cOPR16_STEP_MSTR.STEP_ID = STEP_ID;
            cOPR16_STEP_MSTR.STEP_NAME = STEP_NAME;
            cOPR16_STEP_MSTR.STEP_DESC = STEP_DESC;
            cOPR16_STEP_MSTR.RTV_ID = RTV_ID;
            cOPR16_STEP_MSTR.MANC_ID = MANC_ID;
            cOPR16_STEP_MSTR.FLGACT = FLGACT.ToLower() == "true" ? true : false;
            cOPR16_STEP_MSTR.CRE_BY = username;
            cOPR16_STEP_MSTR.ADATE = AppPropModel.today;

            if (ModelState.IsValid)
            {
                db.COPR16_STEP_MSTR.Add(cOPR16_STEP_MSTR);
                await db.SaveChangesAsync();
            }

            return View();
        }

        // POST: COPR16_STEP_MSTR/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Create([Bind(Include = "STEP_ID,STEP_NAME,STEP_DESC,ADATE,CRE_BY,MOD_BY,NOD_DATE,FLGACT,MANC_ID,RTV_ID,GRPT_ID,SGRPT_ID")] COPR16_STEP_MSTR cOPR16_STEP_MSTR)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.COPR16_STEP_MSTR.Add(cOPR16_STEP_MSTR);
        //        await db.SaveChangesAsync();
        //        return RedirectToAction("Index");
        //    }

        //    return View(cOPR16_STEP_MSTR);
        //}

        // GET: COPR16_STEP_MSTR/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            COPR16_STEP_MSTR cOPR16_STEP_MSTR = await db.COPR16_STEP_MSTR.FindAsync(id);
            if (cOPR16_STEP_MSTR == null)
            {
                return HttpNotFound();
            }
            ProcessStepsModel model = new ProcessStepsModel(db);
            model.cOPR16_STEP_MSTR = cOPR16_STEP_MSTR;

            //return View(cOPR16_STEP_MSTR);
            return View(model);
        }

        // POST: COPR16_STEP_MSTR/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "STEP_ID,STEP_NAME,STEP_DESC,ADATE,CRE_BY,MOD_BY,NOD_DATE,FLGACT,MANC_ID,RTV_ID,GRPT_ID,SGRPT_ID")] COPR16_STEP_MSTR cOPR16_STEP_MSTR)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cOPR16_STEP_MSTR).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(cOPR16_STEP_MSTR);
        }

        // POST: COPR16_STEP_MSTR/SaveJson
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveJson(
            string STEP_ID,
            string STEP_NAME,
            string STEP_DESC,
            string RTV_ID,
            string MANC_ID,
            string FLGACT,
            string username
            )
        {
            COPR16_STEP_MSTR cOPR16_STEP_MSTR = await db.COPR16_STEP_MSTR.FindAsync(STEP_ID);
            if (cOPR16_STEP_MSTR == null)
            {
                return HttpNotFound();
            }
            //cOPR16_STEP_MSTR.STEP_ID = STEP_ID;
            cOPR16_STEP_MSTR.STEP_NAME = STEP_NAME;
            cOPR16_STEP_MSTR.STEP_DESC = STEP_DESC;
            cOPR16_STEP_MSTR.RTV_ID = RTV_ID;
            cOPR16_STEP_MSTR.MANC_ID = MANC_ID;
            cOPR16_STEP_MSTR.FLGACT = FLGACT.ToLower() == "true" ? true : false;
            cOPR16_STEP_MSTR.MOD_BY = username;
            cOPR16_STEP_MSTR.NOD_DATE = AppPropModel.today;
            if (ModelState.IsValid)
            {
                db.Entry(cOPR16_STEP_MSTR).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }
            return View();
        }
        // GET: COPR16_STEP_MSTR/Deactive/5
        public async Task<ActionResult> Deactive(string id)
        {
            string usr = System.Web.HttpContext.Current.User.Identity.Name.Substring(System.Web.HttpContext.Current.User.Identity.Name.IndexOf(@"\") + 1);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            COPR16_STEP_MSTR cOPR16_STEP_MSTR = await db.COPR16_STEP_MSTR.FindAsync(id);
            if (cOPR16_STEP_MSTR == null)
            {
                return HttpNotFound();
            }
            cOPR16_STEP_MSTR.FLGACT = false;
            cOPR16_STEP_MSTR.MOD_BY = usr;
            cOPR16_STEP_MSTR.NOD_DATE = AppPropModel.today;
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        // GET: COPR16_STEP_MSTR/Active/5
        public async Task<ActionResult> Active(string id)
        {
            string usr = System.Web.HttpContext.Current.User.Identity.Name.Substring(System.Web.HttpContext.Current.User.Identity.Name.IndexOf(@"\") + 1);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            COPR16_STEP_MSTR cOPR16_STEP_MSTR = await db.COPR16_STEP_MSTR.FindAsync(id);
            if (cOPR16_STEP_MSTR == null)
            {
                return HttpNotFound();
            }
            cOPR16_STEP_MSTR.FLGACT = true;
            cOPR16_STEP_MSTR.MOD_BY = usr;
            cOPR16_STEP_MSTR.NOD_DATE = AppPropModel.today;
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        //// GET: COPR16_STEP_MSTR/Delete/5
        //public async Task<ActionResult> Delete(string id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    COPR16_STEP_MSTR cOPR16_STEP_MSTR = await db.COPR16_STEP_MSTR.FindAsync(id);
        //    if (cOPR16_STEP_MSTR == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(cOPR16_STEP_MSTR);
        //}

        //// POST: COPR16_STEP_MSTR/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> DeleteConfirmed(string id)
        //{
        //    COPR16_STEP_MSTR cOPR16_STEP_MSTR = await db.COPR16_STEP_MSTR.FindAsync(id);
        //    db.COPR16_STEP_MSTR.Remove(cOPR16_STEP_MSTR);
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
