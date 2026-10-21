using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using System.Collections.Specialized;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Board.BE;

namespace Zeospace.DeskPlus.Portlet.Web.Ws.Package
{
    /// <summary>
    /// TabMain의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class TabMain : Base.BaseWSPortlet
    {

        string Module = "";
        string CatID = "";

        /// <summary>
        /// 작성자 : 심재운
        /// 모듈별 총 카운트 가져오기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetPageTotCount(string p)
        {
            try
            {
                //파라미터 가져오기
                SetDecryptString(p);
                Module = GetDecryptParam("module", "");
                CatID = GetDecryptParam("catID", "0");

                int cnt = 10;

                switch (Module)
                {
                    case "board" :
                        cnt = GetBoardCount();
                        break;
                    case "mail" :
                        cnt = 10; //받은 제타메일
                        break;
                    case "mailbox":
                        cnt = 10; //나의특정메일박스 제타메일
                        break;
                    case "sendmail":
                        cnt = 10; //보낸 제타메일
                        break;
                    case "confirmMail":
                        cnt = 10; //승인확인 제타메일
                        break;
                }

                return cnt;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 게시판 카운트
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private int GetBoardCount()
        {
            try
            {
                Zeospace.DeskPlus.Board.DAL.Category dalCat = new DeskPlus.Board.DAL.Category();
                DataSet dsCat = dalCat.GetCategory(CurrentUserInfo.CompanyID, CatID.ToNumber<int>());

                string optionUseDept = "";//부서별게시판여부
                string optionUseView = "";//보기용 여부

                foreach (DataRow row in dsCat.Tables[0].Rows)
                {
                    optionUseDept = row["OptionUseDept"].ToString();//부서별게시판여부
                    optionUseView = row["OptionUseView"].ToString();//보기용 여부
                }
                
                string auth = GetDecryptParam("auth", ""); //관리자 : 1; 일반인 : 0

                int icnt = 0;                
                bool isChild = (optionUseView == "1") ? true : false; //보기용이면 하위검색가능

                string myAuthCatList = "";

                if (CatID != "0" && CatID.Length > 0)
                {
                    if (auth == "1")
                    {
                        myAuthCatList = CatID.ToString();

                        if (isChild)
                        {
                            Zeospace.DeskPlus.Board.BLL.Category catBiz = new DeskPlus.Board.BLL.Category();
                            myAuthCatList = catBiz.GetChildCategory(CurrentUserInfo.CompanyID, CatID.ToNumber<int>());
                        }
                    }
                    else
                    {
                        myAuthCatList = GetMyAuthCatList(CatID.ToNumber<int>(),optionUseView, optionUseDept, isChild);
                    }
                }
                else
                {
                    myAuthCatList = "-1"; //카테고리 값이 0 넘어오면 -1 로 해서 리스트 안나오게 함
                }

                if (myAuthCatList.Length == 0) myAuthCatList = "-1";


                Zeospace.DeskPlus.Board.DAL.Lists biz = new DeskPlus.Board.DAL.Lists();
                icnt = biz.GetPageListCount(CurrentUserInfo.CompanyID,
                    myAuthCatList,
                    "", "",
                    "", "", optionUseDept, CurrentUserInfo.DeptID);

                return icnt;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 권한별 카테고리 리스트 가져오기
        /// </summary>
        /// <param name="_categoryID">The _category ID.</param>
        /// <param name="_optionUseView">The _option use view.</param>
        /// <param name="_optionUseDept">The _option use dept.</param>
        /// <param name="_useChild">if set to <c>true</c> [_use child].</param>
        /// <returns></returns>
        private string GetMyAuthCatList(int _categoryID, string _optionUseView, string _optionUseDept,  bool _useChild)
        {
            try
            {
                #region 내가 볼 수 있는 하위 카테고리
                NameValueCollection nvc = new NameValueCollection();
                string myAuthCatList = "";// _categoryID.ToString() + ",";
                Zeospace.DeskPlus.Board.BLL.Category biz = new DeskPlus.Board.BLL.Category();

                //보기용은 하위카테고리까지 가져옴
                if (_optionUseView == "1")
                {
                    nvc = biz.GetMyAuthCategory(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID,
                        CurrentUserInfo.DeptID, _categoryID, _optionUseDept, true, BoardAuth.Read, true, false);

                    foreach (string s in nvc.AllKeys) myAuthCatList += s + ",";
                }
                else
                {
                    nvc = biz.GetMyAuthCategory(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID,
                        CurrentUserInfo.DeptID, _categoryID, _optionUseDept, _useChild, BoardAuth.Read, true, false);

                    foreach (string s in nvc.AllKeys) myAuthCatList += s + ",";
                }

                myAuthCatList = myAuthCatList.TrimEnd(',');
                #endregion

                return myAuthCatList;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }
    }
}
