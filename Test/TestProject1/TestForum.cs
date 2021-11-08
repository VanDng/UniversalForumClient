using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversalForumClient.Core;
using UniversalForumClient.Http;
using Xunit;

namespace TestProject1
{
    public class TestForum : IDisposable
    {
        private HttpClientStub _httpClientStub;
        private HttpClient _httpClient;
        private Forum _forum;

        public TestForum()
        {
            _httpClientStub = new HttpClientStub();
            _httpClient = new HttpClient(_httpClientStub);

            _forum = new Forum(_httpClient, "99");
        }

        public void Dispose()
        {
            _httpClientStub.Dispose();
            _httpClient.Dispose();
        }

        private string TestDataPath(string relativeFilePath)
        {
            return Utility.SolutionPath() + @"\Test\TestData\Gvn\" + relativeFilePath;
        }

        [Fact]
        public async Task GetChildForums()
        {
            string[] expectedChildForumIds =
            {
                "covid",
                "gamevn-wiki.504",
                "thu-gian-express-ban-tin-cuoi-ngay.339",
                "an-choi-tiec-tung.85"
            };

            var testDataFilePath = TestDataPath("forum_html_source.html");
            var html_sourcce = File.ReadAllText(testDataFilePath);
            _httpClientStub.SetHttpResponse(html_sourcce);

            var childForums = await _forum.GetChildForums();
            var childForumIds = childForums.Select(s => s.Id);

            Assert.All(expectedChildForumIds, id => Assert.Contains(id, childForumIds));
            Assert.Equal(expectedChildForumIds.Count(), childForumIds.Count());
        }

        [Fact]
        public async Task GetTotalPage()
        {
            int expectedTotalPage = 647;

            var testDataFilePath = TestDataPath("forum_html_source.html");
            var html_sourcce = File.ReadAllText(testDataFilePath);
            _httpClientStub.SetHttpResponse(html_sourcce);

            var totalPage = await _forum.GetTotalPage();

            Assert.Equal(expectedTotalPage, totalPage);
        }

        [Fact]
        public async Task GetThreads()
        {
            string[] expectedThreads =
            {
                "hinh-thu-gian-v66-cung-nhau-day-lui-dich-covid-19-sfw-only-xem-page-1.1423220",
                "event-mo-order-dot-cuoi-cung-item-ki-niem-20-nam-gamevn-tu-25-10-14-11.1523616",
                "youtube-clips-thu-gian-v56-di-mot-ngay-dang-luom-mot-dong-xu.1077912",
                "noi-lam-viec-giua-ban-dieu-hanh-va-mem.758423",
                "mua-he-nong-bong-voi-nhung-co-gai-dep-nhat-the-gioi-ver-xxi.818670",
                "girl-xinh-viet-nam-version-202x-post-anh-bo-spoiler-nghiem-cam-cac-van-de-18-khac.1392135",
                "feedback-event-item-ki-niem-20-nam-gvn-khoe-khoang-chem-gio-chat-chit.1516263",
                "tong-hop-review-nhung-vat-dung-nen-co-cho-cuoc-song-tien-nghi-hon.1494788",
                "noi-quy-box-thu-gian-chu-y-doc-noi-quy-truoc-khi-tham-gia-b50-update-luat-thi-mang-19-09-2021.1513853",
                "box-50-feel-bar-cocktail-for-your-heart-noi-noi-buon-voi-di-cung-gamevn.1367246",
                "nhat-ki-box-50.1169117",
                "seeder-mo-tai-khoan-tpbank.1524071",
                "quan-tra-chanh-tra-da-via-he-nhat-ky-cuoc-song-moi-ngay-chatbox-b50.1523423",
                "game-nft-cap-nhat-thao-luan.1519287",
                "round-2-rap-viet-vs-king-of-rap.1497437",
                "nho-tu-van-tai-chinh-co-phieu.1446545",
                "hoi-san-sale-tren-cho-online-shopee-tiki-lazada-sendo-chotot.1481408",
                "event-gamevn-xiaolonist-talent-cuoc-thi-tim-kiem-tai-nang-xao-lol-cua-gvn.1521933",
                "thong-bao-tien-do-update-ve-viec-lam-qua-tri-an-20-nam-thanh-lap-gamevn.1523784",
                "gym-the-hinh-where-gamers-become-healthier.1516591",
                "dota2-congratulations-to-the-winners-of-the-international-10-team-spirit.1476161",
                "tai-sao-cua-gai-luon-fail-nam-moi-van-fail.1434936",
                "gamevn-trade-coin-team.1259527",
                "pic-cung-chia-se-nhung-khoanh-khac-cua-cuoc-song.1491793",
                "hoi-nhung-nguoi-me-kshow-kpop-kdrama-va-nhung-thu-lien-quan-den-han-xeng.1416391",
                "recommend-truyen-hay-mua-dich.1518572",
                "hoi-that-nghiep-cung-nhau-tim-con-duong-moi.1522115",
                "co-ai-con-choi-lien-quan-ko.1456064",
                "tien-tu-khai-nhan-mat-sang-nhu-den-pha.1520577",
                "ong-bo-bim-sua-ru-con-ngu-tam-cho-con.1485339",
                "gamevn-mens-fashion-noi-quy-ong-lam-dep.1522965"
            };

            var testDataFilePath = TestDataPath("forum_html_source.html");
            var html_sourcce = File.ReadAllText(testDataFilePath);
            _httpClientStub.SetHttpResponse(html_sourcce);

            var threads = await _forum.GetThreads();
            var threadIds = threads.Select(s => s.Id);

            Assert.All(expectedThreads, id => Assert.Contains(id, threadIds));
        }
    }
}
