using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using FaceRecognitionDotNet;
using Image = FaceRecognitionDotNet.Image;

namespace FaceDotNet.Models
{
    public class ClassifiedImage
    {
        public ClassifiedImage(FaceRecognition faceRecognition)
        {
            FaceRecognition = faceRecognition;

            Locations = new List<Location>();
            Encodings = new List<FaceEncoding>();
        }

        private FaceRecognition FaceRecognition { get; }

        public bool IsLoaded { get; private set; }

        public string ImageFilePath { get; private set; }

        public Bitmap Bitmap { get; private set; }

        public Image Image { get; private set; }

        public List<Location> Locations { get; private set; }

        public List<FaceEncoding> Encodings { get; private set; }

        public bool Load(Bitmap bitmap)
        {
            Bitmap = bitmap;

            try
            {
                Image = FaceRecognition.LoadImage(Bitmap);
            }
            catch (Exception e)
            {
                return false;
            }

            Locations = FaceRecognition.FaceLocations(Image).ToList();

            if (Locations.Any())
            {
                Encodings = FaceRecognition.FaceEncodings(Image, Locations).ToList();
            }

            IsLoaded = true;

            return true;
        }

        public bool Load(string imageFilePath)
        {
            Bitmap = (Bitmap) System.Drawing.Image.FromFile(imageFilePath);
            ImageFilePath = imageFilePath;

            try
            {
                //Image = FaceRecognition.LoadImage(Bitmap);
                Image = FaceRecognition.LoadImageFile(imageFilePath);
            }
            catch (Exception e)
            {
                return false;
            }

            Locations = FaceRecognition.FaceLocations(Image).ToList();

            if (Locations.Any())
            {
                Encodings = FaceRecognition.FaceEncodings(Image, Locations).ToList();
            }

            IsLoaded = true;

            return true;
        }
    }
}