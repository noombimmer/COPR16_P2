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
    public class COPR16_RETURNTYPE_MSTRController : Controller
    {
        private COPR16Entities db = new COPR16Entities();


        [HttpPost]
        public async Task<ActionResult> RemoveRDTD(string id,string rtdtid, string username)
        {
            //COPR16_OPTDTVALUE_MSTR cOPR16_OPTDTVALUE_MSTR = await db.COPR16_OPTDTVALUE_MSTR.FindAsync(id);
            //List<COPR16_MEASURETYPE_MSTR> cOPR16_MEASURETYPE_MSTR = await db.COPR16_MEASURETYPE_MSTR.Where(l => l.MSTYPE_ID.Contains(id)).ToListAsync();
            COPR16_RTDT_MSTR dtdt = db.COPR16_RTDT_MSTR.Where(row => row.RTTYPE_ID.Contains(id) && row.RTDT_ID.Contains(rtdtid) && row.FLGACT.Value == true).FirstOrDefault();
            if (dtdt != null)
            {
                dtdt.MOD_BY = username;
                dtdt.MOD_DATE = AppPropModel.today;
                dtdt.FLGACT = false;
                db.Entry(dtdt).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }
            return View();
        }
        // GET: COPR16_RETURNTYPE_MSTR
        public async Task<ActionResult> Index()
        {
            return View(await db.COPR16_RETURNTYPE_MSTR.ToListAsync());
        }

        // GET: COPR16_RETURNTYPE_MSTR/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            COPR16_RETURNTYPE_MSTR cOPR16_RETURNTYPE_MSTR = await db.COPR16_RETURNTYPE_MSTR.FindAsync(id);
            if (cOPR16_RETURNTYPE_MSTR == null)
            {
                return HttpNotFound();
            }
            return View(cOPR16_RETURNTYPE_MSTR);
        }

        // GET: COPR16_RETURNTYPE_MSTR/Create
        public async Task<ActionResult> Create()
        {
            ReturnTypeModel model = new ReturnTypeModel(db);
            model.cOPR16_RETURNTYPE_MSTR = new COPR16_RETURNTYPE_MSTR();
            //model.cOPR16_RTDT_MSTR_LIST = new List<COPR16_RTDT_MSTR>();
            model.cOPR16_RTDT_MSTR_LIST = await db.COPR16_RTDT_MSTR.Where(d => d.RTDT_ID.Equals("")).ToListAsync(); ;
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateJson(
            string rt_id,
            string rt_name,
            string rt_desc,
            string flgact,

            string username,
            List<ReturnDetial> jsonString)
        {
            COPR16_RETURNTYPE_MSTR cOPR16_RETURNTYPE_MSTR = new COPR16_RETURNTYPE_MSTR();
            cOPR16_RETURNTYPE_MSTR.CRE_BY = username;
            cOPR16_RETURNTYPE_MSTR.ADATE = AppPropModel.today;
            cOPR16_RETURNTYPE_MSTR.DESC = rt_desc;
            cOPR16_RETURNTYPE_MSTR.RTYPE_ID = rt_id;
            cOPR16_RETURNTYPE_MSTR.RTYPE_NAME = rt_name;
            cOPR16_RETURNTYPE_MSTR.FLGACT = flgact.ToLower() == "true" ? true:false;
            foreach (ReturnDetial item in jsonString)
            {
                COPR16_RTDT_MSTR dtdt = new COPR16_RTDT_MSTR();
                dtdt.RTTYPE_ID = rt_id;
                dtdt.RTDT_ID = item.RTDT_ID;
                dtdt.RTDT_NAME = item.RTDT_NAME;
                dtdt.MSTYPE_ID = item.MSTYPE_ID;
                dtdt.REF_ID = item.REF_ID;
                dtdt.CRE_BY = username;
                dtdt.FLGACT = true;
                dtdt.ADATE = AppPropModel.today;
                db.COPR16_RTDT_MSTR.Add(dtdt);
                await db.SaveChangesAsync();
            }
            db.COPR16_RETURNTYPE_MSTR.Add(cOPR16_RETURNTYPE_MSTR);
            await db.SaveChangesAsync();
            return View();
        }

        // POST: COPR16_RETURNTYPE_MSTR/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "RTYPE_ID,RTYPE_NAME,ADATE,CRE_BY,MOD_BY,MOD_DATE,MSTYPE_ID")] COPR16_RETURNTYPE_MSTR cOPR16_RETURNTYPE_MSTR)
        {
            if (ModelState.IsValid)
            {
                db.COPR16_RETURNTYPE_MSTR.Add(cOPR16_RETURNTYPE_MSTR);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(cOPR16_RETURNTYPE_MSTR);
        }

        // GET: COPR16_RETURNTYPE_MSTR/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            ReturnTypeModel model = new ReturnTypeModel(db);
            
            //model.cOPR16_RTDT_MSTR_LIST = new List<COPR16_RTDT_MSTR>();
            

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //COPR16_RETURNTYPE_MSTR cOPR16_RETURNTYPE_MSTR = await db.COPR16_RETURNTYPE_MSTR.FindAsync(id);
            model.cOPR16_RETURNTYPE_MSTR = await db.COPR16_RETURNTYPE_MSTR.FindAsync(id);
            model.cOPR16_RTDT_MSTR_LIST = await db.COPR16_RTDT_MSTR.Where(d => d.RTTYPE_ID.Equals(id) && d.FLGACT.Value == true).ToListAsync(); ;
            if (model.cOPR16_RETURNTYPE_MSTR == null)
            {
                return HttpNotFound();
            }
            //return View(cOPR16_RETURNTYPE_MSTR);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveJson(
            string rt_id,
            string rt_name,
            string rt_desc,
            string flgact,

            string username,
            List<ReturnDetial> jsonString)
        {
            COPR16_RETURNTYPE_MSTR cOPR16_RETURNTYPE_MSTR = await db.COPR16_RETURNTYPE_MSTR.FindAsync(rt_id);
            cOPR16_RETURNTYPE_MSTR.MOD_BY = username;
            cOPR16_RETURNTYPE_MSTR.MOD_DATE = AppPropModel.today;
            cOPR16_RETURNTYPE_MSTR.DESC = rt_desc;
            cOPR16_RETURNTYPE_MSTR.RTYPE_ID = rt_id;
            cOPR16_RETURNTYPE_MSTR.RTYPE_NAME = rt_name;
            cOPR16_RETURNTYPE_MSTR.FLGACT = flgact.ToLower() == "true" ? true : false;
            foreach (ReturnDetial item in jsonString)
            {
                bool addNew = false;
                //COPR16_RTDT_MSTR dtdt = new COPR16_RTDT_MSTR();
                COPR16_RTDT_MSTR dtdt = db.COPR16_RTDT_MSTR.Where(
                row => row.RTTYPE_ID.Equals(rt_id) 
                && row.RTDT_ID.Equals(item.RTDT_ID) 
                && row.FLGACT.Value == true).FirstOrDefault();
                if (dtdt == null)
                {
                    dtdt = new COPR16_RTDT_MSTR();
                    addNew = true;
                }
                else
                {
                    addNew = false;
                }

                dtdt.RTTYPE_ID = rt_id;
                dtdt.RTDT_NAME = item.RTDT_NAME;
                dtdt.REF_ID = item.REF_ID;
                dtdt.MSTYPE_ID = item.MSTYPE_ID;
                dtdt.FLGACT = true;
                if (addNew)
                {
                    dtdt.RTDT_ID = item.RTDT_ID;
                    dtdt.CRE_BY = username;
                    dtdt.ADATE = AppPropModel.today;
                    db.COPR16_RTDT_MSTR.Add(dtdt);
                    try
                    {
                        await db.SaveChangesAsync();
                    }catch(Exception e)
                    {
                        cOPR16_RETURNTYPE_MSTR.DESC = "Error : "+ e.Message + "<br> RTDT_ID: " + dtdt.RTDT_ID + ", RTTYPE_ID:" + rt_id;
                        return View(cOPR16_RETURNTYPE_MSTR);
                    }
                }
                else
                {
                    dtdt.MOD_BY = username;
                    dtdt.MOD_DATE = AppPropModel.today;
                    db.Entry(dtdt).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                }
                
            }
            //db.COPR16_RETURNTYPE_MSTR.Add(cOPR16_RETURNTYPE_MSTR);
            db.Entry(cOPR16_RETURNTYPE_MSTR).State = EntityState.Modified;
            await db.SaveChangesAsync();
            return View(cOPR16_RETURNTYPE_MSTR);
        }

        // POST: COPR16_RETURNTYPE_MSTR/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Edit([Bind(Include = "RTYPE_ID,RTYPE_NAME,ADATE,CRE_BY,MOD_BY,MOD_DATE,MSTYPE_ID")] COPR16_RETURNTYPE_MSTR cOPR16_RETURNTYPE_MSTR)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        cOPR16_RETURNTYPE_MSTR.MOD_BY = System.Environment.UserName.ToLower();
        //        cOPR16_RETURNTYPE_MSTR.MOD_DATE = System.DateTime.Now;
        //        db.Entry(cOPR16_RETURNTYPE_MSTR).State = EntityState.Modified;
        //        await db.SaveChangesAsync();
        //        return RedirectToAction("Index");
        //    }
        //    return View(cOPR16_RETURNTYPE_MSTR);
        //}

        // GET: COPR16_RETURNTYPE_MSTR/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            COPR16_RETURNTYPE_MSTR cOPR16_RETURNTYPE_MSTR = await db.COPR16_RETURNTYPE_MSTR.FindAsync(id);
            if (cOPR16_RETURNTYPE_MSTR == null)
            {
                return HttpNotFound();
            }
            return View(cOPR16_RETURNTYPE_MSTR);
        }

        // POST: COPR16_RETURNTYPE_MSTR/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            COPR16_RETURNTYPE_MSTR cOPR16_RETURNTYPE_MSTR = await db.COPR16_RETURNTYPE_MSTR.FindAsync(id);
            db.COPR16_RETURNTYPE_MSTR.Remove(cOPR16_RETURNTYPE_MSTR);
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
