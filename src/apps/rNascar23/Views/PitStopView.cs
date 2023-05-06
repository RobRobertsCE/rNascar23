using Microsoft.Extensions.DependencyInjection;
using rNascar23.Sdk.Common;
using rNascar23.CustomViews;
using rNascar23.Sdk.Flags.Ports;
using rNascar23.Sdk.LiveFeeds.Ports;
using rNascar23.Sdk.Flags.Models;
using rNascar23.Sdk.PitStops.Ports;
using rNascar23.Settings;
using rNascar23.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using PitStop = rNascar23.Sdk.PitStops.Models.PitStop;
using rNascar23.Sdk.LiveFeeds.Models;

namespace rNascar23.Views
{
    public partial class PitStopView : UserControl, IApiDataView<PitStop>
    {
        #region consts

        private const string RightSideTireChange = "TWO_WHEEL_CHANGE_RIGHT";
        private const string LeftSideTireChange = "TWO_WHEEL_CHANGE_LEFT";
        private const string FourTireChange = "FOUR_WHEEL_CHANGE";
        private const string NoTireChange = "OTHER";

        #endregion

        #region fields

        private readonly IFlagStateRepository _flagStateRepository = null;
        private readonly IPitStopsRepository _pitStopsRepository = null;
        private readonly ILiveFeedRepository _liveFeedRepository = null;
        private Color _alternateRowBackColor = Color.Gainsboro;
        IList<PitStopAverages> _averages = new List<PitStopAverages>();

        #endregion

        #region properties

        public ApiSources ApiSource => ApiSources.PitStops;
        public string Title => "Pit Stops";

        private IList<PitStop> _data = new List<PitStop>();
        public IList<PitStop> Data
        {
            get
            {
                return _data;
            }
            set
            {
                _data = value;
            }
        }
        public string Description { get; set; }

        private int? _startLap = null;
        public int? StartLap
        {
            get
            {
                return _startLap;
            }
            set
            {
                _startLap = value;
            }
        }

        private int? _endLap = null;
        public int? EndLap
        {
            get
            {
                return _endLap;
            }
            set
            {
                _endLap = value;
            }
        }

        private int? _currentLap;
        public int? CurrentLap
        {
            get
            {
                return _currentLap;
            }
            set
            {
                _currentLap = value;
            }
        }

        public SeriesTypes SeriesId { get; set; }
        public int RaceId { get; set; }

        #endregion

        #region ctor/load

        public PitStopView()
            : this(1, null, null)
        {

        }
        public PitStopView(int currentLap)
            : this(1, currentLap, currentLap)
        {

        }

        public PitStopView(int? startLap, int? endLap, int? currentLap)
        {
            InitializeComponent();

            SetTheme();

            _startLap = startLap;
            _endLap = endLap;
            _currentLap = currentLap;

            _liveFeedRepository = Program.Services.GetRequiredService<ILiveFeedRepository>();
            _flagStateRepository = Program.Services.GetRequiredService<IFlagStateRepository>();
            _pitStopsRepository = Program.Services.GetRequiredService<IPitStopsRepository>();
        }

        private async void PitStopView_Load(object sender, EventArgs e)
        {
            await UpdateGridsAsync();

            await SetupRangeAsync();

            PopulateStartLapList();

            PopulateEndLapList();

            PopulateDriversListAsync();

            await UpdateDisplayAsync();
        }

        #endregion

        #region private

