using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Text.RegularExpressions;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Collections.Generic;
using System.Text;
using Zeospace.DeskPlus.PMS.BE;

namespace Zeospace.DeskPlus.PMS.Web.Unit
{
    public partial class View : Base.BasePagePMS
    {
        #region Declaration (컨트롤 선언)

        #region 변수설정                     
        public int UnitID;
        public string Status = string.Empty;
        private string eventArgs = String.Empty;
        #endregion

        #endregion

        #region Page Process (페이지 처리)
        protected void Page_Load(object sender, System.EventArgs e)
        {
            #region 다국어 리소스 로드
            ResourceName = "Zeospace.DeskPlus.PMS.Web.Resource.GlobalString.string";			//리소스 명 선언
            AssemblyName = typeof(Zeospace.DeskPlus.PMS.Web.Unit.View).Assembly;	//어셈블리 선언
            GetResourceManager();
            #endregion

            if (!IsPostBack)
            {
                LoadParam();
                SetLanguage();
                LoadData();
            }


            #region 버튼 이미지
         
            #endregion            
        }
        #endregion

        #region Data Load (컨트롤에 데이터를 바인딩 하는 메소드)
        private void LoadData()
        {
            #region 사용자정보가져오기	
            #endregion            

            //자료 뿌려주기
            LoadDataContent();
        }
        

        #endregion

        #region Method (함수)

        /// <summary>
        /// 다국어 설정
        /// </summary>
        private void SetLanguage()
        {
            if (!this.resMng.Equals(null))
            {
                ltlMenuName.Text = resMng.GetString("Unit_Register_ltlTitle");
                cbtnAdd.Text = resMng.GetString("Writing");
                cbtnMod.Text = resMng.GetString("Mod");
                cbtnDel.Text = resMng.GetString("Unit_Register_Delete");
                cbtnPrint.Text = resMng.GetString("Print");
                cbtnCancel.Text = resMng.GetString("List");
                cbtnShortReply.Text = resMng.GetString("WriteShortReply");

                ltlChargeDeptName.Text = resMng.GetString("Unit_Register_ltlChargeDeptName");
                ltlCharger.Text = resMng.GetString("Unit_Register_ltlCharger");
                ltlCode.Text = resMng.GetString("Unit_Register_ltlCode");
                ltlExecuteMoney.Text = resMng.GetString("Unit_Register_ltlExecuteMoney");
                ltlPlanEndDate.Text = resMng.GetString("Unit_Register_ltlPlanEndDate");
                ltlPlanMoney.Text = resMng.GetString("Unit_Register_ltlPlanMoney");
                ltlPlanStartDate.Text = resMng.GetString("Unit_Register_ltlPlanStartDate");
                ltlProcess.Text = resMng.GetString("Unit_Register_ltlProcess");
                ltlRelateDeptName.Text = resMng.GetString("Unit_Register_ltlRelateDeptName");
                ltlStartDate.Text = resMng.GetString("Unit_Register_ltlStartDate");
                ltlEndDate.Text = resMng.GetString("Unit_Register_ltlEndDate");
                ltlStatus.Text = resMng.GetString("Unit_Register_ltlStatus");
                ltlTitle.Text = resMng.GetString("Unit_Register_ltlTitle");
                ltlWeightPercent.Text = resMng.GetString("Unit_Register_ltlWeightPercent");
                ltlWriteName.Text = resMng.GetString("Unit_Owner");
                ltlWriteDate.Text = resMng.GetString("Read_WriteDate");
                ltlPlanDay.Text = resMng.GetString("Unit_Register_ltlPlanDay");
            }
        }

        /// <summary>
        /// 파라미터 정의
        /// </summary>
        private void LoadParam()
        {   
            UnitID = GetDecryptParam("unitID", "0").ToNumber<int>();            
            Status = GetDecryptParam("status", "0");
        }

