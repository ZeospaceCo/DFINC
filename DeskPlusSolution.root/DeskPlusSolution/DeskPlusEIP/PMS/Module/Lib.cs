using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using Zeospace.DeskPlus.PMS.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.PMS.Web.Module
{
    public class Lib
    {

        /// <summary>
        /// MenuType 별 해당 메뉴 URL 을 반환한다.
        /// </summary>
        /// <param name="_menuType"></param>
        /// <returns></returns>
        public string GetMenuUrl(int _menuType)
        {
            string url = "";
            switch (_menuType)
            {
                case 1: url = "/DeskPlusEIP/PMS/Project/ProjectInfo.aspx"; break;
                case 2: url = "/DeskPlusEIP/PMS/Unit/JList.aspx"; break;
                case 3: url = "/DeskPlusEIP/PMS/Board/JList.aspx"; break;
                case 4: url = "/DeskPlusEIP/PMS/Board/JList.aspx"; break;
                case 5: url = "/DeskPlusEIP/PMS/Schedule/List.aspx"; break;
                case 6: url = "/DeskPlusEIP/PMS/Member/JList.aspx"; break;
            }

            return url;
        }


        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 메뉴제목
        /// 작성자     : 심재운
        /// 최초작성일 : 2009-01-22
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_projectID">The _project ID.</param>
        /// <param name="_menuID">The _menu ID.</param>
        /// <param name="_preLang">The _pre lang.</param>
        /// <returns></returns>
        /// <value>The name of the menu.</value>
        public string MenuName(int _companyID, int _projectID, int _menuID, string _preLang)
        {
            DAL.Menu menu = new DAL.Menu();
            List<BE.MenuInfo> al = menu.GetMenuList(_companyID, _projectID, _menuID);

            foreach (BE.MenuInfo menuInfo in al)
            {
                return UtilHelper.GetLangName(menuInfo.MenuBase, menuInfo.MenuKo, menuInfo.MenuEn, menuInfo.MenuJa, menuInfo.MenuFr, menuInfo.MenuZhcn, menuInfo.MenuZhtw, _preLang);
            }

            return "";
        }


        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 메뉴제목
        /// 작성자     : 심재운
        /// 최초작성일 : 2009-01-22
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_projectID">The _project ID.</param>
        /// <param name="_menuID">The _menu ID.</param>
        /// <param name="_menuType">Type of the _menu.</param>
        /// <param name="_preLang">The _pre lang.</param>
        /// <returns></returns>
        /// <value>The name of the menu.</value>
        public string MenuName(int _companyID, int _projectID, int _menuID, int _menuType, string _preLang)
        {
            DAL.Menu menu = new Zeospace.DeskPlus.PMS.DAL.Menu();
            var al = menu.GetMenuList(_companyID, _projectID, _menuID, _menuType);
            foreach(var menuInfo in al)
            {
                return UtilHelper.GetLangName(menuInfo.MenuBase, menuInfo.MenuKo, menuInfo.MenuEn, menuInfo.MenuJa, menuInfo.MenuFr, menuInfo.MenuZhcn, menuInfo.MenuZhtw, _preLang);
            }

            return "";
        }


        /// <summary>
        /// appsetting 의 value 값 가져오기
        /// </summary>
        /// <param name="keyValue">The key value.</param>
        /// <param name="_preLang">The _pre lang.</param>
        /// <param name="_xmlName">Name of the _XML.</param>
        /// <returns></returns>
        public string GetXmlListSearch(string keyValue, string _preLang, string _xmlName)
        {
            HttpContext ctx = HttpContext.Current;
            DataSet ds = null;

            string cacheName = _xmlName + _preLang;

            if (ctx.Cache[cacheName] == null)
            {
                if (_preLang.Length > 0) _preLang = _preLang + "/";
                string filePath = HttpContext.Current.Server.MapPath("/DeskPlusEIP/PMS/App_Data/" + _preLang + _xmlName);
                ds = new DataSet();
                ds.ReadXml(filePath);

                ctx.Cache.Insert(cacheName, ds, null, DateTime.Now.AddDays(1), System.Web.Caching.Cache.NoSlidingExpiration);
                ds = null;
            }

            ds = (DataSet)ctx.Cache[cacheName];

            if (ds.Tables[0].Columns.Contains(keyValue))
            {
                object o = ds.Tables[0].Rows[0][keyValue];

                if (Convert.IsDBNull(o))
                {
                    return String.Empty;
                }
                else
                {
                    return (string)o;
                }
            }
            else
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetXmlListSearch : " + keyValue);
                return "";
            }
        }


        

        /// <summary>
        /// 링크 프로젝트 체크
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_projectID"></param>
        /// <param name="_rqProjectID"></param>
        /// <param name="_userID"></param>
        /// <returns></returns>
        public bool LinkProject(int _companyID, int _projectID, int _rqProjectID)
        {
            bool response = false;
            DAL.RelationLink link = new Zeospace.DeskPlus.PMS.DAL.RelationLink();
            List<int> list = link.GetRelationID(_companyID, _rqProjectID);
            response = (list.Contains(_projectID)) ? true : false;

            return response;
        }



        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 해당 프로젝트 상태여부 (1:대기;2:진행;3:완료;4:지연;5:보류)
        /// 작성자     : 심재운
        /// 최초작성일 : 2009-01-29
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_projectID">The _project ID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <returns></returns>
        public bool UseProject(int _companyID, int _projectID, int _userID)
        {
            bool response = false;

            DAL.Project bll = new Zeospace.DeskPlus.PMS.DAL.Project();
            var al = bll.GetProject(_companyID, _projectID);

            foreach (var info in al)
            {
                //개설자는 상태값과 상관없이 true 이다.
                if (info.WriteUserID == _userID) response = true;
                //상태가 "대기" 아닐 경우, 참여자 정보에 포함되어 있으면 true 이다.
                if (info.Status != 1)
                {
                    DAL.ProjectUser puDal = new Zeospace.DeskPlus.PMS.DAL.ProjectUser();
                    var puList = puDal.GetProjectUser(_companyID, _projectID, _userID);

                    //상태(1:관리자;2;참여자)
                    foreach (var pInfo in puList)
                    {
                        if (pInfo.UserID == _userID)
                        {
                            response = true;
                            break;
                        }
                    }
                }
            }

            return response;
        }



        /// <summary>
        /// 시작일-종료일 기준으로 전체일,투입일,잔여일,지연일을 배열로 반환한다.
        /// 토요일/일요일은 계산에서 제외된다.
        /// 현재 일반 공휴일 체크는 하지 않는다.
        /// </summary>
        /// <param name="_sDate">The _s date.</param>
        /// <param name="_eDate">The _e date.</param>
        /// <param name="_complateDate">The _complate date.</param>
        /// <returns>
        /// 0:전체일,1:투입일,2:잔여일,3:지연일
        /// </returns>
        public List<int> GetProjcectDays(DateTime _sDate, DateTime _eDate, DateTime _complateDate)
        {
            //written  by shimpark
            _complateDate = (_complateDate.ToNullDT() == System.DBNull.Value) ? System.DateTime.Now : _complateDate;

            DateTime tempSDate = _sDate;
            DateTime tempEDate = _eDate;
            int i = 0;
            int inDay = 0;
            int totalDay = 0;
            int delayDay = 0;
            int holiDay = 0;

            if (!DateTime.MaxValue.Equals(_sDate) && !DateTime.MaxValue.Equals(_eDate))
            {
                while (true)
                {
                    i++;
                    if (tempSDate.ToShortDateString().Equals(_eDate.ToShortDateString())) break;
                    if (tempSDate.DayOfWeek == DayOfWeek.Sunday || tempSDate.DayOfWeek == DayOfWeek.Saturday)   holiDay++;
                    if (tempSDate.ToShortDateString().Equals(_complateDate.ToShortDateString())) inDay = i - holiDay;
                    tempSDate = tempSDate.AddDays(1);
                }

                totalDay = i - holiDay;
                                
                i = 0;
                holiDay = 0;

                //프로젝트 종료일보다 완료일이 작을 경우(지연된 사항이다.)
                if (_eDate < _complateDate)
                {
                    while (true)
                    {
                        //완료일과 프로젝트 종료일 만큼 FOR 문 돈다.
                        if (tempEDate.ToShortDateString().Equals(_complateDate.ToShortDateString())) break;
                        //완료일과 프로젝트 종료일 사이의 공휴일(토,일)을 카운트 한다.
                        if (tempEDate.DayOfWeek == DayOfWeek.Sunday || tempEDate.DayOfWeek == DayOfWeek.Saturday)   holiDay++;
                        //완료일에 +1 일 추가한다. 언젠가 프로젝트 종료일과 동일해 지겠지...
                        tempEDate = tempEDate.AddDays(1);
                        i++; //modify shimpark
                    }
                    delayDay = i - holiDay;//프로젝트 종료일 -  프로젝트종료일 - 공휴일
                    inDay = totalDay + delayDay - holiDay;//전체일수 + 지연일수 - 공휴일
                }
            }

            //0:전체일,1:투입일,2:잔여일,3:지연일
            List<int> list = new List<int>();
            list.Add(totalDay);//완료일-시작일-공유일
            list.Add(inDay); //프로젝트종료일 - 시작일
            list.Add(totalDay - inDay);//전체일 - 투입일
            list.Add(delayDay);//프로젝트종료일-종료일
            return list;
        }        
    }
}
