using System;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using OfficeOpenXml;
using OfficeOpenXml.Drawing.Chart;

namespace ExcelLibs_Test.Models
{
    public class bmsExcelTools
    {
        public string fileName { set; get; }
        public string pathName { set; get; }

        public string FullPathOutputFileName { set; get; }
        public string FullPathInputFileName { set; get; }
        public string ofileName { set; get; }
        private FileStream fs { set; get; }

        private ExcelWorkbook templateWorkbook { get; set; }
        private ExcelWorksheet sheet1 { get; set; }
        private ExcelPackage xlsObj { get; set; }
        public ExcelWorksheet getWorkingSheet()
        {
            return sheet1;
        }
        public bmsExcelTools()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            sheet1 = null;
            fs = null;
            templateWorkbook = null;
            ExcelPackage xlsObj  = new ExcelPackage();
        }
        public void setWorkingSheet(string sheetName)
        {
            //sheet1 = templateWorkbook.GetSheet(sheetName);
            sheet1 = templateWorkbook.Worksheets[sheetName];
        }
        public void saveToFile(string fName)
        {
            /*
            using (FileStream fs = new FileStream(fName, FileMode.CreateNew, FileAccess.Write))
            {
                templateWorkbook.Write(fs);
               
            }
            */
            xlsObj.SaveAs(new FileInfo(fName));
        }


        public void setWorkingSheet(int sheetNo)
        {
            sheet1 = templateWorkbook.Worksheets[sheetNo];
        }
        public void updateformular()
        {
            if(sheet1 != null)
            {
                //sheet1.ForceFormulaRecalculation = true;
                //sheet1.Calculate();
            }
            if(this.xlsObj.Workbook != null)
            {
                xlsObj.Workbook.Calculate();
            }
        }
        public void openExcelFile()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            if (fs != null) fs.Close();
            

