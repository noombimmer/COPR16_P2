using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using CM_APPLICATIONS.Models;
using System.IO;
using System.Text;
using System.Runtime.Serialization.Json;

namespace CM_APPLICATIONS.Controllers
{
    public class COPR16_OPTIONVAL_MSTRController : Controller
    {
        private COPR16Entities db = new COPR16Entities();

        // POST: COPR16_OPTIONVAL_MSTR/GetOPTIONVAL/ID
        [HttpPost]
        public async Task<JsonResult> GetOPTIONVAL(string id)
        {
            //COPR16_OPTDTVALUE_MSTR cOPR16_OPTDTVALUE_MSTR = await db.COPR16_OPTDTVALUE_MSTR.FindAsync(id);
            List<COPR16_OPTDTVALUE_MSTR> cOPR16_OPTDTVALUE_MSTR = await db.COPR16_OPTDTVALUE_MSTR.Where(l => l.OPTION_ID.Contains(id)).ToListAsync();

            return Json(cOPR16_OPTDTVALUE_MSTR, JsonRequestBehavior.AllowGet);
        }

        // GET: COPR16_OPTIONVAL_MSTR
        public async Task<ActionResult> Index()
        {
            return View(await db.COPR16_OPTIONVAL_MSTR.ToListAsync());
        }

        // GET: COPR16_OPTIONVAL_MSTR/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            COPR16_OPTIONVAL_MSTR cOPR16_OPTIONVAL_MSTR = await db.COPR16_OPTIONVAL_MSTR.FindAsync(id);
            if (cOPR16_OPTIONVAL_MSTR == null)
            {
                return HttpNotFound();
            }
            return View(cOPR16_OPTIONVAL_MSTR);
        }

        // GET: COPR16_OPTIONVAL_MSTR/Create
        public async Task<ActionResult> Create()
        {
            OptionsModel model = new OptionsModel(db);

            model.cOPR16_OPTDTVALUE_MSTR_LIST = await db.COPR16_OPTDTVALUE_MSTR.Where(d => d.OPTION_ID.Equals("")).ToListAsync(); ;
            return View(model);
        }

        //// POST: COPR16_OPTIONVAL_MSTR/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Create([Bind(Include = "MSTYPE_ID,OPTION_ID,OPTION_NAME,OPTION_VALUE,ADATE,CRE_BY,MOD_BY,MOD_DATE,DEF_FLG")] COPR16_OPTIONVAL_MSTR cOPR16_OPTIONVAL_MSTR)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.COPR16_OPTIONVAL_MSTR.Add(cOPR16_OPTIONVAL_MSTR);
        //        await db.SaveChangesAsync();
        //        return RedirectToAction("Index");
        //    }

