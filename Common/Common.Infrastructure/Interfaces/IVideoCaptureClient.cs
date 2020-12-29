namespace Common.Infrastructure.Interfaces
{
    /// <summary>
    ///     Video capture client
    /// </summary>
    public interface IVideoCaptureClient
    {
        /// <summary>
        ///     Starts the video capture
        /// </summary>
        void StartVideoCapture();

        /// <summary>
        ///     Stops the video capture
        /// </summary>
        void StopVideoCapture();
    }
}