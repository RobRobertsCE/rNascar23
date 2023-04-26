using Microsoft.Extensions.Logging;
using Microsoft.Web.WebView2.Core;
using rNascar23.Sdk.Common;
using rNascar23.Sdk.Media.Models;
using rNascar23.Sdk.Media.Ports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace rNascar23.Dialogs
{
    public partial class AudioPlayer : Form
    {
        #region fields

        private bool _isLoading = false;
        private readonly ILogger<AudioPlayer> _logger = null;
        private readonly IMediaRepository _mediaRepository = null;

        #endregion

        #region properties

        public SeriesTypes SeriesId { get; set; }

        private AudioChannel _selectedChannel = null;
        public AudioChannel SelectedChannel
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

        public AudioPlayer()
        {
            InitializeComponent();
        }

        public AudioPlayer(ILogger<AudioPlayer> logger, IMediaRepository mediaRepository)
        {
            InitializeComponent();

            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mediaRepository = mediaRepository ?? throw new ArgumentNullException(nameof(mediaRepository));

            LogInfoMessage("before InitializeBrowserAsync");
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            InitializeBrowserAsync();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            LogInfoMessage("after InitializeBrowserAsync");
        }

        private async void AudioSelectionDialog_Load(object sender, EventArgs e)
        {
            try
            {
                _isLoading = true;

                webView.CoreWebView2InitializationCompleted += WebView_CoreWebView2InitializationCompleted;

                if ((webView == null) || (webView.CoreWebView2 == null))
                {
                    LogInfoMessage("not ready");
                }

                var audioConfiguration = await LoadAudioConfiguration(SeriesId);

                if (audioConfiguration == null || audioConfiguration.AudioChannels == null)
                {
                    MessageBox.Show("No audio channels are available right now");
                    return;
                }

                DisplayAudioChannelList(audioConfiguration.AudioChannels);
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex, "Exception loading Audio Selection dialog");
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

        private async Task<AudioConfiguration> LoadAudioConfiguration(SeriesTypes seriesId)
        {
            return await _mediaRepository.GetAudioConfigurationAsync(seriesId);
        }

        private void DisplayAudioChannelList(IList<AudioChannel> channels)
        {
            cboChannels.DataSource = null;

            cboChannels.DisplayMember = "driver_name";
            cboChannels.ValueMember = "stream_number";

            cboChannels.DataSource = channels.ToList();

            cboChannels.SelectedIndex = -1;
        }

        private void PlayAudioFeed(AudioChannel audioChannel)
        {
            try
            {
                if (audioChannel == null)
                    return;

                var token = @"<#SOURCE#>";

                var template = Properties.Resources.audioFeedTemplate;

                var html = template.Replace(token, audioChannel.Source);

                LogInfoMessage($"audioFeed.Name: {audioChannel.DriverName}; audioFeed.Source: {audioChannel.Source}");

                webView.NavigateToString(html);

                System.Threading.Thread.Sleep(1000);

                lblInstructions.Visible = true;

                webView.Visible = true;
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex, $"Exception in PlayAudioFeed: {audioChannel.Source}");
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

                SelectedChannel = (AudioChannel)cboChannels.SelectedItem;
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex, "Exception selecting audio channel");
            }
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            try
            {
                PlayAudioFeed(SelectedChannel);
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex, "Exception playing audio channel");
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
            _logger.LogInformation(message);
        }

        #endregion
    }
}
