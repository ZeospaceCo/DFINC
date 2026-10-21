using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Search.BE;
using Zeospace.DeskPlus.Search.BLL.FTR;
using Zeospace.DeskPlus.Component.Converter;

namespace Zeospace.DeskPlus.Search.Web.Prototype
{
    public partial class FTRExam : Zeospace.DeskPlus.Search.Web.Base.BasePageSearch
    {
        //#region 변수설정
        //public string SearchValue = string.Empty;
        //#endregion

        //#region Page Process (페이지 처리)
        //protected void Page_Load(object sender, EventArgs e)
        //{
        //    #region 다국어 리소스 로드/Title 정보
        //    #endregion

        //    if (!IsPostBack)
        //    {
        //        //인덱스 폴더 생성
        //        if (!Directory.Exists(FileText.FTRDir)) Directory.CreateDirectory(FileText.FTRDir);

        //        this.lbOKMsg.Text = "";
        //        this.lbFailMsg.Text = "";

        //    }

        //    LoadData();

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

        //    //테스트 데이타 로드

        //    //인덱스 데이타 로드
        //    gvIndex.DataSource = Zeospace.DeskPlus.Search.BLL.FTR.FileText.GetAllIndexRecords();
        //    gvIndex.DataBind();
        //}
        //#endregion

        //#region Event Handler (이벤트 핸들러)

        ///// <summary>
        ///// Text 변환 테스트
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //protected void lbtnToText_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        string sourcePath = Server.MapPath(tbToText.Text);
        //        string targetPath = tbFolder.Text;
        //        //FileText.Add(folederPath);
        //        long startDate = new DateTime().Ticks;
        //        OfficeToText.GetDirToText(sourcePath, targetPath);
        //        long endDate = new DateTime().Ticks;

        //        this.lbOKMsg.Text = "TXT 생성 성공</br>startDate : " + startDate + "</br> endDate : " + endDate + "</br>";
        //    }
        //    catch (Exception ex)
        //    {
        //        this.lbFailMsg.Text = "TXT 생성 실패 : " + ex.Message;
        //    }
        //}

        ///// <summary>
        ///// 인덱스 추가
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //protected void lbtnAddIndex_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        string folederPath = tbFolder.Text;
        //        //FileText.Add(folederPath);

        //        long startDate = new DateTime().Ticks;
        //        int numIndexed = FileText.IndexFile(folederPath);
        //        long endDate = new DateTime().Ticks;


        //        this.lbOKMsg.Text = "인덱스 생성 성공</br>startDate : " + startDate + "</br> endDate : " + endDate + "</br> Number : " + numIndexed;
        //    }
        //    catch (Exception ex)
        //    {
        //        this.lbFailMsg.Text = "인덱스 생성 실패 : " + ex.Message;
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
        //        FileText.Optimize();

        //        this.lbOKMsg.Text = "옵티마이져 성공";
        //    }
        //    catch (Exception ex)
        //    {
        //        this.lbFailMsg.Text = "옵티마이져 실패 : " + ex.Message;
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
        //    if (FileText.ClearLuceneIndex())
        //    {
        //        this.lbOKMsg.Text = "인덱스 삭제";
        //    }
        //    else
        //    {
        //        this.lbFailMsg.Text = "삭제 실패 (lock)";
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
        //    int ID = Convert.ToInt32(this.tbID.Text);
        //    string FileName = this.fup.FileName;


        //    FileText.AddToIndex(ID, FileName);

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

        //    gvResult.DataSource = string.IsNullOrEmpty(searchField) ? FileText.Search(searchTerm) : FileText.Search(searchTerm, searchField);
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

        //    gvResult.DataSource = string.IsNullOrEmpty(searchField) ? FileText.SearchDefault(searchTerm) : FileText.SearchDefault(searchTerm, searchField);
        //    gvResult.DataBind();
        //}


        //#endregion


        //#region Property (프로퍼티)

        //#endregion
    }
}