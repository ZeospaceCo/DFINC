using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using Zeospace.DeskPlus.Board.BE;
using System.Text;

namespace Zeospace.DeskPlus.Board.Web.Content
{  
    public partial class AuthInfo : Board.Web.Base.BasePageBoard
    {
        public string AuthTable = "";
        public string UseView = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보

            ResourceName = "Zeospace.DeskPlus.Board.Web.Resource.GlobalString.string";			//리소스 명 선언
            AssemblyName = typeof(Zeospace.DeskPlus.Board.Web.Content.AuthInfo).Assembly;	//어셈블리 선언

            #endregion 다국어 리소스 로드/Title 정보

            if (!IsPostBack)
            {
                SetLanguage();
                LoadData();
            }
        }

        private void SetLanguage()
        {
            this.GetResourceManager();
            CbtnClose.Text = GetResource("Close");
        }

        private void LoadData()
        {
            LoadParam();
            LoadAuthority();
        }

        /// <summary>
        /// 권한 가져오기
        /// </summary>
        private void LoadAuthority()
        {
            Zeospace.DeskPlus.Board.DAL.Authority bll = new Zeospace.DeskPlus.Board.DAL.Authority();
            DataTableReader dtr = bll.GetAuthority(CurrentUserInfo.CompanyID, CategoryID);
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


                        AuthTable += CreateTableAuth(i, dtr["Type"].ToString()
                            + "$" + dtr["OwnerID"].ToString(),
                            name, dtr["Authority"].ToString().ToNumber<int>());

                        i += 1;
                    }
                    
                }
                else
                {
                    AuthTable += CreateTableAuth(1, "", GetResource("AllDeptUser"), 4);      
                }
            }
        }

        /// <summary>
        /// 파라메터 가져오기
        /// </summary>
        private void LoadParam()
        {
            CategoryID = GetDecryptParam("categoryID", "0").ToNumber<int>();
            UseView = GetDecryptParam("optionUseView", "0");
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
            string selected = "";

            if (UseView == "1") //만약 하위 읽기 전용 카테고리라면 권한명은 읽음으로 표시한다.
            {
                selected = GetResource("ReadOnlyBoard");                    
            }
            else //읽기 전용이 아니라면 권한에 따라 보여준다. 
            {
                switch (_auth)
                {
                    case 1:
                        selected = GetResource("Read");
                        break;

                    case 2:
                        selected = GetResource("Write");
                        break;

                    case 3:
                        selected = GetResource("ReadReply");
                        break;

                    case 4:
                        selected = GetResource("WriteReply");
                        break;
                }
            }


            

            string tempStr = "<tr id='boardAuth" + _idx + "'>"
            + "<td class='center'>"
            + _name
            + "</td>"
            + "<td class='center'>"
            + selected
            + "</td>" 
            + "</tr>";

            return tempStr;
        }

    }
}