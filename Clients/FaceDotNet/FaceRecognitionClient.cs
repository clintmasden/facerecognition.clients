using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Common.Domain.Entities;
using Common.Infrastructure.Extensions;
using Common.Infrastructure.Interfaces;
using FaceDotNet.Models;
using FaceRecognitionDotNet;
using Point = System.Drawing.Point;

namespace FaceDotNet
{
    public class FaceRecognitionClient : IFaceRecognitionClient
    {
        public FaceRecognitionClient(string modelsDirectory)
        {
            FaceRecognition = FaceRecognition.Create(modelsDirectory);
            FaceDistanceThreshold = .6;
        }

        private FaceRecognition FaceRecognition { get; }

        private double FaceDistanceThreshold { get; }

        public Bitmap GetBitmapWithLocations(Bitmap bitmap)
        {
            var returnBitmap = (Bitmap)bitmap.Clone();

            var capturedImage = FaceRecognition.LoadImage(bitmap);
            var capturedLocations = FaceRecognition.FaceLocations(capturedImage).ToList();

            var g = Graphics.FromImage(returnBitmap);

            foreach (var location in capturedLocations)
            {
                using (var p = new Pen(Color.Yellow, bitmap.Width / 200f))
                {
                    g.DrawRectangle(p, location.Left, location.Top, location.Right - location.Left, location.Bottom - location.Top);
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
                using (var p = new Pen(Color.Yellow, returnBitmap.Width / 200f))
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
            var capturedImage = new ClassifiedImage(FaceRecognition);
            capturedImage.Load(bitmap);

            return capturedImage;
        }

        public List<ClassifiedMatch> GetClassifiedMatchesFromImage(ClassifiedImage image, List<User> users)
        {
            var classifiedMatches = new List<ClassifiedMatch>();

            foreach (var user in users)
            {
                foreach (var userImage in user.UserImages)
                {
                    // you can leverage a saved encoding instead of loading the image then comparing.

                    var userClassifiedImage = new ClassifiedImage(FaceRecognition);
                    userClassifiedImage.Load(userImage.ImageFilePath);

                    for (var encodingIndex = 0; encodingIndex < image.Encodings.Count; encodingIndex++)
                    {
                        foreach (var userEncoding in userClassifiedImage.Encodings)
                        {
                            var faceDistance = FaceRecognition.FaceDistance(image.Encodings[encodingIndex], userEncoding);

                            if (!(faceDistance < FaceDistanceThreshold))
                            {
                                continue;
                            }

                            classifiedMatches.Add(new ClassifiedMatch
                            {
                                User = user,
                                FaceEncoding = image.Encodings[encodingIndex],
                                Distance = faceDistance,
                                Location = GetLocationFromFaceRecognitionLocation(image.Locations[encodingIndex])
                            });
                        }
                    }
                }
            }

            SetIsCenteredOnClassifiedMatches(image, classifiedMatches);

            return classifiedMatches;
        }

        private Rectangle GetLocationFromFaceRecognitionLocation(Location location)
        {
            return new Rectangle(location.Left, location.Top, location.Right - location.Left, location.Bottom - location.Top);
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