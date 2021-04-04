using CM_APPLICATIONS.Models;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
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
        public ActionResult QRREADING()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> USERACCESS()
        {
            List<userRoles> userRolesList = new List<userRoles>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DatabaseServer"].ToString()))
            {
                if(con.State == System.Data.ConnectionState.Closed || con.State == System.Data.ConnectionState.Broken)
                {
                    await con.OpenAsync();
                }
                string SQLCMD = "exec dbo.COPR16_CUST_REPORT_USELIST";
                using (var cmd = con.CreateCommand())
                {

                    cmd.CommandText = SQLCMD;
                    DbDataReader reader = await cmd.ExecuteReaderAsync();
                    {
                        //var model = Utils.Serialize((SqlDataReader)reader);
                        while (reader.Read())
                        {
                            userRoles data = new userRoles();
                            data.UserName = reader["UID"].ToString();
                            data.UserRoles = reader["ROLES"].ToString();
                            data.ExpireDateTime = reader["EXPIRE"] != DBNull.Value ? Convert.ToDateTime(reader["EXPIRE"]) : default(DateTime);                           
                            data.NeverExpire = reader["NEVER_EXPIRE"] != DBNull.Value ? Convert.ToBoolean(reader["NEVER_EXPIRE"]) : default(bool);
                            data.CreateDate = reader["CREATEDATE"] != DBNull.Value ? Convert.ToDateTime(reader["CREATEDATE"]): default(DateTime);
                            data.CreateBy = reader["CREATEBY"].ToString();
                            data.UpdateDate = reader["UPDATEDATE"] != DBNull.Value ? Convert.ToDateTime(reader["UPDATEDATE"]) : default(DateTime);
                            data.UpdateBy = reader["UPDATEBY"].ToString();

                            userRolesList.Add(data);
                        }
                       
                    }
                }


            }
            return View(userRolesList);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> UserRemove(string uid)
        {
            JsonResult rowData = new JsonResult();
            List<userRoles> userRolesList = new List<userRoles>();
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DatabaseServer"].ToString()))
                {
                    if (con.State == System.Data.ConnectionState.Closed || con.State == System.Data.ConnectionState.Broken)
                    {
                        await con.OpenAsync();
                    }
                    string SQLCMD = "exec dbo.COPR16_CUST_REPORT_USEREMOVE @UID";
                    using (var cmd = con.CreateCommand())
                    {
                        cmd.CommandText = SQLCMD;
                        cmd.Parameters.Add(new SqlParameter("@UID", uid));
                        await cmd.ExecuteNonQueryAsync();
                    }
                }
                rowData.Data = "OK";
            }
            catch (Exception ex)
            {
                rowData.Data = "ERROR: " + ex.Message;
            }
            return Json(rowData, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> UserAdd(string uid,string role,string expire,string neverExpire,string uname)
        {
            JsonResult rowData = new JsonResult();
            List<userRoles> userRolesList = new List<userRoles>();
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DatabaseServer"].ToString()))
                {
                    if (con.State == System.Data.ConnectionState.Closed || con.State == System.Data.ConnectionState.Broken)
                    {
                        await con.OpenAsync();
                    }
                    string SQLCMD = "exec [dbo].[COPR16_CUST_REPORT_USERADD] @UID, @ROLES, @EXPIRE, @NEVER_EXPIRE, @UNAME ";
                    using (var cmd = con.CreateCommand())
                    {
                        cmd.CommandText = SQLCMD;
                        cmd.Parameters.Add(new SqlParameter("@UID", uid));
                        cmd.Parameters.Add(new SqlParameter("@ROLES", role));
                        cmd.Parameters.Add(new SqlParameter("@EXPIRE", expire == null || expire == "" ? Convert.ToDateTime("2049-Dec-31") : Convert.ToDateTime(expire)));
                        cmd.Parameters.Add(new SqlParameter("@NEVER_EXPIRE", neverExpire == "0"? 0:1));
                        cmd.Parameters.Add(new SqlParameter("@UNAME", uname));
                        await cmd.ExecuteNonQueryAsync();
                    }
                }
                rowData.Data = "OK";
            }
            catch (Exception ex)
            {
                rowData.Data = "ERROR: " + ex.Message;
            }
            return Json(rowData, JsonRequestBehavior.AllowGet);
        }
        //public JsonResult LABEL_PRINTING()
        public ActionResult LABEL_PRINTING()
        {

            /*Generate QRCOde*/
            /*
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
            */
            /* Prepare Report **/
            /*
            JsonResult jsonData = new JsonResult();
            ReportDocument RptDoc = new ReportDocument();
            string mode = data.mode != null ? data.mode : "2";
            string rptFileName = "/RPTTEMPLATE/" + ConfigurationManager.AppSettings["LABEL_FILE_TEMPLATE"]; 
            RptDoc.Load(Server.MapPath(rptFileName));


            return Json(jsonData, JsonRequestBehavior.AllowGet);
            */
            return View();
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