using GeoLocalizationBL;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using GeoLocalizationCL;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GeoLocalizationUT
{
    
    
    /// <summary>
    ///This is a test class for GeoQuerysTest and is intended
    ///to contain all GeoQuerysTest Unit Tests
    ///</summary>
    [TestClass()]
    public class GeoQuerysTest
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
            GeoQuerys target = new GeoQuerys(); // TODO: Initialize to an appropriate value
            Location pLocation = new Location("P1", double.Parse("52.2165425"), double.Parse("5.4778534")); // TODO: Initialize to an appropriate value
            int maxDistance = 10000; // TODO: Initialize to an appropriate value
            int maxResults = 100; // TODO: Initialize to an appropriate value
            SearchResults sr = new SearchResults();            
            SearchResults actual;

            sr.StartSearch("52.2165425", "5.4778534", maxDistance.ToString(), maxResults.ToString());
            actual = target.GetLocations(pLocation, maxDistance, maxResults, sr);
            actual.EndSearch("");

            Assert.AreEqual(maxResults.ToString(), actual.maxResults);            
        }
    }
}
