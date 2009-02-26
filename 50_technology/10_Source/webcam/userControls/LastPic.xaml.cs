using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace webcam.userControls
{
	public partial class LastPic : UserControl
	{
		public LastPic()
		{
			InitializeComponent();
		}
		private WebPic _webPic = null;
		public WebPic webPic { 
			get { return this._webPic; } 
			set { 
				this._webPic = value;
				
				this.Picture.Source = value.Source;
				this.Height = 52;
				this.Width = this.Height / value.Ratio;
				Thickness t = new Thickness();
				t.Left = 5;
				this.Margin = t;
			}
		}
	}
}
