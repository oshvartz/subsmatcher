using SubCenterSubtitlesMatcher.SubtiltleFileSearcher;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using SubCenterSubtitlesMatcher.Parser;
using SubtitlesMatcher.Common.Results;
using System.Collections.Generic;
using System.Text;
using SubtitlesMatcher.Common.Parser;

namespace SubCenterSubtitlesMatcher.UnitTests
{
    
    
    /// <summary>
    ///This is a test class for SubCenterSubtitleSearcherTest and is intended
    ///to contain all SubCenterSubtitleSearcherTest Unit Tests
    ///</summary>
    [TestClass()]
    public class SubCenterSubtitleSearcherTest
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
        public void FindTvShowSubsTest()
        {
            SubCenterSubtitleSearcher target = new SubCenterSubtitleSearcher(Encoding.Default);
            var mediaFileInfo = new TvShowMediaFileInfo()
            {
                TitleName = "fringe",
                Episode = 18,
                Season = 2,
                VersionName = "LOL"
                
            };
            var res = target.Find(mediaFileInfo);
            
            
        }


        [TestMethod()]
        public void FindMovieSubsTest()
        {
            SubCenterSubtitleSearcher target = new SubCenterSubtitleSearcher(Encoding.Default);
            var mediaFileInfo = new MediaFileInfo()
            {
                TitleName = "The Fourth Kind",
                
                VersionName = "DiAMOND"

            };
            var res = target.Find(mediaFileInfo);


        }
    }
}
