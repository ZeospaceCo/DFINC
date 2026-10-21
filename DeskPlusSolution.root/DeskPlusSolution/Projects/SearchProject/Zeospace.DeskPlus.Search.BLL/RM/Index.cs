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
using Zeospace.DeskPlus.Search.BE.RM;

namespace Zeospace.DeskPlus.Search.BLL.RM
{
    public static class Index
    {
        public static string IndexDir = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, @"SearchIndex\RM");
        public static string FTRDir = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, @"SearchFTR\RM\");

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

        #region 자원관리리스트

        /// <summary>
        /// 작성자 : freeman
        /// 작성일자 : 2015.12.28
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
        /// 작성일자 : 2015.12.28
        /// 작성목적 : 인덱스 추가/수정
        /// </summary>
        /// <param name="searchInfo"></param>
        public static void AddIndex(ListInfo searchInfo)
        {
            AddIndex(new List<ListInfo> { searchInfo });
        }

        /// <summary>
        /// 작성자 : freeman
        /// 작성일자 : 2015.12.28
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
            doc.Add(new Field("ListID", searchInfo.ListID.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field("GroupID", searchInfo.GroupID.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field("ReservationID", searchInfo.ReservationID.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field("Title", searchInfo.Title.ToString(), Field.Store.YES, Field.Index.ANALYZED));
            doc.Add(new Field("Subject", searchInfo.Subject, Field.Store.YES, Field.Index.ANALYZED));
            doc.Add(new Field("Content", searchInfo.Content, Field.Store.YES, Field.Index.ANALYZED));
            doc.Add(new Field("WriteDate", searchInfo.WriteDate, Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field("WriteUserID", searchInfo.WriteUserID, Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field("WriteUserName", searchInfo.WriteUserName, Field.Store.YES, Field.Index.ANALYZED));

            doc.Add(new Field("FileName", searchInfo.Title, Field.Store.YES, Field.Index.ANALYZED));
            doc.Add(new Field("FilePath", searchInfo.FilePath, Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field("FileGUID", searchInfo.FileGUID, Field.Store.YES, Field.Index.NOT_ANALYZED));

            writer.AddDocument(doc);
        }

        /// <summary>
        /// 작성자 : freeman
        /// 작성일자 : 2015.12.28
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
        /// 작성일자 : 2015.12.28
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
        /// 작성일자 : 2015.12.28
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

        #endregion 자원관리리스트

        #region 첨부파일

        /// <summary>
        /// 작성자 : freeman
        /// 작성일자 : 2015.12.28
        /// 작성목적 : 인덱스 추가/수정
        /// </summary>
        /// <param name="searchInfoArr"></param>
        public static void AddFTRIndex(IEnumerable<AttachFileInfo> searchInfoArr)
        {
            //루씬 초기화
            //var analyzer = new StandardAnalyzer(Version.LUCENE_29);
            //var analyzer = new Lucene.Net.Analysis.CJK.CJKAnalyzer(StopWords);
            var analyzer = new Lucene.Net.Analysis.CJK.CJKAnalyzer();

            using (var writer = new IndexWriter(_directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
            {
                //루씬 검색 인덱스 추가(기존에 존재하면 replace)
                foreach (var searchInfo in searchInfoArr) AddToFTRIndex(searchInfo, writer);

                analyzer.Close();
                writer.Close();
                writer.Dispose();
            }
        }

        /// <summary>
        /// 작성자 : freeman
        /// 작성일자 : 2015.12.28
        /// 작성목적 : 인덱스 추가/수정
        /// </summary>
        /// <param name="attachFileInfo"></param>
        /// <param name="writer"></param>
        private static void AddToFTRIndex(AttachFileInfo attachFileInfo, IndexWriter writer)
        {
            try
            {
                // 이전 ID 삭제
                var searchQuery = new TermQuery(new Term("ID", attachFileInfo.ID.ToString()));
                writer.DeleteDocuments(searchQuery);

                //신규 ID 생성
                var doc = new Document();
                doc.Add(new Field("Type", "F", Field.Store.YES, Field.Index.NOT_ANALYZED));
                doc.Add(new Field("ID", attachFileInfo.ID.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
                doc.Add(new Field("CompanyID", attachFileInfo.CompanyID.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
                doc.Add(new Field("ListID", attachFileInfo.ListID.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
                doc.Add(new Field("GroupID", attachFileInfo.GroupID.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
                doc.Add(new Field("ReservationID", attachFileInfo.ReservationID.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
                doc.Add(new Field("Title", attachFileInfo.Title, Field.Store.YES, Field.Index.ANALYZED));
                doc.Add(new Field("Subject", attachFileInfo.Subject, Field.Store.YES, Field.Index.ANALYZED));
                doc.Add(new Field("WriteDate", attachFileInfo.WriteDate, Field.Store.YES, Field.Index.NOT_ANALYZED));
                doc.Add(new Field("WriteUserID", attachFileInfo.WriteUserID, Field.Store.YES, Field.Index.NOT_ANALYZED));
                doc.Add(new Field("WriteUserName", attachFileInfo.WriteUserName, Field.Store.YES, Field.Index.ANALYZED));

                doc.Add(new Field("FileName", attachFileInfo.FileName, Field.Store.YES, Field.Index.ANALYZED));
                doc.Add(new Field("FilePath", attachFileInfo.FilePath, Field.Store.YES, Field.Index.NOT_ANALYZED));
                doc.Add(new Field("FileGUID", attachFileInfo.FileGUID, Field.Store.YES, Field.Index.NOT_ANALYZED));

                // 파일 명 CategoryID_ListID_AttachFileID.txt
                string fileName = Path.Combine(FTRDir, attachFileInfo.CompanyID + "_" + attachFileInfo.GroupID + "_" + attachFileInfo.ListID + "_" + attachFileInfo.AttachFileID + ".txt");
                FileInfo fi = new FileInfo(fileName);

                if (!fi.IsReadOnly && fi.Exists)
                {
                    //doc.Add(new Field("Contents", new System.IO.StreamReader(fi.FullName, System.Text.Encoding.Default)));
                    FileStream _is = new FileStream(fi.FullName, FileMode.Open);
                    //Encoding encoding = Encoding.Default;  // encoding for 한글
                    Encoding encoding = Encoding.UTF8;  // encoding for 한글
                    TextReader reader = new StreamReader(_is, encoding);
                    string line;
                    StringBuilder sb = new StringBuilder("");
                    while ((line = reader.ReadLine()) != null)
                    {
                        sb.Append(line + " ");
                    }
                    reader.Close();
                    string Contents = sb.ToString();
                    doc.Add(new Field("Content", Contents, Field.Store.YES, Field.Index.ANALYZED, Lucene.Net.Documents.Field.TermVector.WITH_POSITIONS_OFFSETS));
                }
                else
                {
                    doc.Add(new Field("Content", "", Field.Store.YES, Field.Index.NOT_ANALYZED));
                }

                writer.AddDocument(doc);
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("AddToFTRIndex : " + ex.Message, ex);
            }
        }

        /// <summary>
        /// 작성자 : freeman
        /// 작성일자 : 2015.12.28
        /// 작성목적 : Text 파일 변환
        /// </summary>
        /// <param name="attachFileInfo"></param>
        /// <param name="writer"></param>
        public static int GetTextFile(IEnumerable<AttachFileInfo> searchInfoArr)
        {
            //폴더 한곳에 모든 txt 파일을 넣을 경우 속도에 영향을 받을까?  폴더당 최대 파일수 : 4,294,967,295
            //파일 명 CategoryID_ListID_AttachFileID.txt
            HttpContext context = HttpContext.Current;
            string sourcePath = String.Empty;
            string targetPath = String.Empty;
            int rtnCnt = 0;

            //초기화
            if (System.IO.Directory.Exists(FTRDir))
            {
                System.IO.Directory.Delete(FTRDir, true);
                System.IO.Directory.CreateDirectory(FTRDir);
            }
            else
            {
                System.IO.Directory.CreateDirectory(FTRDir); //폴더 존재 확인
            }

            DirectoryInfo di = new DirectoryInfo(FTRDir);

            foreach (var searchInfo in searchInfoArr)
            {
                sourcePath = context.Server.MapPath(searchInfo.FilePath + searchInfo.FileGUID);
                targetPath = FTRDir + searchInfo.CompanyID + "_" + searchInfo.GroupID + "_" + searchInfo.ListID + "_" + searchInfo.AttachFileID + ".txt";

                FileInfo f = new FileInfo(sourcePath);

                if (f.Exists)
                {
                    switch (f.Extension.ToLower())
                    {
                        case ".doc":
                            if (WordToText.GetText(sourcePath, targetPath)) rtnCnt++;

                            Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("doc : " + targetPath, Base.ApplicationFrameworks.Helper.AppSettingType.SEARCH);
                            break;

                        case ".docx":
                            if (WordToText.GetText(sourcePath, targetPath)) rtnCnt++;
                            Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("docx : " + targetPath, Base.ApplicationFrameworks.Helper.AppSettingType.SEARCH);
                            break;

                        case ".xls":
                            if (CellToText.GetText(sourcePath, targetPath)) rtnCnt++;
                            Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("xls : " + targetPath, Base.ApplicationFrameworks.Helper.AppSettingType.SEARCH);
                            break;

                        case ".xlsx":
                            if (CellToText.GetText(sourcePath, targetPath)) rtnCnt++;
                            Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("xlsx : " + targetPath, Base.ApplicationFrameworks.Helper.AppSettingType.SEARCH);
                            break;

                        case ".ppt":
                            if (SlideToText.GetPptToText(sourcePath, targetPath)) rtnCnt++;
                            Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("ppt : " + targetPath, Base.ApplicationFrameworks.Helper.AppSettingType.SEARCH);
                            break;

                        case ".pptx":
                            if (SlideToText.GetPptxToText(sourcePath, targetPath)) rtnCnt++;
                            Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("pptx : " + targetPath, Base.ApplicationFrameworks.Helper.AppSettingType.SEARCH);
                            break;

                        case ".pdf":
                            if (PdfToText.GetText(sourcePath, targetPath)) rtnCnt++;
                            break;
                        //case ".eml":

                        //    break;
                    }
                }
                else
                {
                    Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetTextFile : The file does not exist " + sourcePath);
                }
            }
            return rtnCnt;
        }

        #endregion 첨부파일

        #endregion 인덱스 추가/수정/삭제
    }
}