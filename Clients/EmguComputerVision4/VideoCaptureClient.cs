using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using Common.Infrastructure.Interfaces;
using Emgu.CV;

namespace EmguComputerVision4
{
    public class VideoCaptureClient : IVideoCaptureClient
    {
        private CancellationTokenSource _videoCaptureCancellationTokenSource;
        private VideoCapture _videoCapture { get; set; }
        private Bitmap _videoCaptureBitmap { get; set; }

        public void StartVideoCapture()
        {
            _videoCaptureCancellationTokenSource = new CancellationTokenSource();

            _videoCapture = new VideoCapture(0, VideoCapture.API.DShow);
            //_videoCapture.SetCaptureProperty(CapProp.Fps, 30);
            //_videoCapture.SetCaptureProperty(CapProp.FrameHeight, 450);
            //_videoCapture.SetCaptureProperty(CapProp.FrameWidth, 370);

            Task.Run(() => GetCaptureFrames(_videoCaptureCancellationTokenSource.Token), _videoCaptureCancellationTokenSource.Token);
        }

        public void StopVideoCapture()
        {
            _videoCaptureCancellationTokenSource?.Cancel();
        }

        private async Task GetCaptureFrames(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                var nextFrame = _videoCapture.QueryFrame();

                if (nextFrame != null)
                {
                    _videoCaptureBitmap = nextFrame.ToBitmap();
                }

                await Task.Delay(10, token);
            }
        }
    }
}