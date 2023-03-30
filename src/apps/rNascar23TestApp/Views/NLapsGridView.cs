using rNascar23.LapTimes.Models;
using rNascar23.LiveFeeds.Models;
using rNascar23TestApp.CustomViews;
using rNascar23TestApp.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static rNascar23TestApp.Views.NLapsGridView;

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

        public enum ComparisonTypes
        {
            Time,
            Speed
        }

        #endregion

        #region properties

        private ComparisonTypes _comparisonType = ComparisonTypes.Time;
        public ComparisonTypes ComparisonType
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

                SetDataSource(_data);
            }
        }
        public string CustomGridName { get; set; }
        public string Description { get; set; }
        public GridSettings Settings { get; set; }
        public bool IsCustomGrid { get; set; }

        #endregion

        #region ctor

        public NLapsGridView()
            : this(ViewTypes.Last5Laps, ComparisonTypes.Speed)
        {

        }
        public NLapsGridView(ViewTypes viewType, ComparisonTypes comparisonType)
        {
            InitializeComponent();

            _viewType = viewType;
            _comparisonType = comparisonType;

            BuildGridView(Grid);

            Settings = new GridSettings()
            {
                ApiSource = ApiSources.LapTimes,
                SortOrderField = "Average",
                SortOrder = _comparisonType == ComparisonTypes.Speed ? 2 : 1
            };

            this.Width = 260;

            SetGridTitle(_viewType);
        }

        #endregion

        #region private

        private void SetGridTitle(ViewTypes viewType)
        {
            switch (viewType)
            {
                case ViewTypes.Best5Laps:
                    TitleLabel.Text = "Best 5 Laps";
                    TitleLabel.ForeColor = Color.Black;
                    TitleLabel.BackColor = Color.White;
                    break;
                case ViewTypes.Best10Laps:
                    TitleLabel.Text = "Best 10 Laps";
                    TitleLabel.ForeColor = Color.White;
                    TitleLabel.BackColor = Color.Black;
                    break;
                case ViewTypes.Best15Laps:
                    TitleLabel.Text = "Best 15 Laps";
                    TitleLabel.ForeColor = Color.Black;
                    TitleLabel.BackColor = Color.White;
                    break;
                case ViewTypes.Last5Laps:
                    TitleLabel.Text = "Last 5 Laps";
                    TitleLabel.ForeColor = Color.White;
                    TitleLabel.BackColor = Color.Black;
                    break;
                case ViewTypes.Last10Laps:
                    TitleLabel.Text = "Last 10 Laps";
                    TitleLabel.ForeColor = Color.Black;
                    TitleLabel.BackColor = Color.White;
                    break;
                case ViewTypes.Last15Laps:
                    TitleLabel.Text = "Last 15 Laps";
                    TitleLabel.ForeColor = Color.White;
                    TitleLabel.BackColor = Color.Black;
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
        }

        private IList<LapAverageViewModel> BuildViewModels(IList<DriverLaps> data)
        {
            switch (ComparisonType)
            {
                case ComparisonTypes.Time:
                    return BuildViewModelsByTime(data);
                case ComparisonTypes.Speed:
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
                            Average = (float)Math.Round(d.Best5LapAverageSpeed().GetValueOrDefault(999), 3)
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
                        Where(d => d.Best5LapAverageSpeed().GetValueOrDefault(-1) != -1).
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
                        Where(d => d.Best10LapAverageSpeed().GetValueOrDefault(-1) != -1).
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
                        Where(d => d.Best15LapAverageSpeed().GetValueOrDefault(-1) != -1).
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
                        Where(d => d.AverageSpeedLast5Laps().GetValueOrDefault(-1) != -1).
                        Take(10).
                        Select(d => new LapAverageViewModel()
                        {
                            Driver = d.FullName,
                            Average = (float)Math.Round(d.AverageSpeedLast5Laps().GetValueOrDefault(-1), 3)
                        }).
                        ToList();

                case ViewTypes.Last10Laps:
                    return data.
                        OrderBy(d => d.AverageSpeedLast10Laps().GetValueOrDefault(-1)).
                        Where(d => d.AverageSpeedLast10Laps().GetValueOrDefault(-1) != -1).
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

            GridViewColumnBuilder.ConfigureColumn(Column1, "Driver", 150);

            GridViewColumnBuilder.ConfigureColumn(Column2, "Average", 75, "Average");

            dataGridView.ColumnHeadersVisible = true;
            dataGridView.RowHeadersVisible = false;
            dataGridView.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect;
            dataGridView.AutoGenerateColumns = false;

            return dataGridView;
        }

        private void Grid_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            for (int i = 0; i < Grid.Rows.Count; i++)
            {
                var row = Grid.Rows[i];

                if (row.Index % 2 == 0)
                {
                    row.DefaultCellStyle.BackColor = Color.LightGray;
                }
                else
                {
                    row.DefaultCellStyle.BackColor = Color.White;
                }
            }
        }

        #endregion
    }
}