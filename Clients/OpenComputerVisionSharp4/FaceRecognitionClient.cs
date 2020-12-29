using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using Common.Domain.Entities;
using Common.Infrastructure.Extensions;
using Common.Infrastructure.Interfaces;
using OpenComputerVisionSharp4.Configurations;
using OpenComputerVisionSharp4.Models;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using OpenCvSharp.Face;
using Point = System.Drawing.Point;
using Size = OpenCvSharp.Size;

namespace OpenComputerVisionSharp4
{
    public class FaceRecognitionClient : IFaceRecognitionClient
    {
        public FaceRecognitionClient()
        {
            Classifier = new CascadeClassifier(@"Resources\haarcascade_frontalface_default.xml");
            ClassifierConfiguration = new ClassifierConfiguration();
        }

        public FaceRecognitionClient(string classifierFilePath)
        {
            Classifier = new CascadeClassifier(classifierFilePath);
            ClassifierConfiguration = new ClassifierConfiguration();
        }

        private CascadeClassifier Classifier { get; }
        private ClassifierConfiguration ClassifierConfiguration { get; }

        private EigenFaceRecognizer Recognizer { get; set; }

        public Bitmap GetBitmapWithLocations(Bitmap bitmap)
        {
            var returnBitmap = (Bitmap)bitmap.Clone();

            var mat = returnBitmap.ToMat().CvtColor(ColorConversionCodes.BGR2GRAY);

            var locations = Classifier.DetectMultiScale(
                mat, ClassifierConfiguration.ScaleFactor, ClassifierConfiguration.MinimumNeighbors, ClassifierConfiguration.HaarDetectionType, ClassifierConfiguration.MinimumSize, ClassifierConfiguration.MaximumSize);

            var g = Graphics.FromImage(returnBitmap);

            foreach (var rectangle in locations)
            {
                using (var p = new Pen(Color.Purple, returnBitmap.Width / 200f))
                {
                    g.DrawRectangle(p, rectangle.Left, rectangle.Top, rectangle.Right - rectangle.Left, rectangle.Bottom - rectangle.Top);
                }
            }

            return returnBitmap;
        }

        public Bitmap GetBitmapWithClassifiedMatchedLocations(Bitmap bitmap, List<User> users)
        {
            var image = GetClassifiedImage(bitmap);

            if (!image.Locations.Any())
            {
                return bitmap;
            }

            var classifiedMatches = GetClassifiedMatchesFromImage(image, users);

            if (!classifiedMatches.Any())
            {
                return bitmap;
            }

            return GetBitmapWithClassifiedMatchedLocations(bitmap, classifiedMatches);
        }

        public Bitmap GetBitmapWithClassifiedMatchedLocations(Bitmap bitmap, List<ClassifiedMatch> classifiedMatches)
        {
            var returnBitmap = (Bitmap)bitmap.Clone();

            var g = Graphics.FromImage(returnBitmap);

            foreach (var classifiedMatch in classifiedMatches)
            {
                using (var p = new Pen(Color.Purple, returnBitmap.Width / 200f))
                {
                    g.DrawRectangle(p, classifiedMatch.Location);
                }

                var namePosition = new PointF(classifiedMatch.Location.Left + 10, classifiedMatch.Location.Top + 10);
                var otherPosition = new PointF(classifiedMatch.Location.Left + 10, classifiedMatch.Location.Bottom - 50);

                g.DrawString(classifiedMatch.User.Name, SystemFonts.CaptionFont, Brushes.Blue, namePosition);
                g.DrawString($"{classifiedMatch.IsCenter}: {classifiedMatch.Location}", SystemFonts.CaptionFont, Brushes.Green, otherPosition);
            }

            return returnBitmap;
        }

        public ClassifiedImage GetClassifiedImage(Bitmap bitmap)
        {
            var image = new ClassifiedImage(Classifier, ClassifierConfiguration);
            image.Load(bitmap);

            return image;
        }

        public Bitmap GetFaceBitmapFromClassifiedImage(ClassifiedImage image)
        {
            return new Mat(image.Mat, image.Locations.First()).Resize(new Size(100, 100), interpolation: InterpolationFlags.Cubic).ToBitmap();
        }

        public void TrainRecognizer(List<User> users)
        {
            var imageList = new List<Mat>();
            var indexList = new List<int>();

            var userIndex = 0;

            foreach (var user in users)
            {
                foreach (var userImage in user.UserImages.Where(userImage => File.Exists(userImage.ImageFilePath)))
                {
                    imageList.Add(new Mat(userImage.ImageFilePath).CvtColor(ColorConversionCodes.BGR2GRAY));
                    indexList.Add(userIndex);
                }

                userIndex++;
            }

            Recognizer = EigenFaceRecognizer.Create();
            Recognizer.Train(imageList, indexList);
        }

        public List<ClassifiedMatch> GetClassifiedMatchesFromImage(ClassifiedImage image, List<User> users)
        {
            var imageMatches = new List<ClassifiedMatch>();

            foreach (var location in image.Locations)
            {
                var croppedMat = new Mat(image.Mat, location).Resize(new Size(100, 100), interpolation: InterpolationFlags.Cubic).CvtColor(ColorConversionCodes.BGR2GRAY);

                Recognizer.Predict(croppedMat, out var userIndex, out var resultConfidence);

                if (resultConfidence > ClassifierConfiguration.ConfidenceThreshold)
                {
                    continue;
                }

                imageMatches.Add(new ClassifiedMatch
                {
                    User = users[userIndex],
                    Distance = resultConfidence,
                    Location = GetLocationFromOpenCVRect(location)
                });
            }

            SetIsCenteredOnClassifiedMatches(image, imageMatches);

            return imageMatches;
        }

        private Rectangle GetLocationFromOpenCVRect(Rect r)
        {
            return new Rectangle(r.Left, r.Top, r.Right - r.Left, r.Bottom - r.Top);
        }

        private void SetIsCenteredOnClassifiedMatches(ClassifiedImage image, List<ClassifiedMatch> classifiedMatches)
        {
            var centerPoint = new Point(image.Bitmap.Size.Width / 2, image.Bitmap.Height / 2);

            foreach (var classifiedMatch in classifiedMatches)
            {
                if (!classifiedMatches.Any(c => c.IsCenter))
                {
                    classifiedMatch.IsCenter = true;
                    continue;
                }

                var currentIsCenterClassifiedMatch = classifiedMatches.Single(c => c.IsCenter);
                var currentIsCenterCenterDifference = currentIsCenterClassifiedMatch.Location.CenterDifference(centerPoint);

                var classifiedMatchCenterDifference = classifiedMatch.Location.CenterDifference(centerPoint);

                if (currentIsCenterCenterDifference >= classifiedMatchCenterDifference)
                {
                    continue;
                }

                currentIsCenterClassifiedMatch.IsCenter = false;
                classifiedMatch.IsCenter = true;
            }
        }
    }
}