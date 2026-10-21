using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Account.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;

namespace Zeospace.DeskPlus.Account.Web.MyInfo
{
    public partial class Alarm : Base.BasePageAccount
    {
        #region 변수설정

        public int CompanyID = 0;
        public int UserID = 0;
        public string IsAdmin = "0";

        #endregion

        #region Page Process (페이지 처리)
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Account.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Account.Web.MyInfo.Alarm).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                SetLanguage();
                LoadData();
            }
        }
        #endregion

        #region Method (함수)
        private void LoadParam()
        {
            CompanyID = GetDecryptParam("cid", CurrentUserInfo.CompanyID.ToString()).ToNumber<int>();
            UserID = GetDecryptParam("uid", CurrentUserInfo.UserID.ToString()).ToNumber<int>();
            IsAdmin = GetDecryptParam("isadmin", "0");
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2015-10-07
        /// </summary>
        private void LoadData()
        {
            LoadParam();

            //항목바인딩
            BindItem();

            //개인설정정보
            GetUserEnvInfo(CompanyID, UserID);

            //사용자 or 관리자 호출
            cbtnUser.Visible = (IsAdmin.Equals("0")) ? true : false;
        }

        /// <summary>
        /// 다국어 설정
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                cbtnSave.Text = resMng.GetString("BtnSave");
                cbtnList.Text = resMng.GetString("BtnClose");
                cbtnUser.Text = resMng.GetString("Admin_UserManager_tabs0");

                ddlAlarmApproval.Items.Clear();
                ddlAlarmApproval.Items.Add(new ListItem(resMng.GetString("AlarmIsUsing1"), "1"));
                ddlAlarmApproval.Items.Add(new ListItem(resMng.GetString("AlarmIsUsing0"), "0"));
                
                ddlAlarmBoard.Items.Clear();
                ddlAlarmBoard.Items.Add(new ListItem(resMng.GetString("AlarmIsUsing1"), "1"));
                ddlAlarmBoard.Items.Add(new ListItem(resMng.GetString("AlarmIsUsing0"), "0"));

                ddlAlarmPims.Items.Clear();
                ddlAlarmPims.Items.Add(new ListItem(resMng.GetString("AlarmIsUsing1"), "1"));
                ddlAlarmPims.Items.Add(new ListItem(resMng.GetString("AlarmIsUsing0"), "0"));

                ddlAlarmTask.Items.Clear();
                ddlAlarmTask.Items.Add(new ListItem(resMng.GetString("AlarmIsUsing1"), "1"));
                ddlAlarmTask.Items.Add(new ListItem(resMng.GetString("AlarmIsUsing0"), "0"));

                ddlAlarmWork.Items.Clear();
                ddlAlarmWork.Items.Add(new ListItem(resMng.GetString("AlarmIsUsing1"), "1"));
                ddlAlarmWork.Items.Add(new ListItem(resMng.GetString("AlarmIsUsing0"), "0"));

                ddlAlarmCommunity.Items.Clear();
                ddlAlarmCommunity.Items.Add(new ListItem(resMng.GetString("AlarmIsUsing1"), "1"));
                ddlAlarmCommunity.Items.Add(new ListItem(resMng.GetString("AlarmIsUsing0"), "0"));

                ddlAlarmResource.Items.Clear();
                ddlAlarmResource.Items.Add(new ListItem(resMng.GetString("AlarmIsUsing1"), "1"));
                ddlAlarmResource.Items.Add(new ListItem(resMng.GetString("AlarmIsUsing0"), "0"));

                ddlAlarmSNS.Items.Clear();
                ddlAlarmSNS.Items.Add(new ListItem(resMng.GetString("AlarmIsUsing1"), "1"));
                ddlAlarmSNS.Items.Add(new ListItem(resMng.GetString("AlarmIsUsing0"), "0"));
            }
        }

        private void BindItem()
        {
            string name = "";

            #region 전자결재

            DataView dvApp = AppSettingHelper.GetSysCode(CompanyID, "AlarmApproval");
            cbAlarmApproval.Items.Clear(); ;
            cbAlarmApproval.AppendDataBoundItems = true;

            foreach (DataRowView dr in dvApp)
            {
                name = this.GetLangName(
                    dr["NameBase"].ToString(),
                    dr["NameKo"].ToString(),dr["NameEn"].ToString(), dr["NameJa"].ToString(),dr["NameFr"].ToString(),
                    dr["NameZhCn"].ToString(),dr["NameZhTw"].ToString(), PreLang);

                cbAlarmApproval.Items.Add(new ListItem (name, dr["CodeID"].ToString()));
            }

            #endregion

            #region 게시판

            DataView dvBoa = AppSettingHelper.GetSysCode(CompanyID, "AlarmBoard");
            cbAlarmBoard.Items.Clear();
            cbAlarmBoard.AppendDataBoundItems = true;

            foreach (DataRowView dr in dvBoa)
            {
                if (dr["CodeID"].ToString() != "MOBILE")
                {
                    name = this.GetLangName(
                        dr["NameBase"].ToString(),
                        dr["NameKo"].ToString(), dr["NameEn"].ToString(), dr["NameJa"].ToString(), dr["NameFr"].ToString(),
                        dr["NameZhCn"].ToString(), dr["NameZhTw"].ToString(), PreLang);

                    cbAlarmBoard.Items.Add(new ListItem(name, dr["CodeID"].ToString()));
                }
            }

            #endregion

            #region 일정관리

            DataView dvPims = AppSettingHelper.GetSysCode(CompanyID, "AlarmPims");
            cbAlarmPims.Items.Clear();
            cbAlarmPims.AppendDataBoundItems = true;

            foreach (DataRowView dr in dvPims)
            {
                if (dr["CodeID"].ToString() != "MOBILE")
                {
                    name = this.GetLangName(
                        dr["NameBase"].ToString(),
                        dr["NameKo"].ToString(), dr["NameEn"].ToString(), dr["NameJa"].ToString(), dr["NameFr"].ToString(),
                        dr["NameZhCn"].ToString(), dr["NameZhTw"].ToString(), PreLang);

                    cbAlarmPims.Items.Add(new ListItem(name, dr["CodeID"].ToString()));
                }
            }

            #endregion

            #region 업무협업

            DataView dvTask = AppSettingHelper.GetSysCode(CompanyID, "AlarmTask");
            cbAlarmTask.Items.Clear();
            cbAlarmTask.AppendDataBoundItems = true;

            foreach (DataRowView dr in dvTask)
            {
                name = this.GetLangName(
                    dr["NameBase"].ToString(),
                    dr["NameKo"].ToString(), dr["NameEn"].ToString(), dr["NameJa"].ToString(), dr["NameFr"].ToString(),
                    dr["NameZhCn"].ToString(), dr["NameZhTw"].ToString(), PreLang);

                cbAlarmTask.Items.Add(new ListItem(name, dr["CodeID"].ToString()));
            }

            #endregion

            #region 업무일지

            DataView dvWork = AppSettingHelper.GetSysCode(CompanyID, "AlarmWork");
            cbAlarmWork.Items.Clear();
            cbAlarmWork.AppendDataBoundItems = true;

            foreach (DataRowView dr in dvWork)
            {
                name = this.GetLangName(
                    dr["NameBase"].ToString(),
                    dr["NameKo"].ToString(), dr["NameEn"].ToString(), dr["NameJa"].ToString(), dr["NameFr"].ToString(),
                    dr["NameZhCn"].ToString(), dr["NameZhTw"].ToString(), PreLang);

                cbAlarmWork.Items.Add(new ListItem(name, dr["CodeID"].ToString()));
            }

            #endregion

            #region 커뮤니티

            DataView dvComm = AppSettingHelper.GetSysCode(CompanyID, "AlarmCommunity");
            cbAlarmCommunity.Items.Clear();
            cbAlarmCommunity.AppendDataBoundItems = true;

            foreach (DataRowView dr in dvComm)
            {
                name = this.GetLangName(
                    dr["NameBase"].ToString(),
                    dr["NameKo"].ToString(), dr["NameEn"].ToString(), dr["NameJa"].ToString(), dr["NameFr"].ToString(),
                    dr["NameZhCn"].ToString(), dr["NameZhTw"].ToString(), PreLang);

                cbAlarmCommunity.Items.Add(new ListItem(name, dr["CodeID"].ToString()));
            }

            #endregion

            #region 자원관리

            DataView dvRM = AppSettingHelper.GetSysCode(CompanyID, "AlarmResource");
            cbAlarmResource.Items.Clear();
            cbAlarmResource.AppendDataBoundItems = true;

            foreach (DataRowView dr in dvRM)
            {
                name = this.GetLangName(
                    dr["NameBase"].ToString(),
                    dr["NameKo"].ToString(), dr["NameEn"].ToString(), dr["NameJa"].ToString(), dr["NameFr"].ToString(),
                    dr["NameZhCn"].ToString(), dr["NameZhTw"].ToString(), PreLang);

                cbAlarmResource.Items.Add(new ListItem(name, dr["CodeID"].ToString()));
            }

            #endregion

            #region 사내SNS

            DataView dvSNS = AppSettingHelper.GetSysCode(CompanyID, "AlarmSNS");
            cbAlarmSNS.Items.Clear();
            cbAlarmSNS.AppendDataBoundItems = true;

            foreach (DataRowView dr in dvSNS)
            {
                name = this.GetLangName(
                    dr["NameBase"].ToString(),
                    dr["NameKo"].ToString(), dr["NameEn"].ToString(), dr["NameJa"].ToString(), dr["NameFr"].ToString(),
                    dr["NameZhCn"].ToString(), dr["NameZhTw"].ToString(), PreLang);

                cbAlarmSNS.Items.Add(new ListItem(name, dr["CodeID"].ToString()));
            }

            #endregion

        }

        private void GetUserEnvInfo(int _companyID, int _userID)
        {
            Zeospace.DeskPlus.Account.DAL.UserEnvironment dal = new DAL.UserEnvironment();

            List<BE.UserEnvironmentInfo> list = dal.GetUserEnv(_companyID, _userID);
            if (list != null)
            {
                foreach (BE.UserEnvironmentInfo info in list)
                {
                    // 전자결재
                    ddlAlarmApproval.SelectedValue = info.AlarmApproval.Split('-')[0];
                    if (ddlAlarmApproval.SelectedValue.Equals("1"))
                    {
                        foreach (string s in info.AlarmApproval.Split('-')[1].TrimEnd(',').Split(','))
                        {
                            if (cbAlarmApproval.Items.FindByValue(s) != null)
                            {
                                cbAlarmApproval.Items.FindByValue(s).Selected = true;
                            }
                        }
                    }

                    //게시판
                    ddlAlarmBoard.SelectedValue = info.AlarmBoard.Split('-')[0];
                    if (ddlAlarmBoard.SelectedValue.Equals("1"))
                    {
                        foreach (string s in info.AlarmBoard.Split('-')[1].TrimEnd(',').Split(','))
                        {
                            if (cbAlarmBoard.Items.FindByValue(s) != null)
                            {
                                cbAlarmBoard.Items.FindByValue(s).Selected = true;
                            }
                        }
                    }

                    //일정관리
                    ddlAlarmPims.SelectedValue = info.AlarmPims.Split('-')[0];
                    if (ddlAlarmPims.SelectedValue.Equals("1"))
                    {
                        foreach (string s in info.AlarmPims.Split('-')[1].TrimEnd(',').Split(','))
                        {
                            if (cbAlarmPims.Items.FindByValue(s) != null)
                            {
                                cbAlarmPims.Items.FindByValue(s).Selected = true;
                            }
                        }
                    }

                    //업무협업
                    ddlAlarmTask.SelectedValue = info.AlarmTask.Split('-')[0];
                    if (ddlAlarmTask.SelectedValue.Equals("1"))
                    {
                        foreach (string s in info.AlarmTask.Split('-')[1].TrimEnd(',').Split(','))
                        {
                            if (cbAlarmTask.Items.FindByValue(s) != null)
                            {
                                cbAlarmTask.Items.FindByValue(s).Selected = true;
                            }
                        }
                    }

                    //업무일지
                    ddlAlarmWork.SelectedValue = info.AlarmWork.Split('-')[0];
                    if (ddlAlarmWork.SelectedValue.Equals("1"))
                    {
                        foreach (string s in info.AlarmWork.Split('-')[1].TrimEnd(',').Split(','))
                        {
                            if (cbAlarmWork.Items.FindByValue(s) != null)
                            {
                                cbAlarmWork.Items.FindByValue(s).Selected = true;
                            }
                        }
                    }

                    //커뮤니티
                    ddlAlarmCommunity.SelectedValue = info.AlarmCommunity.Split('-')[0];
                    if (ddlAlarmCommunity.SelectedValue.Equals("1"))
                    {
                        foreach (string s in info.AlarmCommunity.Split('-')[1].TrimEnd(',').Split(','))
                        {
                            if (cbAlarmCommunity.Items.FindByValue(s) != null)
                            {
                                cbAlarmCommunity.Items.FindByValue(s).Selected = true;
                            }
                        }
                    }

                    //자원관리
                    ddlAlarmResource.SelectedValue = info.AlarmRM.Split('-')[0];
                    if (ddlAlarmResource.SelectedValue.Equals("1"))
                    {
                        foreach (string s in info.AlarmRM.Split('-')[1].TrimEnd(',').Split(','))
                        {
                            if (cbAlarmResource.Items.FindByValue(s) != null)
                            {
                                cbAlarmResource.Items.FindByValue(s).Selected = true;
                            }
                        }
                    }

                    //사내SNS
                    ddlAlarmSNS.SelectedValue = info.AlarmSNS.Split('-')[0];
                    if (ddlAlarmSNS.SelectedValue.Equals("1"))
                    {
                        foreach (string s in info.AlarmSNS.Split('-')[1].TrimEnd(',').Split(','))
                        {
                            if (cbAlarmSNS.Items.FindByValue(s) != null)
                            {
                                cbAlarmSNS.Items.FindByValue(s).Selected = true;
                            }
                        }
                    }
                }
            }
        }
        #endregion

        #region Event Handler (이벤트 핸들러)
        #endregion Event Handler (이벤트 핸들러)

        #region Property (프로퍼티)
        #endregion Property (프로퍼티)
    }
}