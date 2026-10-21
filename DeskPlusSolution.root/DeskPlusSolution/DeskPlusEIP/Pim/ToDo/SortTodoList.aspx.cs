using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Data;
using Zeospace.DeskPlus.Pim.BE;

namespace Zeospace.DeskPlus.Pim.Web.ToDo
{

    public partial class SortTodoList : Base.BasePagePim
    { 
        #region Declaration (컨트롤 선언)
        #endregion

        #region 변수설정
        public int CompanyID;
        public int UserID;
          
        #endregion

        #region Page Process (페이지 처리)
        #endregion

        #region Method (함수)

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Pim.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Pim.Web.ToDo.SortTodoList).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {                
                SetLanguage();
                
                TodoLoadData();
                
            }
        }

        /// <summary>
        /// 작성목적   : 초기화 데이터 설정
        /// 작성자     : 
        /// </summary>
        private void TodoLoadData()
        {
            LoadParam();
            string sDtae = string.Empty;

            

            sDtae = Date;
            if (sDtae.Length > 2)
            {
                if (sDtae.Substring(0, 2) == "T_")
                {
                    sDtae = sDtae.Replace("T_", "");
                    sDtae = sDtae.Substring(0, 4) + "-" + sDtae.Substring(4, 2) + "-" + sDtae.Substring(6, 2);
                }
            }

            string [] arOpentype = Opentype.Split(',');
            List<object> listobj = new List<object>();

            for (int o = 0; o < arOpentype.Length; o++)
            {

                if (arOpentype[o].Length > 0)
                {
                    BE.ToDoInfo _info = new ToDoInfo();
                    DAL.ToDo _biz = new DAL.ToDo();

                    _info.CompanyID = CurrentUserInfo.CompanyID;
                    _info.UserID = UserID;
                    _info.DeptID = CurrentUserInfo.DeptID;
                    _info.StartDate = DateTime.Parse(sDtae);

                    DataTable dt = new DataTable();
                    DataTableReader dtr = new DataTableReader(dt);

                    switch (arOpentype[o])
                    {
                        case "-1":  //나의
                            dtr = _biz.GetTodo_Mine(_info);
                            break;
                        case "0":  //개인
                            dtr = _biz.GetTodo_Person(_info);
                            break;
                        case "3":  //부서
                            dtr = _biz.GetTodo_Dept(_info);
                            break;
                        case "1":  //공유
                            dtr = _biz.GetTodo_Share(_info);
                            break;
                        case "2":  //전체
                            dtr = _biz.GetTodo_All(_info);
                            break;
                    }

                    if (dtr != null)
                    {
                        if (dtr.HasRows)
                        {
                            while (dtr.Read())
                            {
                                BE.ToDoInfo data = new ToDoInfo();

                                data.ToDoID = dtr["ToDoID"].ToString().ToNumber<int>();

                                string username = "[" + GetLangName(dtr["NameBase"].ToString(), dtr["NameKo"].ToString(), dtr["NameEn"].ToString()
                               , dtr["NameJa"].ToString(), dtr["NameFr"].ToString(), dtr["NameZhcn"].ToString(), dtr["NameZhtw"].ToString(), PreLang) + "]";

                                if (dtr["UserID"].ToString().ToNumber<int>() == CurrentUserInfo.UserID)
                                {
                                    username = "";
                                }


                                data.Title = username + dtr["Title"].ToString();//MaxTextCut(dtr["Title"].ToString(), 25, "...");
                                data.StartDate = DateTime.Parse(dtr["StartDate"].ToString());
                                //data.Type = dtr["Type"].ToString();
                                data.UserID = dtr["UserID"].ToString().ToNumber<int>();
                                switch (dtr["OpenType"].ToString())
                                {
                                    case "-1":
                                        data.Type = GetResource("TYPE1");//"나의";
                                        break;
                                    case "0":
                                        data.Type = GetResource("TYPE2");//"개인";
                                        break;
                                    case "3":
                                        data.Type = GetResource("TYPE3");//"부서";
                                        break;
                                    case "1":
                                        data.Type = GetResource("TYPE4");//"공유";
                                        break;
                                    case "2":
                                        data.Type = GetResource("TYPE5");//"전체";
                                        break;

                                }

                                switch (dtr["Priority"].ToString())
                                {
                                    case "0":
                                        data.Priority = "";
                                        break;
                                    case "1":
                                        data.Priority = GetResource("TOP");//"상";
                                        break;
                                    case "2":
                                        data.Priority = GetResource("MIDDLE");//"중";
                                        break;
                                    case "3":
                                        data.Priority = GetResource("BOTTOM");//"하";
                                        break;
                                }
                                data.Status = dtr["Status"].ToString();

                                data.UserName = GetLangName(dtr["NameBase"].ToString(), dtr["NameKo"].ToString(),
                                     dtr["NameEn"].ToString(), dtr["NameJa"].ToString()
                                   , dtr["NameFr"].ToString(), dtr["NameZhCn"].ToString(), dtr["NameZhTw"].ToString(),
                                   PreLang);

                                data.ReplyCnt = dtr["ReplyCnt"].ToString().ToNumber<int>();

                                listobj.Add(data);
                            }
                        }
                    }

                }
            }

            foreach (ToDoInfo _info in listobj)
            {
                string ReplyCnt = (_info.ReplyCnt > 0) ? " [" + _info.ReplyCnt + "]" : "";
                ulList.InnerHtml += "<li>(" + _info.Type + ")&nbsp;<a href='#' onclick='parent.MoveTodoView(" + _info.ToDoID + ")'>" + _info.Title + ReplyCnt + "<div class=dotLine></div></a></li>";
                
            }
            //ulList.InnerHtml = "<li>1</li><li>1</li><li>1</li><li>1</li><li>1</li><li>1</li><li>1</li><li>1</li><li>1</li><li>1</li><li>1</li><li>1</li>";
        }

 

        /// <summary>
        /// 작성목적   : 파라미터 설정
        /// 작성자     : 
        /// </summary>
        private void LoadParam()
        {
            //CatID = GetDecryptParam("catID", "0").ToNumber<int>();
            CompanyID = GetDecryptParam("CompanyID", "0").ToNumber<int>();
            UserID = GetDecryptParam("UserID", "0").ToNumber<int>();
            Opentype = GetDecryptParam("OpenType", "0").ToString();
            Date = GetDecryptParam("Date", "0").ToString();
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
                divPopTitle.InnerText = GetResource("TODOCADALOG");
                CbtnClosetodo.Text = GetResource("Close");
                //cbtnCancel.Text = resMng.GetString("Cancel");
            }
        }

        #endregion

        #region Event Handler (이벤트 핸들러)

        #endregion

        #region Property (프로퍼티)

        #endregion
    }        
  
     
}