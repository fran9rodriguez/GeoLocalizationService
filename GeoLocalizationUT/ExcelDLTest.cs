using GeoLocalizationDL;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using GeoLocalizationCL;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace GeoLocalizationUT
{
    
    
    /// <summary>
    ///This is a test class for ExcelDLTest and is intended
    ///to contain all ExcelDLTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ExcelDLTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for GetLocations
        ///</summary>
        [TestMethod()]
        public void GetLocationsTest()
        {
            ExcelDL target = new ExcelDL(); // TODO: Initialize to an appropriate value
            List<Location> expected = null; // TODO: Initialize to an appropriate value
            List<Location> actual;
            actual = target.GetLocationsLinqToExcel();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetLocationsOldbConnection
        ///</summary>
        [TestMethod()]
        public void GetLocationsOldbConnectionTest()
        {
            ExcelDL target = new ExcelDL(); // TODO: Initialize to an appropriate value
            DataView expected = null; // TODO: Initialize to an appropriate value
            DataView actual;
            actual = target.GetLocationsOldbConnection();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for getAllLocations
        ///</summary>
        [TestMethod()]
        public void getAllLocationsTest()
        {
            ExcelDL target = new ExcelDL(); // TODO: Initialize to an appropriate value
            string filename = @"E:\Fran\Projects\GeoLocalizationService\GeoLocalizationDL\Data\locations.csv"; // TODO: Initialize to an appropriate value
            List<Location> expected = null; // TODO: Initialize to an appropriate value
            List<Location> actual;
            actual = target.getAllLocationsFileReaderLinq(filename);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for getAllLocations
        ///</summary>
        [TestMethod()]
        public void getAllLocationsTest_MediumSize()
        {
            ExcelDL target = new ExcelDL(); // TODO: Initialize to an appropriate value
            string filename = @"E:\Fran\Projects\GeoLocalizationService\GeoLocalizationDL\Data\locationsMedium.csv"; // TODO: Initialize to an appropriate value
            List<Location> expected = null; // TODO: Initialize to an appropriate value
            List<Location> actual;
            actual = target.getAllLocationsFileReaderLinq(filename);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for getAllLocations
        ///</summary>
        [TestMethod()]
        public void getAllLocationsTest_BigSize()
        {
            ExcelDL target = new ExcelDL(); // TODO: Initialize to an appropriate value
            string filename = @"E:\Fran\Projects\GeoLocalizationService\GeoLocalizationDL\Data\locationsBig.csv"; // TODO: Initialize to an appropriate value
            List<Location> expected = null; // TODO: Initialize to an appropriate value
            List<Location> actual;
            actual = target.getAllLocationsFileReaderLinq(filename);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for geLocationsByRange
        ///</summary>
        [TestMethod()]
        public void geLocationsByRangeTest()
        {
            ExcelDL target = new ExcelDL(); // TODO: Initialize to an appropriate value
            string filename = @"E:\Fran\Projects\GeoLocalizationService\GeoLocalizationDL\Data\locationsBig.csv";
            int index = 0; // TODO: Initialize to an appropriate value
            int offset = 100000; // TODO: Initialize to an appropriate value
            Task<List<Location>> expected = null; // TODO: Initialize to an appropriate value
            Task<List<Location>> actual;
            actual = target.geLocationsByRange(filename, index, offset);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for getAllLocationsParallel
        ///</summary>
        [TestMethod()]
        public void getAllLocationsParallelTest()
        {
            ExcelDL target = new ExcelDL(); // TODO: Initialize to an appropriate value
            string filename = @"E:\Fran\Projects\GeoLocalizationService\GeoLocalizationDL\Data\locationsBig.csv";
            List<Location> expected = null; // TODO: Initialize to an appropriate value
            List<Location> actual;
            actual = target.getAllLocationsStreamParallel(filename);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
