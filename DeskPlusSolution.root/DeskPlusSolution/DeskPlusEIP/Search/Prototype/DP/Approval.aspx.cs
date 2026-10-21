using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Search.BE;
using Zeospace.DeskPlus.Search.BLL;
using Zeospace.DeskPlus.Search.BLL.FTR;
using System.Diagnostics;
using Lucene.Net.Index;

namespace Zeospace.DeskPlus.Search.Web.Prototype.DP
{
    public partial class Approval : Zeospace.DeskPlus.Search.Web.Base.BasePageSearch
    {
        //#region 변수설정
        //public string SearchValue = string.Empty;
        //public static string _luceneDir = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, "Search_Index");
        //#endregion

        //#region Page Process (페이지 처리)
        //protected void Page_Load(object sender, EventArgs e)
        //{
        //    #region 다국어 리소스 로드/Title 정보
        //    #endregion

        //    if (!IsPostBack)
        //    {
        //        //인덱스 폴더 생성
        //        if (!Directory.Exists(LuceneSearch._luceneDir)) Directory.CreateDirectory(LuceneSearch._luceneDir);

        //        //ZEO 파일을 변환하기 위한 임시 폴더 청소
        //        TempZEOPath = Server.MapPath("/DeskPlusEIP/FileData/" + CurrentUserInfo.CompanyID + "/Search/TempZEO/");
        //        DirectoryInfo dir = new DirectoryInfo(TempZEOPath);
        //        if (dir.Exists)
        //        {
        //            string[] folder;
        //            folder = Directory.GetDirectories(TempZEOPath);

        //            foreach (string folderName in folder)
        //            {
        //                Directory.Delete(folderName, true);
        //            }
        //        }

        //        //FTR 파일을 변환하기 위한 폴더 청소
        //        FTRPath = Server.MapPath("/DeskPlusEIP/FileData/" + CurrentUserInfo.CompanyID + "/Search/FTRText/");
        //        DirectoryInfo dirFTR = new DirectoryInfo(FTRPath);
        //        if (dirFTR.Exists)
        //        {
        //            string[] folder;
        //            folder = Directory.GetDirectories(FTRPath);

        //            foreach (string folderName in folder)
        //            {
        //                Directory.Delete(folderName, true);
        //            }
        //        }

        //        LoadData();

        //    }



        //}
        //#endregion

        //#region Method (함수)

        ///// <summary>
        ///// 메서드명   :
        ///// 작성목적   : 내용가져오기
        ///// 작성자     : 
        ///// 최초작성일 : 
        ///// </summary>
        //private void LoadData()
        //{
        //    this.tbID.Text = "";
        //    this.tbName.Text = "";
        //    this.tbSubject.Text = "";
        //    this.tbContent.Text = "";

        //    //첨부파일 데이타 로드
        //    Zeospace.DeskPlus.Search.DAL.Approval dal = new Zeospace.DeskPlus.Search.DAL.Approval();
        //    gvAttach.DataSource = dal.GetAttach();
        //    gvAttach.DataBind();

        //    //테스트 데이타 로드
        //    gvData.DataSource = dal.GetAll();
        //    gvData.DataBind();

        //    //인덱스 데이타 로드
        //    gvIndex.DataSource = Zeospace.DeskPlus.Search.BLL.LuceneSearch.GetAllIndexRecords();
        //    gvIndex.DataBind();


        //}
        //#endregion

        //#region Event Handler (이벤트 핸들러)
        ///// <summary>
        ///// 인덱스 추가
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //protected void lbtnAddIndex_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        Zeospace.DeskPlus.Search.DAL.Approval dal = new Zeospace.DeskPlus.Search.DAL.Approval();
        //        //LuceneSearch.AddUpdateLuceneIndex(dal.GetAll());

        //        this.lbOKMsg.Text = "인덱스 생성 성공 : " + dal.GetAll().Count + " 건";

        //        GetReaderInfo();
        //    }
        //    catch (Exception ex)
        //    {
        //        this.lbFailMsg.Text = "인덱스 생성 실패 : " + ex.Message;

        //        GetReaderInfo();
        //    }
        //    LoadData();
        //}

