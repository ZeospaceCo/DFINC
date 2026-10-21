using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Xml.Linq;

namespace Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper
{
    public class RssHelper
    {
        public IEnumerable<Feed> Feeds { get; set; }

        /// <summary>
        /// 작성자 : freeman
        /// 작성일자 : 2011-12-26
        /// 작성목적 : RSS 테스트
        /// </summary>
        /// <param name="Path"></param>
        public IEnumerable<Feed> RssReader(string Path)
        {
            try
            {
                XDocument doc = XDocument.Load(Path);
                Feeds = from item in doc.Element("rss").Element("channel").Elements("item")
                        select new Feed
                        {
                            Title = item.Element("title").Value,
                            Link = item.Element("link").Value,
                            Description = item.Element("description").Value,
                            PublicationDate = DateTime.Parse(item.Element("pubDate").Value)
                        };
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("RssReader", ex);
            }
            return Feeds;
        }

        /// <summary>
        /// 작성자 : freeman
        /// 작성일자 : 2011-12-26
        /// 작성목적 : RSS 테스트 Async
        /// </summary>
        /// <param name="Path"></param>
        public void RssReaderASync(string Path)
        {
            WebClient rssDownloader = new WebClient();
            rssDownloader.DownloadStringAsync(new Uri(Path));
            rssDownloader.DownloadStringCompleted += new System.Net.DownloadStringCompletedEventHandler(rssDownloader_DownloadStringCompleted);
        }

        /// <summary>
        /// 작성자 : freeman
        /// 작성일자 : 2011-12-26
        /// 작성목적 : RSS 테스트 Async (페이지별로 비동기 구현 검토)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rssDownloader_DownloadStringCompleted(object sender, System.Net.DownloadStringCompletedEventArgs e)
        {
            try
            {
                string rssContent = e.Result;

                XDocument rssParser = XDocument.Parse(rssContent);

                Feeds = from item in rssParser.Descendants("item")
                        select new Feed
                        {
                            Title = item.Element("title").Value,
                            Link = item.Element("link").Value,
                            Description = item.Element("description").Value,
                            PublicationDate = DateTime.Parse(item.Element("pubDate").Value)
                        };
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("rssDownloader_DownloadStringCompleted", ex);
            }
        }
    }

    public class Feed
    {
        public string Title { get; set; }

        public string Link { get; set; }

        public string Description { get; set; }

        public DateTime PublicationDate { get; set; }

        public Guid GUID { get; set; }
    }

    public class Channel
    {
        public string Title { get; set; }

        public string Link { get; set; }

        public string Description { get; set; }

        public string Language { get; set; }

        public DateTime PublicationDate { get; set; }

        public string LastBuildDate { get; set; }
    }
}