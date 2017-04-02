using SubCenterSubtitlesMatcher;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using SubtitlesMatcher.Common.Results;
using System.Collections.Generic;
using SubCenterSubtitlesMatcher.Parser;

namespace SubCenterSubtitlesMatcher.UnitTests
{
    
    
    /// <summary>
    ///This is a test class for SubCenterSubtitleMatcherProviderTest and is intended
    ///to contain all SubCenterSubtitleMatcherProviderTest Unit Tests
    ///</summary>
    [TestClass()]
    public class SubCenterSubtitleMatcherProviderTest
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
        ///A test for Find
        ///</summary>
        [TestMethod()]
        public void MatcherProviderFindTvShowTest()
        {
            SubCenterSubtitleMatcherProvider target = new SubCenterSubtitleMatcherProvider();
            string mediaFileName = @"E:\emuleDownloads\house.s06e16.hdtv.xvid-fqm.avi";
            MediaFileNameParser parser = new MediaFileNameParser();
            var actual = target.Find(parser.Parse(mediaFileName,false), new System.Globalization.CultureInfo("he-IL"));
            
        }
    }
}
