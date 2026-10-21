using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Data;
using System.Text;

namespace Zeospace.DeskPlus.Approval.Web.Admin.FormGeneration
{
    public partial class ModSequence : Base.BasePageApprovalAdmin
    {
        #region Declaration (컨트롤 선언)
        #endregion

        #region 변수설정
        public string FolderList = "";
        public int FormID = 0;
        #endregion

        #region Page Process (페이지 처리)
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Approval.Web.Resource.GlobalString.string";   //리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.Admin.FormGeneration.ModSequence).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                LoadParm();
                SetLanguage();
                LoadData();
            }
        }
        #endregion
        
        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)
        #endregion

        #region (함수)
        /// <summary>
        /// 파라미터처리
        /// </summary>
        private void LoadParm()
        {
            //파라미터
            FormID = GetDecryptParam("formID", "0").ToNumber<int>();
        }
        /// <summary>
        /// 다국어 설정
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                cbtnConfirm.Text = resMng.GetString("Confirm");
                cbtnClose.Text = resMng.GetString("Close");
            }
        }

        /// <summary>
        /// 데이터가져오기
        /// </summary>
        private void LoadData()
        {
            StringBuilder sb = new StringBuilder();

            DAL.FormSeq fsDal = new DAL.FormSeq();
            List<BE.FormSeqInfo> fsList = fsDal.GetFormSeq(CurrentUserInfo.CompanyID, FormID);

            if (fsList != null)
            {
                string cssTag = "";
                foreach (BE.FormSeqInfo fsInfo in fsList)
                {
                    if (fsInfo.ApprovalTag.IndexOf("FLOW") == -1)
                    {
                        cssTag = fsInfo.ApprovalTag.Substring(0, 2);

                        switch (fsInfo.ApprovalTag.Substring(0, 2))
                        {
                            case "ZD":
                                if (fsInfo.ApprovalTag == "ZD0")
                                    fsInfo.ApprovalTag = resMng.GetString("ZD0");  //담당
                                else
                                    fsInfo.ApprovalTag = fsInfo.ApprovalTag.Replace("ZD", resMng.GetString("ApproveS"));    //결재
                                break;
                            case "ZH":
                                fsInfo.ApprovalTag = fsInfo.ApprovalTag.Replace("ZH", resMng.GetString("ZH"));    //협조
                                break;
                            case "ZP":
                                fsInfo.ApprovalTag = fsInfo.ApprovalTag.Replace("ZP", resMng.GetString("Receive")) + resMng.GetString("RejectNo"); //수신(반려불가)
                                break;
                            case "ZQ":
                                fsInfo.ApprovalTag = fsInfo.ApprovalTag.Replace("ZQ", resMng.GetString("Receive")) + resMng.GetString("RejectYes"); //수신(반려가능)
                                break;
                            case "ZC":
                                fsInfo.ApprovalTag = fsInfo.ApprovalTag.Replace("ZC", resMng.GetString("Carboncopy"));    //참조
                                break;
                            case "ZR":
                                fsInfo.ApprovalTag = fsInfo.ApprovalTag.Replace("ZR", resMng.GetString("ZR"));    //공람
                                break;
                            default:
                                break;
                        }
                    }
                    else
                    {
                        cssTag = fsInfo.ApprovalTag.Replace("FLOW", "");

                        switch (fsInfo.ApprovalTag)
                        {
                            case "FLOWZD":
                                fsInfo.ApprovalTag = fsInfo.ApprovalTag.Replace("FLOWZD", resMng.GetString("FlowZD"));//유동결재
                                break;
                            case "FLOWZH":
                                fsInfo.ApprovalTag = fsInfo.ApprovalTag.Replace("FLOWZH", resMng.GetString("FlowZH"));//유동협조
                                break;
                            case "FLOWZA":
                                fsInfo.ApprovalTag = fsInfo.ApprovalTag.Replace("FLOWZA", resMng.GetString("FlowZA"));//유동합의
                                break;
                            case "FLOWZR":
                                fsInfo.ApprovalTag = fsInfo.ApprovalTag.Replace("FLOWZR", resMng.GetString("FlowZR"));//유동공람
                                break;
                            default:
                                break;
                        }
                    }                    

                    sb.Append("<li id='");
                    sb.Append(fsInfo.FormSeqID.ToString());
                    sb.Append("' class='" + cssTag + "'>");
                    sb.Append(fsInfo.ApprovalTag);          //by.withsuki 명칭변경필요
                    sb.Append("</li>");
                }
            }
            FolderList = sb.ToString();
        }
        #endregion

        #region (프로퍼티)
        #endregion

        #region (이벤트핸들러)
        #endregion  

    }
}