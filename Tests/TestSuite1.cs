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
        BusinessService bs; //ha ha
        ReportService rs;
        TextFileService tfs;
        UserDataCollectionService udcs;

        [TestInitialize]
        public void Init()
        {
            // This will work for now but really ought to have DI/IoC.
            bs = new BusinessService();
            udcs = new UserDataCollectionService(bs);
            rs = new ReportService(bs, udcs);
            tfs = new TextFileService(udcs);
        }

        [TestMethod]
        public void TestDriverRegistration()
        {
            var human = new Driver("HumanBeing");
            udcs.CheckDriverThenRegister(human);

            Assert.IsTrue(udcs.AllRegisteredDrivers.Contains(human));
        }

        // YAGNI for this test but...
        //[TestMethod]
        //public void TestDoubleRegistration()
        //{
        //    var human = new Driver("HumanBeing");
        //    udcs.CheckDriverThenRegister(human);
        //    udcs.CheckDriverThenRegister(human);

        //    Assert.AreEqual(udcs.AllRegisteredDrivers.Count(), 1);
        //}

        [TestMethod]
        public void TestReportWithHardcodedData()
        {
            var human = new Driver("HumanBeing");
            udcs.CheckDriverThenRegister(human);
            
            var consistentTime = DateTime.Now;
            udcs.CheckTripThenRegister(human, consistentTime, consistentTime.AddMinutes(60), 40);

            Assert.AreEqual("HumanBeing: 40 miles @ 40 mph", rs.GenerateReport());
        }

        [TestMethod]
        public void TestReportSortsByDistance()
        {
            // Driver Dan
            // Driver Lauren
            // Trip Dan 07:15 07:45 17.3
            // Trip Dan 06:12 06:32 21.8
            // Trip Lauren 12:01 13:16 42.0

            var dan = new Driver("Dan");
            udcs.CheckDriverThenRegister(dan);
            var lauren = new Driver("Lauren");
            udcs.CheckDriverThenRegister(lauren);

            udcs.CheckTripThenRegister(dan, new DateTime(2019, 1, 1, 7, 15, 0), new DateTime(2019, 1, 1, 7, 45, 0), 17.3M);
            udcs.CheckTripThenRegister(dan, new DateTime(2019, 1, 1, 6, 12, 0), new DateTime(2019, 1, 1, 6, 32, 0), 21.8M);
            udcs.CheckTripThenRegister(lauren, new DateTime(2019, 1, 1, 12, 01, 0), new DateTime(2019, 1, 1, 13, 16, 0), 42);
            
            //Lauren comes first even though Dan entered first
            Assert.AreEqual("Lauren: 42 miles @ 34 mph\r\nDan: 39 miles @ 47 mph", rs.GenerateReport());
        }

        [TestMethod]
        public void TestParseSingleDriverCommand()
        {
            tfs.ParseDriverCommand("Driver Dan");
            Assert.IsTrue(udcs.AllRegisteredDrivers.Exists(x => x.Name == "Dan"));
        }

        [TestMethod]
        public void TestParseSingleTripCommand()
        {
            udcs.CheckDriverThenRegister("Dan");
            tfs.ProcessTripCommand("Trip Dan 07:15 07:45 17.3");
            var list = udcs.GetDriverTripSummaries().ToList();
            Assert.IsTrue(list.Exists(x => x.DriverName == "Dan" && x.TotalDistance == 17.3M && x.TotalMinutes == 30));
        }

        // I would definitely write several tests like the following, if the problem statement didn't guarantee good input
        // "The line will be space delimited with the following fields: the command (Trip), driver name, start time, stop time, miles driven."
        //[TestMethod]
        //public void TestProgramDealsWithMalformedInput()
        //{
        //    udcs.CheckDriverThenRegister("Dan");
        //    tfs.ProcessTripCommand("Trip Dan 07:15 17.3");
        //    var list = udcs.GetDriverTripSummaries().ToList();
        //    Assert.IsTrue(list.Exists(x => x.DriverName == "Dan" && x.TotalDistance == 17.3M && x.TotalMinutes == 30));
        //}

        [TestMethod]
        public void TestReadingFileWithSingleDriverCommand()
        {
            tfs.ReadAndProcessTextFile("ExampleTextFiles/SingleDriver.txt");
            Assert.IsTrue(udcs.AllRegisteredDrivers.Exists(x => x.Name == "Dan"));
        }

        [TestMethod]
        public void TestReadingFileWithSingleTripCommand()
        {
            tfs.ReadAndProcessTextFile("ExampleTextFiles/SingleTrip.txt");
            var list = udcs.GetDriverTripSummaries().ToList();
            Assert.IsTrue(list.Exists(x => x.DriverName == "Dan" && x.TotalDistance == 17.3M && x.TotalMinutes == 30));
        }

        [TestMethod]
        public void TestCaseWhenDriverWithNoTrips()
        {
            tfs.ReadAndProcessTextFile("ExampleTextFiles/ExampleFromRoot.txt");

            Assert.AreEqual("Lauren: 42 miles @ 34 mph\r\nDan: 39 miles @ 47 mph\r\nKumi: 0 miles", rs.GenerateReport());
        }

        [TestMethod]
        public void TestCaseSkipWalkingAndAirplanes()
        {
            //Keep Lauren's the same from the Root Example, but skip Dan.
            tfs.ReadAndProcessTextFile("ExampleTextFiles/SlowTripsAndFastTrips.txt");

            Assert.AreEqual("Lauren: 42 miles @ 34 mph\r\nDan: 0 miles\r\nKumi: 0 miles", rs.GenerateReport());
        }

        [TestMethod]
        public void TestEdgeCase1()
        {
            //Tests that it can read all the way from midnight to midnight
            tfs.ReadAndProcessTextFile("ExampleTextFiles/EdgeCase1.txt");

            Assert.AreEqual("Dan: 1200 miles @ 50 mph", rs.GenerateReport());
        }

        [TestMethod]
        public void TestEdgeCase2()
        {
            //Tests that it can read something with literally 0 distance travelled
            tfs.ReadAndProcessTextFile("ExampleTextFiles/EdgeCase2.txt");

            Assert.AreEqual("Dan: 0 miles", rs.GenerateReport());
        }

        //This would need more direction to deal with it. 
        //- I could see the argument for just registering the driver if a trip is processed for an unregistered driver. 
        //- However, it could be that if they're not registered they don't have an access token so they'd get 401'd.
        //- Or the app just wouldn't let them submit until they registered.
        //Since "If the problem statement doesn't specify something, you can make any decision that you want," I'm going to 
        //rule serious error handling beyond the scope of this project.
        //[TestMethod]
        //public void TestEdgeCase3()
        //{
        //    //TODO handle input for ppl that aren't registered
        //    tfs.ReadAndProcessTextFile("ExampleTextFiles/EdgeCase3.txt");

        //    Assert.AreEqual("Trip submitted for unregistered driver.", rs.GenerateReport());
        //}

        //Also, I thought of this while testing and would ask up to see if whoever consumes the report would want this
        //[TestMethod]
        //public void TestReportDropsMilesForUnitaryMile()
        //{
        //    //Tests that it can read something with literally 0 distance travelled
        //    tfs.ReadAndProcessTextFile("ExampleTextFiles/TextServiceCheck.txt");

        //    Assert.AreEqual("Dan: 1 mile", rs.GenerateReport());
        //}
        
    }
}
