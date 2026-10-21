using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Coop.Web.Ws.Content.Receive
{
    /// <summary>
    /// ViewComplete의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class ViewComplete : Base.BaseWSCoop
    {

        /// <summary>
        /// 작성자 : 심재운
        /// 용도 : 전달 사용자 저장하기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool AddForward(string p)
        {
            try
            {
                ResourceName = "Zeospace.DeskPlus.Coop.Web.Resource.GlobalString.string";			//리소스 명 선언
                AssemblyName = typeof(Zeospace.DeskPlus.Coop.Web.Content.Request.Add).Assembly;	//어셈블리 선언
                this.GetResourceManager();

                SetDecryptString(p);

                int groupID = GetDecryptParam("groupID", "").ToNumber<int>();
                string userList = GetDecryptParam("userList", "").TrimEnd(',');

                if (groupID == 0 || userList.Length == 0) return true;

                BLL.ForwardList biz = new BLL.ForwardList();
                BLL.Alarm alarmBiz = new BLL.Alarm();
                DAL.List dalList = new DAL.List();

                var dtrList = dalList.GetList(CurrentUserInfo.CompanyID, groupID.ToString());
                string title = "";
                if (dtrList != null)
                {
                    if (dtrList.HasRows)
                    {
                        while (dtrList.Read())
                        {
                            title = dtrList["Title"].ToString();
                        }
                    }
                }

                //알람
                string alarmContent = "[" + GetResource("Forward") + "] " + title;
                foreach (string forwardUserID in userList.Split(','))
                {
                    if (forwardUserID.ToNumber<int>() > 0)
                    {
                        //전달자에게 추가
                        int forwardGroupID = biz.AddList(CurrentUserInfo.CompanyID, groupID, CurrentUserInfo.UserID, forwardUserID.ToNumber<int>());
                        if (forwardGroupID > 0)
                        {
                            //전달자에게 알람
                            alarmBiz.AlarmForward(CurrentUserInfo.CompanyID, groupID, CurrentUserInfo.UserID, CurrentUserInfo.Name,
                                forwardUserID, forwardGroupID, alarmContent);
                        }
                    }
                }               

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                return false;
            }
        }
    }
}
