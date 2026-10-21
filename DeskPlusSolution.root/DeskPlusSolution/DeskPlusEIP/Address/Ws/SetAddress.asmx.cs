using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;
using System.Text.RegularExpressions;

namespace Zeospace.DeskPlus.Address.Web.Ws
{
    /// <summary>
    /// SetAddress의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class SetAddress : Base.BaseWSAddress
    {

        #region ADD

        /// <summary>
        /// 작성자 : 심재운
        /// 개인주소록 복사하기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool CopyPrivate(string p)
        {
            try
            {
                SetDecryptString(p);
                int categoryID = GetDecryptParam("categoryID", "0").ToNumber<int>(); //원본 카테고리
                string listID = GetDecryptParam("listID", ""); //주소록
                string types = GetDecryptParam("types", ""); //타입
                int toCategoryID = GetDecryptParam("toCategoryID", "0").ToNumber<int>();//북사할 카테고리

                if (listID.Length > 0 && types.Length > 0 && toCategoryID > 0)
                {
                    DAL.Private dal = new DAL.Private();
                    return dal.AddCopy(CurrentUserInfo.CompanyID, categoryID, CurrentUserInfo.UserID,
                        toCategoryID, listID, types);
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("CopyPrivate : " + ex.Message);
                throw;
            }

        }

        /// <summary>
        /// 개인주소록 - 간편주소록 추가
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int AddPrivate(string p)
        {
            try
            {
                SetDecryptString(p);
                int categoryID = GetDecryptParam("categoryID", "0").ToNumber<int>();
                string types = GetDecryptParam("types", "");
                string name = GetDecryptParam("name", "");
                string mail = GetDecryptParam("mail", "");
                string mobile = GetDecryptParam("mobile", "");
                mobile = mobile.Replace("plus", "+");

                //개인주소록만 입력가능하다.
                if (categoryID == 0 || types != "PG") return 0;

                BE.PrivateInfo info = new BE.PrivateInfo();

                info.CompanyID = CurrentUserInfo.CompanyID;
                info.UserID = CurrentUserInfo.UserID;
                info.LoginID = CurrentUserInfo.LogInID;
                info.CategoryID = categoryID;
                info.PrivateID = 0;
                info.LastName = name;
                info.WholeName = name;
                info.NameBase = name; //기본언어
                info.Mail = mail;
                info.MobilePhone = mobile;
                info.IsReceive = "0";
                info.SendUserID = 0;

                DAL.Private dal = new DAL.Private();
                return dal.Add(info);
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("AddPrivate : " + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 공용주소록 - 간편주소록 추가
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int AddCommon(string p)
        {
            try
            {
                SetDecryptString(p);
                int categoryID = GetDecryptParam("categoryID", "0").ToNumber<int>();
                string types = GetDecryptParam("types", "");
                string name = GetDecryptParam("name", "");
                string mail = GetDecryptParam("mail", "");
                string mobile = GetDecryptParam("mobile", "");
                mobile = mobile.Replace("plus", "+");

                //개인주소록만 입력가능하다.
                if (categoryID == 0 || types != "CG") return 0;

                BE.CommonInfo info = new BE.CommonInfo();

                info.CompanyID = CurrentUserInfo.CompanyID;
                info.CategoryID = categoryID;
                info.CommonID = 0;
                info.LastName = name;
                info.WholeName = name;
                info.NameBase = name; //기본언어
                info.Mail = mail;
                info.MobilePhone = mobile;
                info.WriteUserID = CurrentUserInfo.UserID;

                DAL.Common dal = new DAL.Common();
                return dal.Add(info);
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("AddCommon : " + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 즐겨찾기 그룹 추가
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>        
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool AddFavoCategory(string p)
        {
            try
            {
                SetDecryptString(p);
                int ParentcategoryID = GetDecryptParam("parentCategoryID", "0").ToNumber<int>();
                int FolderID = GetDecryptParam("folderID", "0").ToNumber<int>();
                string Types = GetDecryptParam("types", "");

                if (ParentcategoryID < 0 || FolderID < 0 || Types == "")
                    return false;

                BE.FavorCategoryInfo info = new BE.FavorCategoryInfo();

                info.CompanyID = CurrentUserInfo.CompanyID;
                info.ParentCategoryID = ParentcategoryID;
                info.UserID = CurrentUserInfo.UserID;
                info.FolderID = FolderID;
                info.Types = Types;

                DAL.FavorCategory dal = new DAL.FavorCategory();

                if (dal.Add(info) > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("AddFavoCategory : " + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 작성자 : shine
        /// 전달받은 주소록으로 추가
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool AddPrivateReceived(string p)
        {
            try
            {
                SetDecryptString(p);
                string UserIDs = GetDecryptParam("userID", "");    // 주소록을 받을 사람
                string ListID = GetDecryptParam("listID", "");     // 주소록
                bool response = true;

                if (UserIDs == "" || ListID == "")
                    return false;

                DAL.Private dal = new DAL.Private();

                string[] ArrUserID = UserIDs.Split(',');

                foreach (string UserID in ArrUserID)
                {
                    if (!dal.AddSend(CurrentUserInfo.CompanyID, UserID.ToNumber<int>(), CurrentUserInfo.UserID, ListID))
                    {
                        response = false;
                        break;
                    }
                }

                return response;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("AddPrivateReceived : " + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 개인주소록 복사하기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool CopyCommon(string p)
        {
            try
            {
                SetDecryptString(p);
                int categoryID = GetDecryptParam("categoryID", "0").ToNumber<int>(); //원본 카테고리
                string listID = GetDecryptParam("listID", ""); //주소록
                string types = GetDecryptParam("types", ""); //타입
                int toCategoryID = GetDecryptParam("toCategoryID", "0").ToNumber<int>();//북사할 카테고리

                if (listID.Length > 0 && types.Length > 0 && toCategoryID > 0)
                {
                    DAL.Common dal = new DAL.Common();
                    return dal.AddCopy(CurrentUserInfo.CompanyID, categoryID, toCategoryID, listID, types);
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("CopyCommon : " + ex.Message);
                throw;
            }

        }

        
        
        #endregion

        #region MOD

        /// <summary>        
        /// 작성자 : 심재운
        /// 용도 : 주소록 순서 변경하기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool ModOrderCategory(string p)
        {
            try
            {
                SetDecryptString(p);
                string sortCatList = GetDecryptParam("sortCatList", ""); //카테고리값
                string types = GetDecryptParam("types", ""); //주소록 구분자
                sortCatList = sortCatList.TrimEnd(',');

                if (types == "CG" && sortCatList.Length > 0)
                {
                    DAL.CommonCategory dal = new DAL.CommonCategory();
                    return dal.ModOrder(CurrentUserInfo.CompanyID, sortCatList);
                }
                else if (types == "PG" && sortCatList.Length > 0)
                {
                    DAL.PrivateCategory dal = new DAL.PrivateCategory();
                    return dal.ModOrder(CurrentUserInfo.CompanyID, sortCatList,
                        CurrentUserInfo.UserID);
                }
                else if (types == "FG" && sortCatList.Length > 0)
                {
                    DAL.FavorCategory dal = new DAL.FavorCategory();
                    return dal.ModOrder(CurrentUserInfo.CompanyID, sortCatList,
                        CurrentUserInfo.UserID);
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("ModOrderCategory : " + ex.Message);
                throw;
            }

        }

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

                string categoryID = GetDecryptParam("categoryID", "0");
                string types = GetDecryptParam("types", "");
                string listType = GetDecryptParam("listType", "");
                string pageSize = GetDecryptParam("pageSize", "0");

                BE.CategoryUserInfo info = new BE.CategoryUserInfo();
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
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("ModCategoryUser : " + ex.Message);
                throw;
            }

        }

        /// <summary>        
        /// 작성자 : 심재운
        /// 용도 : 개인 주소록 이동
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool MovePrivate(string p)
        {
            try
            {
                SetDecryptString(p);
                int categoryID = GetDecryptParam("categoryID", "0").ToNumber<int>(); //원본 카테고리
                string listID = GetDecryptParam("listID", ""); //주소록            
                int toCategoryID = GetDecryptParam("toCategoryID", "0").ToNumber<int>();//북사할 카테고리

                if (listID.Length > 0 && toCategoryID > 0)
                {
                    DAL.Private dal = new DAL.Private();
                    return dal.ModMove(CurrentUserInfo.CompanyID, categoryID, CurrentUserInfo.UserID,
                        toCategoryID, listID);
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("MovePrivate : " + ex.Message);
                throw;
            }

        }

        /// <summary>        
        /// 작성자 : 심재운
        /// 용도 : 공용 주소록 이동
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool MoveCommon(string p)
        {
            try
            {
                SetDecryptString(p);
                int categoryID = GetDecryptParam("categoryID", "0").ToNumber<int>(); //원본 카테고리
                string listID = GetDecryptParam("listID", "");
                string types = GetDecryptParam("types", "");
                int toCategoryID = GetDecryptParam("toCategoryID", "0").ToNumber<int>();//북사할 카테고리

                if (listID.Length > 0 && toCategoryID > 0)
                {
                    DAL.Common dal = new DAL.Common();
                    return dal.ModMove(CurrentUserInfo.CompanyID, categoryID, toCategoryID, listID, types);
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("MoveCommon : " + ex.Message);
                throw;
            }

        }

        /// <summary>        
        /// 작성자 : 심재운
        /// 용도 : 공용 주소록 이동
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool MoveCommonCategory(string p)
        {
            try
            {
                SetDecryptString(p);
                int categoryID = GetDecryptParam("categoryID", "0").ToNumber<int>(); //원본 카테고리            
                int toCategoryID = GetDecryptParam("toCategoryID", "0").ToNumber<int>();//북사할 카테고리

                if (categoryID > 0)
                {
                    DAL.CommonCategory dal = new DAL.CommonCategory();
                    return dal.ModMoveCategory(CurrentUserInfo.CompanyID, categoryID, toCategoryID);
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("MoveCommonCategory : " + ex.Message);
                throw;
            }
        }

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
                string ownerID = GetDecryptParam("ownerID", "0").TrimEnd(',');//키|구분자,키|구분자
                DAL.ImpoUser dal = new DAL.ImpoUser();

                foreach (string id in ownerID.Split(','))
                {
                    BE.ImpoUserInfo info = new BE.ImpoUserInfo();
                    info.CompanyID = CurrentUserInfo.CompanyID;
                    info.OwnerID = id.Split('|')[0].ToNumber<int>();
                    info.UserID = CurrentUserInfo.UserID;
                    info.Types = id.Split('|')[1];

                    if (info.Types.ToNull().Trim().Length > 0)
                    {
                        dal.Add(info);
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
        
        #endregion

        #region DEL

        /// <summary>
        /// 작성자 : 심재운
        /// 개인주소록 삭제하기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool DelPrivate(string p)
        {
            try
            {
                SetDecryptString(p);
                int categoryID = GetDecryptParam("categoryID", "0").ToNumber<int>(); //원본 카테고리
                string listID = GetDecryptParam("listID", "");

                if (listID.Length > 0)
                {
                    DAL.Private dal = new DAL.Private();
                    return dal.Del(CurrentUserInfo.CompanyID, categoryID, listID, CurrentUserInfo.UserID);
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("DelPrivate : " + ex.Message);
                throw;
            }

        }

        /// <summary>
        /// 작성자 : 심재운
        /// 공용주소록 삭제하기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool DelCommon(string p)
        {
            try
            {
                SetDecryptString(p);
                int categoryID = GetDecryptParam("categoryID", "0").ToNumber<int>(); //원본 카테고리
                string listID = GetDecryptParam("listID", "");

                if (listID.Length > 0)
                {
                    DAL.Common dal = new DAL.Common();
                    return dal.Del(CurrentUserInfo.CompanyID, categoryID, listID);
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("DelCommon : " + ex.Message);
                throw;
            }

        }


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
                string ownerID = GetDecryptParam("ownerID", "");

                if (ownerID.Length > 0)
                {
                    DAL.ImpoUser dal = new DAL.ImpoUser();

                    foreach (string id in ownerID.Split(','))
                    {
                        BE.ImpoUserInfo info = new BE.ImpoUserInfo();
                        info.CompanyID = CurrentUserInfo.CompanyID;
                        info.OwnerID = id.Split('|')[0].ToNumber<int>();
                        info.UserID = CurrentUserInfo.UserID;
                        info.Types = id.Split('|')[1];

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

        

        #endregion        
    }
}
