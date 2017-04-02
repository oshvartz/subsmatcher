using SubtitlesMatcher.Server;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using SubtitlesMatcher.Common;
using System.Globalization;
using SubCenterSubtitlesMatcher;
using System.Diagnostics;
using System.Threading;
using System.ComponentModel.Composition.Hosting;
using SubtitlesMatcher.Common.Parser;
using SubCenterSubtitlesMatcher.Parser;

namespace SubtitleMatcher.Server.Unit.Tests
{
    
    
    /// <summary>
    ///This is a test class for SubtitlesMatcherMgrTest and is intended
    ///to contain all SubtitlesMatcherMgrTest Unit Tests
    ///</summary>
    [TestClass()]
    public class SubtitlesMatcherMgrTest
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
        ///A test for FindAndExtractSubtitles
        ///</summary>
        [TestMethod()]
        public void FindAndExtractSubtitlesTest()
        {

            

            SubtitlesMatcherMgr target = new SubtitlesMatcherMgr();
    
            
            target.OnSubsMatcherStatusChanged += new SubtitlesMatcher.Common.Interfaces.SubsRetrieveStatusEventHandler(target_OnSubsMatcherStatusChanged);
            ISubtitleMatcherProvider subtitleMatcherProvider = new SubCenterSubtitleMatcherProvider();
            IMediaFileNameParser parser = new MediaFileNameParser();
            string mediaFileName = @"E:\emuleDownloads\Lost.S06E13.The.Last.Recruit.HDTV.XviD-FQM.avi";
            CultureInfo language = new CultureInfo("he-IL");
            target.FindAndExtractSubtitles(parser,subtitleMatcherProvider, mediaFileName, language,null);
            Thread.Sleep(1000);
            
        }

        void target_OnSubsMatcherStatusChanged(SubtitlesMatcher.Common.Events.SubtitlesMatcherEventArgs subtitlesMatcherEventArgs)
        {
            Debug.WriteLine(subtitlesMatcherEventArgs.Status.ToString());
        }
    }
}
