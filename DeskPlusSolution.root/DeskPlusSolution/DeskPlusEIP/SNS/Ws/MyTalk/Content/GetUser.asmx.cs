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

namespace Zeospace.DeskPlus.SNS.Web.Ws.MyTalk.Content
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class GetUser : Base.BaseWSMyTalk
    {
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool SetOpenUser(string p)
        {
            SetDecryptString(p);
            string strShare = GetDecryptParam("Share", "").Replace("@", ")");

            return AddOpenUser(strShare);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ShareValue"></param>
        /// <returns></returns>
        private bool AddOpenUser(string ShareValue)
        {
            try
            {
                DAL.OpenList dal = new DAL.OpenList();

                //삭제
                BE.OpenListInfo be = new BE.OpenListInfo();
                be.CompanyID = CurrentUserInfo.CompanyID;
                be.UserID = CurrentUserInfo.UserID;
                dal.DelOpenList(be);

                //추가


                string[] strTemp = ShareValue.Split(')');

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

                            dal.AddOpenList(be);
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
