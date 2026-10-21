using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;
using System.Text.RegularExpressions;
using System.Data;
using System.Collections.Specialized;
using System.Text;
using Zeospace.DeskPlus.Pim.BE;
using Zeospace.DeskPlus.Pim.DAL;
using System.Collections;

namespace Zeospace.DeskPlus.Pim.Web.Ws.Anniversary
{
    /// <summary>
    /// GetPim의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class View : Base.BaseWSPim
    {
        #region Get
         


        #endregion

        #region Set
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int DelSchedule(string p)
        {
            int ScheduleID = 0;
            int CompanyId = 0;
            SetDecryptString(p);

            ScheduleID = GetDecryptParam("ScheduleID", "0").ToNumber<int>();
            CompanyId = GetDecryptParam("CompanyID", "0").ToNumber<int>();

            BE.ScheduleInfo info = new ScheduleInfo();
            info.ScheduleID = ScheduleID;
            info.CompanyID = CompanyId;

            DAL.Schedule dal = new DAL.Schedule();

            dal.Del(info);

            return 1;
        }

        /// <summary>
        /// 첨부파일 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetAttachFileList(string p)
        {
            this.ResourceName = "Zeospace.DeskPlus.Pim.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Pim.Web.Ws.Anniversary.View).Assembly;	//어셈블리 선언
            this.GetResourceManager();

            string companyID = string.Empty;
            string scheduleID = string.Empty;

            SetDecryptString(p);
            companyID = CurrentUserInfo.CompanyID.ToString();
            scheduleID = GetDecryptParam("ScheduleID", "0");

            List<object> listobj = new List<object>();
            BE.AttachInfo info = new AttachInfo();
            info.CompanyID = CurrentUserInfo.CompanyID;
            info.ScheduleID = scheduleID.ToNumber<int>();


            DAL.Attach dal = new Attach();
            List<BE.AttachInfo> attachInfo = dal.GetAttachInfo(info);

            foreach (BE.AttachInfo _attachInfo in attachInfo)
            {
                _attachInfo.FileDel = resMng.GetString("Del");
                listobj.Add(_attachInfo);
            }


            return listobj;
        }
        #endregion
    }
}
