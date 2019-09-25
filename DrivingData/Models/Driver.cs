namespace DrivingData.Models
{
    public class Driver
    {
        public Driver(string name)
        {
            Name = name;
        }

        public string Name { get; set; }

        // This is useful to speed up the .Except because it can do hashtables 
        // ... though ain't nobody got time for nonunique string comparisons
        // override object.Equals
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            return Name == ((Driver) obj).Name;
        }

        // override object.GetHashCode
        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }
    }
}
