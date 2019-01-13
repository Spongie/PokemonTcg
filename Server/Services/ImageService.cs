using System;
using System.IO;

namespace Server.Services
{
    public class ImageService : IService
    {
        public void InitTypes()
        {
            
        }

        public string LoadImage(string imageName)
        {
            byte[] imageBytes = File.ReadAllBytes(Path.Combine("Images", imageName));

            return Convert.ToBase64String(imageBytes, Base64FormattingOptions.None);
        }
    }
}
