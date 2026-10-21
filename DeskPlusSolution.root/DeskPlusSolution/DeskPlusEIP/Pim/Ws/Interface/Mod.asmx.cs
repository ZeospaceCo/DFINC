using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Text.RegularExpressions;
using System.IO;
using System.Collections;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;
using System.Collections.Specialized;
using System.Text;
using Zeospace.DeskPlus.Pim.BE;
using Zeospace.DeskPlus.Pim.DAL;

namespace Zeospace.DeskPlus.Pim.Web.Ws.Interface
{
    /// <summary>
    /// Mod의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class Mod : Base.BaseWSPim
    {
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int DelSchedule(string p)
        {
            try
            {
                int _rtn = 0;

                SetDecryptString(p);

                //////////////////////////////////
                //사용 파라미터
                //////////////////////////////////               
                int _interfaceid = GetDecryptParam("InterfaceID", "").ToNumber<int>(); //인터페이스 아이디
                string _sendtarget = GetDecryptParam("SendTarget", "PIM-Schedule"); //모듈구분
                int _sendid = GetDecryptParam("SendID", "").ToNumber<int>(); //발송 아이디
                int _targetid = GetDecryptParam("TargetID", "").ToNumber<int>(); //수신 아이디
                int _paratype = GetDecryptParam("ParaType", "").ToNumber<int>(); //발송구분 1:인터페이스 아이디, 2:발송 아이디, 3:수신 아이디                
                //////////////////////////////////

                Zeospace.DeskPlus.Common.BLL.Interface InterfaceBLL = new Zeospace.DeskPlus.Common.BLL.Interface();

                BE.DisplayInfo info = new DisplayInfo();
                DAL.Display dal = new Display();

                BE.ScheduleInfo info2 = new ScheduleInfo();               
                DAL.Schedule dal2 = new DAL.Schedule();

                if (_paratype != 3)
                {
                    DataTableReader drList = InterfaceBLL.GetTargetID(CurrentUserInfo.CompanyID, _sendtarget, _targetid, _paratype);

                    while (drList.Read())
                    {
                        _targetid = drList.GetString(5).ToNumber<int>();
                        
                        info.ScheduleID = _targetid;
                        info.CompanyID = CurrentUserInfo.CompanyID;

                        dal.DelDisplay(info);

                        info2.ScheduleID = _targetid;
                        info2.CompanyID = CurrentUserInfo.CompanyID;

                        dal2.Del(info2);
                    }
                }
                else
                {
                    info.ScheduleID = _targetid;
                    info.CompanyID = CurrentUserInfo.CompanyID;

                    dal.DelDisplay(info);

                    info2.ScheduleID = _targetid;
                    info2.CompanyID = CurrentUserInfo.CompanyID;

                    dal2.Del(info2);
                }

                return _rtn;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                return -1;
            }	

        }

        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int DelToDo(string p)
        {
            try
            {
                int _rtn = 0;

                SetDecryptString(p);

                //////////////////////////////////
                //사용 파라미터
                //////////////////////////////////               
                int _interfaceid = GetDecryptParam("InterfaceID", "").ToNumber<int>(); //인터페이스 아이디
                string _sendtarget = GetDecryptParam("SendTarget", "PIM-ToDo"); //모듈구분
                int _sendid = GetDecryptParam("SendID", "").ToNumber<int>(); //발송 아이디
                int _targetid = GetDecryptParam("TargetID", "").ToNumber<int>(); //수신 아이디
                int _paratype = GetDecryptParam("ParaType", "").ToNumber<int>(); //발송구분 1:인터페이스 아이디, 2:발송 아이디, 3:수신 아이디                
                //////////////////////////////////

                Zeospace.DeskPlus.Common.BLL.Interface InterfaceBLL = new Zeospace.DeskPlus.Common.BLL.Interface();

                DAL.ToDo dal = new DAL.ToDo();

                if (_paratype != 3)
                {
                    DataTableReader drList = InterfaceBLL.GetTargetID(CurrentUserInfo.CompanyID, _sendtarget, _targetid, _paratype);

                    while (drList.Read())
                    {
                        _targetid = drList.GetString(5).ToNumber<int>();

                        dal.DelTodo(CurrentUserInfo.CompanyID, _targetid);
                    }
                }
                else
                {
                    if (dal.DelTodo(CurrentUserInfo.CompanyID, _targetid))
                    {
                        _rtn = 1;
                    }
                    else
                    {
                        _rtn = -1;
                    }
                }

                return _rtn;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                return -1;
            }

        }
    }
}
