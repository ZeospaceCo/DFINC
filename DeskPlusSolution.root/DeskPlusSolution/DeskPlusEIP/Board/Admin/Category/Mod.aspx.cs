using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

using Helpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Board.Web.Admin.Category
{
    public partial class Mod : Zeospace.DeskPlus.Board.Web.Base.BasePageBoardAdmin
    {
        #region Declaration (컨트롤 선언)

        private string eventArgs = String.Empty;

        #endregion Declaration (컨트롤 선언)

        #region 변수설정

        public int pCategoryID = 0;//상위부서 카테고리
        public string AuthArrayList = "";
        public string AuthTable = "";
        public string OptionUseDept = "";//1이면 부서별 게시판임
        private string MultiName = String.Empty;
        public string MaxRowMax = "0";
        public string CategoryTitle = ""; //카테고리이름

        #endregion 변수설정

        #region Page Process (페이지 처리)

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보

            ResourceName = "Zeospace.DeskPlus.Board.Web.Resource.GlobalString.string";			//리소스 명 선언
            AssemblyName = typeof(Zeospace.DeskPlus.Board.Web.Admin.Category.Mod).Assembly;	//어셈블리 선언

            #endregion 다국어 리소스 로드/Title 정보

            if (!IsPostBack)
            {
                SetLanguage();

                #region 초기화

                BindItem(CurrentUserInfo.CompanyID);
                LoadData();

                #endregion 초기화
            }

            #region 이미지 처리

            //CbtnAdd.ImagePath = BaseImgPath + "/Lang/" + this.PreLang + "/ico_folderset_add.gif";

            #endregion 이미지 처리
        }

        /// <summary>
        /// Sets the language.
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();

            if (!this.resMng.Equals(null))
            {
                if (CatID == 0)
                {
                    CategoryTitle = resMng.GetString("AddFolder");
                }
                else
                {
                    CategoryTitle = resMng.GetString("ModifyFolder");
                }

                // 일괄 권한 설정
                ddlAllAuthManage.Items.Clear();
                ddlAllAuthManage.Items.Add(new ListItem(resMng.GetString("Read"), "1"));         // 읽기
                ddlAllAuthManage.Items.Add(new ListItem(resMng.GetString("Write"), "2"));        // 쓰기
                ddlAllAuthManage.Items.Add(new ListItem(resMng.GetString("ReadReply"), "3"));    // 읽기+댓글쓰기
                ddlAllAuthManage.Items.Add(new ListItem(resMng.GetString("WriteReply"), "4"));   // 쓰기+댓글쓰기

                ddlListType.Items.Clear();
                ddlListType.Items.Add(new ListItem(resMng.GetString("ListType0"), "0"));//전체
                ddlListType.Items.Add(new ListItem(resMng.GetString("ListType1"), "1"));//리스트
                ddlListType.Items.Add(new ListItem(resMng.GetString("ListType2"), "2"));//블로그
                ddlListType.Items.Add(new ListItem(resMng.GetString("ListType3"), "3"));//갤러리

                CbtnOpenFolder.Text = resMng.GetString("ChangeFolder");
                CbtnMod.Text = resMng.GetString("Submit");
                CbtnAdd.Text = resMng.GetString("Add");
                CbtnMove.Text = resMng.GetString("CategoryOrder");
                CbtnHeaders.Text = resMng.GetString("Headers");
                cbVoteView.Text = resMng.GetString("ViewUserRatingList");
                CbtnCreateMenu.Text = resMng.GetString("CreateMenu");
                CbtnScreenShot.Text = resMng.GetString("ScreenShot");
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// Gets the param.
        /// </summary>
        private void GetParam()
        {
            pCategoryID = GetDecryptParam("pCatID", "0").ToNumber<int>();
            OptionUseDept = GetDecryptParam("optionUseDept", "0");
        }

        #endregion Page Process (페이지 처리)

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        /// <summary>
        /// Loads the data.
        /// </summary>
        private void LoadData()
        {
            GetParam();

            DeptUserTreeScript1.CompanyID = CurrentUserInfo.CompanyID;
            DeptUserTreeScript1.DeptID = CurrentUserInfo.DeptID;
            DeptUserTreeScript1.UseActiveDept = true;
            DeptUserTreeScript1.UseCheckBoxMember = true;
            DeptUserTreeScript1.UseIcon = true;
            DeptUserTreeScript1.UseCheckBoxGroup = true;
            DeptUserTreeScript1.UseMyRootDept = false;
            DeptUserTreeScript1.UseVirtualTree = false;
            DeptUserTreeScript1.UseRootCheckBox = false;

            LoadTemplate();

            if (CatID == 0)
            {
                CbtnScreenShot.Visible = false;
                CbtnCreateMenu.Visible = false;
                CbtnOpenFolder.Visible = false;
                CbtnHeaders.Visible = false;
                cbUseDept.Checked = (OptionUseDept == "1") ? true : false;
                cbUseDept.Enabled = (OptionUseDept == "1") ? false : true;
            }
            else
            {
                LoadCategory();
                LoadAuthority();
            }
        }

        /// <summary>
        /// 코드관리 처리
        /// </summary>
        /// <param name="_companyID"></param>
        private void BindItem(int _companyID)
        {
            // 알람-코드관리로 처리
            DataView dvBoa = AppSettingHelper.GetSysCode(_companyID, "AlarmBoard");

            ddlAlarm.Items.Clear();
            ddlAlarm.AppendDataBoundItems = true;
            string name = "", vals = "";
            foreach (DataRowView dr in dvBoa)
            {
                name = this.GetLangName(
                    dr["NameBase"].ToString(),
                    dr["NameKo"].ToString(), dr["NameEn"].ToString(), dr["NameJa"].ToString(), dr["NameFr"].ToString(),
                    dr["NameZhCn"].ToString(), dr["NameZhTw"].ToString(), PreLang);

                // 코드값이 특정 약어로 처리되는 경우, interfacecode를 이용
                vals = (dr["InterfaceCode"].ToString().ToNull().Trim().Equals("")) ? dr["CodeID"].ToString() : dr["InterfaceCode"].ToString().ToNull().Trim();
                ddlAlarm.Items.Add(new ListItem(name, vals));
            }
        }

        #endregion DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        #region (함수)

        /// <summary>
        /// 작성자 : 심재운
        /// 템플릿 정보 가져오기
        /// </summary>
        private void LoadTemplate()
        {
            Zeospace.DeskPlus.Board.DAL.Templates biz = new DAL.Templates();
            List<BE.TemplatesInfo> al = biz.GetTemplateLists(CurrentUserInfo.CompanyID);

            ddlTemplate.Items.Clear();
            ddlTemplate.AppendDataBoundItems = true;
            ddlTemplate.Items.Add(new ListItem("-------", "0"));
            foreach (BE.TemplatesInfo info in al)
            {
                ddlTemplate.Items.Add(new ListItem(info.Title, info.TemplateID.ToString()));
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 권한 설정하기
        /// </summary>
        private void LoadAuthority()
        {
            Zeospace.DeskPlus.Board.DAL.Authority bll = new Zeospace.DeskPlus.Board.DAL.Authority();
            DataTableReader dtr = bll.GetAuthority(CurrentUserInfo.CompanyID, CatID);
            StringBuilder sb = new StringBuilder();

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    int i = 1;
                    while (dtr.Read())
                    {
                        #region 다국어 이름 설정

                        string name = this.GetLangName(
                            dtr["NameBase"].ToString(),
                            dtr["NameKo"].ToString(),
                            dtr["NameEn"].ToString(),
                            dtr["NameJa"].ToString(),
                            dtr["NameFr"].ToString(),
                            dtr["NameZhCn"].ToString(),
                            dtr["NameZhTw"].ToString(), PreLang);

                        #endregion 다국어 이름 설정

                        //(G$1$영업부,영업부)
                        sb.Append(dtr["Type"].ToString()
                            + "$" + dtr["OwnerID"].ToString()
                            + "$" + name
                            + ";");

                        AuthTable += CreateTableAuth(i, dtr["Type"].ToString()
                            + "$" + dtr["OwnerID"].ToString(),
                            name, dtr["Authority"].ToString().ToNumber<int>());

                        i += 1;
                    }

                    MaxRowMax = i.ToString();
                }
            }

            AuthArrayList = sb.ToString().TrimEnd(';');
        }

        /// <summary>
        /// 카테고리별 권한 테이블 그리기
        /// </summary>
        /// <param name="_idx">The _idx.</param>
        /// <param name="_keyID">The _key ID.</param>
        /// <param name="_name">The _name.</param>
        /// <param name="_auth">The _auth.</param>
        /// <returns></returns>
        private string CreateTableAuth(int _idx, string _keyID, string _name, int _auth)
        {
            string selected1 = "";
            string selected2 = "";
            string selected3 = "";
            string selected4 = "";

            switch (_auth)
            {
                case 1:
                    selected1 = "selected='selected'";
                    break;

                case 2:
                    selected2 = "selected='selected'";
                    break;

                case 3:
                    selected3 = "selected='selected'";
                    break;

                case 4:
                    selected4 = "selected='selected'";
                    break;
            }

            string tempStr = "<tr id='boardAuth" + _idx + "'>"
            + "<td class='center'>"
            + _name
            + "</td>"
            + "<td class='center'>"
            + "<select name='ddlListType' id='ddlListType" + _idx + "' class='searchselect form-control'>"
            + "	<option " + selected1 + " value='1$" + _keyID + "'>" + GetResource("Read") + "</option>"         // 읽기
            + "	<option " + selected2 + " value='2$" + _keyID + "'>" + GetResource("Write") + "</option>"        // 쓰기
            + "	<option " + selected3 + " value='3$" + _keyID + "'>" + GetResource("ReadReply") + "</option>"    // 읽기+댓글쓰기
            + "	<option " + selected4 + " value='4$" + _keyID + "'>" + GetResource("WriteReply") + "</option>"   // 쓰기+댓글쓰기
            + "</select></td>"
            + @"<td class='center'><a class=btn onclick=""Mod.DelAuthRow(" + _idx + ",'" + _keyID + "','" + _name + @"')"">" + resMng.GetString("Del") + "</a></td>"            
            + "</tr>";

            return tempStr;
        }

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 카테고리 정보 가져오기
        /// 작성자     : 심재운
        /// 최초작성일 : 2011-04-27
        /// </summary>
        private void LoadCategory()
        {
            DAL.Category dal = new DAL.Category();
            DataSet ds = dal.GetCategory(CurrentUserInfo.CompanyID, CatID);

            BLL.Category biz = new BLL.Category();
            //CategoryTitle = biz.GetPathName(ds, PreLang); //sitemap 경로 반환

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                if (row["TemplateID"].ToString().ToNumber<int>() > 0)
                {
                    ddlTemplate.SelectedValue = row["TemplateID"].ToString();
                    cbTemplateID.Checked = true;
                }

                #region 전체경로

                string name = this.GetLangName(
                        row["PathNameBase"].ToString(),
                        row["PathNameKo"].ToString(),
                        row["PathNameEn"].ToString(), row["PathNameJa"].ToString(),
                        row["PathNameFr"].ToString(), row["PathNameZhCn"].ToString(),
                        row["PathNameZhTw"].ToString(), PreLang);

                switch (PreLang)
                {
                    case "ko":
                        name = (row["NameKo"].ToString() == "") ? row["PathNameBase"].ToString() : name;
                        break;

                    case "en":
                        name = (row["NameEn"].ToString() == "") ? row["PathNameBase"].ToString() : name;
                        break;

                    case "ja":
                        name = (row["NameJa"].ToString() == "") ? row["PathNameBase"].ToString() : name;
                        break;

                    case "fr":
                        name = (row["NameFr"].ToString() == "") ? row["PathNameBase"].ToString() : name;
                        break;

                    case "zhcn":
                        name = (row["NameZhcn"].ToString() == "") ? row["PathNameBase"].ToString() : name;
                        break;

                    case "zhtw":
                        name = (row["NameZhtw"].ToString() == "") ? row["PathNameBase"].ToString() : name;
                        break;
                }

                ltPathName.Text = name.Substring(1, name.Length - 1);

                #endregion 전체경로

                ctbTitle.Text = row["NameBase"].ToString().ToReverseXSS();

                ctbMultiLang.Text = GetMuiltiLang(
                        row["NameKo"].ToString().ToReverseXSS(),
                        row["NameEn"].ToString().ToReverseXSS(),
                        row["NameJa"].ToString().ToReverseXSS(),
                        row["NameFr"].ToString().ToReverseXSS(),
                        row["NameZhcn"].ToString().ToReverseXSS(),
                        row["NameZhtw"].ToString().ToReverseXSS());               

                cbtnExplan.Text = row["Explan"].ToString().ToReverseXSS();

                cbShowImage.Checked = (row["OptionShowImage"].ToString() == "1") ? true : false;
                cbUseReply.Checked = (row["OptionUseReply"].ToString() == "1") ? true : false;
                cbUseShortReply.Checked = (row["OptionUseShortReply"].ToString() == "1") ? true : false;
                cbShortReplyGood.Checked = (row["OptionShortReplyGood"].ToString() == "1") ? true : false;
                cbShortReplyTop.Checked = (row["OptionShortReplyTop"].ToString() == "1") ? true : false;
                cbShowViewer.Checked = (row["OptionShowViewer"].ToString() == "1") ? true : false;
                cbVote.Checked = (row["OptionVote"].ToString() == "1") ? true : false;
                cbUseView.Checked = (row["OptionUseView"].ToString() == "1") ? true : false;
                cbAnonymity.Checked = (row["OptionAnonymity"].ToString() == "1") ? true : false;
                cbAlarm.Checked = (Convert.ToInt32(row["OptionAlarm"]) > 0) ? true : false;
                cbListType.Checked = true;
                cbMobile.Checked = (row["OptionMobile"].ToString() == "1") ? true : false;
                cbPopUp.Checked = (row["OptionPopUp"].ToString() == "1") ? true : false;
                cbLinkUrl.Checked = (row["OptionLinkURL"].ToString() == "1") ? true : false;
                cbDownLoad.Checked = (row["OptionDownLoad"].ToString() == "1") ? true : false;
                cbUseDept.Checked = (row["OptionUseDept"].ToString() == "1") ? true : false;
                ddlAlarm.SelectedValue = (cbAlarm.Checked) ? row["OptionAlarm"].ToString() : "0";
                ddlListType.SelectedValue = (row["OptionListType"] == null) ? "0" : row["OptionListType"].ToString();
                cbUseDept.Enabled = (row["OptionUseDept"].ToString() == "0") ? true : false;
                cbVoteView.Checked = (row["OptionVoteView"].ToString() == "1") ? true : false;
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 카테고리 추가
        /// </summary>
        private void Register()
        {
            Board.BE.CategoryInfo categoryInfo = new BE.CategoryInfo();
            categoryInfo.CompanyID = CurrentUserInfo.CompanyID;
            categoryInfo.CategoryID = 0;
            categoryInfo.ParentCategoryID = pCategoryID;
            categoryInfo.NameBase = ctbTitle.Text.Trim();

            MultiName = ctbMultiLang.Text.Trim();
            if (MultiName != "" && MultiName.IndexOf(",") != -1)
            {
                categoryInfo.NameKo = MultiName.Split(',')[0];
                categoryInfo.NameEn = MultiName.Split(',')[1];
                categoryInfo.NameJa = MultiName.Split(',')[2];
                categoryInfo.NameFr = MultiName.Split(',')[3];
                categoryInfo.NameZhcn = MultiName.Split(',')[4];
                categoryInfo.NameZhtw = MultiName.Split(',')[5];
            }

            categoryInfo.Explan = cbtnExplan.Text.Trim();
            categoryInfo.OptionUseSecure = "";
            categoryInfo.OptionShowImage = (cbShowImage.Checked) ? "1" : "0";
            categoryInfo.OptionUseReply = (cbUseReply.Checked) ? "1" : "0";
            categoryInfo.OptionUseShortReply = (cbUseShortReply.Checked) ? "1" : "0";
            categoryInfo.OptionShortReplyGood = (cbShortReplyGood.Checked) ? "1" : "0";
            categoryInfo.OptionShortReplyTop = (cbShortReplyTop.Checked) ? "1" : "0";
            categoryInfo.OptionShowViewer = (cbShowViewer.Checked) ? "1" : "0";
            categoryInfo.OptionUseDept = (cbUseDept.Checked) ? "1" : "0";
            categoryInfo.OptionVote = (cbVote.Checked) ? "1" : "0";
            categoryInfo.OptionVoteView = (this.cbVoteView.Checked) ? "1" : "0";
            categoryInfo.OptionUseView = (cbUseView.Checked) ? "1" : "0";
            categoryInfo.OptionAnonymity = (cbAnonymity.Checked) ? "1" : "0";
            categoryInfo.OptionAlarm = (cbAlarm.Checked) ? Convert.ToInt32(ddlAlarm.SelectedValue) : 0;
            categoryInfo.OptionListType = (this.cbListType.Checked) ? this.ddlListType.SelectedValue : "0";
            categoryInfo.OptionMobile = (this.cbMobile.Checked) ? "1" : "0";
            categoryInfo.OptionPopUp = (this.cbPopUp.Checked) ? "1" : "0";
            categoryInfo.OptionLinkURL = (this.cbLinkUrl.Checked) ? "1" : "0";
            categoryInfo.OptionDownLoad = (this.cbDownLoad.Checked) ? "1" : "0";
            categoryInfo.TemplateID = (cbTemplateID.Checked) ? ddlTemplate.SelectedValue.ToNumber<int>() : 0;
            categoryInfo.WriteUserID = CurrentUserInfo.UserID;

            try
            {
                List<BE.AuthorityInfo> al = new List<BE.AuthorityInfo>();

                if (cbUseDept.Checked == false) //부서별게시판이면 권한 할당하지 않는다.
                {
                    string authValue = tbAuthValue.Text.Trim();
                    if (authValue.Length > 0)
                    {
                        foreach (string _authValue in authValue.Split(','))
                        {
                            //권한값$구분$키값$이름
                            string[] authValStr = _authValue.Split('$');

                            BE.AuthorityInfo authInfo = new BE.AuthorityInfo();
                            authInfo.CompanyID = CurrentUserInfo.CompanyID;
                            authInfo.CategoryID = 0;
                            authInfo.Type = authValStr[1];
                            authInfo.OwnerID = authValStr[2].ToNumber<int>();
                            authInfo.Authority = authValStr[0];
                            authInfo.WriteUserID = CurrentUserInfo.UserID;

                            al.Add(authInfo);
                        }
                    }
                }

                BLL.Authority biz = new BLL.Authority();

                int categoryID = biz.AddAuthCategory(categoryInfo, al);

                Dictionary<string, string> paraDic = new Dictionary<string, string>();
                paraDic.Add("menuCatID", this.MenuCatID.ToString());
                paraDic.Add("catID", categoryID.ToString());
                paraDic.Add("optionUseDept", categoryInfo.OptionUseDept);
                paraDic.Add("isTotBoard", "0");
                string url = @"parent.frmLeft.location='{0}';";
                string encParams = GetEncryptParam(Helpers.WebComponent.Utility.GetParameterDic(paraDic));
                url = string.Format(url, "/DeskplusEIP/Board/Admin/Menu/Menu.aspx?p=" + encParams);
                Helpers.WebComponent.Utility.ShowMessageBox(Page, "addSuccess", GetResource("CompleteFolder"), url);
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                Helpers.WebComponent.Utility.ShowMessageBox(Page, "fail", GetResource("ProcessingError"), "");
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 카테고리 수정
        /// </summary>
        private void Modify()
        {
            #region 카테고리

            Board.BE.CategoryInfo categoryInfo = new BE.CategoryInfo();
            categoryInfo.CompanyID = CurrentUserInfo.CompanyID;
            categoryInfo.CategoryID = CatID;
            categoryInfo.NameBase = ctbTitle.Text.Trim();

            MultiName = this.ctbMultiLang.Text.Trim();
            if (MultiName != "" && MultiName.IndexOf(",") != -1)
            {
                categoryInfo.NameKo = MultiName.Split(',')[0];
                categoryInfo.NameEn = MultiName.Split(',')[1];
                categoryInfo.NameJa = MultiName.Split(',')[2];
                categoryInfo.NameFr = MultiName.Split(',')[3];
                categoryInfo.NameZhcn = MultiName.Split(',')[4];
                categoryInfo.NameZhtw = MultiName.Split(',')[5];
            }

            categoryInfo.Explan = cbtnExplan.Text.Trim();
            categoryInfo.OptionUseSecure = "";
            categoryInfo.OptionShowImage = (cbShowImage.Checked) ? "1" : "0";
            categoryInfo.OptionUseReply = (cbUseReply.Checked) ? "1" : "0";
            categoryInfo.OptionUseShortReply = (cbUseShortReply.Checked) ? "1" : "0";
            categoryInfo.OptionShortReplyGood = (cbShortReplyGood.Checked) ? "1" : "0";
            categoryInfo.OptionShortReplyTop = (cbShortReplyTop.Checked) ? "1" : "0";
            categoryInfo.OptionShowViewer = (cbShowViewer.Checked) ? "1" : "0";
            categoryInfo.OptionUseDept = (cbUseDept.Checked) ? "1" : "0";
            categoryInfo.OptionVote = (cbVote.Checked) ? "1" : "0";
            categoryInfo.OptionVoteView = (this.cbVoteView.Checked) ? "1" : "0";
            categoryInfo.OptionUseView = (cbUseView.Checked) ? "1" : "0";
            categoryInfo.OptionAnonymity = (cbAnonymity.Checked) ? "1" : "0";
            categoryInfo.OptionAlarm = (cbAlarm.Checked) ? Convert.ToInt32(ddlAlarm.SelectedValue) : 0;
            categoryInfo.OptionListType = (this.cbListType.Checked) ? this.ddlListType.SelectedValue : "0";
            categoryInfo.TemplateID = (cbTemplateID.Checked) ? ddlTemplate.SelectedValue.ToNumber<int>() : 0;
            categoryInfo.OptionMobile = (this.cbMobile.Checked) ? "1" : "0";
            categoryInfo.OptionPopUp = (this.cbPopUp.Checked) ? "1" : "0";
            categoryInfo.OptionLinkURL = (this.cbLinkUrl.Checked) ? "1" : "0";
            categoryInfo.OptionDownLoad = (this.cbDownLoad.Checked) ? "1" : "0";
            categoryInfo.ModifyUserID = CurrentUserInfo.UserID;

            BLL.Category bizCat = new BLL.Category();
            bizCat.Mod(categoryInfo, cbSubApply.Checked);

            #endregion 카테고리

            #region 권한

            List<BE.AuthorityInfo> alAuth = new List<BE.AuthorityInfo>();

            if (cbUseDept.Checked == false) //부서별게시판이면 권한 할당하지 않는다.
            {
                string authValue = tbAuthValue.Text.Trim();
                if (authValue.Length > 0)
                {
                    foreach (string _authValue in authValue.Split(','))
                    {
                        //권한값$구분$키값$이름
                        string[] authValStr = _authValue.Split('$');

                        BE.AuthorityInfo authInfo = new BE.AuthorityInfo();
                        authInfo.CompanyID = CurrentUserInfo.CompanyID;
                        authInfo.CategoryID = CatID;
                        authInfo.Type = authValStr[1];
                        authInfo.OwnerID = authValStr[2].ToNumber<int>();
                        authInfo.Authority = authValStr[0];
                        authInfo.WriteUserID = CurrentUserInfo.UserID;

                        alAuth.Add(authInfo);
                    }
                }
            }

            BLL.Authority bizAuth = new BLL.Authority();
            bizAuth.Mod(alAuth, CurrentUserInfo.CompanyID, CatID, cbAuthSubApply.Checked);

            Helpers.WebComponent.Utility.ShowMessageBox(Page, "addSuccess", GetResource("Processing"), "location='" + Request.Url.ToString() + "';");

            #endregion 권한
        }

        #endregion (함수)

        #region (이벤트핸들러)

        /// <summary>
        /// Handles the Click event of the LBTNProcess control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void LBTNProcess_Click(object sender, EventArgs e)
        {
            GetParam();

            string args = eventArgs;
            switch (args)
            {
                case "Modify":  //수정
                    Modify();
                    break;

                case "Register":  //추가
                    Register();
                    break;
            }

            //게시판 카테고리 정보 전역캐쉬 삭제
            BLL.Category.ClearCacheCategoryInfo();
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

        #endregion (이벤트핸들러)
    }
}