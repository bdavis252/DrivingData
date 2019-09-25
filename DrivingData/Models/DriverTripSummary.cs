using System;

namespace DrivingData.Models
{
    public class DriverTripSummary
    {
        public string DriverName { get; set; }
        public decimal TotalDistance { get; set; }
        public int TotalMinutes { get; set; } // I used int here because times are always given in whole minutes already

        public decimal GetMph()
        {
            //dividing by 0 is cool, but...
            if (TotalMinutes == 0)
            {
                return 0;
            }
            else
            {
                return TotalDistance / (TotalMinutes / 60M);
            }
        }

        // Problem specified "to nearest int" so I'm using Convert.ToInt32 because it rounds and converts double to int in one step.
        // If this were real life, you better believe I'd be asking someone who cared which way to round these.
        // For now, "If the problem statement doesn't specify something, you can make any decision that you want."
        // Manual says:
        // "rounded to the nearest 32-bit signed integer. If value is halfway between two whole numbers, 
        // the even number is returned; that is, 4.5 is converted to 4, and 5.5 is converted to 6."
        public int getRoundedDistance()
        {
            return Convert.ToInt32(TotalDistance);
        }

        public int getRoundedMph()
        {
            return Convert.ToInt32(GetMph());
        }
    }
}
