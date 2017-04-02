using SubtitlesMatcher.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace SubtitlesMatcher.Infrastructure.Unit.Tests
{
    
    
    /// <summary>
    ///This is a test class for ZipExtractorTest and is intended
    ///to contain all ZipExtractorTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ZipExtractorTest
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
        ///A test for ExtractFiles
        ///</summary>
        [TestMethod()]
        public void ExtractFilesTest()
        {
            string zipFilePath = @"C:\Users\offir\Documents\Downloads\sub0ac056a9028e08ab0cbc4d4f99eda6603275.zip";
            string fileExtension = "srt";
            string outputPath = @"C:\Users\offir\Documents\Downloads\";
            ZipExtractor.ExtractFiles(zipFilePath, fileExtension, outputPath);
            
        }
    }
}
