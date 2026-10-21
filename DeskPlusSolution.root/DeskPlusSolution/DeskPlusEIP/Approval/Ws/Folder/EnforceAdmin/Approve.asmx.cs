using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Text.RegularExpressions;
using System.IO;
using Zeospace.DeskPlus.Approval.BE;
using System.Collections;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;
using System.Collections.Specialized;
using System.Text;
using ObjList = System.Collections.Generic.List<System.Object>;

namespace Zeospace.DeskPlus.Approval.Web.Ws.Folder.EnforceAdmin
{
    /// <summary>
    /// Approve의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class Approve : Base.BaseWSApproval
    {
        /// <summary>
        /// 승인/반려
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool Confirm(string p)
        {
            try
            {
                #region 다국어 리소스 로드/Title 정보
                this.ResourceName = "Zeospace.DeskPlus.Approval.Web.Resource.GlobalString.string";		            //리소스 명 선언
                this.AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.Ws.Folder.EnforceAdmin.Approve).Assembly;	//어셈블리 선언
                this.GetResourceManager();
                #endregion

                SetDecryptString(p);

                int enforceID = GetDecryptParam("enforceID", "0").ToNumber<int>();
                int enforceStampID = GetDecryptParam("enforceStampID", "0").ToNumber<int>();
                int stampDeptID = GetDecryptParam("stampDeptID", "0").ToNumber<int>();
                int reqUserID = GetDecryptParam("reqUserID", "0").ToNumber<int>();
                string state = GetDecryptParam("state", "0");
                string confirmOpinion = GetDecryptParam("confirmOpinion", "0");

                if (enforceID.Equals(0) || enforceStampID.Equals(0) || stampDeptID.Equals(0) || state.Equals(""))
                {
                    return false;
                }

                BE.EnforceInfo info = new EnforceInfo();
                info.CompanyID = CurrentUserInfo.CompanyID;
                info.EnforceID = enforceID;
                info.EnforceStampID = enforceStampID;
                info.StampDeptID = stampDeptID;
                info.ConfirmUserID = CurrentUserInfo.UserID;
                info.ReqUserID = reqUserID;
                info.State = state;
                info.ConfirmOpinion = confirmOpinion;

                info.AlarmSubject = (info.State.ToUpper().Equals("R")) ? this.GetResource("EnforceAlarmSubjectB") : this.GetResource("EnforceAlarmSubjectC");
                info.AlarmContent = (info.State.ToUpper().Equals("R")) ? this.GetResource("EnforceAlarmContentB") : this.GetResource("EnforceAlarmContentC");

                BLL.Enforce bll = new BLL.Enforce();
                BE.UserInfo sendInfo = GetSendUserInfo("", "");
                return bll.Confirm(info, sendInfo);
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 삭제
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool Del(string p)
        {
            try
            {
                SetDecryptString(p);

                int enforceID = GetDecryptParam("enforceID", "0").ToNumber<int>();
                if (enforceID.Equals(0))
                {
                    return false;
                }

                BLL.Enforce bll = new BLL.Enforce();
                return bll.Del(CurrentUserInfo.CompanyID, enforceID, true);
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }
    }
}
