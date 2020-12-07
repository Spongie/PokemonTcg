namespace TCGCards.Core
{
    public class VersionNumber
    {
        public VersionNumber()
        {

        }

        public VersionNumber(string version)
        {
            var numbers = version.Split('.');

            Major = int.Parse(numbers[0]);
            Minor = int.Parse(numbers[1]);
            Build = int.Parse(numbers[2]);
        }

        public int Major { get; set; }
        public int Minor { get; set; }
        public int Build { get; set; }

        public static bool operator>(VersionNumber versionA, VersionNumber versionB)
        {
            if (versionA.Major > versionB.Major)
            {
                return true;
            }
            else if (versionA.Minor > versionB.Minor)
            {
                return true;
            }
            else if (versionA.Build > versionB.Build)
            {
                return true;
            }

            return false;
        }

        public static bool operator<(VersionNumber versionA, VersionNumber versionB)
        {
            if (versionA.Major < versionB.Major)
            {
                return true;
            }
            else if (versionA.Minor < versionB.Minor)
            {
                return true;
            }
            else if (versionA.Build < versionB.Build)
            {
                return true;
            }

            return false;
        }

        public override string ToString()
        {
            return $"{Major}.{Minor}.{Build}";
        }
    }
}
