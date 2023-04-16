using Microsoft.Extensions.Logging;
using Microsoft.Web.WebView2.Core;
using rNascar23.Media.Models;
using rNascar23.Media.Ports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace rNascar23.Dialogs
{
    public partial class VideoPlayer : Form
    {
        #region fields

        private bool _isLoading = false;
        private readonly ILogger<VideoPlayer> _logger = null;
        private readonly IMediaRepository _mediaRepository = null;

        #endregion

        #region properties

        public int SeriesId { get; set; }

        private VideoChannel _selectedChannel = null;
        public VideoChannel SelectedChannel
        {
            get
            {
                return _selectedChannel;
            }
            set
            {
                _selectedChannel = value;

                btnPlay.Enabled = (_selectedChannel != null);
            }
        }

        #endregion

        #region ctor/load

        public VideoPlayer()
        {
            InitializeComponent();
        }

        public VideoPlayer(ILogger<VideoPlayer> logger, IMediaRepository mediaRepository)
        {
            InitializeComponent();

            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mediaRepository = mediaRepository ?? throw new ArgumentNullException(nameof(mediaRepository));

            LogInfoMessage("before InitializeBrowserAsync");
            InitializeBrowserAsync();
            LogInfoMessage("after InitializeBrowserAsync");
        }

        private async void VideoSelectionDialog_Load(object sender, EventArgs e)
        {
            try
            {
                _isLoading = true;

                webView.CoreWebView2InitializationCompleted += WebView_CoreWebView2InitializationCompleted;

                if ((webView == null) || (webView.CoreWebView2 == null))
                {
                    LogInfoMessage("not ready");
                }

                var videoConfiguration = await LoadVideoConfiguration(SeriesId);

                IList<VideoChannel> channels = new List<VideoChannel>();

                foreach (VideoComponent component in videoConfiguration.data)
                {
                    foreach (VideoChannel videoChannel in component.videos)
                    {
                        channels.Add(videoChannel);
                    }
                }

                DisplayVideoChannelList(channels);
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex, "Exception loading Video Selection dialog");
            }
            finally
            {
                _isLoading = false;
            }
        }

        #endregion

        #region private

        private async Task InitializeBrowserAsync(string url = null)
        {
            try
            {
                var userDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\rNascar23";
                var env = await CoreWebView2Environment.CreateAsync(null, userDataFolder);
                await webView.EnsureCoreWebView2Async(env);
                webView.Source = new UriBuilder(url ?? "google.com").Uri;
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex, "Exception in InitializeAsync");
            }
        }

        private void WebView_CoreWebView2InitializationCompleted(object sender, Microsoft.Web.WebView2.Core.CoreWebView2InitializationCompletedEventArgs e)
        {
            try
            {
                LogInfoMessage("WebView_CoreWebView2InitializationCompleted");
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex, "Exception in WebView_CoreWebView2InitializationCompleted");
            }
        }

        private async Task<VideoConfiguration> LoadVideoConfiguration(int seriesId)
        {
            return await _mediaRepository.GetVideoConfigurationAsync(seriesId);
        }

        private void DisplayVideoChannelList(IList<VideoChannel> channels)
        {
            cboChannels.DataSource = null;

            cboChannels.DisplayMember = "title";
            cboChannels.ValueMember = "stream1";

            cboChannels.DataSource = channels.ToList();

            cboChannels.SelectedIndex = -1;
        }

        private void PlayVideoFeed(VideoChannel videoChannel)
        {
            try
            {
                if (videoChannel == null)
                    return;

                var token = @"<#SOURCE#>";

                // https://dw9ptu32blt7h.cloudfront.net/IC01/master.m3u8
                var template = Properties.Resources.videoFeedTemplate;
                var html = template.Replace(token, videoChannel.stream1);

                webView.NavigateToString(html);

                System.Threading.Thread.Sleep(1000);

                webView.Visible = true;
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex, $"Exception in PlayVideoFeed: {videoChannel.stream1}");
            }
        }

        #endregion

        #region private [ event handlers ]

        private void cboChannels_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (_isLoading)
                    return;

                if (cboChannels.SelectedItem == null)
                    return;

                SelectedChannel = (VideoChannel)cboChannels.SelectedItem;
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex, "Exception selecting video channel");
            }
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            try
            {
                pnlSelection.Visible = false;

                this.Text = $"{SelectedChannel.title} In-Car Video";

                PlayVideoFeed(SelectedChannel);
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex, "Exception playing video channel");
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        #endregion

        #region private [ exception handlers ]

        private void ExceptionHandler(Exception ex, string message = "", bool logMessage = true)
        {
            MessageBox.Show(ex.Message);
            if (logMessage)
            {
                string errorMessage = String.IsNullOrEmpty(message) ? ex.Message : message;

                _logger.LogError(ex, errorMessage);
            }
        }

        private void LogInfoMessage(string message)
        {
            Console.WriteLine(message);
            _logger.LogInformation(message);
        }

        #endregion
    }
}
