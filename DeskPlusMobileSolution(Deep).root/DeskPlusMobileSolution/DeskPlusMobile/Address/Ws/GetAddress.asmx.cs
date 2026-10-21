using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlusMobile.Web.Helper;
using System.IO;

namespace Zeospace.DeskPlusMobile.Web.Address.Ws
{
    /// <summary>
    /// GetAddress의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class GetAddress : Helper.MobileBaseService
    {
        /// <summary>
        /// 작성자   : freeman
        /// 작성목적 : 주소록 가지고 오기
        /// 작성일자 : 2011-07-14
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetAddressList(string p)
        {
            SetDecryptString(p);

            string firstChar = GetDecryptParam("firstChar", "");
            string lastChar = String.Empty;
            string searchValue = GetDecryptParam("searchValue", "");
            string gubun = GetDecryptParam("gubun", "G");
            int pageNo = Convert.ToInt32(GetDecryptParam("pageNo", "1"));
            string pageSize = GetDecryptParam("pageSize", "");
            string searchQuery = String.Empty;
            string sqlQuery = String.Empty;    

            switch (firstChar)
            {
                case "ㄱ":
                    lastChar = "ㄴ";
                    break;
                case "ㄴ":
                    lastChar = "ㄷ";
                    break;
                case "ㄷ":
                    lastChar = "ㄹ";
                    break;
                case "ㄹ":
                    lastChar = "ㅁ";
                    break;
                case "ㅁ":
                    lastChar = "ㅂ";
                    break;
                case "ㅂ":
                    lastChar = "ㅅ";
                    break;
                case "ㅅ":
                    lastChar = "ㅇ";
                    break;
                case "ㅇ":
                    lastChar = "ㅈ";
                    break;
                case "ㅈ":
                    lastChar = "ㅊ";
                    break;
                case "ㅊ":
                    lastChar = "ㅋ";
                    break;
                case "ㅋ":
                    lastChar = "ㅌ";
                    break;
                case "ㅌ":
                    lastChar = "ㅍ";
                    break;
                case "ㅍ":
                    lastChar = "ㅎ";
                    break;
                case "ㅎ":
                    lastChar = "A";
                    break;
                case "A":
                    lastChar = "Z";
                    break;
                case "0":
                    lastChar = "9";
                    break;

                default:
                    break;
            }

            //검색어
            if (searchValue.Trim().Length > 0)
            {
                searchValue = searchValue.Replace("&", "&amp;");
                searchValue = searchValue.Replace("\"", "&quot;");
                searchValue = searchValue.Replace("#", "&#35;");
                searchValue = searchValue.Replace("'", "&#39;");
                searchValue = searchValue.Replace("(", "&#40;");
                searchValue = searchValue.Replace(")", "&#41;");
                searchValue = searchValue.Replace("<", "&lt;");
                searchValue = searchValue.Replace(">", "&gt;");

                searchQuery = " AND (ManyName LIKE N'%" + searchValue.Trim() + "%' OR Mail LIKE N'%" + searchValue.Trim() + "%' OR replace(MobilePhone,'-','')  LIKE N'%" + searchValue.Trim() + "%' OR DeptManyName LIKE N'%" + searchValue.Trim() + "%' OR replace(OfficePhone,'-','') LIKE N'%" + searchValue.Trim() + "%') ";
            }

            switch (gubun)
            {
                case "G":
                    sqlQuery = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLAddress, "SEL_QRY_ACCOUNTUSER_LIST_PAGEING");                    
                    break;
                case "C":
                    sqlQuery = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLAddress, "SEL_QRY_COMMONUSER_LIST_PAGEING");                    
                    break;
                case "P":
                    sqlQuery = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLAddress, "SEL_QRY_PRIVATEUSER_LIST_PAGEING");                    
                    break;
                default:
                    sqlQuery = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLAddress, "SEL_QRY_ACCOUNTUSER_LIST_PAGEING");
                    break;
            }

            sqlQuery = GetSearchQuery(firstChar, lastChar, sqlQuery, "NameBase", gubun, searchQuery, pageNo.ToString(), pageSize);
            DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sqlQuery, "SEL_QRY_USER", null);

            DataView dv = ds.Tables[0].DefaultView;
            dv.Sort = "Orders,NameKo,NameEn,NameJa,NameFr,NameZhCn,NameZhTw";

            List<object> listobj = new List<object>();

            if (dv.Table.Rows.Count > 0)
            {
                string photoPath = String.Empty;
                string photoTag = String.Empty;
                for (int i = 0; i < dv.Table.Rows.Count; i++)
                {
                    BE.AddressInfo info = new BE.AddressInfo();

                    info.OwnerID = Convert.ToInt32(dv.Table.Rows[i]["OwnerID"].ToString());

                    info.Name = this.GetLangName(dv.Table.Rows[i]["NameBase"].ToString(), dv.Table.Rows[i]["NameKo"].ToString(), dv.Table.Rows[i]["NameEn"].ToString(),
                               dv.Table.Rows[i]["NameJa"].ToString(),dv.Table.Rows[i]["NameFr"].ToString(), dv.Table.Rows[i]["NameZhcn"].ToString(), dv.Table.Rows[i]["NameZhtw"].ToString(), PreLang);
                    info.TitleName = this.GetLangName(dv.Table.Rows[i]["TitleBase"].ToString(), dv.Table.Rows[i]["TitleKo"].ToString(), dv.Table.Rows[i]["TitleEn"].ToString(),
                               dv.Table.Rows[i]["TitleJa"].ToString(), dv.Table.Rows[i]["TitleFr"].ToString(), dv.Table.Rows[i]["TitleZhcn"].ToString(), dv.Table.Rows[i]["TitleZhtw"].ToString(), PreLang);
                    info.DeptName = this.GetLangName(dv.Table.Rows[i]["DeptBase"].ToString(), dv.Table.Rows[i]["DeptKo"].ToString(), dv.Table.Rows[i]["DeptEn"].ToString(),
                               dv.Table.Rows[i]["DeptJa"].ToString(), dv.Table.Rows[i]["DeptFr"].ToString(), dv.Table.Rows[i]["DeptZhcn"].ToString(), dv.Table.Rows[i]["DeptZhtw"].ToString(), PreLang);

                    info.Mail = dv.Table.Rows[i]["Mail"].ToString();
                    info.KeyValue = info.OwnerID + "_" + info.Mail;

                    /*
                    if (gubun == "G")
                    {
                        photoPath = FilePath + "/{0}/Users/{1}/Picture/" + ds.Tables[0].Rows[i]["PhotoName"].ToString();
                        photoPath = string.Format(photoPath, UserInfo.CompanyID, info.OwnerID);

                        if (!File.Exists(Server.MapPath(photoPath)))
                        {
                            photoPath = "/DeskPlusMobile/Content/ko/Base/Img/noimg.gif";
                        }
                    }
                    else
                    {
                        if (dv.Table.Rows[i]["PhotoName"].ToString().Trim() != "")
                        {
                            photoPath = FilePath + "/{0}/Address/{1}";
                            string[] fileStr = dv.Table.Rows[i]["PhotoName"].ToString().Split('.');
                            photoPath = string.Format(photoPath, UserInfo.CompanyID, fileStr[0] + "_S." + fileStr[1]);
                        }
                        else
                        {
                            photoPath = "/DeskPlusMobile/Content/ko/Base/Img/noimg.gif";
                        }
                    }         

                    Dictionary<string, string> paraDic = new Dictionary<string, string>();
                    paraDic.Add("filePath", photoPath);
                    string encParams = GetEncryptParam(Zeospace.DeskPlusMobile.Web.Helper.WebComponent.Utility.GetParameterDic(paraDic));
                    photoPath = "/DeskPlusMobile/Common/ImageView.aspx?p=" + encParams;

                    photoTag = "<img src='{0}' class='photo' />";
                    photoTag = string.Format(photoTag, photoPath);
                    info.Photo = photoTag;
                    */

                    info.Gubun = gubun;

                    listobj.Add(info);
                }
            }

            return listobj;

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="_firstChar"></param>
        /// <param name="_lastChar"></param>
        /// <param name="_sqlQuery"></param>
        /// <param name="_name"></param>
        /// <param name="_gubun"></param>
        /// <param name="_searchQuery"></param>
        /// <param name="_pageNo"></param>
        /// <param name="_pageSize"></param>
        /// <returns></returns>
        private string GetSearchQuery(string _firstChar, string _lastChar, string _sqlQuery,
            string _name, string _gubun, string _searchQuery, string _pageNo, string _pageSize)
        {
            string privateQuery = "";

            if (_gubun == "P")
            {
                privateQuery = " AND UserID=" + UserID.ToString();
            }

            if (String.IsNullOrEmpty(_firstChar))
            {
                return string.Format(_sqlQuery, CompanyID, "", _searchQuery, privateQuery, _pageNo, _pageSize);
            }

            if (_firstChar == "A" || _firstChar == "0")
            {
                _sqlQuery = string.Format(_sqlQuery, CompanyID, "and (substring(" + _name + ", 1, 1) >='" + _firstChar + "' and substring(" + _name + ", 1, 1) <= '" + _lastChar + "')", _searchQuery, privateQuery, _pageNo, _pageSize);
            }
            else if (_firstChar == "Z")
            {
                _sqlQuery = string.Format(_sqlQuery, CompanyID, "and (substring(" + _name + ", 1, 1) >='" + _firstChar + "')", _searchQuery, privateQuery, _pageNo, _pageSize);
            }
            else
            {
                _sqlQuery = string.Format(_sqlQuery, CompanyID, "and (" + _name + " >='" + _firstChar + "' and " + _name + " < '" + _lastChar + "')", _searchQuery, privateQuery, _pageNo, _pageSize);
            }

            return _sqlQuery;
        }


        /// <summary>
        /// 작성자   : freeman
        /// 작성목적 : 주소 정보 가져오기 
        /// 작성일자 : 2011-06-20
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string GetAddressInfo(string p)
        {
            SetDecryptString(p);
            string gubun = GetDecryptParam("gubun", "G");
            int ownerID = Convert.ToInt32(GetDecryptParam("ownerID", "0"));
            
            string sqlQuery = String.Empty;
            string searchQuery = searchQuery = " AND KeyID=" + ownerID;

            switch (gubun)
            {
                case "G":                    
                    sqlQuery = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLAddress, "SEL_QRY_ACCOUNTUSER_VIEW");
                    break;
                case "C":
                    sqlQuery = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLAddress, "SEL_QRY_COMMONUSER_VIEW");
                    break;
                case "P":
                    sqlQuery = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLAddress, "SEL_QRY_PRIVATEUSER_VIEW");
                    break;
                default:
                    sqlQuery = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLAddress, "SEL_QRY_ACCOUNTUSER_VIEW");
                    break;
            }


            sqlQuery = string.Format(sqlQuery, UserInfo.CompanyID, searchQuery);

            DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sqlQuery, "GetContent", null);
            ds.Tables[0].Columns.Add("Name");
            ds.Tables[0].Columns.Add("DeptName");
            ds.Tables[0].Columns.Add("TitleName");
            ds.Tables[0].Columns.Add("PhotoPath");
            ds.Tables[0].Columns.Add("DeptEtc");

            if (ds != null)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string photoPath = String.Empty;
                    string deptEtc = String.Empty;

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {                     
                        if (gubun == "G")
                        {
                            if (ds.Tables[0].Rows[i]["BaseDept"].ToString() == "1")
                            {
                                ds.Tables[0].Rows[0]["Name"] = this.GetLangName(ds.Tables[0].Rows[i]["NameBase"].ToString(), ds.Tables[0].Rows[i]["NameKo"].ToString(), ds.Tables[0].Rows[i]["NameEn"].ToString(),
                                ds.Tables[0].Rows[0]["NameJa"].ToString(), ds.Tables[0].Rows[i]["NameFr"].ToString(), ds.Tables[0].Rows[i]["NameZhcn"].ToString(), ds.Tables[0].Rows[i]["NameZhtw"].ToString(), PreLang);

                                ds.Tables[0].Rows[0]["DeptName"] = this.GetLangName(ds.Tables[0].Rows[i]["DeptBase"].ToString(), ds.Tables[0].Rows[i]["DeptKo"].ToString(), ds.Tables[0].Rows[i]["DeptEn"].ToString(),
                                  ds.Tables[0].Rows[0]["DeptJa"].ToString(), ds.Tables[0].Rows[i]["DeptFr"].ToString(), ds.Tables[0].Rows[i]["DeptZhcn"].ToString(), ds.Tables[0].Rows[i]["DeptZhtw"].ToString(), PreLang);

                                ds.Tables[0].Rows[0]["TitleName"] = this.GetLangName(ds.Tables[0].Rows[i]["TitleBase"].ToString(), ds.Tables[0].Rows[i]["TitleKo"].ToString(), ds.Tables[0].Rows[i]["TitleEn"].ToString(),
                                       ds.Tables[0].Rows[i]["TitleJa"].ToString(), ds.Tables[0].Rows[i]["TitleFr"].ToString(), ds.Tables[0].Rows[i]["TitleZhcn"].ToString(), ds.Tables[0].Rows[i]["TitleZhtw"].ToString(), PreLang);

                                photoPath = FilePath + "/{0}/Users/{1}/Picture/" + ds.Tables[0].Rows[i]["PhotoName"].ToString();
                                photoPath = string.Format(photoPath, UserInfo.CompanyID, ownerID);

                                if (!File.Exists(Server.MapPath(photoPath)))
                                {
                                    photoPath = "/DeskPlusMobile/Content/ko/Base/Img/DStyle/myimgdefault.gif";
                                }

                                Dictionary<string, string> paraDic = new Dictionary<string, string>();
                                paraDic.Add("filePath", photoPath);
                                string encParams = GetEncryptParam(Zeospace.DeskPlusMobile.Web.Helper.WebComponent.Utility.GetParameterDic(paraDic));
                                photoPath = "/DeskPlusMobile/Common/ImageView.aspx?p=" + encParams;

                                ds.Tables[0].Rows[0]["PhotoPath"] = photoPath;
                            }
                            else
                            {
                                string deptName= this.GetLangName(ds.Tables[0].Rows[i]["DeptBase"].ToString(), ds.Tables[0].Rows[i]["DeptKo"].ToString(), ds.Tables[0].Rows[i]["DeptEn"].ToString(),
                                 ds.Tables[0].Rows[i]["DeptJa"].ToString(), ds.Tables[0].Rows[i]["DeptFr"].ToString(), ds.Tables[0].Rows[i]["DeptZhcn"].ToString(), ds.Tables[0].Rows[i]["DeptZhtw"].ToString(), PreLang);

                                string titleName = this.GetLangName(ds.Tables[0].Rows[i]["TitleBase"].ToString(), ds.Tables[0].Rows[i]["TitleKo"].ToString(), ds.Tables[0].Rows[i]["TitleEn"].ToString(),
                                       ds.Tables[0].Rows[i]["TitleJa"].ToString(), ds.Tables[0].Rows[i]["TitleFr"].ToString(), ds.Tables[0].Rows[i]["TitleZhcn"].ToString(), ds.Tables[0].Rows[i]["TitleZhtw"].ToString(), PreLang);

                                deptEtc += deptName + "(" + titleName + "),";
                            }
                        }
                        else
                        {
                            ds.Tables[0].Rows[0]["Name"] = this.GetLangName(ds.Tables[0].Rows[i]["NameBase"].ToString(), ds.Tables[0].Rows[i]["NameKo"].ToString(), ds.Tables[0].Rows[i]["NameEn"].ToString(),
                            ds.Tables[0].Rows[0]["NameJa"].ToString(), ds.Tables[0].Rows[i]["NameFr"].ToString(), ds.Tables[0].Rows[i]["NameZhcn"].ToString(), ds.Tables[0].Rows[i]["NameZhtw"].ToString(), PreLang);

                            ds.Tables[0].Rows[0]["DeptName"] = this.GetLangName(ds.Tables[0].Rows[i]["DeptBase"].ToString(), ds.Tables[0].Rows[i]["DeptKo"].ToString(), ds.Tables[0].Rows[i]["DeptEn"].ToString(),
                              ds.Tables[0].Rows[i]["DeptJa"].ToString(), ds.Tables[0].Rows[i]["DeptFr"].ToString(), ds.Tables[0].Rows[i]["DeptZhcn"].ToString(), ds.Tables[0].Rows[i]["DeptZhtw"].ToString(), PreLang);

                            ds.Tables[0].Rows[0]["TitleName"] = this.GetLangName(ds.Tables[0].Rows[i]["TitleBase"].ToString(), ds.Tables[0].Rows[i]["TitleKo"].ToString(), ds.Tables[0].Rows[i]["TitleEn"].ToString(),
                                   ds.Tables[0].Rows[i]["TitleJa"].ToString(), ds.Tables[0].Rows[i]["TitleFr"].ToString(), ds.Tables[0].Rows[i]["TitleZhcn"].ToString(), ds.Tables[0].Rows[i]["TitleZhtw"].ToString(), PreLang);


                            if (ds.Tables[0].Rows[0]["PhotoName"].ToString() != "")
                            {
                                photoPath = FilePath + "/{0}/Address/{1}";
                                string[] fileStr = ds.Tables[0].Rows[i]["PhotoName"].ToString().Split('.');
                                photoPath = string.Format(photoPath, UserInfo.CompanyID, fileStr[0] + "_S." + fileStr[1]);
                            }
                            else
                            {
                                photoPath = "/DeskPlusMobile/Content/ko/Base/Img/DStyle/myimgdefault.gif";
                            }

                            Dictionary<string, string> paraDic = new Dictionary<string, string>();
                            paraDic.Add("filePath", photoPath);
                            string encParams = GetEncryptParam(Zeospace.DeskPlusMobile.Web.Helper.WebComponent.Utility.GetParameterDic(paraDic));
                            photoPath = "/DeskPlusMobile/Common/ImageView.aspx?p=" + encParams;

                            ds.Tables[0].Rows[0]["PhotoPath"] = photoPath;
                        }                        
                    }

                    ds.Tables[0].Rows[0]["DeptEtc"] = deptEtc.TrimEnd(',');
                }
            }


            return StringHelper.GetJSONString(ds.Tables[0]);
        }    
    }
}
