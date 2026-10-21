using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Zeospace.DeskPlus.Component.Converter;
using Zeospace.DeskPlus.Search.BE.Address;

namespace Zeospace.DeskPlus.Search.BLL.Address
{
    public static class Index
    {
        public static string IndexDir = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, @"SearchIndex\Address");
        //public static string FTRDir = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, @"SearchFTR\Address\");

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

        #region 인덱스 추가/수정/삭제

        #region Address록리스트

        /// <summary>
        /// 작성자 : freeman
        /// 작성일자 : 2015.02.11
        /// 작성목적 : 인덱스 추가/수정
        /// </summary>
        /// <param name="searchInfoArr"></param>
        public static int AddIndex(IEnumerable<ListInfo> searchInfoArr)
        {
            //루씬 초기화
            //var analyzer = new StandardAnalyzer(Version.LUCENE_29);
            //var analyzer = new Lucene.Net.Analysis.CJK.CJKAnalyzer(StopWords);
            var analyzer = new Lucene.Net.Analysis.CJK.CJKAnalyzer();

            using (var writer = new IndexWriter(_directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
            {
                //루씬 검색 인덱스 추가(기존에 존재하면 replace)
                foreach (var searchInfo in searchInfoArr) AddToIndex(searchInfo, writer);

                analyzer.Close();
                writer.Close();
                writer.Dispose();
            }

            return searchInfoArr.Count<ListInfo>();
        }

        /// <summary>
        /// 작성자 : freeman
        /// 작성일자 : 2015.02.11
        /// 작성목적 : 인덱스 추가/수정
        /// </summary>
        /// <param name="searchInfo"></param>
        public static void AddIndex(ListInfo searchInfo)
        {
            AddIndex(new List<ListInfo> { searchInfo });
        }

        /// <summary>
        /// 작성자 : freeman
        /// 작성일자 : 2015.02.11
        /// 작성목적 : 인덱스 추가
        /// </summary>
        private static void AddToIndex(ListInfo searchInfo, IndexWriter writer)
        {
            // 이전 ID 삭제
            var searchQuery = new TermQuery(new Term("ID", searchInfo.ID.ToString()));
            writer.DeleteDocuments(searchQuery);

            //신규 ID 생성
            var doc = new Document();
            doc.Add(new Field("Type", "T", Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field("ID", searchInfo.ID.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field("CompanyID", searchInfo.CompanyID.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field("GroupID", searchInfo.GroupID.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field("CategoryID", searchInfo.CategoryID.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field("ObjectID", searchInfo.ObjectID.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field("UserID", searchInfo.UserID.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field("Name", searchInfo.Name.ToString(), Field.Store.YES, Field.Index.ANALYZED));
            doc.Add(new Field("Mail", searchInfo.Mail, Field.Store.YES, Field.Index.ANALYZED));
            doc.Add(new Field("DeptName", searchInfo.DeptName, Field.Store.YES, Field.Index.ANALYZED));
            doc.Add(new Field("Company", searchInfo.Company, Field.Store.YES, Field.Index.ANALYZED));
            doc.Add(new Field("MobilePhone", searchInfo.MobilePhone, Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field("OfficePhone", searchInfo.OfficePhone, Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field("Title", searchInfo.Title, Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field("Duty", searchInfo.Duty, Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field("Memo", searchInfo.Memo, Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field("WriteDate", searchInfo.WriteDate, Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field("NameBase", searchInfo.NameBase, Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field("DeptBase", searchInfo.DeptBase, Field.Store.YES, Field.Index.NOT_ANALYZED));

            writer.AddDocument(doc);
        }

        /// <summary>
        /// 작성자 : freeman
        /// 작성일자 : 2015.02.11
        /// 작성목적 : 인덱스 ID 삭제
        /// </summary>
        /// <param name="recordId"></param>
        public static void DelIndexRecord(int recordId)
        {
            //var analyzer = new StandardAnalyzer(Version.LUCENE_29);
            //var analyzer = new Lucene.Net.Analysis.CJK.CJKAnalyzer(StopWords);
            var analyzer = new Lucene.Net.Analysis.CJK.CJKAnalyzer();

            using (var writer = new IndexWriter(_directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
            {
                // 오래된 인덱스 삭제
                var searchQuery = new TermQuery(new Term("ID", recordId.ToString()));
                writer.DeleteDocuments(searchQuery);

                analyzer.Close();
                writer.Close();
                writer.Dispose();
            }
        }

        /// <summary>
        /// 작성자 : freeman
        /// 작성일자 : 2015.02.11
        /// 작성목적 : 인덱스 전체 삭제
        /// </summary>
        /// <returns></returns>
        public static bool DelIndex()
        {
            try
            {
                //var analyzer = new StandardAnalyzer(Version.LUCENE_29);
                //var analyzer = new Lucene.Net.Analysis.CJK.CJKAnalyzer(StopWords);
                var analyzer = new Lucene.Net.Analysis.CJK.CJKAnalyzer();

                using (var writer = new IndexWriter(_directory, analyzer, true, IndexWriter.MaxFieldLength.UNLIMITED))
                {
                    writer.DeleteAll();

                    analyzer.Close();
                    writer.Close();
                    writer.Close();
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 작성자 : freeman
        /// 작성일자 : 2015.02.11
        /// 작성목적 : 최적화
        /// </summary>
        public static void Optimize()
        {
            //var analyzer = new StandardAnalyzer(Version.LUCENE_29);
            //var analyzer = new Lucene.Net.Analysis.CJK.CJKAnalyzer(StopWords);
            var analyzer = new Lucene.Net.Analysis.CJK.CJKAnalyzer();

            using (var writer = new IndexWriter(_directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
            {
                analyzer.Close();
                writer.Optimize();
                writer.Close();
                writer.Dispose();
            }
        }

        #endregion 주소록리스트

        #endregion 인덱스 추가/수정/삭제
    }
}