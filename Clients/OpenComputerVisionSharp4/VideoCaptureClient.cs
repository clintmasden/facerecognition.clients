using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using Common.Infrastructure.Interfaces;
using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace OpenComputerVisionSharp4
{
    public class VideoCaptureClient : IVideoCaptureClient
    {
        private CancellationTokenSource _videoCaptureCancellationTokenSource;
        private VideoCapture _videoCapture { get; set; }
        private Bitmap _videoCaptureBitmap { get; set; }

        public void StartVideoCapture()
        {
            _videoCaptureCancellationTokenSource = new CancellationTokenSource();
            _videoCapture = new VideoCapture(0, VideoCaptureAPIs.DSHOW);

            Task.Run(() => GetCaptureFrames(_videoCaptureCancellationTokenSource.Token), _videoCaptureCancellationTokenSource.Token);
        }

        public void StopVideoCapture()
        {
            _videoCaptureCancellationTokenSource?.Cancel();
        }

        private async Task GetCaptureFrames(CancellationToken token)
        {
            _videoCapture.Open(0);

            if (_videoCapture.IsOpened())
            {
                var mat = new Mat();

                while (!token.IsCancellationRequested)
                {
                    _videoCapture.Read(mat);

                    if (mat.Empty())
                    {
                        continue;
                    }

                    _videoCaptureBitmap = mat.ToBitmap();
                }
            }

            await Task.Delay(10, token);
        }
    }
}