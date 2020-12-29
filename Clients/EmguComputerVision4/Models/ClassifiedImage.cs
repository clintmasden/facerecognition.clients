using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Emgu.CV;
using Emgu.CV.Structure;
using EmguComputerVision4.Configurations;

namespace EmguComputerVision4.Models
{
    public class ClassifiedImage
    {
        public ClassifiedImage(CascadeClassifier classifier, ClassifierConfiguration configuration)
        {
            Classifier = classifier;
            Configuration = configuration;

            Locations = new List<Rectangle>();
        }

        private CascadeClassifier Classifier { get; }

        private ClassifierConfiguration Configuration { get; }

        public bool IsLoaded { get; private set; }

        public Bitmap Bitmap { get; private set; }

        public Image<Gray, byte> Image { get; set; }

        public List<Rectangle> Locations { get; private set; }

        public bool Load(Bitmap bitmap)
        {
            Bitmap = bitmap;

            try
            {
                Image = Bitmap.ToImage<Gray, byte>();

                Locations = Classifier.DetectMultiScale(
                    Image, Configuration.ScaleFactor, Configuration.MinimumNeighbors, Configuration.MinimumSize, Configuration.MaximumSize).ToList();
            }
            catch (Exception e)
            {
                return false;
            }

            IsLoaded = true;

            return true;
        }
    }
}