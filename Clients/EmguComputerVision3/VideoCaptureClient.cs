using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using Common.Infrastructure.Interfaces;
using Emgu.CV;

namespace EmguComputerVision3
{
    public class VideoCaptureClient : IVideoCaptureClient
    {
        private CancellationTokenSource _videoCaptureCancellationTokenSource;

        private Capture _videoCapture { get; set; }
        private Bitmap _videoCaptureBitmap { get; set; }

        public void StartVideoCapture()
        {
            _videoCaptureCancellationTokenSource = new CancellationTokenSource();
            _videoCapture = new Capture();

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
                    _videoCaptureBitmap = nextFrame.Bitmap;
                }
            }

            await Task.Delay(10, token);
        }
    }
}