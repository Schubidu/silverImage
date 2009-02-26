using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace webcam.userControls
{
	public partial class LastPicsControl : UserControl
	{
		public LastPicsControl()
		{
			InitializeComponent();
			this.Loaded += new RoutedEventHandler(LastPicsControl_Loaded);
		}

		void LastPicsControl_Loaded(object sender, RoutedEventArgs e)
		{
			
		}
		public void Add(WebPic webPic) {
			LastPic lastPic = new LastPic();
			lastPic.webPic = webPic;
			LastPicsStack.Children.Add(lastPic);
		}
	}
}
