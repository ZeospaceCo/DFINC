using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Data;
using System.Text;

namespace Zeospace.DeskPlus.Approval.Web.Preference.Flow
{
    public partial class ModSequence : Base.BasePageApproval
    {
        #region Declaration (컨트롤 선언)
        #endregion

        #region 변수설정
        public string FolderList = "";
        public int FavSeqID = 0;
        public string Section = "";
        public string ZAStr = "";
        #endregion

        #region Page Process (페이지 처리)
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Approval.Web.Resource.GlobalString.string";   //리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.Preference.Flow.ModSequence).Assembly;	//어셈블리 선언
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
            FavSeqID = GetDecryptParam("favSeqID", "0").ToNumber<int>();
            Section = GetDecryptParam("section", "");
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

        private string GetTagInfo(string _approvalTag)
        {
            string rtnval = "";
            switch (_approvalTag.ToUpper())
            {
                case "ZD":
                    rtnval = GetResource("SeqTitZD");
                    break;
                case "ZH":
                    rtnval = GetResource("SeqTitZH");
                    break;
                case "ZA":
                    rtnval = GetResource("SeqTitZA");
                    break;
                case "ZP":
                    rtnval = GetResource("SeqTitZP");
                    break;
                case "ZQ":
                    rtnval = GetResource("SeqTitZQ");
                    break;
                case "ZC":
                    rtnval = GetResource("SeqTitZC");
                    break;
                default:
                    rtnval = GetResource("SeqTitZD");
                    break;
            }
            return rtnval;
        }

        /// <summary>
        /// 데이터가져오기
        /// </summary>
        private void LoadData()
        {
            StringBuilder sb = new StringBuilder();

            DAL.FavSeqLine fsDal = new DAL.FavSeqLine();
            DataTableReader dtr = fsDal.GetFavSeqLineList(CurrentUserInfo.CompanyID, FavSeqID, Section);
            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    int i = 1;
                    while (dtr.Read())
                    {
                        sb.Append("<li id='");
                        sb.Append(dtr["FavSeqLineID"].ToString());
                        sb.Append("' class='");
                        sb.Append(dtr["ApprovalTag"].ToString());
                        sb.Append("'>");
                        sb.Append(i.ToString());
                        sb.Append(". [");
                        sb.Append(GetTagInfo(dtr["ApprovalTag"].ToString()));
                        sb.Append("] ");
                        sb.Append(this.GetLangName(
                                    dtr["DeptBase"].ToString().ToNull(), dtr["DeptKo"].ToString().ToNull(),
                                    dtr["DeptEn"].ToString().ToNull(), dtr["DeptJa"].ToString().ToNull(),
                                    dtr["DeptFr"].ToString().ToNull(), dtr["DeptZhcn"].ToString().ToNull(),
                                    dtr["DeptZhtw"].ToString().ToNull(), PreLang).ToReverseXSS());
                        sb.Append(" ");
                        sb.Append(this.GetLangName(
                                    dtr["TitleBase"].ToString().ToNull(), dtr["TitleKo"].ToString().ToNull(),
                                    dtr["TitleEn"].ToString().ToNull(), dtr["TitleJa"].ToString().ToNull(),
                                    dtr["TitleFr"].ToString().ToNull(), dtr["TitleZhcn"].ToString().ToNull(),
                                    dtr["TitleZhtw"].ToString().ToNull(), PreLang).ToReverseXSS());
                        sb.Append("/");
                        sb.Append(this.GetLangName(
                                    dtr["DutyBase"].ToString().ToNull(), dtr["DutyKo"].ToString().ToNull(),
                                    dtr["DutyEn"].ToString().ToNull(), dtr["DutyJa"].ToString().ToNull(),
                                    dtr["DutyFr"].ToString().ToNull(), dtr["DutyZhcn"].ToString().ToNull(),
                                    dtr["DutyZhtw"].ToString().ToNull(), PreLang).ToReverseXSS());
                        sb.Append(" ");
                        sb.Append(this.GetLangName(
                                    dtr["NameBase"].ToString().ToNull(), dtr["NameKo"].ToString().ToNull(),
                                    dtr["NameEn"].ToString().ToNull(), dtr["NameJa"].ToString().ToNull(),
                                    dtr["NameFr"].ToString().ToNull(), dtr["NameZhcn"].ToString().ToNull(),
                                    dtr["NameZhtw"].ToString().ToNull(), PreLang).ToReverseXSS());
                        sb.Append("</li>");
                        i++;

                        if (dtr["ApprovalTag"].ToString().ToUpper().Equals("ZA"))
                        {
                            ZAStr += "ZA,";
                        }
                    }
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