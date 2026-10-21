using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Zeospace.DeskPlus.EIP.BE;
using Zeospace.DeskPlus.EIP.DAL;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Data;

namespace Zeospace.DeskPlus.EIP.Web.Ws.Portal
{
    /// <summary>
    /// AddMenu의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class AddMenu : Base.BaseWSEIP
    {

        /// <summary>
        /// 작성자 : 심재운
        /// 용도 : 게시판 모듈의 메뉴 생성하기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool AddMenuByBoard(string p)
        {
            SetDecryptString(p);

            int menuID = GetDecryptParam("menuID", "0").ToNumber<int>();
            int categoryID = GetDecryptParam("categoryID", "0").ToNumber<int>();

            if (categoryID > 0)
            {
                //통합게시판 회사아이디
                int boaComID = Zeospace.DeskPlus.Board.BLL.Category.GetCompanyCategoryInfo(categoryID);

                Board.DAL.Category catDal = new Board.DAL.Category();
                DataSet ds = catDal.GetCategory(boaComID, categoryID);

                if (ds != null)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        Zeospace.DeskPlus.EIP.BE.MenuInfo info = new MenuInfo();

                        info.CompanyID = CurrentUserInfo.CompanyID;
                        info.MenuID = 0;
                        info.ParentMenuID = menuID;
                        info.MenuOrder = 0;
                        info.NameBase = dr["NameBase"].ToString();
                        info.NameKo = dr["NameKo"].ToString();
                        info.NameEn = dr["NameEn"].ToString();
                        info.NameJa = dr["NameJa"].ToString();
                        info.NameFr = dr["NameFr"].ToString();
                        info.NameZhcn = dr["NameZhcn"].ToString();
                        info.NameZhtw = dr["NameZhtw"].ToString();

                        info.MenuContent = dr["NameBase"].ToString();
                        info.DirectCall = "BOARD" + categoryID.ToString();
                        info.TargetURL = "/DeskPlusEIP/Board/Default.aspx";
                        info.OpenURL = "1"; //하단프레임
                        info.SendMethod = "1"; //post

                        Zeospace.DeskPlus.EIP.DAL.Menu dal = new Zeospace.DeskPlus.EIP.DAL.Menu();
                        int iMenuID = dal.Add(info);

                        Zeospace.DeskPlus.EIP.BE.MenuDataLinkInfo menudataInfo = new MenuDataLinkInfo();
                        Zeospace.DeskPlus.EIP.DAL.MenuDataLink menudataDal = new MenuDataLink();

                        //게시판 제일 상단 트리 설정
                        menudataInfo.CompanyID = CurrentUserInfo.CompanyID;
                        menudataInfo.MenuID = iMenuID;
                        menudataInfo.EncriptType = "1";
                        menudataInfo.EncriptValue = "1";
                        menudataInfo.LinkName = "menuCatID";
                        menudataInfo.LinkValue = categoryID.ToString();
                        menudataInfo.LinkType = "OU";
                        menudataInfo.Description = "top tree of BOARD ID";
                        menudataDal.Add(menudataInfo);

                        //게시판 타켓 트리 설정
                        menudataInfo.LinkName = "catID";
                        menudataInfo.Description = "target tree of BOARD ID";
                        menudataDal.Add(menudataInfo);

                        //user 권한 할당하기
                        Zeospace.DeskPlus.EIP.DAL.Role dalRole = new Role();
                        List<RoleInfo> alRole = dalRole.GetRole(CurrentUserInfo.CompanyID, "user", "1");
                        int roleID = 0;
                        foreach (RoleInfo infoRole in alRole) roleID = infoRole.RoleID;

                        List<Zeospace.DeskPlus.EIP.BE.MenuRoleInfo> al = new List<Zeospace.DeskPlus.EIP.BE.MenuRoleInfo>();
                        Zeospace.DeskPlus.EIP.BE.MenuRoleInfo authInfo = new MenuRoleInfo();
                        authInfo.CompanyID = CurrentUserInfo.CompanyID;
                        authInfo.MenuID = iMenuID;
                        authInfo.RoleID = roleID;

                        al.Add(authInfo);
                        Zeospace.DeskPlus.EIP.BLL.MenuRole bizMenuRole = new Zeospace.DeskPlus.EIP.BLL.MenuRole();
                        bizMenuRole.AddTx(al);

                        //전체 메뉴, 데이터연동, IP 초기화 하기
                        Zeospace.DeskPlus.EIP.BLL.Menu bizMenu = new Zeospace.DeskPlus.EIP.BLL.Menu();
                        bizMenu.ResetMenuAuth(CurrentUserInfo.CompanyID);
                    }
                }
            }

            return true;
        }
    }
}
