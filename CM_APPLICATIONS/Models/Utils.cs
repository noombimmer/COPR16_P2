using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using ZXing;
using ZXing.Common;

namespace CM_APPLICATIONS.Models
{
    public static class Utils
    {
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
        public static Image DrawText(String text, Font font, Color textColor, Color backColor)
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