using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CM_APPLICATIONS.Models
{
    public class rptModels
    {
        public string ModelId { get; set; }
        public string ModelName { get; set; }
        
    }
    public class rptYear
    {
        public string YearId { get; set; }
        public string YearName { get; set; }
        public string ModelName { get; set; }
    }
    public class rptMonths
    {
        public string YearId { get; set; }
        public string MonthId { get; set; }
        public string MonthName { get; set; }
    }
    public class rptReports
    {
        public int YearId { get; set; }
        public int MonthId { get; set; }
        public string Model { get; set; }
        public int ReportType { get; set; }
        public string ReportBody { get; set; }
    }
    public class viewReport
    {
        public List<rptReports> reportData { get; set; }
        public List<ReportTab> reportTabs { get; set; }
    }
    public class saveDataRpt
    {
        public string ModelStr { get; set; }
        public int YearStr { get; set; }
        public int MonthStr { get; set; }
        public string UserName { get; set; }
        public int ReportType { get; set; }
        [AllowHtml]
        public string ReportBody { get; set; }
    }
}