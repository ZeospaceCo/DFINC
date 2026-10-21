using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Zeospace.DeskPlus.Pim.Web.Schedule
{
    
    public partial class AddSimple : Base.BasePagePim
    { 
        #region Declaration (컨트롤 선언)
        #endregion

        #region 변수설정
        DateTime CurrentTime = new DateTime();

        #endregion

        #region Page Process (페이지 처리)
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Pim.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Pim.Web.Schedule.AddSimple).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                SetLanguage();               
                SettingControl();
                LoadData();
            }
        }

        #endregion

        #region Method (함수)


        private void SettingControl()
        {
            ListItem ltcate1 = new ListItem(GetResource("pim1"), "0");
            ListItem ltcate2 = new ListItem(GetResource("pim2"), "3");
            ListItem ltcate3 = new ListItem(GetResource("pim3"), "1");
            ListItem ltcate4 = new ListItem(GetResource("pim4"), "2");


            ddlShare.Items.Add(ltcate1);
            ddlShare.Items.Add(ltcate2);
            ddlShare.Items.Add(ltcate3);
            ddlShare.Items.Add(ltcate4);

            MakeTime();
        }


        private void MakeTime()
        {
            string strVal = string.Empty;
            string strKey = string.Empty;

            for (int i = 0; i < 24; i++)
            {
                if (i < 12)
                {

                    strVal = GetResource("AM") + " ";

                    if (i == 0)
                    {
                        strVal = strVal + " 12";
                    }
                    else
                    {
                        
                            strVal = strVal + " " + i.ToString();
                    }
                }
                else
                {
                    strVal = GetResource("PM") + " ";
                    if (i == 12)
                    {
                        strVal = strVal + " 12";
                    }
                    else
                    {
                        strVal = strVal + " " + (i - 12).ToString();                        
                    }
                }

                string strTemp = string.Empty;
                strTemp = strVal + ":00";
                strKey = i.ToString() + ":00";
                ListItem lv1 = new ListItem(strTemp, strKey);
                ListItem lv12 = new ListItem(strTemp, strKey);

                ddlstartTime.Items.Add(lv1);
                ddlendTime.Items.Add(lv12);

                strTemp = strVal + ":30";
                strKey = i.ToString() + ":30";
                ListItem lv2 = new ListItem(strTemp, strKey);
                ListItem lv22 = new ListItem(strTemp, strKey);
                ddlstartTime.Items.Add(lv2);
                ddlendTime.Items.Add(lv22);
            }
        }




        /// <summary>
        /// 작성목적   : 초기화 데이터 설정
        /// 작성자     : 
        /// </summary>
        private void LoadData()
        {
            LoadParam();

            tbstartDate.Value = CurrentTime.ToString("yyyy-MM-dd");
            string t1 = CurrentTime.ToString("HH:mm");
            if (t1.Substring(0, 1) == "0")
                t1 =  t1.Substring(1,4);

            ddlstartTime.SelectedValue = t1; //CurrentTime.ToString("HH:mm");

            CurrentTime = CurrentTime.AddHours(1);

            tbendDate.Value = CurrentTime.ToString("yyyy-MM-dd");

            string t2 = CurrentTime.ToString("HH:mm");
            if (t2.Substring(0, 1) == "0")
                t2 = t2.Substring(1, 4);

            ddlendTime.SelectedValue = t2;
            //ddlstartTime
        }


        /// <summary>
        /// 작성목적   : 파라미터 설정
        /// 작성자     : 
        /// </summary>
        private void LoadParam()
        {
           CurrentTime = DateTime.Parse(GetDecryptParam("TIME", DateTime.Now.ToString("yyyy-MM-dd hh:mm")));
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
                CbtnConfirm.Text = GetResource("Confirm");
                CbtnClose.Text = GetResource("Close");
            }
        }

        #endregion

        #region Event Handler (이벤트 핸들러)

        #endregion

        #region Property (프로퍼티)

        #endregion
    }        
  
}