        private void SetTheme()
        {
            var settings = UserSettingsService.LoadUserSettings();

            Color backColor;
            Color foreColor;

            if (settings.UseDarkTheme)
            {
                backColor = Color.Black;
                foreColor = Color.Gainsboro;
                _alternateRowBackColor = Color.FromArgb(24, 24, 24);
            }
            else
            {
                backColor = Color.White;
                foreColor = Color.Black;
                _alternateRowBackColor = Color.Gainsboro;
            }

            flpPitStops.BackColor = backColor;
            pnlTop.BackColor = backColor;
            pnlDriverPitStops.BackColor = backColor;
            pnlPitStopAverages.BackColor = backColor;
            pnlBottomGrids.BackColor = backColor;
            pnlTopGrids.BackColor = backColor;

            this.BackColor = backColor;

            flpPitStops.ForeColor = foreColor;
            pnlTop.ForeColor = foreColor;
            pnlDriverPitStops.ForeColor = foreColor;
            pnlPitStopAverages.ForeColor = foreColor;
            pnlBottomGrids.ForeColor = foreColor;
            pnlTopGrids.ForeColor = foreColor;


            btnUpdateByLaps.ForeColor = Color.Black;
            btnUpdateByCaution.ForeColor = Color.Black;
            btnUpdateByDriver.ForeColor = Color.Black;

            SetListViewTheme(lvDriverPitStops, backColor, foreColor);
            SetListViewTheme(lvTotalGainLoss, backColor, foreColor);
            SetListViewTheme(lvAveragePitTime, backColor, foreColor);
            SetListViewTheme(lvAverageTotalTime, backColor, foreColor);
            SetListViewTheme(lvInOutTime, backColor, foreColor);
            SetListViewTheme(lvAverageGreenPitTime, backColor, foreColor);
            SetListViewTheme(lvAverageGreenTotalTime, backColor, foreColor);
            SetListViewTheme(lvGreenOutTime, backColor, foreColor);
        }

        private void SetListViewTheme(ListView listView, Color backColor, Color foreColor)
        {
            listView.BackColor = backColor;
            listView.ForeColor = foreColor;
        }

        private async Task SetupRangeAsync()
        {
            var flagStates = await _flagStateRepository.GetFlagStatesAsync();

            if (flagStates.Count() == 0)
                return;

            var orderedFlagStates = flagStates.
                Where(f => f.State == FlagColors.Green || f.State == FlagColors.Yellow).
                OrderByDescending(f => f.LapNumber);

            var currentFlagState = orderedFlagStates.FirstOrDefault();

            if (currentFlagState != null)
            {
                if (currentFlagState.State == FlagColors.Green)
                {
                    var endOfLastCaution = currentFlagState;

                    var beginningOfLastCaution = orderedFlagStates.
                        Where(f => f.LapNumber < endOfLastCaution.LapNumber).
                        FirstOrDefault(f => f.State == FlagColors.Yellow);

                    _startLap = beginningOfLastCaution == null ? 0 : beginningOfLastCaution.LapNumber;

                    _endLap = endOfLastCaution.LapNumber;
                }
                else if (currentFlagState.State == FlagColors.Yellow)
                {
                    var beginningOfLastCaution = orderedFlagStates.FirstOrDefault(f => f.State == FlagColors.Green);

                    _startLap = beginningOfLastCaution.LapNumber + 1;

                    _endLap = currentFlagState.LapNumber;
                }
            }

            PopulateCautionList(flagStates);
        }

        private void PopulateStartLapList()
        {
            cboStartLap.Items.Clear();

            for (int i = 1; i < _currentLap; i++)
            {
                cboStartLap.Items.Add(i);
            }

            if (_startLap.HasValue)
                cboStartLap.SelectedItem = _startLap.Value;
        }

        private void PopulateEndLapList()
        {
            cboEndLap.Items.Clear();

            for (int i = 1; i < _currentLap; i++)
            {
                cboEndLap.Items.Add(i);
            }

            if (_endLap.HasValue)
                cboEndLap.SelectedItem = _endLap.Value;
        }

