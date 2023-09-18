using ServiceLayer;
using ServiceLayer.Enumerations;
using ServiceLayer.Interfaces;
using ServiceLayer.Models;

namespace UnitTestTestWebCrawlerScraper.ServiceLayerTests
{
    public class LinkInspectorTest
    {
        ILinkInspector _linkInspector;

        public LinkInspectorTest()
        {
            _linkInspector = new LinkInspector();
        }


        [Theory]
        [InlineData("https://www.cnn.com", LinkStatus.Valid_InternalChildPage, true)]
        [InlineData("/thislin/test/value", LinkStatus.Valid_InternalChildPage, false)]
        [InlineData(null, LinkStatus.Invalid_Null, false)]
        [InlineData("/////", LinkStatus.Invalid_Null, false)]
        [InlineData("mailto://///", LinkStatus.Invalid_Mailto, false)]
        [InlineData("tel://///", LinkStatus.Invalid_Phone, false)]
        [InlineData("sms://///", LinkStatus.Invalid_Sms, false)]
        [InlineData("https://www.example.com", LinkStatus.Invalid_External, true)]

        public void ValidateLink_VariousLinkCases(string url, LinkStatus linkStatus, bool expectedQualification)
        {
            //Arrange
            string targetUriHost = "www.cnn.com";
            CrawledLinkInfo crawledLinkInfo = new CrawledLinkInfo()
            {
                AbsoluteLink = null,
                AbsoluteLinkStringFormatted = url,
                LinkStatus = LinkStatus.Found_ForValidation,
                IsLinkFullyQualified = false,
                Level = 1,
                OriginalLink = url,
                ParentPageUrl = url,
                WebSitePage = string.Empty,
                WebSitePageFullFileName = string.Empty,
                WebSitePageStatus = WebSitePageStatus.NotDownloaded
            };
            //Act
            CrawledLinkInfo actualResult = _linkInspector.ValidateLink(targetUriHost, crawledLinkInfo);

            //Assert
            bool isFullyQualified = actualResult.IsLinkFullyQualified == expectedQualification;
            Assert.True(isFullyQualified);
            Assert.Equal(linkStatus, actualResult.LinkStatus);
        }


        [Theory]
        [InlineData(null, false)]
        [InlineData("http", false)]
        [InlineData("https", false)]
        [InlineData("https://", false)]
        [InlineData("https://test", true)]
        public void IsLinkFullyQualified_VariousInputs(string rawLink, bool expectedResult)
        {
            //Act
            var actualResult = _linkInspector.IsLinkFullyQualified(rawLink);
            //Assert
            Assert.Equal(expectedResult, actualResult);
        }


        [Theory]
        [InlineData(null, false)]
        [InlineData("", false)]
        [InlineData(" ", false)]
        [InlineData("http", true)]
        [InlineData("https", true)]
        public void IsUriSchemeValid_VariousInputs(string uriScheme, bool expectedResult)
        {
            //Act
            var actualResult = _linkInspector.IsUriSchemeValid(uriScheme);
            //Assert
            Assert.Equal(expectedResult, actualResult);
        }

        [Theory]
        [InlineData(null, "")]

        public void GetParentUriString_NullInput(Uri parentUri, string expectedResult)
        {
            //Act
            var actualResult = _linkInspector.GetParentUriString(parentUri);
            //Assert
            Assert.Equal(expectedResult, actualResult);
        }

        [Theory]
        [InlineData("https://www.cnn.com?help=values?askus=here", "https://www.cnn.com")]
        public void GetParentUriString_VariousInputs(string parentUri, string expectedResult)
        {
            //Act
            var uri = new Uri(parentUri);
            var actualResult = _linkInspector.GetParentUriString(uri);
            //Assert
            Assert.Equal(expectedResult, actualResult);
        }

        [Theory]
        [InlineData(null, null, null)]
        [InlineData("https://www.cnn.com", "/test", "https://www.cnn.com/test")]
        [InlineData("https://www.cnn.com", "https://www.cnn.com/test", "https://www.cnn.com/www.cnn.com/test")]
        public void CreateAbsoluteUriLink_VariousInputs(string parentUrl, string childUrl, string expectedResult)
        {
            //Act
            var actualResult = _linkInspector.CreateAbsoluteUriLink(parentUrl, childUrl);
            string actualResultStr = (actualResult == null) ? null : actualResult.ToString();
            //Assert
            Assert.Equal(expectedResult, actualResultStr);
        }

        [Fact]
        public void IsAbsoluteUrlLinkUnique_SameParams_Returns_False()
        {
            //Arrange
            string urlLink = "https://www.cnn.com";
            CrawledLinkInfo crawledLinkInfo = new CrawledLinkInfo()
            {
                AbsoluteLink = new Uri(urlLink),

            };
            string _url = "http://www.cnn.com";
            int _pageNumber = 1;
            CrawlJob crawlJob = new CrawlJob(_url, _pageNumber)
            {
                Url = _url,
                Uri = new Uri(_url),
                PageNumber = _pageNumber,
            };
            //Act
            var actualResult = _linkInspector.IsAbsoluteUrlLinkUnique(crawledLinkInfo, crawlJob);
            //Assert
            Assert.True(!actualResult);

        }

