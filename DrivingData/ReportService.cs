using DrivingData.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace DrivingData
{
    // So this isn't a real web API but if it were these would be service endpoints
    public class ReportService
    {
        private UserDataCollectionService udcs;

        public ReportService(UserDataCollectionService udcs)
        {
            this.udcs = udcs;
        }

        public string GenerateReport()
        {
            var driverTripSummaries = udcs.GetDriverTripSummaries(); 
            
            //Generate Report
            var sb = new StringBuilder();
            int count = driverTripSummaries.Count();
            foreach (var summary in driverTripSummaries.Select((x, i) => new { Value = x, Index = i }))
            {
                // Problem specified "to nearest int" so I'm using Convert.ToInt32 because it rounds and converts double to int in one step.
                // If this were real life, you better believe I'd be asking someone who cared which way to round these.
                // For now, "If the problem statement doesn't specify something, you can make any decision that you want."
                // Manual says:
                // "rounded to the nearest 32-bit signed integer. If value is halfway between two whole numbers, 
                // the even number is returned; that is, 4.5 is converted to 4, and 5.5 is converted to 6."
                var miles = Convert.ToInt32(summary.Value.TotalDistance);
                sb.AppendFormat("{0}: {1} miles", summary.Value.DriverName, miles);

                //dividing by 0 is cool, but...
                if (summary.Value.TotalMinutes > 0)
                {
                    sb.AppendFormat(" @ {0} mph", Convert.ToInt32(summary.Value.TotalDistance / (summary.Value.TotalMinutes / 60M)));
                }
                
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
