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
using System.Data.SqlClient;
using CM_APPLICATIONS.Models;
using System.Data.Common;

namespace CM_APPLICATIONS.Controllers
{
    public class COPR16_COPRUNNING_RTController : Controller
    {
        private COPR16Entities db = new COPR16Entities();

        // GET: COPR16_COPRUNNING_RT
        public async Task<ActionResult> Index()
        {
            HandheldModel model = new HandheldModel(db);
            model.cOPR16_COPRUNNING_RT_List = await db.COPR16_COPRUNNING_RT.ToListAsync();
            string SQLCMD = "SELECT * FROM COPR16_MACHINE_MSTR ORDER BY MTYPE_ID ";
            model.cOPR16_MACHINE_MSTR_List = await db.COPR16_MACHINE_MSTR.SqlQuery(SQLCMD).ToListAsync();

            return View(model);
        }
        private HandheldModel getListFromCOP(HandheldModel model)
        {

            return model;
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        // GET: COPR16_COPRUNNING_RT/HANDHELD
        public async Task<ActionResult> HANDHELD_SEARCH(searchCOP parms1)
        {
            HandheldModel model = new HandheldModel(db);
            if(!getBlankOrNull(parms1.fDate)  && !getBlankOrNull(parms1.tDate))
            {
                if ((getBlankOrNull(parms1.COPNO) && getBlankOrNull(parms1.testStatus)))
                {
                    model.cOPR16_COPRUNNING_List = await db.COPR16_COPRUNNING.Where(l => 
                        (l.ADATE >= DateTime.Parse(parms1.fDate) && l.ADATE <= DateTime.Parse(parms1.tDate)) && 
                        (l.COP_STATUS.Equals("READY") || l.COP_STATUS.Equals("TESTING"))
                     ).ToListAsync();
                }
                else if(!getBlankOrNull(parms1.COPNO) && getBlankOrNull(parms1.testStatus))
                {
                    model.cOPR16_COPRUNNING_List = await db.COPR16_COPRUNNING.Where(l =>
                        (l.ADATE >= DateTime.Parse(parms1.fDate) && l.ADATE <= DateTime.Parse(parms1.tDate)) &&
                        (l.COPR_ID.Contains(parms1.COPNO)) && 
                        (l.COP_STATUS.Equals("READY") || l.COP_STATUS.Equals("TESTING"))
                     ).ToListAsync();

                }
                else if (getBlankOrNull(parms1.COPNO) && !getBlankOrNull(parms1.testStatus))
                {
                    model.cOPR16_COPRUNNING_List = await db.COPR16_COPRUNNING.Where(l =>
                        (l.ADATE >= DateTime.Parse(parms1.fDate) && l.ADATE <= DateTime.Parse(parms1.tDate)) &&
                        (l.COP_STATUS.Contains(parms1.testStatus)) &&
                        (l.COP_STATUS.Equals("READY") || l.COP_STATUS.Equals("TESTING"))
                     ).ToListAsync();

                }
                else if (!getBlankOrNull(parms1.COPNO)  && !getBlankOrNull(parms1.testStatus))
                {
                    model.cOPR16_COPRUNNING_List = await db.COPR16_COPRUNNING.Where(l =>
                        (l.ADATE >= DateTime.Parse(parms1.fDate) && l.ADATE <= DateTime.Parse(parms1.tDate)) &&
                        (l.COPR_ID.Contains(parms1.COPNO) && l.COP_STATUS.Contains(parms1.testStatus)) &&
                        (l.COP_STATUS.Equals("READY") || l.COP_STATUS.Equals("TESTING"))
                     ).ToListAsync();

                }

            }
            else if ((!getBlankOrNull(parms1.fDate) && getBlankOrNull(parms1.tDate) ))
            {
                if ((getBlankOrNull(parms1.COPNO) && getBlankOrNull(parms1.testStatus)))
                {
                    model.cOPR16_COPRUNNING_List = await db.COPR16_COPRUNNING.Where(l =>
                        (l.ADATE >= DateTime.Parse(parms1.fDate) ) &&
                        (l.COP_STATUS.Equals("READY") || l.COP_STATUS.Equals("TESTING"))
                     ).ToListAsync();
                }
                else if (!getBlankOrNull(parms1.COPNO) && getBlankOrNull(parms1.testStatus))
                {
                    model.cOPR16_COPRUNNING_List = await db.COPR16_COPRUNNING.Where(l =>
                        (l.ADATE >= DateTime.Parse(parms1.fDate)) &&
                        (l.COPR_ID.Contains(parms1.COPNO)) &&
                        (l.COP_STATUS.Equals("READY") || l.COP_STATUS.Equals("TESTING"))
                     ).ToListAsync();

                }
                else if (getBlankOrNull(parms1.COPNO) && !getBlankOrNull(parms1.testStatus))
                {
                    model.cOPR16_COPRUNNING_List = await db.COPR16_COPRUNNING.Where(l =>
                        (l.ADATE >= DateTime.Parse(parms1.fDate)) &&
                        (l.COP_STATUS.Contains(parms1.testStatus)) &&
                        (l.COP_STATUS.Equals("READY") || l.COP_STATUS.Equals("TESTING"))
                     ).ToListAsync();

                }
                else if (!getBlankOrNull(parms1.COPNO) && !getBlankOrNull(parms1.testStatus))
                {
                    model.cOPR16_COPRUNNING_List = await db.COPR16_COPRUNNING.Where(l =>
                        (l.ADATE >= DateTime.Parse(parms1.fDate)) &&
                        (l.COPR_ID.Contains(parms1.COPNO) && l.COP_STATUS.Contains(parms1.testStatus)) &&
                        (l.COP_STATUS.Equals("READY") || l.COP_STATUS.Equals("TESTING"))
                     ).ToListAsync();

                }
                //model.cOPR16_COPRUNNING_List = await db.COPR16_COPRUNNING.Where(l => (l.ADATE >= DateTime.Parse(parms1.fDate)) && l.COP_STATUS.Equals("RDY") || l.COP_STATUS.Equals("TESTING")).ToListAsync();
            }else if ((getBlankOrNull(parms1.fDate) && !getBlankOrNull(parms1.tDate) ))
            {
                if ((getBlankOrNull(parms1.COPNO) && getBlankOrNull(parms1.testStatus)))
                {
                    model.cOPR16_COPRUNNING_List = await db.COPR16_COPRUNNING.Where(l =>
                        (l.ADATE >= DateTime.Parse(parms1.tDate) && l.ADATE <= DateTime.Parse(parms1.tDate)) && 
                        (l.COP_STATUS.Equals("READY") || l.COP_STATUS.Equals("TESTING"))
                     ).ToListAsync();
                }
                else if (!getBlankOrNull(parms1.COPNO) && getBlankOrNull(parms1.testStatus))
                {
                    model.cOPR16_COPRUNNING_List = await db.COPR16_COPRUNNING.Where(l =>
                        (l.ADATE >= DateTime.Parse(parms1.tDate) && l.ADATE <= DateTime.Parse(parms1.tDate)) &&
                        (l.COPR_ID.Contains(parms1.COPNO)) &&
                        (l.COP_STATUS.Equals("READY") || l.COP_STATUS.Equals("TESTING"))
                     ).ToListAsync();

                }
                else if (getBlankOrNull(parms1.COPNO) && !getBlankOrNull(parms1.testStatus))
                {
                    model.cOPR16_COPRUNNING_List = await db.COPR16_COPRUNNING.Where(l =>
                        (l.ADATE >= DateTime.Parse(parms1.tDate) && l.ADATE <= DateTime.Parse(parms1.tDate)) &&
                        (l.COP_STATUS.Contains(parms1.testStatus)) &&
                        (l.COP_STATUS.Equals("READY") || l.COP_STATUS.Equals("TESTING"))
                     ).ToListAsync();

                }
                else if (!getBlankOrNull(parms1.COPNO) && !getBlankOrNull(parms1.testStatus))
                {
                    model.cOPR16_COPRUNNING_List = await db.COPR16_COPRUNNING.Where(l =>
                        (l.ADATE >= DateTime.Parse(parms1.tDate) && l.ADATE <= DateTime.Parse(parms1.tDate)) &&
                        (l.COPR_ID.Contains(parms1.COPNO) && l.COP_STATUS.Contains(parms1.testStatus)) &&
                        (l.COP_STATUS.Equals("READY") || l.COP_STATUS.Equals("TESTING"))
                     ).ToListAsync();

                }
                //model.cOPR16_COPRUNNING_List = await db.COPR16_COPRUNNING.Where(l => (l.ADATE >= DateTime.Parse(parms1.tDate) && l.ADATE <= DateTime.Parse(parms1.tDate)) && l.COP_STATUS.Equals("RDY") || l.COP_STATUS.Equals("TESTING")).ToListAsync();
            }else if (getBlankOrNull(parms1.fDate) && getBlankOrNull(parms1.tDate))
            {
                if ((getBlankOrNull(parms1.COPNO) && getBlankOrNull(parms1.testStatus)))
                {
                    model.cOPR16_COPRUNNING_List = await db.COPR16_COPRUNNING.Where(l =>
                        l.COP_STATUS.Equals("READY") || l.COP_STATUS.Equals("TESTING")
                     ).ToListAsync();
                }
                else if (!getBlankOrNull(parms1.COPNO) && getBlankOrNull(parms1.testStatus))
                {
                    model.cOPR16_COPRUNNING_List = await db.COPR16_COPRUNNING.Where(l =>
                        (l.COPR_ID.Contains(parms1.COPNO)) &&
                        (l.COP_STATUS.Equals("READY") || l.COP_STATUS.Equals("TESTING"))
                     ).ToListAsync();

                }
                else if (getBlankOrNull(parms1.COPNO) && !getBlankOrNull(parms1.testStatus))
                {
                    model.cOPR16_COPRUNNING_List = await db.COPR16_COPRUNNING.Where(l =>
                        (l.COP_STATUS.Contains(parms1.testStatus)) &&
                        (l.COP_STATUS.Equals("READY") || l.COP_STATUS.Equals("TESTING"))
                     ).ToListAsync();

                }
                else if (!getBlankOrNull(parms1.COPNO) && !getBlankOrNull(parms1.testStatus))
                {
                    model.cOPR16_COPRUNNING_List = await db.COPR16_COPRUNNING.Where(l =>
                        (l.COPR_ID.Contains(parms1.COPNO) && l.COP_STATUS.Contains(parms1.testStatus)) &&
                        (l.COP_STATUS.Equals("READY") || l.COP_STATUS.Equals("TESTING"))
                     ).ToListAsync();

                }
            }
            else
            {
                model.cOPR16_COPRUNNING_List = await db.COPR16_COPRUNNING.Where(l =>
                                        l.COP_STATUS.Equals("READY") || l.COP_STATUS.Equals("TESTING")
                                     ).ToListAsync();
            }
                

            foreach (var rows in model.cOPR16_COPRUNNING_List)
            {
                COPR16_POSITION_MSTR posName = await db.COPR16_POSITION_MSTR.Where(l => l.POS_ID == rows.POSITION_ID).FirstAsync();
                COPR16_MODEL_MSTR modelName = await db.COPR16_MODEL_MSTR.Where(l => l.MODEL_ID == rows.MODEL_ID).FirstAsync();

                rows.POSITION_ID = posName.POS_DESC;
                rows.MODEL_ID = modelName.MODEL_DESC;

            }
            model.searchOpion = parms1;
            //return View(await db.COPR16_COPRUNNING_RT.ToListAsync());
            return View(model);
        }

        // GET: COPR16_COPRUNNING_RT/HANDHELD
        public async Task<ActionResult> HANDHELD()
        {
            HandheldModel model = new HandheldModel(db);
            string SQLCMD = "SELECT * FROM COPR16_COPRUNNING WHERE COP_STATUS IN ('READY','TESTING') ORDER BY COPR_ID DESC";
            //model.cOPR16_COPRUNNING_List = await db.COPR16_COPRUNNING.Where(l => l.COP_STATUS.Equals("READY") || l.COP_STATUS.Equals("TESTING")).ToListAsync();
            model.cOPR16_COPRUNNING_List = await db.COPR16_COPRUNNING.SqlQuery(SQLCMD).ToListAsync();
            foreach (var rows in model.cOPR16_COPRUNNING_List)
            {
                COPR16_POSITION_MSTR posName = await db.COPR16_POSITION_MSTR.Where(l => l.POS_ID == rows.POSITION_ID).FirstAsync();
                COPR16_MODEL_MSTR modelName = await db.COPR16_MODEL_MSTR.Where(l => l.MODEL_ID == rows.MODEL_ID).FirstAsync();

                rows.POSITION_ID = posName.POS_DESC;
                rows.MODEL_ID = modelName.MODEL_DESC;

            }
            //model.cOPR16_COPRUNNING_List = model.cOPR16_COPRUNNING_List.OrderByDescending(l => l.COPR_ID).ToList();
            model.statusCode = "";
            //return View(await db.COPR16_COPRUNNING_RT.ToListAsync());
            return View(model);
        }
        public async Task<ActionResult> StartTest(string id)
        {
            HandheldModel model = new HandheldModel(db);
            //model.cOPR16_COPRUNNING_List = await db.COPR16_COPRUNNING.Where(l => l.COP_STATUS.Equals("RDY")).ToListAsync();
            model.cOPR16_COPRUNNING = await db.COPR16_COPRUNNING.FindAsync(id);// Header
            model.cOPR16_COPRUNNING_DT_List = await db.COPR16_COPRUNNING_DT.Where(l => l.COPR_ID.Equals(id)).ToListAsync();// QR-CODE
            model.cOPR16_COPRUNNING_DT_BKL = model.cOPR16_COPRUNNING_DT_List.Find(l => l.FGTYPE_ID.Equals("BUNKLE"));// QR-CODE
            model.cOPR16_COPRUNNING_DT_SB = model.cOPR16_COPRUNNING_DT_List.Find(l => l.FGTYPE_ID.Equals("SEATBELT"));// QR-CODE
            model.cOPR16_COPRUNNING_RT_List = await db.COPR16_COPRUNNING_RT.Where(l => l.COPR_ID.Equals(id)).ToListAsync(); // test details
            model.cOPR16_WORKFLOW_DT_List = await db.COPR16_WORKFLOW_DT.Where(l => l.WRK_ID.Equals(model.cOPR16_COPRUNNING.WRK_ID)).OrderBy(l => l.WRKD_SEQ).ToListAsync();
            //model.FG_DETAILS = "Count: " + model.cOPR16_COPRUNNING_DT_List.Count().ToString();
            model.cOPR16_WORKFLOW_DT = new COPR16_WORKFLOW_DT();
            model.FG_DETAILS = id;
            string SQLCMD = "select distinct " +
            "C.WRKD_SEQ " +
            ",C.WRKD_DESC," +
            "A.FGTYPE_ID" +
            ",A.WRKD_ID" +
            ",A.MACHINETYPE_ID," +
            "B.MTYPE_NAME" +
             ",A.COPR_ID," +
             "A.STATUS_ID," +
             "A.TEST_START," +
             "A.TEST_BY," +
             "A.TEST_FINISH," +
              "A.CLOSE_BY," +
             "A.MACHINE_ID" +
             " from COPR16_COPRUNNING_RT A" +
             " JOIN COPR16_MANCTYPE_MSTR B on A.MACHINETYPE_ID = B.MTYPE_ID" +
             " JOIN COPR16_WORKFLOW_DT C ON A.WRK_ID = C.WRK_ID AND A.WRKD_ID = C.WRKD_ID WHERE A.COPR_ID ='" + id + "' AND A.STATUS_ID IS NOT NULL ";
            model.WorkProcess_List = await db.Database.SqlQuery<WorkProcess>(SQLCMD).ToListAsync();
            //return View(await db.COPR16_COPRUNNING_RT.ToListAsync());
            model.workProcess = new WorkProcess();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> StartTestForResult(string hCOPR_ID, string hSEQ_NO)
        {
            HandheldModel model = new HandheldModel(db);
            //model.cOPR16_COPRUNNING_List = await db.COPR16_COPRUNNING.Where(l => l.COP_STATUS.Equals("RDY")).ToListAsync();
            model.cOPR16_COPRUNNING = await db.COPR16_COPRUNNING.FindAsync(hCOPR_ID);// Header
            model.cOPR16_COPRUNNING_DT_List = await db.COPR16_COPRUNNING_DT.Where(l => l.COPR_ID.Equals(hCOPR_ID)).ToListAsync();// QR-CODE
            model.cOPR16_COPRUNNING_DT_BKL = model.cOPR16_COPRUNNING_DT_List.Find(l => l.FGTYPE_ID.Equals("BUNKLE"));// QR-CODE
            model.cOPR16_COPRUNNING_DT_SB = model.cOPR16_COPRUNNING_DT_List.Find(l => l.FGTYPE_ID.Equals("SEATBELT"));// QR-CODE
            model.cOPR16_COPRUNNING_RT_List = await db.COPR16_COPRUNNING_RT.Where(l => l.COPR_ID.Equals(hCOPR_ID) && l.SEQ_NO.Equals(hSEQ_NO)).ToListAsync(); // test details
            model.cOPR16_WORKFLOW_DT_List = await db.COPR16_WORKFLOW_DT.Where(l => l.WRK_ID.Equals(model.cOPR16_COPRUNNING.WRK_ID) && l.WRKD_SEQ.Equals(hSEQ_NO)).OrderBy(l => l.WRKD_SEQ).ToListAsync();
            //model.FG_DETAILS = "Count: " + model.cOPR16_COPRUNNING_DT_List.Count().ToString();
            model.cOPR16_WORKFLOW_DT = new COPR16_WORKFLOW_DT();
            model.FG_DETAILS = hCOPR_ID;
            string SQLCMD = "select distinct " +
            "C.WRKD_SEQ " +
            ",C.WRKD_DESC," +
            "A.FGTYPE_ID" +
            ",A.WRKD_ID" +
            ",A.MACHINETYPE_ID," +
            "B.MTYPE_NAME" +
             ",A.COPR_ID," +
             "A.STATUS_ID," +
             "A.TEST_START," +
             "A.TEST_BY," +
             "A.TEST_FINISH," +
              "A.CLOSE_BY," +
             "A.MACHINE_ID" +
             " from COPR16_COPRUNNING_RT A" +
             " JOIN COPR16_MANCTYPE_MSTR B on A.MACHINETYPE_ID = B.MTYPE_ID" +
             " JOIN COPR16_WORKFLOW_DT C ON A.WRK_ID = C.WRK_ID AND A.WRKD_ID = C.WRKD_ID WHERE A.COPR_ID ='" + hCOPR_ID + "' AND SEQ_NO = '"+ hSEQ_NO + "' ";
            model.WorkProcess_List = await db.Database.SqlQuery<WorkProcess>(SQLCMD).ToListAsync();
            //return View(await db.COPR16_COPRUNNING_RT.ToListAsync());
            model.workProcess = new WorkProcess();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> MACHINE_SELECTforResult(string hCOPR_ID, string hSEQ_NO)
        {
            HandheldModel model = new HandheldModel(db);
            //model.cOPR16_COPRUNNING_List = await db.COPR16_COPRUNNING.Where(l => l.COP_STATUS.Equals("RDY")).ToListAsync();
            model.cOPR16_COPRUNNING = await db.COPR16_COPRUNNING.FindAsync(hCOPR_ID);// Header
            model.cOPR16_COPRUNNING_DT_List = await db.COPR16_COPRUNNING_DT.Where(l => l.COPR_ID.Equals(hCOPR_ID)).ToListAsync();// QR-CODE
            model.cOPR16_COPRUNNING_DT_BKL = model.cOPR16_COPRUNNING_DT_List.Find(l => l.FGTYPE_ID.Equals("BUNKLE"));// QR-CODE
            model.cOPR16_COPRUNNING_DT_SB = model.cOPR16_COPRUNNING_DT_List.Find(l => l.FGTYPE_ID.Equals("SEATBELT"));// QR-CODE
            model.cOPR16_COPRUNNING_RT_List = await db.COPR16_COPRUNNING_RT.Where(l => l.COPR_ID.Equals(hCOPR_ID) && l.SEQ_NO.Equals(hSEQ_NO)).ToListAsync(); // test details
            model.cOPR16_WORKFLOW_DT_List = await db.COPR16_WORKFLOW_DT.Where(l => l.WRK_ID.Equals(model.cOPR16_COPRUNNING.WRK_ID) && l.WRKD_SEQ.Equals(hSEQ_NO)).OrderBy(l => l.WRKD_SEQ).ToListAsync();
            //model.FG_DETAILS = "Count: " + model.cOPR16_COPRUNNING_DT_List.Count().ToString();
            model.cOPR16_WORKFLOW_DT = new COPR16_WORKFLOW_DT();
            
            model.FG_DETAILS = hCOPR_ID;
            string SQLCMD = "select distinct " +
            "C.WRKD_SEQ " +
            ",C.WRKD_DESC," +
            "A.FGTYPE_ID" +
            ",A.WRKD_ID" +
            ",A.MACHINETYPE_ID," +
            "B.MTYPE_NAME" +
             ",A.COPR_ID," +
             "A.STATUS_ID," +
             "A.TEST_START," +
             "A.TEST_BY," +
             "A.TEST_FINISH," +
              "A.CLOSE_BY," +
             "A.MACHINE_ID" +
             " from COPR16_COPRUNNING_RT A" +
             " JOIN COPR16_MANCTYPE_MSTR B on A.MACHINETYPE_ID = B.MTYPE_ID" +
             " JOIN COPR16_WORKFLOW_DT C ON A.WRK_ID = C.WRK_ID AND A.WRKD_ID = C.WRKD_ID WHERE A.COPR_ID ='" + hCOPR_ID + "' AND SEQ_NO = '" + hSEQ_NO + "' ";
            model.WorkProcess_List = await db.Database.SqlQuery<WorkProcess>(SQLCMD).ToListAsync();
            string mtype = model.WorkProcess_List.Where(m => m.WRKD_SEQ.Equals(model.WorkProcess_List.First().WRKD_SEQ)).FirstOrDefault().MACHINETYPE_ID.ToString();
            model.cOPR16_MACHINE_MSTR = await db.COPR16_MACHINE_MSTR.Where(l => l.MTYPE_ID.Equals(mtype)).FirstAsync();
            model.cOPR16_MANCTYPE_MSTR = await db.COPR16_MANCTYPE_MSTR.Where(l => l.MTYPE_ID.Equals(mtype)).FirstAsync();
            //return View(await db.COPR16_COPRUNNING_RT.ToListAsync());
            model.workProcess = new WorkProcess();
            return View(model);
        }
        public async Task<ActionResult> MACHINE_SELECT(string id)
        {
            HandheldModel model = new HandheldModel(db);
            //model.cOPR16_COPRUNNING_List = await db.COPR16_COPRUNNING.Where(l => l.COP_STATUS.Equals("RDY")).ToListAsync();
            model.cOPR16_COPRUNNING = await db.COPR16_COPRUNNING.FindAsync(id);// Header
            model.cOPR16_COPRUNNING_DT_List = await db.COPR16_COPRUNNING_DT.Where(l => l.COPR_ID.Equals(id)).ToListAsync();// QR-CODE
            model.cOPR16_COPRUNNING_DT_BKL = model.cOPR16_COPRUNNING_DT_List.Find(l => l.FGTYPE_ID.Equals("BUNKLE"));// QR-CODE
            model.cOPR16_COPRUNNING_DT_SB = model.cOPR16_COPRUNNING_DT_List.Find(l => l.FGTYPE_ID.Equals("SEATBELT"));// QR-CODE
            model.cOPR16_COPRUNNING_RT_List = await db.COPR16_COPRUNNING_RT.Where(l => l.COPR_ID.Equals(id)).ToListAsync(); // test details
            model.cOPR16_WORKFLOW_DT_List = await db.COPR16_WORKFLOW_DT.Where(l => l.WRK_ID.Equals(model.cOPR16_COPRUNNING.WRK_ID)).OrderBy(l => l.WRKD_SEQ).ToListAsync();
            //model.FG_DETAILS = "Count: " + model.cOPR16_COPRUNNING_DT_List.Count().ToString();
            model.cOPR16_WORKFLOW_DT = new COPR16_WORKFLOW_DT();

            model.FG_DETAILS = id;
            string SQLCMD = "select distinct " +
            "C.WRKD_SEQ " +
            ",C.WRKD_DESC," +
            "A.FGTYPE_ID" +
            ",A.WRKD_ID" +
            ",A.MACHINETYPE_ID," +
            "B.MTYPE_NAME" +
             ",A.COPR_ID," +
             "A.STATUS_ID," +
             "A.TEST_START," +
             "A.TEST_BY," +
             "A.TEST_FINISH," +
              "A.CLOSE_BY," +
             "A.MACHINE_ID" +
             " from COPR16_COPRUNNING_RT A" +
             " JOIN COPR16_MANCTYPE_MSTR B on A.MACHINETYPE_ID = B.MTYPE_ID" +
             " JOIN COPR16_WORKFLOW_DT C ON A.WRK_ID = C.WRK_ID AND A.WRKD_ID = C.WRKD_ID WHERE A.COPR_ID ='" + id + "' AND A.STATUS_ID = 'WAIT'  ";
            model.WorkProcess_List = await db.Database.SqlQuery<WorkProcess>(SQLCMD).ToListAsync();
            string mtype = model.WorkProcess_List.Where(m => m.WRKD_SEQ.Equals(model.WorkProcess_List.First().WRKD_SEQ)).FirstOrDefault().MACHINETYPE_ID.ToString();
            model.cOPR16_MACHINE_MSTR = await db.COPR16_MACHINE_MSTR.Where(l => l.MTYPE_ID.Equals(mtype)).FirstAsync();
            model.cOPR16_MANCTYPE_MSTR = await db.COPR16_MANCTYPE_MSTR.Where(l => l.MTYPE_ID.Equals(mtype)).FirstAsync();
            //return View(await db.COPR16_COPRUNNING_RT.ToListAsync());
            model.workProcess = new WorkProcess();
            return View(model);
        }
        public async Task<ActionResult> STARTCONFIRM(string id,string seq,string machine_id)
        {
            HandheldModel model = new HandheldModel(db);
            //model.cOPR16_COPRUNNING_List = await db.COPR16_COPRUNNING.Where(l => l.COP_STATUS.Equals("RDY")).ToListAsync();
            model.cOPR16_COPRUNNING = await db.COPR16_COPRUNNING.FindAsync(id);// Header
            model.cOPR16_COPRUNNING_DT_List = await db.COPR16_COPRUNNING_DT.Where(l => l.COPR_ID.Equals(id)).ToListAsync();// QR-CODE
            model.cOPR16_COPRUNNING_DT_BKL = model.cOPR16_COPRUNNING_DT_List.Find(l => l.FGTYPE_ID.Equals("BUNKLE"));// QR-CODE
            model.cOPR16_COPRUNNING_DT_SB = model.cOPR16_COPRUNNING_DT_List.Find(l => l.FGTYPE_ID.Equals("SEATBELT"));// QR-CODE
            model.cOPR16_COPRUNNING_RT_List = await db.COPR16_COPRUNNING_RT.Where(l => l.COPR_ID.Equals(id) && l.STATUS_ID.Contains("WAIT")).ToListAsync(); // test details
            model.cOPR16_WORKFLOW_DT_List = await db.COPR16_WORKFLOW_DT.Where(l => l.WRK_ID.Equals(model.cOPR16_COPRUNNING.WRK_ID)).OrderBy(l => l.WRKD_SEQ).ToListAsync();
            //model.FG_DETAILS = "Count: " + model.cOPR16_COPRUNNING_DT_List.Count().ToString();
            model.cOPR16_WORKFLOW_DT = new COPR16_WORKFLOW_DT();
            model.machiune_id = machine_id == null ? "" : machine_id;
            model.FG_DETAILS = id;
            string SQLCMD = "select top 1 * from (select distinct " +
            "C.WRKD_SEQ " +
            ",C.WRKD_DESC," +
            "A.FGTYPE_ID" +
            ",A.WRKD_ID" +
            ",A.MACHINETYPE_ID," +
            "B.MTYPE_NAME" +
             ",A.COPR_ID," +
             "A.STATUS_ID," +
             "A.TEST_START," +
             "A.TEST_BY," +
             "A.TEST_FINISH," +
              "A.CLOSE_BY," +
             "A.MACHINE_ID" +
             " from COPR16_COPRUNNING_RT A" +
             " JOIN COPR16_MANCTYPE_MSTR B on A.MACHINETYPE_ID = B.MTYPE_ID" +
             " JOIN COPR16_WORKFLOW_DT C ON A.WRK_ID = C.WRK_ID AND A.WRKD_ID = C.WRKD_ID WHERE A.COPR_ID ='" + id + "' AND C.WRKD_SEQ = '" + seq + "') x";
            model.WorkProcess_List = await db.Database.SqlQuery<WorkProcess>(SQLCMD).ToListAsync();

            string mtype = model.WorkProcess_List.FirstOrDefault().MACHINETYPE_ID.ToString();
            model.cOPR16_MACHINE_MSTR = await db.COPR16_MACHINE_MSTR.Where(l => l.MTYPE_ID.Equals(mtype)).FirstAsync();

            //return View(await db.COPR16_COPRUNNING_RT.ToListAsync());
            model.workProcess = new WorkProcess();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> TestResult(string hCOPR_ID, string hSEQ_NO)
        {
            ResultModel model = new ResultModel();

            model.cOPR16_COPRUNNING_RT = await db.COPR16_COPRUNNING_RT.Where(l => l.COPR_ID.Equals(hCOPR_ID) && l.SEQ_NO.Equals(hSEQ_NO)).FirstAsync();

            string SQLCMD = "select * from ( SELECT A.COPR_ID, A.PNO, A.FGTYPE_ID, A.WRK_ID,A.WRKD_ID," +
            "CONCAT(A.MACHINETYPE_ID, ': ', C.MTYPE_NAME) MACHINETYPE_ID, " +
            "A.RETURNTYPE_ID, A.[VALUE], A.STATUS_ID,  A.TEST_START,  A.TEST_FINISH, " +
            "A.TEST_BY, A.REV, A.SEQ_NO, " +
            "CONCAT(A.MACHINE_ID, ': ', B.MANC_DESC) MACHINE_ID, A.CLOSE_BY, A.RTDT_ID, A.RTDT_NAME, D.UNIT_TEXT AS RTDT_REF_ID " +
            "FROM COPR16_COPRUNNING_RT A " +
            "LEFT OUTER JOIN COPR16_MACHINE_MSTR B ON A.MACHINE_ID = B.MANC_ID AND A.MACHINE_ID is not null " +
            "LEFT OUTER JOIN COPR16_MANCTYPE_MSTR C ON A.MACHINETYPE_ID = C.MTYPE_ID " +
            "LEFT OUTER JOIN [dbo].[COPR16_UNITS_MSTR] D ON A.RTDT_REF_ID = D.UNIT_ID " +
            "WHERE A.COPR_ID='" + hCOPR_ID + "' AND A.SEQ_NO = '"+ hSEQ_NO + "'" +
            ") x " +
            "" +
            "ORDER BY CAST(x.SEQ_NO AS INT);";
            model.cOPR16_COPRUNNING_RT_List = db.Database.SqlQuery<COPR16_COPRUNNING_RT>(SQLCMD).ToList();

            //model.cOPR16_COPRUNNING_RT_List = await db.COPR16_COPRUNNING_RT.Where(l => l.COPR_ID.Equals(hCOPR_ID) && l.SEQ_NO.Equals(hSEQ_NO)).ToListAsync();
            model.cOPR16_RTDT_MSTR = await db.COPR16_RTDT_MSTR.Where(l => l.RTTYPE_ID.Equals(model.cOPR16_COPRUNNING_RT.RETURNTYPE_ID)).FirstAsync();
            model.cOPR16_MEASURETYPE_MSTR = await db.COPR16_MEASURETYPE_MSTR.Where(l => l.MSTYPE_ID.Equals(model.cOPR16_RTDT_MSTR.MSTYPE_ID)).FirstAsync();
            model.cOPR16_MEASURETYPE_MSTR_list = await db.COPR16_MEASURETYPE_MSTR.Where(l => l.MSTYPE_ID.Equals(model.cOPR16_RTDT_MSTR.MSTYPE_ID)).ToListAsync();

            COPR16_WORKFLOW_DT wrkName = await db.COPR16_WORKFLOW_DT.Where(l => l.WRKD_ID == model.cOPR16_COPRUNNING_RT.WRKD_ID).FirstAsync();
            COPR16_MANCTYPE_MSTR mctName = await db.COPR16_MANCTYPE_MSTR.Where(l => l.MTYPE_ID == model.cOPR16_COPRUNNING_RT.MACHINETYPE_ID).FirstAsync();
            COPR16_MACHINE_MSTR mcName = await db.COPR16_MACHINE_MSTR.Where(l => l.MANC_ID == model.cOPR16_COPRUNNING_RT.MACHINE_ID).FirstAsync();

            model.cOPR16_COPRUNNING_RT.MACHINETYPE_ID = model.cOPR16_COPRUNNING_RT.MACHINETYPE_ID + ":" + mctName.MTYPE_NAME;
            model.cOPR16_COPRUNNING_RT.MACHINE_ID = model.cOPR16_COPRUNNING_RT.MACHINE_ID + ":" + mcName.MANC_DESC;
            model.cOPR16_COPRUNNING_RT.WRKD_ID = wrkName.WRKD_DESC + "("+ model.cOPR16_COPRUNNING_RT.WRKD_ID + ")";
            if (model.cOPR16_MEASURETYPE_MSTR.OPTION_UNIT == true)
            {
                model.isOptionList = true;
                
                model.cOPR16_OPTDTVALUE_MSTR_List = await db.COPR16_OPTDTVALUE_MSTR.Where(l => l.OPTION_ID.Equals(model.cOPR16_MEASURETYPE_MSTR.OPT_ID)).ToListAsync();
            }
            else
            {
                model.cOPR16_RTDT_MSTR_list = await db.COPR16_RTDT_MSTR.ToListAsync();
                model.cOPR16_MEASURETYPE_MSTR_list = await db.COPR16_MEASURETYPE_MSTR.ToListAsync();
                model.cOPR16_OPTDTVALUE_MSTR_List = await db.COPR16_OPTDTVALUE_MSTR.ToListAsync();
                model.isOptionList = false;

            }
            
            return View(model);
        }
        // GET: COPR16_COPRUNNING_RT/VIEW_DT/5
        public async Task<ActionResult> VIEW_DT(string id)
        {
            HandheldModel model = new HandheldModel(db);
            //model.cOPR16_COPRUNNING_List = await db.COPR16_COPRUNNING.Where(l => l.COP_STATUS.Equals("RDY")).ToListAsync();
            model.cOPR16_COPRUNNING = await db.COPR16_COPRUNNING.FindAsync(id);// Header
            model.cOPR16_COPRUNNING_DT_List = await db.COPR16_COPRUNNING_DT.Where(l => l.COPR_ID.Equals(id)).ToListAsync();// QR-CODE
            model.cOPR16_COPRUNNING_DT_BKL = model.cOPR16_COPRUNNING_DT_List.Find(l => l.FGTYPE_ID.Equals("BUNKLE"));// QR-CODE
            model.cOPR16_COPRUNNING_DT_SB = model.cOPR16_COPRUNNING_DT_List.Find(l => l.FGTYPE_ID.Equals("SEATBELT"));// QR-CODE
            model.cOPR16_COPRUNNING_RT_List = await db.COPR16_COPRUNNING_RT.Where(l => l.COPR_ID.Equals(id)).ToListAsync(); // test details
            model.cOPR16_WORKFLOW_DT_List = await db.COPR16_WORKFLOW_DT.Where(l => l.WRK_ID.Equals(model.cOPR16_COPRUNNING.WRK_ID)).OrderBy(l => l.WRKD_SEQ).ToListAsync();
            //model.FG_DETAILS = "Count: " + model.cOPR16_COPRUNNING_DT_List.Count().ToString();
            model.cOPR16_WORKFLOW_DT = new COPR16_WORKFLOW_DT();
            model.FG_DETAILS = id;

            string SQLCMD = "SELECT * FROM (select distinct "+
            "C.WRKD_SEQ "+
            ",C.WRKD_DESC,"+
            "A.FGTYPE_ID"+
            ",A.WRKD_ID"+
            ",A.MACHINETYPE_ID + ' ' + B.MTYPE_NAME AS MACHINETYPE_ID," +
            "B.MTYPE_NAME"+
             ",A.COPR_ID,"+
             "A.STATUS_ID,"+
             "A.TEST_START,"+
             "A.TEST_BY,"+
             "A.TEST_FINISH,"+
              "A.CLOSE_BY,"+
             "A.MACHINE_ID + ' ('+ B.MTYPE_NAME +') ' MACHINE_ID " +
             " from COPR16_COPRUNNING_RT A"+
             " JOIN COPR16_MANCTYPE_MSTR B on A.MACHINETYPE_ID = B.MTYPE_ID"+
             " JOIN COPR16_WORKFLOW_DT C ON A.WRK_ID = C.WRK_ID AND A.WRKD_ID = C.WRKD_ID WHERE A.COPR_ID ='"+id+ "') A ORDER BY cast(WRKD_SEQ AS INT) ";
            model.WorkProcess_List = await db.Database.SqlQuery<WorkProcess>(SQLCMD).ToListAsync();
            
            //return View(await db.COPR16_COPRUNNING_RT.ToListAsync());

            COPR16_POSITION_MSTR posName = await db.COPR16_POSITION_MSTR.Where(l => l.POS_ID == model.cOPR16_COPRUNNING.POSITION_ID).FirstAsync();
            COPR16_MODEL_MSTR modelName = await db.COPR16_MODEL_MSTR.Where(l => l.MODEL_ID == model.cOPR16_COPRUNNING.MODEL_ID).FirstAsync();

            model.cOPR16_COPRUNNING.POSITION_ID =  posName.POS_DESC;
            model.cOPR16_COPRUNNING.MODEL_ID =  modelName.MODEL_DESC;
            model.workProcess = new WorkProcess();
            return View(model);
        }


        // GET: COPR16_COPRUNNING_RT/REPORTS
        public async Task<ActionResult> REPORTS()
        {
            return View(await db.COPR16_COPRUNNING_RT.ToListAsync());
        }
        // GET: COPR16_COPRUNNING_RT/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            COPR16_COPRUNNING_RT cOPR16_COPRUNNING_RT = await db.COPR16_COPRUNNING_RT.FindAsync(id);
            if (cOPR16_COPRUNNING_RT == null)
            {
                return HttpNotFound();
            }
            return View(cOPR16_COPRUNNING_RT);
        }

        // GET: COPR16_COPRUNNING_RT/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: COPR16_COPRUNNING_RT/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "COPR_ID,PNO,FGTYPE_ID,WRK_ID,WRKD_ID,MACHINETYPE_ID,RETURNTYPE_ID,VALUE,STATUS_ID,TEST_START,TEST_FINISH,TEST_BY,REV,SEQ_NO")] COPR16_COPRUNNING_RT cOPR16_COPRUNNING_RT)
        {
            if (ModelState.IsValid)
            {
                db.COPR16_COPRUNNING_RT.Add(cOPR16_COPRUNNING_RT);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(cOPR16_COPRUNNING_RT);
        }

        // GET: COPR16_COPRUNNING_RT/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            COPR16_COPRUNNING_RT cOPR16_COPRUNNING_RT = await db.COPR16_COPRUNNING_RT.FindAsync(id);
            if (cOPR16_COPRUNNING_RT == null)
            {
                return HttpNotFound();
            }
            return View(cOPR16_COPRUNNING_RT);
        }

        // POST: COPR16_COPRUNNING_RT/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "COPR_ID,PNO,FGTYPE_ID,WRK_ID,WRKD_ID,MACHINETYPE_ID,RETURNTYPE_ID,VALUE,STATUS_ID,TEST_START,TEST_FINISH,TEST_BY,REV,SEQ_NO")] COPR16_COPRUNNING_RT cOPR16_COPRUNNING_RT)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cOPR16_COPRUNNING_RT).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(cOPR16_COPRUNNING_RT);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveResult(ResultSaveTest jsonString)
        {
            var rowData = new JsonResult();
            var valueList = jsonString.valueList;
            string SQLCMD2 = "exec dbo.sp_update_copr_step @VALUE,@CLOSEBY,@COPR_ID,@SEQ_NO,@RTDT_ID ";
            try
            {
                foreach (var item in valueList)
                {
                    List<SqlParameter> param2 = new List<SqlParameter>();
                    param2.Add(new SqlParameter("@VALUE", item.VALUE == null ? "" : item.VALUE));
                    param2.Add(new SqlParameter("@CLOSEBY", jsonString.CLOSE_BY == null ? "" : jsonString.CLOSE_BY));
                    param2.Add(new SqlParameter("@COPR_ID", jsonString.COPR_ID == null ? "" : jsonString.COPR_ID));
                    param2.Add(new SqlParameter("@SEQ_NO", jsonString.SEQ_NO == null ? "" : jsonString.SEQ_NO));
                    param2.Add(new SqlParameter("@RTDT_ID", item.RTDT_ID == null ? "" : item.RTDT_ID));
                    await db.Database.ExecuteSqlCommandAsync(SQLCMD2, param2.ToArray());
                }
                rowData.Data = "Success";
            }
            catch(Exception e)
            {
                rowData.Data = "Error: " + e.Message;
            }


            return Json(rowData, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> checkMachineLock(string MANC_ID,string MANCTYPE)
        {
            var rowData = new JsonResult();

            string SQLCMD2 = "exec dbo.sp_check_mahine_lock @MANC_ID,@MANC_TYPE";
            //List<SqlParameter> param2 = new List<SqlParameter>();
            //param2.Add(new SqlParameter("@MANC_ID", MANC_ID == null ? "" : MANC_ID));
            //await db.Database.ExecuteSqlCommandAsync(SQLCMD2, param2.ToArray());

            using (var cmd = db.Database.Connection.CreateCommand())
            {
                await db.Database.Connection.OpenAsync();
                cmd.CommandText = SQLCMD2;
                cmd.Parameters.Add(new SqlParameter("@MANC_ID", MANC_ID == null ? "" : MANC_ID));
                cmd.Parameters.Add(new SqlParameter("@MANC_TYPE", MANCTYPE == null ? "" : MANCTYPE));
                DbDataReader reader = await cmd.ExecuteReaderAsync();
                {
                    var model = Serialize((SqlDataReader)reader);
                    rowData.Data = model;
                }
            }

            return Json(rowData, JsonRequestBehavior.AllowGet);
        }
        public IEnumerable<Dictionary<string, object>> Serialize(SqlDataReader reader)
        {
            var results = new List<Dictionary<string, object>>();
            var cols = new List<string>();
            for (var i = 0; i < reader.FieldCount; i++)
                cols.Add(reader.GetName(i));

            while (reader.Read())
                results.Add(SerializeRow(cols, reader));

            return results;
        }
        private Dictionary<string, object> SerializeRow(IEnumerable<string> cols,
                                                        SqlDataReader reader)
        {
            var result = new Dictionary<string, object>();
            foreach (var col in cols)
                result.Add(col, reader[col]);
            return result;
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> StartTestSave(StartSaveTest jsonString)
        {

            
            var rowData = new JsonResult();

            //string SQLCMD2 = "exec dbo.sp_save_teststep_to_queue @COPR_ID";
            //List<SqlParameter> param2 = new List<SqlParameter>();
            //param2.Add(new SqlParameter("@COPR_ID", jsonString.COPR_ID == null ? "" : jsonString.COPR_ID));
            //await db.Database.ExecuteSqlCommandAsync(SQLCMD2, param2.ToArray());

            string SQLCMD1 = "exec dbo.sp_mahine_lock @MANC_ID,@COPR_ID,@TEST_BY";
            List<SqlParameter> param1 = new List<SqlParameter>();
            param1.Add(new SqlParameter("@MANC_ID", jsonString.MACHINE_ID == null ? "" : jsonString.MACHINE_ID));
            param1.Add(new SqlParameter("@COPR_ID", jsonString.COPR_ID == null ? "" : jsonString.COPR_ID));
            param1.Add(new SqlParameter("@TEST_BY", jsonString.TEST_BY == null ? "" : jsonString.TEST_BY));
            await db.Database.ExecuteSqlCommandAsync(SQLCMD1, param1.ToArray());

            string SQLCMD3 = "exec dbo.sp_save_StartTestSave @COPR_ID,@MACHINETYPE_ID,@SEQ_NO,@WRKD_ID,@TEST_BY,@MACHINE_ID";
            List<SqlParameter> param3 = new List<SqlParameter>();
            param3.Add(new SqlParameter("@COPR_ID", jsonString.COPR_ID == null ? "" : jsonString.COPR_ID));
            param3.Add(new SqlParameter("@MACHINETYPE_ID", jsonString.MACHINETYPE_ID == null ? "" : jsonString.MACHINETYPE_ID));
            param3.Add(new SqlParameter("@SEQ_NO", jsonString.WRKD_SEQ == null ? "" : jsonString.WRKD_SEQ));
            param3.Add(new SqlParameter("@WRKD_ID", jsonString.WRKD_ID == null ? "" : jsonString.WRKD_ID));
            param3.Add(new SqlParameter("@TEST_BY", jsonString.TEST_BY == null ? "" : jsonString.TEST_BY));
            param3.Add(new SqlParameter("@MACHINE_ID", jsonString.MACHINE_ID == null ? "" : jsonString.MACHINE_ID));
            
            await db.Database.ExecuteSqlCommandAsync(SQLCMD3, param3.ToArray());

            /*
            COPR16_COPRUNNING_RT cOPR16_COPRUNNING_RT = await db.COPR16_COPRUNNING_RT.Where(l => l.COPR_ID.Equals(jsonString.COPR_ID) 
                && l.MACHINETYPE_ID.Equals(jsonString.MACHINETYPE_ID)
                && l.SEQ_NO.Equals(jsonString.WRKD_SEQ)
                && l.WRKD_ID.Equals(jsonString.WRKD_ID)
                ).FirstAsync();

            cOPR16_COPRUNNING_RT.STATUS_ID = "TESTING";                                                                                                                                                                                                                                        
            cOPR16_COPRUNNING_RT.TEST_START = AppPropModel.today;
            cOPR16_COPRUNNING_RT.TEST_BY = jsonString.TEST_BY;
            cOPR16_COPRUNNING_RT.MACHINE_ID = jsonString.MACHINE_ID;

            db.Entry(cOPR16_COPRUNNING_RT).State = EntityState.Modified;

            await db.SaveChangesAsync();

            COPR16_COPRUNNING cOPR16_COPRUNNING = await db.COPR16_COPRUNNING.Where(l => l.COPR_ID.Equals(jsonString.COPR_ID)
                ).FirstAsync();

            cOPR16_COPRUNNING.COP_STATUS = "TESTING";
            db.Entry(cOPR16_COPRUNNING).State = EntityState.Modified;

            await db.SaveChangesAsync();
            */

            rowData.Data = jsonString;
            return Json(rowData, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveAbortTest(StartSaveTest jsonString)
        {
            var rowData = new JsonResult();

            COPR16_COPRUNNING_RT cOPR16_COPRUNNING_RT = await db.COPR16_COPRUNNING_RT.Where(l => l.COPR_ID.Equals(jsonString.COPR_ID)
                && l.MACHINETYPE_ID.Equals(jsonString.MACHINETYPE_ID)
                && l.SEQ_NO.Equals(jsonString.WRKD_SEQ)
                && l.WRKD_ID.Equals(jsonString.WRKD_ID)
                ).FirstAsync();

            cOPR16_COPRUNNING_RT.STATUS_ID = "WAIT";
            cOPR16_COPRUNNING_RT.TEST_FINISH = AppPropModel.today;
            cOPR16_COPRUNNING_RT.CLOSE_BY = jsonString.TEST_BY;
            cOPR16_COPRUNNING_RT.MACHINE_ID = null;

            db.Entry(cOPR16_COPRUNNING_RT).State = EntityState.Modified;

            await db.SaveChangesAsync();
            rowData.Data = jsonString;
            return Json(rowData, JsonRequestBehavior.AllowGet);
        }


        // GET: COPR16_COPRUNNING_RT/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            COPR16_COPRUNNING_RT cOPR16_COPRUNNING_RT = await db.COPR16_COPRUNNING_RT.FindAsync(id);
            if (cOPR16_COPRUNNING_RT == null)
            {
                return HttpNotFound();
            }
            return View(cOPR16_COPRUNNING_RT);
        }

        // POST: COPR16_COPRUNNING_RT/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            COPR16_COPRUNNING_RT cOPR16_COPRUNNING_RT = await db.COPR16_COPRUNNING_RT.FindAsync(id);
            db.COPR16_COPRUNNING_RT.Remove(cOPR16_COPRUNNING_RT);
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
        private bool getBlankOrNull(string val)
        {
            if (val == "") return true;
            else if (val == null) return true;
            else return false;
        }
    }
}
