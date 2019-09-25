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
                sb.AppendFormat("{0}: {1} miles", summary.Value.DriverName, summary.Value.getRoundedDistance());

                int mph = Convert.ToInt32(summary.Value.GetMph());
                if (mph > 0)
                {
                    sb.AppendFormat(" @ {0} mph", mph);
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
