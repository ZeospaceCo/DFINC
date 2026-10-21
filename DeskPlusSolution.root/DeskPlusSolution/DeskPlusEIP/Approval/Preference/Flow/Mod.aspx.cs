using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Approval.BE;
using Zeospace.DeskPlus.Approval.DAL;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Approval.Web.Preference.Flow
{
    public partial class Mod : Base.BasePageApproval
    {
        #region Declaration (컨트롤 선언)
        #endregion

        #region 변수설정
        public string EventArgs = string.Empty;
        public int FavSeqID = 0;
        #endregion

        #region Page Process (페이지 처리)
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Approval.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.Preference.Flow.Mod).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                LoadParam();
                SetLanguage();
                LoadData(FavSeqID);
            }
        }
        #endregion

        #region Method (함수)
        private void LoadParam()
        {
            FavSeqID = GetDecryptParam("favSeqID", "0").ToNumber<int>();
            //DeptNm = GetDecryptParam("deptNm", "");
        }

        /// <summary>
        /// 다국어 설정
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
            }
        }

        /// <summary>
        /// 결재선정보 가져오기
        /// </summary>
        private void LoadData(int _favSeqID)
        {
            if (_favSeqID < 1) return;

            DAL.FavSeq fDal = new FavSeq();
            List<BE.FavSeqInfo> list = fDal.GetFavSeqInfo(CurrentUserInfo.CompanyID, _favSeqID);
            if (list != null)
            {
                foreach (BE.FavSeqInfo info in list)
                {
                    ctbSubject.Text = info.Subject.ToReverseXSS();
                    ctbMemo.Text = info.Memo.ToReverseXSS();
                }
            }
        }

        #endregion

        #region Event Handler (이벤트 핸들러)

        /// <summary>
        /// 들어오는 포스트백 이벤트를 처리하도록 포스트백을 발생시킨 서버 컨트롤에 알립니다.
        /// </summary>
        /// <param name="sourceControl">포스트백을 발생시킨 ASP.NET 서버 컨트롤입니다.이 컨트롤은 <see cref="T:System.Web.UI.IPostBackEventHandler"/> 인터페이스를 구현해야 합니다.</param>
        /// <param name="eventArgument">포스트백 인수입니다.</param>
        protected override void RaisePostBackEvent(IPostBackEventHandler sourceControl, string eventArgument)
        {
            LoadParam(); //파라미터처리

            EventArgs = eventArgument;
            base.RaisePostBackEvent(sourceControl, eventArgument);
        }

        #endregion

        #region Property (프로퍼티)

        #endregion
    }
}