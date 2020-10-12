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
    public class COPR16_MACHINE_MSTRController : Controller
    {
        private COPR16Entities db = new COPR16Entities();

        // GET: COPR16_MACHINE_MSTR
        public async Task<ActionResult> Index()
        {
            return View(await db.COPR16_MACHINE_MSTR.ToListAsync());
        }

        // GET: COPR16_MACHINE_MSTR/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            COPR16_MACHINE_MSTR cOPR16_MACHINE_MSTR = await db.COPR16_MACHINE_MSTR.FindAsync(id);
            if (cOPR16_MACHINE_MSTR == null)
            {
                return HttpNotFound();
            }
            return View(cOPR16_MACHINE_MSTR);
        }

        // GET: COPR16_MACHINE_MSTR/Create
        public ActionResult Create()
        {
            MachineListModel model = new MachineListModel(db);
            model.cOPR16_MACHINE_MSTR = new COPR16_MACHINE_MSTR();
            return View(model);
        }

        // POST: COPR16_MACHINE_MSTR/CreateJson
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateJson(
            string MANC_ID,
            string MANC_DESC,
            string MTYPE_ID,
            string FLGACT,
            string username
            )
        {
            COPR16_MACHINE_MSTR cOPR16_MACHINE_MSTR = new COPR16_MACHINE_MSTR();
            cOPR16_MACHINE_MSTR.MANC_ID = MANC_ID;
            cOPR16_MACHINE_MSTR.MANC_DESC = MANC_DESC;
            cOPR16_MACHINE_MSTR.MTYPE_ID = MTYPE_ID;

            cOPR16_MACHINE_MSTR.FLGACT = FLGACT.ToLower() == "true" ? true : false;
            cOPR16_MACHINE_MSTR.CRE_BY = username;
            cOPR16_MACHINE_MSTR.ADATE = AppPropModel.today;
            db.COPR16_MACHINE_MSTR.Add(cOPR16_MACHINE_MSTR);
            await db.SaveChangesAsync();
            return View();
        }

        // POST: COPR16_MACHINE_MSTR/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "MANC_ID,MANC_DESC,MTYPE_ID,ADATE,CRE_BY,MOD_BY,MOD_DATE,IPADDR,SUBNET,GATEWAY,USERNAME,PASSWORD,HOME_PATH,SUB_PATH,FILENAME,FLGACT")] COPR16_MACHINE_MSTR cOPR16_MACHINE_MSTR)
        {
            if (ModelState.IsValid)
            {
                db.COPR16_MACHINE_MSTR.Add(cOPR16_MACHINE_MSTR);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(cOPR16_MACHINE_MSTR);
        }

        // GET: COPR16_MACHINE_MSTR/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            COPR16_MACHINE_MSTR cOPR16_MACHINE_MSTR = await db.COPR16_MACHINE_MSTR.FindAsync(id);
            if (cOPR16_MACHINE_MSTR == null)
            {
                return HttpNotFound();
            }
            MachineListModel model = new MachineListModel(db);
            model.cOPR16_MACHINE_MSTR = cOPR16_MACHINE_MSTR;
            return View(model);
        }
        // POST: COPR16_MACHINE_MSTR/SaveJson
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveJson(
            string MANC_ID,
            string MANC_DESC,
            string MTYPE_ID,
            string FLGACT,
            string username
            )
        {
            COPR16_MACHINE_MSTR cOPR16_MACHINE_MSTR = await db.COPR16_MACHINE_MSTR.FindAsync(MANC_ID);
            //cOPR16_MACHINE_MSTR.MANC_ID = MANC_ID;
            cOPR16_MACHINE_MSTR.MANC_DESC = MANC_DESC;
            cOPR16_MACHINE_MSTR.MTYPE_ID = MTYPE_ID;
            cOPR16_MACHINE_MSTR.FLGACT = FLGACT.ToLower() == "true" ? true : false;
            cOPR16_MACHINE_MSTR.MOD_BY = username;
            cOPR16_MACHINE_MSTR.MOD_DATE = AppPropModel.today;
            //db.COPR16_MACHINE_MSTR.Add(cOPR16_MACHINE_MSTR);
            db.Entry(cOPR16_MACHINE_MSTR).State = EntityState.Modified;
            await db.SaveChangesAsync();
            return View();
        }

        // POST: COPR16_MACHINE_MSTR/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Edit([Bind(Include = "MANC_ID,MANC_DESC,MTYPE_ID,ADATE,CRE_BY,MOD_BY,MOD_DATE,IPADDR,SUBNET,GATEWAY,USERNAME,PASSWORD,HOME_PATH,SUB_PATH,FILENAME,FLGACT")] COPR16_MACHINE_MSTR cOPR16_MACHINE_MSTR)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(cOPR16_MACHINE_MSTR).State = EntityState.Modified;
        //        await db.SaveChangesAsync();
        //        return RedirectToAction("Index");
        //    }
        //    return View(cOPR16_MACHINE_MSTR);
        //}

        // GET: COPR16_MACHINE_MSTR/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            COPR16_MACHINE_MSTR cOPR16_MACHINE_MSTR = await db.COPR16_MACHINE_MSTR.FindAsync(id);
            if (cOPR16_MACHINE_MSTR == null)
            {
                return HttpNotFound();
            }
            return View(cOPR16_MACHINE_MSTR);
        }
        // GET: COPR16_MACHINE_MSTR/Deaction/5
        public async Task<ActionResult> Active(string id)
        {
            string usr = System.Web.HttpContext.Current.User.Identity.Name.Substring(System.Web.HttpContext.Current.User.Identity.Name.IndexOf(@"\") + 1);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            COPR16_MACHINE_MSTR cOPR16_MACHINE_MSTR = await db.COPR16_MACHINE_MSTR.FindAsync(id);
            if (cOPR16_MACHINE_MSTR == null)
            {
                return HttpNotFound();
            }
            cOPR16_MACHINE_MSTR.FLGACT = true;
            cOPR16_MACHINE_MSTR.MOD_BY = usr;
            cOPR16_MACHINE_MSTR.MOD_DATE = AppPropModel.today;
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
            
        }
        // GET: COPR16_MACHINE_MSTR/Deaction/5
        public async Task<ActionResult> Deactive(string id)
        {
            string usr = System.Web.HttpContext.Current.User.Identity.Name.Substring(System.Web.HttpContext.Current.User.Identity.Name.IndexOf(@"\") + 1);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            COPR16_MACHINE_MSTR cOPR16_MACHINE_MSTR = await db.COPR16_MACHINE_MSTR.FindAsync(id);
            if (cOPR16_MACHINE_MSTR == null)
            {
                return HttpNotFound();
            }
            cOPR16_MACHINE_MSTR.FLGACT = false;
            cOPR16_MACHINE_MSTR.MOD_BY = usr;
            cOPR16_MACHINE_MSTR.MOD_DATE = AppPropModel.today;
            await db.SaveChangesAsync();
            return RedirectToAction("Index");

        }

        // POST: COPR16_MACHINE_MSTR/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            COPR16_MACHINE_MSTR cOPR16_MACHINE_MSTR = await db.COPR16_MACHINE_MSTR.FindAsync(id);
            db.COPR16_MACHINE_MSTR.Remove(cOPR16_MACHINE_MSTR);
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