            FullPathInputFileName = Path.Combine("", fileName);
            fs = new FileStream(FullPathInputFileName, FileMode.Open, FileAccess.Read);
            xlsObj = new ExcelPackage(fs);
            templateWorkbook = xlsObj.Workbook;
            sheet1 = templateWorkbook.Worksheets[0];
        }
        public MemoryStream saveASMemStream()
        {
            var memoryStream = new MemoryStream();
            xlsObj.SaveAs(memoryStream);
            //templateWorkbook.Write(memoryStream);
            memoryStream.FlushAsync();
            memoryStream.Position = 0;
            return memoryStream;

        }
        public void openExcelFile(string strPathName,string strfileName)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            if (fs != null) fs.Close();
           
            fileName = strfileName;
            FullPathInputFileName = Path.Combine(strPathName, fileName);
            fs = new FileStream(FullPathInputFileName, FileMode.Open, FileAccess.Read);
            fs.Position = 0;
            //FileInputStream fs1 = new FileInputStream(FullPathInputFileName);

            xlsObj = new ExcelPackage(fs);
            templateWorkbook = xlsObj.Workbook;
            //templateWorkbook = new XSSFWorkbook(fs);

            sheet1 = templateWorkbook.Worksheets[0];
        }
        public void saveAs(string pathString, string dfileName,string sfileName)
        {
            FileInfo _file = new FileInfo(Path.Combine(pathString, fileName));
            FileInfo _fileOut = new FileInfo(Path.Combine(pathString, dfileName));
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage xlsx = new ExcelPackage(_file);
            xlsx.SaveAs(_fileOut);
            
        }
        [Obsolete]
        public void saveAs(string pathString, string dfileName)
        {

           
            string temppath = pathString;
            if (!Directory.Exists(temppath))
            {
                Directory.CreateDirectory(temppath);
            }
            ofileName = dfileName;
            FullPathOutputFileName = Path.Combine(temppath, ofileName);
            FileStream tempstream = new FileStream(FullPathOutputFileName, FileMode.Create, FileAccess.Write);
            xlsObj.SaveAs(tempstream);
            fs = null;
            templateWorkbook = null;
            sheet1 = null;
        }

        public void setCellValue(int rowNumber,int cellNumber, string value)
        {
            getCell(rowNumber, cellNumber).Value = value;
            /*
            if (rowNumber <= 0) return;
            if (cellNumber <= 0) return;
            if (sheet1 != null)
            {
                int rowCount = sheet1.Dimension.End.Row;
                int colCount = sheet1.Dimension.End.Column;
                if (rowNumber > rowCount)
                {
                    sheet1.InsertRow(rowCount,1);
                }
                if(cellNumber > colCount)
                {
                    sheet1.InsertColumn(colCount,1);
                }

                sheet1.SetValue(rowNumber, cellNumber, value);
            }
            */
        }
        public void setCellValue(int rowNumber, int cellNumber, int value)
        {
            getCell(rowNumber, cellNumber).Value = value;
            /*
            if (rowNumber <= 0) return;
            if (cellNumber <= 0) return;
            if (sheet1 != null)
            {
                int rowCount = sheet1.Dimension.End.Row;
                int colCount = sheet1.Dimension.End.Column;
                if (rowNumber > rowCount)
                {
                    sheet1.InsertRow(rowCount, 1);
                }
                if (cellNumber > colCount)
                {
                    sheet1.InsertColumn(colCount, 1);
                }

                sheet1.SetValue(rowNumber, cellNumber, value);
            }
            */
        }
        public ExcelRange getCell(int rowNumber, int cellNumber)
        {
            return sheet1.Cells[rowNumber, cellNumber];
        }
        public void setCellValue(int rowNumber, int cellNumber, double value)
        {
            getCell(rowNumber, cellNumber).Value = value;
            /*
            if (rowNumber <= 0) return;
            if (cellNumber <= 0) return;
            if (sheet1 != null)
            {
                int rowCount = 1;
                int colCount = 1;
                if (sheet1.Dimension == null) {
                    rowCount = 1;
                    colCount = 1;
                }
                else
                {
                    rowCount = sheet1.Dimension.End.Row;
                    colCount = sheet1.Dimension.End.Column;
                }
                //int rowCount = sheet1.Dimension.End.Row;
                
                
                if (rowNumber > rowCount)
                {
                    sheet1.InsertRow(rowCount, 1);
                }
                if (cellNumber > colCount)
                {
                    sheet1.InsertColumn(colCount, 1);
                }

                sheet1.SetValue(rowNumber, cellNumber, value);
            }
            */
        }
        public void setCellValue(int rowNumber, int cellNumber, float value)
        {
            getCell(rowNumber, cellNumber).Value = value;
            /*
            if (rowNumber <= 0) return;
            if (cellNumber <= 0) return;
            if (sheet1 != null)
            {
                int rowCount = sheet1.Dimension.End.Row;
                int colCount = sheet1.Dimension.End.Column;
                if (rowNumber > rowCount)
                {
                    sheet1.InsertRow(rowCount, 1);
                }
                if (cellNumber > colCount)
                {
                    sheet1.InsertColumn(colCount, 1);
                }

                sheet1.SetValue(rowNumber, cellNumber, value);
            }
            */
        }
        public void setCellFormular(int rowNumber, int cellNumber, string formularString)
        {
            getCell(rowNumber, cellNumber).Formula = formularString;
            /*
            if (sheet1 != null)
            {
                //sheet1.SetValue(rowNumber, cellNumber, formularString);
                sheet1.Cells[rowNumber, cellNumber].Formula = formularString;
                //updateformular();
            }
            */
        }
        public void refreshAllSheet()
        {
            sheet1.Calculate();
            
            
        }
        public void renameSheet(string oldName, string newName)
        {
            if (sheet1!=null)
            {
                sheet1.Name = newName;
                xlsObj.Save();
            }
            else
            {
                sheet1 = templateWorkbook.Worksheets[oldName];
                sheet1.Name = newName;
                xlsObj.Save();
            }
        }
        public void save()
        {
            xlsObj.Save();
        }
        public void getLineChart()
        {
            ExcelScatterChart lineChart = sheet1.Drawings["Chart1"] as ExcelScatterChart;
            var serie11 = lineChart.Series[0];
            var serie22 = lineChart.Series[1];
            serie11.Series = sheet1.Cells[2, 2, 1024, 2].FullAddress;
            serie11.XSeries = sheet1.Cells[2, 1, 1024, 1].FullAddress;
            serie11.Header = "YDec";

            serie22.Series = sheet1.Cells[2, 3, 1024, 3].FullAddress;
            serie22.XSeries = sheet1.Cells[2, 1, 1024, 1].FullAddress;
            serie22.DataLabel.ShowLeaderLines = true;
            serie22.Header = "MAX";
            serie22.DataLabel.Position = eLabelPosition.Left;
            
            /*
            lineChart.Series.Delete(0);
            lineChart.Series.Delete(0);

            var serie1 = lineChart.Series.Add(sheet1.Cells[2,2,1024,2], sheet1.Cells[2,1,1024,1]);
            serie1.Header = "YDec";
            serie1.DataLabel.ShowValue = false;
            
            var serie2 = lineChart.Series.Add(sheet1.Cells[2, 3, 1024, 3], sheet1.Cells[2, 1, 1024, 1]);
            serie2.Header = "MAX";
            serie2.DataLabel.Position = eLabelPosition.Left;
            serie2.DataLabel.ShowValue = true;
            serie2.DataLabel.ShowLeaderLines = true;
            
            //serie2.DataLabel.ShowLeaderLines
            lineChart.SetPosition(2, 2, 6, 0);
            lineChart.SetSize(600, 400);
            */
        }
    }
    
    public  class fileNPath
    {
        public  string fileForDownload { get; set; }
        public  string pathToDownload { get; set;  }
    }
}