using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Collections;

namespace Zeospace.DeskPlus.Board.Web.Admin.Statistic
{
    public partial class VisualChart : Board.Web.Base.BasePageBoardAdmin
    {
        #region Declaration (컨트롤 선언)

        #endregion

        #region 변수설정
        public string YY;
        #endregion

        #region Page Process (페이지 처리)
        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Board.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Board.Web.Admin.Statistic.VisualChart).Assembly;	//어셈블리 선언
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
        /// 작성자  : 심재운
        /// 언어 설정하기
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();

            if (!this.resMng.Equals(null))
            {
                
            }
        }

        /// <summary>
        /// Loads the param.
        /// </summary>
        private void LoadParam()
        {
            YY = GetDecryptParam("yy", System.DateTime.Now.Year.ToString());
        }

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 내용가져오기
        /// 작성자     : 심재운
        /// 최초작성일 : 2011-04-25
        /// </summary>
        private void LoadData()
        {
            LoadParam();
            LoadList();
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 용도 : 게시물 내용
        /// </summary>
        private void LoadList()
        {
            StringBuilder sb = new StringBuilder();

            

            //foreach
           
            DAL.Statistic dal = new DAL.Statistic();
            DataTableReader dtr = dal.GetStatYYMM(CurrentUserInfo.CompanyID, YY);

            Dictionary<int, int> dic = new Dictionary<int, int>();

            if (dtr != null)
            {
                if (dtr.HasRows)
                {                    
                    while (dtr.Read()) dic.Add(dtr["MM"].ToString().ToNumber<int>(), dtr["Cnt"].ToString().ToNumber<int>());
                }
            }

            string tmp = "";

            for (int i = 1; i <= 12; i++)
            {
                tmp = string.Format(@"<tr><th scope=""row"">{0}</th><td>0</td></tr>", YY + "-" + i.ToString());

                foreach (var o in dic)
                {
                    if (o.Key == i)
                    {
                        tmp = string.Format(@"<tr><th scope=""row"">{0}</th><td>{1}</td></tr>", YY + "-" + o.Key.ToString(), o.Value);
                        break;
                    }
                }

                sb.Append(tmp);
            }


            ltlConent.Text = sb.ToString();
        }

        #endregion

        #region (함수)

        

        #endregion

        #region (프로퍼티)

        #endregion

        #region (이벤트핸들러)

        #endregion
    }
}