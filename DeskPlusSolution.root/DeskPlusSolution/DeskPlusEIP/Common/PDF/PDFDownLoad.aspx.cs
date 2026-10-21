using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Common.Web.PDF
{
    public partial class PDFDownLoad : Base.BasePageCommon
    {
        #region 변수설정

        private string Module = string.Empty;
        private int ListID = 0;//고유키값
        private string Number2 = string.Empty;
        private int CatID = 0;
        #endregion

        #region Page Process (페이지 처리)

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보

            this.ResourceName = "Zeospace.DeskPlus.Common.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Common.Web.PDF.PDFDownLoad).Assembly;	//어셈블리 선언

            #endregion

            if (!IsPostBack)
            {
                LoadParam();

                if (CurrentUserInfo.CompanyID.Equals(0) || CurrentUserInfo.UserID.Equals(0) || ListID.Equals(0)) return;

                /*
                if (Directory.Exists(Server.MapPath(FileSpool)))
                {
                    FileHelper.DelFolder(FileSpool, false);
                }                
                */

                LoadData();
            }
        }

        #endregion

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        /// <summary>
        /// 작성자 :
        /// 작성일자 :
        /// 작성목적 : Loads the data.
        /// </summary>
        private void LoadData()
        {
            string rtnpath = "";
            rtnpath = GetPDFLog();

            #region 파일 다운로드
            if (!rtnpath.Equals(""))
            {
                if (rtnpath.IndexOf("Result.pdf") < 0)
                {
                    rtnpath = rtnpath + "Result.pdf";
                }

                if (Response.IsClientConnected)
                {
                    String header = Request.Headers["User-Agent"];

                    Response.AddHeader("Content-Disposition", "attachment;filename=PrintAll_" + DateTime.Today.ToShortDateString() + ".pdf");
                    Response.ContentType = "application/pdf";
                    // 파일 다운로드 시키기
                    Response.TransmitFile(Server.MapPath(rtnpath));
                    Response.Flush();
                    //Response.Close();
                }
                else
                {
                    Response.End();
                }
            }
            #endregion
        }

        #endregion

        #region (함수)

        /// <summary>
        /// 작성자 :
        /// 작성일자 :
        /// 작성목적 : 파라미터
        /// </summary>
        private void LoadParam()
        {
            Module = GetDecryptParam("module", "");
            ListID = GetDecryptParam("listID", "0").ToNumber<int>();

            //POST 방식으로 암호화처리 하지 않고 넘김(결재->게시판이관용)
            if (Module == "" && ListID.Equals(0))
            {
                Module = ((string)(Request.Params["module"] ?? "") == "") ? "" : (string)(Request.Params["module"] ?? "");
                ListID = ((string)(Request.Params["listID"] ?? "") == "") ? 0 : (Request.Params["listID"]).ToNumber<int>();
            }

            //Number2 = GetDecryptParam("num2", "");            

            //if (Number2.Equals(""))
            //{
            //    Number2 = ((string)(Request.Params["num2"] ?? "") == "") ? "" : (string)(Request.Params["num2"] ?? "");
            //}

            //CatID = GetDecryptParam("catID", "0").ToNumber<int>();

            //if(CatID.Equals(0))
            //{
            //    CatID = ((string)(Request.Params["catID"] ?? "") == "") ? 0 : (Request.Params["catID"]).ToNumber<int>();
            //}

        }

        /// <summary>
        /// PDF정보 가져오기
        /// </summary>
        /// <returns></returns>
        private string GetPDFLog()
        {
            try
            {
                string filePath = string.Empty;
                DAL.CreatePDF dal = new DAL.CreatePDF();
                DataSet ds = null;

                ds = dal.GetPDFLog(CurrentUserInfo.CompanyID, Module, ListID);
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    //filePath = ds.Tables[0].Rows[0]["FilePath"].ToString() + ds.Tables[0].Rows[0]["FileName"].ToString();
                    filePath = ds.Tables[0].Rows[0]["FilePath"].ToString();
                }

                return filePath;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetPDFLog", ex);
                return "";
            }
        }

        #endregion (함수)
    }
}