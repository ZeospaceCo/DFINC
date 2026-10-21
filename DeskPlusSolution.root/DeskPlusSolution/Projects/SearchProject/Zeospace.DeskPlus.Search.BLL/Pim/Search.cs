using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Zeospace.DeskPlus.Search.BE.Pim;
using Version = Lucene.Net.Util.Version;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Search.BLL.Pim
{
    public static class Search
    {
        public static string IndexDir = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, @"SearchIndex\Pim");
        private static string StopwordDir = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, @"Search\Dic\stopword.dic");

        private static FSDirectory _directoryTemp;

        private static FSDirectory _directory
        {
            get
            {
                if (_directoryTemp == null) _directoryTemp = FSDirectory.Open(new DirectoryInfo(IndexDir));
                if (IndexWriter.IsLocked(_directoryTemp)) IndexWriter.Unlock(_directoryTemp);

                var lockFilePath = Path.Combine(IndexDir, "write.lock");

                if (File.Exists(lockFilePath)) File.Delete(lockFilePath);
                return _directoryTemp;
            }
        }

        #region Prototype

        /// <summary>
        /// 작성자 :
        /// 작성일자 :
        /// 작성목적 : 검색
        /// </summary>
        /// <param name="input"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static IEnumerable<ListInfo> Get(string input, string fieldName, string startDate, string endDate)
        {
            if (string.IsNullOrEmpty(input)) return new List<ListInfo>();

            var terms = input.Trim().Replace("-", " ").Split(' ').Where(x => !string.IsNullOrEmpty(x)).Select(x => x.Trim() + "*");
            input = string.Join(" ", terms);

            return GetSearch(input, fieldName, startDate, endDate);
        }

        /// <summary>
        /// 작성자 :
        /// 작성일자 :
        /// 작성목적 : 검색
        /// </summary>
        /// <param name="input"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        private static IEnumerable<ListInfo> GetSearch(string searchQuery, string searchField, string startDate, string endDate)
        {
            if (string.IsNullOrEmpty(searchQuery.Replace("*", "").Replace("?", ""))) return new List<ListInfo>();

            //lucene searcher 준비
            using (var searcher = new IndexSearcher(_directory, false))
            {
                var hits_limit = 1000;
                //var analyzer = new StandardAnalyzer(Version.LUCENE_29);
                //var analyzer = new Lucene.Net.Analysis.CJK.CJKAnalyzer(StopWords);
                var analyzer = new Lucene.Net.Analysis.CJK.CJKAnalyzer();

                // 단순 검색

                if (!string.IsNullOrEmpty(searchField))
                {
                    var parser = new QueryParser(Version.LUCENE_29, searchField, analyzer);
                    var query = parseQuery(searchQuery, parser);
                    //var hits = searcher.Search(query, hits_limit).ScoreDocs;
                    //var results = GetLuceneToDataList(hits, searcher);

                    //========================================================================
                    // 하이라이팅 기능 테스트
                    var hits = searcher.Search(query);
                    var formatter = new Lucene.Net.Highlight.SimpleHTMLFormatter(
                                            "<span class=\"highlighting\">",
                                            "</span>");

                    var fragmenter = new Lucene.Net.Highlight.SimpleFragmenter(100);
                    var scorer = new Lucene.Net.Highlight.QueryScorer(query);
                    var highlighter = new Lucene.Net.Highlight.Highlighter(formatter, scorer);
                    highlighter.SetTextFragmenter(fragmenter);

                    List<ListInfo> results = new List<ListInfo>();

                    for (int i = 0; i < hits.Length(); i++)
                    {
                        ListInfo ListInfo = new ListInfo();

                        Lucene.Net.Documents.Document doc = hits.Doc(i);

                        string contents = HttpUtility.HtmlEncode(doc.Get("Content"));
                        Lucene.Net.Analysis.TokenStream stream = analyzer.TokenStream("Content", new StringReader(contents));

                        List<Lucene.Net.Analysis.Token> tokenList = new List<Lucene.Net.Analysis.Token>();

                        while (true)
                        {
                            Lucene.Net.Analysis.Token token = stream.Next();
                            if (token == null) break;

                            tokenList.Add(token);
                        }

                        ListInfo.ID = doc.Get("ID");
                        ListInfo.Type = doc.Get("Type");
                        ListInfo.CompanyID = Convert.ToInt32(doc.Get("CompanyID"));
                        ListInfo.ListID = Convert.ToInt32(doc.Get("ListID"));
                        ListInfo.DisplayID = Convert.ToInt32(doc.Get("DisplayID"));
                        ListInfo.ListType = doc.Get("ListType");
                        ListInfo.IDType = doc.Get("IDType");
                        ListInfo.StartTime = doc.Get("StartTime");
                        ListInfo.EndTime = doc.Get("EndTime");
                        ListInfo.IsOpen = doc.Get("IsOpen");
                        ListInfo.OpenType = Convert.ToInt32(doc.Get("OpenType"));
                        ListInfo.StatusID = Convert.ToInt32(doc.Get("StatusID")); 
                        ListInfo.Colors = doc.Get("Colors");

                        ListInfo.Title = doc.Get("Title");
                        ListInfo.Content = doc.Get("Content");
                        ListInfo.ContentHTML = highlighter.GetBestFragments(stream, contents, 1, "...");

                        results.Add(ListInfo);
                    }

                    //========================================================================

                    analyzer.Close();
                    searcher.Close();
                    searcher.Dispose();
                    return results;
                }
                else  //복합검색
                {
                    var parser = new MultiFieldQueryParser(Version.LUCENE_29, new[] { "Title", "Content", "FileName" }, analyzer);
                    var query = parseQuery(searchQuery, parser);
                    //var hits = searcher.Search(query, null, hits_limit, Sort.INDEXORDER).ScoreDocs;  //new Sort(new SortField[]{new SortField("ObjectId"), new SortField("Subject", SortField.INT, true)})
                    //var results = GetLuceneToDataList(hits, searcher);

                    //========================================================================
                    // 하이라이팅 기능 테스트
                    var hits = searcher.Search(query);
                    var formatter = new Lucene.Net.Highlight.SimpleHTMLFormatter(
                                            "<span class=\"highlighting\">",
                                            "</span>");

                    var fragmenter = new Lucene.Net.Highlight.SimpleFragmenter(100);
                    var scorer = new Lucene.Net.Highlight.QueryScorer(query);
                    var highlighter = new Lucene.Net.Highlight.Highlighter(formatter, scorer);
                    highlighter.SetTextFragmenter(fragmenter);

                    List<ListInfo> results = new List<ListInfo>();

                    for (int i = 0; i < hits.Length(); i++)
                    {
                        ListInfo ListInfo = new ListInfo();

                        Lucene.Net.Documents.Document doc = hits.Doc(i);

                        string contents = HttpUtility.HtmlEncode(doc.Get("Content"));
                        Lucene.Net.Analysis.TokenStream stream = analyzer.TokenStream("Content", new StringReader(contents));

                        List<Lucene.Net.Analysis.Token> tokenList = new List<Lucene.Net.Analysis.Token>();

                        while (true)
                        {
                            Lucene.Net.Analysis.Token token = stream.Next();
                            if (token == null) break;

                            tokenList.Add(token);
                        }

                        ListInfo.ID = doc.Get("ID");
                        ListInfo.Type = doc.Get("Type");
                        ListInfo.CompanyID = Convert.ToInt32(doc.Get("CompanyID"));
                        ListInfo.ListID = Convert.ToInt32(doc.Get("ListID"));
                        ListInfo.DisplayID = Convert.ToInt32(doc.Get("DisplayID"));
                        ListInfo.ListType = doc.Get("ListType");
                        ListInfo.IDType = doc.Get("IDType");                        
                        ListInfo.StartTime = doc.Get("StartTime");
                        ListInfo.EndTime = doc.Get("EndTime");
                        ListInfo.IsOpen = doc.Get("IsOpen");
                        ListInfo.OpenType = Convert.ToInt32(doc.Get("OpenType"));
                        ListInfo.StatusID = Convert.ToInt32(doc.Get("StatusID"));
                        ListInfo.Colors = doc.Get("Colors");

                        ListInfo.Title = doc.Get("Title");
                        ListInfo.Content = doc.Get("Content");
                        ListInfo.ContentHTML = highlighter.GetBestFragments(stream, contents, 1, "...");

                        results.Add(ListInfo);
                    }

                    //========================================================================

                    analyzer.Close();
                    searcher.Close();
                    searcher.Dispose();
                    return results;
                }
            }
        }

        /// <summary>
        /// 작성자 : freeman
        /// 작성일자 : 2013.12.09
        /// 작성목적 : 모든 색인정보 가져오기
        /// </summary>
        /// <param name="searchQuery"></param>
        /// <param name="parser"></param>
        /// <returns></returns>
        public static IEnumerable<ListInfo> GetAllIndexRecords()
        {
            if (!System.IO.Directory.EnumerateFiles(IndexDir).Any()) return new List<ListInfo>();
            //var hits_limit = 100;
            var searcher = new IndexSearcher(_directory, false);
            var reader = IndexReader.Open(_directory, false);
            var docs = new List<Document>();
            var term = reader.TermDocs();
            int i = 0;
            while (term.Next())
            {
                //if (i > hits_limit) break;
                docs.Add(searcher.Doc(term.Doc()));

                //i++;
            }

            reader.Close();
            reader.Dispose();
            searcher.Close();
            searcher.Dispose();

            return GetLuceneToDataList(docs);
        }

        #endregion Prototype

        #region 인덱스 색인 리스트

        /// <summary>
        /// 작성자 : freeman
        /// 작성일자 : 2013.12.09
        /// 작성목적 : 검색어에서 특수문자 제거
        /// </summary>
        /// <param name="searchQuery"></param>
        /// <param name="parser"></param>
        /// <returns></returns>
        private static Query parseQuery(string searchQuery, QueryParser parser)
        {
            Query query;
            try
            {
                query = parser.Parse(searchQuery.Trim());
            }
            catch (ParseException)
            {
                query = parser.Parse(QueryParser.Escape(searchQuery.Trim()));
            }

            return query;
        }

        /// <summary>
        /// 작성자 : freeman
        /// 작성일자 : 2013.12.09
        /// 작성목적 : 데이터 인덱스 검색 색인
        /// </summary>
        /// <param name="hits"></param>
        /// <returns></returns>
        private static IEnumerable<ListInfo> GetLuceneToDataList(IEnumerable<Document> hits)
        {
            return hits.Select(GetLuceneDocumentToData).ToList();
        }

        /// <summary>
        /// 작성자 : freeman
        /// 작성일자 : 2013.12.09
        /// 작성목적 : 데이터 인덱스 검색 색인
        /// </summary>
        /// <param name="hits"></param>
        /// <param name="searcher"></param>
        /// <returns></returns>
        private static IEnumerable<SearchInfo> GetLuceneToDataList(IEnumerable<ScoreDoc> hits, IndexSearcher searcher)
        {
            return hits.Select(hit => GetLuceneDocumentToData(searcher.Doc(hit.doc))).ToList();
        }

        /// <summary>
        /// 작성자 : freeman
        /// 작성일자 : 2013.12.09
        /// 작성목적 : Doc에서 BE 담기
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        private static ListInfo GetLuceneDocumentToData(Document doc)
        {
            return new ListInfo
            {
                Module = "Pim",
                Type = doc.Get("Type"),
                ID = doc.Get("ID"),
                CompanyID = Convert.ToInt32(doc.Get("CompanyID")),
                ListID = Convert.ToInt32(doc.Get("ListID")),
                DisplayID = Convert.ToInt32(doc.Get("DisplayID")),
                ListType = doc.Get("ListType"),
                IDType = doc.Get("IDType"),
                StartTime = doc.Get("StartTime"),
                EndTime = doc.Get("EndTime"),
                IsOpen = doc.Get("IsOpen"),
                OpenType = Convert.ToInt32(doc.Get("OpenType")),
                StatusID = Convert.ToInt32(doc.Get("StatusID")),
                Colors = doc.Get("Colors"),

                Title = doc.Get("Title"),
                FileName = doc.Get("FileName"),
                Content = String.IsNullOrEmpty(doc.Get("Content")) ? "" : doc.Get("Content"),
                WriteDate = doc.Get("WriteDate"),
                WriteUserID = doc.Get("WriteUserID"),
                WriteUserName = doc.Get("WriteUserName")
            };
        }

        #endregion 인덱스 색인 리스트

        #region 검색 리스트

        /// <summary>
        /// 작성자   : freeman
        /// 작성일자 : 2011-10-19
        /// 작성목적 : 폴더별 게시물 개수
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <param name="_folderID"></param>
        /// <param name="_searchField"></param>
        /// <param name="_searchValue"></param>
        /// <param name="_startDate"></param>
        /// <param name="_endDate"></param>
        /// <returns></returns>
        public static int GetPageListCount(int _companyID, int _userID, string _searchValue, string _startDate, string _endDate)
        {
            if (!System.IO.Directory.EnumerateFiles(IndexDir).Any()) return 0;
            if (string.IsNullOrEmpty(_searchValue.Replace("*", "").Replace("?", "")) && _userID != 0) return 0;

            int rtCnt = 0;

            if (string.IsNullOrEmpty(_searchValue) && _userID == 0)
            {
                using (var reader = IndexReader.Open(_directory, false))
                {
                    rtCnt = reader.NumDocs();
                }

                return rtCnt;
            }

            using (var searcher = new IndexSearcher(_directory, false))
            {
                var analyzer = new Lucene.Net.Analysis.CJK.CJKAnalyzer();
                var parser = new MultiFieldQueryParser(Version.LUCENE_29, new[] { "Title", "Content", "FileName" }, analyzer);
                var query = parseQuery(_searchValue, parser);

                SortField sf = new SortField("WriteDate", SortField.LONG, true);
                Sort sort = new Sort(sf);

                TopFieldCollector collector = TopFieldCollector.create(sort, 1000, false, true, false, false);

                if (!String.IsNullOrEmpty(_startDate) && !String.IsNullOrEmpty(_endDate))
                {
                    RangeQuery rq = new RangeQuery(new Term("WriteDate", _startDate), new Term("WriteDate", _endDate), true);

                    QueryWrapperFilter filter = new QueryWrapperFilter(rq);

                    searcher.Search(query, filter, collector);
                }
                else
                {
                    searcher.Search(query, collector);
                }

                TopDocs td = collector.TopDocs();

                using (var reader = IndexReader.Open(_directory, false))
                {
                    ArrayList arrList = GetListIDByUserID(_companyID, _userID, true);

                    for (int i = 0; i < td.scoreDocs.Length; i++)
                    {
                        if (arrList.Contains(reader.Document(td.scoreDocs[i].doc).Get("ListID").ToString()))
                        {
                            rtCnt++;
                        }
                    }
                }
            }

            return rtCnt;
        }

        /// <summary>
        /// 작성자   : freeman
        /// 작성일자 : 2011-10-20
        /// 작성목적 : 검색 페이징 리스트 관리자
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <param name="_folderID"></param>
        /// <param name="_pageSize"></param>
        /// <param name="_pageSizePageNo"></param>
        /// <param name="_searchField"></param>
        /// <param name="_searchValue"></param>
        /// <param name="_startDate"></param>
        /// <param name="_endDate"></param>
        /// <param name="_order"></param>
        /// <returns></returns>
        public static IEnumerable<ListInfo> GetPageList(int _companyID, int _userID, int _pageSize, int _pageSizePageNo,
            string _searchField, string _searchValue, string _startDate, string _endDate, string _order)
        {
            if (!System.IO.Directory.EnumerateFiles(IndexDir).Any()) return new List<ListInfo>();
            if (string.IsNullOrEmpty(_searchValue.Replace("*", "").Replace("?", "")) && _userID != 0) return new List<ListInfo>();

            var hits_limit = 1000;
            var docs = new List<Document>();

            if (string.IsNullOrEmpty(_searchValue) && _userID == 0)
            {
                var searcher = new IndexSearcher(_directory, false);
                using (var reader = IndexReader.Open(_directory, false))
                {
                    var term = reader.TermDocs();
                    int i = 0;
                    while (term.Next())
                    {
                        if (i > hits_limit) break;
                        docs.Add(searcher.Doc(term.Doc()));
                        i++;
                    }
                }

                return GetPage(GetLuceneToDataList(docs), _pageSize, _pageSizePageNo);
            }

            try
            {
                using (var searcher = new IndexSearcher(_directory, false))
                {
                    var analyzer = new Lucene.Net.Analysis.CJK.CJKAnalyzer();
                    var parser = new MultiFieldQueryParser(Version.LUCENE_29, new[] { "Title", "Content", "FileName" }, analyzer);
                    //parser.SetDefaultOperator(QueryParser.Operator.AND);
                    var query = parseQuery(_searchValue, parser);

                    SortField sf = new SortField("WriteDate", SortField.LONG, true);
                    Sort sort = new Sort(sf);

                    //TopFieldCollector collector = TopFieldCollector.create(sort, 60, false, true, false, false);

                    TopDocs td;
                    if (!String.IsNullOrEmpty(_startDate) && !String.IsNullOrEmpty(_endDate))
                    {
                        RangeQuery rq = new RangeQuery(new Term("WriteDate", _startDate), new Term("WriteDate", _endDate), true);

                        QueryWrapperFilter filter = new QueryWrapperFilter(rq);

                        td = searcher.Search(query, filter, (_pageSizePageNo + 1) * _pageSize, sort);
                    }
                    else
                    {
                        td = searcher.Search(query, null, (_pageSizePageNo + 1) * _pageSize, sort);
                    }

                    using (var reader = IndexReader.Open(_directory, false))
                    {
                        ArrayList arrList = GetListIDByUserID(_companyID, _userID, true);
                        for (int i = 0; i < td.scoreDocs.Length; i++)
                        {
                            if (arrList.Contains(reader.Document(td.scoreDocs[i].doc).Get("ListID").ToString()))
                            {
                                docs.Add(reader.Document(td.scoreDocs[i].doc));
                            }
                        }
                    }

                    return GetPage(GetLuceneToDataList(docs), _pageSize, _pageSizePageNo);
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetPageList " + ex);
                return new List<ListInfo>();
            }
        }

        /// <summary>
        /// 작성자   : freeman
        /// 작성일자 : 2011-10-20
        /// 작성목적 : 검색 페이징 리스트 사용자
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <param name="_pageSize"></param>
        /// <param name="_pageSizePageNo"></param>
        /// <param name="_searchField"></param>
        /// <param name="_searchValue"></param>
        /// <param name="_startDate"></param>
        /// <param name="_endDate"></param>
        /// <param name="_order"></param>
        /// <returns></returns>
        public static IEnumerable<ListInfo> GetPageListH(int _companyID, int _userID, int _pageSize, int _pageSizePageNo,
            string _searchField, string _searchValue, string _startDate, string _endDate, string _order)
        {
            if (!System.IO.Directory.EnumerateFiles(IndexDir).Any()) return new List<ListInfo>();
            if (string.IsNullOrEmpty(_searchValue.Replace("*", "").Replace("?", "")) && _userID != 0) return new List<ListInfo>();

            var hits_limit = 1000;
            var docs = new List<Document>();

            if (string.IsNullOrEmpty(_searchValue) && _userID == 0)
            {
                var searcher = new IndexSearcher(_directory, false);
                using (var reader = IndexReader.Open(_directory, false))
                {
                    var term = reader.TermDocs();
                    int i = 0;
                    while (term.Next())
                    {
                        if (i > hits_limit) break;
                        docs.Add(searcher.Doc(term.Doc()));
                        i++;
                    }
                }

                return GetPage(GetLuceneToDataList(docs), _pageSize, _pageSizePageNo);
            }

            try
            {
                using (var searcher = new IndexSearcher(_directory, false))
                {
                    var analyzer = new Lucene.Net.Analysis.CJK.CJKAnalyzer();
                    var parser = new MultiFieldQueryParser(Version.LUCENE_29, new[] { "Title", "Content", "FileName" }, analyzer);
                    var query = parseQuery(_searchValue, parser);

                    SortField sf = new SortField("WriteDate", SortField.LONG, true);
                    Sort sort = new Sort(sf);

                    //TopFieldCollector collector = TopFieldCollector.create(sort, 60, false, true, false, false);

                    TopDocs td;

                    if (!String.IsNullOrEmpty(_startDate) && !String.IsNullOrEmpty(_endDate))
                    {
                        RangeQuery rq = new RangeQuery(new Term("WriteDate", _startDate), new Term("WriteDate", _endDate), true);

                        QueryWrapperFilter filter = new QueryWrapperFilter(rq);

                        td = searcher.Search(query, filter, (_pageSizePageNo + 1) * _pageSize, sort);
                    }
                    else
                    {
                        td = searcher.Search(query, null, (_pageSizePageNo + 1) * _pageSize, sort);
                    }

                    //========================================================================
                    // 하이라이팅 기능 테스트
                    var formatter = new Lucene.Net.Highlight.SimpleHTMLFormatter("<span class=\"highlighting\">", "</span>");
                    var fragmenter = new Lucene.Net.Highlight.SimpleFragmenter(200);
                    var scorer = new Lucene.Net.Highlight.QueryScorer(query);
                    var highlighter = new Lucene.Net.Highlight.Highlighter(formatter, scorer);
                    highlighter.SetTextFragmenter(fragmenter);

                    List<ListInfo> results = new List<ListInfo>();

                    using (var reader = IndexReader.Open(_directory, false))
                    {
                        ArrayList arrList = GetListIDByUserID(_companyID, _userID, true);

                        for (int i = 0; i < td.scoreDocs.Length; i++)
                        {
                            ListInfo ListInfo = new ListInfo();

                            //docs.Add(reader.Document(td.scoreDocs[i].doc));
                            Lucene.Net.Documents.Document doc = reader.Document(td.scoreDocs[i].doc);

                            //권한 체크
                            if (arrList.Contains(doc.Get("ListID").ToString()))
                            {
                                string contents = HttpUtility.HtmlEncode(doc.Get("Content"));

                                if (String.IsNullOrEmpty(contents))
                                {
                                    ListInfo.Content = "";
                                }
                                else
                                {
                                    Lucene.Net.Analysis.TokenStream stream = analyzer.TokenStream("Content", new StringReader(contents));
                                    ListInfo.Content = highlighter.GetBestFragments(stream, contents, 1, "...");
                                }

                                ListInfo.Module = "PIM";
                                ListInfo.Type = doc.Get("Type");
                                ListInfo.ID = doc.Get("ID");
                                ListInfo.CompanyID = Convert.ToInt32(doc.Get("CompanyID"));
                                ListInfo.ListID = Convert.ToInt32(doc.Get("ListID"));
                                ListInfo.DisplayID = Convert.ToInt32(doc.Get("DisplayID"));
                                ListInfo.IDType = doc.Get("IDType");
                                ListInfo.Title = doc.Get("Title");
                                ListInfo.FileName = doc.Get("FileName");
                                ListInfo.WriteDate = doc.Get("WriteDate");
                                ListInfo.WriteUserID = doc.Get("WriteUserID");
                                ListInfo.WriteUserName = doc.Get("WriteUserName");

                                if (doc.Get("Type") == "F") // 파일이면 다운로드 정보 추가
                                {
                                    Dictionary<string, string> paraDic = new Dictionary<string, string>();
                                    paraDic.Add("filePath", doc.Get("FilePath") + doc.Get("FileGUID"));
                                    paraDic.Add("fileName", doc.Get("FileName"));
                                    ListInfo.FilePath = GetEncryptParam(Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.GetParameterDic(paraDic));

                                    ListInfo.FileGUID = doc.Get("FileGUID");
                                }

                                results.Add(ListInfo);
                            }
                        }
                    }

                    return GetPage(results, _pageSize, _pageSizePageNo);
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetPageList " + ex);
                return new List<ListInfo>();
            }
        }

        /// <summary>
        /// 작성자   : freeman
        /// 작성일자 : 2011-10-20
        /// 작성목적 : 권한 확인
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <returns></returns>
        private static BooleanQuery GetListIDByUserID(int _companyID, int _userID)
        {
            DAL.Pim dal = new DAL.Pim();

            DataTableReader dtr = dal.GetListID(_companyID, _userID);

            BooleanQuery filterQuery = new BooleanQuery();

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        filterQuery.Add(new TermQuery(new Term("ListID", Convert.ToInt32(dtr["ListID"]).ToString())), BooleanClause.Occur.SHOULD);
                    }
                }
            }

            return filterQuery;
        }

        /// <summary>
        /// 작성자   : freeman
        /// 작성일자 : 2013-12-19
        /// 작성목적 : 권한 확인  (maxClauseCount is set to 1024) 로 인하여 변경.
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <returns></returns>
        private static ArrayList GetListIDByUserID(int _companyID, int _userID, bool isArray)
        {
            DAL.Pim dal = new DAL.Pim();

            DataTableReader dtr = dal.GetListID(_companyID, _userID);

            ArrayList arrayList = new ArrayList();

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        arrayList.Add(dtr["ListID"].ToString());
                    }
                }
            }

            return arrayList;
        }

        /// <summary>
        /// 작성자 : freeman
        /// 작성일자 : 2013.12.09
        /// 작성목적 : 색인정보 페이징
        /// </summary>
        /// <param name="_pageNo"></param>
        /// <param name="_record"></param>
        /// <returns></returns>
        public static IEnumerable<ListInfo> GetPage(IEnumerable<ListInfo> _listInfo, int _pageSize, int _pageSizePageNo)
        {
            var records = from list in _listInfo select list;

            records = records.Skip((_pageSizePageNo - 1) * _pageSize).Take(_pageSize);

            List<ListInfo> results = new List<ListInfo>();

            foreach (var r in records)
            {
                results.Add(r);
            }

            return results;
        }

        /// <summary>
        /// 작성자 : freeman
        /// 작성일자 : 2013.12.09
        /// 작성목적 : 파라미터 암호화
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private static string GetEncryptParam(string data)
        {
            //빈값처리 Ajax에서는 빈값을 보낼경우 오류 발생 하므로 INULL 이라는 값으로 null 표시함
            if (String.IsNullOrEmpty(data)) return "!NULL";

            try
            {
                HttpContext context = HttpContext.Current;

                return Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.EncryptString(context.Server.UrlEncode(data));
            }
            catch
            {
                return "!NULL";
            }
        }

        #endregion 검색 리스트
    }
}