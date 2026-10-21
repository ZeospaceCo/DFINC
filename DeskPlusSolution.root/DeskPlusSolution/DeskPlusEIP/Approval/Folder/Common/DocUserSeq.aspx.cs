using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;

namespace Zeospace.DeskPlus.Approval.Web.Folder.Common
{
    public partial class DocUserSeq : Base.BasePageApproval
    {
        #region 변수설정
        public int ChildDocID;
        public int GroupID;
        public int DocumentID;

        public string DocName = string.Empty;           // 문서명
        public string WriteName = string.Empty;         // 작성자
        public string WriteDate = string.Empty;         // 작성일자
        public string DocSubject = string.Empty;        // 제목   
        public string DeptName = string.Empty;          // 부서명
        public string TitleName = string.Empty;         // 직급명

        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Approval.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.Folder.Common.DocUserSeq).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                SetLanguage();
                LoadData();
            }
        }

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        private void LoadData()
        {
            LoadParam();

            DAL.Document dalDoc = new DAL.Document();
            DateTime dt;

            int parentDocID = dalDoc.GetParentDocID(CurrentUserInfo.CompanyID, ChildDocID);
            List<BE.DocumentInfo> alDoc = dalDoc.GetDocList(CurrentUserInfo.CompanyID, parentDocID);

            foreach (var docInfo in alDoc)
            {   
                // 문서명
                DocName = GetLangName(docInfo.FormBase,
                    docInfo.FormKo,
                    docInfo.FormEn,
                    docInfo.FormJa,
                    docInfo.FormFr,
                    docInfo.FormZhcn,
                    docInfo.FormZhtw, PreLang);

                // 작성자
                WriteName = GetLangName(docInfo.NameBase, 
                    docInfo.NameKo, 
                    docInfo.NameEn, 
                    docInfo.NameJa, 
                    docInfo.NameFr, 
                    docInfo.NameZhcn, 
                    docInfo.NameZhtw, PreLang);

                #region 작성일자
                if (DateTime.TryParse(docInfo.WriteTime.ToString(), out dt))
                    WriteDate = docInfo.WriteTime.ToShortDateString() + " " + docInfo.WriteTime.ToShortTimeString();
                else
                    WriteDate = string.Empty;
                #endregion

                // 제목
                DocSubject = docInfo.Subject.ToNull();

                // 부서명
                DeptName = GetLangName(docInfo.DeptBase, 
                    docInfo.DeptKo, 
                    docInfo.DeptEn, 
                    docInfo.DeptJa, 
                    docInfo.DeptFr, 
                    docInfo.DeptZhcn, 
                    docInfo.DeptZhtw, PreLang);
                
                // 직급명
                TitleName = GetLangName(docInfo.TitleBase,
                    docInfo.TitleKo,
                    docInfo.TitleEn,
                    docInfo.TitleJa,
                    docInfo.TitleFr,
                    docInfo.TitleZhcn,
                    docInfo.TitleZhtw, PreLang);

            }
           
        }
        #endregion

        #region (함수)

        /// <summary>
        /// 다국어
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                cbtnCancel.Text = resMng.GetString("Close");
            }
        }

        /// <summary>
        /// 작성자 : 
        /// 작성일자 : 
        /// 작성목적 : 파라미터
        /// </summary>
        private void LoadParam()
        {
            ChildDocID = GetDecryptParam("childDocID", "").ToNumber<int>();
            GroupID = GetDecryptParam("groupID", "").ToNumber<int>();
            DocumentID = GetDecryptParam("documentID", "").ToNumber<int>();
        }

        #endregion
    }
}