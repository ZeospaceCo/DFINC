using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Text.RegularExpressions;
using System.Text;
using System.IO;
using Helpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using System.Xml;
using System.Collections.Specialized;

namespace Zeospace.DeskPlus.Pim.Web.ToDo
{
    public partial class AddSimple : Base.BasePagePim
    {
        #region 변수설정
        public int TodoID;
        public int CompanyID;
        public int UserID;
        public int CurrentUserID;
        
        #endregion

        #region 페이지처리
        protected void Page_Load(object sender, EventArgs e)
        {
            CompanyID = CurrentUserInfo.CompanyID;
            UserID = CurrentUserInfo.UserID;

            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Pim.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Pim.Web.ToDo.AddSimple).Assembly;	//어셈블리 선언
            #endregion


            if (!IsPostBack)
            {
                SetLanguage();
                SettingControl();
                LoadData();
            }
        }
        #endregion

        #region 함수
        /// <summary>
        /// 다국어 설정
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                CbtnMod.Text = GetResource("DETAIL");
                CbtnConfirm.Text = GetResource("Save");
                CbtnClose.Text = GetResource("Close");
            }
        }

        /// <summary>
        /// 콘트롤 바인딩
        /// </summary>
        private void SettingControl()
        {
            ddlPriorty.Items.Clear();
            ddlDate.Items.Clear();
            ddltodo.Items.Clear();

            //일정
            //할일
            ListItem ltodo1 = new ListItem(GetResource("TOP"), "1");
            ListItem ltodo2 = new ListItem(GetResource("MIDDLE"), "2");
            ListItem ltodo3 = new ListItem(GetResource("BOTTOM"), "3");

            ddltodo.Items.Add(ltodo1);
            ddltodo.Items.Add(ltodo2);
            ddltodo.Items.Add(ltodo3);
            //진척도
            //ddlPriorty
            //W I E C T D
            ListItem ltpro1 = new ListItem(GetResource("priority1"), "W");
            ListItem ltpro2 = new ListItem(GetResource("priority2"), "I");
            ListItem ltpro3 = new ListItem(GetResource("priority3"), "E");
            ListItem ltpro4 = new ListItem(GetResource("priority4"), "C");
            ListItem ltpro5 = new ListItem(GetResource("priority5"), "T");
            ListItem ltpro6 = new ListItem(GetResource("priority6"), "D");
            ddlPriorty.Items.Add(ltpro1);
            ddlPriorty.Items.Add(ltpro2);
            ddlPriorty.Items.Add(ltpro3);
            ddlPriorty.Items.Add(ltpro4);
            ddlPriorty.Items.Add(ltpro5);
            ddlPriorty.Items.Add(ltpro6);

            ListItem lttod1 = new ListItem(GetResource("todo1"), "1");
            ListItem lttod2 = new ListItem(GetResource("todo2"), "2");
            ListItem lttod3 = new ListItem(GetResource("todo3"), "3");
            ListItem lttod4 = new ListItem(GetResource("todo4"), "4");
            ListItem lttod5 = new ListItem(GetResource("todo5"), "5");
            ListItem lttod6 = new ListItem(GetResource("todo6"), "0");
            ddlDate.Items.Add(lttod6);
            ddlDate.Items.Add(lttod1);
            ddlDate.Items.Add(lttod2);
            ddlDate.Items.Add(lttod3);
            ddlDate.Items.Add(lttod4);
            ddlDate.Items.Add(lttod5);

            ListItem ltcate1 = new ListItem(GetResource("TodoShare1"), "0");
            ListItem ltcate2 = new ListItem(GetResource("TodoShare2"), "3");
            ListItem ltcate3 = new ListItem(GetResource("TodoShare3"), "1");
            ListItem ltcate4 = new ListItem(GetResource("TodoShare4"), "2");

            ddlShare.Items.Add(ltcate1);
            ddlShare.Items.Add(ltcate2);
            ddlShare.Items.Add(ltcate3);
            ddlShare.Items.Add(ltcate4);
            
        }

         /// <summary>
        /// 날짜 변환
        /// </summary>
        /// <param name="_date"></param>
        /// <returns></returns>
        private string SetDate(string _date)
        {
            string tmpVal = String.Empty;
            DateTime dt = Convert.ToDateTime(_date);
            switch (CurrentUserInfo.PreferredLanguage)
            {
                case "ko":
                    tmpVal = dt.Year.ToString() + "-" + dt.Month.ToString().PadLeft(2, '0') + "-" + dt.Day.ToString().PadLeft(2, '0');
                    break;
                case "en":
                    tmpVal = dt.Month.ToString().PadLeft(2, '0') + "/" + dt.Day.ToString().PadLeft(2, '0') + "/" + dt.Year.ToString();
                    break;
                case "en-gb":
                    tmpVal = dt.Day.ToString().PadLeft(2, '0') + "/" + dt.Month.ToString().PadLeft(2, '0') + "/" + dt.Year.ToString();
                    break;
                case "ja":
                    tmpVal = dt.Year.ToString() + "/" + dt.Month.ToString().PadLeft(2, '0') + "/" + dt.Day.ToString().PadLeft(2, '0');
                    break;
                case "fr":
                    tmpVal = dt.Day.ToString().PadLeft(2, '0') + "/" + dt.Month.ToString().PadLeft(2, '0') + "/" + dt.Year.ToString();
                    break;
                case "zh-cn":
                    tmpVal = dt.Year.ToString() + "/" + dt.Month.ToString().PadLeft(2, '0') + "/" + dt.Day.ToString().PadLeft(2, '0');
                    break;
                case "zh-tw":
                    tmpVal = dt.Year.ToString() + "-" + dt.Month.ToString().PadLeft(2, '0') + "-" + dt.Day.ToString().PadLeft(2, '0');
                    break;
            }


            return tmpVal;
        }

        private void LoadData()
        {
            LoadParam();
            DAL.ToDo dal = new DAL.ToDo();
            DataTableReader dtr = dal.GetTodoID(CompanyID, TodoID);

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        //제목
                        txtTitle.Value = dtr["Title"].ToString().ToReverseXSS();
                        lblTitle.Text = dtr["Title"].ToString().ToReverseXSS();
                        //중요도
                        ddltodo.SelectedValue = dtr["Priority"].ToString();
                        lbltodo.Text = ddltodo.SelectedItem.Text;
                        //직척도
                        ddlPriorty.SelectedValue = dtr["Status"].ToString();
                        lblPriorty.Text = ddlPriorty.SelectedItem.Text;
                        //일정
                        txtDate.Value = SetDate(dtr["StartDate"].ToString());                       
                        //게시범위
                        ddlShare.SelectedValue = dtr["OpenType"].ToString();
                        lblShare.Text = ddlShare.SelectedItem.Text;
                        //등록자
                        lblName.Text = GetLangName(dtr["NameBase"].ToString(), dtr["NameKo"].ToString(), dtr["NameEn"].ToString()
                            , dtr["NameJa"].ToString(), dtr["NameFr"].ToString(), dtr["NameZhcn"].ToString(), dtr["NameZhtw"].ToString(), PreLang);

                        if (CurrentUserInfo.UserID != dtr["UserID"].ToString().ToNumber<int>())
                        {
                            lblHearder.Text = GetResource("TODOVIEW");
                            tMe.Visible = false;
                            tShare.Visible = true;
                            CbtnConfirm.Visible = false;
                        }
                        else
                        {
                            lblHearder.Text = GetResource("ModToDo");
                            tMe.Visible = true;
                            tShare.Visible = false;
                        }
                        CurrentUserID = dtr["UserID"].ToString().ToNumber<int>();
                    }
                }
            }   
        }

        private void LoadParam()
        {
            TodoID = GetDecryptParam("TodoID", "0").ToNumber<int>();
        }


        private void ModTodo()
        {
            LoadParam();

            BE.ToDoInfo info = new BE.ToDoInfo();
            DAL.ToDo dal = new DAL.ToDo();
            DataTableReader dtr= dal.GetTodoID(CompanyID, TodoID);

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        info.CompanyID = CurrentUserInfo.CompanyID;

                        info.Type = dtr["TYPE"].ToString();
                        info.Status = dtr["Status"].ToString();
                        info.Priority = dtr["Priority"].ToString();
                        info.Content = dtr["Content"].ToString();
                        info.StartDate = DateTime.Parse(dtr["StartDate"].ToString());
                        info.EndDate = DateTime.Parse(dtr["EndDate"].ToString());
                        info.ScheduleYN = dtr["ScheduleYN"].ToString();
                        info.IsOpen = dtr["ScheduleYN"].ToString();
                        info.UserID = UserID;
                        info.ReservationID = int.Parse(dtr["ReservationID"].ToString());
                        info.ToDoID = TodoID;
                        info.DeptID = CurrentUserInfo.DeptID;

                        //제목
                        info.Title = txtTitle.Value.Trim();
                        //중요도
                        info.Priority = ddltodo.SelectedValue.ToString();
                        //진척도
                        info.Status = ddlPriorty.SelectedValue.ToString();

                        //일정
                        if (ddlDate.SelectedValue == "2")//특정일까지 할일
                        {
                            //info.StartDate =
                            info.EndDate = DateTime.Parse(txtDate.Value);
                        }
                        else if (ddlDate.SelectedValue == "3")//특정일에 할일
                        {
                            info.StartDate = DateTime.Parse(txtDate.Value);
                            info.EndDate = DateTime.Parse(info.StartDate.ToString("yyyy-MM-dd 23:59"));
                        }
                        else if (ddlDate.SelectedValue == "4")  // 내일로연기
                        {
                            info.StartDate = DateTime.Parse(txtDate.Value).AddDays(1);
                            info.EndDate = DateTime.Parse(info.StartDate.ToString("yyyy-MM-dd 23:59"));
                        }
                        else if (ddlDate.SelectedValue == "5")//특정일연기
                        {
                            info.StartDate = DateTime.Parse(txtDate.Value);
                            info.EndDate =DateTime.Parse(info.StartDate.ToString("yyyy-MM-dd 23:59"));
                            info.Status = "D";
                        }

                        dal.ModTodo(info);

                        if (ddlDate.SelectedValue == "3")
                        {
                            info.Status = "W";
                            dal.TodoAdd(info);
                        }
                    }
                }
            }
        }

        #endregion

        #region 이벤트핸들러

        protected void lbtnWrite_Click(object sender, EventArgs e)
        {
            try
            {
                ModTodo();
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("lbtnWrite_Click", ex);
            }
            
        }
        #endregion 

        #region 프로퍼티
        #endregion 

        

        


    }
}