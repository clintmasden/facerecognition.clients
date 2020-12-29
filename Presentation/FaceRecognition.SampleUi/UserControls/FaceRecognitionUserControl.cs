using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Common.Domain.Configurations;
using Common.Infrastructure.Interfaces;
using FaceRecognition.SampleUi.Clients;
using FaceRecognition.SampleUi.Extensions;
using FaceRecognition.SampleUi.States;

namespace FaceRecognition.SampleUi.UserControls
{
    public partial class FaceRecognitionUserControl : UserControl
    {
        public FaceRecognitionUserControl(ApplicationState applicationState)
        {
            InitializeComponent();

            ApplicationState = applicationState;

            StartVideoCapture();
        }

        private ApplicationState ApplicationState { get; }
        private ApplicationConfiguration Configuration => ApplicationState.ApplicationConfiguration;
        private IFaceRecognitionClient FaceRecognitionClient => ApplicationState.FaceRecognitionClient;

        private CancellationTokenSource VideoCaptureCancellationTokenSource { get; set; }
        private WebCamClient VideoCapture { get; set; }

        private async Task CaptureCamera(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                var capturedBitmap = VideoCapture.GetBitmap();

                CapturePictureBox.Image = Configuration.Users.Any() ? FaceRecognitionClient.GetBitmapWithClassifiedMatchedLocations(capturedBitmap, Configuration.Users) : capturedBitmap;

                await Task.Delay(10, token);
            }
        }

        private void StartVideoCapture()
        {
            StartToolStripMenuItem.Enabled = false;
            StopToolStripMenuItem.Enabled = true;

            VideoCaptureCancellationTokenSource = new CancellationTokenSource();

            var webCamDevices = WebCamClient.FindDevices();
            var webCamFormats = WebCamClient.GetVideoFormat(0);

            VideoCapture = new WebCamClient(0, webCamFormats[0]);
            VideoCapture.Start();

            Task.Run(() => CaptureCamera(VideoCaptureCancellationTokenSource.Token), VideoCaptureCancellationTokenSource.Token);
        }

        private void StopVideoCapture()
        {
            StartToolStripMenuItem.Enabled = true;
            StopToolStripMenuItem.Enabled = false;

            VideoCaptureCancellationTokenSource?.Cancel();
            VideoCapture.Stop();
        }

        private void NewUserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StopVideoCapture();
            UserControlExtensions.OpenUserControlWithXtraForm(new UserUserControl(ApplicationState));
        }

        private void OpenUsersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StopVideoCapture();
            UserControlExtensions.OpenUserControlWithXtraForm(new UserBrowserUserControl(ApplicationState));
        }

        private void StartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StartVideoCapture();
        }

        private void StopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StopVideoCapture();
        }
    }
}