using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Data;
using System.Web;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.Address.BLL
{
    public class List
    {
        /// <summary>
        /// 작성자 : 심재운
        /// 용도 : 사이트맵 추출하기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <param name="_categoryID"></param>
        /// <param name="_type"></param>
        /// <param name="_preLang"></param>
        /// <returns></returns>
        public string GetSitemapName(int _companyID, int _userID, int _categoryID, string _type, string _preLang)
        {
            string siteMap = "";
            string name = "";
            switch (_type)
            {
                case "GG":
                    Zeospace.DeskPlus.Account.DAL.Dept deptDal = new Account.DAL.Dept();
                    List<Zeospace.DeskPlus.Account.BE.DeptInfo> alDept = deptDal.GetDeptLists(_companyID,
                        _categoryID, true);

                    foreach (Account.BE.DeptInfo info in alDept)
                    {
                        siteMap = UtilHelper.GetLangName(info.NameBase, info.NameKo, info.NameEn,
                            info.NameJa, info.NameFr, info.NameZhcn, info.NameZhtw, _preLang);

                        /*
                        name =  UtilHelper.GetLangName(info.PathNameBase, info.PathNameKo, info.PathNameEn,
                            info.PathNameJa, info.PathNameFr, info.PathNameZhcn, info.PathNameZhtw, _preLang);

                        switch (_preLang)
                        {
                            case "ko":
                                name = (info.NameKo == "") ? info.PathNameBase.ToString() : name;
                                break;
                            case "en":
                                name = (info.NameEn.ToString() == "") ? info.PathNameBase.ToString() : name;
                                break;
                            case "ja":
                                name = (info.NameJa.ToString() == "") ? info.PathNameBase.ToString() : name;
                                break;
                            case "fr":
                                name = (info.NameFr.ToString() == "") ? info.PathNameBase.ToString() : name;
                                break;
                            case "zhcn":
                                name = (info.NameZhcn.ToString() == "") ? info.PathNameBase.ToString() : name;
                                break;
                            case "zhtw":
                                name = (info.NameZhtw.ToString() == "") ? info.PathNameBase.ToString() : name;
                                break;
                        }          
                        
                        siteMap = (name.Length > 0) ? name.Substring(1, name.Length - 1) : "";
                        */
                    }

                    break;
                case "CG":                    
                    Zeospace.DeskPlus.Address.DAL.CommonCategory comCatDal = new DAL.CommonCategory();
                    DataSet dsComCat = comCatDal.GetCommonCategory(_companyID, _categoryID);

                    if (dsComCat != null)
                    {
                        foreach (DataRow dr in dsComCat.Tables[0].Rows)
                        {
                            siteMap = UtilHelper.GetLangName(
                                dr["NameBase"].ToString(),
                                dr["NameKo"].ToString(), dr["NameEn"].ToString(), dr["NameJa"].ToString(),
                                dr["NameFr"].ToString(), dr["NameZhCn"].ToString(), dr["NameZhTw"].ToString(), _preLang);
                            /*
                            name = UtilHelper.GetLangName(
                                dr["PathNameBase"].ToString(),
                                dr["PathNameKo"].ToString(), dr["PathNameEn"].ToString(), dr["PathNameJa"].ToString(),
                                dr["PathNameFr"].ToString(), dr["PathNameZhCn"].ToString(), dr["PathNameZhTw"].ToString(), _preLang);

                            switch (_preLang)
                            {
                                case "ko":
                                    name = (dr["NameKo"].ToString() == "") ? dr["PathNameBase"].ToString() : name;
                                    break;
                                case "en":
                                    name = (dr["NameEn"].ToString() == "") ? dr["PathNameBase"].ToString() : name;
                                    break;
                                case "ja":
                                    name = (dr["NameJa"].ToString() == "") ? dr["PathNameBase"].ToString() : name;
                                    break;
                                case "fr":
                                    name = (dr["NameFr"].ToString() == "") ? dr["PathNameBase"].ToString() : name;
                                    break;
                                case "zhcn":
                                    name = (dr["NameZhcn"].ToString() == "") ? dr["PathNameBase"].ToString() : name;
                                    break;
                                case "zhtw":
                                    name = (dr["NameZhtw"].ToString() == "") ? dr["PathNameBase"].ToString() : name;
                                    break;
                            }

                            siteMap = (name.Length > 0) ?  name.Substring(1, name.Length - 1) : "";
                            */
                        }
                    }


                    break;
                case "PG":                    
                    Zeospace.DeskPlus.Address.DAL.PrivateCategory priCatDal = new DAL.PrivateCategory();
                    DataSet dsPriCate = priCatDal.GetPrivateCategory(_companyID, _categoryID,
                        _userID);

                    if (dsPriCate != null)
                    {
                        foreach (DataRow dr in dsPriCate.Tables[0].Rows)
                        {
                            siteMap = UtilHelper.GetLangName(
                                dr["NameBase"].ToString(),
                                dr["NameKo"].ToString(), dr["NameEn"].ToString(), dr["NameJa"].ToString(),
                                dr["NameFr"].ToString(), dr["NameZhCn"].ToString(), dr["NameZhTw"].ToString(), _preLang);

                            /*
                            name = UtilHelper.GetLangName(
                                dr["PathNameBase"].ToString(),
                                dr["PathNameKo"].ToString(), dr["PathNameEn"].ToString(), dr["PathNameJa"].ToString(),
                                dr["PathNameFr"].ToString(), dr["PathNameZhCn"].ToString(), dr["PathNameZhTw"].ToString(), _preLang);

                            switch (_preLang)
                            {
                                case "ko":
                                    name = (dr["NameKo"].ToString() == "") ? dr["PathNameBase"].ToString() : name;
                                    break;
                                case "en":
                                    name = (dr["NameEn"].ToString() == "") ? dr["PathNameBase"].ToString() : name;
                                    break;
                                case "ja":
                                    name = (dr["NameJa"].ToString() == "") ? dr["PathNameBase"].ToString() : name;
                                    break;
                                case "fr":
                                    name = (dr["NameFr"].ToString() == "") ? dr["PathNameBase"].ToString() : name;
                                    break;
                                case "zhcn":
                                    name = (dr["NameZhcn"].ToString() == "") ? dr["PathNameBase"].ToString() : name;
                                    break;
                                case "zhtw":
                                    name = (dr["NameZhtw"].ToString() == "") ? dr["PathNameBase"].ToString() : name;
                                    break;
                            }

                            siteMap = (name.Length > 0) ?  name.Substring(1, name.Length - 1) : "";
                            */
                        }
                    }


                    break;                
                case "TG":                    
                    Zeospace.DeskPlus.Common.DAL.Code tgDal = new DeskPlus.Common.DAL.Code();
                    List<Zeospace.DeskPlus.Common.BE.CodeInfo> tgCode = tgDal.GetCodeByID(_companyID, _categoryID);

                    foreach (DeskPlus.Common.BE.CodeInfo info in tgCode)
                    {
                        siteMap = UtilHelper.GetLangName(info.NameBase, info.NameKo, info.NameEn,
                            info.NameJa, info.NameFr, info.NameZhcn, info.NameZhtw, _preLang);
                        /*
                        name = UtilHelper.GetLangName(info.PathNameBase, info.PathNameKo, info.PathNameEn,
                            info.PathNameJa, info.PathNameFr, info.PathNameZhcn, info.PathNameZhtw, _preLang);

                        switch (_preLang)
                        {
                            case "ko":
                                name = (info.NameKo == "") ? info.PathNameBase.ToString() : name;
                                break;
                            case "en":
                                name = (info.NameEn.ToString() == "") ? info.PathNameBase.ToString() : name;
                                break;
                            case "ja":
                                name = (info.NameJa.ToString() == "") ? info.PathNameBase.ToString() : name;
                                break;
                            case "fr":
                                name = (info.NameFr.ToString() == "") ? info.PathNameBase.ToString() : name;
                                break;
                            case "zhcn":
                                name = (info.NameZhcn.ToString() == "") ? info.PathNameBase.ToString() : name;
                                break;
                            case "zhtw":
                                name = (info.NameZhtw.ToString() == "") ? info.PathNameBase.ToString() : name;
                                break;
                        }

                        siteMap = (name.Length > 0) ?  name.Substring(1, name.Length - 1) : "";
                        */
                    }

                    break;
                case "DG":                    
                    Zeospace.DeskPlus.Common.DAL.Code dgDal = new DeskPlus.Common.DAL.Code();
                    List<Zeospace.DeskPlus.Common.BE.CodeInfo> dgCode = dgDal.GetCodeByID(_companyID, _categoryID);

                    foreach (DeskPlus.Common.BE.CodeInfo info in dgCode)
                    {
                        siteMap = UtilHelper.GetLangName(info.NameBase, info.NameKo, info.NameEn,
                            info.NameJa, info.NameFr, info.NameZhcn, info.NameZhtw, _preLang);
                        /*
                        name = UtilHelper.GetLangName(info.PathNameBase, info.PathNameKo, info.PathNameEn,
                            info.PathNameJa, info.PathNameFr, info.PathNameZhcn, info.PathNameZhtw, _preLang);

                        switch (_preLang)
                        {
                            case "ko":
                                name = (info.NameKo == "") ? info.PathNameBase.ToString() : name;
                                break;
                            case "en":
                                name = (info.NameEn.ToString() == "") ? info.PathNameBase.ToString() : name;
                                break;
                            case "ja":
                                name = (info.NameJa.ToString() == "") ? info.PathNameBase.ToString() : name;
                                break;
                            case "fr":
                                name = (info.NameFr.ToString() == "") ? info.PathNameBase.ToString() : name;
                                break;
                            case "zhcn":
                                name = (info.NameZhcn.ToString() == "") ? info.PathNameBase.ToString() : name;
                                break;
                            case "zhtw":
                                name = (info.NameZhtw.ToString() == "") ? info.PathNameBase.ToString() : name;
                                break;
                        }

                        siteMap = (name.Length > 0) ?  name.Substring(1, name.Length - 1) : "";
                        */
                    }

                    break;
                case "FG":                    
                    Zeospace.DeskPlus.Address.DAL.FavorCategory fvDal = new DAL.FavorCategory();

                    DataSet dsFavor = fvDal.GetFavoCategory(_companyID, _categoryID,
                        _userID);

                    if (dsFavor != null)
                    {
                        foreach (DataRow dr in dsFavor.Tables[0].Rows)
                        {
                            siteMap = UtilHelper.GetLangName(
                                dr["NameBase"].ToString(),
                                dr["NameKo"].ToString(), dr["NameEn"].ToString(), dr["NameJa"].ToString(),
                                dr["NameFr"].ToString(), dr["NameZhCn"].ToString(), dr["NameZhTw"].ToString(), _preLang);

                            /*
                            name = UtilHelper.GetLangName(
                                dr["PathNameBase"].ToString(),
                                dr["PathNameKo"].ToString(), dr["PathNameEn"].ToString(), dr["PathNameJa"].ToString(),
                                dr["PathNameFr"].ToString(), dr["PathNameZhCn"].ToString(), dr["PathNameZhTw"].ToString(), _preLang);

                            switch (_preLang)
                            {
                                case "ko":
                                    name = (dr["NameKo"].ToString() == "") ? dr["PathNameBase"].ToString() : name;
                                    break;
                                case "en":
                                    name = (dr["NameEn"].ToString() == "") ? dr["PathNameBase"].ToString() : name;
                                    break;
                                case "ja":
                                    name = (dr["NameJa"].ToString() == "") ? dr["PathNameBase"].ToString() : name;
                                    break;
                                case "fr":
                                    name = (dr["NameFr"].ToString() == "") ? dr["PathNameBase"].ToString() : name;
                                    break;
                                case "zhcn":
                                    name = (dr["NameZhcn"].ToString() == "") ? dr["PathNameBase"].ToString() : name;
                                    break;
                                case "zhtw":
                                    name = (dr["NameZhtw"].ToString() == "") ? dr["PathNameBase"].ToString() : name;
                                    break;
                            }

                            siteMap = (name.Length > 0) ? name.Substring(1, name.Length - 1) : "";
                            */
                        }
                    }

                    break;
            }

            return siteMap;
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 용도 : 하위 주소록 카테고리 아이디값 반환하기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <param name="_categoryID"></param>
        /// <param name="_types"></param>
        /// <returns></returns>
        public string GetSubCatList(int _companyID, int _userID, string _categoryID, string _types)
        {
            string catList = "";                        

            switch (_types)
            {
                case "GG":
                    if (_categoryID == "0")
                    {
                        Account.BLL.Dept deptBiz = new Account.BLL.Dept();
                        catList = deptBiz.GetChildDeptList(_companyID, _categoryID.ToNumber<int>(), true);
                    }
                    break;
                case "CG":
                    if (_categoryID == "0")
                    {
                        BLL.CommonCategory comCat = new BLL.CommonCategory();
                        catList = comCat.GetChildList(_companyID, _categoryID.ToNumber<int>());
                    }
                    break;
                case "PG":
                    if (_categoryID == "0")
                    {
                        BLL.PrivateCategory priCat = new BLL.PrivateCategory();
                        catList = priCat.GetChildList(_companyID, _categoryID.ToNumber<int>(), _userID);
                    }
                    break;
                case "TG":
                    Zeospace.DeskPlus.Common.DAL.Code titleDal = new DeskPlus.Common.DAL.Code();
                    int rootTitleID = titleDal.GetRootCode(_companyID, "T", true);
                    if (catList == rootTitleID.ToString())
                    {
                        Zeospace.DeskPlus.Common.BLL.Code codeBiz = new DeskPlus.Common.BLL.Code();
                        catList = codeBiz.GetChildList(_companyID, _categoryID.ToNumber<int>(), true);
                        catList = (catList.Length == 0) ? _categoryID : catList;
                    }
                    break;
                case "DG":
                    Zeospace.DeskPlus.Common.DAL.Code dutyDal = new DeskPlus.Common.DAL.Code();
                    int rootDutyID = dutyDal.GetRootCode(_companyID, "D", true);
                    if (catList == rootDutyID.ToString())
                    {
                        Zeospace.DeskPlus.Common.BLL.Code codeBiz = new DeskPlus.Common.BLL.Code();
                        catList = codeBiz.GetChildList(_companyID, _categoryID.ToNumber<int>(), true);
                    }
                    break;
            }

            if (catList == "") catList = _categoryID;
            return catList;
        }

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 필터링 쿼리 구문(ㄱ,ㄴ,ㄷ 조건문)
        /// 작성자     : 심재운
        /// 최초작성일 : 2009-02-02
        /// </summary>
        /// <param name="_idx">The _idx.</param>
        /// <returns></returns>
        public static string FilterString(string _idx)
        {
            string fieldName = "NameBase";
            string filterStr = "";
            if (_idx.Length > 0)
            {
                switch (_idx)
                {
                    case "ㄱ":
                        filterStr = fieldName + " >='ㄱ' and " + fieldName + " < 'ㄴ'";
                        break;
                    case "ㄴ":
                        filterStr = fieldName + " >='ㄴ' and " + fieldName + " < 'ㄷ'";
                        break;
                    case "ㄷ":
                        filterStr = fieldName + " >='ㄷ' and " + fieldName + " < 'ㄹ'";
                        break;
                    case "ㄹ":
                        filterStr = fieldName + " >='ㄹ' and " + fieldName + " < 'ㅁ'";
                        break;
                    case "ㅁ":
                        filterStr = fieldName + " >='ㅁ' and " + fieldName + " < 'ㅂ'";
                        break;
                    case "ㅂ":
                        filterStr = fieldName + " >='ㅂ' and  " + fieldName + " < 'ㅅ'";
                        break;
                    case "ㅅ":
                        filterStr = fieldName + " >='ㅅ' and " + fieldName + " < 'ㅇ'";
                        break;
                    case "ㅇ":
                        filterStr = fieldName + " >='ㅇ' and " + fieldName + " < 'ㅈ'";
                        break;
                    case "ㅈ":
                        filterStr = fieldName + " >='ㅈ' and " + fieldName + " < 'ㅊ'";
                        break;
                    case "ㅊ":
                        filterStr = fieldName + " >='ㅊ' and " + fieldName + " < 'ㅋ'";
                        break;
                    case "ㅋ":
                        filterStr = fieldName + " >='ㅋ' and " + fieldName + " < 'ㅌ'";
                        break;
                    case "ㅌ":
                        filterStr = fieldName + " >='ㅌ' and " + fieldName + " < 'ㅍ'";
                        break;
                    case "ㅍ":
                        filterStr = fieldName + " >='ㅍ' and " + fieldName + " < 'ㅎ'";
                        break;
                    case "ㅎ":
                        filterStr = fieldName + " >='ㅎ' and " + fieldName + " < 'A'";
                        break;
                    case "A":
                        filterStr = fieldName + " >='A' and " + fieldName + " <= 'Z'";
                        break;
                    case "1":
                        filterStr = fieldName + " >='0' and " + fieldName + " <= '9'";
                        break;
                }
            }

            return filterStr;
        }

        #region 리스트용

        /// <summary>
        /// 작성자 : 심재운
        /// 주소록 객체에 담아서 반환하기
        /// </summary>
        /// <param name="_prelang">The _prelang.</param>
        /// <param name="dtr">The DTR.</param>
        /// <returns></returns>
        public List<object> GetListData(string _prelang, DataTableReader dtr)
        {
            List<object> listobj = new List<object>();
            HttpContext context = HttpContext.Current;
            string baseFileDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
            string baseImageDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseImageDirectory", "/DeskPlusEIP/Design/");
            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        Zeospace.DeskPlus.Address.BE.PageListInfo info = new BE.PageListInfo();
                        info.ROW = dtr["ROW"].ToString().ToNumber<int>();
                        info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                        info.CategoryID = dtr["CategoryID"].ToString().ToNumber<int>();
                        info.UserID = dtr["UserID"].ToString().ToNumber<int>();
                        info.KeyID = dtr["KeyID"].ToString().ToNumber<int>();
                        info.ImpoCnt = dtr["ImpoCnt"].ToString().ToNumber<int>(); //중요표시여부

                        string name = UtilHelper.GetLangName(
                            dtr["NameBase"].ToString(),
                            dtr["NameKo"].ToString(), dtr["NameEn"].ToString(), dtr["NameJa"].ToString(),
                            dtr["NameFr"].ToString(), dtr["NameZhCn"].ToString(), dtr["NameZhTw"].ToString(), _prelang);

                        info.Name = name;
                        //info.OwnerID = dtr["OwnerID"].ToString();
                        //info.ParentOwnerID = dtr["ParentOwnerID"].ToString();
                        info.Type = dtr["Type"].ToString();

                        string titleName = UtilHelper.GetLangName(
                            dtr["TitleBase"].ToString(),
                            dtr["TitleKo"].ToString(), dtr["TitleEn"].ToString(), dtr["TitleJa"].ToString(),
                            dtr["TitleFr"].ToString(), dtr["TitleZhCn"].ToString(), dtr["TitleZhTw"].ToString(), _prelang);
                        info.TitleName = titleName;

                        //info.DutyID = dtr["DutyID"].ToString();
                        info.Mail = dtr["Mail"].ToString();
                        info.MobilePhone = dtr["MobilePhone"].ToString();

                        #region 다국어 회사 설정
                        string comName = UtilHelper.GetLangName(
                            dtr["CompanyBase"].ToString(),
                            dtr["CompanyKo"].ToString(), dtr["CompanyEn"].ToString(), dtr["CompanyJa"].ToString(),
                            dtr["CompanyFr"].ToString(), dtr["CompanyZhCn"].ToString(), dtr["CompanyZhTw"].ToString(), _prelang);
                        #endregion

                        info.Company = comName;
                        info.OfficePhone = dtr["OfficePhone"].ToString();
                        info.BaseDept = dtr["BaseDept"].ToString();

                        #region 다국어 회사 설정
                        string deptName = UtilHelper.GetLangName(
                            dtr["DeptBase"].ToString(),
                            dtr["DeptKo"].ToString(), dtr["DeptEn"].ToString(), dtr["DeptJa"].ToString(),
                            dtr["DeptFr"].ToString(), dtr["DeptZhCn"].ToString(), dtr["DeptZhTw"].ToString(), _prelang);
                        #endregion
                        info.DeptName = deptName;

                        #region 사진 이미지 설정
                        string photoName = dtr["PhotoName"].ToString();
                        info.PhotoName = "";
                        string imgPath = "";
                        string imgTag = "";
                        string encParams = "";
                        Dictionary<string, string> paraDic = null;

                        if (!string.IsNullOrEmpty(photoName) && photoName.Length > 0)
                        {
                            switch (info.Type)
                            {
                                case "GM":
                                    imgPath = baseFileDirectory + "{0}/Users/{1}/Picture/" + photoName;
                                    imgPath = string.Format(imgPath, info.CompanyID, info.KeyID); 

                                    if (System.IO.File.Exists(context.Server.MapPath(imgPath)))
                                    {
                                        #region 스크린샷
                                        paraDic = new Dictionary<string, string>();
                                        paraDic.Add("filePath", imgPath);
                                        encParams = Utility.EncryptString(context.Server.UrlEncode(Utility.GetParameterDic(paraDic)));
                                        imgPath = "/DeskPlusEIP/Common/DownLoad/ImageView.aspx?p=" + encParams;
                                        #endregion

                                        imgTag = "<img src='{0}' width='52' height='52' />";
                                        imgTag = string.Format(imgTag, imgPath);
                                        info.PhotoName = imgTag;
                                    }

                                    break;
                                case "TM":
                                    imgPath = baseFileDirectory + "{0}/Users/{1}/Picture/" + photoName;
                                    imgPath = string.Format(imgPath, info.CompanyID, info.KeyID);

                                    if (System.IO.File.Exists(context.Server.MapPath(imgPath)))
                                    {
                                        #region 스크린샷
                                        paraDic = new Dictionary<string, string>();
                                        paraDic.Add("filePath", imgPath);
                                        encParams = Utility.EncryptString(context.Server.UrlEncode(Utility.GetParameterDic(paraDic)));
                                        imgPath = "/DeskPlusEIP/Common/DownLoad/ImageView.aspx?p=" + encParams;
                                        #endregion

                                        imgTag = "<img src='{0}' width='52' height='52' />";
                                        imgTag = string.Format(imgTag, imgPath);
                                        info.PhotoName = imgTag;
                                    }

                                    break;
                                case "DM":
                                    imgPath = baseFileDirectory + "{0}/Users/{1}/Picture/" + photoName;
                                    imgPath = string.Format(imgPath, info.CompanyID, info.KeyID);

                                    if (System.IO.File.Exists(context.Server.MapPath(imgPath)))
                                    {
                                        #region 스크린샷
                                        paraDic = new Dictionary<string, string>();
                                        paraDic.Add("filePath", imgPath);
                                        encParams = Utility.EncryptString(context.Server.UrlEncode(Utility.GetParameterDic(paraDic)));
                                        imgPath = "/DeskPlusEIP/Common/DownLoad/ImageView.aspx?p=" + encParams;
                                        #endregion

                                        imgTag = "<img src='{0}' width='52' height='52' />";
                                        imgTag = string.Format(imgTag, imgPath);
                                        info.PhotoName = imgTag;
                                    }

                                    break;
                                default:
                                    imgPath = baseFileDirectory + "{0}/Address/{1}";
                                    string[] fileStr = photoName.Split('.');
                                    imgPath = string.Format(imgPath, info.CompanyID, fileStr[0] + "_S." + fileStr[1]);

                                    if (System.IO.File.Exists(context.Server.MapPath(imgPath)))
                                    {
                                        #region 스크린샷
                                        paraDic = new Dictionary<string, string>();
                                        paraDic.Add("filePath", imgPath);
                                        encParams = Utility.EncryptString(context.Server.UrlEncode(Utility.GetParameterDic(paraDic)));
                                        imgPath = "/DeskPlusEIP/Common/DownLoad/ImageView.aspx?p=" + encParams;
                                        #endregion

                                        imgTag = "<img src='{0}' width='52' height='52' />";
                                        imgTag = string.Format(imgTag, imgPath);
                                        info.PhotoName = imgTag;
                                    }
                                    break;
                            }
                        }

                        if (info.PhotoName.Length == 0)
                        {
                            #region 스크린샷
                            paraDic = new Dictionary<string, string>();
                            //paraDic.Add("filePath", baseImageDirectory + "Blue/Img/Icon/ico_noimg.gif");
                            paraDic.Add("filePath", baseImageDirectory + "C/Img/Icon/ico_noimg.gif");
                            encParams = Utility.EncryptString(context.Server.UrlEncode(Utility.GetParameterDic(paraDic)));
                            imgPath = "/DeskPlusEIP/Common/DownLoad/ImageView.aspx?p=" + encParams;
                            #endregion

                            info.PhotoName = "<img src='" + imgPath + "' width='52' height='52' alt='' title='' />";
                        }

                        #endregion

                        switch (info.Type)
                        {
                            case "GM":
                                info.TypeName = "<div class='r_gubun_gm'></div>";
                                break;
                            case "CM":
                                info.TypeName = "<div class='r_gubun_cm'></div>";
                                break;
                            case "PM":
                                info.TypeName = "<div class='r_gubun_pm'></div>";
                                break;
                            case "RM":
                                info.TypeName = "<div class='r_gubun_rm'></div>";
                                break;
                            case "TM":
                                info.TypeName = "<div class='r_gubun_tm'></div>";
                                break;
                            case "DM":
                                info.TypeName = "<div class='r_gubun_dm'></div>";
                                break;
                            case "FM":
                                info.TypeName = "<div class='gubun_fm'></div>";
                                break;
                            default:
                                info.TypeName = "";
                                break;
                        }

                        listobj.Add(info);
                    }
                }
            }
            return listobj;
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 회사별 주소록 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_preLang">The _pre lang.</param>
        /// <param name="categoryID">The category ID.</param>
        /// <param name="totalCount">The total count.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="pageSizePageNo">The page size page no.</param>
        /// <param name="_searchKey">The _search key.</param>
        /// <param name="_searchValue">The _search value.</param>
        /// <param name="_searchFilter">The _search filter.</param>
        /// <param name="_orders">The _orders.</param>
        /// <returns></returns>
        public List<object> GetCompany(int _companyID, string _preLang, int categoryID, int totalCount, string pageSize,
            string pageSizePageNo, string _searchOption, string _searchKey, string _searchValue, string _searchFilter, string _orders, int _userID)
        {
            string deptList = categoryID.ToString();

            if (categoryID == 0)
            {
                Account.BLL.Dept deptBiz = new Account.BLL.Dept();
                deptList = deptBiz.GetChildDeptList(_companyID, categoryID, true);
                deptList = (deptList.Length == 0) ? categoryID.ToString() : deptList;
            }

            Address.DAL.Private pBiz = new DAL.Private();
            //int totalCount = pBiz.GetPageCount(CurrentUserInfo.CompanyID, deptList, 0, "GM", _searchKey, _searchValue);

            string rtnSetPaging = Utility.SetPaging(totalCount, pageSize.ToNumber<int>(), pageSizePageNo.ToNumber<int>());
            string[] setPageingStr = rtnSetPaging.Split(',');

            DataTableReader dtr = pBiz.GetPageListWithSub(_companyID,
                deptList, _userID, "GM", setPageingStr[0], setPageingStr[1], _searchOption, _searchKey, _searchValue, BLL.List.FilterString(_searchFilter), _orders);
            
            return GetListData(_preLang, dtr);
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 공용주소록 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_preLang">The _pre lang.</param>
        /// <param name="categoryID">The category ID.</param>
        /// <param name="totalCount">The total count.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="pageSizePageNo">The page size page no.</param>
        /// <param name="_searchKey">The _search key.</param>
        /// <param name="_searchValue">The _search value.</param>
        /// <param name="_searchFilter">The _search filter.</param>
        /// <param name="_orders">The _orders.</param>
        /// <returns></returns>
        public List<object> GetCommon(int _companyID, string _preLang, int categoryID, int totalCount, string pageSize,
            string pageSizePageNo, string _searchOption, string _searchKey, string _searchValue, string _searchFilter, string _orders, int _userID)
        {
            string comList = categoryID.ToString();

            if (categoryID == 0)
            {
                BLL.CommonCategory comCat = new BLL.CommonCategory();
                comList = comCat.GetChildList(_companyID, categoryID);
                comList = (comList.Length == 0) ? categoryID.ToString() : comList;
            }

            Address.DAL.Private pBiz = new DAL.Private();
            //int totalCount = pBiz.GetPageCount(CurrentUserInfo.CompanyID, deptList, 0, "GM", _searchKey, _searchValue);

            string rtnSetPaging = Utility.SetPaging(totalCount, pageSize.ToNumber<int>(), pageSizePageNo.ToNumber<int>());
            string[] setPageingStr = rtnSetPaging.Split(',');

            DataTableReader dtr = pBiz.GetPageListWithSub(_companyID,
                comList, _userID, "CM", setPageingStr[0], setPageingStr[1], _searchOption, _searchKey, _searchValue, BLL.List.FilterString(_searchFilter), _orders);

            return GetListData(_preLang, dtr);
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 개인주소록 가져오기
        /// 주의 : 개인주소록, 전달주소록은 나의것만 봐야 한다.
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <param name="_preLang">The _pre lang.</param>
        /// <param name="categoryID">The category ID.</param>
        /// <param name="totalCount">The total count.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="pageSizePageNo">The page size page no.</param>
        /// <param name="_searchKey">The _search key.</param>
        /// <param name="_searchValue">The _search value.</param>
        /// <param name="_searchFilter">The _search filter.</param>
        /// <param name="_orders">The _orders.</param>
        /// <returns></returns>
        public List<object> GetPrivate(int _companyID, int _userID, string _preLang, int categoryID, int totalCount, string pageSize,
            string pageSizePageNo, string _searchOption, string _searchKey, string _searchValue, string _searchFilter, string _orders)
        {
            string priList = categoryID.ToString();

            if (categoryID == 0)
            {
                BLL.PrivateCategory priCat = new BLL.PrivateCategory();
                priList = priCat.GetChildList(_companyID, categoryID, _userID);
                priList = (priList.Length == 0) ? categoryID.ToString() : priList;
            }

            Address.DAL.Private pBiz = new DAL.Private();
            //int totalCount = pBiz.GetPageCount(CurrentUserInfo.CompanyID, deptList, 0, "GM", _searchKey, _searchValue);

            string rtnSetPaging = Utility.SetPaging(totalCount, pageSize.ToNumber<int>(), pageSizePageNo.ToNumber<int>());
            string[] setPageingStr = rtnSetPaging.Split(',');

            DataTableReader dtr = pBiz.GetPageListWithSub(_companyID,
                priList, _userID, "PM", setPageingStr[0], setPageingStr[1], _searchOption, _searchKey, _searchValue, BLL.List.FilterString(_searchFilter), _orders);

            return GetListData(_preLang, dtr);
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 전달받은주소록
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <param name="_preLang">The _pre lang.</param>
        /// <param name="categoryID">The category ID.</param>
        /// <param name="totalCount">The total count.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="pageSizePageNo">The page size page no.</param>
        /// <param name="_searchKey">The _search key.</param>
        /// <param name="_searchValue">The _search value.</param>
        /// <param name="_searchFilter">The _search filter.</param>
        /// <param name="_orders">The _orders.</param>
        /// <returns></returns>
        public List<object> GetTransfer(int _companyID, int _userID, string _preLang, int categoryID, int totalCount, string pageSize,
            string pageSizePageNo, string _searchKey, string _searchValue, string _searchFilter, string _orders)
        {
            Address.DAL.Private pBiz = new DAL.Private();
            //int totalCount = pBiz.GetPageCount(CurrentUserInfo.CompanyID, deptList, 0, "GM", _searchKey, _searchValue);

            string rtnSetPaging = Utility.SetPaging(totalCount, pageSize.ToNumber<int>(), pageSizePageNo.ToNumber<int>());
            string[] setPageingStr = rtnSetPaging.Split(',');

            DataTableReader dtr = pBiz.GetPageList(_companyID,
                "0", _userID, "RM", setPageingStr[0], setPageingStr[1], _searchKey, _searchValue, BLL.List.FilterString(_searchFilter), _orders);

            return GetListData(_preLang, dtr);
        }

        /// <summary>
        /// 직급 리스트
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_preLang">The _pre lang.</param>
        /// <param name="categoryID">The category ID.</param>
        /// <param name="totalCount">The total count.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="pageSizePageNo">The page size page no.</param>
        /// <param name="_searchKey">The _search key.</param>
        /// <param name="_searchValue">The _search value.</param>
        /// <param name="_searchFilter">The _search filter.</param>
        /// <param name="_orders">The _orders.</param>
        /// <returns></returns>
        public List<object> GetTitle(int _companyID, string _preLang, int categoryID, int totalCount, string pageSize,
            string pageSizePageNo, string _searchKey, string _searchValue, string _searchFilter, string _orders, int _userID)
        {
            string titleList = categoryID.ToString();
            Zeospace.DeskPlus.Common.DAL.Code codeDal = new DeskPlus.Common.DAL.Code();
            int rootTitleID = codeDal.GetRootCode(_companyID, "T", true);

            if (categoryID == rootTitleID)
            {
                Zeospace.DeskPlus.Common.BLL.Code codeBiz = new DeskPlus.Common.BLL.Code();
                titleList = codeBiz.GetChildList(_companyID, categoryID, true);
                titleList = (titleList.Length == 0) ? categoryID.ToString() : titleList;
            }

            Address.DAL.Private pBiz = new DAL.Private();
            //int totalCount = pBiz.GetPageCount(CurrentUserInfo.CompanyID, deptList, 0, "GM", _searchKey, _searchValue);

            string rtnSetPaging = Utility.SetPaging(totalCount, pageSize.ToNumber<int>(), pageSizePageNo.ToNumber<int>());
            string[] setPageingStr = rtnSetPaging.Split(',');

            DataTableReader dtr = pBiz.GetPageList(_companyID,
                titleList, 0, "TM", setPageingStr[0], setPageingStr[1], _searchKey, _searchValue, BLL.List.FilterString(_searchFilter), _orders);

            return GetListData(_preLang, dtr);
        }

        /// <summary>
        /// 직책 리스트
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_preLang">The _pre lang.</param>
        /// <param name="categoryID">The category ID.</param>
        /// <param name="totalCount">The total count.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="pageSizePageNo">The page size page no.</param>
        /// <param name="_searchKey">The _search key.</param>
        /// <param name="_searchValue">The _search value.</param>
        /// <param name="_searchFilter">The _search filter.</param>
        /// <param name="_orders">The _orders.</param>
        /// <returns></returns>
        public List<object> GetDuty(int _companyID, string _preLang, int categoryID, int totalCount, string pageSize,
            string pageSizePageNo, string _searchKey, string _searchValue, string _searchFilter, string _orders, int _userID)
        {
            string dutyList = categoryID.ToString();
            Zeospace.DeskPlus.Common.DAL.Code codeDal = new DeskPlus.Common.DAL.Code();
            int rootDutyID = codeDal.GetRootCode(_companyID, "D", true);

            if (categoryID == rootDutyID)
            {
                Zeospace.DeskPlus.Common.BLL.Code codeBiz = new DeskPlus.Common.BLL.Code();
                dutyList = codeBiz.GetChildList(_companyID, categoryID, true);
                dutyList = (dutyList.Length == 0) ? categoryID.ToString() : dutyList;
            }

            Address.DAL.Private pBiz = new DAL.Private();
            //int totalCount = pBiz.GetPageCount(CurrentUserInfo.CompanyID, deptList, 0, "GM", _searchKey, _searchValue);

            string rtnSetPaging = Utility.SetPaging(totalCount, pageSize.ToNumber<int>(), pageSizePageNo.ToNumber<int>());
            string[] setPageingStr = rtnSetPaging.Split(',');

            DataTableReader dtr = pBiz.GetPageList(_companyID,
                dutyList, 0, "DM", setPageingStr[0], setPageingStr[1], _searchKey, _searchValue, BLL.List.FilterString(_searchFilter), _orders);

            return GetListData(_preLang, dtr);
        }

        #endregion

        #region 주소록 연동


        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-06-08
        /// 작성목적 : 주소록 객체에 담아서 반환하기
        /// </summary>
        /// <param name="_prelang">The _prelang.</param>
        /// <param name="dtr">The DTR.</param>
        /// <returns></returns>
        public List<object> GetAddressData(string _prelang, DataTableReader dtr)
        {
            List<object> listobj = new List<object>();
            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        Zeospace.DeskPlus.Address.BE.PageListInfo info = new BE.PageListInfo();
                        info.ROW = dtr["ROW"].ToString().ToNumber<int>();
                        info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                        info.CategoryID = dtr["CategoryID"].ToString().ToNumber<int>();
                        info.UserID = dtr["UserID"].ToString().ToNumber<int>();
                        info.KeyID = dtr["KeyID"].ToString().ToNumber<int>();
                        info.OwnerID = dtr["OwnerID"].ToString();

                        string name = UtilHelper.GetLangName(
                            dtr["NameBase"].ToString(),
                            dtr["NameKo"].ToString(), dtr["NameEn"].ToString(), dtr["NameJa"].ToString(),
                            dtr["NameFr"].ToString(), dtr["NameZhCn"].ToString(), dtr["NameZhTw"].ToString(), _prelang);

                        info.Name = name;
                        info.Type = dtr["Type"].ToString();

                        string titleName = UtilHelper.GetLangName(
                            dtr["TitleBase"].ToString(),
                            dtr["TitleKo"].ToString(), dtr["TitleEn"].ToString(), dtr["TitleJa"].ToString(),
                            dtr["TitleFr"].ToString(), dtr["TitleZhCn"].ToString(), dtr["TitleZhTw"].ToString(), _prelang);
                        info.TitleName = titleName;

                        info.Mail = dtr["Mail"].ToString();
                        info.MobilePhone = dtr["MobilePhone"].ToString();

                        info.OfficePhone = dtr["OfficePhone"].ToString();
                        info.BaseDept = dtr["BaseDept"].ToString();

                        #region 다국어 부서 설정
                        string deptName = UtilHelper.GetLangName(
                            dtr["DeptBase"].ToString(),
                            dtr["DeptKo"].ToString(), dtr["DeptEn"].ToString(), dtr["DeptJa"].ToString(),
                            dtr["DeptFr"].ToString(), dtr["DeptZhCn"].ToString(), dtr["DeptZhTw"].ToString(), _prelang);
                        #endregion
                        info.DeptName = deptName;
                       
                        listobj.Add(info);
                    }
                }
            }
            return listobj;
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-06-08
        /// 작성목적 : 회사별 주소록 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_preLang"></param>
        /// <param name="categoryID"></param>
        /// <param name="_searchValue"></param>
        /// <returns></returns>
        public List<object> GetCompany(int _companyID, string _preLang, int categoryID, string _searchField, string _searchValue)
        {
            string deptList = categoryID.ToString();

            if (categoryID == 0)
            {
                Account.BLL.Dept deptBiz = new Account.BLL.Dept();
                deptList = deptBiz.GetChildDeptList(_companyID, categoryID, true);
                deptList = (deptList.Length == 0) ? categoryID.ToString() : deptList;
            }

            Address.DAL.Private pBiz = new DAL.Private();

            DataTableReader dtr = pBiz.GetListWithSub(_companyID, deptList, 0, "GM", _searchField, _searchValue);

            return GetAddressData(_preLang, dtr);
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-06-08
        /// 작성목적 : 공용 주소록 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_preLang"></param>
        /// <param name="categoryID"></param>
        /// <param name="_searchValue"></param>
        /// <returns></returns>
        public List<object> GetCommon(int _companyID, string _preLang, int categoryID, string _searchField, string _searchValue)
        {
            string comList = categoryID.ToString();

            if (categoryID == 0)
            {
                BLL.CommonCategory comCat = new BLL.CommonCategory();
                comList = comCat.GetChildList(_companyID, categoryID);
                comList = (comList.Length == 0) ? categoryID.ToString() : comList;
            }

            Address.DAL.Private pBiz = new DAL.Private();

            DataTableReader dtr = pBiz.GetListWithSub(_companyID, comList, 0, "CM", _searchField, _searchValue);

            return GetAddressData(_preLang, dtr);
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-06-08
        /// 작성목적 : 개인 주소록 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <param name="_preLang"></param>
        /// <param name="categoryID"></param>
        /// <param name="_searchValue"></param>
        /// <returns></returns>
        public List<object> GetPrivate(int _companyID, int _userID, string _preLang, int categoryID, string _searchField, string _searchValue)
        {
            string priList = categoryID.ToString();

            if (categoryID == 0)
            {
                BLL.PrivateCategory priCat = new BLL.PrivateCategory();
                priList = priCat.GetChildList(_companyID, categoryID, _userID);
                priList = (priList.Length == 0) ? categoryID.ToString() : priList;
            }

            Address.DAL.Private pBiz = new DAL.Private();

            DataTableReader dtr = pBiz.GetListWithSub(_companyID, priList, _userID, "PM", _searchField, _searchValue);

            return GetAddressData(_preLang, dtr);
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-06-08
        /// 작성목적 : 전달받은 주소록 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <param name="_preLang"></param>
        /// <param name="categoryID"></param>
        /// <param name="_searchValue"></param>
        /// <returns></returns>
        public List<object> GetTransfer(int _companyID, int _userID, string _preLang, int categoryID, string _searchField, string _searchValue)
        {
            Address.DAL.Private pBiz = new DAL.Private();

            DataTableReader dtr = pBiz.GetList(_companyID, "0", _userID, "RM", _searchField, _searchValue);

            return GetAddressData(_preLang, dtr);
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-06-08
        /// 작성목적 : 직급 주소록 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_preLang"></param>
        /// <param name="categoryID"></param>
        /// <param name="_searchValue"></param>
        /// <returns></returns>
        public List<object> GetTitle(int _companyID, string _preLang, int categoryID, string _searchField, string _searchValue)
        {
            string titleList = categoryID.ToString();
            Zeospace.DeskPlus.Common.DAL.Code codeDal = new DeskPlus.Common.DAL.Code();
            int rootTitleID = codeDal.GetRootCode(_companyID, "T", true);

            if (categoryID == rootTitleID)
            {
                Zeospace.DeskPlus.Common.BLL.Code codeBiz = new DeskPlus.Common.BLL.Code();
                titleList = codeBiz.GetChildList(_companyID, categoryID, true);
                titleList = (titleList.Length == 0) ? categoryID.ToString() : titleList;
            }

            Address.DAL.Private pBiz = new DAL.Private();

            DataTableReader dtr = pBiz.GetList(_companyID, titleList, 0, "TM", _searchField, _searchValue);

            return GetAddressData(_preLang, dtr);
        }


        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-06-08
        /// 작성목적 : 전체 주소록 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <param name="_preLang"></param>
        /// <param name="_searchValue"></param>
        /// <returns></returns>
        public List<object> GetTotal(int _companyID, int _userID, string _preLang, string _searchField, string _searchValue)
        {
            Address.DAL.Private pBiz = new DAL.Private();
            DataTableReader dtr = pBiz.GetList(_companyID, _userID, _searchField, _searchValue);

            return GetAddressData(_preLang, dtr);
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-06-11
        /// 작성목적 : 직책 주소록 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_preLang"></param>
        /// <param name="categoryID"></param>
        /// <param name="_searchField"></param>
        /// <param name="_searchValue"></param>
        /// <returns></returns>
        public List<object> GetDuty(int _companyID, string _preLang, int categoryID, string _searchField, string _searchValue)
        {
            string dutyList = categoryID.ToString();
            Zeospace.DeskPlus.Common.DAL.Code codeDal = new DeskPlus.Common.DAL.Code();
            int rootDutyID = codeDal.GetRootCode(_companyID, "D", true);

            if (categoryID == rootDutyID)
            {
                Zeospace.DeskPlus.Common.BLL.Code codeBiz = new DeskPlus.Common.BLL.Code();
                dutyList = codeBiz.GetChildList(_companyID, categoryID, true);
                dutyList = (dutyList.Length == 0) ? categoryID.ToString() : dutyList;
            }

            Address.DAL.Private pBiz = new DAL.Private();

            DataTableReader dtr = pBiz.GetList(_companyID, dutyList, 0, "DM", _searchField, _searchValue);

            return GetAddressData(_preLang, dtr);
        }
        #endregion 
    }
}