        /// <summary>
        /// Recs the status.
        /// </summary>
        /// <param name="_status">The _status.</param>
        /// <returns></returns>
        public string GetStatus(string _status)
        {
            if (_status.Length == 0) return "";
            string response = "";

            switch (_status)
            {
                case "1":
                    response = GetResource("List_Status1");
                    break;
                case "2":
                    response = GetResource("List_Status2");
                    break;
                case "3":
                    response = GetResource("List_Status3");
                    break;
                case "4":
                    response = GetResource("List_Status4");
                    break;
                case "5":
                    response = GetResource("List_Status5");
                    break;
            }


            return response;
        }


        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 자료 가져오기
        /// 작성자     : 심재운
        /// 최초작성일 : 2008-03-27
        /// </summary>
        private void LoadDataContent()
        {

            #region 첨부파일 가져오기
            
            StringBuilder sbShowImage = new StringBuilder();
            DAL.UnitAttach attachfile = new Zeospace.DeskPlus.PMS.DAL.UnitAttach();
            List<BE.UnitAttachInfo> attachList = attachfile.GetAttachFile(CurrentUserInfo.CompanyID, ProjectID, UnitID);

            foreach (BE.UnitAttachInfo info in attachList)
            {
                if (info.FileType.ToUpper() == ".GIF"
                    || info.FileType.ToUpper() == ".JPG"
                    || info.FileType.ToUpper() == ".PNG"
                    || info.FileType.ToUpper() == ".BMP"
                    || info.FileType.ToUpper() == ".EMF")
                {
                    if (System.IO.File.Exists(Server.MapPath(info.FilePath + info.FileGUID)))
                    {
                        #region 이미지 경로 암호화
                        string images = info.FilePath + info.FileGUID;
                        Dictionary<string, string> paraDic = new Dictionary<string, string>();
                        paraDic.Add("filePath", images);
                        string encParams = GetEncryptParam(Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.GetParameterDic(paraDic));
                        images = "/DeskPlusEIP/Common/DownLoad/ImageView.aspx?p=" + encParams;
                        #endregion

                        sbShowImage.Append(string.Format("<br><br><br><img id='{0}' class='shrink-me' src='{1}' alt='{2}' title='{2}' ><br>[{2}]",
                            info.AttachFileID, images, info.FileName));
                    }
                }
            }

            #endregion


            #region 내용 가져오기
            DAL.Unit list = new Zeospace.DeskPlus.PMS.DAL.Unit();

            //1. 내용 가져오기
            var alList = list.GetUnit(CurrentUserInfo.CompanyID, ProjectID, UnitID);
            foreach (var listInfo in alList)
            {

                if (listInfo != null)
                {
                    //담당부서
                    if (listInfo.ChargeDeptID.Length > 0)
                    {
                        Zeospace.DeskPlus.Account.DAL.Dept deptDal = new Account.DAL.Dept();
                        var deptList = deptDal.GetDeptLists(CurrentUserInfo.CompanyID, listInfo.ChargeDeptID, true);
                        string deptName = "";
                        foreach (var deptInfo in deptList)
                        {
                            deptName += this.GetLangName(
                                    deptInfo.NameBase,
                                    deptInfo.NameKo, deptInfo.NameEn, deptInfo.NameJa,
                                    deptInfo.NameFr, deptInfo.NameZhcn, deptInfo.NameZhtw, PreLang).ToReverseXSS() + ",";
                        }

                        lblChargeDeptName.Text = deptName.TrimEnd(',');
                    }

                    //관련부서
                    if (listInfo.RelateDeptID.Length > 0)
                    {
                        Zeospace.DeskPlus.Account.DAL.Dept deptDal = new Account.DAL.Dept();
                        var deptList = deptDal.GetDeptLists(CurrentUserInfo.CompanyID, listInfo.RelateDeptID, true);
                        string deptName = "";
                        foreach (var deptInfo in deptList)
                        {
                            deptName += this.GetLangName(
                                    deptInfo.NameBase,
                                    deptInfo.NameKo, deptInfo.NameEn, deptInfo.NameJa,
                                    deptInfo.NameFr, deptInfo.NameZhcn, deptInfo.NameZhtw, PreLang).ToReverseXSS() + ",";
                        }

                        lblRelateDeptName.Text = deptName.TrimEnd(',');
                    }

                    
                    lblCode.Text = listInfo.Code;

                    lblExecuteMoney.Text = String.Format("{0:#,###}", listInfo.ExecuteMoney);
                    lblProcess.Text = listInfo.Process.ToString();
                    
                    lblStatus.Text = GetStatus(listInfo.Status.ToString());
                    lblTitle.Text = listInfo.Subjects;
                    lblWeightPercent.Text = listInfo.WeightPercent.ToString();
                    lblWriteDate.Text = listInfo.WriteDate.ToShortDateString();
                    //lblWriteName.Text = Component.WebComponent.StringHelper.GetNameDBPreLang(PreLang, listInfo.WriteUserName);
                    
                    lblWriteName.Text = this.GetLangName(
                                    listInfo.NameBase,
                                    listInfo.NameKo, listInfo.NameEn, listInfo.NameJa,
                                    listInfo.NameFr, listInfo.NameZhcn, listInfo.NameZhtw, PreLang).ToReverseXSS();

                    lblPlanMoney.Text = String.Format("{0:#,###}", listInfo.PlanMoney);
                    lblPlanDay.Text = listInfo.PlanDay.ToString();

                    lblPlanEndDate.Text = (listInfo.PlanEndDate.ToNullDT() == System.DBNull.Value) ? "" : listInfo.PlanEndDate.ToShortDateString();
                    lblPlanStartDate.Text = (listInfo.PlanStartDate.ToNullDT() == System.DBNull.Value) ? "" : listInfo.PlanStartDate.ToShortDateString();
                    lblEndDate.Text = (listInfo.EndDate.ToNullDT() == System.DBNull.Value) ? "" : listInfo.EndDate.ToShortDateString();
                    lblStartDate.Text = (listInfo.StartDate.ToNullDT() == System.DBNull.Value) ? "" : listInfo.StartDate.ToShortDateString();

                    string tmpContent = Regex.Replace(listInfo.Contents, "</body>\r\n</html>", "</body></html>", RegexOptions.IgnoreCase);
                    listInfo.Contents = Regex.Replace(tmpContent, "</body></html>", sbShowImage.ToString() + "</body></html>", RegexOptions.IgnoreCase);
                    FormContent = listInfo.Contents;
                    pnContent.Controls.Add(new LiteralControl(listInfo.Contents));                   

                    #region 작성일자

                    lblWriteDate.Text = listInfo.WriteDate.ToShortDateString();

                    #endregion

                    int writeUserID = listInfo.WriteUserID;

                    #region 수정버튼 활성화 여부

                    //cbsEdit.IsDisabled = (writeUserID == UserID) ? "0" : "1"; //수정모드 비활성화
                    //cbsDel.IsDisabled = (writeUserID == UserID) ? "0" : "1"; //삭제버튼 활성화	

                    #endregion


                    #region 이름

                    #endregion

                }


                #region 책임자 정보 가져오기
                DAL.UnitCharger charger = new Zeospace.DeskPlus.PMS.DAL.UnitCharger();
                List<BE.UnitChargerInfo> al = charger.GetUnitCharger(CurrentUserInfo.CompanyID, ProjectID, UnitID);

                if (al != null)
                {
                    string chargerValue = "";
                    foreach (BE.UnitChargerInfo be in al)
                    {
                        //chargerValue += Component.WebComponent.StringHelper.GetNameDBPreLang(PreLang, be.UserName.Trim()) + ",";                        

                        chargerValue += this.GetLangName(
                                    be.NameBase,
                                    be.NameKo, be.NameEn, be.NameJa,
                                    be.NameFr, be.NameZhcn, be.NameZhtw, PreLang).ToReverseXSS() + ",";
                    }

                    chargerValue = chargerValue.TrimEnd(',');

                    lblCharger.Text = chargerValue;
                }
                #endregion
            }


            #endregion
        }


        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 작성자 이름설정하기
        /// 작성자     : 심재운
        /// 최초작성일 : 2008-03-27
        /// </summary>
        /// <param name="_writeUserName">Name of the _write user.</param>
        /// <returns></returns>
        private string GetWriteUserName(string _writeUserName)
        {
            //return Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.StringHelper.GetNameDBPreLang(PreLang, _writeUserName);
            return _writeUserName;
        }



        
        #endregion

