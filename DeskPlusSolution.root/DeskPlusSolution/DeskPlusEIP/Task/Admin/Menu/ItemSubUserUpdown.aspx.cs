using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using System.Data;
using System.Text;
using Helpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Task.DAL;


namespace Zeospace.DeskPlus.Task.Web.Admin.Menu
{
    public partial class ItemSubUserUpdown : Base.BasePageTaskAdmin
    {
        #region 변수설정       
        public string ItemSubUserList = ""; 
        #endregion
        
        #region Page Process (페이지처리)

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Task.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Task.Web.Admin.Menu.ItemSubUserUpdown).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                SetLanguage();
                LoadData();
            }

        }
        #endregion

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        private void LoadData()
        {
            DAL.ItemSubUser itemSub = new DAL.ItemSubUser();

            if (IsType == "TO")
            {
                DataTableReader dtr = itemSub.GetSubToUser(CurrentUserInfo.CompanyID, ItemSubID);

                StringBuilder sb = new StringBuilder();

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {

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

                            string deptname = this.GetLangName(
                                dtr["DeptBase"].ToString(),
                                dtr["DeptKo"].ToString(),
                                dtr["DeptEn"].ToString(),
                                dtr["DeptJa"].ToString(),
                                dtr["DeptFr"].ToString(),
                                dtr["DeptzhCn"].ToString(),
                                dtr["DeptZhTw"].ToString(), PreLang);

                            string titleName = this.GetLangName(
                                dtr["TitleBase"].ToString(),
                                dtr["TitleKo"].ToString(),
                                dtr["TitleEn"].ToString(),
                                dtr["TitleJa"].ToString(),
                                dtr["TitleFr"].ToString(),
                                dtr["TitlezhCn"].ToString(),
                                dtr["TitleZhTw"].ToString(), PreLang);
                            #endregion

                            sb.Append("<li id='");
                            sb.Append(dtr["ItemSubUserID"].ToString());
                            sb.Append("'>");
                            sb.Append(name + "  " + deptname + "  " + titleName);
                            sb.Append("</li>");



                        }
                        ItemSubUserList = sb.ToString();
                    }
                }
            }
            else
            {
                DataTableReader dtr = itemSub.GetSubCcUser(CurrentUserInfo.CompanyID, ItemSubID);

                StringBuilder sb = new StringBuilder();

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {

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

                            string deptname = this.GetLangName(
                                dtr["DeptBase"].ToString(),
                                dtr["DeptKo"].ToString(),
                                dtr["DeptEn"].ToString(),
                                dtr["DeptJa"].ToString(),
                                dtr["DeptFr"].ToString(),
                                dtr["DeptzhCn"].ToString(),
                                dtr["DeptZhTw"].ToString(), PreLang);

                            string titleName = this.GetLangName(
                                dtr["TitleBase"].ToString(),
                                dtr["TitleKo"].ToString(),
                                dtr["TitleEn"].ToString(),
                                dtr["TitleJa"].ToString(),
                                dtr["TitleFr"].ToString(),
                                dtr["TitlezhCn"].ToString(),
                                dtr["TitleZhTw"].ToString(), PreLang);
                            #endregion

                            if (dtr["IsGroup"].ToString() == "G")
                            {
                                sb.Append("<li id='");
                                sb.Append(dtr["ItemSubUserID"].ToString());
                                sb.Append("'>");
                                sb.Append(name);
                                sb.Append("</li>");

                            }
                            else
                            {
                                sb.Append("<li id='");
                                sb.Append(dtr["ItemSubUserID"].ToString());
                                sb.Append("'>");
                                sb.Append(name + "  " + deptname + "  " + titleName);
                                sb.Append("</li>");
                            }

                        }
                        ItemSubUserList = sb.ToString();
                    }
                }
            }
        }

        #endregion

        #region 함수
        /// <summary>
        /// 작성자 : maniaysw
        /// 작성일자 : 2015-04-14
        /// 작성목적 : 다국어
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                cbtnConfirm.Text = resMng.GetString("Apply");
                cbtnClose.Text = resMng.GetString("Close");
                ltlTitle.Text = resMng.GetString("SpecifierOrder");
            }

        }


        #endregion

        #region 속성

        #endregion

        #region 이벤트 핸들러

        #endregion

    }
}