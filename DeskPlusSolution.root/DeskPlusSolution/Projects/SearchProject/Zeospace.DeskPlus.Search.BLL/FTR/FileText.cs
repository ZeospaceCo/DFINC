namespace Zeospace.DeskPlus.Search.BLL.FTR
{
    public static class FileText
    {
        //public static string FTRDir = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, "ftr_index");

        //private static int indexID = 0;
        //private static FSDirectory _directoryTemp;
        //private static FSDirectory _directory
        //{
        //    get
        //    {
        //        if (_directoryTemp == null) _directoryTemp = FSDirectory.Open(new DirectoryInfo(FTRDir));
        //        if (IndexWriter.IsLocked(_directoryTemp)) IndexWriter.Unlock(_directoryTemp);

        //        var lockFilePath = Path.Combine(FTRDir, "write.lock");

        //        if (File.Exists(lockFilePath)) File.Delete(lockFilePath);
        //        return _directoryTemp;
        //    }

        //}

        //#region ADD()

        //public static void Add(string _dataDir)
        //{
        //    long startDate = new DateTime().Ticks;
        //    int numIndexed = IndexFile(_dataDir);
        //    long endDate = new DateTime().Ticks;

        //}

        //public static int IndexFile(string _dataDir)
        //{
        //    indexID = 0;
        //    //루씬 초기화
        //    //var analyzer = new StandardAnalyzer(Version.LUCENE_29);
        //    var analyzer = new Lucene.Net.Analysis.CJK.CJKAnalyzer();
        //    int numIndexed = 0;
        //    using (var writer = new IndexWriter(_directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
        //    {
        //        writer.SetUseCompoundFile(false);

        //        //루씬 검색 인덱스 추가(기존에 존재하면 replace)

        //        AddToIndex(_dataDir, writer);

        //        numIndexed = writer.MaxDoc();
        //        analyzer.Close();
        //        writer.Close();
        //        writer.Dispose();
        //    }

        //    return numIndexed;
        //}

        ///// <summary>
        ///// 작성자 :
        ///// 작성일자 :
        ///// 작성목적 : 인덱스 추가
        ///// </summary>
        ///// <param name="_id"></param>
        ///// <param name="_dataDir"></param>
        ///// <param name="writer"></param>
        //private static void AddToIndex(string _dataDir, IndexWriter writer)
        //{
        //    DirectoryInfo dir = new DirectoryInfo(_dataDir);
        //    DirectoryInfo[] childDir = dir.GetDirectories();

        //    foreach (DirectoryInfo dirInfo in childDir)
        //    {
        //        AddToIndex(dirInfo.FullName, writer);
        //    }

        //    FileInfo[] files = dir.GetFiles();

        //    foreach (FileInfo fi in files)
        //    {
        //        if (fi.FullName.ToLower().EndsWith(".txt"))
        //        {
        //            if (fi.IsReadOnly || !fi.Exists) continue;

        //            var doc = new Document();

        //            doc.Add(new Field("ID", indexID.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
        //            indexID++;
        //            doc.Add(new Field("FileName", fi.Name, Field.Store.YES, Field.Index.ANALYZED));

        //            //doc.Add(new Field("Contents", new System.IO.StreamReader(fi.FullName, System.Text.Encoding.Default)));
        //            FileStream _is = new FileStream(fi.FullName, FileMode.Open);
        //            //Encoding encoding = Encoding.Default;  // MCCHAE add encoding for 한글
        //            Encoding encoding = Encoding.UTF8;  // MCCHAE add encoding for 한글
        //            TextReader reader = new StreamReader(_is, encoding);
        //            string line;
        //            StringBuilder sb = new StringBuilder();
        //            while ((line = reader.ReadLine()) != null)
        //            {
        //                sb.Append(line + " ");
        //            }
        //            reader.Close();
        //            string Contents = sb.ToString();
        //            doc.Add(new Field("Contents", Contents, Field.Store.YES, Field.Index.ANALYZED, Lucene.Net.Documents.Field.TermVector.WITH_POSITIONS_OFFSETS));

        //            writer.AddDocument(doc);
        //        }
        //    }
        //}

        ///// <summary>
        ///// 작성자 :
        ///// 작성일자 :
        ///// 작성목적 : 인덱스 추가
        ///// </summary>
        ///// <param name="_id"></param>
        ///// <param name="_dataDir"></param>
        ///// <param name="writer"></param>
        //public static void AddToIndex(int _id, string _filePath)
        //{
        //    //루씬 초기화
        //    //var analyzer = new StandardAnalyzer(Version.LUCENE_29);
        //    var analyzer = new Lucene.Net.Analysis.CJK.CJKAnalyzer();
        //    int numIndexed = 0;
        //    using (var writer = new IndexWriter(_directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
        //    {
        //        writer.SetUseCompoundFile(false);

        //        //루씬 검색 인덱스 추가(기존에 존재하면 replace)

        //        FileInfo fi = new FileInfo(_filePath);

        //        if (fi.Name.ToLower().EndsWith(".txt"))
        //        {
        //            if (fi.IsReadOnly || !fi.Exists) return;

        //            // 이전 ID 삭제
        //            var searchQuery = new TermQuery(new Term("ID", _id.ToString()));
        //            writer.DeleteDocuments(searchQuery);

        //            var doc = new Document();

        //            doc.Add(new Field("ID", _id.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
        //            doc.Add(new Field("FileName", fi.Name, Field.Store.YES, Field.Index.ANALYZED));

        //            //doc.Add(new Field("Contents", new System.IO.StreamReader(fi.FullName, System.Text.Encoding.Default)));
        //            FileStream _is = new FileStream(fi.FullName, FileMode.Open);
        //            Encoding encoding = Encoding.Default;  // MCCHAE add encoding for 한글
        //            TextReader reader = new StreamReader(_is, encoding);
        //            string line;
        //            StringBuilder sb = new StringBuilder();
        //            while ((line = reader.ReadLine()) != null)
        //            {
        //                sb.Append(line + " ");
        //            }
        //            reader.Close();
        //            string Contents = sb.ToString();
        //            doc.Add(new Field("Contents", Contents, Field.Store.YES, Field.Index.ANALYZED, Lucene.Net.Documents.Field.TermVector.WITH_POSITIONS_OFFSETS));

        //            writer.AddDocument(doc);
        //        }

        //        numIndexed = writer.MaxDoc();
        //        analyzer.Close();
        //        writer.Close();
        //        writer.Dispose();
        //    }
        //}

        ///// <summary>
        ///// 작성자 :
        ///// 작성일자 :
        ///// 작성목적 : 검색
        ///// </summary>
        ///// <returns></returns>
        //public static IEnumerable<FTRInfo> GetAllIndexRecords()
        //{
        //    if (!System.IO.Directory.EnumerateFiles(FTRDir).Any()) return new List<FTRInfo>();

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

        ///// <summary>
        ///// 작성자 :
        ///// 작성일자 :
        ///// 작성목적 : 최적화
        ///// </summary>
        //public static void Optimize()
        //{
        //    //var analyzer = new StandardAnalyzer(Version.LUCENE_29);
        //    var analyzer = new Lucene.Net.Analysis.CJK.CJKAnalyzer();
        //    using (var writer = new IndexWriter(_directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
        //    {
        //        analyzer.Close();
        //        writer.Optimize();
        //        writer.Close();
        //        writer.Dispose();
        //    }
        //}

        //#endregion

        //#region DEL

        ///// <summary>
        ///// 작성자 :
        ///// 작성일자 :
        ///// 작성목적 : 인덱스 ID 삭제
        ///// </summary>
        ///// <param name="record_id"></param>
        //public static void ClearLuceneIndexRecord(int record_id)
        //{
        //    //var analyzer = new StandardAnalyzer(Version.LUCENE_29);
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

        //#endregion

        //#region GET

        //#region 검색
        ///// <summary>
        ///// 작성자 :
        ///// 작성일자 :
        ///// 작성목적 : 검색
        ///// </summary>
        ///// <param name="input"></param>
        ///// <param name="fieldName"></param>
        ///// <returns></returns>
        //public static IEnumerable<FTRInfo> Search(string input, string fieldName = "")
        //{
        //    if (string.IsNullOrEmpty(input)) return new List<FTRInfo>();

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
        //public static IEnumerable<FTRInfo> SearchDefault(string input, string fieldName = "")
        //{
        //    return string.IsNullOrEmpty(input) ? new List<FTRInfo>() : _search(input, fieldName);
        //}

        ///// <summary>
        ///// 작성자 :
        ///// 작성일자 :
        ///// 작성목적 : 검색
        ///// </summary>
        ///// <param name="searchQuery"></param>
        ///// <param name="searchField"></param>
        ///// <returns></returns>
        //private static IEnumerable<FTRInfo> _search(string searchQuery, string searchField = "")
        //{
        //    //if (string.IsNullOrEmpty(searchQuery.Replace("*", "").Replace("?", "")))
        //        return new List<FTRInfo>();

        //    ////lucene searcher 준비
        //    //using (var searcher = new IndexSearcher(_directory, false))
        //    //{
        //    //    var hits_limit = 1000;
        //    //    var analyzer = new StandardAnalyzer(Version.LUCENE_29);

        //    //    // 단순 검색

        //    //    if (!string.IsNullOrEmpty(searchField))
        //    //    {
        //    //        var parser = new QueryParser(Version.LUCENE_29, searchField, analyzer);
        //    //        var query = parseQuery(searchQuery, parser);
        //    //        //var hits = searcher.Search(query, hits_limit).ScoreDocs;

        //    //        //if (query.GetType() == typeof(Lucene.Net.Search.PrefixQuery)) {
        //    //        //        ((Lucene.Net.Search.PrefixQuery)query).SetRewriteMethod(Lucene.Net.Search.PrefixQuery.SCORING_BOOLEAN_QUERY_REWRITE);
        //    //        //}

        //    //        Lucene.Net.Search.Hits hits = searcher.Search(query);

        //    //        var formatter = new Lucene.Net.Highlight.SimpleHTMLFormatter(
        //    //            "<span class=\"highlighting\">",
        //    //            "</span>");

        //    //        var fragmenter = new Lucene.Net.Highlight.SimpleFragmenter(100);
        //    //        var scorer = new Lucene.Net.Highlight.QueryScorer(query);
        //    //        var highlighter = new Lucene.Net.Highlight.Highlighter(formatter, scorer);
        //    //        highlighter.SetTextFragmenter(fragmenter);

        //    //        List<FTRInfo> results = new List<FTRInfo>();

        //    //        for (int i = 0; i < hits.Length(); i++)
        //    //        {
        //    //            FTRInfo ftrInfo = new FTRInfo();
        //    //            Lucene.Net.Documents.Document doc = hits.Doc(i);

        //    //            string contents = HttpUtility.HtmlEncode(doc.Get("Contents"));
        //    //            Lucene.Net.Analysis.TokenStream stream = analyzer.TokenStream("Contents", new StringReader(contents));

        //    //            List<Lucene.Net.Analysis.Token> tokenList = new List<Lucene.Net.Analysis.Token>();

        //    //            while (true)
        //    //            {
        //    //                Lucene.Net.Analysis.Token token = stream.Next();
        //    //                if (token == null) break;

        //    //                tokenList.Add(token);
        //    //            }

        //    //            ftrInfo.ID = Convert.ToInt32(doc.Get("ID"));
        //    //            ftrInfo.FileName = doc.Get("FileName");
        //    //            ftrInfo.Contents = highlighter.GetBestFragments(stream, contents, 1, "...");

        //    //            results.Add(ftrInfo);
        //    //        }

        //    //        analyzer.Close();
        //    //        searcher.Close();
        //    //        searcher.Dispose();

        //    //        return results;

        //    //        //var parser = new QueryParser(Version.LUCENE_29, searchField, analyzer);
        //    //        //var query = parseQuery(searchQuery, parser);
        //    //        //var hits = searcher.Search(query, hits_limit).ScoreDocs;
        //    //        //var results = GetLuceneToDataList(hits, searcher);
        //    //        //analyzer.Close();
        //    //        //searcher.Close();
        //    //        //searcher.Dispose();
        //    //        //return results;
        //    //    }
        //    //    else  //복합검색
        //    //    {
        //    //        var parser = new MultiFieldQueryParser(Version.LUCENE_29, new[] { "ID", "FileName", "Contents" }, analyzer);
        //    //        var query = parseQuery(searchQuery, parser);
        //    //        var hits = searcher.Search(query, null, hits_limit, Sort.INDEXORDER).ScoreDocs;
        //    //        var results = GetLuceneToDataList(hits, searcher);
        //    //        analyzer.Close();
        //    //        searcher.Close();
        //    //        searcher.Dispose();
        //    //        return results;
        //    //    }
        //    //}
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
        //#endregion

        //#region 인덱스 검색 색인
        ///// <summary>
        ///// 작성자 :
        ///// 작성일자 :
        ///// 작성목적 : 데이터 인덱스 검색 색인
        ///// </summary>
        ///// <param name="hits"></param>
        ///// <returns></returns>
        //private static IEnumerable<FTRInfo> GetLuceneToDataList(IEnumerable<Document> hits)
        //{
        //    return hits.Select(GetLuceneDocumentToData).ToList();
        //}

        //private static IEnumerable<FTRInfo> GetLuceneToDataList(IEnumerable<ScoreDoc> hits, IndexSearcher searcher)
        //{
        //    return hits.Select(hit => GetLuceneDocumentToData(searcher.Doc(hit.doc))).ToList();
        //}

        //private static FTRInfo GetLuceneDocumentToData(Document doc)
        //{
        //    return new FTRInfo
        //    {
        //        ID = Convert.ToInt32(doc.Get("ID")),
        //        FileName = doc.Get("FileName"),
        //        Contents = doc.Get("Contents"),
        //    };
        //}
        //#endregion
    }
}