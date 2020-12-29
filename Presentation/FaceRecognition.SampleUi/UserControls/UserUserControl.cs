using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Common.Domain.Configurations;
using Common.Domain.Entities;
using Common.Infrastructure.Interfaces;
using FaceRecognition.SampleUi.Clients;
using FaceRecognition.SampleUi.Models;
using FaceRecognition.SampleUi.States;

namespace FaceRecognition.SampleUi.UserControls
{
    public partial class UserUserControl : UserControl
    {
        public UserUserControl(ApplicationState applicationState)
        {
            InitializeComponent();

            ApplicationState = applicationState;

            User = new User();
            Configuration.Users.Add(User);

            SetUserControlDataBindings();
            SetDataGridViewBindings();
            SetDefaultPictureBoxImage();
        }

        public UserUserControl(ApplicationState applicationState, User user)
        {
            InitializeComponent();

            ApplicationState = applicationState;

            User = user;

            DeleteButton.Enabled = true;

            SetUserControlDataBindings();
            SetDataGridViewBindings();
            SetDefaultPictureBoxImage();
        }

        private ApplicationState ApplicationState { get; }
        private ApplicationConfiguration Configuration => ApplicationState.ApplicationConfiguration;
        private IFaceRecognitionClient FaceRecognitionClient => ApplicationState.FaceRecognitionClient;

        private User User { get; }

        private CancellationTokenSource VideoCaptureCancellationTokenSource { get; set; }

        private WebCamClient VideoCapture { get; set; }
        private Bitmap VideoCaptureBitmap { get; set; }

        private void SetDataGridViewBindings()
        {
            UserImagesDataGridView.Enabled = true;
            UserImagesDataGridView.ColumnCount = 1;

            UserImagesDataGridView.Columns[0].Name = "File Path";
            UserImagesDataGridView.Columns[0].DataPropertyName = "ImageFilePath";
            UserImagesDataGridView.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            UserImagesDataGridView.AutoGenerateColumns = false;

            SetDataGridViewBindingSource_LameWinForms();
            //UserImagesDataGridView.DataSource = User.UserImages;
        }

        private void SetDataGridViewBindingSource_LameWinForms()
        {
            UserImagesDataGridView.DataSource = new BindingSource
            {
                DataSource = User.UserImages
            };
        }

        private void SetUserControlDataBindings()
        {
            NameTextBox.DataBindings.Add("Text", User, nameof(User.Name), true, DataSourceUpdateMode.OnPropertyChanged);
        }

        private void SetDefaultPictureBoxImage()
        {
            UserImagePictureBox.Image = null;

            if (!User.UserImages.Any())
            {
                return;
            }

            var userImage = User.UserImages.First();

            if (File.Exists(userImage.ImageFilePath))
            {
                UserImagePictureBox.Image = LoadBitmapUnlocked(User.UserImages.First().ImageFilePath);
            }
        }

        private void UserImagesDataGridView_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            var deleteImagePath = User.UserImages[e.Row.Index].ImageFilePath;

            if (File.Exists(deleteImagePath))
            {
                File.Delete(deleteImagePath);
            }
        }

        private void UserImagesDataGridView_SelectionChanged(object sender, EventArgs e)
        {
            var rowIndex = UserImagesDataGridView.CurrentRow?.Index;

            if (!rowIndex.HasValue)
            {
                return;
            }

            UserImagePictureBox.Image = LoadBitmapUnlocked(User.UserImages[rowIndex.Value].ImageFilePath);
        }

        private Bitmap LoadBitmapUnlocked(string filePath)
        {
            using (Bitmap bitmap = new Bitmap(filePath))
            {
                return new Bitmap(bitmap);
            }
        }

