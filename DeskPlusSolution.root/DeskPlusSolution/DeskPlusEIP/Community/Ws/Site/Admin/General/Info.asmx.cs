using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Community.Web.Ws.Site.Admin.General
{
    /// <summary>
    /// Info의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]    
    public class Info : Base.BaseWSCommunity
    {
        #region GET()

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-05-11
        /// 작성목적 : 소분류값 가져오기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetCategory(string p)
        {
            try
            {
                SetDecryptString(p);
                int parentCategory = GetDecryptParam("parentCategory", "0").ToNumber<int>();

                DAL.Category dal = new DAL.Category();
                DataTableReader dtr = dal.GetCategory(CurrentUserInfo.CompanyID, parentCategory);

                List<object> listobj = new List<object>();

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            BE.CategoryInfo info = new BE.CategoryInfo();

                            info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                            info.CategoryID = dtr["CategoryID"].ToString().ToNumber<int>();
                            info.ParentCategoryID = dtr["ParentCategoryID"].ToString().ToNumber<int>();
                            info.NameBase = this.GetLangName(dtr["NameBase"].ToString(), dtr["NameKo"].ToString(), dtr["NameEn"].ToString(), dtr["NameJa"].ToString(), dtr["NameFr"].ToString(),
                                                                dtr["NameZhcn"].ToString(), dtr["NameZhtw"].ToString(), PreLang).ToReverseXSS();


                            listobj.Add(info);

                        }
                    }
                }

                return listobj;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

        #endregion 

        #region ADD()

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-05-11
        /// 작성목적 : 커뮤니티 정보수정
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool ModCom(string p)
        {
            SetDecryptString(p);
            int communityID = GetDecryptParam("communityID", "0").ToNumber<int>();
            string name = GetDecryptParam("name", "");
            string description = GetDecryptParam("description", "");
            string question = GetDecryptParam("question", "");
            string answer = GetDecryptParam("answer", "");
            int parentID = GetDecryptParam("parentID", "0").ToNumber<int>();
            int childID = GetDecryptParam("childID", "0").ToNumber<int>();
            int type = GetDecryptParam("type", "1").ToNumber<int>();
            int joinType = GetDecryptParam("joinType", "2").ToNumber<int>();

            BE.ComInfo info = new BE.ComInfo();
            info.CompanyID = CurrentUserInfo.CompanyID;
            info.CommunityID = communityID;
            info.Name = name;
            info.Description = description;
            info.Question = question;
            info.Answer = answer;
            info.ParentID = parentID;
            info.ChildID = childID;
            info.Type = type;
            info.JoinType = joinType;

            DAL.Com dal = new DAL.Com();

            if (!dal.Mod(info))
            {
                return false;
            }

            return true;
        }

        #endregion 
    }
}
