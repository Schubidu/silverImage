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
	public partial class Page : UserControl
	{
		WebPic webPic = null;
		string imageUrl = "../webimage.xml";
		public string ImageUrl { get { return this.imageUrl; } set { this.imageUrl = (value != null && value != String.Empty) ? value : imageUrl; } }
		public int Refresh { get { return 3; } }
		private int _refreshCount;
		private int RefreshCount
		{
			get { return _refreshCount; }
			set
			{
				this._refreshCount = value;
				this.progressBar.Value = (double)_refreshCount / (double)Refresh;
			}
		}
		int errors = 0;

		public Page()
		{
			InitializeComponent();
			this.Loaded += new RoutedEventHandler(Page_Loaded);
			Initparams init = new Initparams();
			string webcamPictureUrl = init.GetSingleParam("pictureUrl");
			this.ImageUrl = webcamPictureUrl;
			this.VisiblePlayButAndPlay();
			this.LayoutRoot.SizeChanged += new SizeChangedEventHandler(LayoutRoot_SizeChanged);
		}

		void LayoutRoot_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			if (webPic != null) this.LayoutUpdate();
		}

		void Page_LayoutUpdated(object sender, EventArgs e)
		{
		}

		System.Windows.Threading.DispatcherTimer myDispatcherTimer = new System.Windows.Threading.DispatcherTimer();


		void Page_Loaded(object sender, RoutedEventArgs e)
		{
			this.myDispatcherTimer.Interval = new TimeSpan(0, 0, 0, 1, 0); // 100 Milliseconds
			this.myDispatcherTimer.Tick += new EventHandler(myDispatcherTimer_Tick);
			this.myDispatcherTimer.Start();
			this.imageControl.ImageFailed += new EventHandler<ExceptionRoutedEventArgs>(imageControl_ImageFailed);
		}

		void imageControl_ImageFailed(object sender, ExceptionRoutedEventArgs e)
		{
			this.ImageLoad();
		}

		void myDispatcherTimer_Tick(object sender, EventArgs e)
		{
			this.RefreshCount++;
			if (this.RefreshCount >= this.Refresh)
				this.ImageLoad();
		}

		void ImageLoad()
		{

			this.myDispatcherTimer.Stop();
			this.progressBar.Visibility = Visibility.Visible;
			this.progressBar.Value = 0;
			WebClient downloader = new WebClient();
			downloader.DownloadProgressChanged += new DownloadProgressChangedEventHandler(downloader_DownloadProgressChanged);
			downloader.DownloadStringCompleted += new DownloadStringCompletedEventHandler(downloader_DownloadStringCompleted);
			string fileName = ImageUrl + "?" + this.GetQueryUID();
			downloader.DownloadStringAsync(new Uri(fileName, UriKind.RelativeOrAbsolute));
		}

		string GetQueryUID()
		{
			DateTime currentTime = DateTime.Now;
			int ticks = (int)currentTime.Ticks;
			ticks = (ticks < 0) ? ticks * -1 : ticks;
			Random rand1 = new Random();
			Random rand2 = new Random();
			string query = "t=" + ((long)rand1.Next(ticks) * (long)rand2.Next(ticks) + currentTime.Ticks).ToString() + DateTime.Now.Ticks;
			return ((this.webPic == null || this.webPic.XmlUID.Equals("")) ? "" : "uid=" + this.webPic.XmlUID + "&") + query;
		}

		void downloader_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
		{
			this.progressBar.Value = 1-e.ProgressPercentage;
		}

		void downloader_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
		{
			if (this.webPic != null) 
			{
				LastPics.Add(this.webPic);
				VisualStateManager.GoToState(this, "LastPicsState", true);
			}
			this.webPic = new WebPic(XDocument.Parse(e.Result));
			if (!this.webPic.HasError)
			{
				this.RefreshCount = 0;
				if (this.webPic.Source != null)
				{
					this.imageControl.Source = this.webPic.Source;
					this.currentTimeText.Text = this.webPic.DateAndTime;
					this.LayoutUpdate();
					this.myDispatcherTimer.Start();
				}
			}
			else
			{
				this.AddError();
			}
		}

		private void LayoutUpdate()
		{
			double ratio = this.LayoutRoot.ActualHeight / this.LayoutRoot.ActualWidth;
			this.webPic.Border = this.OuterBorder.BorderThickness;
			this.Outer.MaxHeight = webPic.Height;
			this.Outer.MaxWidth = webPic.Width;
			this.Outer.Width = Double.NaN;
			this.Outer.Height = Double.NaN;
			if (ratio < this.webPic.Ratio)
			{
				this.Outer.Width = this.LayoutRoot.ActualHeight / webPic.Ratio;
				this.Outer.Height = this.LayoutRoot.ActualHeight;
			}
			if (ratio > this.webPic.Ratio)
			{
				this.Outer.Height = this.LayoutRoot.ActualWidth * webPic.Ratio;
				this.Outer.Width = this.LayoutRoot.ActualWidth;
			}
		}

		private void AddError()
		{
			if (this.errors <= 2)
			{
				this.errors++;
				this.myDispatcherTimer.Start();
				//VisiblePlayButAndPlay();
			}
			else
			{
				//MessageBox.Show(ex.InnerException.ToString());
				this.VisibleStopButAndStop();
				this.errorText.Visibility = Visibility.Visible;
			}
		}

		private void VisiblePlayButAndPlay()
		{
			this.StopBut.Visibility = Visibility.Visible;
			this.PlayBut.Visibility = Visibility.Collapsed;
			this.errorText.Visibility = Visibility.Collapsed;
			this.ImageLoad();
		}

		private void VisibleStopButAndStop()
		{
			this.StopBut.Visibility = Visibility.Collapsed;
			this.PlayBut.Visibility = Visibility.Visible;
			this.myDispatcherTimer.Stop();
			this.errors = 0;
		}

		private void PlayBut_Click(object sender, RoutedEventArgs e)
		{
			this.VisiblePlayButAndPlay();
		}

		private void StopBut_Click(object sender, RoutedEventArgs e)
		{
			this.VisibleStopButAndStop();
		}
	}
}
