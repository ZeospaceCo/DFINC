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

namespace Zeospace.DeskPlus.Pim.Web.Ws.Target
{
    /// <summary>
    /// GetPim의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class List : Base.BaseWSPim
    {
        #region Get

        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetTargetList(string p)
        {

            int iUserID = 0;
            string startDate = string.Empty;
            int DeptID = 0;

            SetDecryptString(p);

            iUserID = GetDecryptParam("UserID", "0").ToNumber<int>();
            startDate = GetDecryptParam("startDate", DateTime.Now.ToString("yyyy-MM-dd"));
            DeptID = GetDecryptParam("DeptID", "0").ToNumber<int>();

            if (DeptID == 0)
                DeptID = CurrentUserInfo.DeptID;

            List<object> listobj = new List<object>();

            BE.MBOInfo _info = new MBOInfo();

            _info.CompanyID = CurrentUserInfo.CompanyID;
            _info.UserID = iUserID;
            _info.StartDate = DateTime.Parse(startDate);
            _info.DeptID = CurrentUserInfo.DeptID;

            DAL.MBO biz = new MBO();


            DataTableReader dtr = biz.GetMBOInfo(_info);

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        BE.MBOInfo data = new MBOInfo();

                        //data.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                        data.MBOID = dtr["MBOID"].ToString().ToNumber<int>();
                        data.ParentMBOID = dtr["ParentMBOID"].ToString().ToNumber<int>();
                        data.MBOGroup = dtr["MBOGroup"].ToString().ToNumber<int>();
                        data.MBOOrder = dtr["MBOOrder"].ToString().ToNumber<int>();
                        data.Dept = dtr["Dept"].ToString().ToNumber<int>();
                        data.Title = dtr["Title"].ToString();

                        data.StartDate = DateTime.Parse(dtr["StartDate"].ToString());
                        data.EndDate = DateTime.Parse(dtr["EndDate"].ToString());
                        data.UserID = dtr["UserId"].ToString().ToNumber<int>();
                        data.attachYN = dtr["attachYN"].ToString();
                        data.GroupCnt = dtr["GroupCnt"].ToString().ToNumber<int>();

                        if (CurrentUserInfo.UserID == dtr["UserId"].ToString().ToNumber<int>())
                        {
                            data.AuthYN = "0";
                        }
                        else
                        {
                            data.AuthYN = "1";
                        }

                        data.strStartDate = DateTime.Parse(dtr["StartDate"].ToString()).ToString("yyyy-MM-dd");

                        data.strEndDate = DateTime.Parse(dtr["EndDate"].ToString()).ToString("yyyy-MM-dd");

                        if (DeptID != _info.DeptID)
                        {
                            int cnt = biz.GetMBOInfoIDCnt(_info.CompanyID, data.MBOID, DeptID);
                            
                            if(cnt>0)
                                listobj.Add(data);
                        }
                        else
                        {
                            listobj.Add(data);
                        }
                    }
                }
            }
            return listobj;
        }

        /// <summary>
        /// 삭제
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int DelTarget(string p)
        {
            string strRt = string.Empty;
            int MBOID = 0;
            int CompanyID = 0;

            SetDecryptString(p);

            MBOID = GetDecryptParam("MBOID", "").ToNumber<int>();
            CompanyID = GetDecryptParam("CompanyID", "0").ToNumber<int>();

            DAL.MBO biz = new MBO();

            biz.DelMBO(CompanyID, MBOID);

            return 1;
        }

        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string GetTargetFileList(string p)
        {

            int iMBOID = 0;
            int CompanyID = 0;

            SetDecryptString(p);

            CompanyID = GetDecryptParam("CompanyID", "0").ToNumber<int>();
            iMBOID = GetDecryptParam("MBOID", "0").ToNumber<int>();

            List<object> listobj = new List<object>();
            DAL.MBOAttachFiles biz = new MBOAttachFiles();

            DataTableReader dtr = biz.GetMBOFileID(CompanyID, iMBOID);

            string response = "0";
            string totalFileInfo = "";

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        if (dtr["FileGUID"].ToString().Trim().Length > 0)
                        {
                            totalFileInfo += dtr["FileName"].ToString() + ":" + dtr["FilePath"].ToString() + dtr["FileGUID"].ToString() + "|"; // Key < 파일경로파일명
                        }
                        else
                        {
                            totalFileInfo += dtr["FileName"].ToString() + ":" + dtr["FilePath"].ToString() + dtr["FileGUID"].ToString() + "|"; // Key < 파일경로파일명
                        }
                    }
                }
            }
            totalFileInfo = totalFileInfo.TrimEnd('|');

            if (totalFileInfo.Length > 0)
            {
                response = totalFileInfo.Replace("&", "&amp;");		// 첨부파일 정보 가져오기
            }


            return response;
        }

        #endregion

        #region Set
        #endregion
    }
}
