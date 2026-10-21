using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Account.BE;
using System.Drawing;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using System.Data;

namespace Zeospace.DeskPlus.Account.Web.MyInfo
{
    public partial class LinkageInfo : Base.BasePageAccount
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Account.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Account.Web.MyInfo.LinkageInfo).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                SetLanguage();
                LoadData();
            }
        }

        #region Method (함수)

        /// <summary>
        /// 작성목적   : 내용가져오기
        /// 작성자     : shine
        /// 최초작성일 : 2012-06-19
        /// </summary>
        private void LoadData()
        {
            LoadParam();

            DAL.UserMoney dal = new DAL.UserMoney();

            DataTableReader dtr = dal.GetUserMoneyInfo(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID);

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        ctbDeptName.Text = dtr["DeptName"].ToString().ToNull();
                        ctbBankNum.Text = dtr["BankNum"].ToString().ToNull();
                        ctbComPoint.Text = dtr["ComPoint"].ToString().ToNull();
                        ctbBankName.Text = dtr["BankName"].ToString().ToNull();
                        ctbKmsUserCode.Text = dtr["UserCode"].ToString().ToNull();
                        ctbKmsUserGrade.Text = dtr["UserGrade"].ToString().ToNull();
                        ctbJobCode.Text = dtr["Gcode"].ToString().ToNull();
                        ctbJobName.Text = dtr["Gname"].ToString().ToNull();
                        ctbCarName.Text = dtr["Cname"].ToString().ToNull();
                        ctbCarNumber.Text = dtr["Cnum"].ToString().ToNull();
                        ctbMoney.Text = dtr["Cmoney"].ToString().ToNull();
                        ctbAllCheck.Text = dtr["Hdaesang"].ToString().ToNull() == "1" ? "유" : "무";
                        ctbMPDS.Text = dtr["MPDS"].ToString().ToNull() == "1" ? "유" : "무";
                        ctbReason.Text = dtr["Reason"].ToString().ToNull();
                        ctbFName.Text = dtr["mbo1stname"].ToString().ToNull();
                        ctbSName.Text = dtr["mbo2ndname"].ToString().ToNull();
                        ctbFGubun.Text = dtr["1stPyMan"].ToString().ToNull() == "1" ? "유" : "무";
                        ctbSGubun.Text = dtr["2ndPyMan"].ToString().ToNull() == "1" ? "유" : "무";
                        ctbIDNumber.Text = dtr["IDNumber"].ToString().ToNull();
                        ctbPrivateMail.Text = dtr["PrivateMail"].ToString().ToNull();
                        ctbHeight.Text = dtr["Height"].ToString().ToNull();
                        ctbWeight.Text = dtr["Weight"].ToString().ToNull();
                        ctbSightL.Text = dtr["SightL"].ToString().ToNull();
                        ctbSightR.Text = dtr["SightR"].ToString().ToNull();
                        ctbBloodType.Text = dtr["BloodType"].ToString().ToNull();
                        ctbSkill.Text = dtr["Skill"].ToString().ToNull();
                        ctbHobby.Text = dtr["Hobby"].ToString().ToNull();
                        ctbReligion.Text = dtr["Religion"].ToString().ToNull();
                        ctbLivingType.Text = dtr["LivingType"].ToString().ToNull();
                        ctbOwnType.Text = dtr["OwnType"].ToString().ToNull();
                        ctbArmyType.Text = dtr["ArmyType"].ToString().ToNull() == "1" ? "유" : "무";
                        ctbArmyGubun.Text = dtr["ArmyGubun"].ToString().ToNull();
                        ctbArmyNumber.Text = dtr["ArmyNumber"].ToString().ToNull();
                        ctbArmyGroup.Text = dtr["ArmyGroup"].ToString().ToNull();
                        ctbArmyBranch.Text = dtr["ArmyBranch"].ToString().ToNull();
                        ctbArmyJoinDate.Text = dtr["ArmyJoinDate"].ToString().ToNull();
                        ctbArmyRetireDate.Text = dtr["ArmyRetireDate"].ToString().ToNull();
                        ctbClass.Text = dtr["Class"].ToString().ToNull();
                        ctbVeteranType.Text = dtr["VeteranType"].ToString().ToNull() == "1" ? "유" : "무";
                        ctbVeteranNumber.Text = dtr["VeteranNumber"].ToString().ToNull();
                        ctbObstacleType.Text = dtr["ObstacleType"].ToString().ToNull() == "1" ? "유" : "무";
                        ctbObstacleNumber.Text = dtr["ObstacleNumber"].ToString().ToNull();
                        ctbEtc.Text = dtr["Etc"].ToString().ToNull();

                        ctbEtcPlus1.Text = dtr["EtcPlus1"].ToString().ToNull();
                        ctbEtcPlus2.Text = dtr["EtcPlus2"].ToString().ToNull();
                        ctbEtcPlus3.Text = dtr["EtcPlus3"].ToString().ToNull();
                        ctbEtcPlus4.Text = dtr["EtcPlus4"].ToString().ToNull();
                        ctbEtcPlus5.Text = dtr["EtcPlus5"].ToString().ToNull();
                        ctbEtcPlus6.Text = dtr["EtcPlus6"].ToString().ToNull();
                        ctbEtcPlus7.Text = dtr["EtcPlus7"].ToString().ToNull();
                        ctbEtcPlus8.Text = dtr["EtcPlus8"].ToString().ToNull();
                        ctbEtcPlus9.Text = dtr["EtcPlus9"].ToString().ToNull();
                        ctbEtcPlus10.Text = dtr["EtcPlus10"].ToString().ToNull();
                    }
                }
            }
        }

        /// <summary>
        /// Loads the param.
        /// </summary>
        private void LoadParam()
        {
            
        }

        /// <summary>
        /// 작성목적   : 다국어
        /// 작성자     : shine
        /// 최초작성일 : 2012-06-19
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                cbtnConfirm.Text = resMng.GetString("Admin_CompanyManager_btnSubmit");
                cbtnUser.Text = resMng.GetString("Admin_UserManager_tabs0");                    // "기본정보";
            }
        }

        /// <summary>
        /// 작성목적   : 내용저장하기
        /// 작성자     : shine
        /// 최초작성일 : 2012-06-21
        /// </summary>
        /// <returns></returns>
        private bool Register()
        {
            UserMoneyInfo info = new UserMoneyInfo();

            info.CompanyID = CurrentUserInfo.CompanyID;
            info.UserID = CurrentUserInfo.UserID;
            info.DeptName = ctbDeptName.Text;
            info.BankNum = ctbBankNum.Text;
            info.BankName = ctbBankName.Text;
            info.ComPoint = ctbComPoint.Text;
            info.UserCode = ctbKmsUserCode.Text.ToNumber<int>();
            info.UserGrade = ctbKmsUserGrade.Text;
            info.Gcode = ctbJobCode.Text;
            info.Gname = ctbJobName.Text;
            info.Cname = ctbCarName.Text;
            info.Cnum = ctbCarNumber.Text;
            info.Cmoney = ctbMoney.Text;
            info.Hdaesang = ctbAllCheck.Text == "유" ? "1" : "0";
            info.MPDS = ctbMPDS.Text == "유" ? 1 : 0;
            info.Reason = ctbReason.Text;
            info.mbo1stname = ctbFName.Text;
            info.mbo2stname = ctbSName.Text;
            info.F1stPyMan = ctbFGubun.Text == "유" ? "1" : "0";
            info.S2ndPyMan = ctbSGubun.Text == "유" ? "1" : "0";
            info.IDNumber = ctbIDNumber.Text;
            info.PrivateMail = ctbPrivateMail.Text;
            info.Height = ctbHeight.Text;
            info.Weight = ctbWeight.Text;
            info.SightL = ctbSightL.Text;
            info.SightR = ctbSightR.Text;
            info.BloodType = ctbBloodType.Text;
            info.Skill = ctbSkill.Text;
            info.Hobby = ctbHobby.Text;
            info.Religion = ctbReligion.Text;
            info.LivingType = ctbLivingType.Text;
            info.OwnType = ctbOwnType.Text;
            info.ArmyType = ctbArmyType.Text == "유" ? "1" : "0";
            info.ArmyGubun = ctbArmyGubun.Text;
            info.ArmyNumber = ctbArmyNumber.Text;
            info.ArmyGroup = ctbArmyGroup.Text;
            info.ArmyBranch = ctbArmyBranch.Text;
            info.ArmyJoinDate = ctbArmyJoinDate.Text;
            info.ArmyRetireDate = ctbArmyRetireDate.Text;
            info.Class = ctbClass.Text;
            info.VeteranType = ctbVeteranType.Text == "유" ? "1" : "0";
            info.VeteranNumber = ctbVeteranNumber.Text;
            info.ObstacleType = ctbObstacleType.Text == "유" ? "1" : "0";
            info.ObstacleNumber = ctbObstacleNumber.Text;
            info.Etc = ctbEtc.Text;

            info.etcplus1 = ctbEtcPlus1.Text;
            info.etcplus2 = ctbEtcPlus2.Text;
            info.etcplus3 = ctbEtcPlus3.Text;
            info.etcplus4 = ctbEtcPlus4.Text;
            info.etcplus5 = ctbEtcPlus5.Text;
            info.etcplus6 = ctbEtcPlus6.Text;
            info.etcplus7 = ctbEtcPlus7.Text;
            info.etcplus8 = ctbEtcPlus8.Text;
            info.etcplus9 = ctbEtcPlus9.Text.ToNumber<int>();
            info.etcplus10 = ctbEtcPlus10.Text.ToNumber<int>();

            DAL.UserMoney dal = new DAL.UserMoney();
            return dal.Add(info);
        }
        #endregion

        #region Event Handler (이벤트 핸들러)

        /// <summary>
        /// 작성목적   : 내용저장하기
        /// 작성자     : shine
        /// 최초작성일 : 2012-06-21
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void lbtnRegister_Click(object sender, EventArgs e)
        {
            try
            {
                if (Register())
                {
                    LoadData();
                    Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "ok", GetResource("CompleteSave"), "");
                }
                else
                {
                    Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("lbtnRegister_Click");
                    Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "fail", GetResource("ProcessingError"), "");
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("lbtnRegister_Click", ex);
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "fail", GetResource("ProcessingError"), "");
            }

        }
        #endregion

        #region Property (프로퍼티)
        #endregion
    }
}