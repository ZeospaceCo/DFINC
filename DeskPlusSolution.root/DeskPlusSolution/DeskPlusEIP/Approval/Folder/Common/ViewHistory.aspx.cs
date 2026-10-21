using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Approval.Web.Folder.Common
{
    public partial class ViewHistory : Base.BasePageApproval
    {
        #region 변수설정
        public int DocID;
        public int HistoryID;
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
            this.ResourceName = "Zeospace.DeskPlus.Approval.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.Folder.Common.ViewHistory).Assembly;	//어셈블리 선언
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

        /// <summary>
        /// 작성자 : 
        /// 작성일자 : 
        /// 작성목적 : Loads the data.
        /// </summary>
        private void LoadData()
        {
            DAL.History dal = new DAL.History();
            var alList =  dal.GetHistoryID(CurrentUserInfo.CompanyID, DocID, HistoryID);

            foreach (var info in alList)
            {
                ltlDocType.Text = info.Number3;

                Zeospace.DeskPlus.Common.DAL.Code codeDal = new DeskPlus.Common.DAL.Code();
                var codeList = codeDal.GetCodeByID(CurrentUserInfo.CompanyID, info.AccessTitleID);

                foreach (var codeInfo in codeList)
                {
                    string name = UtilHelper.GetLangName(codeInfo.NameBase, codeInfo.NameKo, codeInfo.NameEn,
                            codeInfo.NameJa, codeInfo.NameFr, codeInfo.NameZhcn, codeInfo.NameZhtw, PreLang);
                    ltlOpenAuth.Text = name;
                }

                ltlOpinion.Text = info.Opinion.ToNull();
                ltlAttach.Text = info.AttachFileInfo.ToNull();
                ltlBaseDoc.Text = info.BaseDocInfo.ToNull();


                ltlAttachCnt.Text = "0";
                ltlBaseDocCnt.Text = "0";

                if (info.AttachFileInfo.ToNull().Length > 0) ltlAttachCnt.Text = info.AttachFileInfo.Split(',').Count().ToString();
                if (info.BaseDocInfo.ToNull().Length > 0) ltlBaseDocCnt.Text = info.BaseDocInfo.Split(',').Count().ToString();

                htaFormHTML.Value = info.ContentHTML;
            }
        }

        #endregion

        #region (함수)

        /// <summary>
        /// 작성자 : 
        /// 작성일자 : 
        /// 작성목적 : 다국어
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                cbtnPrint.Text = resMng.GetString("Print");          //인쇄
                cbtnClose.Text = resMng.GetString("Close");  
            }
        }

        /// <summary>
        /// 작성자 : 
        /// 작성일자 : 
        /// 작성목적 : 파라미터
        /// </summary>
        private void LoadParam()
        {
            DocID = GetDecryptParam("docID", "0").ToNumber<int>();
            HistoryID = GetDecryptParam("historyID", "0").ToNumber<int>();
        }


        #endregion

        #region (프로퍼티)
        #endregion

        #region (이벤트핸들러)
        #endregion
    }
}