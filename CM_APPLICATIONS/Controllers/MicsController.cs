using CM_APPLICATIONS.Models;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Web;
using System.Web.Mvc;


namespace CM_APPLICATIONS.Controllers
{
    public class MicsController : Controller
    {
        // GET: Mics
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult QRMAKER()
        {
            return View();
        }

        [HttpPost]
        public JsonResult LABEL_PRINTING(qrPrint data)
        {

            /*Generate QRCOde*/
            bool export_pdf = data.pdf != null ? (data.pdf.ToLower().Equals("1") ? true : false) : false;
            string strText = data.copnoa != null ? data.copnoa : "no parameter";
            string fileName = Guid.NewGuid().ToString() + ".jpg";
            string pathFile = Server.MapPath("/qrcode-cache/" + fileName);
            string pathUrl = Server.UrlPathEncode("http://" + Request.Headers["host"] + "/qrcode-cache/" + fileName);
            Byte[] b;
            Byte[] b2;
            //b = Utils.ImageToByteArray(Utils.genQR(strText));
            Image img1 = Utils.genQR(strText);

            Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();
            //Response.ContentType = "image/jpeg";
            img1.Save(pathFile, System.Drawing.Imaging.ImageFormat.Jpeg);

            /* Prepare Report **/
            JsonResult jsonData = new JsonResult();
            ReportDocument RptDoc = new ReportDocument();
            string mode = data.mode != null ? data.mode : "2";
            string rptFileName = "/RPTTEMPLATE/" + ConfigurationManager.AppSettings["LABEL_FILE_TEMPLATE"]; 
            RptDoc.Load(Server.MapPath(rptFileName));


            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult QR_PRINTING(string parameter, string pdf)
        {
            bool export_pdf = pdf != null ? (pdf.ToLower().Equals("1") ? true : false) : false;
            string strText = parameter != null ? parameter : "no parameter";
            string fileName = Guid.NewGuid().ToString() + ".jpg";
            string pathFile = Server.MapPath("/qrcode-cache/" + fileName);
            string pathUrl = Server.UrlPathEncode("http://" + Request.Headers["host"] + "/qrcode-cache/" + fileName);
            Byte[] b;
            Byte[] b2;
            //b = Utils.ImageToByteArray(Utils.genQR(strText));
            Image img1 = Utils.genQR(strText);

            Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();
            //Response.ContentType = "image/jpeg";
            img1.Save(pathFile, System.Drawing.Imaging.ImageFormat.Jpeg);

            ReportDocument RptDoc = new ReportDocument();
            string url = "http://localhost/" + pathFile;
            //string mode = data.mode != null ? data.mode : "2";
            //string rptFileName = "QRMAKER.rpt";
            string rptFileName = ConfigurationManager.AppSettings["QRMAKER_FILE_TEMPLATE"];
            RptDoc.Load(Path.Combine(Server.MapPath("/RPTTEMPLATE/"), rptFileName));
            RptDoc.SetParameterValue("image_url", url);

            //ExportOptions exopt = default(ExportOptions);
            DiskFileDestinationOptions dfdopt = new DiskFileDestinationOptions();
            //string fname = "QRMAKER.pdf";
            string fname = ConfigurationManager.AppSettings["QRMAKER_FILE_EXPORT"];
            dfdopt.DiskFileName = Server.MapPath(fname);

            string PathFs = Path.Combine(Server.MapPath("/qrcode-cache/"), fname);

            DiskFileDestinationOptions dfdoFile = new DiskFileDestinationOptions();

            FileInfo file = new FileInfo(PathFs);
            dfdoFile.DiskFileName = PathFs;

            RptDoc.ExportOptions.ExportDestinationType = CrystalDecisions.Shared.ExportDestinationType.DiskFile;
            RptDoc.ExportOptions.ExportFormatType = CrystalDecisions.Shared.ExportFormatType.PortableDocFormat;
            RptDoc.ExportOptions.DestinationOptions = dfdoFile;

            if (export_pdf) {
                RptDoc.Export();
                b2 = System.IO.File.ReadAllBytes(PathFs);
                RptDoc.Clone();
                return File(b2, "application/pdf");
            }else{
                RptDoc.PrintToPrinter(1, true, 0, 0);
                RptDoc.Clone();
                return View();
            }
            
        }

        [HttpGet]
        public ActionResult QRCODE(string parameter)
        {
            string strText = parameter != null ? parameter : "no parameter";
            Byte[] b;
            b = Utils.ImageToByteArray(Utils.genQR(strText));

            Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();
            Response.ContentType = "image/jpeg";
            return File(b, "image/jpeg", "qrcode.jpg");
        }
        [HttpGet]
        public ActionResult QRCODE2(string parameter)
        {
            string strText = parameter != null ? parameter : "no parameter";
            Byte[] b;
            //b = Utils.ImageToByteArray(Utils.genQR(strText));
            Image img1 = Utils.genQR(strText);
            string fileName = Guid.NewGuid().ToString() + ".jpg";
            Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();
            //Response.ContentType = "image/jpeg";
            img1.Save(Server.MapPath("/"+ fileName), System.Drawing.Imaging.ImageFormat.Jpeg);
            //return File(b, "image/jpeg");
            return Redirect("/" + fileName);
        }
        [HttpGet]
        public JsonResult genQRCODE(string parameter)
        {
            JsonResult rs = new JsonResult(); 
            string strText = parameter != null ? parameter : "no parameter";
            string fileName = Guid.NewGuid().ToString() + ".jpg";
            string pathFile = Server.MapPath("/qrcode-cache/" + fileName);
            string pathUrl = Server.UrlPathEncode("http://"+Request.Headers["host"] +  "/qrcode-cache/" + fileName);
            Byte[] b;
            //b = Utils.ImageToByteArray(Utils.genQR(strText));
            Image img1 = Utils.genQR(strText);
            
            Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();
            //Response.ContentType = "image/jpeg";
            img1.Save(pathFile, System.Drawing.Imaging.ImageFormat.Jpeg);
            //return File(b, "image/jpeg");
            rs.Data = new qrFile() { fileName= pathFile,Url = pathUrl,TextValue = strText };
            //return rs;
            return Json(rs, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public FileContentResult IMG()
        {

            Byte[] b;
            b = System.IO.File.ReadAllBytes(Server.MapPath( "/1.jpg"));

        /*
            Response.ClearContent();
            Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();

            Response.AddHeader("Content-Disposition", "attachment; filename=image.jpg" );
            Response.AddHeader("Content-Length", b.Length.ToString());
            Response.ContentType = "image/jpeg";
            //Response.OutputStream.Write(b,0,b.Length);
        */
            
            return File(b, "image/jpeg"); 
        }
        public ActionResult TextToImg(string text)
        {
            string strText = text != null ? text : "no text";
            Byte[] b;
            Image image = Utils.DrawText(strText, new Font(FontFamily.GenericSansSerif, 72), Color.Magenta, Color.GreenYellow);
            b = Utils.ImageToByteArray(image);

            Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();
            Response.ContentType = "image/jpeg";
            return File(b, "image/jpeg","textimg.jpg");
        }

        public ActionResult CameraTest()
        {
            return View();
        }
    }
}