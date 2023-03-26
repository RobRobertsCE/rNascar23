using rNascar23.LapTimes.Models;
using rNascar23TestApp.CustomViews;
using rNascar23TestApp.Settings;
using rNascar23TestApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace rNascar23TestApp.Views
{
    public partial class NLapsGridView : UserControl, IGridView<DriverLaps>
    {
        #region enums

        public enum ViewTypes
        {
            Best5Laps,
            Best10Laps,
            Best15Laps,
            Last5Laps,
            Last10Laps,
            Last15Laps,
        }
             
        #endregion

        #region properties

        public ApiSources ApiSource => ApiSources.LapTimes;
        public string Title => "NLaps";

        private SpeedTimeType _comparisonType = SpeedTimeType.Seconds ;
        public SpeedTimeType ComparisonType
        {
            get
            {
                return _comparisonType;
            }
            set
            {
                _comparisonType = value;
            }
        }
        private ViewTypes _viewType = ViewTypes.Best5Laps;
        public ViewTypes ViewType
        {
            get
            {
                return _viewType;
            }
            set
            {
                _viewType = value;
            }
        }

        private IList<DriverLaps> _data = new List<DriverLaps>();
        public IList<DriverLaps> Data
        {
            get
            {
                return _data;
            }
            set
            {
                _data = value;

                if (_data != null)
                    SetDataSource(_data);
            }
        }
        public string CustomGridName { get; set; }
        public string Description { get; set; }
        public GridSettings Settings { get; set; }
        public bool IsCustomGrid { get; set; }
        public DataGridView DataGridView
        {
            get
            {
                return Grid;
            }
        }
        public SpeedTimeType DisplayType { get; set; }

        #endregion

        #region ctor

        public NLapsGridView()
            : this(ViewTypes.Last5Laps, SpeedTimeType.MPH)
        {

        }
        public NLapsGridView(ViewTypes viewType, SpeedTimeType comparisonType)
        {
            InitializeComponent();

            _viewType = viewType;
            _comparisonType = comparisonType;

            BuildGridView(Grid);

            Settings = new GridSettings()
            {
                ApiSource = ApiSources.LapTimes,
                SortOrderField = "Average",
                SortOrder = _comparisonType == SpeedTimeType.MPH ? 2 : 1
            };

            SetGridTitle(_viewType);

            this.Width = 190;
        }

        #endregion

        #region private

        private void SetGridTitle(ViewTypes viewType)
        {
            var scale = _comparisonType == SpeedTimeType.MPH ? " (Avg M.P.H.)" : " (Avg Lap Time)";

            switch (viewType)
            {
                case ViewTypes.Best5Laps:
                    TitleLabel.Text = $"Best 5 Laps{scale}";
                    TitleLabel.ForeColor = Color.Black;
                    TitleLabel.BackColor = Color.FromArgb(0, 148, 255);
                    break;
                case ViewTypes.Best10Laps:
                    TitleLabel.Text = $"Best 10 Laps{scale}";
                    TitleLabel.ForeColor = Color.Black;
                    TitleLabel.BackColor = Color.FromArgb(0, 148, 255);
                    break;
                case ViewTypes.Best15Laps:
                    TitleLabel.Text = $"Best 15 Laps{scale}";
                    TitleLabel.ForeColor = Color.Black;
                    TitleLabel.BackColor = Color.FromArgb(0, 148, 255);
                    break;
                case ViewTypes.Last5Laps:
                    TitleLabel.Text = $"Last 5 Laps{scale}";
                    TitleLabel.ForeColor = Color.White;
                    TitleLabel.BackColor = Color.FromArgb(0, 74, 127);
                    break;
                case ViewTypes.Last10Laps:
                    TitleLabel.Text = $"Last 10 Laps{scale}";
                    TitleLabel.ForeColor = Color.White;
                    TitleLabel.BackColor = Color.FromArgb(0, 74, 127);
                    break;
                case ViewTypes.Last15Laps:
                    TitleLabel.Text = $"Last 15 Laps{scale}";
                    TitleLabel.ForeColor = Color.White;
                    TitleLabel.BackColor = Color.FromArgb(0, 74, 127);
                    break;
                default:
                    break;
            }
        }

        private void SetDataSource<T>(IList<T> values)
        {
            var models = BuildViewModels((IList<DriverLaps>)values);

            var dataTable = GridViewTableBuilder.ToDataTable<LapAverageViewModel>(models.ToList());

            var dataView = new DataView(dataTable);

            GridBindingSource.DataSource = dataView;

            Grid.DataSource = GridBindingSource;

            foreach (DataGridViewTextBoxColumn column in Grid.Columns)
            {
                column.Name = column.DataPropertyName;
            }

            if (!String.IsNullOrEmpty(Settings.SortOrderField))
            {
                var sortDirection = Settings.SortOrder == 0 ? String.Empty :
                    Settings.SortOrder == 1 ? "ASC" :
                    "DESC";

                var sortString = $"{Settings.SortOrderField} {sortDirection}".Trim();

                GridBindingSource.Sort = sortString;
            }

            Grid.ClearSelection();
        }

        private IList<LapAverageViewModel> BuildViewModels(IList<DriverLaps> data)
        {
            switch (ComparisonType)
            {
                case SpeedTimeType.Seconds:
                    return BuildViewModelsByTime(data);
                case SpeedTimeType.MPH:
                    return BuildViewModelsBySpeed(data);
                default:
                    return new List<LapAverageViewModel>();
            }
        }
        private IList<LapAverageViewModel> BuildViewModelsByTime(IList<DriverLaps> data)
        {
            switch (ViewType)
            {
                case ViewTypes.Best5Laps:
                    return data.
                        OrderBy(d => d.Best5LapAverageTime().GetValueOrDefault(999)).
                        Take(10).
                        Select(d => new LapAverageViewModel()
                        {
                            Driver = d.FullName,
                            Average = (float)Math.Round(d.Best5LapAverageTime().GetValueOrDefault(999), 3)
                        }).
                        ToList();

                case ViewTypes.Best10Laps:
                    return data.
                        OrderBy(d => d.Best10LapAverageTime().GetValueOrDefault(999)).
                        Take(10).
                        Select(d => new LapAverageViewModel()
                        {
                            Driver = d.FullName,
                            Average = (float)Math.Round(d.Best10LapAverageTime().GetValueOrDefault(999), 3)
                        }).
                        ToList();

                case ViewTypes.Best15Laps:
                    return data.
                        OrderBy(d => d.Best15LapAverageTime().GetValueOrDefault(999)).
                        Take(10).
                        Select(d => new LapAverageViewModel()
                        {
                            Driver = d.FullName,
                            Average = (float)Math.Round(d.Best15LapAverageTime().GetValueOrDefault(999), 3)
                        }).
                        ToList();

                case ViewTypes.Last5Laps:
                    return data.
                        OrderBy(d => d.AverageTimeLast5Laps().GetValueOrDefault(999)).
                        Take(10).
                        Select(d => new LapAverageViewModel()
                        {
                            Driver = d.FullName,
                            Average = (float)Math.Round(d.AverageTimeLast10Laps().GetValueOrDefault(999), 3)
                        }).
                        ToList();

                case ViewTypes.Last10Laps:
                    return data.
                        OrderBy(d => d.AverageTimeLast10Laps().GetValueOrDefault(999)).
                        Take(10).
                        Select(d => new LapAverageViewModel()
                        {
                            Driver = d.FullName,
                            Average = (float)Math.Round(d.AverageTimeLast10Laps().GetValueOrDefault(999), 3)
                        }).
                        ToList();

                case ViewTypes.Last15Laps:
                    return data.
                        OrderBy(d => d.AverageTimeLast15Laps().GetValueOrDefault(999)).
                        Take(10).
                        Select(d => new LapAverageViewModel()
                        {
                            Driver = d.FullName,
                            Average = (float)Math.Round(d.AverageTimeLast15Laps().GetValueOrDefault(999), 3)
                        }).
                        ToList();

                default:
                    return new List<LapAverageViewModel>();
            }
        }
        private IList<LapAverageViewModel> BuildViewModelsBySpeed(IList<DriverLaps> data)
        {
            switch (ViewType)
            {
                case ViewTypes.Best5Laps:
                    return data.
                        OrderBy(d => d.Best5LapAverageSpeed().GetValueOrDefault(-1)).
                        Where(d => d.AverageSpeedLast15Laps().GetValueOrDefault(-1) != -1).
                        Take(10).
                        Select(d => new LapAverageViewModel()
                        {
                            Driver = d.FullName,
                            Average = (float)Math.Round(d.Best5LapAverageSpeed().GetValueOrDefault(-1), 3)
                        }).
                        ToList();

                case ViewTypes.Best10Laps:
                    return data.
                        OrderBy(d => d.Best10LapAverageSpeed().GetValueOrDefault(-1)).
                        Where(d => d.AverageSpeedLast15Laps().GetValueOrDefault(-1) != -1).
                        Take(10).
                        Select(d => new LapAverageViewModel()
                        {
                            Driver = d.FullName,
                            Average = (float)Math.Round(d.Best10LapAverageSpeed().GetValueOrDefault(-1), 3)
                        }).
                        ToList();

                case ViewTypes.Best15Laps:
                    return data.
                        OrderBy(d => d.Best15LapAverageSpeed().GetValueOrDefault(-1)).
                        Where(d => d.AverageSpeedLast15Laps().GetValueOrDefault(-1) != -1).
                        Take(10).
                        Select(d => new LapAverageViewModel()
                        {
                            Driver = d.FullName,
                            Average = (float)Math.Round(d.Best15LapAverageSpeed().GetValueOrDefault(-1), 3)
                        }).
                        ToList();

                case ViewTypes.Last5Laps:
                    return data.
                        OrderBy(d => d.AverageSpeedLast5Laps().GetValueOrDefault(-1)).
                        Where(d => d.AverageSpeedLast15Laps().GetValueOrDefault(-1) != -1).
                        Take(10).
                        Select(d => new LapAverageViewModel()
                        {
                            Driver = d.FullName,
                            Average = (float)Math.Round(d.AverageSpeedLast10Laps().GetValueOrDefault(-1), 3)
                        }).
                        ToList();

                case ViewTypes.Last10Laps:
                    return data.
                        OrderBy(d => d.AverageSpeedLast10Laps().GetValueOrDefault(-1)).
                        Where(d => d.AverageSpeedLast15Laps().GetValueOrDefault(-1) != -1).
                        Take(10).
                        Select(d => new LapAverageViewModel()
                        {
                            Driver = d.FullName,
                            Average = (float)Math.Round(d.AverageSpeedLast10Laps().GetValueOrDefault(-1), 3)
                        }).
                        ToList();

                case ViewTypes.Last15Laps:
                    return data.
                        OrderBy(d => d.AverageSpeedLast15Laps().GetValueOrDefault(-1)).
                        Where(d => d.AverageSpeedLast15Laps().GetValueOrDefault(-1) != -1).
                        Take(10).
                        Select(d => new LapAverageViewModel()
                        {
                            Driver = d.FullName,
                            Average = (float)Math.Round(d.AverageSpeedLast15Laps().GetValueOrDefault(-1), 3)
                        }).
                        ToList();

                default:
                    return new List<LapAverageViewModel>();
            }
        }

        private DataGridView BuildGridView(DataGridView dataGridView)
        {
            DataGridViewTextBoxColumn Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();

            dataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[]
            {
                Column1,
                Column2,
            });

            dataGridView.DefaultCellStyle.Font = new Font("Tahoma", 10, FontStyle.Regular);

            GridViewColumnBuilder.ConfigureColumn(Column1, "Driver", 120);

            GridViewColumnBuilder.ConfigureColumn(Column2, "Average", 65, "Avg.", "N3");

            dataGridView.ColumnHeadersVisible = false;
            dataGridView.RowHeadersVisible = false;
            dataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView.ReadOnly = true;
            dataGridView.AutoGenerateColumns = false;
            dataGridView.AllowUserToResizeRows = false;
            dataGridView.SelectionChanged += (s, e) => Grid.ClearSelection();

            return dataGridView;
        }

        #endregion
    }
}