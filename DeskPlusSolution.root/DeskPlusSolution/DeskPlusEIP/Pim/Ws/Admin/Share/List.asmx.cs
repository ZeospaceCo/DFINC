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

namespace Zeospace.DeskPlus.Pim.Web.Ws.Admin.Share
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
        /// <summary>
        /// 공유대상가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetOpenUser(string p)
        {
            string companyID = string.Empty;
            string UserId = string.Empty;

            SetDecryptString(p);
            companyID = GetDecryptParam("companyID", "0");
            UserId = GetDecryptParam("UserId", "0");

            List<object> listobj = new List<object>();

            DAL.OpenUser bll = new OpenUser();


            DataTableReader dtr = bll.GetOpenUser(companyID.ToNumber<int>(), UserId.ToNumber<int>());

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        BE.OpenUserInfo data = new OpenUserInfo();
                        data.Name = this.GetLangName(dtr["NameBase"].ToString(), dtr["NameKo"].ToString(),
                            dtr["NameEn"].ToString(), dtr["NameJa"].ToString(), dtr["NameFr"].ToString(),
                            dtr["NameZhcn"].ToString(), dtr["NameZhtw"].ToString(), PreLang
                            );
                        data.OpenUserID = dtr["OpenUserID"].ToString().ToNumber<int>();
                        data.UserID = dtr["UserID"].ToString().ToNumber<int>();
                        data.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                        data.GroupID = dtr["GroupID"].ToString().ToNumber<int>();
                        listobj.Add(data);
                    }
                }
            }


            return listobj;
        }

        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool SetOpenUser(string p)
        {
            SetDecryptString(p);
            string strShare = GetDecryptParam("Share", "");

            return AddOpenUser(strShare);

        }

        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int AddAdminOpenUser(string p)
        {
            string strRt = string.Empty;
            int sUserId = 0;

            SetDecryptString(p);

            strRt = GetDecryptParam("UserList", "");
            sUserId = GetDecryptParam("UserId", "0").ToNumber<int>();

            DAL.OpenUser dal = new DAL.OpenUser();
            BE.OpenUserInfo be = new BE.OpenUserInfo();
            be.CompanyID = CurrentUserInfo.CompanyID;
            be.UserID = sUserId;
            dal.OpenUserDel(be);

            string[] strTemp = strRt.Split('#');

            for (int i = 0; i < strTemp.Length; i++)
            {
                string[] temp1 = strTemp[i].Split('|');
                if (temp1.Length > 1)
                {
                    string[] temp2 = temp1[1].Split('$');

                    if (temp2.Length > 0)
                    {
                        be.OpenUserID = int.Parse(temp2[1]);

                        if (temp2[0] == "M") //개인
                        {
                            be.OpenUserID = int.Parse(temp2[1]);
                            be.GroupID = 0;
                        }
                        else if (temp2[0] == "G")//그룹
                        {
                            be.OpenUserID = 0;
                            be.GroupID = int.Parse(temp2[1]);
                        }
                        dal.OpenUserAdd(be);
                    }
                }
            }
            return 1;
        }


        private bool AddOpenUser(string ShareValue)
        {
            try
            {
                DAL.OpenUser dal = new DAL.OpenUser();

                //삭제
                BE.OpenUserInfo be = new BE.OpenUserInfo();
                be.CompanyID = CurrentUserInfo.CompanyID;
                be.UserID = CurrentUserInfo.UserID;
                dal.OpenUserDel(be);

                //추가


                string[] strTemp = ShareValue.Split('#');

                for (int i = 0; i < strTemp.Length; i++)
                {
                    string[] temp1 = strTemp[i].Split('|');
                    if (temp1.Length > 1)
                    {
                        string[] temp2 = temp1[1].Split('$');

                        if (temp2.Length > 0)
                        {
                            be.OpenUserID = int.Parse(temp2[1]);

                            if (temp2[0] == "M") //개인
                            {
                                be.OpenUserID = int.Parse(temp2[1]);
                                be.GroupID = 0;
                            }
                            else if (temp2[0] == "G")//그룹
                            {
                                be.OpenUserID = 0;
                                be.GroupID = int.Parse(temp2[1]);
                            }

                            be.kind = "P";
                            dal.OpenUserAdd(be);
                        }
                    }
                }

                return true;
            }
            catch (Exception exc)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("AddOpenUser", exc);
                return false;
            }

        }
    }
}
