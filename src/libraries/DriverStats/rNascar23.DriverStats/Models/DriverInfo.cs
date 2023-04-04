namespace rNascar23.DriverStatistics.Models
{
    public class DriverInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string FirstName
        {
            get
            {
                var names = Name.Trim().Split(' ');
                if (names.Length == 2)
                    return names[0];
                else
                    return Name;
            }
        }

        public string LastName
        {
            get
            {
                var names = Name.Trim().Split(' ');
                if (names.Length == 2)
                    return names[1];
                if (names.Length == 3) // ex. Joe Graf Jr
                    return $"{names[1]} {names[2]}";
                else
                    return names[names.Length - 1];
            }
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
