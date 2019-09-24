using DrivingData;
using DrivingData.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Tests
{
    [TestClass]
    public class TestSuite1
    {
        ReportService rs;
        TextFileService tfs;
        UserDataCollectionService udcs;

        [TestInitialize]
        public void Init()
        {
            // This will work for now but really ought to have DI/IoC.
            // As both of the bottom ones depend on the same instance of the top, for now, meh.
            udcs = new UserDataCollectionService();
            rs = new ReportService(udcs);
            tfs = new TextFileService(udcs);
        }

        [TestMethod]
        public void TestReportWithHardcodedData()
        {
            var human = new Driver("HumanBeing");
            udcs.RegisterDriver(human);
            
            var consistentTime = DateTime.Now;
            udcs.RegisterTrip(human, consistentTime, consistentTime.AddMinutes(60), 40);

            Assert.AreEqual("HumanBeing: 40 miles @ 40 mph", rs.GenerateReport());
        }

        [TestMethod]
        public void TestReportSortsWithHardcodedData()
        {
            // Trip Dan 07:15 07:45 17.3
            // Trip Dan 06:12 06:32 21.8
            // Trip Lauren 12:01 13:16 42.0

            var dan = new Driver("Dan");
            udcs.RegisterDriver(dan);
            var lauren = new Driver("Lauren");
            udcs.RegisterDriver(lauren);

            udcs.RegisterTrip(dan, new DateTime(2019, 1, 1, 7, 15, 0), new DateTime(2019, 1, 1, 7, 45, 0), 17.3M);
            udcs.RegisterTrip(dan, new DateTime(2019, 1, 1, 6, 12, 0), new DateTime(2019, 1, 1, 6, 32, 0), 21.8M);
            udcs.RegisterTrip(lauren, new DateTime(2019, 1, 1, 12, 01, 0), new DateTime(2019, 1, 1, 13, 16, 0), 42);
            
            Assert.AreEqual("Lauren: 42 miles @ 34 mph\r\nDan: 39 miles @ 47 mph", rs.GenerateReport());
        }

        [TestMethod]
        public void TestParseSingleDriverCommand()
        {
            tfs.ProcessDriverCommand("Driver Dan");
            Assert.IsTrue(udcs.AllRegisteredDrivers.Exists(x => x.Name == "Dan"));
        }

        [TestMethod]
        public void TestParseSingleTripCommand()
        {
            udcs.RegisterDriver("Dan");
            tfs.ProcessTripCommand("Trip Dan 07:15 07:45 17.3");
            var list = udcs.GetDriverTripSummaries().ToList();
            Assert.IsTrue(list.Exists(x => x.DriverName == "Dan" && x.TotalDistance == 17.3M && x.TotalMinutes == 30));
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
