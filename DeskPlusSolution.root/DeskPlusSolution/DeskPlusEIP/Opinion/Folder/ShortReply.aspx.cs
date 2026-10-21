using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Data;
using System.Text;
using Zeospace.DeskPlus.Opinion.BE;

namespace Zeospace.DeskPlus.Opinion.Web.Folder
{
    public partial class ShortReply : Base.BasePageOpinion
    {
        #region Declaration (컨트롤 선언)

        #endregion

        #region 변수설정
        public int QuesID;
        public int ReplyID;
        #endregion

        #region Page Process (페이지 처리)
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Opinion.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Opinion.Web.Folder.ShortReply).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                LoadData();
            }

        }
        #endregion

        #region Method (함수)
        private void LoadData()
        {
            LoadParam();
            SetLanguage();

            if (QuesID > 0 && ReplyID > 0) LoadReply(QuesID, ReplyID);
        }

        /// <summary>
        /// 파라미터
        /// </summary>
        private void LoadParam()
        {
            QuesID = GetDecryptParam("quesID", "0").ToNumber<int>();
            ReplyID = GetDecryptParam("replyID", "0").ToNumber<int>();
        }

        /// <summary>
        /// 다국어 설정
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                CbtnConfirm.Text = this.GetResource("BtnConfirm");
                CbtnClose.Text = this.GetResource("BtnClose");
            }
        }

        /// <summary>
        /// 답글정보가져오기
        /// </summary>
        /// <param name="_quesID"></param>
        /// <param name="_replyID"></param>
        private void LoadReply(int _quesID, int _replyID)
        {
            if (_quesID.Equals(0) || _replyID.Equals(0)) return;

            DAL.Reply rDAL = new DAL.Reply();
            List<ReplyInfo> rList = rDAL.GetReply(CurrentUserInfo.CompanyID, _quesID, _replyID);

            foreach (ReplyInfo rInfo in rList)
            {
                this.txtCont.Value = rInfo.Cont;
            }
        }
        #endregion

        #region Event Handler (이벤트 핸들러)

        #endregion

        #region Property (프로퍼티)

        #endregion
    }
}