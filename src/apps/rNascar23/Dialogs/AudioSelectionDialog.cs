using Microsoft.Extensions.Logging;
using Microsoft.Web.WebView2.WinForms;
using rNascar23.LoopData.Ports;
using rNascar23.Media.Models;
using rNascar23.Media.Ports;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace rNascar23.Dialogs
{
    public partial class AudioSelectionDialog : Form
    {
        #region fields

        private bool _isLoading = false;
        private readonly ILogger<AudioSelectionDialog> _logger = null;
        private readonly IMediaRepository _mediaRepository = null;

        #endregion

        #region properties

        public int SeriesId { get; set; }

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

        public AudioSelectionDialog()
        {
            InitializeComponent();
        }

        public AudioSelectionDialog(ILogger<AudioSelectionDialog> logger, IMediaRepository mediaRepository)
        {
            InitializeComponent();

            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mediaRepository = mediaRepository ?? throw new ArgumentNullException(nameof(mediaRepository));
        }

        private async void AudioSelectionDialog_Load(object sender, EventArgs e)
        {
            try
            {
                _isLoading = true;

                var audioConfiguration = await LoadAudioConfiguration(SeriesId);

                DisplayAudioChannelList(audioConfiguration.audio_config);
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

        private async Task<AudioConfiguration> LoadAudioConfiguration(int seriesId)
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
            if (audioChannel == null)
                return;

            var token = @"<#SOURCE#>";

            var template = Properties.Resources.audioFeedTemplate2;

            var html = template.Replace(token, audioChannel.source);

            Console.WriteLine($"audioFeed.Name: {audioChannel.driver_name}; audioFeed.Source: {audioChannel.source}");

            webView.NavigateToString(html);

            System.Threading.Thread.Sleep(1000);

            lblInstructions.Visible = true;

            webView.Visible = true;
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

        private void ExceptionHandler(Exception ex, string message = "", bool logMessage = false)
        {
            MessageBox.Show(ex.Message);
            if (logMessage)
            {
                string errorMessage = String.IsNullOrEmpty(message) ? ex.Message : message;

                _logger.LogError(ex, errorMessage);
            }
        }

        #endregion
    }
}
