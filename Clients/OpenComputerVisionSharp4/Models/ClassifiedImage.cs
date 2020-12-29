using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using OpenComputerVisionSharp4.Configurations;
using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace OpenComputerVisionSharp4.Models
{
    public class ClassifiedImage
    {
        public ClassifiedImage(CascadeClassifier classifier, ClassifierConfiguration configuration)
        {
            Classifier = classifier;
            Configuration = configuration;

            Locations = new List<Rect>();
        }

        private CascadeClassifier Classifier { get; }

        private ClassifierConfiguration Configuration { get; }

        public bool IsLoaded { get; private set; }

        public Bitmap Bitmap { get; private set; }

        public Mat Mat { get; private set; }

        public List<Rect> Locations { get; private set; }

        public bool Load(Bitmap bitmap)
        {
            Bitmap = bitmap;

            try
            {
                Mat = Bitmap.ToMat();

                if (Mat.Channels() > 1)
                {
                    Mat.CvtColor(ColorConversionCodes.BGR2GRAY);
                }

                // Mat = bitmap.ToMat().CvtColor(ColorConversionCodes.BGR2GRAY);

                Locations = Classifier.DetectMultiScale(
                    Mat, Configuration.ScaleFactor, Configuration.MinimumNeighbors, Configuration.HaarDetectionType, Configuration.MinimumSize, Configuration.MaximumSize).ToList();
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