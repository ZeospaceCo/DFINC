using System;
using System.Text.RegularExpressions;
using System.Web.Services;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Board.BE;

namespace Zeospace.DeskPlus.Board.Web.Ws
{
    /// <summary>
    /// SetBoard의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다.
    [System.Web.Script.Services.ScriptService]
    public class SetBoard : Base.BaseWSBoard
    {
        #region ADD()

        #region 임시글 이동하기

        /// <summary>
        /// 임시글을 복사하기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool MoveMyTemp(string p)
        {
            SetDecryptString(p);
            string categoryID = GetDecryptParam("categoryID", "0");
            string listID = GetDecryptParam("listID", "0");

            BLL.MyTemplate bll = new BLL.MyTemplate();
            bll.Move(CurrentUserInfo.CompanyID, categoryID.ToNumber<int>(), listID.ToNumber<int>(),
                CurrentUserInfo.DeptID, CurrentUserInfo.UserID, CurrentUserInfo.LogInID,
                CurrentUserInfo.Name, CurrentUserInfo.MobilePhone, PreLang);

            return true;
        }

        #endregion 임시글 이동하기

        #region 댓글

        /// <summary>
        /// 댓글 ADD
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int AddShortReply(string p)
        {
            string categoryID = string.Empty;
            string listID = string.Empty;
            string contents = string.Empty;

            SetDecryptString(p);
            categoryID = GetDecryptParam("categoryID", "0");
            listID = GetDecryptParam("listID", "0");
            contents = GetDecryptParam("contents", "");
            contents = Regex.Replace(contents, @"<(.|\n)*?>", string.Empty);

            BE.ShortReplyInfo info = new BE.ShortReplyInfo();

            info.CompanyID = CurrentUserInfo.CompanyID;
            info.CategoryID = categoryID.ToNumber<int>();
            info.ListID = listID.ToNumber<int>();
            info.Content = contents;
            info.UserID = CurrentUserInfo.UserID;

            DAL.ShortReply bsr = new DAL.ShortReply();
            return bsr.Add(info);
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 투표 추가
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int AddRating(string p)
        {
            SetDecryptString(p);
            string categoryID = GetDecryptParam("categoryID", "0");
            string listID = GetDecryptParam("listID", "0");
            string rating = GetDecryptParam("rating", "");
            string contents = GetDecryptParam("contents", "");

            rating = rating.Replace("rating", "");

            BE.RatingInfo info = new BE.RatingInfo();
            info.CompanyID = CurrentUserInfo.CompanyID;
            info.CategoryID = categoryID.ToNumber<int>();
            info.ListID = listID.ToNumber<int>();
            info.RatingID = 0;
            info.Rating = rating.ToNumber<int>();
            info.WriteUserID = CurrentUserInfo.UserID;
            //info.WriteUserName = CurrentUserInfo.Name;
            info.Comments = contents;

            DAL.Ratings biz = new DAL.Ratings();
            return biz.Add(info);
        }

        #endregion 댓글

        #region 즐겨찾기

        /// <summary>
        /// 작성자 : 심재운
        /// 용도 : 게시판 폴더를 즐겨찾기에 넣기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool AddFavoriteCategory(string p)
        {
            SetDecryptString(p);

            string parentFavoriteID = GetDecryptParam("parentFavoriteID", "0");
            string categoryID = GetDecryptParam("categoryID", "0");

            DAL.Favorite dal = new DAL.Favorite();
            return dal.AddFavoriteCategory(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID,
                categoryID.ToNumber<int>(), parentFavoriteID.ToNumber<int>());
        }

        #endregion 즐겨찾기

        #region 말머리

        /// <summary>
        /// 말머리 추가
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool AddHeader(string p)
        {
            try
            {
                SetDecryptString(p);
                int categoryID = GetDecryptParam("categoryID", "0").ToNumber<int>();
                int headerID = GetDecryptParam("headerID", "0").ToNumber<int>();
                string name = GetDecryptParam("name", "");

                HeaderInfo be = new HeaderInfo();
                be.CompanyID = CurrentUserInfo.CompanyID;
                be.CategoryID = categoryID;
                be.HeaderID = headerID;
                be.NameBase = name;
                be.Orders = 0;
                be.UseActive = "1";
                be.WriteUserID = CurrentUserInfo.UserID;

                DAL.Header dal = new DAL.Header();
                dal.AddMod(be);

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("AddHeader : " + ex.Message);
                throw;
            }
        }

        #endregion 말머리

        #endregion ADD()

        #region MOD()

        #region 이동

        /// <summary>
        /// 작성자 : 심재운
        /// 용도 : 게시물 이동
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool Move(string p)
        {
            try
            {
                SetDecryptString(p);
                int toCateogryID = GetDecryptParam("toCateogryID", "0").ToNumber<int>(); //선택한 카테고리
                string listID = GetDecryptParam("listID", "").TrimEnd(',');//선택한 리스트들 구분자 콤마

                if (listID.Length > 0 && toCateogryID > 0)
                {
                    DAL.Lists dal = new DAL.Lists();
                    dal.Move(CurrentUserInfo.CompanyID, listID, toCateogryID);
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

        #region 댓글

        /// <summary>
        /// 댓글 Mod
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool ModShortReple(string p)
        {
            string categoryID = string.Empty;
            string listID = string.Empty;
            string contents = string.Empty;
            string shortReplyID = string.Empty;
            contents = Regex.Replace(contents, @"<(.|\n)*?>", string.Empty);

            SetDecryptString(p);
            categoryID = GetDecryptParam("categoryID", "0");
            listID = GetDecryptParam("listID", "0");
            contents = GetDecryptParam("contents", "");
            shortReplyID = GetDecryptParam("shortReplyID", "0");

            BE.ShortReplyInfo info = new BE.ShortReplyInfo();

            info.CompanyID = CurrentUserInfo.CompanyID;
            info.CategoryID = categoryID.ToNumber<int>();
            info.ListID = listID.ToNumber<int>();
            info.Content = contents;
            info.ShortReplyID = shortReplyID.ToNumber<int>();
            info.UserID = CurrentUserInfo.UserID;

            DAL.ShortReply biz = new DAL.ShortReply();
            return biz.Mod(info);
        }

        #endregion 댓글

        #region 카테고리

        /// <summary>
        /// 작성자 : 심재운
        /// 용도 : 게시판 폴더 순서 변경하기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool ModOrderCategory(string p)
        {
            SetDecryptString(p);
            string sortCatList = GetDecryptParam("sortCatList", ""); //카테고리값
            sortCatList = sortCatList.TrimEnd(',');

            DAL.Category dal = new DAL.Category();
            return dal.ModOrderCategory(CurrentUserInfo.CompanyID, sortCatList);
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 용도 : 게시판 폴더 이동
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool MoveCategory(string p)
        {
            SetDecryptString(p);
            int categoryID = GetDecryptParam("categoryID", "0").ToNumber<int>(); //원본 카테고리
            int toCategoryID = GetDecryptParam("toCategoryID", "0").ToNumber<int>();//북사할 카테고리

            if (categoryID > 0)
            {
                DAL.Category dal = new DAL.Category();
                return dal.ModMoveCategory(CurrentUserInfo.CompanyID, categoryID, toCategoryID);
            }
            else
            {
                return false;
            }
        }

        #endregion 카테고리

        #region 즐겨찾기

        /// <summary>
        /// 작성자 : 심재운
        /// 용도 : 즐겨찾기 폴더 순서 변경하기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool ModOrderFavorite(string p)
        {
            SetDecryptString(p);
            string sortCatList = GetDecryptParam("sortCatList", ""); //카테고리값
            sortCatList = sortCatList.TrimEnd(',');

            DAL.Favorite dal = new DAL.Favorite();
            return dal.ModOrderFavorite(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, sortCatList);
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 목적 : 상위폴더 수정
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool MoveFavorite(string p)
        {
            SetDecryptString(p);
            int favoriteID = GetDecryptParam("favoriteID", "0").ToNumber<int>(); //원본 카테고리
            int parentFavoriteID = GetDecryptParam("parentFavoriteID", "0").ToNumber<int>();//북사할 카테고리

            if (favoriteID > 0)
            {
                DAL.Favorite dal = new DAL.Favorite();
                return dal.ModParentFavorite(CurrentUserInfo.CompanyID, favoriteID, parentFavoriteID,
                    CurrentUserInfo.UserID);
            }
            else
            {
                return false;
            }
        }

        #endregion 즐겨찾기

        #region 카테고리별 사용자정보

        /// <summary>
        /// 용도 : 카테고리별 사용자정보 설정하기
        /// 작성자 : 심재운
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool ModCategoryUser(string p)
        {
            try
            {
                SetDecryptString(p);

                string categoryID = GetDecryptParam("catID", "0");
                string listType = GetDecryptParam("listType", "0");
                string pageSize = GetDecryptParam("pageSize", "0");
                string types = GetDecryptParam("types", "BL"); //BL : 게시판 TL : 임시저장 IL : 중요표시

                BE.CategoryUserInfo info = new CategoryUserInfo();
                info.CompanyID = CurrentUserInfo.CompanyID;
                info.CategoryID = categoryID.ToNumber<int>();
                info.UserID = CurrentUserInfo.UserID;
                info.PageSize = pageSize.ToNumber<int>();
                info.ListType = listType;
                info.Types = types;

                DAL.CategoryUser dal = new DAL.CategoryUser();
                dal.AddMod(info);

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

        #endregion 카테고리별 사용자정보

        #region 중요표시

        /// <summary>
        /// 작성자 : 심재운
        /// 용도 : 중요표시 추가
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool AddImpUser(string p)
        {
            try
            {
                SetDecryptString(p);
                string listID = GetDecryptParam("listID", "").TrimEnd(',');//listid,listid

                if (listID.Length > 0)
                {
                    DAL.ImpoUser dal = new DAL.ImpoUser();
                    foreach (string id in listID.Split(','))
                    {
                        BE.ImpoUserInfo info = new BE.ImpoUserInfo();
                        info.CompanyID = CurrentUserInfo.CompanyID;
                        info.ListID = id.ToNumber<int>();
                        info.UserID = CurrentUserInfo.UserID;

                        if (info.ListID > 0)
                        {
                            dal.Add(info);
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("AddImpUser : " + ex.Message);
                throw;
            }
        }

        #endregion 중요표시

        #endregion MOD()

        #region DEL()

        #region 리스트

        /// <summary>
        /// 리스트 삭제
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool DelList(string p)
        {
            SetDecryptString(p);
            string listID = GetDecryptParam("listID", "0");

            if (listID.Length > 0)
            {
                DAL.Lists biz = new DAL.Lists();
                Zeospace.DeskPlus.Common.BLL.CreatePDF pdfBiz = new DeskPlus.Common.BLL.CreatePDF();
                foreach (string list in listID.Split(','))
                {
                    biz.Del(CurrentUserInfo.CompanyID, list.ToNumber<int>());

                    //pdf삭제
                    pdfBiz.DelPdf(CurrentUserInfo.CompanyID, "BOARD", list.ToNumber<int>());
                }
            }

            return true;
        }

        #endregion 리스트

        #region 댓글

        /// <summary>
        /// 댓글 DEL
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool DelShortReply(string p)
        {
            SetDecryptString(p);
            string categoryID = GetDecryptParam("categoryID", "0");
            string listID = GetDecryptParam("listID", "0");
            string shortReplyID = GetDecryptParam("ShortReplyID", "0");
            string groupID = GetDecryptParam("groupID", "0");

            DAL.ShortReply biz = new DAL.ShortReply();
            BE.ShortReplyInfo info = biz.GetShortReplyListID(CurrentUserInfo.CompanyID, categoryID.ToNumber<int>(),
                listID.ToNumber<int>(), shortReplyID.ToNumber<int>());

            if (info.UserID == CurrentUserInfo.UserID)
            {
                return biz.Del(CurrentUserInfo.CompanyID,
                    categoryID.ToNumber<int>(),
                    listID.ToNumber<int>(),
                    shortReplyID.ToNumber<int>(),
                    groupID.ToNumber<int>());
            }
            else
            {
                return false;
            }
        }

        #endregion 댓글

        #region 첨부파일

        /// <summary>
        /// 작성자 : 심재운
        /// 용도 : 첨부파일 id 별 삭제하기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool DelAttachFile(string p)
        {
            SetDecryptString(p);
            int categoryID = GetDecryptParam("categoryID", "0").ToNumber<int>();
            int listID = GetDecryptParam("listID", "0").ToNumber<int>();
            int attachFileID = GetDecryptParam("attachFileID", "0").ToNumber<int>();

            BLL.AttachFile attachBiz = new BLL.AttachFile();

            attachBiz.Del(CurrentUserInfo.CompanyID, categoryID, listID, attachFileID);

            return true;
        }

        #endregion 첨부파일

        #region 중요표시

        /// <summary>
        /// 작성자 : 심재운
        /// 용도 : 중요표시 삭제
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool DelImpUser(string p)
        {
            try
            {
                SetDecryptString(p);
                string listID = GetDecryptParam("listID", "").TrimEnd(',');//listid,listid

                if (listID.Length > 0)
                {
                    DAL.ImpoUser dal = new DAL.ImpoUser();

                    foreach (string id in listID.Split(','))
                    {
                        BE.ImpoUserInfo info = new BE.ImpoUserInfo();
                        info.CompanyID = CurrentUserInfo.CompanyID;
                        info.ListID = id.ToNumber<int>();
                        info.UserID = CurrentUserInfo.UserID;

                        dal.Del(info);
                    }
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("DelImpUser : " + ex.Message);
                throw;
            }

            return true;
        }

        #endregion 중요표시

        #region 말머리

        /// <summary>
        /// 말머리 삭제
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool DelHeader(string p)
        {
            try
            {
                SetDecryptString(p);
                int categoryID = GetDecryptParam("categoryID", "0").ToNumber<int>();
                int headerID = GetDecryptParam("headerID", "0").ToNumber<int>();

                HeaderInfo be = new HeaderInfo();
                be.CompanyID = CurrentUserInfo.CompanyID;
                be.CategoryID = categoryID;
                be.HeaderID = headerID;

                DAL.Header dal = new DAL.Header();
                dal.Del(be);

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("DelHeader : " + ex.Message);
                throw;
            }
        }

        #endregion 말머리

        #region 임시저장

        /// <summary>
        /// 작성자 : 심재운
        /// 용도 : 임시저장 첨부파일 id 별 삭제하기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool DelMyAttachFile(string p)
        {
            SetDecryptString(p);
            string categoryID = GetDecryptParam("categoryID", "0");
            string listID = GetDecryptParam("listID", "0");
            string attachFileID = GetDecryptParam("attachFileID", "0");

            BLL.MyAttachFile attachBiz = new BLL.MyAttachFile();

            attachBiz.Del(CurrentUserInfo.CompanyID, categoryID.ToNumber<int>(),
                listID.ToNumber<int>(), attachFileID.ToNumber<int>());

            return true;
        }

        #endregion 임시저장

        #endregion DEL()
    }
}