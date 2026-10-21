namespace Zeospace.DeskPlus.Search.BLL
{
    public static class LuceneSearch
    {
        //public static string _luceneDir = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, "Search_Index");

        //private static string _stopwordDir = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, @"Search\Dic\stopword.dic");
        //private static FSDirectory _directoryTemp;
        //private static FSDirectory _directory
        //{
        //    get
        //    {
        //        if (_directoryTemp == null) _directoryTemp = FSDirectory.Open(new DirectoryInfo(_luceneDir));
        //        if (IndexWriter.IsLocked(_directoryTemp)) IndexWriter.Unlock(_directoryTemp);

        //        var lockFilePath = Path.Combine(_luceneDir, "write.lock");

        //        if (File.Exists(lockFilePath)) File.Delete(lockFilePath);
        //        return _directoryTemp;
        //    }

        //}

        ///// <summary>
        ///// 작성자 :
        ///// 작성일자 :
        ///// 작성목적 : 검색
        ///// </summary>
        ///// <returns></returns>
        //public static IEnumerable<SearchInfo> GetAllIndexRecords()
        //{
        //    if (!System.IO.Directory.EnumerateFiles(_luceneDir).Any()) return new List<SearchInfo>();

        //    var searcher = new IndexSearcher(_directory, false);
        //    var reader = IndexReader.Open(_directory, false);
        //    var docs = new List<Document>();
        //    var term = reader.TermDocs();

        //    while (term.Next()) docs.Add(searcher.Doc(term.Doc()));

        //    reader.Close();
        //    reader.Dispose();
        //    searcher.Close();
        //    searcher.Dispose();

        //    return GetLuceneToDataList(docs);

        //}

        //#region 검색
        ///// <summary>
        ///// 작성자 :
        ///// 작성일자 :
        ///// 작성목적 : 검색
        ///// </summary>
        ///// <param name="input"></param>
        ///// <param name="fieldName"></param>
        ///// <returns></returns>
        //public static IEnumerable<SearchInfo> Search(string input, string fieldName = "")
        //{
        //    if (string.IsNullOrEmpty(input)) return new List<SearchInfo>();

        //    var terms = input.Trim().Replace("-", " ").Split(' ').Where(x => !string.IsNullOrEmpty(x)).Select(x => x.Trim() + "*");
        //    input = string.Join(" ", terms);

        //    return _search(input, fieldName);
        //}

        ///// <summary>
        ///// 작성자 :
        ///// 작성일자 :
        ///// 작성목적 : 검색
        ///// </summary>
        ///// <param name="input"></param>
        ///// <param name="fieldName"></param>
        ///// <returns></returns>
        //public static IEnumerable<SearchInfo> SearchDefault(string input, string fieldName = "")
        //{
        //    return string.IsNullOrEmpty(input) ? new List<SearchInfo>() : _search(input, fieldName);
        //}

        ///// <summary>
        ///// 작성자 :
        ///// 작성일자 :
        ///// 작성목적 : 검색
        ///// </summary>
        ///// <param name="searchQuery"></param>
        ///// <param name="searchField"></param>
        ///// <returns></returns>
        //private static IEnumerable<SearchInfo> _search(string searchQuery, string searchField = "")
        //{
        //    if (string.IsNullOrEmpty(searchQuery.Replace("*", "").Replace("?", ""))) return new List<SearchInfo>();

        //    //lucene searcher 준비
        //    using (var searcher = new IndexSearcher(_directory, false))
        //    {
        //        var hits_limit = 1000;
        //        //var analyzer = new StandardAnalyzer(Version.LUCENE_29);
        //        //var analyzer = new Lucene.Net.Analysis.CJK.CJKAnalyzer(StopWords);
        //        var analyzer = new Lucene.Net.Analysis.CJK.CJKAnalyzer();

        //        // 단순 검색

        //        if (!string.IsNullOrEmpty(searchField))
        //        {
        //            var parser = new QueryParser(Version.LUCENE_29, searchField, analyzer);
        //            var query = parseQuery(searchQuery, parser);
        //            //var hits = searcher.Search(query, hits_limit).ScoreDocs;
        //            //var results = GetLuceneToDataList(hits, searcher);

        //            //========================================================================
        //            // 하이라이팅 기능 테스트
        //            var hits = searcher.Search(query);
        //            var formatter = new Lucene.Net.Highlight.SimpleHTMLFormatter(
        //                                    "<span class=\"highlighting\">",
        //                                    "</span>");

        //            var fragmenter = new Lucene.Net.Highlight.SimpleFragmenter(100);
        //            var scorer = new Lucene.Net.Highlight.QueryScorer(query);
        //            var highlighter = new Lucene.Net.Highlight.Highlighter(formatter, scorer);
        //            highlighter.SetTextFragmenter(fragmenter);

        //            List<SearchInfo> results = new List<SearchInfo>();

        //            for (int i = 0; i < hits.Length(); i++)
        //            {
        //                SearchInfo searchInfo = new SearchInfo();

        //                Lucene.Net.Documents.Document doc = hits.Doc(i);

        //                string contents = HttpUtility.HtmlEncode(doc.Get("ContentTEXT"));
        //                Lucene.Net.Analysis.TokenStream stream = analyzer.TokenStream("ContentTEXT", new StringReader(contents));

        //                List<Lucene.Net.Analysis.Token> tokenList = new List<Lucene.Net.Analysis.Token>();

        //                while (true)
        //                {
        //                    Lucene.Net.Analysis.Token token = stream.Next();
        //                    if (token == null) break;

        //                    tokenList.Add(token);
        //                }

        //                searchInfo.ID = doc.Get("ID");
        //                searchInfo.CompanyID = Convert.ToInt32(doc.Get("CompanyID"));
        //                searchInfo.Subject = doc.Get("Subject");
        //                searchInfo.ContentHTML = doc.Get("ContentHTML");
        //                searchInfo.ContentTEXT = highlighter.GetBestFragments(stream, contents, 1, "...");

        //                results.Add(searchInfo);
        //            }

        //            //========================================================================

        //            analyzer.Close();
        //            searcher.Close();
        //            searcher.Dispose();
        //            return results;
        //        }
        //        else  //복합검색
        //        {
        //            var parser = new MultiFieldQueryParser(Version.LUCENE_29, new[] { "ID", "CompanyID", "UserID", "Subject", "ContentTEXT" }, analyzer);
        //            var query = parseQuery(searchQuery, parser);
        //            //var hits = searcher.Search(query, null, hits_limit, Sort.INDEXORDER).ScoreDocs;  //new Sort(new SortField[]{new SortField("ObjectId"), new SortField("Subject", SortField.INT, true)})
        //            //var results = GetLuceneToDataList(hits, searcher);

        //            //========================================================================
        //            // 하이라이팅 기능 테스트
        //            var hits = searcher.Search(query);
        //            var formatter = new Lucene.Net.Highlight.SimpleHTMLFormatter(
        //                                    "<span class=\"highlighting\">",
        //                                    "</span>");

        //            var fragmenter = new Lucene.Net.Highlight.SimpleFragmenter(100);
        //            var scorer = new Lucene.Net.Highlight.QueryScorer(query);
        //            var highlighter = new Lucene.Net.Highlight.Highlighter(formatter, scorer);
        //            highlighter.SetTextFragmenter(fragmenter);

        //            List<SearchInfo> results = new List<SearchInfo>();

        //            for (int i = 0; i < hits.Length(); i++)
        //            {
        //                SearchInfo searchInfo = new SearchInfo();

        //                Lucene.Net.Documents.Document doc = hits.Doc(i);

        //                string contents = HttpUtility.HtmlEncode(doc.Get("ContentTEXT"));
        //                Lucene.Net.Analysis.TokenStream stream = analyzer.TokenStream("ContentTEXT", new StringReader(contents));

        //                List<Lucene.Net.Analysis.Token> tokenList = new List<Lucene.Net.Analysis.Token>();

        //                //while (true)
        //                //{
        //                //    Lucene.Net.Analysis.Token token = stream.Next();
        //                //    if (token == null) break;

        //                //    tokenList.Add(token);
        //                //}

        //                searchInfo.ID = doc.Get("ID");
        //                searchInfo.CompanyID = Convert.ToInt32(doc.Get("CompanyID"));
        //                searchInfo.Subject = doc.Get("Subject");
        //                searchInfo.ContentHTML = doc.Get("ContentHTML");
        //                searchInfo.ContentTEXT = highlighter.GetBestFragments(stream, contents, 1, "...");

        //                displayTokens(analyzer, doc.Get("ContentTEXT"));

        //                results.Add(searchInfo);
        //            }

        //            //========================================================================

        //            analyzer.Close();
        //            searcher.Close();
        //            searcher.Dispose();
        //            return results;
        //        }
        //    }
        //}

        //private static void displayTokens(Analyzer analyzer, String text)
        //{
        //    Lucene.Net.Analysis.Token[] tokens = tokensFromAnalysis(analyzer, text);
        //    string tokenString = String.Empty;
        //    for (int i = 0; i < tokens.Length; i++)
        //    {
        //        Lucene.Net.Analysis.Token token = tokens[i];

        //        tokenString += "[" + token.TermText() + "] ";
        //    }

        //    Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert(tokenString, Base.ApplicationFrameworks.Helper.AppSettingType.SEARCH);
        //}

        ///// <summary>
        ///// 상세 토큰 분석
        ///// </summary>
        ///// <param name="analyzer"></param>
        ///// <param name="text"></param>
        //private static void displayTokensWithFullDetails(Analyzer analyzer, String text)
        //{
        //    Lucene.Net.Analysis.Token[] tokens = tokensFromAnalysis(analyzer, text);

        //    int position = 0;

        //    for (int i = 0; i < tokens.Length; i++)
        //    {
        //        Lucene.Net.Analysis.Token token = tokens[i];

        //        int increment = token.GetPositionIncrement();

        //        if (increment > 0)
        //        {
        //            position = position + increment;

        //            Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("\r", Base.ApplicationFrameworks.Helper.AppSettingType.SEARCH);
        //            Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert(position + " : ", Base.ApplicationFrameworks.Helper.AppSettingType.SEARCH);
        //        }

        //        Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("[" + token.TermText() + ":" + token.StartOffset() + "->" + token.EndOffset() + ":" + token.Type() + "] ", Base.ApplicationFrameworks.Helper.AppSettingType.SEARCH);
        //    }
        //}

        //private static Lucene.Net.Analysis.Token[] tokensFromAnalysis(Analyzer analyzer, string Text)
        //{
        //    TokenStream stream = analyzer.TokenStream("contents", new StringReader(Text));

        //    //ArrayList tokenList = new ArrayList();

        //    List<Lucene.Net.Analysis.Token> tokenList = new List<Lucene.Net.Analysis.Token>();

        //    while (true)
        //    {
        //        Lucene.Net.Analysis.Token token = stream.Next();
        //        if (token == null) break;

        //        tokenList.Add(token);
        //    }

        //    //while (stream.IncrementToken())
        //    //{
        //    //    if (token == null) break;

        //    //    tokenList.Add(token);
        //    //}

        //    return (Lucene.Net.Analysis.Token[])tokenList.ToArray();
        //}

        ///// <summary>
        ///// 작성자 :
        ///// 작성일자 :
        ///// 작성목적 : 검색어에서 특수문자 제거
        ///// </summary>
        ///// <param name="searchQuery"></param>
        ///// <param name="parser"></param>
        ///// <returns></returns>
        //private static Query parseQuery(string searchQuery, QueryParser parser)
        //{
        //    Query query;
        //    try
        //    {
        //        query = parser.Parse(searchQuery.Trim());
        //    }
        //    catch (ParseException)
        //    {
        //        query = parser.Parse(QueryParser.Escape(searchQuery.Trim()));
        //    }

        //    return query;
        //}

        //#endregion

        //#region 인덱스 검색 색인
        ///// <summary>
        ///// 작성자 :
        ///// 작성일자 :
        ///// 작성목적 : 데이터 인덱스 검색 색인
        ///// </summary>
        ///// <param name="hits"></param>
        ///// <returns></returns>
        //private static IEnumerable<SearchInfo> GetLuceneToDataList(IEnumerable<Document> hits)
        //{
        //    return hits.Select(GetLuceneDocumentToData).ToList();
        //}

        //private static IEnumerable<SearchInfo> GetLuceneToDataList(IEnumerable<ScoreDoc> hits, IndexSearcher searcher)
        //{
        //    return hits.Select(hit => GetLuceneDocumentToData(searcher.Doc(hit.doc))).ToList();
        //}

        //private static SearchInfo GetLuceneDocumentToData(Document doc)
        //{
        //    return new SearchInfo
        //    {
        //        ID = doc.Get("ID"),
        //        CompanyID = Convert.ToInt32(doc.Get("CompanyID")),
        //        Subject = doc.Get("Subject"),
        //        ContentHTML = doc.Get("ContentHTML"),
        //        ContentTEXT =  doc.Get("ContentTEXT")
        //    };
        //}
        //#endregion

        //#region 인덱스 추가/수정/삭제
        ///// <summary>
        ///// 작성자 :
        ///// 작성일자 :
        ///// 작성목적 : 인덱스 추가/수정
        ///// </summary>
        ///// <param name="searchInfoArr"></param>
        //public static void AddUpdateLuceneIndex(IEnumerable<SearchInfo> searchInfoArr)
        //{
        //    //루씬 초기화
        //    //var analyzer = new StandardAnalyzer(Version.LUCENE_29);
        //    //var analyzer = new Lucene.Net.Analysis.CJK.CJKAnalyzer(StopWords);
        //    var analyzer = new Lucene.Net.Analysis.CJK.CJKAnalyzer();

        //    using (var writer = new IndexWriter(_directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
        //    {
        //        //루씬 검색 인덱스 추가(기존에 존재하면 replace)
        //        foreach (var searchInfo in searchInfoArr) AddToIndex(searchInfo, writer);

        //        analyzer.Close();
        //        writer.Close();
        //        writer.Dispose();
        //    }
        //}

        ///// <summary>
        ///// 작성자 :
        ///// 작성일자 :
        ///// 작성목적 : 인덱스 추가/수정
        ///// </summary>
        ///// <param name="searchInfo"></param>
        //public static void AddUpdateLuceneIndex(SearchInfo searchInfo)
        //{
        //    AddUpdateLuceneIndex(new List<SearchInfo> { searchInfo });
        //}

        ///// <summary>
        ///// 작성자 :
        ///// 작성일자 :
        ///// 작성목적 : 인덱스 ID 삭제
        ///// </summary>
        ///// <param name="record_id"></param>
        //public static void ClearLuceneIndexRecord(int record_id)
        //{
        //    //var analyzer = new StandardAnalyzer(Version.LUCENE_29);
        //    //var analyzer = new Lucene.Net.Analysis.CJK.CJKAnalyzer(StopWords);
        //    var analyzer = new Lucene.Net.Analysis.CJK.CJKAnalyzer();

        //    using (var writer = new IndexWriter(_directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
        //    {
        //        // 오래된 인덱스 삭제
        //        var searchQuery = new TermQuery(new Term("ID", record_id.ToString()));
        //        writer.DeleteDocuments(searchQuery);

        //        analyzer.Close();
        //        writer.Close();
        //        writer.Dispose();
        //    }
        //}

        ///// <summary>
        ///// 작성자 :
        ///// 작성일자 :
        ///// 작성목적 : 인덱스 전체 삭제
        ///// </summary>
        ///// <returns></returns>
        //public static bool ClearLuceneIndex()
        //{
        //    try
        //    {
        //        //var analyzer = new StandardAnalyzer(Version.LUCENE_29);
        //        //var analyzer = new Lucene.Net.Analysis.CJK.CJKAnalyzer(StopWords);
        //        var analyzer = new Lucene.Net.Analysis.CJK.CJKAnalyzer();

        //        using (var writer = new IndexWriter(_directory, analyzer, true, IndexWriter.MaxFieldLength.UNLIMITED))
        //        {
        //            writer.DeleteAll();

        //            analyzer.Close();
        //            writer.Close();
        //            writer.Close();
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        return false;
        //    }
        //    return true;
        //}

        ///// <summary>
        ///// 작성자 :
        ///// 작성일자 :
        ///// 작성목적 : 최적화
        ///// </summary>
        //public static void Optimize()
        //{
        //    //var analyzer = new StandardAnalyzer(Version.LUCENE_29);
        //    //var analyzer = new Lucene.Net.Analysis.CJK.CJKAnalyzer(StopWords);
        //    var analyzer = new Lucene.Net.Analysis.CJK.CJKAnalyzer();

        //    using (var writer = new IndexWriter(_directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
        //    {
        //        analyzer.Close();
        //        writer.Optimize();
        //        writer.Close();
        //        writer.Dispose();
        //    }
        //}

        ///// <summary>
        ///// 작성자 :
        ///// 작성일자 :
        ///// 작성목적 : 인덱스 추가
        ///// </summary>
        //private static void AddToIndex(SearchInfo searchInfo, IndexWriter writer)
        //{
        //    // 이전 ID 삭제
        //    var searchQuery = new TermQuery(new Term("ID", searchInfo.ID.ToString()));
        //    writer.DeleteDocuments(searchQuery);

        //    //신규 ID 생성
        //    var doc = new Document();

        //    doc.Add(new Field("ID", searchInfo.ID.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
        //    doc.Add(new Field("CompanyID", searchInfo.CompanyID.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
        //    //doc.Add(new Field("ObjectID", searchInfo.ObjectID.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
        //    //doc.Add(new Field("UserID", searchInfo.UserID.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
        //    doc.Add(new Field("Subject", searchInfo.Subject, Field.Store.YES, Field.Index.ANALYZED));
        //    doc.Add(new Field("ContentHTML", searchInfo.ContentHTML, Field.Store.YES, Field.Index.ANALYZED));
        //    doc.Add(new Field("ContentTEXT", searchInfo.ContentTEXT, Field.Store.YES, Field.Index.ANALYZED));

        //    writer.AddDocument(doc);
        //}

        ///// <summary>
        ///// STOP Word 리스트 가져오기
        ///// </summary>
        //private static string[] StopWords
        //{
        //    get
        //    {
        //        return File.ReadAllLines(_stopwordDir, Encoding.UTF8); ;
        //    }
        //}

        //#endregion
    }
}