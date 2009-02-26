using System;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Xml.Linq;
using Utils;

namespace webcam
{
	public class WebPic
	{
		public WebPic(XDocument xd)
		{
			this.HasError = !(Int32.Parse(xd.Descendants("errorcode").First().Value) >= 0);
			this.XmlUID = xd.Descendants("uid").First().Value;
			this.Source = Helper.GetBase64Image(xd.Descendants("data").First().Value);
			this.DateAndTime = Helper.GetDateTime(xd.Descendants("createtime").First().Value).ToString();
			this.Width = Double.Parse(xd.Descendants("size").First().Descendants("width").First().Value);
			this.Height = Double.Parse(xd.Descendants("size").First().Descendants("height").First().Value);
		}

		public bool HasError { get; set; }

		public string XmlUID { get; set; }

		public BitmapImage Source { get; set; }

		public string DateAndTime { get; set; }

		public double Width { get; set; }

		public double Height { get; set; }

		private double ActualWidth { get { return this.Width + Border.Left + Border.Right; } }

		private double ActualHeight { get { return this.Height + Border.Top + Border.Bottom; } }

		public double Ratio { get { return this.ActualHeight/this.ActualWidth;} }

		public Thickness Border
		{
			get;
			set;
		}
	}
}
