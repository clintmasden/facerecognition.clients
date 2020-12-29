using System.Drawing;

namespace EmguComputerVision4.Configurations
{
    public class ClassifierConfiguration
    {
        public ClassifierConfiguration()
        {
            ScaleFactor = 1.08;
            MinimumNeighbors = 10;
            MinimumSize = new Size(20, 20);
            MaximumSize = default;
            DistanceThreshold = 3000;
        }

        public double ScaleFactor { get; set; }

        public int MinimumNeighbors { get; set; }

        public Size MinimumSize { get; set; }

        public Size MaximumSize { get; set; }

        public double DistanceThreshold { get; set; }

    }
}