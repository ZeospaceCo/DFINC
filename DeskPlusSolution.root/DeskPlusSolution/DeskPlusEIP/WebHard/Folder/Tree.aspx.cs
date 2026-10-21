using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using System.Data;

namespace Zeospace.DeskPlus.WebHard.Web.Folder
{
    public partial class Tree : Base.BasePageWebHard
    {
        #region 변수설정 
        private string eventArgs = String.Empty;
        public string p_AccActive = "0"; //accordianActive

        public string UseCount = "0";// 개인이 사용한 용량
        public int PrivateQuota = 0;// 개인에게 할당된 용량
        public string PercentCount = "0";
        public string DisPlayUse = "0";
        public string DisPlayQuota = "0";

        public string PUseCount = "0";// 개인이 사용한 용량
        public int PPrivateQuota = 0;// 개인에게 할당된 용량
        public string PPercentCount = "0";
        public string PDisPlayUse = "0";
        public string PDisPlayQuota = "0";

        public string Manager = string.Empty;
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
            this.ResourceName = "Zeospace.DeskPlus.WebHard.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.WebHard.Web.Folder.Tree).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                LoadData();
            }
        }

        #endregion 

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        /// 작성자 : famo23
        /// 작성일자 : 2011-10-18
        /// 작성목적 : Loads the data.
        private void LoadData()
        { 
            LoadParam();

            //공용폴더
            PUseCount = "0";
            PPrivateQuota = 0;
            PDisPlayQuota = "∞";
            PDisPlayUse = "0 B";

            DAL.File dal = new DAL.File();
            DataSet ds = dal.GetPublicUseCount(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, 0, "U");

            if (ds != null)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    // 공용폴더 사용한 용량 가져오기
                    PUseCount = dr["FileLengthSum"].ToString();
                }
            }

            PDisPlayUse = FormatSize(Convert.ToDouble(PUseCount));

            //개인폴더
            //1024 * 1024 = 1048576
            // 개인이 사용한 용량 가져오기
            UseCount = dal.GetUseCount(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID);

            // 개인에게 할당된 용량 가져오기 (MB)
            PrivateQuota = CurrentUserInfo.PrivateQuota;
            DisPlayQuota = (PrivateQuota == 0) ? "∞" : PrivateQuota.ToString() + "MB";

            if (UseCount == "0")
            {
                PercentCount = "0";
            }
            else if (PrivateQuota == 0)
            {
                PercentCount = "0";
            }
            else
            {
                Double tmpPerCent = ((Convert.ToDouble(UseCount) / (Convert.ToDouble(PrivateQuota.ToString()) * 1048576)) * 100);

                if ((0 < tmpPerCent) && (tmpPerCent < 1))
                {
                    PercentCount = "0.1";
                }
                else
                {
                    PercentCount = tmpPerCent.ToString("#.#");
                }
            }

            if (Convert.ToDouble(PercentCount) > 100)
            {
                PercentCount = "100";
            }

            DisPlayUse = FormatSize(Convert.ToDouble(UseCount));
        }

        #endregion

        #region (함수)

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2011-10-18
        /// 작성목적 : 파라미터
        /// </summary>
        private void LoadParam()
        {
            p_AccActive = GetDecryptParam("accActive", "0");
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2011-10-18
        /// 작성목적 : 폴더 삭제 
        /// </summary>
        private void FolderDel()
        {            
            int FolderID = tbFID.Text.ToNumber<int>();

            if (FolderID > 0)
            {
                BLL.Folder bll = new BLL.Folder();
                if (bll.Del(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, FolderID))
                {
                    Dictionary<string, string> paraDic = new Dictionary<string, string>();
                    paraDic.Add("accActive", "1");

                    string encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));
                    string goPage = "Tree.aspx?p=" + encParams;
                    Response.Redirect(goPage, false);
                }
                else
                {
                    Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("WebHard.Folder.FolderDel()");
                    Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "fail", GetResource("ProcessingError"), "");
                }
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2011-12-26
        /// 작성목적 : 즐겨찾기 폴더 삭제 
        /// </summary>
        private void FavoriteDel()
        {
            int FavoriteID = tbFID.Text.ToNumber<int>();

            if (FavoriteID > 0)
            {
                DAL.Favorite dal = new DAL.Favorite();
                if (dal.Del(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, FavoriteID))
                {
                    Dictionary<string, string> paraDic = new Dictionary<string, string>();
                    paraDic.Add("accActive", "4");

                    string encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));
                    string goPage = "Tree.aspx?p=" + encParams;
                    Response.Redirect(goPage, false);
                }
                else
                {
                    Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("WebHard.Folder.FavoriteDel()");
                    Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "fail", GetResource("ProcessingError"), "");
                }
            }
        }

        /// <summary>
        /// 사이즈 처리
        /// </summary>
        /// <param name="fileSize"></param>
        /// <returns></returns>
        protected string FormatSize(double fileSize)
        {
            if (fileSize < 1024)
                return String.Format("{0:N0} B", fileSize);
            else if (fileSize < 1024 * 1024)
                return String.Format("{0:N1}KB", fileSize / 1024);
            else if (fileSize < 1024 * 1024 * 1024)
                return String.Format("{0:N1}MB", fileSize / (1024 * 1024));
            else
                return String.Format("{0:N1}GB", fileSize / (1024 * 1024 * 1024));
        }

        #endregion

        #region (이벤트핸들러)

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2011-10-18
        /// 작성목적 : 폴더 삭제
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LBTNProcess_Click(object sender, EventArgs e)
        {
            LoadParam();
            string args = eventArgs;

            switch (args)
            {
                case "P":               // 개인폴더
                    FolderDel();
                    break;
                case "Favorite":        // 즐겨찾기폴더
                    FavoriteDel();
                    break;
            }
                 
        }

        /// <summary>
        /// 들어오는 포스트백 이벤트를 처리하도록 포스트백을 발생시킨 서버 컨트롤에 알립니다.
        /// </summary>
        /// <param name="sourceControl">포스트백을 발생시킨 ASP.NET 서버 컨트롤입니다.이 컨트롤은 <see cref="T:System.Web.UI.IPostBackEventHandler"/> 인터페이스를 구현해야 합니다.</param>
        /// <param name="eventArgument">포스트백 인수입니다.</param>
        protected override void RaisePostBackEvent(IPostBackEventHandler sourceControl, string eventArgument)
        {
            eventArgs = eventArgument;
            base.RaisePostBackEvent(sourceControl, eventArgument);
        }
        #endregion
    }
}