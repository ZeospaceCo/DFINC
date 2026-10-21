using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Web.UI;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using System.Text;
using System.Text.RegularExpressions;

namespace Zeospace.DeskPlus.Task.Web.Common
{
    public partial class ViewHistory : Base.BasePageTask
    {
        #region 변수설정
        public string Gubun = "";
        #endregion 변수설정

        #region Page Process (페이지 처리)

        protected void Page_Load(object sender, System.EventArgs e)
        {
            #region 다국어 리소스 로드

            ResourceName = "Zeospace.DeskPlus.Task.Web.Resource.GlobalString.string";			//리소스 명 선언
            AssemblyName = typeof(Zeospace.DeskPlus.Task.Web.Common.ViewHistory).Assembly;	    //어셈블리 선언
            GetResourceManager();

            #endregion 다국어 리소스 로드

            if (!IsPostBack)
            {
                SetLanguage();
                LoadData();
            }
        }

        #endregion Page Process (페이지 처리)

        #region Data Load (컨트롤에 데이터를 바인딩 하는 메소드)

        private void LoadData()
        {
            LoadParam();

            //데이터 가져오기
            LoadDataContent();          
        }

        #endregion Data Load (컨트롤에 데이터를 바인딩 하는 메소드)

        #region Method (함수)

        /// <summary>
        /// 다국어 설정
        /// </summary>
        private void SetLanguage()
        {
            if (!this.resMng.Equals(null))
            {
                CbtnClose.Text = resMng.GetString("Close");       //"닫기";
            }
        }

        /// <summary>
        /// 파라미터 정의
        /// </summary>
        private void LoadParam()
        {
            Gubun = GetDecryptParam("gubun", "TO");
        }

