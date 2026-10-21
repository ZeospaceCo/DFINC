using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Services;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Account.Web.Ws.Common
{
    /// <summary>
    /// GetUserDept의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다.
    [System.Web.Script.Services.ScriptService]
    public class GetUserDept : Base.BaseWSAccount
    {
        /// <summary>
        /// 부서 및 이름 검색
        /// 심재운 2014.11.05
        /// </summary>
        /// <param name="companyID">The company identifier.</param>
        /// <param name="gubun">The gubun.</param>
        /// <param name="searchName">Name of the search.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<GetDeptUserSearchEntity> GetDeptUserSearch(string companyID, string gubun, string searchName)
        {
            this.ResourceName = "Zeospace.DeskPlus.Account.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Account.Web.Ws.Common.GetUserDept).Assembly;	//어셈블리 선언

            //이름,부서,이메일 검색 키워드
            searchName = searchName.Trim();

            List<GetDeptUserSearchEntity> obj = new List<GetDeptUserSearchEntity>();
            if (searchName.Length == 0) return obj;

            if (companyID.Length == 0) companyID = CurrentUserInfo.CompanyID.ToString();

            Zeospace.DeskPlus.Account.BLL.Dept dept = new Zeospace.DeskPlus.Account.BLL.Dept();
            Account.DAL.UserDept biz = new DAL.UserDept();
            DataSet ds = biz.GetDeptUserByName(companyID.ToNumber<int>(), searchName, true);

            if (ds != null)
            {
                string name = "";
                string values = "";
                string deptName = "";
                string titleName = "";

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (dr["BaseDept"].ToString() == "0" && dr["Type"].ToString() == "M") continue;

                    name = this.GetLangName(dr["NameBase"].ToString(), dr["NameKo"].ToString(), dr["NameEn"].ToString(), dr["NameJa"].ToString(),
                            dr["NameFr"].ToString(), dr["NameZhCn"].ToString(), dr["NameZhTw"].ToString(), PreLang).ToReverseXSS();

                    values = name;

                    titleName = this.GetLangName(dr["TitleBase"].ToString(), dr["TitleKo"].ToString(), dr["TitleEn"].ToString(), dr["TitleJa"].ToString(),
                        dr["TitleFr"].ToString(), dr["TitleZhCn"].ToString(), dr["TitleZhTw"].ToString(), PreLang).ToReverseXSS();

                    deptName = this.GetLangName(dr["DeptBase"].ToString(), dr["DeptKo"].ToString(), dr["DeptEn"].ToString(), dr["DeptJa"].ToString(),
                        dr["DeptFr"].ToString(), dr["DeptZhCn"].ToString(), dr["DeptZhTw"].ToString(), PreLang).ToReverseXSS();

                    if (dr["Type"].ToString() == "M") name = name + " " + titleName + " (" + deptName + ")"; //이름 표기

                    if (dr["Type"].ToString() == "G" && gubun == "G")
                    {
                        obj.Add(new GetDeptUserSearchEntity()
                        {
                            Name = name,
                            Values = values
                        });
                    }
                    else if (dr["Type"].ToString() == "M" && gubun == "M")
                    {
                        obj.Add(new GetDeptUserSearchEntity()
                        {
                            Name = name,
                            Values = values
                        });
                    }
                    else if (gubun == "")
                    {
                        //부서표기 (이름과 구분)
                        if (dr["Type"].ToString() == "G") name = name + " (" + GetResource("Private_AccountInfo_ltUserDept") + ")";

                        obj.Add(new GetDeptUserSearchEntity()
                        {
                            Name = name,
                            Values = values
                        });
                    }
                }
            }

            return obj;
        }
    }

    [Serializable]
    public class GetDeptUserSearchEntity
    {
        public string Name { get; set; }

        public string Values { get; set; }
    }
}