using System.Drawing;
using Common.Domain.Entities;
using FaceRecognitionDotNet;

namespace FaceDotNet.Models
{
    public class ClassifiedMatch
    {
        public User User { get; set; }

        public FaceEncoding FaceEncoding { get; set; }

        public double Distance { get; set; }

        public Rectangle Location { get; set; }

        public bool IsCenter { get; set; }
    }
}