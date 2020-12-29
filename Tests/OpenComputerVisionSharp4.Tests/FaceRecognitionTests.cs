using System.Collections.Generic;
using System.Drawing;
using Common.Domain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace OpenComputerVisionSharp4.Tests
{
    [TestClass]
    public class FaceRecognitionTests
    {
        [TestMethod]
        public void GetClassifiedImage_Pass()
        {
            var client = new FaceRecognitionClient(@"..\..\..\..\..\Resources\ComputerVision.Models\haarcascade_frontalface_default.xml");

            var originalBidenImage = client.GetClassifiedImage(new Bitmap(@"..\..\..\..\..\Resources\Tests.Data\images\biden.jpg"));
            Assert.IsTrue(originalBidenImage.Locations.Count == 1);

            var cropBidenImage = client.GetClassifiedImage(new Bitmap(@"..\..\..\..\..\Resources\Tests.Data\images\biden-100-cubic.bmp"));
            Assert.IsTrue(cropBidenImage.Locations.Count == 1);

            var originalObamaImage = client.GetClassifiedImage(new Bitmap(@"..\..\..\..\..\Resources\Tests.Data\images\obama.jpg"));
            Assert.IsTrue(originalBidenImage.Locations.Count == 1);

            var cropObamaImage = client.GetClassifiedImage(new Bitmap(@"..\..\..\..\..\Resources\Tests.Data\images\obama-100-cubic.bmp"));
            Assert.IsTrue(cropBidenImage.Locations.Count == 1);
        }

        [TestMethod]
        public void GetClassifiedMatchesFromImage_Pass()
        {
            var client = new FaceRecognitionClient(@"..\..\..\..\..\Resources\ComputerVision.Models\haarcascade_frontalface_default.xml");

            var users = new List<User>()
            {
                new User()
                {
                    Name = "Biden",
                    UserImages = new List<UserImage>()
                    {
                        new UserImage()
                        {
                            ImageFilePath = @"..\..\..\..\..\Resources\Tests.Data\images\biden-100-cubic.bmp"
                        }
                    }
                },
                new User()
                {
                    Name = "Obama",
                    UserImages = new List<UserImage>()
                    {
                        new UserImage()
                        {
                            ImageFilePath = @"..\..\..\..\..\Resources\Tests.Data\images\obama-100-cubic.bmp"
                        }
                    }
                }
            };

            client.TrainRecognizer(users);

            var compareImage = client.GetClassifiedImage(new Bitmap(@"..\..\..\..\..\Resources\Tests.Data\images\obama-biden-compare.jpg"));
            Assert.IsTrue(compareImage.Locations.Count == 2);

            var classifiedMatches = client.GetClassifiedMatchesFromImage(compareImage, users);

            Assert.IsTrue(classifiedMatches.Count == 2);
            Assert.IsTrue(classifiedMatches[0].User == users[0]);
            Assert.IsTrue(classifiedMatches[1].User == users[1]);
        }
    }
}