        private void PopulateCautionList(IEnumerable<FlagState> flagStates)
        {
            var previouslySelectedIndex = cboCautions.SelectedItem != null ? (int?)cboCautions.SelectedIndex : null;

            cboCautions.DataSource = null;

            IList<CautionViewModel> cautionViewModels = new List<CautionViewModel>();
            CautionViewModel cautionViewModel = null;

            foreach (FlagState flagState in flagStates)
            {
                if (flagState.State == FlagColors.Green && cautionViewModel != null)
                {
                    // end of caution
                    cautionViewModel.EndLap = flagState.LapNumber;
                    cautionViewModel.CautionNumber = cautionViewModels.Count + 1;

                    cautionViewModels.Add(cautionViewModel);

                    cautionViewModel = null;
                }
                else if (flagState.State == FlagColors.Yellow)
                {
                    // start of caution
                    cautionViewModel = new CautionViewModel()
                    {
                        StartLap = flagState.LapNumber,
                        Comment = flagState.Comment
                    };
                }
            }

            cboCautions.DataSource = cautionViewModels.OrderBy(c => c.CautionNumber).ToList();

            if (previouslySelectedIndex.HasValue)
                cboCautions.SelectedIndex = previouslySelectedIndex.Value;
            else
                cboCautions.SelectedIndex = cautionViewModels.Count - 1;
        }

        private async void PopulateDriversListAsync()
        {
            IList<DriverViewModel> drivers = new List<DriverViewModel>();

            cboDrivers.DataSource = null;

            var liveFeed = await _liveFeedRepository.GetLiveFeedAsync();

            foreach (Vehicle vehicle in liveFeed.Vehicles)
            {
                var viewModel = new DriverViewModel()
                {
                    CarNumber = vehicle.VehicleNumber,
                    Name = vehicle.Driver.FullName
                };

                drivers.Add(viewModel);
            }

            cboDrivers.DisplayMember = "Name";
            cboDrivers.ValueMember = "CarNumber";

            cboDrivers.DataSource = drivers.OrderBy(d => d.Name).ToList();

            cboDrivers.SelectedIndex = -1;
        }

        private void SetDataSource<T>(IList<T> values)
        {
            var viewModels = BuildViewModels((IList<PitStop>)values);

            flpPitStops.Controls.Clear();

            for (int i = 0; i < viewModels.Count; i++)
            {
                var viewModel = viewModels[i];
                DriverPitStopView view = null;

                if (flpPitStops.Controls.OfType<DriverPitStopView>().Count() >= i + 1)
                {
                    // control exists
                    view = flpPitStops.Controls.OfType<DriverPitStopView>().ToList()[i];
                }
                else
                {
                    view = new DriverPitStopView();
                    flpPitStops.Controls.Add(view);

                    view.ViewSelected += View_ViewSelected;
                }

                if (flpPitStops.Controls.OfType<DriverPitStopView>().Count() == 1)
                {
                    view = flpPitStops.Controls.OfType<DriverPitStopView>().ToList()[0];
                    flpPitStops.Width = view.Width + 28;
                }

                view.ViewModel = viewModel;
            }

            // Auto-select first pit stop, if any
            if (flpPitStops.Controls.OfType<DriverPitStopView>().Count() >= 1)
            {
                var view = flpPitStops.Controls.OfType<DriverPitStopView>().ToList()[0];

                view.Selected = true;

                View_ViewSelected(view, EventArgs.Empty);
            }
        }

        private IList<DriverPitStopViewModel> BuildViewModels(IList<PitStop> pitStops)
        {
            IList<DriverPitStopViewModel> driverPitStops = new List<DriverPitStopViewModel>();

            foreach (var pitStop in pitStops.OrderBy(p => p.PitOutRank))
            {
                var viewModel = new DriverPitStopViewModel()
                {
                    CarNumber = pitStop.VehicleNumber,
                    DriverName = pitStop.DriverName,
                    PitOnLap = pitStop.LapCount,
                    PitStopTime = pitStop.TotalDuration,
                    PositionDelta = pitStop.PositionsGainedLost,
                    PositionIn = pitStop.PitInRank,
                    PositionOut = pitStop.PitOutRank,
                    RunningPosition = pitStop.PitOutRank,
                    Changes = pitStop.PitStopType == NoTireChange ? PitStopChanges.Other :
                        pitStop.PitStopType == FourTireChange ? PitStopChanges.FourTires :
                        pitStop.PitStopType == LeftSideTireChange ? PitStopChanges.LeftSide :
                        pitStop.PitStopType == RightSideTireChange ? PitStopChanges.RightSide :
                        PitStopChanges.Other
                };

                driverPitStops.Add(viewModel);
            }

            return driverPitStops;
        }

