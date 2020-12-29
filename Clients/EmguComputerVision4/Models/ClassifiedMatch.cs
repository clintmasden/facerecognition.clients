using System.Drawing;
using Common.Domain.Entities;

namespace EmguComputerVision4.Models
{
    public class ClassifiedMatch
    {
        public User User { get; set; }

        public Rectangle Location { get; set; }

        public double Distance { get; set; }

        public bool IsCenter { get; set; }
    }
}