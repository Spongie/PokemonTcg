using System.Linq;

namespace Launcher
{
    public class LauncherCard
    {
        public string Name { get; set; }
        public string SetCode { get; set; }
        public string ImageUrl { get; set; }
        public bool Completed { get; set; }

        public string GetImageName()
        {
            return ImageUrl.Split('/').Last();
        }
    }
}