        //    return View(cOPR16_OPTIONVAL_MSTR);
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateJson(
            string option_id,
            string option_name,
            string username,
            List<OptionDetailsModel> jsonString)
        {
            OptionsModel model = new OptionsModel(db);

            //COPR16_OPTIONVAL_MSTR cOPR16_OPTIONVAL_MSTR = await db.COPR16_OPTIONVAL_MSTR.FindAsync(option_id);

            COPR16_OPTIONVAL_MSTR cOPR16_OPTIONVAL_MSTR = new COPR16_OPTIONVAL_MSTR();
            cOPR16_OPTIONVAL_MSTR.CRE_BY = username;
            cOPR16_OPTIONVAL_MSTR.ADATE = today;
            cOPR16_OPTIONVAL_MSTR.OPTION_ID = option_id;
            cOPR16_OPTIONVAL_MSTR.OPTION_NAME = option_name;

            model.cOPR16_OPTIONVAL_MSTR = new COPR16_OPTIONVAL_MSTR();
            model.option_id = option_id;
            model.option_name = option_name;
            model.jsonString = jsonString;


            foreach (OptionDetailsModel item in jsonString)
            {
                COPR16_OPTDTVALUE_MSTR cOPR16_OPTDTVALUE_MSTR = db.COPR16_OPTDTVALUE_MSTR.Where(row => row.OPTION_ID.Contains(option_id) && row.OPTDT_ID.Contains(item.OPTDT_ID)).FirstOrDefault();
                if (cOPR16_OPTDTVALUE_MSTR == null)
                {
                    COPR16_OPTDTVALUE_MSTR dtdt = new COPR16_OPTDTVALUE_MSTR();
                    dtdt.OPTION_ID = option_id;
                    dtdt.OPTDT_ID = item.OPTDT_ID;
                    dtdt.OPTDT_VALUE = item.OPTDT_VALUE;
                    dtdt.FLGDEF = item.FLGDEF.ToLower() == "true" ? true : false;
                    dtdt.CRE_BY = username;
                    dtdt.FLGACT = true;
                    dtdt.DESC = item.DESC;
                    dtdt.ADATE = today;
                    db.COPR16_OPTDTVALUE_MSTR.Add(dtdt);
                    await db.SaveChangesAsync();
                }
                else
                {
                    cOPR16_OPTDTVALUE_MSTR.OPTION_ID = option_id;
                    cOPR16_OPTDTVALUE_MSTR.OPTDT_VALUE = item.OPTDT_VALUE;
                    cOPR16_OPTDTVALUE_MSTR.FLGDEF = item.FLGDEF == "true" ? true : false;
                    cOPR16_OPTDTVALUE_MSTR.MOD_BY = username;
                    cOPR16_OPTDTVALUE_MSTR.FLGACT = true;
                    cOPR16_OPTDTVALUE_MSTR.DESC = item.DESC;
                    cOPR16_OPTDTVALUE_MSTR.MOD_DATE = today;
                    db.Entry(cOPR16_OPTDTVALUE_MSTR).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                }
            }

            db.COPR16_OPTIONVAL_MSTR.Add(cOPR16_OPTIONVAL_MSTR);
            await db.SaveChangesAsync();

            return View();
        }

