using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using Zeospace.DeskPlus.RM.BE;

namespace Zeospace.DeskPlus.RM.Web.Admin.Content
{
    public partial class View : Zeospace.DeskPlus.RM.Web.Base.BasePageRMAdmin
    {
        #region 변수설정
        public int CategoryID;
        public int ListID;
        public int CurrentPage = 0;
        public string SearchField = String.Empty; //검색키워드
        public string SearchValue = String.Empty; //검색어
        public string SearchTerm = String.Empty;  //기간검색키워드
        public string SearchTermValue = String.Empty;  //기간검색어       
        public string Order = String.Empty; //sort           
        #endregion

        #region Page Process (페이지 처리)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.RM.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.RM.Web.Admin.Content.View).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                SetLanguage();
                LoadData();
            }
        }
        #endregion

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-02-21
        /// 작성목적 : Data Load
        /// </summary>
        private void LoadData()
        {
            LoadParam();
            GetData();
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-02-22
        /// 작성목적 : 자원 정보 가져오기
        /// </summary>
        private void GetData()
        {
            DAL.List dal = new DAL.List();

            DataTableReader dtr = dal.GetListInfo(CurrentUserInfo.CompanyID, ListID);

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        ltFolderName.Text = this.GetLangName(dtr["CNameBase"].ToString(), dtr["CNameKo"].ToString(),
                                            dtr["CNameEn"].ToString(), dtr["CNameJa"].ToString(), dtr["CNameFr"].ToString(),
                                            dtr["CNameZhcn"].ToString(), dtr["CNameZhtw"].ToString(), PreLang);

                        string title = dtr["Title"].ToString();
                        ltName.Text = title;
                        ltCount.Text = dtr["Qty"].ToString();
                        ltCode.Text = dtr["Code"].ToString();
                        ltReCount.Text = dtr["UsableQty"].ToString();
                        ltPlace.Text = dtr["Place"].ToString();
                        ltQRCode.Text = (dtr["QRCodeUse"].ToString() == "0") ? resMng.GetString("No") : resMng.GetString("Yes");
                        ltAdminConfirm.Text = (dtr["Authority"].ToString() == "0") ? resMng.GetString("No") : resMng.GetString("Yes");
                        //IconName = BaseImgPath + "/RM/rs_icon" + dtr["IconName"].ToString() + ".png";

                        if (dtr["IconName"].ToString() == "1")
                        {
                            IconName = "<i class='fa fa-building' style='font-size:2em;'></i>";
                        }
                        else if (dtr["IconName"].ToString() == "2")
                        {
                            IconName = "<i class='fa fa-address-book' style='font-size:2em;'></i>";
                        }
                        else if (dtr["IconName"].ToString() == "3")
                        {
                            IconName = "<i class='fa fa-calendar' style='font-size:2em;'></i>";
                        }
                        else if (dtr["IconName"].ToString() == "4")
                        {
                            IconName = "<i class='fa fa-car' style='font-size:2em;'></i>";
                        }
                        else if (dtr["IconName"].ToString() == "5")
                        {
                            IconName = "<i class='fa fa-comments' style='font-size:2em;'></i>";
                        }
                        else if (dtr["IconName"].ToString() == "6")
                        {
                            IconName = "<i class='fa fa-fax' style='font-size:2em;'></i>";
                        }
                        else if (dtr["IconName"].ToString() == "7")
                        {
                            IconName = "<i class='fa fa-desktop' style='font-size:2em;'></i>";
                        }
                        else if (dtr["IconName"].ToString() == "8")
                        {
                            IconName = "<i class='fa fa-microphone' style='font-size:2em;'></i>";
                        }
                        else if (dtr["IconName"].ToString() == "9")
                        {
                            IconName = "<i class='fa fa-print' style='font-size:2em;'></i>";
                        }

                        ltDesc.Text = dtr["Content"].ToString().Replace("\r\n", "<br>");

                        if (dtr["QRCodeUse"].ToString() == "1")
                        {
                            QRCode.Visible = true;
                            QRCodeList.Visible = true;

                            title = title.ToReverseXSS();
                            title = title.Replace("\\", "");
                            title = title.Replace("/", "");
                            title = title.Replace(":", "");
                            title = title.Replace("*", "");
                            title = title.Replace("?", "");
                            title = title.Replace("\"", "");
                            title = title.Replace("<", "");
                            title = title.Replace(">", "");
                            title = title.Replace("|", "");

                            if (title.Length > 10)
                            {
                                title = title.Substring(0, 10);
                            }

                            if (title.Length == 0)
                            {
                                title = "QRCode";
                            }

                            title = title + ".png";
                            
                            string QRCodePath = dtr["QRCodePath"].ToString();
                            Dictionary<string, string> paraDic = new Dictionary<string, string>();
                            paraDic.Add("filePath", QRCodePath);
                            paraDic.Add("fileName", title);
                            QRCodePath = GetEncryptParam(Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.GetParameterDic(paraDic));

                            //QRCodeContent = "<tr><td class='center'><a onclick=\"View.FileDownload('" + QRCodePath + "')\">" + title +"</a></td>";
                            //QRCodeContent += "<td class='center'><a onclick=\"View.FileDownload('" + QRCodePath + "')\" style='display:inline-block;'><span class=\"btn_file_dw\" title=\"다운로드\"></span></a></td></tr>";

                            QRCodeContent += "<li><a onclick=\"View.FileDownload('" + QRCodePath + "')\">" + title + "</a></li>"; ;

                        }
                        else
                        {
                            QRCode.Visible = false;
                            QRCodeList.Visible = false;
                        }                      
                    }
                }               
            }
        }
     
        #endregion

        #region (함수)

        /// <summary>
        /// 언어 설정하기
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();

            if (!this.resMng.Equals(null))
            {
                cbtnDel.Text = resMng.GetString("Del");   
                cbtnMod.Text = resMng.GetString("Mod");         
                cbtnPrint.Text = resMng.GetString("Print");
                cbtnList.Text = resMng.GetString("list");       
            }
        }


        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-02-22
        /// 작성목적 : 파라미터
        /// </summary>
        private void LoadParam()
        {
            CategoryID = GetDecryptParam("categoryID", "0").ToNumber<int>();
            ListID = GetDecryptParam("listID", "0").ToNumber<int>();
            CurrentPage = GetDecryptParam("currentPage", "1").ToNumber<int>();
            SearchField = GetDecryptParam("searchField", "");
            SearchValue = GetDecryptParam("searchValue", "");
            SearchTerm = GetDecryptParam("searchTerm", "");
            SearchTermValue = GetDecryptParam("searchTermValue", "");
            Order = GetDecryptParam("order", "");
            Order = (Order.Length == 0) ? "ListOrder, WriteDate desc" : Order;
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-02-23
        /// 작성목적 : 자원 삭제
        /// </summary>
        /// <returns></returns>
        private bool Del()
        {                
           DAL.List dal = new DAL.List();
           return dal.Upd(CurrentUserInfo.CompanyID, "0", ListID.ToString());
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-02-23
        /// 작성목적 : 자원 삭제
        /// </summary>
        private void DelList()
        {
            if (Del())
            {
                Dictionary<string, string> paraDic = new Dictionary<string, string>();
                paraDic.Add("categoryID", CategoryID.ToString());
                paraDic.Add("currentPage", CurrentPage.ToString());
                paraDic.Add("searchTerm", SearchTerm.ToString());
                paraDic.Add("searchTermValue", SearchTermValue.ToString());
                paraDic.Add("searchField", SearchField.ToString());
                paraDic.Add("searchValue", SearchValue.ToString());
                paraDic.Add("order", Order.ToString());
                paraDic.Add("alertType", AlertMsgEnum.Alert_Delete.ToString());
                paraDic.Add("alertMsg", GetResource("GoList"));
                paraDic.Add("goURL", "/DeskPlusEIP/RM/Admin/Content/List.aspx");

                string encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));
                Utility.Redirect(encParams);
            }
        }



        #endregion

        #region (프로퍼티)

        /// <summary>
        /// IconName
        /// </summary>
        public string IconName
        {
            get { return GetViewStateString("IconName", ""); }
            set { ViewState["IconName"] = value; }
        } 

        /// <summary>
        /// QRCodeContent
        /// </summary>
        public string QRCodeContent
        {
            get { return GetViewStateString("QRCodeContent", ""); }
            set { ViewState["QRCodeContent"] = value; }
        } 

        #endregion

        #region (이벤트핸들러)

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-02-23
        /// 작성목적 : 삭제 버튼 클릭시 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnConfirm_Click(object sender, EventArgs e)
        {
            LoadParam();

            DelList();
        }
        #endregion
    }
}