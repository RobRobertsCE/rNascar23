using Microsoft.Extensions.Logging;
using rNascar23.Replay;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace rNascar23.Dialogs
{
    public partial class ReplaySelectionDialog : Form
    {
        #region fields

        private IList<EventReplay> _replays = null;
        private readonly ILogger<ReplaySelectionDialog> _logger = null;

        #endregion

        #region properties

        private EventReplay _selectedReplay = null;
        public EventReplay SelectedReplay
        {
            get
            {
                return _selectedReplay;
            }
            private set
            {
                _selectedReplay = value;

                btnSelect.Enabled = (_selectedReplay != null);
            }
        }

        public IList<int> SelectedFrames { get; set; } = new List<int>();

        public string ReplayDirectory { get; set; }

        #endregion

        #region ctor/load

        public ReplaySelectionDialog()
        {
            InitializeComponent();
        }

        public ReplaySelectionDialog(ILogger<ReplaySelectionDialog> logger)
        {
            InitializeComponent();

            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        private void ReplaySelectionDialog_Load(object sender, EventArgs e)
        {
            try
            {
                _replays = EventReplayFactory.LoadEventReplays(ReplayDirectory);

                DisplayEventReplays(_replays);
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex, "Exception loading ReplaySelectionDialog");
            }
        }

        #endregion

        #region private [event handlers]

        private void lvEvents_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                SelectedReplay = null;

                if (lvEvents.SelectedItems.Count == 0)
                    return;

                var selectedItem = lvEvents.SelectedItems[0];

                var eventReplay = selectedItem.Tag as EventReplay;

                if (eventReplay != null)
                {
                    SelectedReplay = eventReplay;
                    DisplayEventFrames(eventReplay.Frames);
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex, "Exception selecting event");
            }
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            if (lvFrames.SelectedIndices.Count > 1)
            {
                IList<EventReplayFrame> selectedFrames = new List<EventReplayFrame>();

                for (int i = 0; i < lvFrames.SelectedIndices.Count; i++)
                {
                    selectedFrames.Add(SelectedReplay.Frames[lvFrames.SelectedIndices[i]]);
                }

                SelectedReplay.Frames = selectedFrames;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void lvEvents_DoubleClick(object sender, EventArgs e)
        {
            if (SelectedReplay != null)
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        #endregion

        #region private

        private void DisplayEventReplays(IList<EventReplay> replays)
        {
            try
            {
                lvEvents.BeginUpdate();

                lvEvents.Items.Clear();

                foreach (var replay in replays.OrderBy(r => r.EventDate))
                {
                    var lvi = new ListViewItem(replay.EventDate.ToString("M-d-yyyy"));
                    lvi.SubItems.Add(replay.Series);
                    lvi.SubItems.Add(replay.EventType);
                    lvi.SubItems.Add(replay.TrackName);
                    lvi.SubItems.Add(replay.EventName);
                    lvi.SubItems.Add(replay.Frames.Count.ToString());

                    lvi.Tag = replay;

                    lvEvents.Items.Add(lvi);
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                lvEvents.EndUpdate();
            }
        }

        private void DisplayEventFrames(IList<EventReplayFrame> frames)
        {
            try
            {
                lvFrames.BeginUpdate();

                lvFrames.Items.Clear();

                foreach (var frame in frames.OrderBy(f => f.Index))
                {
                    var lvi = new ListViewItem(frame.Index.ToString());
                    lvi.SubItems.Add(frame.Timestamp.ToString("HH:mm.ss"));
                    lvi.SubItems.Add(Path.GetFileName(frame.FileName));

                    lvi.Tag = frame;

                    lvFrames.Items.Add(lvi);
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                lvFrames.EndUpdate();
            }
        }

        private void ExceptionHandler(Exception ex, string message = "")
        {
            string errorMessage = String.IsNullOrEmpty(message) ? ex.Message : message;

            _logger?.LogError(ex, errorMessage);
        }

        #endregion
    }
}
