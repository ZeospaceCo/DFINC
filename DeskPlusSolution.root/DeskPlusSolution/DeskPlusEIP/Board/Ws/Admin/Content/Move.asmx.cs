using System;
using System.Web.Services;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Board.Web.Ws.Admin.Content
{
    /// <summary>
    /// Move의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다.
    [System.Web.Script.Services.ScriptService]
    public class Move : Base.BaseWSBoard
    {
        #region Mod()

        #region 이동

        /// <summary>
        /// 작성자 : 심재운
        /// 용도 : 게시물 이동
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool MoveList(string p)
        {
            try
            {
                SetDecryptString(p);
                int toCateogryID = GetDecryptParam("toCateogryID", "0").ToNumber<int>(); //선택한 카테고리
                string listID = GetDecryptParam("listID", "").TrimEnd(',');//선택한 리스트들 구분자 콤마

                if (listID.Length > 0 && toCateogryID > 0)
                {
                    //통합게시판 회사아이디
                    int boaComID = BLL.Category.GetCompanyCategoryInfo(toCateogryID);
                    DAL.Lists dal = new DAL.Lists();
                    dal.Move(boaComID, listID, toCateogryID);
                }

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Move : " + ex.Message);
                throw;
            }
        }

        #endregion 이동

        #endregion Mod()
    }
}