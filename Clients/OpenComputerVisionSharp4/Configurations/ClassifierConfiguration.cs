using OpenCvSharp;

namespace OpenComputerVisionSharp4.Configurations
{
    public class ClassifierConfiguration
    {
        public ClassifierConfiguration()
        {
            ScaleFactor = 1.08;
            MinimumNeighbors = 10;
            HaarDetectionType = HaarDetectionType.DoCannyPruning;
            MinimumSize = new Size(20, 20);
            MaximumSize = null;

            ConfidenceThreshold = 3000;
        }

        public double ScaleFactor { get; set; }

        public int MinimumNeighbors { get; set; }

        public HaarDetectionType HaarDetectionType { get; set; }

        public Size? MinimumSize { get; set; }

        public Size? MaximumSize { get; set; }

        public double ConfidenceThreshold { get; set; }
    }
}