        // GET: COPR16_OPTIONVAL_MSTR/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            COPR16_OPTIONVAL_MSTR cOPR16_OPTIONVAL_MSTR = await db.COPR16_OPTIONVAL_MSTR.FindAsync(id);
            if (cOPR16_OPTIONVAL_MSTR == null)
            {
                return HttpNotFound();
            }
            OptionsModel model = new OptionsModel(db);
            model.cOPR16_OPTIONVAL_MSTR = cOPR16_OPTIONVAL_MSTR;
            model.cOPR16_OPTDTVALUE_MSTR_LIST = await db.COPR16_OPTDTVALUE_MSTR.Where(d => d.OPTION_ID.Contains(id)).ToListAsync();
            return View(model);
        }

        //public string UserName { get => AppPropModel.GetUserNameEx(); set { } }
        public static System.DateTime today { get { return System.DateTime.Now; } set { } }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveJson(
            string option_id, 
            string option_name,
            string username,
            List<OptionDetailsModel> jsonString)
        {
            OptionsModel model = new OptionsModel(db);

            //IEnumerable<OptionDetailsModel> lineitems = Deserialise<IEnumerable<OptionDetailsModel>>(jsonString);

            COPR16_OPTIONVAL_MSTR cOPR16_OPTIONVAL_MSTR = await db.COPR16_OPTIONVAL_MSTR.FindAsync(option_id);

            if (cOPR16_OPTIONVAL_MSTR == null)
            {
                return HttpNotFound();
            }

            //cOPR16_OPTIONVAL_MSTR.MOD_BY = System.Security.Principal.WindowsIdentity.GetCurrent().Name;

            cOPR16_OPTIONVAL_MSTR.MOD_BY = username;
            cOPR16_OPTIONVAL_MSTR.MOD_DATE = today;
            cOPR16_OPTIONVAL_MSTR.OPTION_ID = option_id;
            cOPR16_OPTIONVAL_MSTR.OPTION_NAME = option_name;

            model.cOPR16_OPTIONVAL_MSTR = new COPR16_OPTIONVAL_MSTR();
            model.option_id = option_id;
            model.option_name = option_name;
            model.jsonString = jsonString;
            foreach(OptionDetailsModel item in jsonString)
            {
                COPR16_OPTDTVALUE_MSTR cOPR16_OPTDTVALUE_MSTR = db.COPR16_OPTDTVALUE_MSTR.Where(row => row.OPTION_ID.Contains(option_id) && row.OPTDT_ID.Contains(item.OPTDT_ID) ).FirstOrDefault();
                if (cOPR16_OPTDTVALUE_MSTR == null)
                {
                    COPR16_OPTDTVALUE_MSTR dtdt = new COPR16_OPTDTVALUE_MSTR();
                    dtdt.OPTION_ID = option_id;
                    dtdt.OPTDT_ID = item.OPTDT_ID;
                    dtdt.OPTDT_VALUE = item.OPTDT_VALUE;
                    dtdt.FLGDEF = item.FLGDEF.ToLower() == "true" ? true : false;
                    dtdt.CRE_BY = username;
                    dtdt.FLGACT = true;
                    dtdt.DESC = item.DESC;
                    dtdt.ADATE = today;
                    db.COPR16_OPTDTVALUE_MSTR.Add(dtdt);
                    await db.SaveChangesAsync();
                }
                else
                {
                    cOPR16_OPTDTVALUE_MSTR.OPTION_ID = option_id;
                    //cOPR16_OPTDTVALUE_MSTR.OPTDT_ID = item.OPTDT_ID;
                    cOPR16_OPTDTVALUE_MSTR.OPTDT_VALUE = item.OPTDT_VALUE;
                    cOPR16_OPTDTVALUE_MSTR.FLGDEF = item.FLGDEF == "true" ? true : false;
                    cOPR16_OPTDTVALUE_MSTR.MOD_BY = username;
                    cOPR16_OPTDTVALUE_MSTR.FLGACT = true;
                    cOPR16_OPTDTVALUE_MSTR.DESC = item.DESC;
                    cOPR16_OPTDTVALUE_MSTR.MOD_DATE = today;
                    db.Entry(cOPR16_OPTDTVALUE_MSTR).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                }
            }
            

            db.Entry(cOPR16_OPTIONVAL_MSTR).State = EntityState.Modified;
            await db.SaveChangesAsync();

            return View(model);
        }


        public async Task<ActionResult> Deactive(string id)
        {
            string usr = System.Web.HttpContext.Current.User.Identity.Name.Substring(System.Web.HttpContext.Current.User.Identity.Name.IndexOf(@"\") + 1);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            COPR16_OPTIONVAL_MSTR cOPR16_OPTIONVAL_MSTR = await db.COPR16_OPTIONVAL_MSTR.FindAsync(id);
            if (cOPR16_OPTIONVAL_MSTR == null)
            {
                return HttpNotFound();
            }
            cOPR16_OPTIONVAL_MSTR.MOD_BY = usr;
            cOPR16_OPTIONVAL_MSTR.MOD_DATE = System.DateTime.Now;
            cOPR16_OPTIONVAL_MSTR.FLGACT = false;
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // GET: COPR16_OPTIONVAL_MSTR/Active/5
        public async Task<ActionResult> Active(string id)
        {
            string usr = System.Web.HttpContext.Current.User.Identity.Name.Substring(System.Web.HttpContext.Current.User.Identity.Name.IndexOf(@"\") + 1);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            COPR16_OPTIONVAL_MSTR cOPR16_OPTIONVAL_MSTR = await db.COPR16_OPTIONVAL_MSTR.FindAsync(id);
            if (cOPR16_OPTIONVAL_MSTR == null)
            {
                return HttpNotFound();
            }
            cOPR16_OPTIONVAL_MSTR.MOD_BY = usr;
            cOPR16_OPTIONVAL_MSTR.MOD_DATE = System.DateTime.Now;
            cOPR16_OPTIONVAL_MSTR.FLGACT = true;
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
