using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using Common.Domain.Entities;
using Common.Infrastructure.Extensions;
using Common.Infrastructure.Interfaces;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Face;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using EmguComputerVision4.Configurations;
using EmguComputerVision4.Models;

namespace EmguComputerVision4
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

            var image = returnBitmap.ToImage<Gray, byte>(); // var mat = image.ToUMat();

            var locations = Classifier.DetectMultiScale(image, ClassifierConfiguration.ScaleFactor, ClassifierConfiguration.MinimumNeighbors, ClassifierConfiguration.MinimumSize, ClassifierConfiguration.MaximumSize);

            var g = Graphics.FromImage(returnBitmap);

            foreach (var location in locations)
            {
                using (var p = new Pen(Color.Red, returnBitmap.Width / 200f))
                {
                    g.DrawRectangle(p, location);
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
                using (var p = new Pen(Color.Red, returnBitmap.Width / 200f))
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
            return image.Image.Copy(image.Locations.First()).Resize(100, 100, Inter.Cubic).ToBitmap();
        }

        public void TrainRecognizer(List<User> users)
        {
            var imageList = new VectorOfMat();
            var indexList = new VectorOfInt();

            var userIndex = 0;

            foreach (var user in users)
            {
                foreach (var userImage in user.UserImages.Where(userImage => File.Exists(userImage.ImageFilePath)))
                {
                    imageList.Push(new Image<Gray, byte>(userImage.ImageFilePath));
                    indexList.Push(new[] { userIndex });
                }

                userIndex++;
            }

            Recognizer = new EigenFaceRecognizer(imageList.Size);
            Recognizer.Train(imageList, indexList);
        }

        public List<ClassifiedMatch> GetClassifiedMatchesFromImage(ClassifiedImage image, List<User> users)
        {
            var classifiedMatches = new List<ClassifiedMatch>();

            foreach (var location in image.Locations)
            {
                var croppedImage = image.Image.Copy(location).Convert<Gray, byte>().Resize(100, 100, Inter.Cubic);

                var result = Recognizer.Predict(croppedImage);

                Console.WriteLine($"{result.Distance} - {users[result.Label].Name}");

                if (result.Distance > ClassifierConfiguration.DistanceThreshold)
                {
                    continue;
                }

                classifiedMatches.Add(new ClassifiedMatch
                {
                    User = users[result.Label],
                    Distance = result.Distance,
                    Location = location
                });
            }

            SetIsCenteredOnClassifiedMatches(image, classifiedMatches);

            return classifiedMatches;
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