using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Data;
using System.Text;

namespace Zeospace.DeskPlus.Approval.Web.Admin.FormGeneration
{
    public partial class ModUserSequence : Base.BasePageApprovalAdmin
    {
        public string FolderList = "";
        public int CompanyID;           //회사ID
        public int FormID;              //ID
        public string AppointerKind;
        public string DeptID;
        public string DeptName;
        public string UserID;
        public string UserName;
        public string IsDept;
        public string IsActive;

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Approval.Web.Resource.GlobalString.string";   //리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.Admin.FormGeneration.ModUserSequence).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                LoadParm();
                SetLanguage();
                LoadData();
            }
        }

        #region (함수)
        /// <summary>
        /// 파라미터처리
        /// </summary>
        private void LoadParm()
        {
            CompanyID = GetDecryptParam("companyID", "0").ToNumber<int>();  //화사ID
            FormID = GetDecryptParam("formID", "0").ToNumber<int>();        //ID

            AppointerKind = GetDecryptParam("appointerKind", "");
            DeptID = GetDecryptParam("deptID", "");
            DeptName = GetDecryptParam("deptName", "");
            UserID = GetDecryptParam("userID", "");
            UserName = GetDecryptParam("userName", "");
            IsDept = GetDecryptParam("isDept", "");
            IsActive = GetDecryptParam("isActive", "");
        }
        /// <summary>
        /// 다국어 설정
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                cbtnConfirm.Text = resMng.GetString("Confirm");
                cbtnClose.Text = resMng.GetString("Close");
            }
        }

        /// <summary>
        /// 데이터가져오기
        /// </summary>
        private void LoadData()
        {
            StringBuilder sb = new StringBuilder();

            int cnt = 0;
            if (!DeptID.Length.Equals(0))
            {
                string[] _AppointerKind = AppointerKind.Split(',');
                string[] _DeptID = DeptID.Split(',');
                string[] _DeptName = DeptName.Split(',');
                string[] _UserID = UserID.Split(',');
                string[] _UserName = UserName.Split(',');
                string[] _IsDept = IsDept.Split(',');
                string[] _IsActive = IsActive.Split(',');
                

                for (int i = 0; i < _DeptID.Length; i++)
                {
                    string appointerKindName = "";

                    cnt++;

                    if (_AppointerKind[i] == "ZC")
                    {
                        appointerKindName = resMng.GetString("Carboncopy");
                    }
                    else
                    {
                        appointerKindName = resMng.GetString("Receive");
                    }

                    sb.Append("<li id='");
                    sb.Append(_AppointerKind[i] + "_" + _DeptID[i] + "_" + _DeptName[i] + "_" + _UserID[i] + "_" + _UserName[i] + "_" + _IsDept[i] + "_" + _IsActive[i]);
                    sb.Append("'>");
                    sb.Append(cnt.ToString());
                    sb.Append(". [");
                    sb.Append(appointerKindName);
                    sb.Append("] ");
                    sb.Append(_DeptName[i] + " " + _UserName[i]);
                    sb.Append("</li>");
                }

                FolderList = sb.ToString();
            }
        }
        #endregion
    }
}