        [Fact]
        public void IsAbsoluteUrlLinkUnique_DifferentParams_Returns_True()
        {
            //Arrange
            string urlLink = "https://www.Examople.com";
            CrawledLinkInfo crawledLinkInfo = new CrawledLinkInfo()
            {
                AbsoluteLink = new Uri(urlLink),

            };
            string _url = "http://www.cnn.com";
            int _pageNumber = 1;
            CrawlJob crawlJob = new CrawlJob(_url, _pageNumber)
            {
                Url = _url,
                Uri = new Uri(_url),
                PageNumber = _pageNumber,
            };
            //Act
            var actualResult = _linkInspector.IsAbsoluteUrlLinkUnique(crawledLinkInfo, crawlJob);
            //Assert
            Assert.True(actualResult);

        }

        [Fact]
        public void IsAbsoluteUriLinkStoredInPositiveResults_ReturnsFalse()
        {
            //Arrange
            SortedDictionary<string, CrawledLinkInfo> positiveResults = new SortedDictionary<string, CrawledLinkInfo>();
            positiveResults.Add("", new CrawledLinkInfo());

            string url = "https://www.cnn.com/";
            CrawledLinkInfo crawledLinkInfoFound = new CrawledLinkInfo()
            {
                AbsoluteLink = new Uri(url)
            };
            //Act
            var actualResult = _linkInspector.IsAbsoluteUriLinkStoredInPositiveResults(positiveResults, crawledLinkInfoFound);
            //Assert
            Assert.True(!actualResult);
        }

        [Fact]
        public void IsAbsoluteUriLinkStoredInPositiveResults_ReturnsTrue()
        {
            //Arrange
            string url = "https://www.cnn.com/";
            SortedDictionary<string, CrawledLinkInfo> positiveResults = new SortedDictionary<string, CrawledLinkInfo>();
            positiveResults.Add(url, new CrawledLinkInfo());


            CrawledLinkInfo crawledLinkInfoFound = new CrawledLinkInfo()
            {
                AbsoluteLink = new Uri(url)
            };
            //Act
            var actualResult = _linkInspector.IsAbsoluteUriLinkStoredInPositiveResults(positiveResults, crawledLinkInfoFound);
            //Assert
            Assert.True(actualResult);
        }



        [Fact]
        public void IsAbsoluteUriLinkStoredInFailedResults_ReturnsFalse()
        {
            //Arrange
            SortedDictionary<string, CrawledLinkInfo> failedResults = new SortedDictionary<string, CrawledLinkInfo>();
            failedResults.Add("", new CrawledLinkInfo());

            string url = "https://www.cnn.com/";
            CrawledLinkInfo crawledLinkInfoFound = new CrawledLinkInfo()
            {
                AbsoluteLink = new Uri(url)
            };
            //Act
            var actualResult = _linkInspector.IsAbsoluteUriLinkStoredInFailedResults(failedResults, crawledLinkInfoFound);
            //Assert
            Assert.True(!actualResult);
        }

        [Fact]
        public void IsAbsoluteUriLinkStoredInFailedResults_ReturnsTrue()
        {
            //Arrange
            string url = "https://www.cnn.com/";
            SortedDictionary<string, CrawledLinkInfo> failedResults = new SortedDictionary<string, CrawledLinkInfo>();
            failedResults.Add(url, new CrawledLinkInfo());


            CrawledLinkInfo crawledLinkInfoFound = new CrawledLinkInfo()
            {
                AbsoluteLink = new Uri(url)
            };
            //Act
            var actualResult = _linkInspector.IsAbsoluteUriLinkStoredInPositiveResults(failedResults, crawledLinkInfoFound);
            //Assert
            Assert.True(actualResult);
        }

        [Fact]
        public void IsAbsoluteUriLinkJobEnqueued_ReturnsTrue()
        {
            //Arrange
            string url = "https://www.cnn.com/";
            Queue<CrawlJob> jobQueues = new Queue<CrawlJob>();

            CrawlJob crawlJob = new CrawlJob(url, 1);

            jobQueues.Enqueue(crawlJob);


            CrawledLinkInfo crawledLinkInfoFound = new CrawledLinkInfo()
            {
                AbsoluteLink = new Uri(url)
            };
            //Act
            var actualResult = _linkInspector.IsAbsoluteUriLinkJobEnqueued(jobQueues, crawledLinkInfoFound);
            //Assert
            Assert.True(actualResult);
        }

        [Fact]
        public void IsAbsoluteUriLinkJobEnqueued_ReturnsFalse()
        {
            //Arrange
            string url1 = "https://www.cnn.com/";
            string url2 = "https://www.example.com";
            Queue<CrawlJob> jobQueues = new Queue<CrawlJob>();

            CrawlJob crawlJob = new CrawlJob(url1, 1);

            jobQueues.Enqueue(crawlJob);


            CrawledLinkInfo crawledLinkInfoFound = new CrawledLinkInfo()
            {
                AbsoluteLink = new Uri(url2)
            };
            //Act
            var actualResult = _linkInspector.IsAbsoluteUriLinkJobEnqueued(jobQueues, crawledLinkInfoFound);
            //Assert
            Assert.True(!actualResult);
        }
    }
}