        private async void View_ViewSelected(object sender, EventArgs e)
        {
            var selectedView = sender as DriverPitStopView;

            var selectedViewModel = selectedView.ViewModel;

            var driverPitStops = await GetAllPitStopsByDriverAsync(selectedViewModel.CarNumber);

            DisplayDriverPitStops(driverPitStops, selectedViewModel.CarNumber, selectedViewModel.DriverName);

            foreach (DriverPitStopView view in flpPitStops.Controls.OfType<DriverPitStopView>())
            {
                view.Selected = false;
            }

            selectedView.Selected = true;
        }

        private async void btnUpdate_Click(object sender, EventArgs e)
        {
            cboCautions.SelectedIndex = -1;
            cboDrivers.SelectedIndex = -1;

            StartLap = (int)cboStartLap.SelectedItem;

            EndLap = (int)cboEndLap.SelectedItem;

            await UpdateDisplayAsync();
        }

        private async Task UpdateDisplayAsync()
        {
            var pitStops = await _pitStopsRepository.GetPitStopsInRangeAsync(SeriesId, RaceId, StartLap, EndLap);

            Data = pitStops.ToList();

            SetDataSource(Data);
        }

        private async Task UpdateDisplayByDriverAsync(string carNumber, string driverName)
        {
            var driverPitStopData = await GetAllPitStopsByDriverAsync(carNumber);

            DisplayDriverPitStops(driverPitStopData, carNumber, driverName);
        }

        private async void btnUpdateByCaution_Click(object sender, EventArgs e)
        {
            if (cboCautions.SelectedItem == null)
                return;

            var selectedCaution = cboCautions.SelectedItem as CautionViewModel;

            cboStartLap.SelectedItem = selectedCaution.StartLap;
            cboEndLap.SelectedItem = selectedCaution.EndLap;

            cboDrivers.SelectedIndex = -1;

            StartLap = selectedCaution.StartLap;
            EndLap = selectedCaution.EndLap;

            await UpdateDisplayAsync();
        }

        private async void btnUpdateByDriver_Click(object sender, EventArgs e)
        {
            if (cboDrivers.SelectedItem == null)
                return;

            var selectedDriver = cboDrivers.SelectedItem as DriverViewModel;

            cboStartLap.SelectedIndex = -1;
            cboEndLap.SelectedIndex = -1;
            cboCautions.SelectedIndex = -1;

            StartLap = null;
            EndLap = null;

            flpPitStops.Controls.Clear();

            await UpdateDisplayByDriverAsync(selectedDriver.CarNumber, selectedDriver.Name);
        }

        private async Task<IEnumerable<PitStop>> GetAllPitStopsByDriverAsync(string carNumber)
        {
            return await _pitStopsRepository.GetPitStopsInRangeAsync(SeriesId, RaceId, null, null, carNumber);
        }

