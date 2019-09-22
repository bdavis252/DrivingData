using DrivingData.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace DrivingData
{
    // So this isn't a real web API but if it were these would be service endpoints
    public static class Services
    {
        public static string GenerateReport(List<Trip> trips)
        {
            //TODO discard < 5 and > 100

            //Aggregate everything
            var driverTripSummaries = trips.GroupBy(t => t.DriverName).Select(g => new DriverTripSummary()
            {
                DriverName = g.Key,
                TotalDistance = g.Sum(t => t.MilesDriven),
                TotalMinutes = Convert.ToInt32(g.Sum(t => t.EndTime.Subtract(t.StartTime).TotalMinutes)) // convert the sum rather than each item
            }).OrderByDescending(dts => dts.TotalDistance);
            
            //Generate Report
            var sb = new StringBuilder();
            int count = driverTripSummaries.Count();
            foreach (var summary in driverTripSummaries.Select((x, i) => new { Value = x, Index = i }))
            {
                // Problem specified "to nearest int" so I'm using Convert.ToInt32 because it rounds and converts double to int in one step.
                // If this were real life, you better believe I'd be asking someone who cared which way to round these. 
                // Manual says:
                // "rounded to the nearest 32-bit signed integer. If value is halfway between two whole numbers, 
                // the even number is returned; that is, 4.5 is converted to 4, and 5.5 is converted to 6."
                var miles = Convert.ToInt32(summary.Value.TotalDistance);
                var mph = Convert.ToInt32(summary.Value.TotalDistance / (summary.Value.TotalMinutes/60M));

                sb.AppendFormat("{0}: {1} miles @ {2} mph", summary.Value.DriverName, miles, mph);

                //new lines except at the end
                if (summary.Index < count-1)
                {
                    sb.AppendLine();
                }
            }

            return sb.ToString();
        }
    }
}
