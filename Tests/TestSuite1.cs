using DrivingData;
using DrivingData.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Tests
{
    [TestClass]
    public class TestSuite1
    {
        [TestMethod]
        public void TestReportWithHardcodedData()
        {
            var consistentTime = DateTime.Now;

            var trips = new List<Trip>();
            trips.Add(new Trip() {
                DriverName = "HumanBeing",
                StartTime = consistentTime,
                EndTime = consistentTime.AddMinutes(60),
                MilesDriven = 40
            });

            Assert.AreEqual("HumanBeing: 40 miles @ 1 mph", Services.GenerateReport(trips));
        }
    }
}