        private async Task GetVideoCaptureFrames(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                var bitmap = VideoCapture.GetBitmap();

                if (bitmap != null)
                {
                    VideoCaptureBitmap = bitmap;
                    UserImagePictureBox.Image = FaceRecognitionClient.GetBitmapWithLocations(VideoCaptureBitmap);
                }

                await Task.Delay(10, token);
            }
        }

        private void StartCaptureButton_Click(object sender, EventArgs e)
        {
            StopCaptureButton.Enabled = true;
            AddImageButton.Enabled = true;
            StartCaptureButton.Enabled = false;
            UserImagesDataGridView.Enabled = false;

            VideoCaptureCancellationTokenSource = new CancellationTokenSource();

            var webCamDevices = WebCamClient.FindDevices();
            var webCamFormats = WebCamClient.GetVideoFormat(0);

            VideoCapture = new WebCamClient(0, webCamFormats[0]);
            VideoCapture.Start();

            Task.Run(() => GetVideoCaptureFrames(VideoCaptureCancellationTokenSource.Token), VideoCaptureCancellationTokenSource.Token);
        }

        private void StopCaptureButton_Click(object sender, EventArgs e)
        {
            StopCaptureButton.Enabled = false;
            AddImageButton.Enabled = false;
            StartCaptureButton.Enabled = true;
            UserImagesDataGridView.Enabled = true;

            VideoCapture.Stop();

            VideoCaptureCancellationTokenSource?.Cancel();
        }

        private void AddImageButton_Click(object sender, EventArgs e)
        {
            if (VideoCaptureBitmap == null)
            {
                MessageBox.Show("There is no capture.");
                return;
            }

            GetFaceBitmapFromRecognitionClient();

            SetDataGridViewBindingSource_LameWinForms();
            UserImagesDataGridView.Invalidate();
        }

        private void GetFaceBitmapFromRecognitionClient()
        {
            var imageFilePath = Path.Combine(Configuration.UserImagesDirectory, $@"{User.Name.Trim()}.{DateTime.Now.Ticks}.bmp");

            switch (ApplicationState.FaceClient)
            {
                case FaceClients.EmguComputerVision3:
                    var emgu3FaceClient = (EmguComputerVision3.FaceRecognitionClient)FaceRecognitionClient;
                    var emgu3FaceImage = emgu3FaceClient.GetClassifiedImage(VideoCaptureBitmap);

                    if (emgu3FaceImage.Locations.Any())
                    {
                        if (emgu3FaceImage.Locations.Count > 1)
                        {
                            MessageBox.Show("More than one face in capture.");
                            return;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Unable to determine faces in capture.");
                        return;
                    }

                    var emgu3FaceBitmap = emgu3FaceClient.GetFaceBitmapFromClassifiedImage(emgu3FaceImage);

                    emgu3FaceBitmap.Save(imageFilePath);
                    User.UserImages.Add(new UserImage { ImageFilePath = imageFilePath });

                    break;

                case FaceClients.EmguComputerVision4:
                    var emgu4FaceClient = (EmguComputerVision4.FaceRecognitionClient)FaceRecognitionClient;
                    var emgu4FaceImage = emgu4FaceClient.GetClassifiedImage(VideoCaptureBitmap);

                    if (emgu4FaceImage.Locations.Any())
                    {
                        if (emgu4FaceImage.Locations.Count > 1)
                        {
                            MessageBox.Show("More than one face in capture.");
                            return;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Unable to determine faces in capture.");
                        return;
                    }

                    var emgu4FaceBitmap = emgu4FaceClient.GetFaceBitmapFromClassifiedImage(emgu4FaceImage);

                    emgu4FaceBitmap.Save(imageFilePath);
                    User.UserImages.Add(new UserImage { ImageFilePath = imageFilePath });

                    break;

                case FaceClients.FaceDotNet:
                    var dotNetFaceClient = (FaceDotNet.FaceRecognitionClient)FaceRecognitionClient;
                    var dotNetFaceImage = dotNetFaceClient.GetClassifiedImage(VideoCaptureBitmap);

                    if (dotNetFaceImage.Locations.Any())
                    {
                        if (dotNetFaceImage.Locations.Count > 1)
                        {
                            MessageBox.Show("More than one face in capture.");
                            return;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Unable to determine faces in capture.");
                        return;
                    }

                    // The face crop makes the recognition fail on the next load, saving the entire image for now (a helper method could be provided here)
                    var dotNetFaceBitmap = VideoCaptureBitmap;

                    dotNetFaceBitmap.Save(imageFilePath);
                    User.UserImages.Add(new UserImage { ImageFilePath = imageFilePath });
                    break;

                case FaceClients.OpenComputerVisionSharp4:
                    var sharp4FaceClient = (OpenComputerVisionSharp4.FaceRecognitionClient)FaceRecognitionClient;
                    var sharp4FaceImage = sharp4FaceClient.GetClassifiedImage(VideoCaptureBitmap);

                    if (sharp4FaceImage.Locations.Any())
                    {
                        if (sharp4FaceImage.Locations.Count > 1)
                        {
                            MessageBox.Show("More than one face in capture.");
                            return;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Unable to determine faces in capture.");
                        return;
                    }

                    var sharp4FaceBitmap = sharp4FaceClient.GetFaceBitmapFromClassifiedImage(sharp4FaceImage);

                    sharp4FaceBitmap.Save(imageFilePath);
                    User.UserImages.Add(new UserImage { ImageFilePath = imageFilePath });
                    break;
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            Configuration.Save();
            ApplicationState.InitializeFaceRecognitionClient();
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            Configuration.Users.Remove(User);
            Configuration.Save();

            ApplicationState.InitializeFaceRecognitionClient();
        }
    }
}