        #region Property (프로퍼티)

        #region viewstate

        

        /// <summary>
        /// Gets or sets the content of the form.
        /// </summary>
        /// <value>The content of the form.</value>
        public string FormContent
        {
            get
            {
                return GetViewStateString("FormContent", "");
            }
            set
            {
                ViewState["FormContent"] = value;
            }
        }


        /// <summary>
        /// Gets or sets the img URL.
        /// </summary>
        /// <value>The img URL.</value>
        public string ImgUrl
        {
            get
            {
                return GetViewStateString("ImgUrl", "");
            }
            set
            {
                ViewState["ImgUrl"] = value;
            }
        }


        /// <summary>
        /// 첨부파일 경로
        /// </summary>
        /// <value>The files info.</value>
        public string AttachFilesInfo
        {
            get
            {
                return GetViewStateString("AttachFilesInfo", "");
            }
        }


        /// <summary>
        /// 첨부파일 개수
        /// </summary>
        /// <value>The attach files count.</value>
        public string AttachFilesCount
        {
            get
            {
                return GetViewStateString("AttachFilesCount", "");
            }
        }        

        #endregion

        #endregion

        #region Event Handler (이벤트 핸들러)


        /// <summary>
        /// Dels this instance.
        /// </summary>
        private void Del()
        {
            LoadParam();
            BE.UnitInfo be = new UnitInfo();
            be.CompanyID = CurrentUserInfo.CompanyID;
            be.ProjectID = ProjectID;
            be.UnitID = UnitID;
            be.WriteUserID = CurrentUserInfo.UserID;

            BLL.Unit bll = new Zeospace.DeskPlus.PMS.BLL.Unit();

            if (bll.Del(be))
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "success", GetResource("MSG_SUCCESS"), "");
            }
            else
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "fail", GetResource("MSG_FAIL"), "");
            }

            Dictionary<string, string> paraDic = new Dictionary<string, string>();
            paraDic.Add("projectID", ProjectID.ToString());
            paraDic.Add("menuID", MenuID.ToString());
            paraDic.Add("menuType", MenuType.ToString());            
            paraDic.Add("currentPage", CurrentPage.ToString());
            paraDic.Add("recordPerPage", RecordPerPage.ToString());
            paraDic.Add("searchField", SearchField);
            paraDic.Add("searchValue", SearchValue);
            paraDic.Add("searchTerm", SearchTerm);
            paraDic.Add("searchTermValue", SearchTermValue);
            paraDic.Add("order", Order);

            string encparm = GetEncryptParam(Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.GetParameterDic(paraDic));
            string url = "JList.aspx?p=" + encparm;

            // 자바스크립트 주소 이동하기
            string scripts = @"<script type=""text/javascript"">location = '" + url + "';</script>";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "goPage", scripts);
        }
                

        /// <summary>
        /// Handles the Click event of the LBTNProcess control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void LBTNProcess_Click(object sender, EventArgs e)
        {
            try
            {
                LoadParam();
                string args = eventArgs;
                switch (args)
                {
                    case "DEL":  //삭제
                        Del();
                        break;
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("LBTNProcess_Click", ex);
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "fail", GetResource("ProcessingError"), "");
            }
        }

        /// <summary>
        /// 들어오는 포스트백 이벤트를 처리하도록 포스트백을 발생시킨 서버 컨트롤에 알립니다.
        /// </summary>
        /// <param name="sourceControl">포스트백을 발생시킨 ASP.NET 서버 컨트롤입니다.이 컨트롤은 <see cref="T:System.Web.UI.IPostBackEventHandler"/> 인터페이스를 구현해야 합니다.</param>
        /// <param name="eventArgument">포스트백 인수입니다.</param>
        protected override void RaisePostBackEvent(IPostBackEventHandler sourceControl, string eventArgument)
        {
            eventArgs = eventArgument;
            base.RaisePostBackEvent(sourceControl, eventArgument);
        }

        #endregion
    }
}