        private void DisplayDriverPitStops(IEnumerable<PitStop> pitStops, string carNumber, string driverName)
        {
            try
            {
                lblDriverPitStopsTitle.Text = $"Driver Pit Stops - {carNumber} {driverName}";
                lvDriverPitStops.BeginUpdate();

                lvDriverPitStops.Items.Clear();

                lblNumStops.Text = String.Empty;
                lblAvgGainLoss.Text = String.Empty;
                lblAvgPitTime.Text = String.Empty;
                lblAvgTotalTime.Text = String.Empty;
                lblAvgInOut.Text = String.Empty;

                lblAvgGainLossRank.Text = String.Empty;
                lblAvgPitTimeRank.Text = String.Empty;
                lblAvgTotalTimeRank.Text = String.Empty;
                lblAvgInOutRank.Text = String.Empty;

                int i = 0;

                foreach (var pitStop in pitStops)
                {
                    var lvi = new ListViewItem(pitStop.LapCount.ToString())
                    {
                        UseItemStyleForSubItems = false
                    };

                    Color flagColor = pitStop.PitInFlagStatus == 1 ? Color.Green :
                      pitStop.PitInFlagStatus == 2 ? Color.Gold :
                      lvi.BackColor;

                    var flagSubItem = new ListViewItem.ListViewSubItem(lvi, String.Empty, lvi.ForeColor, flagColor, lvi.Font);

                    lvi.SubItems.Add(flagSubItem);

                    var tireChanges = pitStop.PitStopType == NoTireChange ? String.Empty :
                        pitStop.PitStopType == FourTireChange ? "4" :
                        pitStop.PitStopType == LeftSideTireChange ? "Left" :
                        pitStop.PitStopType == RightSideTireChange ? "Right" :
                        String.Empty;

                    lvi.SubItems.Add(tireChanges);

                    lvi.SubItems.Add(pitStop.TotalDuration.ToString("N2"));
                    lvi.SubItems.Add(pitStop.PitStopDuration.ToString("N2"));
                    lvi.SubItems.Add(pitStop.PitInRank.ToString());
                    lvi.SubItems.Add(pitStop.PitOutRank.ToString());

                    Color gainLossColor = pitStop.PositionsGainedLost < 0 ? Color.Red :
                        pitStop.PositionsGainedLost > 0 ? Color.Green :
                        Color.Black;

                    var gainLossSubItem = new ListViewItem.ListViewSubItem(lvi, pitStop.PositionsGainedLost.ToString(), gainLossColor, lvDriverPitStops.BackColor, lvDriverPitStops.Font);

                    lvi.SubItems.Add(gainLossSubItem);

                    lvi.Tag = pitStop;

                    if (i % 2 == 0)
                    {
                        lvi.BackColor = _alternateRowBackColor;

                        for (int x = 2; x < lvi.SubItems.Count; x++)
                        {
                            lvi.SubItems[x].BackColor = _alternateRowBackColor;
                        }
                    }

                    lvDriverPitStops.Items.Add(lvi);

                    i++;
                }

                if (pitStops.Count() > 0)
                {
                    lblNumStops.Text = pitStops.Count().ToString();
                    lblAvgGainLoss.Text = pitStops.Average(p => p.PositionsGainedLost).ToString("N2");
                    lblAvgPitTime.Text = pitStops.Average(p => p.PitStopDuration).ToString("N2");
                    lblAvgTotalTime.Text = pitStops.Average(p => p.TotalDuration).ToString("N2");
                    lblAvgInOut.Text = pitStops.Average(p => (p.InTravelDuration + p.OutTravelDuration)).ToString("N2");

                    if (_averages != null && _averages.Count > 0)
                    {
                        var carAverages = _averages.FirstOrDefault(a => a.CarNumber == carNumber);

                        lblAvgGainLossRank.Text = $"{_averages.OrderByDescending(a => a.TotalGainLoss).ToList().IndexOf(carAverages) + 1}";
                        lblAvgPitTimeRank.Text = $"{_averages.OrderBy(a => a.AveragePitTime).ToList().IndexOf(carAverages) + 1}";
                        lblAvgTotalTimeRank.Text = $"{_averages.OrderBy(a => a.AverageTotalTime).ToList().IndexOf(carAverages) + 1}";
                        lblAvgInOutRank.Text = $"{_averages.OrderBy(a => a.AverageInOutTime).ToList().IndexOf(carAverages) + 1}";
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                lvDriverPitStops.EndUpdate();
            }
        }

        private async Task UpdateGridsAsync()
        {
            var pitStops = await _pitStopsRepository.GetPitStopsAsync(SeriesId, RaceId);

            _averages = GetPitStopAverages(pitStops);

            UpdateTotalGainLossGrid(_averages);
            UpdateAveragePitTimeGrid(_averages);
            UpdateAverageTotalTimeGrid(_averages);
            UpdateAverageInOutTimeGrid(_averages);
            UpdateAverageGreenPitTimeGrid(_averages);
            UpdateAverageGreenTotalTimeGrid(_averages);
            UpdateAverageGreenInOutTimeGrid(_averages);
        }

        private IList<PitStopAverages> GetPitStopAverages(IEnumerable<PitStop> pitStops)
        {
            var averages = new List<PitStopAverages>();

            foreach (var driverPitStops in pitStops.GroupBy(p => p.VehicleNumber))
            {
                if (driverPitStops != null && driverPitStops.Count() > 0)
                {
                    var driverPitStopSet = new PitStopAverages
                    {
                        CarNumber = driverPitStops.Key,
                        Driver = driverPitStops.First().DriverName,
                        TotalGainLoss = driverPitStops.Sum(p => p.PositionsGainedLost),
                        AveragePitTime = driverPitStops.Average(p => p.PitStopDuration),
                        AverageInOutTime = driverPitStops.Average(p => (p.InTravelDuration + p.OutTravelDuration)),
                        AverageTotalTime = driverPitStops.Average(p => p.TotalDuration)
                    };

                    var greenFlagStops = driverPitStops.Where(p => p.PitInFlagStatus == 1);

                    if (greenFlagStops != null && greenFlagStops.Count() > 0)
                    {
                        driverPitStopSet.AverageGreenPitTime = greenFlagStops.Average(p => p.PitStopDuration);
                        driverPitStopSet.AverageGreenTotalTime = greenFlagStops.Average(p => p.TotalDuration);
                        driverPitStopSet.AverageGreenInOutTime = greenFlagStops.Average(p => p.InTravelDuration + p.OutTravelDuration);
                    }

                    averages.Add(driverPitStopSet);
                }
            }

            return averages;
        }

        private void UpdateTotalGainLossGrid(IList<PitStopAverages> averages)
        {
            try
            {
                lvTotalGainLoss.BeginUpdate();

                lvTotalGainLoss.Items.Clear();

                int i = 1;

                foreach (var item in averages.OrderByDescending(a => a.TotalGainLoss))
                {
                    var lvi = new ListViewItem(i.ToString());

                    lvi.SubItems.Add(item.Driver);
                    lvi.SubItems.Add(item.TotalGainLoss.ToString());

                    if (i % 2 == 0)
                    {
                        lvi.BackColor = _alternateRowBackColor;
                    }

                    lvTotalGainLoss.Items.Add(lvi);

                    i++;
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                lvTotalGainLoss.EndUpdate();
            }
        }

        private void UpdateAveragePitTimeGrid(IList<PitStopAverages> averages)
        {
            try
            {
                lvAveragePitTime.BeginUpdate();

                lvAveragePitTime.Items.Clear();

                int i = 1;

                foreach (var item in averages.Where(a => a.AveragePitTime > 0).OrderBy(a => a.AveragePitTime))
                {
                    var lvi = new ListViewItem(i.ToString());

                    lvi.SubItems.Add(item.Driver);
                    lvi.SubItems.Add(item.AveragePitTime.ToString("N3"));

                    if (i % 2 == 0)
                    {
                        lvi.BackColor = _alternateRowBackColor;
                    }

                    lvAveragePitTime.Items.Add(lvi);

                    i++;
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                lvAveragePitTime.EndUpdate();
            }
        }

        private void UpdateAverageTotalTimeGrid(IList<PitStopAverages> averages)
        {
            try
            {
                lvAverageTotalTime.BeginUpdate();

                lvAverageTotalTime.Items.Clear();

                int i = 1;

                foreach (var item in averages.Where(a => a.AverageTotalTime > 0).OrderBy(a => a.AverageTotalTime))
                {
                    var lvi = new ListViewItem(i.ToString());

                    lvi.SubItems.Add(item.Driver);
                    lvi.SubItems.Add(item.AverageTotalTime.ToString("N3"));

                    if (i % 2 == 0)
                    {
                        lvi.BackColor = _alternateRowBackColor;
                    }

                    lvAverageTotalTime.Items.Add(lvi);

                    i++;
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                lvAverageTotalTime.EndUpdate();
            }
        }

        private void UpdateAverageInOutTimeGrid(IList<PitStopAverages> averages)
        {
            try
            {
                lvInOutTime.BeginUpdate();

                lvInOutTime.Items.Clear();

                int i = 1;

                foreach (var item in averages.Where(a => a.AverageInOutTime > 0).OrderBy(a => a.AverageInOutTime))
                {
                    var lvi = new ListViewItem(i.ToString());

                    lvi.SubItems.Add(item.Driver);
                    lvi.SubItems.Add(item.AverageInOutTime.ToString("N3"));

                    if (i % 2 == 0)
                    {
                        lvi.BackColor = _alternateRowBackColor;
                    }

                    lvInOutTime.Items.Add(lvi);

                    i++;
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                lvInOutTime.EndUpdate();
            }
        }

        private void UpdateAverageGreenPitTimeGrid(IList<PitStopAverages> averages)
        {
            try
            {
                lvAverageGreenPitTime.BeginUpdate();

                lvAverageGreenPitTime.Items.Clear();

                int i = 1;

                foreach (var item in averages.Where(a => a.AverageGreenPitTime > 0).OrderBy(a => a.AverageGreenPitTime))
                {
                    var lvi = new ListViewItem(i.ToString());

                    lvi.SubItems.Add(item.Driver);
                    lvi.SubItems.Add(item.AverageGreenPitTime.ToString("N3"));

                    if (i % 2 == 0)
                    {
                        lvi.BackColor = _alternateRowBackColor;
                    }

                    lvAverageGreenPitTime.Items.Add(lvi);

                    i++;
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                lvAverageGreenPitTime.EndUpdate();
            }
        }

        private void UpdateAverageGreenTotalTimeGrid(IList<PitStopAverages> averages)
        {
            try
            {
                lvAverageGreenTotalTime.BeginUpdate();

                lvAverageGreenTotalTime.Items.Clear();

                int i = 1;

                foreach (var item in averages.Where(a => a.AverageGreenTotalTime > 0).OrderBy(a => a.AverageGreenTotalTime))
                {
                    var lvi = new ListViewItem(i.ToString());

                    lvi.SubItems.Add(item.Driver);
                    lvi.SubItems.Add(item.AverageGreenTotalTime.ToString("N3"));

                    if (i % 2 == 0)
                    {
                        lvi.BackColor = _alternateRowBackColor;
                    }

                    lvAverageGreenTotalTime.Items.Add(lvi);

                    i++;
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                lvAverageGreenTotalTime.EndUpdate();
            }
        }

        private void UpdateAverageGreenInOutTimeGrid(IList<PitStopAverages> averages)
        {
            try
            {
                lvGreenOutTime.BeginUpdate();

                lvGreenOutTime.Items.Clear();

                int i = 1;

                foreach (var item in averages.Where(a => a.AverageGreenInOutTime > 0).OrderBy(a => a.AverageGreenInOutTime))
                {
                    var lvi = new ListViewItem(i.ToString());

                    lvi.SubItems.Add(item.Driver);
                    lvi.SubItems.Add(item.AverageGreenInOutTime.ToString("N3"));

                    if (i % 2 == 0)
                    {
                        lvi.BackColor = _alternateRowBackColor;
                    }

                    lvGreenOutTime.Items.Add(lvi);

                    i++;
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                lvGreenOutTime.EndUpdate();
            }
        }

        #endregion

        #region classes

        private class PitStopAverages
        {
            public string CarNumber { get; set; }
            public string Driver { get; set; }
            public int TotalGainLoss { get; set; }
            public float AveragePitTime { get; set; }
            public float AverageGreenPitTime { get; set; }
            public float AverageTotalTime { get; set; }
            public float AverageGreenTotalTime { get; set; }
            public float AverageInOutTime { get; set; }
            public float AverageGreenInOutTime { get; set; }
        }

        private class DriverViewModel
        {
            public string CarNumber { get; set; }
            public string Name { get; set; }
        }

        private class CautionViewModel
        {
            public int CautionNumber { get; set; }
            public int StartLap { get; set; }
            public int EndLap { get; set; }
            public string Comment { get; set; }

            public override string ToString()
            {
                return $"[{CautionNumber}] {StartLap}-{EndLap}  {Comment}";
            }
        }

        #endregion
    }
}
