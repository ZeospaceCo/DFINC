using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;

namespace Zeospace.DeskPlus.Approval.Web.Folder.Carboncopy
{
    public partial class CirDocUser : Base.BasePageApproval
    {
        #region 변수설정

        public int CirDocumentID;
        private int DeptCodeID;
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
            this.AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.Folder.Carboncopy.CirDocUser).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                SetLanguage();
                LoadData();
            }
        }

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        /// <summary>
        /// Loads the data.
        /// </summary>
        private void LoadData()
        {
            LoadParam();

            DAL.CirculationDocument dal = new DAL.CirculationDocument();

            List<BE.CirculationDocumentInfo> al = dal.GetDocList(CurrentUserInfo.CompanyID, CirDocumentID);

            foreach (var cirDocInfo in al)
            {
                DeptCodeID = cirDocInfo.DeptCodeID;
                // 문서명
                DocName = cirDocInfo.Number3.ToNull();
                // 작성자
                WriteName = GetLangName(cirDocInfo.NameBase,
                    cirDocInfo.NameKo,
                    cirDocInfo.NameEn,
                    cirDocInfo.NameJa,
                    cirDocInfo.NameFr,
                    cirDocInfo.NameZhcn,
                    cirDocInfo.NameZhtw, PreLang);

                #region 작성일자
                DateTime dt;
                if (DateTime.TryParse(cirDocInfo.WriteTime.ToString(), out dt))
                    WriteDate = cirDocInfo.WriteTime.ToShortDateString() + " " + cirDocInfo.WriteTime.ToShortTimeString();
                else
                    WriteDate = string.Empty;
                #endregion

                // 제목
                DocSubject = cirDocInfo.Subject.ToNull();
            }

            // 부서, 직급 정보 가져오기
            Zeospace.DeskPlus.Common.DAL.DeptCode dalDept = new DeskPlus.Common.DAL.DeptCode();
            List<Zeospace.DeskPlus.Common.BE.DeptCodeInfo> alDeptInfo = dalDept.GetDepCodeLists(CurrentUserInfo.CompanyID, DeptCodeID);

            foreach (var deptInfo in alDeptInfo)
            {
                // 부서명
                DeptName = GetLangName(deptInfo.DeptBase,
                    deptInfo.DeptKo,
                    deptInfo.DeptEn,
                    deptInfo.DeptJa,
                    deptInfo.DeptFr,
                    deptInfo.DeptZhcn,
                    deptInfo.DeptZhtw, PreLang);

                // 직급명
                TitleName = GetLangName(deptInfo.TitleBase,
                    deptInfo.TitleKo,
                    deptInfo.TitleEn,
                    deptInfo.TitleJa,
                    deptInfo.TitleFr,
                    deptInfo.TitleZhcn,
                    deptInfo.TitleZhtw, PreLang);
            }

        }

        #endregion

        #region 함수

        /// <summary>
        /// 다국어
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                cbtnCancel.Text = resMng.GetString("Close");
                cbtnShowImage.Text = resMng.GetString("ShowImage");   // 이미지 보기
                cbtnZIP.Text = resMng.GetString("SelectDownLoadZIP");    // ZIP 선택다운로드
            }
        }

        /// <summary>
        /// 작성자 : 
        /// 작성일자 : 
        /// 작성목적 : 파라미터
        /// </summary>
        private void LoadParam()
        {
            CirDocumentID = GetDecryptParam("cirDocID", "").ToNumber<int>();
        }
        #endregion
    }
}