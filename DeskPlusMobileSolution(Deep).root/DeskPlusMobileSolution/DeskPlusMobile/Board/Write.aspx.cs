using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlusMobile.Web.Helper;
using DateHelpers = Zeospace.DeskPlusMobile.Web.Helper.WebComponent.DateHelper;
using System.Xml;
using Zeospace.DeskPlusMobile.Web.Board.Ws.BE;
using Zeospace.DeskPlusMobile.Web.Board.Ws.Data;
using System.Data;
using System.IO;

namespace Zeospace.DeskPlusMobile.Web.Board
{
    public partial class Write : Zeospace.DeskPlusMobile.Web.Helper.MobileBasePage
    {
        #region 변수설정
        public int pageNo;
        public int mainCID;
        public string searchValue;
        public string pCatID;
        public int draftListID;
        public string FileCheck;
        public string blockExt;
        public string upFileSize;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlusMobile.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlusMobile.Web.Board.Write).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                SetLanguage();
                Today = DateHelpers.GetCultureDate(DateTime.Now, UserInfo.PreferredLanguage).Substring(0, 10);

                switch (DateTime.Now.DayOfWeek)
                {
                    case DayOfWeek.Sunday: Today += "(" + resMng.GetString("Sun") + ")"; break;
                    case DayOfWeek.Monday: Today += "(" + resMng.GetString("Mon") + ")"; break;
                    case DayOfWeek.Tuesday: Today += "(" + resMng.GetString("Tue") + ")"; break;
                    case DayOfWeek.Wednesday: Today += "(" + resMng.GetString("Wed") + ")"; break;
                    case DayOfWeek.Thursday: Today += "(" + resMng.GetString("Thu") + ")"; break;
                    case DayOfWeek.Friday: Today += "(" + resMng.GetString("Fri") + ")"; break;
                    case DayOfWeek.Saturday: Today += "(" + resMng.GetString("Sat") + ")"; break;
                };

