using DomainLayer;
using DomainLayer.Utils.Interfaces;

namespace UnitTestTestWebCrawlerScraper.DomainLayerTests
{
    public class HtmlParserTest
    {
        IHtmlParser _htmlParser;
        public HtmlParserTest()
        {
            _htmlParser = new HtmlParser();
        }

        [Fact]
        public void GetLinks_ValidInput_ReturnsOkLinks()
        {
            //Arrange
            string htmlContent = @"<html> 
                                  <a href=""https://www.cnn.com""> Go to Here </a> 
                                  <a href=""mailto:contact@html.com""> get in touch</a> 
                                  <a href=""#Specify_a_Hyperlink_Target_href"">This first anchor element</a>
                                 </html>";
            //Act
            List<string> actualResult = _htmlParser.GetLinks(htmlContent);
            int total = actualResult.Count;
            //Assert
            Assert.True(total == 3);
        }

        [Fact]
        public void GetLinks_ValidInputNoAnchorTags_Returns_O_Links()
        {
            //Arrange
            string htmlContent = "<html class=\"js no-flash geolocation websockets localstorage webworkers no-touchevents fontface supports textshadow csscolumns csscolumns-width csscolumns-span csscolumns-fill csscolumns-gap csscolumns-rule csscolumns-rulecolor csscolumns-rulestyle csscolumns-rulewidth csscolumns-breakbefore csscolumns-breakafter csscolumns-breakinside flexbox csstransforms3d no-mobile no-phone no-tablet mobilegradea no-gdpr userconsent-cntry-mx userconsent-reg-global no-ios no-android no-iospre10 no-iemobile no-ieunsupported no-ie11unsupported no-ie no-edge csspositionstickysupport\" style=\"\" data-formfactor=\"desktop\">\r\n<head><script type=\"text/javascript\" src=\"https://consumer.krxd.net/consent/set/f3b6d00d-676f-48d8-80ef-2b48af61105e?idt=device&amp;dt=kxcookie&amp;dc=1&amp;al=1&amp;tg=1&amp;cd=1&amp;sh=1&amp;re=1&amp;callback=Krux.ns._default.kxjsonp_consent_set_1\"></script><script type=\"text/javascript\" src=\"https://cdn.krxd.net/userdata/get?pub=f3b6d00d-676f-48d8-80ef-2b48af61105e&amp;technographics=1&amp;callback=Krux.ns._default.kxjsonp_userdata\"></script><script type=\"text/javascript\" src=\"https://beacon.krxd.net/optout_check?callback=Krux.ns._default.kxjsonp_optOutCheck\"></script><script type=\"text/javascript\" async=\"\" src=\"https://static.criteo.net/js/ld/publishertag.prebid.130.js\"></script><script type=\"text/javascript\" src=\"https://consumer.krxd.net/consent/get/f3b6d00d-676f-48d8-80ef-2b48af61105e?idt=device&amp;dt=kxcookie&amp;callback=Krux.ns._default.kxjsonp_consent_get_0\"></script><script async=\"\" src=\"//cdn.krxd.net/ctjs/controltag.js.d58f47095e6041e576ee04944cca45da\"></script><script type=\"text/javascript\" async=\"\" src=\"//www.ugdturner.com/xd.sjs\"></script><script async=\"\" src=\"//static.adsafeprotected.com/iasPET.1.js\"></script><script async=\"\" src=\"//c.amazon-adsystem.com/aax2/apstag.js\"></script><script src=\"https://rules.quantcount.com/rules-p-D1yc5zQgjmqr5.js\" async=\"\"></script><script async=\"\" src=\"https://z.cdp-dev.cnn.com/sp/current/zion-sp.js\"></script><script type=\"text/javascript\" async=\"\" src=\"//www.i.cdn.cnn.com/zion/zion-mb.min.js\"></script><script async=\"\" src=\"https://cdn.boomtrain.com/p13n/cnn/p13n.min.js\"></script><script id=\"GPTScript\" type=\"text/javascript\" src=\"https://securepubads.g.doubleclick.net/tag/js/gpt.js\"></script><script type=\"text/javascript\" async=\"\" src=\"https://sb.scorecardresearch.com/beacon.js\"></script><script type=\"text/javascript\" async=\"\" src=\"//s.cdn.turner.com/analytics/comscore/streamsense.5.2.0.160629.min.js\"></script><script async=\"\" src=\"https://cdn.ml314.com/taglw.js\"></script><meta content=\"IE=edge,chrome=1\" http-equiv=\"X-UA-Compatible\"><meta charset=\"utf-8\"><meta content=\"text/html\" http-equiv=\"Content-Type\"><meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0, minimum-scale=1.0\"><link rel=\"dns-prefetch\" href=\"/optimizelyjs/128727546.js\"><link rel=\"dns-prefetch\" href=\"//tpc.googlesyndication.com\"><link rel=\"dns-prefetch\" href=\"//pagead2.googlesyndication.com\"><link rel=\"dns-prefetch\" href=\"//www.googletagservices.com\"><link rel=\"dns-prefetch\" href=\"//partner.googleadservices.com\"><link rel=\"dns-prefetch\" href=\"//www.google.com\"><link rel=\"dns-prefetch\" href=\"//aax.amazon-adsystem.com\"><link rel=\"dns-prefetch\" href=\"//c.amazon-adsystem.com\"><link rel=\"dns-prefetch\" href=\"//cdn.krxd.net\"><link rel=\"dns-prefetch\" href=\"//ads.rubiconproject.com\"><link rel=\"dns-prefetch\" href=\"//optimized-by.rubiconproject.com\"><link rel=\"dns-prefetch\" href=\"//fastlane.rubiconproject.com\"><link rel=\"dns-prefetch\" href=\"//fastlane-adv.rubiconproject.com\"><link rel=\"dns-prefetch\" href=\"”//ib.adnxs.com”/\"><link rel=\"dns-prefetch\" href=\"”//prebid.adnxs.com”/\"><link rel=\"dns-prefetch\" href=\"//segment-data-us-east.zqtk.net/turner-47fcf6\"><link rel=\"dns-prefetch\" href=\"//w.usabilla.com\"><link rel=\"dns-prefetch\" href=\"\"><link rel=\"dns-prefetch\" href=\"//data.api.cnn.io\"><link rel=\"dns-prefetch\" href=\"//pmd.cdn.turner.com\"><link rel=\"dns-prefetch\" href=\"//amd.cdn.turner.com\"><link rel=\"dns-prefetch\" href=\"//ht.cdn.turner.com\"><link rel=\"dns-prefetch\" href=\"//www.ugdturner.com\"><link rel=\"dns-prefetch\" href=\"//vrt.outbrain.com\"><link rel=\"dns-prefetch\" href=\"//consent.truste.com\"><link rel=\"dns-prefetch\" href=\"//as.casalemedia.com\"><link rel=\"dns-prefetch\" href=\"//as-sec.casalemedia.com\"><link rel=\"dns-prefetch\" href=\"//dsum-sec.casalemedia.com\"><link rel=\"dns-prefetch\" href=\"//js-sec.indexww.com\"><link rel=\"dns-prefetch\" href=\"//data.cnn.com\"><link rel=\"dns-prefetch\" href=\"//cdn.cnn.com\"><link rel=\"dns-prefetch\" href=\"//edition.i.cdn.cnn.com\"><link rel=\"preload\" href=\"//cdn.cnn.com/ads/cnni/cnni_homepage.json\" as=\"fetch\" type=\"application/json\" crossorigin=\"anonymous\">\r\n<link rel=\"preload\" href=\"/.a/bundles/header.a08d286bea3922ee8f5f.bundle.js\" as=\"script\" type=\"application/javascript\">\r\n<link rel=\"preload\" href=\"https://www.googletagservices.com/tag/js/gpt.js\" as=\"script\" type=\"text/javascript\">\r\n<link rel=\"preload\" href=\"https://c.amazon-adsystem.com/aax2/apstag.js\" as=\"script\" type=\"application/javascript\">\r\n<link rel=\"preload\" href=\"/.a/2.308.1/js/cnn-header-second-react.min.js\" as=\"script\" type=\"application/javascript\">\r\n<link rel=\"preload\" href=\"/optimizelyjs/128727546.js\" as=\"script\" type=\"text/javascript\">\r\n<link rel=\"preload\" href=\"https://cdn.cookielaw.org/scripttemplates/otSDKStub.js\" as=\"script\" type=\"application/javascript\">\r\n<link rel=\"preload\" href=\"//edition.i.cdn.cnn.com/.a/fonts/cnn/3.9.0/cnnsans-regular.woff2\" as=\"font\" type=\"font/woff2\" crossorigin=\"anonymous\">\r\n<link rel=\"preload\" href=\"//edition.i.cdn.cnn.com/.a/fonts/cnn/3.9.0/cnnsans-lightit.woff2\" as=\"font\" type=\"font/woff2\" crossorigin=\"anonymous\">\r\n<link rel=\"preload\" href=\"//edition.i.cdn.cnn.com/.a/fonts/cnn/3.9.0/cnnsans-italic.woff2\" as=\"font\" type=\"font/woff2\" crossorigin=\"anonymous\">\r\n<link rel=\"preload\" href=\"//edition.i.cdn.cnn.com/.a/fonts/icons/2.4.10/cnn-icons.woff2\" as=\"font\" type=\"font/woff2\" crossorigin=\"anonymous\">\r\n<link rel=\"preload\" href=\"//edition.i.cdn.cnn.com/.a/fonts/cnn/3.9.0/cnnsans-medium.woff2\" as=\"font\" type=\"font/woff2\" crossorigin=\"anonymous\">\r\n<link rel=\"preload\" href=\"//edition.i.cdn.cnn.com/.a/fonts/cnn/3.9.0/cnnsans-bold.woff2\" as=\"font\" type=\"font/woff2\" crossorigin=\"anonymous\">\r\n<link rel=\"preload\" href=\"//lightning.cnn.com/launch/7be62238e4c3/97fa00444124/launch-2878c87af5e3.min.js\" as=\"script\" type=\"application/x-javascript\">\r\n<link rel=\"preload\" href=\"/.a/2.308.1/js/cnn-footer-lib-react.min.js\" as=\"script\" type=\"application/javascript\">\r\n<link rel=\"preload\" href=\"//amplify.outbrain.com/cp/obtp.js\" as=\"script\" type=\"application/x-javascript\">\r\n<link href=\"/favicon.ie9.ico\" rel=\"Shortcut Icon\" type=\"image/x-icon\">\r\n<link href=\"//cdn.cnn.com/cnn/.e/img/3.0/global/misc/apple-touch-icon.png\" rel=\"apple-touch-icon\" type=\"image/png\"><!--[if lte IE 9]><meta http-equiv=\"refresh\" content=\"1;url=/2.308.1/static/unsupp.html\" />\r\n\r\n<![endif]-->";

            //Act
            List<string> actualResult = _htmlParser.GetLinks(htmlContent);
            int total = actualResult.Count;
            //Assert
            Assert.True(total == 0);
        }

        [Theory]
        [InlineData("", 0)]
        [InlineData(null, 0)]
        public void GetLinks_VariousCases(string htmlContent, int expectedTotal)
        {
            //Act
            List<string> actualResult = _htmlParser.GetLinks(htmlContent);
            int total = actualResult.Count;
            //Assert
            Assert.True(total == expectedTotal);
        }

    }
}
