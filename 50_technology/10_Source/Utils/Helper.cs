using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Media.Imaging;
using System.IO;

namespace Utils
{
    public static class Helper
    {
         public static DateTime GetDateTime(string str) {
            try
            {
                return DateTime.Parse(str);
            }
            catch {
                return DateTime.Parse("1-1-1");
            }
         }
         public static BitmapImage GetBase64Image(string str) {
             try
             {
                 byte[] imageData = Convert.FromBase64String(str);
                 MemoryStream ms = new MemoryStream(imageData);
                 BitmapImage tempImage = new BitmapImage();
                 tempImage.SetSource(ms);
                 ms.Dispose();
                 return tempImage;
             }
             catch {
                 return null;
             }
         }

    }
}