                LoadParam();
                LoadData();
            }
        }

        /// <summary>
        /// Loads the param.
        /// </summary>
        private void LoadParam()
        {
            pageNo = GetDecryptParam("PageNo", "1").ToNumber<int>();
            mainCID = GetDecryptParam("MainCID", "").ToNumber<int>();
            searchValue = GetDecryptParam("Sv", "");
            pCatID = GetDecryptParam("PCatID", "");
            draftListID = GetDecryptParam("DraftListID", "0").ToNumber<int>();
            SwfUploadGUID = GetDecryptParam("SwfUploadGUID", "");
            FileCheck = GetDecryptParam("FileCheck", "N");
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2015-06-24
        /// 작성목적 : 다국어
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {

            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void LoadData()
        {
            blockExt = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "BlockExt");
            upFileSize = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "UpFileSize");

            BoaComID = Ws.BLL.Category.GetCompanyCategoryInfo(pCatID.ToNumber<int>());

            GetCategory();
            SetFontColor();
            SetFontSize();
            SetCategoryHeader();

            ddlDay.Items.Clear();
            for (int i = 5; i < 101; i = i + 5)
            {
                ddlDay.Items.Add(new ListItem(i.ToString(), i.ToString()));
            }

            if (draftListID > 0)
            {
                GetDraft();
            }

            try
            {
                if (FileCheck == "N")
                {
                    SwfUploadGUID = System.Guid.NewGuid().ToString();
                    string uploadPath = Server.MapPath("/DeskPlusMobile/FileData/" + CompanyID + "/Users/" + UserID + "/Mobile/");
                    if (Directory.Exists(uploadPath)) Directory.Delete(uploadPath, true);
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// 카테고리 정보 가져오기
        /// </summary>
        private void GetCategory()
        {
            Ws.Data.Category dal = new Ws.Data.Category();
            DataSet ds = dal.GetCategory(BoaComID, pCatID.ToNumber<int>());

            if (ds != null)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    string name = this.GetLangName(
                       dr["PathNameBase"].ToString(),
                       dr["PathNameKo"].ToString(),
                       dr["PathNameEn"].ToString(), dr["PathNameJa"].ToString(),
                       dr["PathNameFr"].ToString(), dr["PathNameZhCn"].ToString(),
                       dr["PathNameZhTw"].ToString(), PreLang);

                    switch (PreLang)
                    {
                        case "ko":
                            name = (dr["NameKo"].ToString() == "") ? dr["PathNameBase"].ToString() : name;
                            break;

                        case "en":
                            name = (dr["NameEn"].ToString() == "") ? dr["PathNameBase"].ToString() : name;
                            break;

                        case "ja":
                            name = (dr["NameJa"].ToString() == "") ? dr["PathNameBase"].ToString() : name;
                            break;

                        case "fr":
                            name = (dr["NameFr"].ToString() == "") ? dr["PathNameBase"].ToString() : name;
                            break;

                        case "zhcn":
                            name = (dr["NameZhcn"].ToString() == "") ? dr["PathNameBase"].ToString() : name;
                            break;

                        case "zhtw":
                            name = (dr["NameZhtw"].ToString() == "") ? dr["PathNameBase"].ToString() : name;
                            break;
                    }

                    CategoryName = name.Substring(1, name.Length - 1);
                    OptionAnonymity = dr["OptionAnonymity"].ToString();
                    cbOptionAnonymity.Visible = (OptionAnonymity == "1") ? true : false;
                    spAnonymity.Visible = (OptionAnonymity == "1") ? true : false;
                    OptionAlarm = dr["optionAlarm"].ToString();
                    OptionListType = dr["optionListType"].ToString();
                    cbPopUp.Visible = dr["OptionPopUp"].ToString() == "1" ? true : false;
                    spPopUp.Visible = dr["OptionPopUp"].ToString() == "1" ? true : false;
                    cbCheckDownLoad.Checked = dr["OptionDownload"].ToString() == "1" ? true : false;
                }
            }
        }

        /// <summary>
        /// 말머리
        /// </summary>
        private void SetCategoryHeader()
        {
            string name = "";
            ddlCatHeader.Items.Clear();
            ddlCatHeader.Items.Add(new ListItem(GetResource("SelectHeader"), "0"));
            Ws.Data.Header dal = new Ws.Data.Header();
            List<Ws.BE.HeaderInfo> al = dal.GetHeaderList(BoaComID, pCatID.ToNumber<int>(), PreLang);

            foreach (Ws.BE.HeaderInfo be in al)
            {
                #region 다국어 이름 설정

                name = this.GetLangName(
                    be.NameBase.ToReverseXSS(),
                    be.NameKo.ToReverseXSS(), be.NameEn.ToReverseXSS(), be.NameJa.ToReverseXSS(),
                    be.NameFr.ToReverseXSS(), be.NameZhcn.ToReverseXSS(), be.NameZhtw.ToReverseXSS(), PreLang);

                #endregion 다국어 이름 설정

                ListItem tmpItem = new ListItem(name, be.HeaderID.ToString());
                this.ddlCatHeader.Items.Add(tmpItem);
            }
        }

        /// <summary>
        /// 폰트 칼라
        /// </summary>
        private void SetFontColor()
        {
            ddlFontColor.Items.Clear();

            string xmlPath = Server.MapPath("/DeskPlusMobile/XmlData/FontColor/" + PreLang + ".xml");
            List<Ws.BE.XmlInfo> al = XmlLoadData(xmlPath);

            foreach (Ws.BE.XmlInfo be in al)
            {
                ListItem tmpItem = new ListItem(be.KeyValue, be.KeyID);
                tmpItem.Attributes.Add("style", "color:" + be.KeyID);
                this.ddlFontColor.Items.Add(tmpItem);
            }
        }

        /// <summary>
        /// 폰트사이즈
        /// </summary>
        private void SetFontSize()
        {
            ddlFontSize.Items.Clear();
            string xmlPath = Server.MapPath("/DeskPlusMobile/XmlData/FontSize/" + PreLang + ".xml");
            List<Ws.BE.XmlInfo> al = XmlLoadData(xmlPath);

            foreach (Ws.BE.XmlInfo be in al)
            {
                ListItem tmpItem = new ListItem(be.KeyValue, be.KeyID);
                this.ddlFontSize.Items.Add(tmpItem);
            }
        }

        public static List<Ws.BE.XmlInfo> XmlLoadData(string _xmlPath)
        {
            XmlTextReader reader = null;
            List<Ws.BE.XmlInfo> al = new List<Ws.BE.XmlInfo>();

            try
            {
                reader = new XmlTextReader(_xmlPath);

                while (reader.Read())
                {
                    if (reader.HasAttributes)
                    {
                        if (reader.GetAttribute("Value") != "" && reader.GetAttribute("Value") != null)
                        {
                            Ws.BE.XmlInfo be = new Ws.BE.XmlInfo();
                            be.KeyText = reader.GetAttribute("Text");
                            be.KeyValue = reader.GetAttribute("Value");
                            be.KeyID = reader.GetAttribute("ID");

                            al.Add(be);
                        }
                    }
                }
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }

            return al;
        }

        /// <summary>
        /// 임시저장 정보 가져오기
        /// </summary>
        private void GetDraft()
        {
            Ws.Data.List dal = new Ws.Data.List();
            DataSet ds = dal.GetDraft(CompanyID, draftListID);

            if (ds != null)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    ddlCatHeader.SelectedValue = dr["HeaderID"].ToString();
                    ddlFontColor.SelectedValue = dr["FontColor"].ToString();
                    ddlFontSize.SelectedValue = dr["FontSize"].ToString();
                    cbxBold.Checked = (dr["Bold"].ToString() == "1") ? true : false;
                    cbOptionModify.Checked = (dr["OptionModify"].ToString() == "1") ? true : false;
                    cbOptionAnonymity.Checked = (dr["OptionAnonymity"].ToString() == "1") ? true : false;

                    if (dr["OptionTop"].ToString() == "1")
                    {
                        cbOptionTop.Checked = true;
                        OptionTop = dr["OptionTop"].ToString();
                        ddlDay.SelectedValue = dr["OptionTopDay"].ToString();
                    }

                    if (dr["PopUp"].ToString() == "1")
                    {
                        cbPopUp.Checked = true;
                        PopUp = dr["PopUp"].ToString();
                        tbStartDate.Value = dr["StartDate"].ToString();
                        tbEndDate.Value = dr["EndDate"].ToString();
                    }

                    tbTitle.Value = dr["Title"].ToString();
                    tbcontent.Value = dr["Content"].ToString();
                }
            }
        }


        public string Today { get; set; }

        public int BoaComID { get; set; }

        public string OptionAnonymity { get; set; }

        public string OptionAlarm { get; set; }

        public string OptionListType { get; set; }

        public string CategoryName { get; set; }

        public string OptionTop { get; set; }

        public string PopUp { get; set; }

        /// <summary>
        /// SwfUploadGUID
        /// </summary>
        public string SwfUploadGUID
        {
            get { return GetViewStateString("SwfUploadGUID", ""); }
            set { ViewState["SwfUploadGUID"] = value; }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성일자 : 2012-11-08
        /// 작성목적 : SSL 사용 유무      
        /// </summary>
        public string httpType
        {
            get
            {
                return Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "HTTP_TYPE");
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성일자 : 2012-11-08
        /// 작성목적 : 도메인 정보        
        /// </summary>
        public string MobileDOMAIN
        {
            get
            {
                return Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "MobileDOMAIN");
            }
        }
    }
}