using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlus.Account.DAL;

namespace Zeospace.DeskPlus.Address.Web.Content
{
    public partial class Print : Base.BasePageAddress
    {

        #region Declaration (컨트롤 선언)
        #endregion

        #region 변수설정
        public string KeyID = String.Empty;
        public string Types = String.Empty;
        private DataTableReader dtrList = null;      
        #endregion

        #region Page Process (페이지 처리)
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Address.Web.Resource.GlobalString.string";	//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Address.Web.Content.Print).Assembly;	//어셈블리 선언
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
            LoadParam();

            //중요표시에서 인쇄 선택한 경우, 다양한 주소록 형태로 반환됨
            if (Types == "IG")
            {                
                if (KeyID.Length > 0)
                {
                    string queryList = ""; //(IMPO.UserID = 7 or IMPO.Types = 'GM')

                    foreach (string id in KeyID.Split(','))
                    {
                        queryList += "(IMPO.UserID = " + id.Split('|')[0] + " or IMPO.Types = '" + id.Split('|')[1] + "'),";                        
                    }

                    queryList = queryList.TrimEnd(',');
                    queryList = queryList.Replace(",", " OR ");

                    DAL.ImpoUser dal = new DAL.ImpoUser();
                    dtrList = dal.GetImpoPrintList(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, queryList);
                }
            }
            else
            {
                GetPrintList();
            }


            LoadPrintList();
        }

        #endregion

        #region (함수)

        private void SetLanguage()
        {
            this.GetResourceManager();

            if (!this.resMng.Equals(null))
            {
                cbtnPreview.Text = resMng.GetString("Preview");     // 미리보기
                cbtnPrint.Text = resMng.GetString("Print");         // 인쇄
                cbtnList.Text = resMng.GetString("BtnCancel");           // 목록                
            }
        }

        /// <summary>
        /// 파라미터명시
        /// </summary>
        private void LoadParam()
        {
            KeyID = GetDecryptParam("keyID", "");
            Types = GetDecryptParam("types", "");           
        }
 
        
        /// <summary>
        /// 인쇄를 선택한 사용자의 정보 가져오기
        /// </summary>
        private void GetPrintList()
        {
            Account.DAL.User user = new User();
            

            int companyID = CurrentUserInfo.CompanyID;

            switch (Types)
            {
                case "GG":      // 회사주소록
                    dtrList = user.GetUserListByAddress(companyID, KeyID, true);
                    break;
                case "CG":      // 공용주소록
                    {
                        DAL.Common comm = new DAL.Common(); 
                        dtrList = comm.GetUserListByAddress(companyID, KeyID);                                             
                    }
                    break;
                case "PG":      // 개인주소록
                    {
                        DAL.Private pr = new DAL.Private();
                        dtrList = pr.GetUserListByAddress(companyID, KeyID, CurrentUserInfo.UserID);
                    }
                    break;
                case "RG":      // 전달받은주소록
                    {
                        DAL.Private pr = new DAL.Private();
                        dtrList = pr.GetUserListByAddress(companyID, KeyID, CurrentUserInfo.UserID);
                    }
                    break;
                case "TG":      // 직책주소록
                    dtrList = user.GetUserListByAddress(companyID, KeyID, true);
                    break;
                case "DG":      // 직급주소록
                    dtrList = user.GetUserListByAddress(companyID, KeyID, true);
                    break;
                default:
                    dtrList = null;
                    break;
            }
        }
        /// <summary>
        /// 인쇄 할 테이블
        /// </summary>
        private void LoadPrintList()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<table class='table_typeA'>");
            sb.Append(" <caption>" + GetResource("SelectAddressList") + "</caption>");
            sb.Append("     <colgroup>");
            for (int i = 0; i < 6; i++)
            {
                sb.Append("     <col />");
            }
            sb.Append("     </colgroup>");
            sb.Append(PrintHeader());
            sb.Append(PrintContent());
            sb.Append("</table>");
            sb.Append("<table><tr><td></td></tr></table>");
            		    
            plContent.Controls.Add(new LiteralControl(sb.ToString()));
        }


        /// <summary>
        /// 인쇄 테이블의 헤더
        /// </summary>
        /// <returns></returns>
        private string PrintHeader()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<thead>");
            sb.Append(" <tr>");
            sb.Append("     <th class='All Name center' style='width:80px'>" + GetResource("FirstName") + "</th>");
            sb.Append("     <th class='All Email center'>" + GetResource("E-Mail") + "</th>");
            sb.Append("     <th class='All Mobile center' style='width:120px'>" + GetResource("MobilePhone") + "</th>");
            sb.Append("     <th class='All HomePhone center' style='width:120px'>" + GetResource("Phone") + "</th>");
            sb.Append("     <th class='All HomeZipCode center' style='width:80px'>" + GetResource("HomeZipCode") + "</th>");
            sb.Append("     <th class='All HomeAddress center'>" + GetResource("HomeAddress") + "</th>");
            sb.Append(" </tr>");
            sb.Append("</thead>");
            return sb.ToString();
        }

        /// <summary>
        /// 인쇄 테이블의 내용
        /// </summary>
        /// <returns></returns>
        private string PrintContent()
        {
            StringBuilder sb = new StringBuilder();            
            
            sb.Append("<tbody>");

            if (dtrList != null)
            {
                if (dtrList.HasRows)
                {
                    string name = "";

                    while (dtrList.Read())
                    {
                        name = this.GetLangName(
                                dtrList["NameBase"].ToString(),
                                dtrList["NameKo"].ToString(), dtrList["NameEn"].ToString(), dtrList["NameJa"].ToString(),
                                dtrList["NameFr"].ToString(), dtrList["NameZhCn"].ToString(), dtrList["NameZhTw"].ToString(), PreLang);

                        sb.Append(" <tr>");
                        sb.Append("     <td class='All Name center'>" + name + "</td>");
                        sb.Append("     <td class='All Email'>" + dtrList["Mail"] + "</td>");
                        sb.Append("     <td class='All Mobile center'>" + dtrList["MobilePhone"] + "</td>");
                        sb.Append("     <td class='All HomePhone center'>" + dtrList["HomePhone"] + "</td>");
                        sb.Append("     <td class='All HomeZipCode center'>" + dtrList["HomeZipCode"] + "</td>");
                        sb.Append("     <td class='All HomeAddress'>" + dtrList["HomeAddress"] + "</td>");
                        sb.Append(" </tr>");
                    }
                }
            }

            sb.Append("</tbody>");

            return sb.ToString();
        }

       

        #endregion

        #region (속성)
        #endregion

        #region (이벤트핸들러)
        #endregion
    }
}