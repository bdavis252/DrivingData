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
        ReportService rs;
        TextFileService ts;
        
        [TestInitialize]
        public void Init()
        {
            //This will work for now but needs DI/IoC
            rs = new ReportService();
            ts = new TextFileService();
        }

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

            Assert.AreEqual("HumanBeing: 40 miles @ 40 mph", rs.GenerateReport(trips));
        }

        [TestMethod]
        public void TestReportSortsWithHardcodedData()
        {
            // Trip Dan 07:15 07:45 17.3
            // Trip Dan 06:12 06:32 21.8
            // Trip Lauren 12:01 13:16 42.0
            
            var trips = new List<Trip>();
            trips.Add(new Trip()
            {
                DriverName = "Dan",
                StartTime = new DateTime(2019, 1, 1, 7, 15, 0),
                EndTime = new DateTime(2019, 1, 1, 7, 45, 0),
                MilesDriven = 17.3M
            });
            trips.Add(new Trip()
            {
                DriverName = "Dan",
                StartTime = new DateTime(2019, 1, 1, 6, 12, 0),
                EndTime = new DateTime(2019, 1, 1, 6, 32, 0),
                MilesDriven = 21.8M
            });
            trips.Add(new Trip()
            {
                DriverName = "Lauren",
                StartTime = new DateTime(2019, 1, 1, 12, 01, 0),
                EndTime = new DateTime(2019, 1, 1, 13, 16, 0),
                MilesDriven = 42
            });

            var debug = rs.GenerateReport(trips);
            Assert.AreEqual("Lauren: 42 miles @ 34 mph\r\nDan: 39 miles @ 47 mph", rs.GenerateReport(trips));
        }

        [TestMethod]
        public void TestReadingFileWithSingleDriverCommand()
        {

        }

        [TestMethod]
        public void TestReadingFileWithSingleTripCommand()
        {

        }

        //TODO test the discard of < 5 or > 100

        //TODO handle the case when there is a driver with no trips (print the 0)

        //TODO what about duplicate driver registrations
        //TODO make a test to handle input for ppl that aren't registered?
        //TODO make a test to handle junk time/distance inputs?

        //TODO way later test 24 hr input in text file?
    }
}
