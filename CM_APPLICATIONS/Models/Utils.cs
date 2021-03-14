using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using ZXing;
using ZXing.Common;
using Microsoft.Office.Interop.Excel;

namespace CM_APPLICATIONS.Models
{
    interface IExcel
    {
        void Create();
        void SetData(int i, int j, string data);
        void SaveAs();
        void Release();
    }

    public class CBExcel : IExcel
    {
        Application xlApp;
        Workbook xlWorkBook;
        Worksheet xlWorkSheet;
        object misValue = System.Reflection.Missing.Value;
        const string ChartStart = "A1";
        string m_ChartEnd;
        int m_MaxI;
        int m_MaxJ;
        public CBExcel()
        {
            m_ChartEnd = "A1";
            m_MaxI = -1;
            m_MaxJ = -1;
        }
        public void open(string fileName)
        {
            if (xlWorkBook != null)
            {
                xlWorkBook.Close();
            }
            if (xlApp == null)
            {
                xlApp = new Microsoft.Office.Interop.Excel.Application();
            }
            xlWorkBook = xlApp.Workbooks.OpenXML("D:\\SRC_01\\NoteXLS.xlsx");
            xlApp.DisplayAlerts = false;
        }
        public void close()
        {
            if (xlWorkBook != null)
            {
                this.SaveAs();
                xlWorkBook.Close();
            }
        }
        public void SetData(int i, int j, string data)
        {
            xlWorkSheet.Cells[i, j] = data;
            CheckChartEnd(i, j);
        }

        private void CheckChartEnd(int i, int j)
        {
            if (m_MaxI <= i)
                m_MaxI = i;
            if (m_MaxJ <= j)
                m_MaxJ = j;
            const int a = 0x41;
            int word = a + j - 1;
            m_ChartEnd = string.Format("{0}{1}", Convert.ToChar(word), m_MaxI);
        }

        public void SetChart(XlChartType type)
        {
            Range chartRange;

            ChartObjects xlCharts = (ChartObjects)xlWorkSheet.ChartObjects(Type.Missing);
            ChartObject myChart = (ChartObject)xlCharts.Add(500, 80, 350, 350);
            Chart chartPage = myChart.Chart;

            chartRange = xlWorkSheet.get_Range(ChartStart, m_ChartEnd);
            chartPage.SetSourceData(chartRange, misValue);
            chartPage.ChartType = type;
        }

        public void SetChart(string start, string end, XlChartType type)
        {
            Range chartRange;

            ChartObjects xlCharts = (ChartObjects)xlWorkSheet.ChartObjects(Type.Missing);
            ChartObject myChart = (ChartObject)xlCharts.Add(500, 80, 350, 350);
            Chart chartPage = myChart.Chart;

            chartRange = xlWorkSheet.get_Range(start, end);
            chartPage.SetSourceData(chartRange, misValue);
            chartPage.ChartType = type;
        }

        private void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch
            {
                obj = null;
            }
            finally
            {
                GC.Collect();
            }
        }

        public void Create()
        {
            xlApp = new Application();
            xlWorkBook = xlApp.Workbooks.Add(misValue);
            xlWorkSheet = (Worksheet)xlWorkBook.Worksheets.get_Item(1);
        }

        public void SaveAs()
        {
            SetChart(XlChartType.xlLine);
            xlWorkBook.Close(true, misValue, misValue);
            xlApp.Quit();
        }

        public void Release()
        {
            releaseObject(xlWorkSheet);
            releaseObject(xlWorkBook);
            releaseObject(xlApp);
        }
    }
    public static class Utils
    {
        public static string getValueFromReader(DbDataReader reader,string FieldName,int rowNum = 0)
        {
            string strReturn = "";
            int rows = 0;
            int colIndex = -1;
            for (int n =0;n<=reader.FieldCount-1;n++)
            {
                if (reader.GetName(n) == FieldName)
                {
                    colIndex = n;
                    break;
                }
            }
            if (colIndex == -1)
            {
                strReturn = null;
                return strReturn;
            }
            if(reader == null)
            {
                strReturn = null;
                return strReturn;
            }
            if (reader.IsClosed)
            {
                strReturn = null;
                return strReturn;
            }
            while (reader.Read())
            {
                if (rows == rowNum) {
                    strReturn = reader.GetString(colIndex);
                }
                rows++;
            }
            return strReturn;
        }
        public static IEnumerable<Dictionary<string, object>> Serialize(SqlDataReader reader)
        {
            var results = new List<Dictionary<string, object>>();
            var cols = new List<string>();
            for (var i = 0; i < reader.FieldCount; i++)
                cols.Add(reader.GetName(i));

            while (reader.Read())
                results.Add(SerializeRow(cols, reader));

            return results;
        }
        private static Dictionary<string, object> SerializeRow(IEnumerable<string> cols,
                                                SqlDataReader reader)
        {
            var result = new Dictionary<string, object>();
            foreach (var col in cols)
                result.Add(col, reader[col]);
            return result;
        }
        public static Image genQR(string text)
        {
            /*
             Format = BarcodeFormat.CODE_39,
             Options = new EncodingOptions
             {
                 Height = 200,
                 Width = 600
             }
             */

            var QCwriter = new BarcodeWriter();
            QCwriter.Format = BarcodeFormat.QR_CODE;
            EncodingOptions opt = new EncodingOptions();
            opt.Height = 200;
            opt.Width = 200;
            opt.PureBarcode = true;
            opt.Margin = 0;
            opt.GS1Format = true;
            QCwriter.Options = opt;
            var result = QCwriter.Write(text);
            var img = new Bitmap(result);
            return img;
        }
        public static bool IsNullableType(Type valueType)
        {
            return (valueType.IsGenericType &&
                valueType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)));
        }
        public static Image DrawText(String text, System.Drawing.Font font, Color textColor, Color backColor)
        {
            //first, create a dummy bitmap just to get a graphics object  
            Image img = new Bitmap(1, 1);
            Graphics drawing = Graphics.FromImage(img);

            //measure the string to see how big the image needs to be  
            SizeF textSize = drawing.MeasureString(text, font);

            //free up the dummy image and old graphics object  
            img.Dispose();
            drawing.Dispose();

            //create a new image of the right size  
            img = new Bitmap((int)textSize.Width, (int)textSize.Height);

            drawing = Graphics.FromImage(img);

            //paint the background  
            drawing.Clear(backColor);

            //create a brush for the text  
            Brush textBrush = new SolidBrush(textColor);

            drawing.DrawString(text, font, textBrush, 0, 0);

            drawing.Save();

            textBrush.Dispose();
            drawing.Dispose();

            return img;

        }
        public static byte[] ImageToByteArray(System.Drawing.Image imageIn)
        {
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            return ms.ToArray();
        }
    }
    public class qrFile
    {
        public string fileName { set; get; }
        public string Url { set; get; }
        public string TextValue { set; get; }
    }
    public class qrPrint
    {
        public string copnoa { set; get; }
        public string copnob { set; get; }
        public string mode { set; get; }
        public string pdf { set; get; }
    }
}