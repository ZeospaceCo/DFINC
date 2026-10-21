using System;
using System.Collections.Generic;
using System.ServiceModel.Syndication;
using System.Xml;

namespace Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper
{
    /// <summary>
    /// 작성자 : 심재운
    /// 목적 : NHN 기사 RSS 2.0 가져오기
    /// </summary>
    public static class Rss20Helper
    {
        /// <summary>
        /// RSS 2.0 네이버 기사 데이터 가져오기
        /// </summary>
        /// <param name="_rssUrl"></param>
        /// <returns></returns>
        public static List<Rss20HelperInfo> GetRss20Reader(string _rssUrl)
        {
            XmlNode nodeRss = null;
            XmlNode nodeChannel = null;
            XmlNode nodeItem = null;

            if (_rssUrl.Length == 0) return null;

            XmlTextReader rssReader = new XmlTextReader(_rssUrl);
            XmlDocument rssDoc = new XmlDocument();
            rssDoc.Load(rssReader);

            // rss 태크에서 확인
            for (int i = 0; i < rssDoc.ChildNodes.Count; i++)
            {
                // rss 태그가 존재하면?
                if (rssDoc.ChildNodes[i].Name == "rss")
                {
                    // rss 태그를 찾아서 넣는다.
                    nodeRss = rssDoc.ChildNodes[i];
                }
            }

            // channel 태그에서 확인
            for (int i = 0; i < nodeRss.ChildNodes.Count; i++)
            {
                //channel 태그가 존재하면?
                if (nodeRss.ChildNodes[i].Name == "channel")
                {
                    // channel 태그를 찾아서 넣는다.
                    nodeChannel = nodeRss.ChildNodes[i];
                }
            }

            List<Rss20HelperInfo> al = new List<Rss20HelperInfo>();

            for (int i = 0; i < nodeChannel.ChildNodes.Count; i++)
            {
                // item 태그가 있으면, 하위 태그들의 값들을 넣는다.
                if (nodeChannel.ChildNodes[i].Name == "item")
                {
                    nodeItem = nodeChannel.ChildNodes[i];

                    Rss20HelperInfo info = new Rss20HelperInfo();
                    info.Title = (nodeItem["title"] != null) ? nodeItem["title"].InnerText : "";
                    info.Link = (nodeItem["link"] != null) ? nodeItem["link"].InnerText : "";
                    info.Pubdate = (nodeItem["pubDate"] != null) ? nodeItem["pubDate"].InnerText : "";
                    info.Category = (nodeItem["category"] != null) ? nodeItem["category"].InnerText : "";

                    al.Add(info);
                }
            }

            return al;
        }

        /// <summary>
        /// 작성자 : 심재운
        /// RSS, ATOM 생성하기
        /// </summary>
        /// <param name="_feedInfo">FEED 값</param>
        /// <param name="_itemInfo">ITEM 값</param>
        /// <returns></returns>
        public static SyndicationFeed SyndicationHelper(Rss20DataInfo _feedInfo, List<Rss20DataInfo> _itemInfo)
        {
            //feed 생성하기
            Uri uri = new Uri(_feedInfo.LinkUrl);

            SyndicationFeed syndicationFeed = new SyndicationFeed(
                _feedInfo.Title,
                _feedInfo.Description,
                uri,
                _feedInfo.RssID,
                DateTime.Now
            );

            //item 생성하기
            List<SyndicationItem> items = new List<SyndicationItem>();

            foreach (Rss20DataInfo info in _itemInfo)
            {
                SyndicationItem oItem = new SyndicationItem(info.Title,
                                                            SyndicationContent.CreateHtmlContent(info.Description),
                                                            new Uri(info.LinkUrl),
                                                            info.RssID.ToString(),
                                                            info.LastUpdatedTime);
                items.Add(oItem);
            }
            syndicationFeed.Items = items;
            return syndicationFeed;
        }

        /// <summary>
        /// 작성자 : 심재운
        /// SyndicationFeed 방식으로 rss 읽기
        /// </summary>
        /// <param name="_rssUrl">The _RSS URL.</param>
        /// <returns></returns>
        public static List<Rss20HelperInfo> GetRss20SyndiReader(string _rssUrl)
        {
            IEnumerable<SyndicationItem> items = null;
            SyndicationFeed rareBookNews =
                    SyndicationFeed.Load(XmlReader.Create(_rssUrl));

            items = rareBookNews.Items;

            List<Rss20HelperInfo> al = new List<Rss20HelperInfo>();

            foreach (SyndicationItem syn in items)
            {
                Rss20HelperInfo info = new Rss20HelperInfo();
                info.Category = info.Category;
                info.Title = info.Title;
                info.Link = info.Link;
                info.Pubdate = info.Pubdate;

                al.Add(info);
            }

            return al;
        }
    }

    /// <summary>
    /// NAVER 2.0 RSS 기사들
    /// </summary>
    [Serializable]
    public class Rss20HelperInfo
    {
        public Rss20HelperInfo()
        {
        }

        /// <summary>
        /// 제목
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 기사링크
        /// </summary>
        public string Link { get; set; }

        /// <summary>
        /// 날짜
        /// </summary>
        public string Pubdate { get; set; }

        /// <summary>
        /// 카테고리
        /// </summary>
        public string Category { get; set; }
    }

    /// <summary>
    /// Rss20DataInfo
    /// </summary>
    [Serializable]
    public class Rss20DataInfo
    {
        public Rss20DataInfo()
        {
        }

        public string Title { get; set; }

        public string Description { get; set; }

        public string LinkUrl { get; set; }

        public string RssID { get; set; }

        public DateTime LastUpdatedTime { get; set; }
    }
}