        /// <summary>
        /// 그룹별 내용 출력하기
        /// </summary>
        private void LoadDataContent()
        {
            string replyHtml = "";

            if (Gubun == "TO")
            {
                DAL.HistoryTO dal = new DAL.HistoryTO();
                var dtr = dal.GetHistoryList(CurrentUserInfo.CompanyID, GroupID);                

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            // 글쓴이
                            string tDeptName = this.GetLangName(dtr["DeptBase"].ToString(), dtr["DeptKo"].ToString(), dtr["DeptEn"].ToString(),
                               dtr["DeptJa"].ToString(), dtr["DeptFr"].ToString(), dtr["DeptZhcn"].ToString(),
                               dtr["DeptZhtw"].ToString(), PreLang).ToReverseXSS();

                            string tTitle = this.GetLangName(dtr["TitleBase"].ToString(), dtr["TitleKo"].ToString(), dtr["TitleEn"].ToString(),
                                     dtr["TitleJa"].ToString(), dtr["TitleFr"].ToString(), dtr["TitleZhcn"].ToString(),
                                     dtr["TitleZhtw"].ToString(), PreLang).ToReverseXSS();

                            string tName = this.GetLangName(dtr["NameBase"].ToString(), dtr["NameKo"].ToString(), dtr["NameEn"].ToString(),
                                       dtr["NameJa"].ToString(), dtr["NameFr"].ToString(), dtr["NameZhcn"].ToString(),
                                       dtr["NameZhtw"].ToString(), PreLang).ToReverseXSS();

                            string name = tDeptName + " " + tTitle + " " + tName;

                            replyHtml += CreateHtmlGroup(dtr["Contents"].ToString(), name, Convert.ToDateTime(dtr["WriteDate"].ToString()), dtr["IsType"].ToString());

                        }
                    }
                }
            }
            else if (Gubun == "CC")
            {
                DAL.HistoryCC dal = new DAL.HistoryCC();
                var dtr = dal.GetHistoryList(CurrentUserInfo.CompanyID, GroupID);

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            // 글쓴이
                            string tDeptName = this.GetLangName(dtr["DeptBase"].ToString(), dtr["DeptKo"].ToString(), dtr["DeptEn"].ToString(),
                               dtr["DeptJa"].ToString(), dtr["DeptFr"].ToString(), dtr["DeptZhcn"].ToString(),
                               dtr["DeptZhtw"].ToString(), PreLang).ToReverseXSS();

                            string tTitle = this.GetLangName(dtr["TitleBase"].ToString(), dtr["TitleKo"].ToString(), dtr["TitleEn"].ToString(),
                                     dtr["TitleJa"].ToString(), dtr["TitleFr"].ToString(), dtr["TitleZhcn"].ToString(),
                                     dtr["TitleZhtw"].ToString(), PreLang).ToReverseXSS();

                            string tName = this.GetLangName(dtr["NameBase"].ToString(), dtr["NameKo"].ToString(), dtr["NameEn"].ToString(),
                                       dtr["NameJa"].ToString(), dtr["NameFr"].ToString(), dtr["NameZhcn"].ToString(),
                                       dtr["NameZhtw"].ToString(), PreLang).ToReverseXSS();

                            string name = tDeptName + " " + tTitle + " " + tName;

                            replyHtml += CreateHtmlGroup(dtr["Contents"].ToString(), name, Convert.ToDateTime(dtr["WriteDate"].ToString()), dtr["IsType"].ToString());

                        }
                    }
                }
            }
            else if (Gubun == "DATE")
            {
                DAL.HistoryOption dal = new DAL.HistoryOption();
                var dtr = dal.GetHistoryList(CurrentUserInfo.CompanyID, GroupID);

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            // 글쓴이
                            string tDeptName = this.GetLangName(dtr["DeptBase"].ToString(), dtr["DeptKo"].ToString(), dtr["DeptEn"].ToString(),
                               dtr["DeptJa"].ToString(), dtr["DeptFr"].ToString(), dtr["DeptZhcn"].ToString(),
                               dtr["DeptZhtw"].ToString(), PreLang).ToReverseXSS();

                            string tTitle = this.GetLangName(dtr["TitleBase"].ToString(), dtr["TitleKo"].ToString(), dtr["TitleEn"].ToString(),
                                     dtr["TitleJa"].ToString(), dtr["TitleFr"].ToString(), dtr["TitleZhcn"].ToString(),
                                     dtr["TitleZhtw"].ToString(), PreLang).ToReverseXSS();

                            string tName = this.GetLangName(dtr["NameBase"].ToString(), dtr["NameKo"].ToString(), dtr["NameEn"].ToString(),
                                       dtr["NameJa"].ToString(), dtr["NameFr"].ToString(), dtr["NameZhcn"].ToString(),
                                       dtr["NameZhtw"].ToString(), PreLang).ToReverseXSS();

                            string name = tDeptName + " " + tTitle + " " + tName;

                            replyHtml += CreateHtmlGroup(dtr["Contents"].ToString(), name, Convert.ToDateTime(dtr["WriteDate"].ToString()), dtr["IsType"].ToString());

                        }
                    }
                }
            }

            if (replyHtml.Length > 0) pnContent.Controls.Add(new LiteralControl(replyHtml));
        }

        /// <summary>
        /// 내용 그려주기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_contents"></param>
        /// <param name="_name"></param>
        /// <param name="_writeDate"></param>
        /// <param name="_isType"></param>
        /// <returns></returns>
        private string CreateHtmlGroup(string _contents, string _name, DateTime _writeDate, string _isType)
        {
            string bodyHtml = "";
            string writeGubun = "";

            #region 템플릿 양식
            bodyHtml = @"<div class='cw_Abox'>
                    <table cellspacing='0' class='tab_typeCN'>
                        <colgroup>
                            <col width='80px' /><col /><col width='80px' /><col width='140px' />
                        </colgroup>
                        <tbody>
                            <tr><th>{3}</th><td>{0}</td><th>" + GetResource("DateOfWrite") + @"</th><td>{1}</td>
                            </tr>
                            <tr>                    
                                <td colspan='4' valign='top'>{2}</td>
                            </tr>
                        </tbody>
                    </table>
                </div>";         

            #endregion 템플릿 양식          

            if (_isType == "RE")
            {
                writeGubun = GetResource("Requester");
            }
            else if (_isType == "TO")
            {
                writeGubun = GetResource("Receiver");
            }
            else if (_isType == "CC")
            {
                writeGubun = GetResource("Referencer");
            }
            else
            {
                writeGubun = GetResource("ETCUser");
            }

            bodyHtml = string.Format(
                bodyHtml,
                _name,
                _writeDate.ToShortDateString() + " " + _writeDate.ToShortTimeString(),
                _contents,
                writeGubun);

            return bodyHtml;
        }


        #endregion Method (함수)

        #region Property (프로퍼티)
        #endregion Property (프로퍼티)

        #region Event Handler (이벤트 핸들러)       
        #endregion Event Handler (이벤트 핸들러)
    }
}