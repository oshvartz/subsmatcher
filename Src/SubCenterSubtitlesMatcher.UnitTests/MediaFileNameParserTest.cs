using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using SubCenterSubtitlesMatcher.Parser;

namespace SubCenterSubtitlesMatcher.UnitTests
{
    
    
    /// <summary>
    ///This is a test class for MediaFileNameParserTest and is intended
    ///to contain all MediaFileNameParserTest Unit Tests
    ///</summary>
    [TestClass()]
    public class MediaFileNameParserTest
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
        ///A test for Parse
        ///</summary>
        [TestMethod()]
        public void ParseTvShowTest()
        {
            MediaFileNameParser target = new MediaFileNameParser();
            string mediaFileName = "House.S06E13.HDTV.XviD-XII.avi";//"The.Simpsons.S21E17.HDTV.XviD - LOL.avi"; 
            var res = target.Parse(mediaFileName,false);
            
        }

        [TestMethod()]
        public void ParseMovieTest()
        {
            MediaFileNameParser target = new MediaFileNameParser();
            string mediaFileName = "Shutter Island (2010) R5 DVDRip XviD-MAXSPEED";//"The.Simpsons.S21E17.HDTV.XviD - LOL.avi"; 
            var res = target.Parse(mediaFileName, false);

        }
    }
}
