using System.Collections.Generic;

namespace Common.Domain.Entities
{
    public class User
    {
        public User()
        {
            UserImages = new List<UserImage>();
        }

        public string Name { get; set; }

        public List<UserImage> UserImages { get; set; }
    }
}