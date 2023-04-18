using Microsoft.Extensions.DependencyInjection;
using rNascar23.Dialogs;
using System;
using System.Windows.Forms;

namespace rNascar23
{
    public partial class MultiView : Form
    {
        public int SeriesId { get; set; }

        public MultiView()
        {
            InitializeComponent();
        }

        private void MultiView_Load(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
        }

        private void ExceptionHandler(Exception ex)
        {
            Console.WriteLine(ex.ToString());
            MessageBox.Show(ex.Message);
        }

        private void btnAddVideo_Click(object sender, EventArgs e)
        {
            try
            {
                AddVideo();
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
        }

        private void btnAddAudio_Click(object sender, EventArgs e)
        {
            try
            {
                AddAudio();
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
        }

        private void btnTileHorizontal_Click(object sender, EventArgs e)
        {
            try
            {
                LayoutMdi(MdiLayout.TileHorizontal);
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
        }

        private void btnTileVertical_Click(object sender, EventArgs e)
        {
            try
            {
                LayoutMdi(MdiLayout.TileVertical);
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
        }

        private void AddVideo()
        {
            var childForm = Program.Services.GetRequiredService<VideoPlayer>();

            childForm.MdiParent = this;
            childForm.SeriesId = SeriesId;

            childForm.Show();
        }

        private void AddAudio()
        {
            var childForm = Program.Services.GetRequiredService<AudioPlayer>();

            childForm.MdiParent = this;
            childForm.SeriesId = SeriesId;

            childForm.Show();
        }
    }
}
