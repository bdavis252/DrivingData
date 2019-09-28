using DrivingData.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DrivingData
{
    //The purpose of this class is to handle calculations and rounding and stuff.
    //If this were a real Web API, it would not be a service endpoint. 
    //I'd probably pull it out to be its own project and make it a dependency of other projects that would need it.
    public class BusinessService
    {
        public int GetMinutesElapsed(Trip t)
        {
            return Convert.ToInt32(t.EndTime.Subtract(t.StartTime).TotalMinutes);
        }

        internal decimal GetMph(decimal distance, decimal minutes)
        {
            //dividing by 0 is cool, but...
            if (minutes == 0)
            {
                return 0;
            }
            else
            {
                return distance / (minutes / 60M);
            }
        }

        // Problem specified "to nearest int" so I'm using Convert.ToInt32 because it rounds and converts double to int in one step.
        // If this were real life, you better believe I'd be asking someone who cared which way to round these.
        // For now, "If the problem statement doesn't specify something, you can make any decision that you want."
        internal int GetRoundedMph(decimal distance, decimal minutes)
        {
            // Manual says:
            // "rounded to the nearest 32-bit signed integer. If value is halfway between two whole numbers, 
            // the even number is returned; that is, 4.5 is converted to 4, and 5.5 is converted to 6."
            return Convert.ToInt32(GetMph(distance, minutes));
        }

        internal int GetRoundedDistance(decimal distance)
        {
            return Convert.ToInt32(distance);
        }
    }
}
