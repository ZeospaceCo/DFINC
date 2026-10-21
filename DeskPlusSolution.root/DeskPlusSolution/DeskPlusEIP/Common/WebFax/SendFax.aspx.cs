using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Auth;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using System.Data;
using Zeospace.DeskPlus.Component.Linked.WebFax;
using System.IO;
using System.Collections;
using System.Text;
using Zeospace.DeskPlus.Component.Linked.WebFax.Entity;


namespace Zeospace.DeskPlus.Common.Web.WebFax
{
    public partial class SendFax : Base.BasePageCommon
    {
        #region 변수설정        
        private string eventArgs = String.Empty;
        public int OwnerID;
        public string Gubun = String.Empty;
        #endregion

        #region Page Process (페이지 처리)
        #endregion

        #region Method (함수)

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Common.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Common.Web.WebFax.SendFax).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                SwfUploadGUID = System.Guid.NewGuid().ToString().Replace("-","");
                SetLanguage();
                LoadData();
            }
        }

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 내용가져오기
        /// 작성자     : 심재운
        /// 최초작성일 : 2011-04-25
        /// </summary>
        private void LoadData()
        {
            LoadParam();
        }


        /// <summary>
        /// Loads the param.
        /// </summary>
        private void LoadParam()
        {
            OwnerID = GetDecryptParam("ownerID", "0").ToNumber<int>();
            Gubun = GetDecryptParam("gubun", "");

            if (OwnerID == 0)
            {
                if (!Page.ClientScript.IsClientScriptBlockRegistered("SendFaxClose"))
                {
                    string strScript = @"<script type=""text/javascript"">alert('{0}');self.close();" + "<" + "/script>";

                    switch (Gubun)
                    {
                        case "approval" :
                            strScript = String.Format(strScript, GetResource("FaxUseAfterCheckAppDoc"));
                            break;
                        case "approvalCir":
                            strScript = String.Format(strScript, GetResource("FaxUseAfterCheckAppCirDoc"));
                            break;
                        default :
                            break;
                    }
                    
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "SendFaxClose", strScript);
                }
            }
        }

        /// <summary>
        /// 다국어 설정
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                cbtnAdd.Text = resMng.GetString("Add");
                cbtnAddress.Text = resMng.GetString("FaxAddressAdd");
                cbtnSendFax.Text = resMng.GetString("FaxSendFax");
            }
        }

        /// <summary>
        /// 추가
        /// </summary>
        private bool Add()
        {
            bool response = false;            

            //comName + "^" + userName + "^" + faxNum
            string faxUserList = hdnFaxUser.Text.Trim().TrimEnd('$');
            faxUserList = Server.UrlDecode(faxUserList);

            if (faxUserList.Length > 0)
            {
                #region 송신자 정보

                string comName = "";
                Zeospace.DeskPlus.Common.DAL.Company comDal = new DAL.Company();
                var comList = comDal.GetCompany(true);
                foreach (var info in comList)
                {
                    if (info.CompanyID == CurrentUserInfo.CompanyID)
                    {
                        comName = this.GetLangName(
                            info.NameBase, info.NameKo, info.NameEn, info.NameJa, info.NameFr, info.NameZhcn, info.NameZhtw, PreLang);
                    }
                }

                string branchName = "";
                Zeospace.DeskPlus.Account.DAL.User dal = new Account.DAL.User();
                var al = dal.GetUserInfo(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, true, true);
                if (al.Count > 0)
                {
                    branchName = this.GetLangName(
                             al[0].DeptBase, al[0].DeptKo, al[0].DeptEn, al[0].DeptJa, al[0].DeptFr,
                             al[0].DeptZhCn, al[0].DeptZhTw, PreLang);
                }

                FaxInfo faxInfo = new FaxInfo();
                faxInfo.CompanyID = CurrentUserInfo.CompanyID;
                faxInfo.ClientNo = ""; //필수
                faxInfo.Dept = comName + ":" + branchName;//회사명:지회명
                faxInfo.UserName = CurrentUserInfo.Name;
                faxInfo.Email = CurrentUserInfo.MailID + "@" + CurrentUserInfo.MailDomain;
                faxInfo.UserCode = CurrentUserInfo.LogInID; //필수                
                faxInfo.Key = System.Guid.NewGuid().ToString().Replace("-", "");//GUID(필수)
                faxInfo.Subject = cbtnSubject.Text.Trim();                

                #region 본문내용 + 첨부파일

                string filePath = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
                filePath = filePath + "{0}/Users/{1}/Spool/";
                string spoolPath = string.Format(filePath, CurrentUserInfo.CompanyID, CurrentUserInfo.UserID);
                string fileDirPath = Server.MapPath(spoolPath + SwfUploadGUID + ".htm");

                FaxInfo tmpFaxInfo = new FaxInfo();
                Zeospace.DeskPlus.Common.BLL.WebFax appBiz = new BLL.WebFax();

                //결재 본문 내용 HTML 생성하기
                if (Gubun == "approval" && OwnerID > 0)
                {
                    tmpFaxInfo = appBiz.GetAppDoc(CurrentUserInfo.CompanyID, OwnerID, SwfUploadGUID, fileDirPath);
                    faxInfo.FileCount = tmpFaxInfo.FileCount;
                    faxInfo.FileNameList = tmpFaxInfo.FileNameList;
                } 
                else if (Gubun == "approvalCir" && OwnerID > 0)
                {
                    tmpFaxInfo = appBiz.GetAppDocCir(CurrentUserInfo.CompanyID, OwnerID, SwfUploadGUID, fileDirPath);
                    faxInfo.FileCount = tmpFaxInfo.FileCount;
                    faxInfo.FileNameList = tmpFaxInfo.FileNameList;
                }

                #endregion

                #endregion

                #region 수신자 정보
                List<FaxRecvInfo> alRecvList = new List<FaxRecvInfo>();
                alRecvList = appBiz.GetRecvList(faxUserList);
                #endregion

                //전송하기
                Zeospace.DeskPlus.Component.Linked.WebFax.SendFax biz = new Component.Linked.WebFax.SendFax();
                response = biz.Transfer(faxInfo, alRecvList);
            }

            return response;
        }

        /// <summary>
        /// 수정
        /// </summary>
        private void Mod()
        {

        }

        /// <summary>
        /// 삭제
        /// </summary>
        private void Del()
        {

        }
        

        /// <summary>
        /// SwfUploadGUID
        /// </summary>
        public string SwfUploadGUID
        {
            get { return GetViewStateString("SwfUploadGUID", ""); }
            set { ViewState["SwfUploadGUID"] = value; }
        }


        #endregion

        #region Event Handler (이벤트 핸들러)


        protected void lbtnRegister_Click(object sender, EventArgs e)
        {
            LoadParam();

            //string args = eventArgs;

            bool response = Add();

            if (response)
            {
                if (!Page.ClientScript.IsClientScriptBlockRegistered("SendFaxCloseOK"))
                {
                    string strScript = @"<script type=""text/javascript"">alert('{0}');self.close();" + "<" + "/script>";

                    switch (Gubun)
                    {
                        case "approval":
                            strScript = String.Format(strScript, GetResource("FaxCompletSendFaxDoc"));
                            break;
                        case "approvalCir":
                            strScript = String.Format(strScript, GetResource("FaxCompletSendFaxDocCir"));
                            break;
                    }

                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "SendFaxCloseOK", strScript);
                }
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

        #region Property (프로퍼티)

        #endregion
    }
}