        ///// <summary>
        ///// 인덱스 옵티마이져
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //protected void lbtnOptimize_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        LuceneSearch.Optimize();

        //        this.lbOKMsg.Text = "옵티마이져 성공";

        //        GetReaderInfo();
        //    }
        //    catch (Exception ex)
        //    {
        //        this.lbFailMsg.Text = "옵티마이져 실패 : " + ex.Message;

        //        GetReaderInfo();
        //    }
        //    LoadData();
        //}

        ///// <summary>
        ///// 인덱스 삭제
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //protected void lbtnDelIndex_Click(object sender, EventArgs e)
        //{
        //    if (LuceneSearch.ClearLuceneIndex())
        //    {
        //        this.lbOKMsg.Text = "인덱스 삭제";

        //        GetReaderInfo();
        //    }
        //    else
        //    {
        //        this.lbFailMsg.Text = "삭제 실패 (lock)";

        //        GetReaderInfo();
        //    }

        //    LoadData();
        //}

        ///// <summary>
        ///// 인덱스 추가
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //protected void btnAddIndex_Click(object sender, EventArgs e)
        //{
        //    SearchInfo searchInfo = new SearchInfo();
        //    searchInfo.ID = Guid.NewGuid().ToString();
        //    searchInfo.CompanyID = CurrentUserInfo.CompanyID;
        //    searchInfo.UserID = CurrentUserInfo.UserID;
        //    //searchInfo.ObjectID = Convert.ToInt32(this.tbID.Text);
        //    //searchInfo.ManyName = this.tbName.Text;
        //    searchInfo.Subject = this.tbSubject.Text;
        //    searchInfo.ContentHTML = this.tbContent.Text;
        //    searchInfo.ContentTEXT = this.tbID.Text;

        //    LuceneSearch.AddUpdateLuceneIndex(searchInfo);

        //    LoadData();
        //}

        ///// <summary>
        ///// 사용자 정의 검색
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //protected void btnCustomSearch_Click(object sender, EventArgs e)
        //{
        //    string searchTerm = this.tbCustomSearch.Text;
        //    string searchField = this.ddlCustomSearchField.SelectedValue;

        //    gvResult.DataSource = string.IsNullOrEmpty(searchField) ? LuceneSearch.Search(searchTerm) : LuceneSearch.Search(searchTerm, searchField);
        //    gvResult.DataBind();
        //}

        ///// <summary>
        ///// 기본검색
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //protected void btnSearch_Click(object sender, EventArgs e)
        //{
        //    string searchTerm = this.tbSearch.Text;
        //    string searchField = this.ddlSearchField.SelectedValue;

        //    gvResult.DataSource = string.IsNullOrEmpty(searchField) ? LuceneSearch.SearchDefault(searchTerm) : LuceneSearch.SearchDefault(searchTerm, searchField);
        //    gvResult.DataBind();
        //}

        ///// <summary>
        ///// 첨부파일 Convert
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //protected void lbtnConvertFTR_Click(object sender, EventArgs e)
        //{
        //    /*
        //      * zeo 파일이면 
        //        - Y : 특정 폴더로 원본 확장자로 카피
        //        - N : ConvertPath로 변환  (파일명 변환여부 확인)
        //      * 특정 폴더에 카피된 zeo 파일 변환
        //      ProcessStartInfo startInfo = new ProcessStartInfo(@"C:\FlashPaper\FlashPrinter.exe");
        //      startInfo.Arguments = @"C:\1.doc -o C:\1.swf";
        //      Process.Start(startInfo);
        //     * */

        //    Zeospace.DeskPlus.Search.DAL.Approval dal = new Zeospace.DeskPlus.Search.DAL.Approval();
        //    List<BE.Approval.AttachFileInfo> al = dal.GetAttach();



        //    //ZEO 파일을 변환하기위한 임시 폴더 생성
        //    if (!Directory.Exists(TempZEOPath)) Directory.CreateDirectory(TempZEOPath);
        //    //FTR을 하기위한 Text 파일로 변환
        //    if (!Directory.Exists(FTRPath)) Directory.CreateDirectory(FTRPath);

        //    string filePath = String.Empty;
        //    string targetPath = string.Empty;
        //    foreach (BE.AttachInfo attachInfo in al)
        //    {
        //        // 확장자가 .ZEO 이면
        //        if (attachInfo.FileGUID.EndsWith(".zeo"))
        //        {
                    
