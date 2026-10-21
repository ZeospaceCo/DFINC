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
using Zeospace.DeskPlus.Component.Linked.WebFax.Entity;

namespace Zeospace.DeskPlus.Common.Web.WebFax
{
    public partial class Register : Base.BasePageCommon
    {
        #region 변수설정
        private string FaxFileData = String.Empty;
        private string eventArgs = String.Empty;
        #endregion

        #region Page Process (페이지 처리)
        #endregion

        #region Method (함수)

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Common.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Common.Web.WebFax.Register).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                SwfUploadGUID = System.Guid.NewGuid().ToString();
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

            bool isAdmin = CurrentUserInfo.IsInRole("webfax");//webfax admin
            cbtnAdmin.Visible = (isAdmin) ? true : false;
        }


        /// <summary>
        /// Loads the param.
        /// </summary>
        private void LoadParam()
        {
            
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
                cbtnSwfFileAdd.Text = resMng.GetString("FaxAddFile");
                cbtnAdmin.Text = resMng.GetString("FaxAdmin");
            }
        }

        /// <summary>
        /// 추가
        /// </summary>
        private bool Add()
        {
            bool response = false;

            #region LDCC 가 지정한 회사번호 4자리
            string clinetNo = "";
            DataView dv = AppSettingHelper.GetSysCode(CurrentUserInfo.CompanyID, "FaxClientNo");
            for (int i = 0; i < dv.Count; i++)
            {
                if (dv[i]["CodeID"].ToString() == "ClientNo")
                {
                    clinetNo = this.GetLangName(
                            dv[i]["NameBase"].ToString(),
                            dv[i]["NameKo"].ToString(),
                            dv[i]["NameEn"].ToString(), dv[i]["NameJa"].ToString(),
                            dv[i]["NameFr"].ToString(), dv[i]["NameZhCn"].ToString(),
                            dv[i]["NameZhTw"].ToString(), PreLang);


                }

                if (dv[i]["CodeID"].ToString() == "FaxFileDirectory") FaxFileData = dv[i]["NameBase"].ToString() + "SendData/";

            }

            if (clinetNo.Length == 0) return false;
            #endregion

            //comName + "^" + userName + "^" + faxNum
            string faxUserList = hdnFaxUser.Text.Trim().TrimEnd('$');
            faxUserList = Server.UrlDecode(faxUserList);

            if (faxUserList.Length > 0)
            {
                #region 송신자 정보

                string comName = "";
                Zeospace.DeskPlus.Common.DAL.Company comDal = new DAL.Company();
                Zeospace.DeskPlus.Common.BLL.WebFax faxBiz = new BLL.WebFax();

                var comList = comDal.GetCompany(true);
                foreach (var info in comList)
                {
                    if (info.CompanyID == CurrentUserInfo.CompanyID)
                    {
                        comName = this.GetLangName(
                            info.NameBase, info.NameKo, info.NameEn, info.NameJa, info.NameFr, info.NameZhcn, info.NameZhtw, PreLang);
                    }
                }                
                
                FaxInfo faxInfo = new FaxInfo();
                faxInfo.CompanyID = CurrentUserInfo.CompanyID;
                faxInfo.ClientNo = clinetNo; //필수
                faxInfo.Dept = comName + ":" + CurrentUserInfo.DeptName;//회사명:지회명
                faxInfo.UserName = CurrentUserInfo.Name;
                faxInfo.Email = CurrentUserInfo.MailID + "@" + CurrentUserInfo.MailDomain;
                faxInfo.UserCode = CurrentUserInfo.LogInID; //필수                
                faxInfo.Key = System.Guid.NewGuid().ToString().Replace("-", "");//GUID(필수)
                faxInfo.Subject = cbtnSubject.Text.Trim();

                #region 첨부파일
                string filePath = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
                filePath = filePath + "{0}/Users/{1}/Spool/{2}/";
                string spoolPath = string.Format(filePath, CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, SwfUploadGUID);
                ArrayList fileArr = faxBiz.CopyFaxFile(spoolPath, FaxFileData);

                faxInfo.FileCount = fileArr.Count;
                List<string> fileNameList = new List<string>();
                for (int i = 0; i < fileArr.Count; i++)
                {
                    fileNameList.Add(((UploadFileInfo)fileArr[i]).fileGUID);
                }

                faxInfo.FileNameList = fileNameList;
                #endregion

                #endregion

                #region 수신자 정보
                List<FaxRecvInfo> alRecvList = new List<FaxRecvInfo>();
                alRecvList = faxBiz.GetRecvList(faxUserList);
                #endregion

                //전송하기
                Zeospace.DeskPlus.Component.Linked.WebFax.SendFax biz = new Component.Linked.WebFax.SendFax();
                response= biz.Transfer(faxInfo, alRecvList);

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
                Dictionary<string, string> paraDic = new Dictionary<string, string>();
                paraDic.Add("alertType", AlertMsgEnum.Alert_Register.ToString());
                paraDic.Add("alertMsg", GetResource("GoList"));
                paraDic.Add("goURL", "/DeskplusEIP/Common/WebFax/Register.aspx");

                string encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));
                Utility.Redirect(encParams);
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