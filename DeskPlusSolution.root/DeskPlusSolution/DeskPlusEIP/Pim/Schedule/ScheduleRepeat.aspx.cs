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

namespace Zeospace.DeskPlus.Pim.Web.Schedule
{
    public partial class ScheduleRepeat : Base.BasePagePim
    {
        #region 변수설정
        public int CompanyID = 0;
        public int UserID = 0;

        

        #endregion

        #region 컨트롤선언

        #endregion

        #region 페이지처리
        
        protected void Page_Load(object sender, EventArgs e)
        {
            CompanyID = CurrentUserInfo.CompanyID;
            UserID = CurrentUserInfo.UserID;

            #region 다국어 리소스 로드/Title 정보            
            this.ResourceName = "Zeospace.DeskPlus.Pim.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Pim.Web.Schedule.ScheduleRepeat).Assembly;	//어셈블리 선언
            #endregion            
            
            if (!IsPostBack)
            {
                SetLanguage();
                SetControl();
            }

        }

        #endregion

        #region 함수
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                CbtnConfirm.Text = GetResource("Confirm");
                CbtnClose.Text = GetResource("Close");

            }
        }

        private void SetControl()
        {
            //ddlorder
            ListItem lv1 = new ListItem(GetResource("order1"), "1");
            ListItem lv2 = new ListItem(GetResource("order2"), "2");
            ListItem lv3 = new ListItem(GetResource("order3"), "3");
            ListItem lv4 = new ListItem(GetResource("order4"), "4");
            ListItem lv5 = new ListItem(GetResource("order5"), "5");
            ddlorder.Items.Add(lv1);
            ddlorder.Items.Add(lv2);
            ddlorder.Items.Add(lv3);
            ddlorder.Items.Add(lv4);
            ddlorder.Items.Add(lv5);

            ListItem lvt1 = new ListItem(GetResource("kind1"), "0");
            ListItem lvt2 = new ListItem(GetResource("kind2"), "1");
            ListItem lvt3 = new ListItem(GetResource("kind3"), "2");
            ListItem lvt4 = new ListItem(GetResource("kind4"), "3");
            ListItem lvt5 = new ListItem(GetResource("kind5"), "4");
            ListItem lvt6 = new ListItem(GetResource("kind6"), "5");
            ListItem lvt7 = new ListItem(GetResource("kind7"), "6");
            ListItem lvt8 = new ListItem(GetResource("kind8"), "7");
            ListItem lvt9 = new ListItem(GetResource("kind9"), "8");
            ListItem lvt10 = new ListItem(GetResource("kind10"), "9");

            ddlkind.Items.Add(lvt1);
            ddlkind.Items.Add(lvt2);
            ddlkind.Items.Add(lvt3);
            ddlkind.Items.Add(lvt4);
            ddlkind.Items.Add(lvt5);
            ddlkind.Items.Add(lvt6);
            ddlkind.Items.Add(lvt7);
            ddlkind.Items.Add(lvt8);
            ddlkind.Items.Add(lvt9);
            ddlkind.Items.Add(lvt10);

        }

        #endregion 
        
        #region 이벤트핸들러
        #endregion

        #region 프로퍼티
        #endregion
    }
}