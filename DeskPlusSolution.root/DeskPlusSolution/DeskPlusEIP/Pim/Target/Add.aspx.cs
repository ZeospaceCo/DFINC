using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.Pim.Web.Target
{
    
    public partial class Add  : Base.BasePagePim
    { 
        #region Declaration (컨트롤 선언)
        #endregion

        #region 변수설정        
        public int CompanyID = 0;
        public int UserID = 0;
        
        #endregion

        #region Page Process (페이지 처리)

        protected void Page_Load(object sender, EventArgs e)
        {

            CompanyID = CurrentUserInfo.CompanyID;
            UserID = CurrentUserInfo.UserID;

            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Pim.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Pim.Web.Target.Add).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                SwfUploadGUID = System.Guid.NewGuid().ToString();
                SetLanguage();
                LoadData();
                startDate.Value = DateTime.Now.ToString("yyyy-MM-dd");
                endDate.Value = DateTime.Now.ToString("yyyy-MM-dd");
            }
        }


        #endregion

        #region Method (함수)

        

        /// <summary>
        /// 작성목적   : 초기화 데이터 설정
        /// 작성자     : 
        /// </summary>
        private void LoadData()
        {
            LoadParam();

            DAL.MBO biz = new DAL.MBO();
            //상위목표
            //상위기간


        }


        /// <summary>
        /// 작성목적   : 파라미터 설정
        /// 작성자     : 
        /// </summary>
        private void LoadParam()
        {
            //CatID = GetDecryptParam("catID", "0").ToNumber<int>();
          
        }

        /// <summary>
        /// 작성목적   : 다국어 설정
        /// 작성자     : 
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                CbtnAdd.Text = GetResource("Save");
                CbtnCancel.Text = GetResource("Cancel");
            }
        }



        private void AddMBO()
        {
            BE.MBOInfo _info = new BE.MBOInfo();
            DAL.MBO biz = new DAL.MBO();

            //제목 
            _info.CompanyID = CurrentUserInfo.CompanyID;
            _info.ParentMBOID = 0;
            _info.MBOGroup = 0;
            _info.MBOOrder = 0;
            _info.Dept = 1;
            _info.Title = txttitle.Value.Trim();
            //시작일
            _info.StartDate = DateTime.Parse(startDate.Value);
            _info.EndDate = DateTime.Parse(endDate.Value);
            _info.ToDoListYN = "N";
            _info.UserID = CurrentUserInfo.UserID;           

            int mboid= biz.AddMBO(_info);

            //첨부파일
            string filePath = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
            filePath = filePath + "{0}/Users/{1}/Spool/{2}/";
            string strPath = string.Format(filePath, CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, SwfUploadGUID);

              
            
            DAL.MBOAttachFiles dalatta = new DAL.MBOAttachFiles();
            dalatta.AddList(CurrentUserInfo.CompanyID, mboid, strPath);
            
            
            //공유자

            string strTemp = hdShareList.Text.Trim();
            BE.MBOShareUserInfo infouser = new BE.MBOShareUserInfo();
            BE.MBOShareDeptInfo infodept = new BE.MBOShareDeptInfo();
            DAL.MBOShareUser bizshare = new DAL.MBOShareUser();
            DAL.MBOShareDept bizdept = new DAL.MBOShareDept();

            infodept.CompanyID = CurrentUserInfo.CompanyID;
            infodept.MBOID = mboid;
            
            infouser.CompanyID = CurrentUserInfo.CompanyID;
            infouser.MBOID = mboid;


            if (strTemp.Length > 0)
            {
                string[] arVal = strTemp.Split(',');
                foreach (string _be in arVal)
                {
                    string[] arSource = _be.Split('$');

                    if (arSource[0].Equals("G"))
                    {
                        //그룹
                        infodept.DeptID = int.Parse(arSource[1]);
                        infodept.DeptName = arSource[2];
                        infodept.TypeTargetNum = 0;
                
                        bizdept.AddMBOShareDeft(infodept);
                    }
                    else if (arSource[0].Equals("M"))
                    {
                        //유저공유
                        infouser.TypeTargetNum = 0;
                        infouser.TargetUserID = int.Parse(arSource[1]);
                        infouser.UserMail = "";
                        infouser.UserName = arSource[2];
                        bizshare.AddMBOShareUser(infouser);
                    }
                    
                }

            }

            Dictionary<string, string> paraDic = new Dictionary<string, string>();
            paraDic.Add("alertType", AlertMsgEnum.Alert_Register.ToString());
            paraDic.Add("alertMsg", GetResource("GoToList"));
            paraDic.Add("goURL", "/DeskPlusEIP/Pim/Target/List.aspx");
            string encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));
            Utility.Redirect(encParams);      
        }

        #endregion

        #region Event Handler (이벤트 핸들러)
        protected void lbtnWrite_Click(object sender, EventArgs e)
        {
            try
            { 
                AddMBO();
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("lbtnWrite_Click", ex);
            }
        }
        #endregion

        #region Property (프로퍼티)

        /// <summary>
        /// SwfUploadGUID
        /// </summary>
        public string SwfUploadGUID
        {
            get { return GetViewStateString("SwfUploadGUID", ""); }
            set { ViewState["SwfUploadGUID"] = value; }
        }

        /// <summary>
        /// Gets or sets the upload path.
        /// </summary>
        /// <value>
        /// The upload path.
        /// </value>
        public string UploadPath
        {
            get { return GetViewStateString("UploadPath", ""); }
            set { ViewState["UploadPath"] = value; }
        }

        #endregion
    }        
  
}