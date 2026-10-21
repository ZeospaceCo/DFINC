using System;
using System.Collections.Generic;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.Address.Web.Outlook
{
    public partial class InOutStep2 : Base.BasePageAddress
    {
        #region 변수설정

        public string CntAll = "0";
        public string CntExcept = "0";
        public string CntOverlap = "0";

        #endregion 변수설정

        #region Page Process (페이지 처리)

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보

            this.ResourceName = "Zeospace.DeskPlus.Address.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Address.Web.Outlook.InOutStep2).Assembly;	//어셈블리 선언

            #endregion 다국어 리소스 로드/Title 정보

            if (!IsPostBack)
            {
                SetLanguage();
                LoadData();
            }
        }

        #endregion Page Process (페이지 처리)

        #region (함수)

        /// <summary>
        /// 다국어 설정
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                cbtnAdd.Text = resMng.GetString("Confirm");         // "확인";
            }
        }

        private void LoadData()
        {
            DAL.CSV dal = new DAL.CSV();
            DataTableReader dtr = dal.GetCSVCount(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID);

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    int i = 1;
                    while (dtr.Read())
                    {
                        if (i == 1) CntAll = dtr["CNT"].ToString();
                        if (i == 2) CntExcept = dtr["CNT"].ToString();
                        if (i == 3) CntOverlap = dtr["CNT"].ToString();
                        i += 1;
                    }
                }
            }
        }

        #endregion (함수)

        #region (이벤트핸들러)

        /// <summary>
        /// Handles the Click event of the lbtnRegister control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void lbtnRegister_Click(object sender, EventArgs e)
        {
            try
            {
                string csvIDs = tbxCSVID.Text.Trim();
                if (csvIDs.Length > 0)
                {
                    #region 글자수가 길어서, bundle 숫자만큼 잘라서 저장하자. shimpark

                    int indexStart = 0; //분리 시작위치
                    int indexEnd = 0; //분리 끝 위치
                    int bundle = 100; //100 개 단위로 잘라서 쓰자
                    char splitChr = ','; //글자 마지막 자를 특수문자
                    string result = ""; //자른 문자열 값
                    int totLen = csvIDs.Length; //총 원본 글자글 총수
                    int forCnt = totLen / bundle; //몫

                    DAL.CSV dal = new DAL.CSV();

                    for (int i = 0; i < forCnt + 1; i++)
                    {
                        if (indexStart + bundle > totLen) indexEnd = totLen - indexStart;
                        else indexEnd = csvIDs.Substring(indexStart, bundle).LastIndexOf(splitChr);

                        result = csvIDs.Substring(indexStart, indexEnd);
                        indexStart += indexEnd + 1;

                        dal.AddReceived(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, result);
                    }

                    #endregion 글자수가 길어서, bundle 숫자만큼 잘라서 저장하자. shimpark

                    Dictionary<string, string> paraDic = new Dictionary<string, string>();
                    paraDic.Add("alertType", AlertMsgEnum.Alert_Register.ToString());
                    paraDic.Add("alertMsg", GetResource("GoPageStep3"));
                    paraDic.Add("goURL", "/DeskplusEIP/Address/Outlook/InOutStep3.aspx");

                    string encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));
                    Utility.Redirect(encParams);
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("lbtnRegister_Click", ex);
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "fail", GetResource("ProcessingError"), "");
            }
        }

        #endregion (이벤트핸들러)
    }
}