        //            filePath = Server.MapPath(attachInfo.FilePath) + attachInfo.FileGUID;
        //            targetPath = TempZEOPath + attachInfo.FileGUID.Replace(".zeo", attachInfo.FileType);
        //            if (File.Exists(filePath)) File.Copy(filePath, targetPath,true);
        //        }
        //        else
        //        {
        //            filePath = Server.MapPath(attachInfo.FilePath) + attachInfo.FileGUID;
        //            targetPath = FTRPath + attachInfo.FileGUID.Substring(0, attachInfo.FileGUID.IndexOf('.')) + ".txt";
        //            GetTextFile(filePath, targetPath);
        //            FileText.AddToIndex(attachInfo.AttachFileID, targetPath);
        //        }
        //    }


        //    if (Directory.Exists(TempZEOPath))
        //    {
        //        //.zeo 파일이 원래 파일명으로 파일 Copy가 다 되었는지는 모르겠지만 우선 진행함.
        //        string[] files;
        //        files = Directory.GetFiles(TempZEOPath, "*.txt");

        //        foreach (string file in files)
        //        {
        //            targetPath = FTRPath + file.Substring(0, file.IndexOf('.')) + ".txt";
        //            GetTextFile(file, targetPath);
        //        }
        //    }

        //}

        //private bool GetTextFile(string _filePath, string _targetPath)
        //{
        //    try
        //    {
        //        if (!File.Exists(_filePath)) return false;
        //        string exePath = Server.MapPath("/DeskPlusEIP/Search/Util/a2tcmd.exe");
        //        ProcessStartInfo startInfo = new ProcessStartInfo(exePath);

        //        //_filePath = Server.MapPath("/DeskPlusEIP/Search/Util/3e49a271-77d3-4aaf-a8e8-e4e2041d0279.xlsx");
        //        //_targetPath = Server.MapPath("/DeskPlusEIP/Search/Util/3e49a271-77d3-4aaf-a8e8-e4e2041d0279.txt");
        //        //string arg = String.Format("{0} {1}", _filePath, _targetPath);
        //        //startInfo.Arguments = arg; //.Replace(@"\", @"/");

        //        startInfo.Arguments = String.Format("{0} {1}", _filePath, _targetPath);
        //        startInfo.UseShellExecute= false;
        //        startInfo.RedirectStandardInput = true;
        //        startInfo.RedirectStandardOutput = true;
        //        startInfo.RedirectStandardError = true;

        //        string exitMsg = "";
        //        int exitCode = 1; 


        //        using (Process proc = Process.Start(startInfo))
        //        {
        //            exitMsg = proc.StandardOutput.ReadToEnd();
        //            proc.WaitForExit(1000);
        //            exitCode = proc.ExitCode;

        //            proc.Close();
        //            proc.Dispose();
        //        } 


        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }
        //}

        //private void GetReaderInfo()
        //{
        //    using (var reader = IndexReader.Open(_luceneDir, true))
        //    {
        //        this.lbOKMsg.Text = this.lbOKMsg.Text + "<br/> Index Reader Test - MaxDoc : " + reader.MaxDoc() + " NumDocs : " + reader.NumDocs();


        //        reader.Dispose();
        //        reader.Close();
        //    }
        //}
        //#endregion



        //#region Property (프로퍼티)
        //public string TempZEOPath
        //{
        //    get { return GetViewStateString("TempZEOPath", ""); }
        //    set { ViewState["TempZEOPath"] = value; }
        //}

        //public string FTRPath
        //{
        //    get { return GetViewStateString("FTRPath", ""); }
        //    set { ViewState["FTRPath"] = value; }
        //} 
        //#endregion

        //protected void lbtnIndexReader_Click(object sender, EventArgs e)
        //{
        //    using (var reader = IndexReader.Open(_luceneDir, true))
        //    {
        //        this.lbOKMsg.Text = "Index Reader Test - MaxDoc : " + reader.MaxDoc() + " NumDocs : " + reader.NumDocs();


        //        reader.Dispose();
        //        reader.Close();
        //    }
        //}
    }
}