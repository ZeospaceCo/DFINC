using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Helper = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Auth;
using System.Data;
using System.Collections.Specialized;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
namespace Zeospace.DeskPlus.Coop.Web.Content.Total
{
    public partial class BaseView : Base.BasePageCoop
    {
        #region 변수설정
        /// <summary>
        /// 1:대기;2:접수;3:처리중;4:완료;5:반려
        /// </summary>
        public string Status = string.Empty;
        #endregion

        #region Page Process (페이지 처리)

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Coop.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Coop.Web.Content.Total.BaseView).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                LoadParam();
                SetLanguage();
                LoadData();
            }
        }

        #endregion

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        private void LoadData()
        {
            //자료 뿌려주기
            LoadDataContent();
        }
        #endregion

        #region (함수)

        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                cbtnPrint.Text = resMng.GetString("Print");
                cbtnList.Text = resMng.GetString("Close");
            }
        }

        private void LoadParam()
        {
            ListID = GetDecryptParam("listID", "0").ToNumber<int>();
        }

        /// <summary>
        /// 메서드명 :
        /// 작성목적 : 템플릿 적용
        /// 작성자   : 심재운
        /// </summary>
        /// <returns></returns>
        private string GetTemplate(int _itemSubID)
        {
            DAL.Template bll = new DAL.Template();
            var alList = bll.GetTemplateLists(CurrentUserInfo.CompanyID, _itemSubID);

            foreach (var be in alList) return be.Contents;

            string templateHeader = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.GetEditorHtmlHeader(PreLang, "");
            string templateFooter = "</body></html>";
            return templateHeader + templateFooter;
        }

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 자료 가져오기
        /// 작성자     : 심재운
        /// 최초작성일 : 2008-03-27
        /// </summary>
        private void LoadDataContent()
        {
            DAL.List dalList = new DAL.List();

            //1. 내용 가져오기
            var dtrList = dalList.GetList(CurrentUserInfo.CompanyID, ListID.ToString());

            if (dtrList != null)
            {
                if (dtrList.HasRows)
                {
                    while (dtrList.Read())
                    {
                        ltlItem.Text = this.GetLangName(
                                    dtrList["ItemNameBase"].ToString(),
                                    dtrList["ItemNameKo"].ToString(),
                                    dtrList["ItemNameEn"].ToString(),
                                    dtrList["ItemNameJa"].ToString(),
                                    dtrList["ItemNameFr"].ToString(),
                                    dtrList["ItemNameZhcn"].ToString(),
                                    dtrList["ItemNameZhtw"].ToString(), PreLang).ToReverseXSS();

                        ltlItemSub.Text = this.GetLangName(
                                    dtrList["ItemSubNameBase"].ToString(),
                                    dtrList["ItemSubNameKo"].ToString(),
                                    dtrList["ItemSubNameEn"].ToString(),
                                    dtrList["ItemSubNameJa"].ToString(),
                                    dtrList["ItemSubNameFr"].ToString(),
                                    dtrList["ItemSubNameZhcn"].ToString(),
                                    dtrList["ItemSubNameZhtw"].ToString(), PreLang).ToReverseXSS();

                        ltlRequestDate.Text = DateHelper.GetCultureDateChar8(dtrList["RequestDate"].ToString(), PreLangReal);
                        ltlCompleteRequestDate.Text = DateHelper.GetCultureDateChar8(dtrList["CompleteRequestDate"].ToString(), PreLangReal);
                        ltlIsClose.Text = (dtrList["IsClose"].ToString() == "2") ? GetResource("Private") : GetResource("Public");

                        //1:대기;2:접수;3:처리중;4:완료;5:반려
                        Status = dtrList["StatusLast"].ToString();

                        ltlStartDate.Text = DateHelper.GetCultureDateChar8(dtrList["StartDate"].ToString(), PreLangReal);
                        ltlEndDate.Text = DateHelper.GetCultureDateChar8(dtrList["EndDate"].ToString(), PreLangReal);

                        //난이도(상:1;중:2;하:3)
                        ltlDifficulty.Text = GetDifficulty(dtrList["Difficultys"].ToString());
                        //상태
                        ltlStatus.Text = GetStatus(dtrList["StatusLast"].ToString());                        
                    }
                }
            }

            LoadDataGroup();

            DAL.ListTO dalListTo = new DAL.ListTO();
            var listTo = dalListTo.GetListTo(CurrentUserInfo.CompanyID, ListID);
            string toList = "";
            foreach (var listInfo in listTo)
            {
                if (listInfo != null)
                {
                    toList += string.Format(@"<a onclick=""AccountInfo('{0}');return false;"" style='cursor:pointer'>{1}</a>, ", listInfo.UserID, this.GetLangName(
                                listInfo.NameBase,
                                listInfo.NameKo, listInfo.NameEn, listInfo.NameJa,
                                listInfo.NameFr, listInfo.NameZhcn, listInfo.NameZhtw, PreLang).ToReverseXSS());

                }

                ltlReceive.Text = toList.Trim().TrimEnd(',');
            }

            DAL.ListCC dalListCC = new DAL.ListCC();
            var listCC = dalListCC.GetListCC(CurrentUserInfo.CompanyID, ListID);
            string ccList = "";
            foreach (var listInfo in listCC)
            {
                if (listInfo != null)
                {
                    ccList += string.Format(@"<a onclick=""AccountInfo('{0}');return false;"" style='cursor:pointer'>{1}</a>, ", listInfo.UserID, this.GetLangName(
                                listInfo.NameBase,
                                listInfo.NameKo, listInfo.NameEn, listInfo.NameJa,
                                listInfo.NameFr, listInfo.NameZhcn, listInfo.NameZhtw, PreLang).ToReverseXSS());

                }

                this.ltlReference.Text = ccList.Trim().TrimEnd(',');
            }
        }

        /// <summary>
        /// 그룹별 내용 출력하기
        /// </summary>
        private void LoadDataGroup()
        {
            DAL.List dal = new DAL.List();

            var dtrList = dal.GetListByGroup(CurrentUserInfo.CompanyID, ListID);
            if (dtrList != null)
            {
                if (dtrList.HasRows)
                {
                    while (dtrList.Read())
                    {

                        #region 이름 다국어 설정
                        string name = this.GetLangName(
                                dtrList["NameBase"].ToString().ToNull(), dtrList["NameKo"].ToString().ToNull(),
                                dtrList["NameEn"].ToString().ToNull(), dtrList["NameJa"].ToString().ToNull(),
                                dtrList["NameFr"].ToString().ToNull(), dtrList["NameZhcn"].ToString().ToNull(),
                                dtrList["NameZhtw"].ToString().ToNull(), PreLang);
                        #endregion

                        pnContent.Controls.Add(new LiteralControl(
                                CreateHtmlGroup(dtrList["CompanyID"].ToString().ToNumber<int>(),
                                    dtrList["ListID"].ToString().ToNumber<int>(),
                                    dtrList["GroupID"].ToString().ToNumber<int>(),
                                    dtrList["ListOrder"].ToString().ToNumber<int>(),
                                    dtrList["Title"].ToString(), dtrList["Contents"].ToString(), dtrList["MobileContent"].ToString(),
                                    dtrList["WriteUserID"].ToString().ToNumber<int>(),
                                    name,
                                    GetStatus(dtrList["Status"].ToString().ToNull()),
                                    dtrList["StatusLast"].ToString().ToNull(),
                                    Convert.ToDateTime(dtrList["WriteDate"].ToString()))
                            ));
                    }
                }
            }


        }

        /// <summary>
        /// 그룹별 내용 출력하기 - html
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_listID">The _list ID.</param>
        /// <param name="_groupID">The _group ID.</param>
        /// <param name="_listOrder">The _list order.</param>
        /// <param name="_title">The _title.</param>
        /// <param name="_contents">The _contents.</param>
        /// <param name="_writeUserID">The _write user ID.</param>
        /// <param name="_name">The _name.</param>
        /// <param name="_status">The _status.</param>
        /// <param name="_writeDate">The _write date.</param>
        /// <returns></returns>
        private string CreateHtmlGroup(int _companyID, int _listID, int _groupID, int _listOrder, string _title,
            string _contents, string _mobileContent, int _writeUserID, string _name, string _status, string _statusLast, DateTime _writeDate)
        {

            string tmpName = @"<a onclick=""AccountInfo('{0}');return false;"" style='cursor:pointer'>{1}</a>";
            _name = string.Format(tmpName, _writeUserID, _name);

            string headerHtml = "";
            string bodyHtml = "";
            string attchHtml = "";
            string baseDocHtml = "";

            string headerTitle = GetResource("Text");
            if (_listID != _groupID) headerTitle = GetResource("ReplyTitle");

            headerHtml = @"                
        <div class='layout_subbox'><h2>" + headerTitle + @"</h2><ul class='btn_right'>
        {0}
        </ul></div>
        <div class='layout_subbox'>
            <table cellspacing='0' class='table_typeB'>
                <colgroup>
                    <col width='80px' />
                    <col />
                    <col width='80px' />
                    <col />
                    <col width='80px' />
                    <col />
                </colgroup>
                <tbody>
                    <tr>
                        <th>" + GetResource("Title") + @"</th>
                        <td colspan='5'>{1}</td>
                    </tr>
                    <tr>
                        <th>" + GetResource("WriteName") + @"</th>
                        <td>{2}</td>
                        <th>" + GetResource("State") + @"</th>
                        <td>{3}</td>
                        <th>" + GetResource("DateOfWrite") + @"</th>
                        <td>{4}</td>
                    </tr>
                </tbody>
            </table>
        </div>";

            string btnHtml = "";

            //_statusLast 은 1:대기;2:접수;3:처리중;4:완료;5:반려
            if (CurrentUserInfo.UserID == _writeUserID && _listOrder > 0 && (_statusLast == "1" || _statusLast == "2" || _statusLast == "3"))
            {
                btnHtml = @"<li class='btn_blue'><a id='cbtnMod_{0}' onclick=""View.Mod('{1}','{2}')""><span>" + GetResource("ReplyMod") + @"</span></a></li>";
                btnHtml += @"<li class='btn_blue'><a id='cbtnDel_{0}' onclick=""View.Del('{1}','{2}')""><span>" + GetResource("ReplyDel") + @"</span></a></li>";
                btnHtml = string.Format(btnHtml, _listID.ToString(), _listID.ToString(), _groupID.ToString());
            }

            headerHtml = string.Format(headerHtml, btnHtml, _title, _name, _status, _writeDate.ToShortDateString() + " " + _writeDate.ToShortTimeString());

            bodyHtml = @"
        <div class='content_box'  style='overflow:auto;'> 
        <table cellspacing='0' style='min-height:50px;width:100%;'> 
            <tr> 
                <td valign='top'>{0}</td> 
            </tr> 
        </table> 
        </div>        ";

            bodyHtml = string.Format(bodyHtml, _contents);

            attchHtml = @"
            <div class='layout_subbox'><h2>" + GetResource("AttachName") + @"</h2><ul class='btn_right'></ul></div>                
            <table class='table_typeA'>
            <caption></caption>
            <colgroup>
                <col />
                <col width='100px' />
                <col width='100px' />
                <col width='60px' />
            </colgroup>
            <thead>
                <th>" + GetResource("FileName") + @"</th>
                <th>" + GetResource("FileType") + @"</th>
                <th>" + GetResource("FileLen") + @"</th>
                <th>" + GetResource("FileDownload") + @"</th>
            </thead>
            <tbody>";


            //첨부파일
            DAL.Attach dal = new DAL.Attach();
            List<BE.AttachInfo> attachInfo = dal.GetAttachFileLists(_companyID, _listID);

            if (attachInfo.Count == 0)
            {
                attchHtml = "";
            }
            else
            {
                foreach (BE.AttachInfo _attachInfo in attachInfo)
                {
                    _attachInfo.FileGUID = (_attachInfo.FileGUID.ToNull().Length > 0) ? _attachInfo.FileGUID.ToNull() : _attachInfo.FileName;

                    Dictionary<string, string> paraDic = new Dictionary<string, string>();
                    paraDic.Add("filePath", _attachInfo.FilePath + _attachInfo.FileGUID);
                    paraDic.Add("fileName", _attachInfo.FileName);
                    _attachInfo.FilePath = GetEncryptParam(Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.GetParameterDic(paraDic));

                    _attachInfo.FileType = _attachInfo.FileType.Replace(".", "");
                    _attachInfo.FileLength = _attachInfo.FileLength;
                    _attachInfo.FileLengthKB = String.Format("{0:n1}", Convert.ToDecimal(_attachInfo.FileLength) / 1024) + " KB";

                    string temphtml = @"                
                <tr>
                    <td><a href='#' onclick=""javascript:View.FileDownload('{0}','{1}','{2}')"">{1}</a></td>
                    <td>{3}</td>
                    <td>{4}</td>
                    <td class='center'><a href='#' onclick=""javascript:View.FileDownload('{0}','{1}','{2}')"" style='display:inline-block;'>
                        <span class='btn_file_dw' title='DownLoad'></span></a></td>
                </tr>";

                    attchHtml += string.Format(temphtml, _attachInfo.FilePath, _attachInfo.FileName, _attachInfo.FileGUID, _attachInfo.FileType, _attachInfo.FileLengthKB);
                }

                attchHtml += @"</tbody></table><div class='table_typeAbottom_left'><p></p></div>";
            }


            //근거문서
            baseDocHtml = @"                           
            <div class='layout_subbox'><h2>" + GetResource("BaseDoc") + @"</h2><ul class='btn_right'></ul></div>                
            <div class='table_typeAtop_left'><p></p></div>

            <table class='table_typeA'>
            <caption>" + GetResource("BaseDoc") + @"</caption>
            <colgroup>
                <col />
                <col width='100px' /> 
            </colgroup>
            <thead>
                <tr>
                    <th>" + GetResource("Title") + @"</th>
                    <th class='center'>" + GetResource("DocView") + @"</th>
                </tr>
            </thead>
            <tbody>";

            DAL.APPBaseDoc appDal = new DAL.APPBaseDoc();
            var dtr = appDal.GetAppBaseDoc(_companyID, _listID.ToString());

            BE.APPBaseDocInfo Info = null;

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        Info = new BE.APPBaseDocInfo();

                        Info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                        Info.ListID = dtr["ListID"].ToString().ToNumber<int>();
                        Info.DocumentID = dtr["DocumentID"].ToString().ToNumber<int>();

                        Info.ItemName = this.GetLangName(
                                dtr["ItemNameBase"].ToString().ToNull(), dtr["ItemNameKo"].ToString().ToNull(),
                                dtr["ItemNameEn"].ToString().ToNull(), dtr["ItemNameJa"].ToString().ToNull(),
                                dtr["ItemNameFr"].ToString().ToNull(), dtr["ItemNameZhcn"].ToString().ToNull(),
                                dtr["ItemNameZhtw"].ToString().ToNull(), PreLang);

                        Info.ItemSubName = this.GetLangName(
                                dtr["ItemSubNameBase"].ToString().ToNull(), dtr["ItemSubNameKo"].ToString().ToNull(),
                                dtr["ItemSubNameEn"].ToString().ToNull(), dtr["ItemSubNameJa"].ToString().ToNull(),
                                dtr["ItemSubNameFr"].ToString().ToNull(), dtr["ItemSubNameZhcn"].ToString().ToNull(),
                                dtr["ItemSubNameZhtw"].ToString().ToNull(), PreLang);

                        Info.RequestDate = DateHelper.GetCultureDateChar8(dtr["RequestDate"].ToString(), PreLangReal);
                        Info.StartDate = DateHelper.GetCultureDateChar8(dtr["StartDate"].ToString(), PreLangReal);
                        Info.EndDate = DateHelper.GetCultureDateChar8(dtr["EndDate"].ToString(), PreLangReal);
                        Info.Title = dtr["Title"].ToString();

                        Info.NameBase = this.GetLangName(dtr["NameBase"].ToString(), dtr["NameKo"].ToString(), dtr["NameEn"].ToString(), dtr["NameJa"].ToString(),
                                            dtr["NameFr"].ToString(), dtr["NameZhCn"].ToString(), dtr["NameZhTw"].ToString(), PreLang).ToReverseXSS();

                        // 근거문서 문서제목
                        Info.Name = dtr["Name"].ToString().ToNull();

                        string temphtml = @"<tr>
                        <td>{0}</td>
                        <td class='center'><a onclick=""Common.View('{1}')"">&nbsp;&nbsp;<img class='BtnViews' alt=''></a></td>
                    </tr>";

                        baseDocHtml += string.Format(temphtml, Info.Name, Info.DocumentID);
                    }
                }
            }

            if (Info == null)
            {
                baseDocHtml = "";
            }
            else
            {
                baseDocHtml += @"</tbody></table>";
            }

            return headerHtml + attchHtml + baseDocHtml + bodyHtml + "<div class='dotLine'></div>";
        }

        #endregion

        #region (프로퍼티)

        /// <summary>
        /// ActiveX 유무
        /// </summary>
        public string NonActiveX
        {
            get { return GetViewStateString("NonActiveX", ""); }
            set { ViewState["NonActiveX"] = value; }
        }

        #endregion

        #region (이벤트핸들러)
        #endregion
    }
}