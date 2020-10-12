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
    public class COPR16_MEASURETYPE_MSTRController : Controller
    {
        private COPR16Entities db = new COPR16Entities();

        [HttpPost]
        public async Task<JsonResult> GetREFID(string id)
        {
            //COPR16_OPTDTVALUE_MSTR cOPR16_OPTDTVALUE_MSTR = await db.COPR16_OPTDTVALUE_MSTR.FindAsync(id);
            List<COPR16_MEASURETYPE_MSTR> cOPR16_MEASURETYPE_MSTR = await db.COPR16_MEASURETYPE_MSTR.Where(l => l.MSTYPE_ID.Contains(id)).ToListAsync();

            return Json(cOPR16_MEASURETYPE_MSTR, JsonRequestBehavior.AllowGet);
        }
        // GET: COPR16_MEASURETYPE_MSTR
        public async Task<ActionResult> Index()
        {
            return View(await db.COPR16_MEASURETYPE_MSTR.ToListAsync());
        }

        // GET: COPR16_MEASURETYPE_MSTR/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            COPR16_MEASURETYPE_MSTR cOPR16_MEASURETYPE_MSTR = await db.COPR16_MEASURETYPE_MSTR.FindAsync(id);
            if (cOPR16_MEASURETYPE_MSTR == null)
            {
                return HttpNotFound();
            }
            return View(cOPR16_MEASURETYPE_MSTR);
        }

        // GET: COPR16_MEASURETYPE_MSTR/Create
        public ActionResult Create()
        {
            MSTypeModel model = new MSTypeModel(db);
            model.cOPR16_MEASURETYPE_MSTR = new COPR16_MEASURETYPE_MSTR();
            return View(model);
        }

        // POST: COPR16_MEASURETYPE_MSTR/CreateJson
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateJson(
            string MSTYPE_ID,
            string MSTYPE_NAME,
            string DESC,
            string FROM_FILE,
            string OPTION_UNIT,
            string OPT_ID,
            string UNIT_ID,
            string FILE_ID,
            string DEF_VALUE,
            string MIN_VALUE,
            string MAX_VALUE,
            string FLGACT,
            string username
            )
        {
            COPR16_MEASURETYPE_MSTR cOPR16_MEASURETYPE_MSTR = new COPR16_MEASURETYPE_MSTR();
            cOPR16_MEASURETYPE_MSTR.MSTYPE_ID = MSTYPE_ID;
            cOPR16_MEASURETYPE_MSTR.MSTYPE_NAME = MSTYPE_NAME;
            cOPR16_MEASURETYPE_MSTR.DESC = DESC;
            cOPR16_MEASURETYPE_MSTR.FROM_FILE = FROM_FILE.ToLower() == "true"?true:false;
            cOPR16_MEASURETYPE_MSTR.OPTION_UNIT = OPTION_UNIT.ToLower() == "true" ? true : false;
            cOPR16_MEASURETYPE_MSTR.OPT_ID = OPT_ID;
            cOPR16_MEASURETYPE_MSTR.UNIT_ID = UNIT_ID;
            cOPR16_MEASURETYPE_MSTR.FILE_ID = FILE_ID;
            cOPR16_MEASURETYPE_MSTR.DEF_VALUE = Convert.ToDecimal(DEF_VALUE.Length == 0 ? "0": DEF_VALUE);
            cOPR16_MEASURETYPE_MSTR.MIN_VALUE = Convert.ToDecimal(MIN_VALUE.Length == 0 ? "0" : MIN_VALUE);
            cOPR16_MEASURETYPE_MSTR.MAX_VALUE = Convert.ToDecimal(MAX_VALUE.Length == 0 ? "0" : MAX_VALUE);
            cOPR16_MEASURETYPE_MSTR.FLGACT = FLGACT.ToLower() == "true" ? true : false;
            cOPR16_MEASURETYPE_MSTR.CRE_BY = username;
            cOPR16_MEASURETYPE_MSTR.ADATE = AppPropModel.today;
            db.COPR16_MEASURETYPE_MSTR.Add(cOPR16_MEASURETYPE_MSTR);
            await db.SaveChangesAsync();
            return View();

            
        }
        // POST: COPR16_MEASURETYPE_MSTR/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Create([Bind(Include = "MSTYPE_ID,MSTYPE_NAME,UNIT_ID,DEF_VALUE,ADATE,CRE_BY,MOD_BY,MOD_DATE,OPTION_UNIT,FROM_FILE,FLGACT")] COPR16_MEASURETYPE_MSTR cOPR16_MEASURETYPE_MSTR)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.COPR16_MEASURETYPE_MSTR.Add(cOPR16_MEASURETYPE_MSTR);
        //        await db.SaveChangesAsync();
        //        return RedirectToAction("Index");
        //    }

        //    return View(cOPR16_MEASURETYPE_MSTR);
        //}

        // GET: COPR16_MEASURETYPE_MSTR/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            COPR16_MEASURETYPE_MSTR cOPR16_MEASURETYPE_MSTR = await db.COPR16_MEASURETYPE_MSTR.FindAsync(id);
            if (cOPR16_MEASURETYPE_MSTR == null)
            {
                return HttpNotFound();
            }
            MSTypeModel model = new MSTypeModel(db);
            model.cOPR16_MEASURETYPE_MSTR = cOPR16_MEASURETYPE_MSTR;
            //return View(cOPR16_MEASURETYPE_MSTR);
            return View(model);
        }

        // POST: COPR16_MEASURETYPE_MSTR/SaveJson
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveJson(
            string MSTYPE_ID,
            string MSTYPE_NAME,
            string DESC,
            string FROM_FILE,
            string OPTION_UNIT,
            string OPT_ID,
            string UNIT_ID,
            string FILE_ID,
            string DEF_VALUE,
            string MIN_VALUE,
            string MAX_VALUE,
            string FLGACT,
            string username
            )
        {
            COPR16_MEASURETYPE_MSTR cOPR16_MEASURETYPE_MSTR = await db.COPR16_MEASURETYPE_MSTR.FindAsync(MSTYPE_ID);
            if (cOPR16_MEASURETYPE_MSTR == null)
            {
                return HttpNotFound();
            }
            //cOPR16_MEASURETYPE_MSTR.MSTYPE_ID = MSTYPE_ID;
            cOPR16_MEASURETYPE_MSTR.MSTYPE_NAME = MSTYPE_NAME;
            cOPR16_MEASURETYPE_MSTR.DESC = DESC;
            cOPR16_MEASURETYPE_MSTR.FROM_FILE = FROM_FILE.ToLower() == "true" ? true : false;
            cOPR16_MEASURETYPE_MSTR.OPTION_UNIT = OPTION_UNIT.ToLower() == "true" ? true : false;
            cOPR16_MEASURETYPE_MSTR.OPT_ID = OPT_ID;
            cOPR16_MEASURETYPE_MSTR.UNIT_ID = UNIT_ID;
            cOPR16_MEASURETYPE_MSTR.FILE_ID = FILE_ID;
            cOPR16_MEASURETYPE_MSTR.DEF_VALUE = Convert.ToDecimal(DEF_VALUE.Length == 0 ? "0" : DEF_VALUE);
            cOPR16_MEASURETYPE_MSTR.MIN_VALUE = Convert.ToDecimal(MIN_VALUE.Length == 0 ? "0" : MIN_VALUE);
            cOPR16_MEASURETYPE_MSTR.MAX_VALUE = Convert.ToDecimal(MAX_VALUE.Length == 0 ? "0" : MAX_VALUE);
            cOPR16_MEASURETYPE_MSTR.FLGACT = FLGACT.ToLower() == "true" ? true : false;
            cOPR16_MEASURETYPE_MSTR.MOD_BY = username;
            cOPR16_MEASURETYPE_MSTR.MOD_DATE = AppPropModel.today;

            //db.COPR16_MEASURETYPE_MSTR.Add(cOPR16_MEASURETYPE_MSTR);
            //await db.SaveChangesAsync();
            //return View();

            if (ModelState.IsValid)
            {
                db.Entry(cOPR16_MEASURETYPE_MSTR).State = EntityState.Modified;
                await db.SaveChangesAsync();
                //return RedirectToAction("Index");
            }
            return View();
        }
        // POST: COPR16_MEASURETYPE_MSTR/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Edit([Bind(Include = "MSTYPE_ID,MSTYPE_NAME,UNIT_ID,DEF_VALUE,ADATE,CRE_BY,MOD_BY,MOD_DATE,OPTION_UNIT,FROM_FILE,FLGACT")] COPR16_MEASURETYPE_MSTR cOPR16_MEASURETYPE_MSTR)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(cOPR16_MEASURETYPE_MSTR).State = EntityState.Modified;
        //        await db.SaveChangesAsync();
        //        return RedirectToAction("Index");
        //    }
        //    return View(cOPR16_MEASURETYPE_MSTR);
        //}

        // GET: COPR16_MEASURETYPE_MSTR/Delete/5
        //public async Task<ActionResult> Delete(string id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    COPR16_MEASURETYPE_MSTR cOPR16_MEASURETYPE_MSTR = await db.COPR16_MEASURETYPE_MSTR.FindAsync(id);
        //    if (cOPR16_MEASURETYPE_MSTR == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(cOPR16_MEASURETYPE_MSTR);
        //}

        // GET: COPR16_MEASURETYPE_MSTR/Deactive/5
        public async Task<ActionResult> Deactive(string id)
        {
            string usr = System.Web.HttpContext.Current.User.Identity.Name;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            COPR16_MEASURETYPE_MSTR cOPR16_MEASURETYPE_MSTR = await db.COPR16_MEASURETYPE_MSTR.FindAsync(id);
            if (cOPR16_MEASURETYPE_MSTR == null)
            {
                return HttpNotFound();
            }
            cOPR16_MEASURETYPE_MSTR.FLGACT = false;
            cOPR16_MEASURETYPE_MSTR.MOD_DATE = AppPropModel.today;
            cOPR16_MEASURETYPE_MSTR.MOD_BY = usr.Substring(usr.IndexOf(@"\") + 1);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
            //return View(cOPR16_MEASURETYPE_MSTR);
        }

        // GET: COPR16_MEASURETYPE_MSTR/Active/5
        public async Task<ActionResult> Active(string id)
        {
            string usr = System.Web.HttpContext.Current.User.Identity.Name;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            COPR16_MEASURETYPE_MSTR cOPR16_MEASURETYPE_MSTR = await db.COPR16_MEASURETYPE_MSTR.FindAsync(id);
            if (cOPR16_MEASURETYPE_MSTR == null)
            {
                return HttpNotFound();
            }
            cOPR16_MEASURETYPE_MSTR.FLGACT = true;
            cOPR16_MEASURETYPE_MSTR.MOD_DATE = AppPropModel.today;
            cOPR16_MEASURETYPE_MSTR.MOD_BY = usr.Substring(usr.IndexOf(@"\") + 1);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
            //return View(cOPR16_MEASURETYPE_MSTR);
        }
   
        //// POST: COPR16_MEASURETYPE_MSTR/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> DeleteConfirmed(string id)
        //{
        //    COPR16_MEASURETYPE_MSTR cOPR16_MEASURETYPE_MSTR = await db.COPR16_MEASURETYPE_MSTR.FindAsync(id);
        //    db.COPR16_MEASURETYPE_MSTR.Remove(cOPR16_MEASURETYPE_MSTR);
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
