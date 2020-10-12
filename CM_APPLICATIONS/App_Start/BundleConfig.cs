using System.Web;
using System.Web.Optimization;

namespace CM_APPLICATIONS
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery-qrcode").Include(
                        "~/Scripts/jquery.qrcode.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/sheetjs").Include(
                        "~/Scripts/sheetjs/xlsx.full.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));
            bundles.Add(new ScriptBundle("~/bundles/printThis").Include(
                        "~/Scripts/printThis.js"));
            bundles.Add(new ScriptBundle("~/bundles/jQuery-print").Include(
                        "~/Scripts/jQuery.print.js"));
            bundles.Add(new ScriptBundle("~/bundles/html2canvas").Include(
            "~/Scripts/html2canvas.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery-ui").Include(
                        "~/Scripts/jquery-ui.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css",
                      "~/Content/themes/jquery-ui.css",
                      "~/Content/themes/jquery-ui.theme.css",
                      "~/Content/themes/jquery-ui.structure.css"));
            
        }
    }
}
